using TTClassLibrary.IServices;
using TTClassLibrary.DataModel;

namespace TTClassLibrary.Functions.Blog
{

    public interface IBlogRequestSender 
    {
        Task<HttpResponseMessage> DeleteBlog(int id);
        Task<HttpResponseMessage> SaveBlog(MultipartFormDataContent formData);
        Task<HttpResponseMessage> UpdateBlog(MultipartFormDataContent formData, int id);
        Task<HttpResponseMessage> GetAllBlogs(BlogFilter parameters);
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

        public async Task<HttpResponseMessage> GetAllBlogs(BlogFilter parameters)
        {
            string beginDate = parameters.BeginDate.HasValue ? parameters.BeginDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss");
            string endDate = parameters.EndDate.HasValue ? parameters.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var url = $"api/Blog/GetAllBlogs?search={parameters.Search}&beginDate={beginDate}&endDate={endDate}&page={parameters.Page}";
            return await requestSender.Get(url);
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

    public class BlogFilter 
    {
        public string? Search { get; set; } = "ShowAll";
        public DateTime? BeginDate { get; set; } = DateTime.MinValue;
        public DateTime? EndDate { get; set; } = DateTime.Now;
        public int? Page { get; set; } = 1;

    }

    public class GetAllBlogsResponseParamertes
    {
        public List<BlogContent> Blogs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
