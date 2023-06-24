using Newtonsoft.Json;

namespace Shop.Module.Core.MiniProgram.Models;

public class GetUserPhoneNumberResponse
{
    public GetUserPhoneNumberResponse() { }

    public string ErrorMessage { get; set; }

    public int ErrorCode { get; set; }

    [JsonProperty("phone_info")]
    public PhoneInfo PhoneInfo { get; set; }
}

public class PhoneInfo
{
    public PhoneInfo()
    {
    }

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }

    [JsonProperty("purePhoneNumber")]
    public string PurePhoneNumber { get; set; }

    [JsonProperty("countryCode")]
    public string CountryCode { get; set; }

    [JsonProperty("watermark")]
    public Watermark Watermark { get; set; }
}

public class Watermark
{
    public Watermark()
    {
    }

    [JsonProperty("timestamp")]
    public int Timestamp { get; set; }

    [JsonProperty("appid")]
    public string AppId { get; set; }
}