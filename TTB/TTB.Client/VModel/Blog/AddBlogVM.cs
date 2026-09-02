using System.ComponentModel;
using System.Runtime.CompilerServices;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.CollectionForm;
using TTClassLibrary.Support;



namespace TTB.Client.VModel.Blog
{
    public class AddBlogVM: IBlogEdit
    {
        AddNewBlogExampleDM newBlogDM;


        public AddBlogVM(AddNewBlogExampleDM _newBlogDM)
        {
            newBlogDM = _newBlogDM;
        }

        public async Task<int?> SavePost() 
        {
            newBlogDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var response = await newBlogDM.CreateBlogContentAsync();
            HttpResponseMessageDeserialize<int> jsonSerializer;
            jsonSerializer = new HttpResponseMessageDeserialize<int>();
            var id = await jsonSerializer.DeserealizeResultToContentAsync(response);
            return id;
        }

        public void SaveImageContent(MakeImageContent makeImageContent)
        {
            newBlogDM.SaveImageContent(makeImageContent.ImageFile);
            ImagePath = makeImageContent.ObjectUrl;
        }


        public string? Date
        {
            get { return newBlogDM.Content.PostDate; }
        }


        public string Title
        {
            get { return newBlogDM.Content.Title; }
            set { newBlogDM.Content.Title = value; }
        }


        public string Article 
        {
            get { return newBlogDM.Content.Article; }
            set { newBlogDM.Content.Article = value; }
        }

   
        public bool State
        {
            get 
            { 
                if(newBlogDM.Content.Status == "InWork") return true;
                else return false;
            }
            set 
            { 
                if (value == true) newBlogDM.Content.Status = "InWork";
                else newBlogDM.Content.Status = "Published";
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
