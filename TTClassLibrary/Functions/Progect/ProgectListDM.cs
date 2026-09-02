using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Progect
{
    public class ProgectListDM
    {
        IProgectRequestSender requestMaker;

        List<ProgectExampleDM> dmList;
        public List<ProgectExampleDM> DMList
        {
            get { return dmList; }
        }

        ProgectFilter requestParametres;

        public ProgectFilter Parametres
        {
            get { return requestParametres; }
        }

        int totalPages;
        public int TotalPages { get { return totalPages; } }

        public ProgectListDM(ProgectRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;
            dmList = new List<ProgectExampleDM>();
            requestParametres = new ProgectFilter()
            {
                Search = "ShowAll",
                Page = 1
            };
            totalPages = 1;
        }

        public async Task InitializeAsync()
        {
            await SetListAsync();
        }

        public async Task SetListAsync() 
        {
            dmList.Clear();
            var response = await requestMaker.GetAllProgects(requestParametres);
            var jsonSerializer = new HttpResponseMessageDeserialize<GetAllProgectsResponseParamertes>();
            var responseParametres = await jsonSerializer.DeserealizeResultToContentAsync(response);
            totalPages = responseParametres.TotalPages;
            Parametres.Page = responseParametres.CurrentPage;
            foreach (var content in responseParametres.Progects)
            {
                var progectDM = await CtreateProgectExampleDM(content);
                dmList.Add(progectDM);
            }
        }

        public async Task<ProgectExampleDM> CtreateProgectExampleDM(ProgectContent content)
        {
            var dm = await ProgectExampleDM.CreateAsync(requestMaker, content);
            return dm;
        }

        public AddNewProgectExampleDM CreateAddNewProgectExampleDM()
        {
            var dm = new AddNewProgectExampleDM(requestMaker);
            return dm;
        }

    }
}
