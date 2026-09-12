using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Service
{
    public class ServiceExampleDM
    {
        IServiceRequestSender requestMaker;


        ServiceContent content;
        

        public ServiceContent Content
        {
            get { return content; }
        }


        public ServiceExampleDM(IServiceRequestSender _requestMaker, ServiceContent _content)
        {
            requestMaker = _requestMaker;
            content = _content;
        }

        public static async Task<ServiceExampleDM> CreateAsync(IServiceRequestSender _requestMaker, ServiceContent _content)
        {
            var dm = new ServiceExampleDM(_requestMaker, _content);
            return dm;
        }

        public static async Task<ServiceExampleDM> CreateAsync(ServiceRequestSender _requestMaker, int id)
        {
            var response = await _requestMaker.GetService(id);
            var jsonSerializer = new HttpResponseMessageDeserialize<ServiceContent>();
            ServiceContent _content = await jsonSerializer.DeserealizeResultToContentAsync(response);
            return await CreateAsync(_requestMaker, _content);
        }

        public async Task UpdateDM(ServiceContent newcontent)
        {
            content = newcontent;
        }


        public async Task DeleteAsync()
        {
            await requestMaker.DeleteService(Content.ID);
        }

       
        public async Task<HttpResponseMessage> ChangeServiceContentAsync()
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();

            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "serviceContent");

            var response = await requestMaker.UpdateService(formData, Content.ID);
            return response;
        }
    }
}
