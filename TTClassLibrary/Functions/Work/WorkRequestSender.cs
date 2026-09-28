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
        Task<HttpResponseMessage> GetRequests(WorkFilter parametres);

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

        public async Task<HttpResponseMessage> GetRequests(WorkFilter parametres) 
        {
            string beginDate = parametres.BeginDate.HasValue ? parametres.BeginDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss");
            string endDate = parametres.EndDate.HasValue ? parametres.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            var url = $"api/Request/GetRequests?search={parametres.Search}&beginDate={beginDate}&endDate={endDate}&page={parametres.Page}";
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

    public class WorkFilter
    {
        public string? Search { get; set; } = "ShowAll";
        public DateTime? BeginDate { get; set; } = DateTime.MinValue;
        public DateTime? EndDate { get; set; } = DateTime.Now;
        public int? Page { get; set; } = 1;

    }

    public class GetAllWorkResponseParamertes
    {
        public List<Request> Requests { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
