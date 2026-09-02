using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;


namespace TTB.VModel.BlogVM
{
    public interface INotAuthBlogItemVM 
    {
        int Id { get; }
        string ImagePath { get; }
        string PostDate { get; }
        string Title { get; }
        RenderFragment Fragment { get; }
    }



    public class BlogItemVM: INotAuthBlogItemVM
    {

        BlogExampleDM blogExampleDM;

        public int Id { get { return blogExampleDM.Content.ID; } }

        string postDate;
        public string PostDate { get { return postDate; } }

        public string Title { get { return blogExampleDM.Content.Title; } }

        RenderFragment fragment;
        public RenderFragment Fragment { get { return fragment; } }

        string imagePath;
        public string ImagePath { get { return imagePath; } }


        public BlogItemVM(BlogExampleDM _blogExampleDM) 
        {
            blogExampleDM = _blogExampleDM;
            fragment = builder => builder.AddMarkupContent(0, blogExampleDM.Content.Article);
            imagePath = "img/" + blogExampleDM.Content.IllustrationId;
            DateTime dateTime = DateTime.ParseExact(blogExampleDM.Content.PostDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            postDate = dateTime.ToString("F");
        }

        public async Task DeletePost() 
        {
            await blogExampleDM.DeleteAsync();
        }

    }
}
