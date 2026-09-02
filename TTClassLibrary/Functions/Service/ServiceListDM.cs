using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Service
{
    public class ServiceListDM
    {
        IServiceRequestSender requestMaker;

        List<ServiceExampleDM> dmList;
        public List<ServiceExampleDM> DMList
        {
            get { return dmList; }
        }
        public ServiceListDM(ServiceRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;
            dmList = new List<ServiceExampleDM>();
        }

        public async Task InitializeAsync()
        {
            await SetListAsync();
        }

        public async Task SetListAsync() 
        {
            dmList.Clear();
            var response = await requestMaker.GetAllServices();
            var jsonSerializer = new HttpResponseMessageDeserialize<List<ServiceContent>>();
            var contentList = await jsonSerializer.DeserealizeResultToContentAsync(response);
            foreach (var content in contentList)
            {
                var serviceDM = CreateServiceExampleDM(content);
                dmList.Add(serviceDM);
            }
        }

        public ServiceExampleDM CreateServiceExampleDM(ServiceContent content)
        {
            var dm = new ServiceExampleDM(requestMaker, content);
            return dm;
        }
        public AddNewServiceExampleDM CreateAddNewServiceExampleDM()
        {
            var dm = new AddNewServiceExampleDM(requestMaker);
            return dm;
        }
    }
}
