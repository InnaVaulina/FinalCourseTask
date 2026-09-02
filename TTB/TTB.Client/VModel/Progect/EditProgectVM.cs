using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.Support;

namespace TTB.Client.VModel.Progect
{
    public class EditProgectVM:IProgectEdit
    {
        ProgectExampleDM progectDM;

        public EditProgectVM(ProgectExampleDM _progectExampleDM)
        {
            progectDM = _progectExampleDM;
            imagePath = "img/" + progectDM.Content.IllustrationId;
        }

        public async Task SavePost()
        {
            await progectDM.ChangeProgectContentAsync();
        }

        public void SaveImageContent(MakeImageContent makeImageContent)
        {
            progectDM.SaveImageContent(makeImageContent.ImageFile);
            ImagePath = makeImageContent.ObjectUrl;
        }

        public string? Date
        {
            get { return progectDM.Content.PostDate; }
        }

        public string Title
        {
            get { return progectDM.Content.Title; }
            set { progectDM.Content.Title = value; }
        }


        public string Description
        {
            get { return progectDM.Content.Description; }
            set { progectDM.Content.Description = value; }
        }

        public bool State
        {
            get
            {
                if (progectDM.Content.Status == "InWork") return true;
                else return false;
            }
            set
            {
                if (value == true) progectDM.Content.Status = "InWork";
                else progectDM.Content.Status = "Published";
                progectDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
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
