using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Service;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Work
{
    public class RequestExampleDM
    {
        IWorkRequestSender requestMaker;

        Request content;

        public Request Content
        {
            get { return content; }
        }

        public RequestExampleDM(IWorkRequestSender _requestMaker, Request _content)
        {
            requestMaker = _requestMaker;
            content = _content;
        }

        public async Task<HttpResponseMessage> UpdateRequestEx()
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();

            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "workContent");

            var response = await requestMaker.UpdateRequest(formData, content.ID);
            return response;
        }

        public async Task<HttpResponseMessage> SaveRequestEx()
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();

            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "workContent");

            var response = await requestMaker.SaveRequest(formData);
            return response;
        }
    }
}

