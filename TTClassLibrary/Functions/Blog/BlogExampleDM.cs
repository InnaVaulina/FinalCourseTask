using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TTClassLibrary.DataModel;
using TTClassLibrary.Support;


namespace TTClassLibrary.Functions.Blog
{
    public class BlogExampleDM
    {
        IBlogRequestSender requestMaker;

        BlogContent content;
        MultipartFormDataContent formData;

        public BlogContent Content 
        { 
            get { return content; }
        }

       
        public BlogExampleDM(IBlogRequestSender _requestMaker, BlogContent _content) 
        {
            requestMaker = _requestMaker;
            content = _content;
            formData = new MultipartFormDataContent();
        }

        public static async Task<BlogExampleDM> CreateAsync(IBlogRequestSender _requestMaker, BlogContent _content)
        {
            var dm = new BlogExampleDM(_requestMaker, _content);
            await dm.InitializeAsync();
            return dm;
        }

        public static async Task<BlogExampleDM> CreateAsync(IBlogRequestSender _requestMaker, int id)
        {
            var response = await _requestMaker.GetBlog(id);
            var jsonSerializer = new HttpResponseMessageDeserialize<BlogContent>();
            BlogContent _content = await jsonSerializer.DeserealizeResultToContentAsync(response);
            return await CreateAsync(_requestMaker, _content);
        }

        public async Task UpdateDM(BlogContent newcontent) 
        {
            content = newcontent;
            formData = new MultipartFormDataContent();
            await InitializeAsync();
        }

        private async Task InitializeAsync() 
        {
            if (content.IllustrationId != "")
                await requestMaker.GetImage(content.IllustrationId);
        }

        public async Task DeleteAsync()
        {
            await requestMaker.DeleteBlog(Content.ID);
        }

        public void SaveImageContent(ImageFileModel fileInfo)
        {
            var imageContent = new ByteArrayContent(fileInfo.Content, 0, fileInfo.Content.Length);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            formData.Add(imageContent, "blogPicture", fileInfo.FileName);
        }

        public async Task<HttpResponseMessage> ChangeBlogContentAsync()
        {
            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "blogContent");

            var response = await requestMaker.UpdateBlog(formData, Content.ID);
            return response;
        }
            
        
    }
}
