using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.IServices;

namespace TTClassLibrary.Functions.Work
{
    public interface IWorkRequestSender
    {
        Task<HttpResponseMessage> GetAllRequests(RequestRange range);
        Task<HttpResponseMessage> GetReceivedRequests(RequestRange range);
        Task<HttpResponseMessage> GetTakenOnWorkRequests(RequestRange range);
        Task<HttpResponseMessage> GetRejectedRequests(RequestRange range);
        Task<HttpResponseMessage> GetFinishedRequests(RequestRange range);
        Task<HttpResponseMessage> GetCancelledRequests(RequestRange range);
        Task<HttpResponseMessage> UpdateRequest(MultipartFormDataContent formData, int id);
        Task<HttpResponseMessage> SaveRequest(MultipartFormDataContent formData);
    }
    public class WorkRequestSender: IWorkRequestSender
    {
        protected IHttpRequestSender requestSender;
        public WorkRequestSender(IHttpRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }

        public async Task<HttpResponseMessage> GetAllRequests(RequestRange range) 
        {
            var url = $"api/Request/GetAll?start={range.Start}&end={range.End}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetReceivedRequests(RequestRange range) 
        {
            var url = $"api/Request/GetReceived?start={range.Start.ToString("MM.dd.yyyy")}&end={range.End.ToString("MM.dd.yyyy")}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetTakenOnWorkRequests(RequestRange range) 
        {
            var url = $"api/Request/GetTakenOnWork?start={range.Start}&end={range.End}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetRejectedRequests(RequestRange range) 
        {
            var url = $"api/Request/GetRejected?start={range.Start}&end={range.End}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetFinishedRequests(RequestRange range)
        {
            var url = $"api/Request/GetFinished?start={range.Start}&end={range.End}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetCancelledRequests(RequestRange range)
        {
            var url = $"api/Request/GetCancelled?start={range.Start}&end={range.End}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> UpdateRequest(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Request/UpdateRequest?id={id}";
            return await requestSender.Put(url, formData);
        }

        public async Task<HttpResponseMessage> SaveRequest(MultipartFormDataContent formData)
        {
            var url = $"api/Request/SaveRequest";
            return await requestSender.Post(url, formData);
        }
    }

    public class RequestRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
