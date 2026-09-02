using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using TTClassLibrary.IServices;

namespace TTClassLibrary.Functions.CollectionForm
{
    public class CollectionFormRequestSender
    {
        IHttpRequestSender requestSender;

        public CollectionFormRequestSender(IHttpRequestSender _requestSender)
        {
            this.requestSender = _requestSender;
        }

        public async Task<HttpResponseMessage> CreateHeader(MultipartFormDataContent formData)
        {
            var url = $"api/Header/CreateHeader";
            using (formData)
            {
                return await requestSender.Post(url, formData);
            }
        }

        public async Task<HttpResponseMessage> SaveHeader(MultipartFormDataContent formData)
        {
            var url = $"api/Header/UpdateHeader";
            using (formData)
            {
                return await requestSender.Put(url, formData);
            }
        }

        public async Task<HttpResponseMessage> GetHeader()
        {
            var url = $"api/Header/GetHeader";
            return await requestSender.Get(url);
        }

        public async Task<FileInfo?> GetImage(string fileName)
        {
            return await requestSender.GetImage(fileName);
        }
    }
}
