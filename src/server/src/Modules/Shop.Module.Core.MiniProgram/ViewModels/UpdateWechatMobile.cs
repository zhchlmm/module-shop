using System.ComponentModel.DataAnnotations;

namespace Shop.Module.Core.MiniProgram.ViewModels
{
    public class UpdateWechatMobile
    {
        [Required]
        public string Code { get; set; }
    }
}
