using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Service;
using TTClassLibrary.Functions.Work;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Work
{
    public class WorkTableDM
    {
        IWorkRequestSender requestMaker;

        List<RequestExampleDM> requests;

        public List<RequestExampleDM> DMList
        {
            get { return requests; }
        }

        WorkFilter requestParametres;

        public WorkFilter Parametres
        {
            get { return requestParametres; }
        }

        int totalPages;
        public int TotalPages { get { return totalPages; } }

        public WorkTableDM(IWorkRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;
            requests = new List<RequestExampleDM>();
            requestParametres = new WorkFilter()
            {
                Search = "ShowAll",
                BeginDate = DateTime.MinValue,
                EndDate = DateTime.Now,
                Page = 1
            };
            totalPages = 1;
        }


        public async Task SelectRequests()
        {
            if (requestParametres.EndDate.Value.Date == DateTime.Today.Date)
            {
                requestParametres.EndDate = DateTime.Now;
            }
            else requestParametres.EndDate = requestParametres.EndDate.Value.Date.AddDays(1).AddSeconds(-1);

            var response = await requestMaker.GetRequests(requestParametres);

            requests.Clear();
            var jsonSerializer = new HttpResponseMessageDeserialize<GetAllWorkResponseParamertes>();
            var responseParametres = await jsonSerializer.DeserealizeResultToContentAsync(response);
            totalPages = responseParametres.TotalPages;
            Parametres.Page = responseParametres.CurrentPage;
            foreach (var content in responseParametres.Requests)
            {
                var requestExampleDM = CreateRequestExampleDM(content);
                requests.Add(requestExampleDM);
            }
        }

        public RequestExampleDM CreateRequestExampleDM(Request content)
        {
            var dm = new RequestExampleDM(requestMaker, content);
            return dm;
        }
    }
}
