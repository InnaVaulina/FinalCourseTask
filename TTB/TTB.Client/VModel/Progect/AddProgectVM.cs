using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.Support;

namespace TTB.Client.VModel.Progect
{
    public class AddProgectVM: IProgectEdit
    {
        AddNewProgectExampleDM newProgectDM;

        public AddProgectVM(AddNewProgectExampleDM _newProgectDM)
        {
            newProgectDM = _newProgectDM;
        }

        public async Task<int?> SavePost()
        {
            newProgectDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var response = await newProgectDM.CreateProgectContentAsync();
            var jsonSerializer = new HttpResponseMessageDeserialize<int>();
            var id = await jsonSerializer.DeserealizeResultToContentAsync(response);
            return id;
        }
        public void SaveImageContent(MakeImageContent makeImageContent)
        {
            newProgectDM.SaveImageContent(makeImageContent.ImageFile);
            ImagePath = makeImageContent.ObjectUrl;
        }


        public string? Date
        {
            get { return newProgectDM.Content.PostDate; }
        }

        public string Title
        {
            get { return newProgectDM.Content.Title; }
            set { newProgectDM.Content.Title = value; }
        }


        public string Description
        {
            get { return newProgectDM.Content.Description; }
            set { newProgectDM.Content.Description = value; }
        }

        public bool State
        {
            get
            {
                if (newProgectDM.Content.Status == "InWork") return true;
                else return false;
            }
            set
            {
                if (value == true) newProgectDM.Content.Status = "InWork";
                else newProgectDM.Content.Status = "Published";
            }
        }

        string? imagePath;
        public string? ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
            }
        }
    }
}
