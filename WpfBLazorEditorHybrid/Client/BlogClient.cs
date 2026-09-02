using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Contacts.AVM;


namespace WpfBLazorHybridClient.Client
{
    public class BlogClient: HttpRequestSender
    {
        public BlogClient(User user): base(user) { }

        public async Task<HttpResponseMessage> DeleteBlog(BlogContent blog)
        {
            var url = $"{baseAddress}/api/Blog/DeleteBlog?id={blog.ID}";
            return await Delete(url);
        }

        public async Task<HttpResponseMessage> SaveBlog(BlogContent content, FileInfo fileInfo)
        {
            var url = $"{baseAddress}/api/Blog/SaveBlog";
            using (var formData = new MultipartFormDataContent())
            {
                var json = JsonSerializer.Serialize(content);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "blogContent");

                var imageContent = MakeImageContent(fileInfo);
                if (imageContent != null)
                    formData.Add(imageContent, "blogPicture", fileInfo.Name);

                return await Post(url, formData);
            }
        }

 

        public async Task<HttpResponseMessage> UpdateBlog(BlogContent content, FileInfo imageInfo)
        {
            var url = $"{baseAddress}/api/Blog/UpdateBlog?id={content.ID}";
            using (var formData = new MultipartFormDataContent())
            {
                var json = JsonSerializer.Serialize(content);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "blogContent");

                var imageContent = MakeImageContent(imageInfo);
                if (imageContent != null)
                    formData.Add(imageContent, "blogPicture", imageInfo.Name);

                return await Put(url, formData);
            }
        }

        public async Task<HttpResponseMessage> GetAllBlogs()
        {
            var url = $"{baseAddress}/api/Blog/GetAllBlogs";
            return await Get(url);
        }


        public async Task<HttpResponseMessage> GetBlog(Uri? getblogurl) 
        {
            if(getblogurl == null) 
                throw new Exception("GetBlog: getblogurl is null");
            return await Get(getblogurl);
        }

    }
}
