using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.IServices;

namespace TTClassLibrary.Functions.Service
{

    public interface IServiceRequestSender
    {
        Task<HttpResponseMessage> DeleteService(int id);
        Task<HttpResponseMessage> SaveService(MultipartFormDataContent formData);
        Task<HttpResponseMessage> UpdateService(MultipartFormDataContent formData, int id);
        Task<HttpResponseMessage> GetAllServices();
        Task<HttpResponseMessage> GetService(int id);
        Task<FileInfo?> GetImage(string fileName);
    }
    public class ServiceRequestSender: IServiceRequestSender
    {
        protected IHttpRequestSender requestSender;
        public ServiceRequestSender(IHttpRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }

        public async Task<HttpResponseMessage> DeleteService(int id)
        {
            var url = $"api/Service/DeleteService?id={id}";
            return await requestSender.Delete(url);
        }

        public virtual async Task<HttpResponseMessage> SaveService(MultipartFormDataContent formData)
        {
            var url = $"api/Service/SaveServiceRId";
            using (formData)
            {
                return await requestSender.Post(url, formData);
            }
        }

        public virtual async Task<HttpResponseMessage> UpdateService(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Service/UpdateServiceROk?id={id}";
            using (formData)
            {
                return await requestSender.Put(url, formData);
            }
        }

        public async Task<HttpResponseMessage> GetAllServices()
        {
            var url = $"api/Service/GetAllServices";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetService(int id)
        {
            var url = $"api/Service/GetService?id={id}";
            return await requestSender.Get(url);
        }


        public async Task<FileInfo?> GetImage(string fileName)
        {
            return await requestSender.GetImage(fileName);
        }
    }

    public class ServiceRequestSenderWPF : ServiceRequestSender
    {
        public ServiceRequestSenderWPF(IHttpRequestSender _requestSender) : base(_requestSender) { }

        public override async Task<HttpResponseMessage> SaveService(MultipartFormDataContent formData)
        {
            var url = $"api/Service/SaveServiceRContent";
            using (formData)
            {
                return await requestSender.Post(url, formData);
            }
        }

        public override async Task<HttpResponseMessage> UpdateService(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Service/UpdateServiceRContent?id={id}";
            using (formData)
            {
                return await requestSender.Put(url, formData);
            }
        }
    }
}
