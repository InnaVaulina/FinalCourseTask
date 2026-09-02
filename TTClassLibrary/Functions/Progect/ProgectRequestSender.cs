using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.IServices;

namespace TTClassLibrary.Functions.Progect
{

    public interface IProgectRequestSender
    {
        Task<HttpResponseMessage> DeleteProgect(int id);
        Task<HttpResponseMessage> SaveProgect(MultipartFormDataContent formData);
        Task<HttpResponseMessage> UpdateProgect(MultipartFormDataContent formData, int id);
        Task<HttpResponseMessage> GetAllProgects(ProgectFilter parameters);
        Task<HttpResponseMessage> GetProgect(int id);
        Task<FileInfo?> GetImage(string fileName);

    }
    public class ProgectRequestSender: IProgectRequestSender
    {
        protected IHttpRequestSender requestSender;
        public ProgectRequestSender(IHttpRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }

        public async Task<HttpResponseMessage> DeleteProgect(int id)
        {
            var url = $"api/Progect/DeleteProgect?id={id}";
            return await requestSender.Delete(url);
        }

        public virtual async Task<HttpResponseMessage> SaveProgect(MultipartFormDataContent formData)
        {
            var url = $"api/Progect/SaveProgectRId";
            using (formData)
            {
                return await requestSender.Post(url, formData);
            }
        }

        public virtual async Task<HttpResponseMessage> UpdateProgect(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Progect/UpdateProgectROk?id={id}";
            using (formData)
            {
                return await requestSender.Put(url, formData);
            }
        }

        public async Task<HttpResponseMessage> GetAllProgects(ProgectFilter parameters)
        {
             var url = $"api/Progect/GetAllProgects?search={parameters.Search}&page={parameters.Page}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetProgect(int id)
        {
            var url = $"api/Progect/GetProgect?id={id}";
            return await requestSender.Get(url);
        }

        public async Task<FileInfo?> GetImage(string fileName)
        {
            return await requestSender.GetImage(fileName);
        }
    }


    public class ProgectRequestSenderWPF : ProgectRequestSender
    {
        public ProgectRequestSenderWPF(IHttpRequestSender _requestSender) : base(_requestSender) { }

        public override async Task<HttpResponseMessage> SaveProgect(MultipartFormDataContent formData)
        {
            var url = $"api/Progect/SaveProgectRContent";
            using (formData)
            {
                return await requestSender.Post(url, formData);
            }
        }

        public override async Task<HttpResponseMessage> UpdateProgect(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Progect/UpdateProgectRContent?id={id}";
            using (formData)
            {
                return await requestSender.Put(url, formData);
            }
        }
    }

    public class ProgectFilter
    {
        public string? Search { get; set; } = "ShowAll";
        public int? Page { get; set; } = 1;

    }

    public class GetAllProgectsResponseParamertes
    {
        public List<ProgectContent> Progects { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
