using TTClassLibrary.Functions.Service;
using TTClassLibrary.Support;

namespace TTB.Client.VModel.Service
{
    public class AddServiceVM: IServiceEdit
    {
        AddNewServiceExampleDM newServiceDM;

        public AddServiceVM(AddNewServiceExampleDM _newServiceDM)
        {
            newServiceDM = _newServiceDM;
        }

        public async Task<int?> SavePost()
        {
            var response = await newServiceDM.CreateServiceContentAsync();
            var jsonSerializer = new HttpResponseMessageDeserialize<int>();
            var id = await jsonSerializer.DeserealizeResultToContentAsync(response);
            return id;
        }


        public string Title
        {
            get { return newServiceDM.Content.Title; }
            set { newServiceDM.Content.Title = value; }
        }


        public string Description
        {
            get { return newServiceDM.Content.Description; }
            set { newServiceDM.Content.Description = value; }
        }

   

    }
}
