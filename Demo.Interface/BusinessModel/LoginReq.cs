using Newtonsoft.Json;

namespace Demo.Interface.BussinessModel
{
    public class LoginReq
    {
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

