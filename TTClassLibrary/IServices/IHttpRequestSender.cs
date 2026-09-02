using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TTClassLibrary.IServices
{
    public interface IHttpRequestSender
    {
        Task<HttpResponseMessage> Get(string url);
        Task<HttpResponseMessage> Get(Uri geturl);
        Task<HttpResponseMessage> Post(string url, JsonContent content);
        Task<HttpResponseMessage> Post(string url, MultipartFormDataContent formData);
        Task<HttpResponseMessage> Put(string url, JsonContent content);
        Task<HttpResponseMessage> Put(string url, MultipartFormDataContent formData);
        Task<FileInfo?> GetImage(string fileName);
        Task<HttpResponseMessage> Delete(string url);
    }
}
