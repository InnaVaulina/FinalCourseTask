using TTB.VModel.ServiceVM;
using TTClassLibrary.Functions.Service;

namespace TTB.VModel.ServiceVM
{
    public class AuthServiceListVM
    {
        ServiceListDM serviceListDM;

        List<ServiceItemVM> serviceItemVMs;
        public List<ServiceItemVM> ServiceItemVMs { get { return serviceItemVMs; } }

        public AuthServiceListVM(ServiceListDM _serviceListDM)
        {
            serviceListDM = _serviceListDM;
            serviceItemVMs = new List<ServiceItemVM>();
        }

        public async Task SetListAsync()
        {
            await serviceListDM.SetListAsync();
            serviceItemVMs.Clear();
            foreach (var item in serviceListDM.DMList)
            {
                var vm = new ServiceItemVM(item);
                serviceItemVMs.Add(vm);
            }

        }
    }
}
