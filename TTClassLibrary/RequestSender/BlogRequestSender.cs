using System.Net.Http.Json;
using TTClassLibrary.DataModel;

namespace TTClassLibrary.RequestSender
{

    public interface IBlogRequestSender 
    {
        Task<HttpResponseMessage> DeleteBlog(int id);
        Task<HttpResponseMessage> SaveBlog(MultipartFormDataContent formData);
        Task<HttpResponseMessage> UpdateBlog(MultipartFormDataContent formData, int id);
        Task<HttpResponseMessage> GetAllBlogs(BlogFilterParameters parameters);
        Task<HttpResponseMessage> GetAllBlogs(JsonContent parameters);
        Task<HttpResponseMessage> GetBlog(int id);
        Task<FileInfo?> GetImage(string fileName);

    }
    public class BlogRequestSender: IBlogRequestSender
    {
        protected IHttpRequestSender requestSender;
        public BlogRequestSender(IHttpRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }

        public async Task<HttpResponseMessage> DeleteBlog(int id)
        {
            var url = $"api/Blog/DeleteBlog?id={id}";
            return await requestSender.Delete(url);
        }

        public virtual async Task<HttpResponseMessage> SaveBlog(MultipartFormDataContent formData)
        {
            var url = $"api/Blog/SaveBlogRId";
            using (formData)
            {
                return await requestSender.Post(url, formData);
            }
        }



        public virtual async Task<HttpResponseMessage> UpdateBlog(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Blog/UpdateBlogROk?id={id}";
            using (formData)
            {
                return await requestSender.Put(url, formData);
            }
        }

        public async Task<HttpResponseMessage> GetAllBlogs(BlogFilterParameters parameters)
        {
            var url = $"api/Blog/GetAllBlogs?search={parameters.Search}&beginDate={parameters.BeginDate}&endDate={parameters.EndDate}&page={parameters.Page}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetAllBlogs(JsonContent parameters)
        {
            var url = $"api/Blog/GetAllBlogs";
            return await requestSender.Get(url, parameters);
        }

        public async Task<HttpResponseMessage> GetBlog(int id)
        {
            var url = $"api/Blog/GetBlog?id={id}";
            return await requestSender.Get(url);
        }

        public async Task<FileInfo?> GetImage(string fileName)
        {
            return await requestSender.GetImage(fileName);
        }

    }

    public class BlogRequestSenderWPF: BlogRequestSender 
    {
        public BlogRequestSenderWPF(IHttpRequestSender _requestSender): base(_requestSender) { }

        public override async Task<HttpResponseMessage> SaveBlog(MultipartFormDataContent formData)
        {
            var url = $"api/Blog/SaveBlogRContent";
            using (formData)
            {
                return await requestSender.Post(url, formData);
            }
        }

        public override async Task<HttpResponseMessage> UpdateBlog(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Blog/UpdateBlogRContent?id={id}";
            using (formData)
            {
                return await requestSender.Put(url, formData);
            }
        }
    }

    public class BlogFilterParameters 
    {
        public string? Search { get; set; } = "ShowAll";
        public string? BeginDate { get; set; } = DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss");
        public string? EndDate { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        public int Page { get; set; } = 1;

    }

    public class GetAllBlogsResponseParamertes
    {
        public List<BlogContent> Blogs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
