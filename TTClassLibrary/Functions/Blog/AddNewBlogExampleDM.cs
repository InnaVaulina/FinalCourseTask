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
        ImageFileModel fileInfo;
       

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
            fileInfo = new ImageFileModel()
            {
                FileName = "",
                Content = Array.Empty<byte>()
            };
        }

        public void SaveImageContent(ImageFileModel _fileInfo)
        {
            fileInfo = _fileInfo;
        }

        public async Task<HttpResponseMessage> CreateBlogContentAsync()
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();

            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "blogContent");

            var imageContent = new ByteArrayContent(fileInfo.Content, 0, fileInfo.Content.Length);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            formData.Add(imageContent, "blogPicture", fileInfo.FileName);

            var response = await requestMaker.SaveBlog(formData);
            return response;
        }

    }

}
