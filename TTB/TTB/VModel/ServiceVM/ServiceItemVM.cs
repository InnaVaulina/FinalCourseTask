using Microsoft.AspNetCore.Components;
using System.Globalization;
using TTClassLibrary.Functions.Service;

namespace TTB.VModel.ServiceVM
{
    public interface INotAuthServiceItemVM
    {
        int Id { get; }
        string Title { get; }

        event ServiceItemVMOnShowHandler OnShow;
        bool Show { get; set; }
        RenderFragment Fragment { get; }
    }

    public delegate void ServiceItemVMOnShowHandler();

    public class ServiceItemVM : INotAuthServiceItemVM
    {
        ServiceExampleDM serviceExampleDM;

        public int Id { get { return serviceExampleDM.Content.ID; } }
        public string Title { get { return serviceExampleDM.Content.Title; } }

        public event ServiceItemVMOnShowHandler OnShow;

        bool show;
        public bool Show { get { return show; } set { show = value; OnShow?.Invoke(); } }

        RenderFragment fragment;
        public RenderFragment Fragment { get { return fragment; } }


        public ServiceItemVM(ServiceExampleDM _serviceExampleDM)
        {
            serviceExampleDM = _serviceExampleDM;
            fragment = builder => builder.AddMarkupContent(0, serviceExampleDM.Content.Description);
            show = false;
        }

        public async Task DeletePost()
        {
            await serviceExampleDM.DeleteAsync();
        }
    }
}
