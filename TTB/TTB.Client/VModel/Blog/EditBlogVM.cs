using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Support;

namespace TTB.Client.VModel.Blog
{
    
    public class EditBlogVM: IBlogEdit
    {

        BlogExampleDM blogDM;

        public EditBlogVM(BlogExampleDM _blogExampleDM)
        {
            blogDM = _blogExampleDM;
            imagePath = "img/" + blogDM.Content.IllustrationId;
        }

        public async Task SavePost()
        {
            await blogDM.ChangeBlogContentAsync();       
        }

        public void SaveImageContent(MakeImageContent makeImageContent)
        {
            blogDM.SaveImageContent(makeImageContent.ImageFile);
            ImagePath = makeImageContent.ObjectUrl;
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

        public string? Date
        {
            get { return blogDM.Content.PostDate; }
        }


        public string Title
        {
            get { return blogDM.Content.Title; }
            set { blogDM.Content.Title = value; }
        }


        public string Article
        {
            get { return blogDM.Content.Article; }
            set { blogDM.Content.Article = value; }
        }

    
        public bool State
        {
            get
            {
                if (blogDM.Content.Status == "InWork") return true;
                else return false;
            }
            set
            {
                if (value == true) blogDM.Content.Status = "InWork";
                else blogDM.Content.Status = "Published";
                blogDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }

        

    }
}
