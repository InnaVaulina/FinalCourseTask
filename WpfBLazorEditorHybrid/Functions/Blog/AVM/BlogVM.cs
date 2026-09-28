using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Blog.AVM
{
    public delegate void DeleteBlogHandler(UC_BlogItem2? item);
    public delegate void UpdateBlogHandler();
    public delegate Task AddBlogHandler(BlogContent newcontent);
    public class BlogVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new;

        ListTabVM tab;
        BlogListDM blogListDM;

        ObservableCollection<UC_BlogItem2> list;
        public ObservableCollection<UC_BlogItem2> List { get { return list; } }

        public BlogVM(BlogListDM _blogListDM, ListTabVM _tab)
        {            
            blogListDM = _blogListDM;
            tab = _tab;
            list = new ObservableCollection<UC_BlogItem2>();

            isBackButtonEnabled = false;
            isNextButtonEnabled = false;
            pageNumber = 1;

            openAddPage = new WCommand(async _ =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Добавить пост"
                };

                var model = new AddBlogVM(blogListDM.CreateAddNewBlogExampleDM(), page);
                model.Notify_close_page += tab.TabClose;
                model.Notify_add += AddItem;

                page.Content = new UC_BlogItemAdd(model);
                Notify_new?.Invoke(page);
            });

            loadNewList = new WCommand(async _ =>
            {
                await InitializeAsync();
            });
            goNextPage = new WCommand(async _ =>
            {
                if (blogListDM.Parametres.Page < blogListDM.TotalPages)
                {
                    blogListDM.Parametres.Page++;
                    await InitializeAsync();
                }
            });
            goPreviousPage = new WCommand(async _ =>
            {
                if (blogListDM.Parametres.Page > 1)
                {
                    blogListDM.Parametres.Page--;
                    await InitializeAsync();
                }
            });

            selectAll = new WCommand(o => { DesiredStatus = "ShowAll"; });
            selectPublished = new WCommand(o => { DesiredStatus = "ShowPublished"; });
            selectTakenOnWork = new WCommand(o => { DesiredStatus = "ShowInWork"; });

            setThatDay = new WCommand(o =>
            {
                BeginDate = DateTime.Today;
                EndDate = DateTime.Today;
            });

            setYesterday = new WCommand(o =>
            {
                BeginDate = DateTime.Today.AddDays(-1);
                EndDate = DateTime.Today.AddDays(-1);
            });

            setWeek = new WCommand(o =>
            {
                BeginDate = DateTime.Today.AddDays(-7);
                EndDate = DateTime.Today;
            });

            setMonth = new WCommand(o =>
            {
                BeginDate = DateTime.Today.AddMonths(-1);
                EndDate = DateTime.Today;
            });
        }

        

        WCommand openAddPage;
        public WCommand OpenAddPage { get { return openAddPage; } }

        WCommand loadNewList;
        public WCommand LoadNewList { get { return loadNewList; } }

        WCommand goNextPage;
        public WCommand GoNextPage { get { return goNextPage; } }

        WCommand goPreviousPage;
        public WCommand GoPreviousPage { get { return goPreviousPage; } }


        WCommand selectAll;
        public WCommand SelectAll { get { return selectAll; } }

        WCommand selectPublished;
        public WCommand SelectPublished { get { return selectPublished; } }

        WCommand selectTakenOnWork;
        public WCommand SelectTakenOnWork { get { return selectTakenOnWork; } }

        WCommand setThatDay;
        public WCommand SetThatDay { get { return setThatDay; } }

        WCommand setYesterday;
        public WCommand SetYesterday { get { return setYesterday; } }

        WCommand setWeek;
        public WCommand SetWeek { get { return setWeek; } }

        WCommand setMonth;
        public WCommand SetMonth { get { return setMonth; } }

        public async Task InitializeAsync() 
        {
            await CatchExeption.ExecuteWithCatchAsync(async () => 
            {
                await blogListDM.SetListAsync();
                List.Clear();
                IsBackButtonEnabled = blogListDM.Parametres.Page > 1;
                IsNextButtonEnabled = blogListDM.Parametres.Page < blogListDM.TotalPages;
                PageNumber = blogListDM.Parametres.Page;
                foreach (var item in blogListDM.DMList)
                {
                    var vm = new BlogItemVM(item, tab);
                    var ucitem = new UC_BlogItem2(vm);
                    vm.UCBlogItem = ucitem;
                    ucitem.Model.Notify_new_page += tab.TabAdd;
                    ucitem.Model.Notify_delete += DeleteItem;
                    List.Add(ucitem);
                }
            });
        }

        public void DeleteItem(UC_BlogItem2 item) 
        { 
            List.Remove(item);
        }

        public async Task AddItem(BlogContent blog)
        {
            await CatchExeption.ExecuteWithCatchAsync(async () =>
            {
                var dm = await blogListDM.CtreateBlogExampleDM(blog);
                var vm = new BlogItemVM(dm, tab);
                var ucitem = new UC_BlogItem2(vm);
                vm.UCBlogItem = ucitem;
                ucitem.Model.Notify_new_page += tab.TabAdd;
                ucitem.Model.Notify_delete += DeleteItem;
                List.Insert(0, ucitem);
            });
        }

        public string DesiredStatus
        {
            get 
            {
                switch (blogListDM.Parametres.Search) 
                {
                    case "ShowAll": return "Все";
                    case "ShowPublished": return "Опубликовано";
                    case "ShowInWork": return "В работе";
                    default: return "Все";
                }
            }
            set 
            {
                blogListDM.Parametres.Search = value; OnPropertyChanged("DesiredStatus");
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

        int? pageNumber;
        public int? PageNumber
        {
            get { return pageNumber; }
            set
            {
                pageNumber = value;
                OnPropertyChanged("PageNumber");
            }
        }

        bool isBackButtonEnabled;
        public bool IsBackButtonEnabled
        {
            get { return isBackButtonEnabled; }
            set
            {
                isBackButtonEnabled = value; OnPropertyChanged("IsBackButtonEnabled");
            }
        }

        bool isNextButtonEnabled;
        public bool IsNextButtonEnabled
        {
            get { return isNextButtonEnabled; }
            set
            {
                isNextButtonEnabled = value; OnPropertyChanged("IsNextButtonEnabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
