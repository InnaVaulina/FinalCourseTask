using TTClassLibrary.Functions.Blog;

namespace TTB.VModel.BlogVM
{
    public class NotAuthBlogListVM
    {
        BlogListDM blogListDM;

        List<INotAuthBlogItemVM> iblogItemVMs;
        public List<INotAuthBlogItemVM> BlogItemVMs { get { return iblogItemVMs; } }
        public NotAuthBlogListVM(BlogListDM _blogListDM)
        {
            blogListDM = _blogListDM;
            iblogItemVMs = new List<INotAuthBlogItemVM>();
            blogListDM.Parametres.Search = "ShowPublished";
        }

        public DateTime? BeginDate
        {
            get { return blogListDM.Parametres.BeginDate; }
            set { blogListDM.Parametres.BeginDate = value; }
        }
        public DateTime? EndDate
        {
            get { return blogListDM.Parametres.EndDate; }
            set { blogListDM.Parametres.EndDate = value; }
        }

        public int? Page
        {
            get { return blogListDM.Parametres.Page; }
            set { blogListDM.Parametres.Page = value; }
        }

        public int TotalPages { get { return blogListDM.TotalPages; } }

        public async Task SetListAsync()
        {
            await blogListDM.SetListAsync();
            iblogItemVMs.Clear();
            foreach (var item in blogListDM.DMList)
            {
                var vm = new BlogItemVM(item);
                iblogItemVMs.Add(vm);
            }

        }
    }
}
