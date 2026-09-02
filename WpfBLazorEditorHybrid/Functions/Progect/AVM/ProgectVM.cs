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
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Progect.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Progect.AVM
{

    public delegate void DeleteProgectHandler(UC_ProgectItem? item);
    public delegate void UpdateProgectHandler();
    public delegate Task AddProgectHandler(ProgectContent? progect);
    public class ProgectVM: INotifyPropertyChanged
    {

        public event TabAddHandler Notify_new;
        ListTabVM tab;
        ProgectListDM progectListDM;

        ObservableCollection<UC_ProgectItem> list;
        public ObservableCollection<UC_ProgectItem> List { get { return list; } }

        public ProgectVM(ProgectListDM _progectListDM, ListTabVM _tab) 
        {
            progectListDM = _progectListDM;
            tab = _tab;
            list = new ObservableCollection<UC_ProgectItem>();
            ShowAll = true;
            isBackButtonEnabled = false;
            isNextButtonEnabled = false;
            pageNumber = 1;

            openAddPage = new WCommand(o =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Добавить пост"
                };

                
                var model = new AddProgectVM(progectListDM.CreateAddNewProgectExampleDM(), page);
                model.Notify_close_page += tab.TabClose;
                model.Notify_add += AddItem;

                page.Content = new UC_ProgectItemAdd(model);
                Notify_new?.Invoke(page);
            });

            loadNewList = new WCommand(async _ =>
            {
                await InitializeAsync();
            });
            goNextPage = new WCommand(async _ =>
            {
                if (progectListDM.Parametres.Page < progectListDM.TotalPages)
                {
                    progectListDM.Parametres.Page++;
                    await InitializeAsync();
                }
            });
            goPreviousPage = new WCommand(async _ =>
            {
                if (progectListDM.Parametres.Page > 1)
                {
                    progectListDM.Parametres.Page--;
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
                await progectListDM.SetListAsync();
                
                IsBackButtonEnabled = progectListDM.Parametres.Page > 1;
                IsNextButtonEnabled = progectListDM.Parametres.Page < progectListDM.TotalPages;
                PageNumber = progectListDM.Parametres.Page;
                foreach (var item in progectListDM.DMList)
                {
                    var vm = new ProgectItemVM(item, tab);
                    var ucitem = new UC_ProgectItem(vm);
                    vm.UCProgectItem = ucitem;
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

        public void DeleteItem(UC_ProgectItem item)
        {
            list.Remove(item);
        }


        public async Task AddItem(ProgectContent? progect)
        {
            try
            {
                var dm = await progectListDM.CtreateProgectExampleDM(progect);
                var vm = new ProgectItemVM(dm, tab);
                var ucitem = new UC_ProgectItem(vm);
                vm.UCProgectItem = ucitem;
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
                    progectListDM.Parametres.Search = "ShowAll";
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
                    progectListDM.Parametres.Search = "ShowPublished";
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
                    progectListDM.Parametres.Search = "ShowInWork";
                }
            }
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
