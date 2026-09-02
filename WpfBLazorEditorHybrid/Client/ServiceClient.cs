using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Progect.AVM;
using WpfBLazorHybridClient.Functions.Service.AVM;


namespace WpfBLazorHybridClient.Client
{
    public class ServiceClient: HttpRequestSender
    {
        public ServiceClient(User user): base(user) { }

        public async Task<HttpResponseMessage> DeleteService(ServiceContent service)
        {
            var url = $"{baseAddress}/api/Service/DeleteService?id={service.ID}";
            try
            {
                return await Delete(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<HttpResponseMessage> SaveService(AddServiceVM model)
        {
            var url = $"{baseAddress}/api/Service/SaveService";
            using (var formData = new MultipartFormDataContent())
            {
                try
                {
                    formData.Add(new StringContent(model.Title), "title");
                    formData.Add(new StringContent(model.Description), "description");

                    //if (model.State == true)
                    //    formData.Add(new StringContent("готово"), "status");
                    //else
                    //    formData.Add(new StringContent("в работе"), "status");

                    return await Post(url,formData);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }


        public async Task<HttpResponseMessage> UpdateService(AddServiceVM model)
        {
            var url = $"{baseAddress}/api/Service/UpdateService";
            using (var formData = new MultipartFormDataContent())
            {
                try
                {
                    formData.Add(new StringContent(model.Title), "title");
                    formData.Add(new StringContent(model.Description), "description");

                    //if (model.State == true)
                    //    formData.Add(new StringContent("готово"), "status");
                    //else
                    //    formData.Add(new StringContent("в работе"), "status");

                    return await Put(url, formData);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<HttpResponseMessage> GetAllServices()
        {
            var url = $"{baseAddress}/api/Service/GetAllServices";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> GetService(Uri? getserviceurl)
        {
            if (getserviceurl == null)
                throw new ArgumentNullException(nameof(getserviceurl));
            try
            {
                return await Get(getserviceurl);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
