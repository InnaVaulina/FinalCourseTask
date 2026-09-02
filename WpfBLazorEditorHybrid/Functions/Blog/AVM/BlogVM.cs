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
            ShowAll = true;
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
        }

        

        WCommand openAddPage;
        public WCommand OpenAddPage { get { return openAddPage; } }

        WCommand loadNewList;
        public WCommand LoadNewList { get { return loadNewList; } }

        WCommand goNextPage;
        public WCommand GoNextPage { get { return goNextPage; } }

        WCommand goPreviousPage;
        public WCommand GoPreviousPage { get { return goPreviousPage; } }

        public async Task InitializeAsync() 
        {
            try
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
            }
            catch (ScopedExeption ex)
            {
                Logger.Log(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
            
        }

        public void DeleteItem(UC_BlogItem2 item) 
        { 
            List.Remove(item);
        }

        public async Task AddItem(BlogContent blog)
        {
            try 
            {
                var dm = await blogListDM.CtreateBlogExampleDM(blog);
                var vm = new BlogItemVM(dm, tab);
                var ucitem = new UC_BlogItem2(vm);
                vm.UCBlogItem = ucitem;
                ucitem.Model.Notify_new_page += tab.TabAdd;
                ucitem.Model.Notify_delete += DeleteItem;
                List.Insert(0, ucitem);
            }
            catch (ScopedExeption ex)
            {
                Logger.Log(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
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
                    blogListDM.Parametres.Search = "ShowInWork";
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
