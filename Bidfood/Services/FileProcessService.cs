using Bidfood.Common;
using Bidfood.Models;
using Newtonsoft.Json;

namespace Bidfood.Services
{
    public class FileProcessService : IFileProcessService
    {
        private readonly ILogger<UserRegistrationService> _logger;
        private readonly IWebHostEnvironment environment;
        private const string folderName = "Files";
        private const string fileName = "\\UserDetails.json";

        public FileProcessService(IWebHostEnvironment environment, ILogger<UserRegistrationService> logger)
        {
            this.environment = environment;
            this._logger = logger;
        }

        public bool StoreUserDetailsIntoJsonFile(UserDetail userDetail)
        {
            try
            {
                var directoryPath = this.environment.ContentRootPath + folderName;
           
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = directoryPath + fileName;
                var serializedData = JsonConvert.SerializeObject(new
                {
                    TimeStamp = DateTime.Now.ToString(),
                    DataReceived = userDetail
                }, Formatting.Indented);

                // Write serialized data to json file
                File.WriteAllText(filePath, serializedData);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(Constants.FileProcessUnhandledExceptionErrorLog, ex);
                return false;
            }           
        }
    }

    public interface IFileProcessService
    {
        bool StoreUserDetailsIntoJsonFile(UserDetail userDetail);
    }
}
