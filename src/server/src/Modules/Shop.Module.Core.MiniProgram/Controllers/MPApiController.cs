using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;
using Shop.Module.Core.Entities;
using Shop.Module.Core.Models;
using Shop.Module.Core.Services;
using Shop.Module.Core.ViewModels;
using Shop.Module.Core.MiniProgram.Data;
using Shop.Module.Core.MiniProgram.Models;
using Shop.Module.Core.MiniProgram.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Shop.Module.Core.Cache;
using Shop.Module.Core.Data;
using Shop.Module.Core.Extensions;
using System.Net.Http.Json;
using System.Reflection.Metadata;

namespace Shop.Module.Core.MiniProgram.Controllers
{
    [ApiController]
    [Route("api/mp")]
    public class MPApiController : ControllerBase
    {
        const string Code2SessionUrl = "https://api.weixin.qq.com/sns/jscode2session";
        const string AccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token";
        const string WxaCodeUnlimited = "https://api.weixin.qq.com/wxa/getwxacodeunlimit";

        private readonly MiniProgramOptions _option;
        private readonly IRepository<UserLogin> _userLoginRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<User> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;

        public MPApiController(
            IAppSettingService appSettingService,
            IAccountService accountService,
            IRepository<UserLogin> userLoginRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IRepository<User> userRepository,
            ITokenService tokenService,
            IOptionsMonitor<MiniProgramOptions> options,
            IHttpClientFactory httpClientFactory,
            IStaticCacheManager cacheManager,
            IWorkContext workContext)
        {
            _option = options.CurrentValue;
            _userLoginRepository = userLoginRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
            _cacheManager = cacheManager;
            _workContext = workContext;
        }

        /// <summary>
        /// 小程序登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<Result> Login([FromBody] LoginByMpParam param)
        {
            var url = $"{Code2SessionUrl}?appid={_option.AppId}&secret={_option.AppSecret}&js_code={param.Code}&grant_type=authorization_code";
            var httpClient = _httpClientFactory.CreateClient();// new HttpClient();
            var content = await httpClient.GetStringAsync(url);
            if (string.IsNullOrWhiteSpace(content))
            {
                return Result.Fail("登录失败");
            }

            var result = JsonConvert.DeserializeObject<Code2SessionGetResult>(content);
            if (result.ErrCode != 0)
            {
                return Result.Fail(result.ErrMessage);
            }

            User user = null;
            var model = await _userLoginRepository.Query()
                .FirstOrDefaultAsync(c => c.LoginProvider == MiniProgramDefaults.AuthenticationScheme && c.ProviderKey == result.OpenId);
            if (model == null)
            {
                // 创建用户
                var userName = Guid.NewGuid().ToString("N");
                user = new User
                {
                    UserName = userName,
                    FullName = param.NickName ?? userName,
                    AvatarUrl = param.AvatarUrl,
                    IsActive = true,
                    Culture = GlobalConfiguration.DefaultCulture
                };
                var transaction = _userRepository.BeginTransaction();
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    transaction.Rollback();
                    return Result.Fail("创建用户失败");
                }
                await _userManager.AddToRoleAsync(user, RoleWithId.guest.ToString());
                _userLoginRepository.Add(new UserLogin()
                {
                    LoginProvider = MiniProgramDefaults.AuthenticationScheme,
                    ProviderDisplayName = MiniProgramDefaults.DisplayName,
                    UserId = user.Id,
                    ProviderKey = result.OpenId,
                    UnionId = result.UnionId
                });
                await _userLoginRepository.SaveChangesAsync();
                transaction.Commit();
            }
            else
            {
                user = await _userManager.FindByIdAsync(model.UserId.ToString());
            }
            if (user == null)
            {
                return Result.Fail("登录失败，请稍后重试");
            }

            // var providers = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var signInResult = await _signInManager.ExternalLoginSignInAsync(MiniProgramDefaults.AuthenticationScheme, result.OpenId, false);

            if (signInResult.IsLockedOut)
            {
                return Result.Fail("用户已锁定，请稍后重试");
            }
            else if (signInResult.IsNotAllowed)
            {
                return Result.Fail("用户邮箱未验证或手机未验证，不允许登录");
            }
            else if (signInResult.Succeeded)
            {
                var token = await _tokenService.GenerateAccessToken(user);
                var roles = await _userManager.GetRolesAsync(user);
                var loginResult = new LoginResult()
                {
                    Token = token,
                    Avatar = user.AvatarUrl,
                    Email = user.Email,
                    Name = user.FullName,
                    Phone = user.PhoneNumber,
                    Roles = roles
                };
                return Result.Ok(loginResult);
            }
            return Result.Fail("用户登录失败");
        }

        /// <summary>
        /// 更新微信手机号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("mobile")]
        public async Task<Result> Mobile([FromBody] UpdateWechatMobile param)
        {
            var user = await _workContext.GetCurrentUserOrNullAsync();
            if (user == null)
            {
                return Result.Fail("用户未登录");
            }

            var _key = ShopKeys.UserWechatAccessToken + user.Id;
            var accessToken = await _cacheManager.GetAsync(_key, async () =>
             {
                 string text = await (await _httpClientFactory.CreateClient().SendAsync(new HttpRequestMessage(requestUri: $"{AccessTokenUrl}?grant_type=client_credential&appid=" + _option.AppId + "&secret=" + _option.AppSecret, method: HttpMethod.Get)).ConfigureAwait(continueOnCapturedContext: false)).Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                 return (JObject.Parse(text).SelectToken("$.access_token") ?? throw new NullReferenceException("无法获取到 AccessToken，微信 API 返回的内容为：" + text)).Value<string>();
             }, 119);

            var targetUrl = $"https://api.weixin.qq.com/wxa/business/getuserphonenumber?access_token={accessToken}";
            HttpClient httpClient = _httpClientFactory.CreateClient();// new HttpClient();
            var responseMessage = await httpClient.PostAsync(targetUrl, new StringContent("{\"code\":\"" + param.Code + "\"}"));

            var resultStr = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(resultStr))
            {
                return Result.Fail("获取手机号码失败！");
            }

            var response = JsonConvert.DeserializeObject<GetUserPhoneNumberResponse>(resultStr);
            if (response != null && response?.PhoneInfo?.PurePhoneNumber != null)
            {
                var phoneNumber = response?.PhoneInfo?.PurePhoneNumber;
                user.UpdatedOn = DateTime.Now;

                var changePhoneNumberToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
                var identityResult = await _userManager.ChangePhoneNumberAsync(user, phoneNumber, changePhoneNumberToken);
                if (identityResult.Succeeded)
                {
                    return Result.Ok(phoneNumber);
                }
            }

            return Result.Fail("更新用户手机号失败！");
        }

    }
}
