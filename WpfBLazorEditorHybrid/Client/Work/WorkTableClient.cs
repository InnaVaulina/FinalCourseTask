using WpfBLazorHybridClient.DataModel;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http;
using System.IO;
using System.Configuration;
using System.Net.Http.Headers;

namespace WpfBLazorHybridClient.Client.Work
{
    public class WorkTableClient: HttpRequestSender
    {
        public WorkTableClient(User _user): base(_user) { }

        public async Task<HttpResponseMessage> GetAllRequests(RequestRange range) 
        {
            var url = $"{baseAddress}/api/Request/GetAll?start={range.Start}&end={range.End}";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<HttpResponseMessage> GetReceivedRequests(RequestRange range)
        {
            var url = $"{baseAddress}/api/Request/GetReceived?start={range.Start.ToString("MM.dd.yyyy")}&end={range.End.ToString("MM.dd.yyyy")}";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> GetTakenOnWorkRequests(RequestRange range)
        {
            using (var client = new HttpClient())
            {
                var url = $"{baseAddress}/api/Request/GetTakenOnWork?start={range.Start}&end={range.End}";
                try
                {
                    return await Get(url);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<HttpResponseMessage> GetRejectedRequests(RequestRange range)
        {
            var url = $"{baseAddress}/api/Request/GetRejected?start={range.Start}&end={range.End}";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> GetFinishedRequests(RequestRange range)
        {
            var url = $"{baseAddress}/api/Request/GetFinished?start={range.Start}&end={range.End}";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> GetCancelledRequests(RequestRange range)
        {
            var url = $"{baseAddress}/api/Request/GetCancelled?start={range.Start}&end={range.End}";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<HttpResponseMessage> UpdateRequest(int id)
        {
            var url = $"{baseAddress}/api/Request/UpdateRequest?id={id}";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        


        public async Task<HttpResponseMessage> SaveRequest(Request request)
        {
            var url = $"{baseAddress}/api/Request/SaveRequest?id={request.ID}";
            JsonContent content = JsonContent.Create(request);
            try
            {
                return await Post(url, content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
