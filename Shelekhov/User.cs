using Newtonsoft.Json;
using PhoneApp.Domain.DTO;

namespace Shelekhov
{
    public class User : DataTransferObject
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
