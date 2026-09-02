using TTClassLibrary.Functions.Service;
using TTClassLibrary.Support;

namespace TTB.Client.VModel.Service
{
    public class EditServiceVM: IServiceEdit
    {
        ServiceExampleDM serviceDM;

        public EditServiceVM(ServiceExampleDM _serviceExampleDM)
        {
            serviceDM = _serviceExampleDM;
        }

        public async Task SavePost()
        {
            await serviceDM.ChangeServiceContentAsync();
        }


        public string Title
        {
            get { return serviceDM.Content.Title; }
            set { serviceDM.Content.Title = value; }
        }


        public string Description
        {
            get { return serviceDM.Content.Description; }
            set { serviceDM.Content.Description = value; }
        }
    }
}
