using TTB.VModel.ServiceVM;
using TTClassLibrary.Functions.Service;

namespace TTB.VModel.ServiceVM
{
    public class NotAuthServiceListVM
    {
        ServiceListDM serviceListDM;

        List<INotAuthServiceItemVM> iserviceItemVMs;
        public List<INotAuthServiceItemVM> IserviceItemVMs { get { return iserviceItemVMs; } }
        public NotAuthServiceListVM(ServiceListDM _serviceListDM)
        {
            serviceListDM = _serviceListDM;
            iserviceItemVMs = new List<INotAuthServiceItemVM>();
        }


        public async Task SetListAsync()
        {
            await serviceListDM.SetListAsync();
            iserviceItemVMs.Clear();
            foreach (var item in serviceListDM.DMList)
            {
                var vm = new ServiceItemVM(item);
                iserviceItemVMs.Add(vm);
            }

        }
    }
}
