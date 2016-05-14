using Newtonsoft.Json;

namespace Client
{
    public class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}