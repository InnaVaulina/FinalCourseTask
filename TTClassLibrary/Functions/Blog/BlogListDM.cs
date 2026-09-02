
using static System.Net.Mime.MediaTypeNames;
using TTClassLibrary.Support;
using TTClassLibrary.DataModel;


namespace TTClassLibrary.Functions.Blog
{
    public class BlogListDM
    {
        IBlogRequestSender requestMaker;
        

        List<BlogExampleDM> dmList;
        public List<BlogExampleDM> DMList 
        { 
            get { return dmList; } 
        }

        BlogFilter requestParametres;

        public BlogFilter Parametres
        {
            get { return requestParametres; }
        }

        int totalPages;
        public int TotalPages { get { return totalPages; } }

        public BlogListDM(IBlogRequestSender _requestMaker) 
        {
            requestMaker = _requestMaker;
            dmList = new List<BlogExampleDM>();
            requestParametres = new BlogFilter()
            {
                Search = "ShowAll",
                BeginDate = DateTime.MinValue,
                EndDate = DateTime.Now,
                Page = 1
            };
            totalPages = 1;
        }

        public async Task InitializeAsync()
        {
            await SetListAsync();
        }

        public async Task SetListAsync()
        {
            dmList.Clear();
            if (requestParametres.EndDate.Value.Date == DateTime.Today.Date)
            {
                requestParametres.EndDate = DateTime.Now;
            }
            else requestParametres.EndDate = requestParametres.EndDate.Value.Date.AddDays(1).AddSeconds(-1);

            var response = await requestMaker.GetAllBlogs(requestParametres);
            var jsonSerializer = new HttpResponseMessageDeserialize<GetAllBlogsResponseParamertes>();
            var responseParametres = await jsonSerializer.DeserealizeResultToContentAsync(response);
            totalPages = responseParametres.TotalPages;
            Parametres.Page = responseParametres.CurrentPage;
            foreach (var content in responseParametres.Blogs)
            {
                var blogDM = await CtreateBlogExampleDM(content);
                dmList.Add(blogDM);
            }
        }

        public async Task<BlogExampleDM> CtreateBlogExampleDM(BlogContent content) 
        {
            var dm = await BlogExampleDM.CreateAsync(requestMaker, content);
            return dm;
        }

        public AddNewBlogExampleDM CreateAddNewBlogExampleDM() 
        {
            var dm = new AddNewBlogExampleDM(requestMaker);
            return dm;
        }

    }

    
}
