using Common.BL;

namespace FileStorage.WebService.Models
{
    public class UploadResponse : ServiceResponse
    {
        public UploadResponse(string itemToken = null)
        {
            ItemToken = itemToken;
        }
        public string ItemToken { get; }
    }
}
