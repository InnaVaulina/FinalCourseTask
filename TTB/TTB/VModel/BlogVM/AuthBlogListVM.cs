using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.Functions.Blog;

namespace TTB.VModel.BlogVM
{
    public class AuthBlogListVM
    {
        BlogListDM blogListDM;

        List<BlogItemVM> blogItemVMs;
        public List<BlogItemVM> BlogItemVMs { get { return blogItemVMs; } }

        public AuthBlogListVM(BlogListDM _blogListDM)
        {
            blogListDM = _blogListDM;
            blogItemVMs = new List<BlogItemVM>();

            ShowAll = true;
        }

        bool showAll;
        public bool ShowAll
        {
            get { return showAll; }
            set 
            {
                showAll = value;
                if (showAll) 
                {
                    ShowPublished = false;
                    ShowInWork = false;
                    blogListDM.Parametres.Search = "ShowAll";
                }
            }
        }

        bool showPublished;
        public bool ShowPublished
        {
            get { return showPublished; }
            set
            {
                showPublished = value;
                if (showPublished)
                {
                    ShowAll = false;
                    ShowInWork = false;
                    blogListDM.Parametres.Search = "ShowPublished";
                }
            }
        }

        bool showInWork;
        public bool ShowInWork
        {
            get { return showInWork; }
            set
            {
                showInWork = value;
                if (showInWork)
                {
                    ShowAll = false;
                    ShowPublished = false;
                    blogListDM.Parametres.Search = "ShowInWork";
                }
            }
        }

        public string Search
        {
            get { return blogListDM.Parametres.Search; }
            set 
            {
                switch (value) 
                {
                    case "ShowAll":
                        ShowAll = true;
                        break;
                    case "ShowPublished":
                        ShowPublished = true;
                        break;
                    case "ShowInWork":
                        ShowInWork = true;
                        break;
                    default:
                        Search = "ShowAll";
                        break;
                }
            }
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
            blogItemVMs.Clear();
            foreach (var item in blogListDM.DMList)
            {
                var vm = new BlogItemVM(item);
                blogItemVMs.Add(vm);
            }

        }


    }

}
