using Newtonsoft.Json;
using System.Collections.Generic;

namespace Shelekhov
{
    public class ApiResponse
    {
        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }
}
