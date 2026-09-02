using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TTClassLibrary.DataModel;
using TTClassLibrary.Support;


namespace TTClassLibrary.Functions.Blog
{
    public class AddNewBlogExampleDM
    {

        IBlogRequestSender requestMaker;

        BlogContent content;
        MultipartFormDataContent formData;

        public BlogContent Content
        {
            get { return content; }
        }

        public AddNewBlogExampleDM(IBlogRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;
      
            content = new BlogContent()
            {
                ID = 0,
                Title = "Новый блог",
                WriterId = "",
                Article = "",
                PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                Status = "InWork",
                IllustrationId = ""
            };
            formData = new MultipartFormDataContent();
        }

        public void SaveImageContent(ImageFileModel fileInfo)
        {
            var imageContent = new ByteArrayContent(fileInfo.Content, 0, fileInfo.Content.Length);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            formData.Add(imageContent, "blogPicture", fileInfo.FileName);
        }

        public async Task<HttpResponseMessage> CreateBlogContentAsync()
        {
            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "blogContent");

            var response = await requestMaker.SaveBlog(formData);
            return response;
        }

    }

}
