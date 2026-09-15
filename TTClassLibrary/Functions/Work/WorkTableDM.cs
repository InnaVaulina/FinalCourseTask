using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Service;
using TTClassLibrary.Functions.Work;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Work
{
    public class WorkTableDM
    {
        IWorkRequestSender requestMaker;

        List<RequestExampleDM> requests;

        public WorkTableDM(IWorkRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;
            requests = new List<RequestExampleDM>();
        }

        public async Task SetExampleDMList(HttpResponseMessage response)
        {
            requests.Clear();
            var jsonSerializer = new HttpResponseMessageDeserialize<List<Request>>();
            var contentList = await jsonSerializer.DeserealizeResultToContentAsync(response);
            foreach (var content in contentList)
            {
                var requestExampleDM = CreateRequestExampleDM(content);
                requests.Add(requestExampleDM);
            }
        }

        public async Task<List<RequestExampleDM>> SelectAllRequests(RequestRange range)
        {
            var response = await requestMaker.GetAllRequests(range);
            await SetExampleDMList(response);
            return requests;
        }

        public async Task<List<RequestExampleDM>> SelectReceivedRequests(RequestRange range)
        {
            var response = await requestMaker.GetReceivedRequests(range);
            await SetExampleDMList(response);
            return requests;
        }

        public async Task<List<RequestExampleDM>> SelectTakenOnWorkRequests(RequestRange range)
        {
            var response = await requestMaker.GetTakenOnWorkRequests(range);
            await SetExampleDMList(response);
            return requests;
        }

        public async Task<List<RequestExampleDM>> SelectRejectedRequests(RequestRange range)
        {
            var response = await requestMaker.GetRejectedRequests(range);
            await SetExampleDMList(response);
            return requests;
        }

        public async Task<List<RequestExampleDM>> SelectFinishedRequests(RequestRange range)
        {
            var response = await requestMaker.GetFinishedRequests(range);
            await SetExampleDMList(response);
            return requests;
        }

        public async Task<List<RequestExampleDM>> SelectCancelledRequests(RequestRange range)
        {
            var response = await requestMaker.GetCancelledRequests(range);
            await SetExampleDMList(response);
            return requests;
        }

        public RequestExampleDM CreateRequestExampleDM(Request content)
        {
            var dm = new RequestExampleDM(requestMaker, content);
            return dm;
        }
    }
}
