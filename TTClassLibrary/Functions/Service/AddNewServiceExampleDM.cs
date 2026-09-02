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
    public class AddNewServiceExampleDM
    {
        IServiceRequestSender requestMaker;


        ServiceContent content;
        MultipartFormDataContent formData;

        public ServiceContent Content
        {
            get { return content; }
        }

        public AddNewServiceExampleDM(IServiceRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;

            content = new ServiceContent()
            {
                ID = 0,
                Title = "Новая услуга",
                Description = ""
            };
            formData = new MultipartFormDataContent();
        }

        public async Task<HttpResponseMessage> CreateServiceContentAsync()
        {
            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "serviceContent");

            var response = await requestMaker.SaveService(formData);
            return response;
        }
    }
}
