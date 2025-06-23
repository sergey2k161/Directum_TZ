using Newtonsoft.Json;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shelekhov
{
    [Author(Name = "Sergey Shelekhov")]
    public class ShelekhovPlugin : IPluggable
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> input)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = new List<DataTransferObject>();

            foreach (var item in input)
            {
                result.Add(item);
            }

            try
            {
                logger.Info("ShelekhovPlugin: I'm starting to download users from the API...");

                var users = LoadUsersFromApiAsync().Result;

                logger.Info($"ShelekhovPlugin: Uploaded {users.Count} users");

                foreach (var user in users)
                {
                    var dto = new EmployeesDTO
                    {
                        Name = $"{user.FirstName} {user.LastName}"
                    };
                    dto.AddPhone(user.Phone);
                    result.Add(dto);
                    logger.Info($"ShelekhovPlugin: Adding a user {dto.Name} from phones {dto.Phone}");
                }

                logger.Info($"ShelekhovPlugin: Successfully added {users.Count} new contacts");
            }
            catch (Exception ex)
            {
                logger.Error($"ShelekhovPlugin: Error when uploading users: {ex.Message}");
            }

            return result;
        }

        private async Task<List<User>> LoadUsersFromApiAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync("https://dummyjson.com/users");
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                return apiResponse.Users ?? new List<User>();
            }
            catch (HttpRequestException ex)
            {
                logger.Error($"HTTP request error: {ex.Message}");
                return new List<User>();
            }
            catch (JsonException ex)
            {
                logger.Error($"JSON parsing error: {ex.Message}");
                return new List<User>();
            }
        }
    }
}
