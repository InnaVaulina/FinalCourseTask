
using static System.Net.Mime.MediaTypeNames;
using TTClassLibrary.Support;
using TTClassLibrary.DataModel;
using TTClassLibrary.RequestSender;
using TTClassLibrary.Support.Filters;


namespace TTClassLibrary.Functions.Blog
{
    public class BlogListDM
    {
        IBlogRequestSender requestMaker;
        IFilter filter;

        List<BlogExampleDM> dmList;
        public List<BlogExampleDM> DMList 
        { 
            get { return dmList; } 
        }

   
        public BlogListDM(IBlogRequestSender _requestMaker, IFilter _filter) 
        {
            requestMaker = _requestMaker;
            filter = _filter;
            dmList = new List<BlogExampleDM>();

        }

        public async Task InitializeAsync()
        {
            await SetListAsync();
        }

        public async Task SetListAsync()
        {
            dmList.Clear();
            var response = await requestMaker.GetAllBlogs(filter.FilterContent);
            var jsonSerializer = new HttpResponseMessageDeserialize<GetAllBlogsResponseParamertes>();
            var responseParametres = await jsonSerializer.DeserealizeResultToContentAsync(response);
            filter.TotalPages = responseParametres.TotalPages;
            filter.Page = responseParametres.CurrentPage;
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
