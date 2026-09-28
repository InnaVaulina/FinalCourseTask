using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Work;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Work.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;


namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public delegate void UpdateWorkHandler();
    public class WorkTableVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new;

        WorkTableDM workTableDM;
        ListTabVM tab;

        ObservableCollection<UC_RequestItem> list;
        public ObservableCollection<UC_RequestItem> List { get { return list; } }

        public DateTime? StartTime 
        {
            get { return workTableDM.Parametres.BeginDate; }
            set 
            {
                workTableDM.Parametres.BeginDate = value; 
                OnPropertyChanged("StartTime");               
            }
        }

        public DateTime? EndTime 
        {
            get { return workTableDM.Parametres.EndDate; }
            set 
            {
                workTableDM.Parametres.EndDate = value;
                OnPropertyChanged("EndTime");
            }
        }


        string startTimeText;
        public string StartTimeText
        {
            get { return startTimeText; }
            set { startTimeText = value; OnPropertyChanged("StartTimeText"); }
        }

        string endTimeText;
        public string EndTimeText
        {
            get { return endTimeText; }
            set { endTimeText = value; OnPropertyChanged("EndTimeText"); }
        }

        string? selectedStatus;
        public string? SelectedStatus 
        {
            get { return selectedStatus; }
            set {  selectedStatus = value; OnPropertyChanged("SelectedStatus"); }
        }


        public string? DesiredStatus
        {
            get
            {
                switch (workTableDM.Parametres.Search)
                {
                    case "ShowAll": return "Все";
                    case "ShowReceived": return "Поступило";
                    case "ShowTaken": return "В работе";
                    case "ShowRejected": return "Отклонена";
                    case "ShowFinished": return "Выполнена";
                    case "ShowCancelled": return "Отменена";
                    default: return "Все";
                }
            }
            set { workTableDM.Parametres.Search = value; OnPropertyChanged("DesiredStatus"); }
        }



        public WorkTableVM(WorkTableDM _workTableDM, ListTabVM _tab)
        {
            workTableDM = _workTableDM;
            tab = _tab;
            list = new ObservableCollection<UC_RequestItem>();

            startTimeText = workTableDM.Parametres.BeginDate.HasValue ? workTableDM.Parametres.BeginDate.Value.ToString("D") : DateTime.MinValue.ToString("D");
            endTimeText = workTableDM.Parametres.EndDate.HasValue ? workTableDM.Parametres.EndDate.Value.ToString("D") : DateTime.Now.ToString("D");
            selectedStatus = DesiredStatus;

            isBackButtonEnabled = false;
            isNextButtonEnabled = false;
            pageNumber = 1;

            selectAll = new WCommand(o => { DesiredStatus = "ShowAll"; });
            selectReceived = new WCommand(o => { DesiredStatus = "ShowReceived"; });
            selectTakenOnWork = new WCommand(o => { DesiredStatus = "ShowTaken"; });
            selectRejected = new WCommand(o => { DesiredStatus = "ShowRejected"; });
            selectFinished = new WCommand(o => { DesiredStatus = "ShowFinished"; });
            selectCancelled = new WCommand(o => { DesiredStatus = "ShowCancelled"; });

            setThatDay = new WCommand(o =>
            {
                StartTime = DateTime.Today;
                EndTime = DateTime.Today;
            });

            setYesterday = new WCommand(o =>
            {
                StartTime = DateTime.Today.AddDays(-1);
                EndTime = DateTime.Today.AddDays(-1);
            });

            setWeek = new WCommand(o =>
            {
                StartTime = DateTime.Today.AddDays(-7);
                EndTime = DateTime.Today;
            });

            setMonth = new WCommand(o =>
            {
                StartTime = DateTime.Today.AddMonths(-1);
                EndTime = DateTime.Today;
            });

            goNextPage = new WCommand(async _ =>
            {
                if (workTableDM.Parametres.Page < workTableDM.TotalPages)
                {
                    workTableDM.Parametres.Page++;
                    await InitializeAsync();
                }
            });
            goPreviousPage = new WCommand(async _ =>
            {
                if (workTableDM.Parametres.Page > 1)
                {
                    workTableDM.Parametres.Page--;
                    await InitializeAsync();
                }
            });

            updateList = new WCommand(async _ =>
            {
                await InitializeAsync();
            });

        }

        public async Task InitializeAsync()
        {
            await CatchExeption.ExecuteWithCatchAsync(async () =>
            {
                await workTableDM.SelectRequests();
                List.Clear();
                IsBackButtonEnabled = workTableDM.Parametres.Page > 1;
                IsNextButtonEnabled = workTableDM.Parametres.Page < workTableDM.TotalPages;
                PageNumber = workTableDM.Parametres.Page;
                foreach (var item in workTableDM.DMList)
                {
                    var vm = new RequestItemVM(item, tab);
                    var ucitem = new UC_RequestItem(vm);
                    vm.UC_RequestItem = ucitem;
                    ucitem.Model.Notify_new += tab.TabAdd;
                    List.Add(ucitem);
                }
                StartTimeText = workTableDM.Parametres.BeginDate.HasValue ? workTableDM.Parametres.BeginDate.Value.ToString("D") : DateTime.MinValue.ToString("D");
                EndTimeText = workTableDM.Parametres.EndDate.HasValue ? workTableDM.Parametres.EndDate.Value.ToString("D") : DateTime.Now.ToString("D");
                SelectedStatus = DesiredStatus;
            });
        }


        WCommand selectAll;
        public WCommand SelectAll { get { return selectAll; } }

        WCommand selectReceived;
        public WCommand SelectReceived { get { return selectReceived; } }

        WCommand selectTakenOnWork;
        public WCommand SelectTakenOnWork { get { return selectTakenOnWork; } }

        WCommand selectRejected;
        public WCommand SelectRejected { get { return selectRejected; } }

        WCommand selectFinished;
        public WCommand SelectFinished { get { return selectFinished; } }

        WCommand selectCancelled;
        public WCommand SelectCancelled { get { return selectCancelled; } }


        WCommand setThatDay;
        public WCommand SetThatDay { get { return setThatDay; } }

        WCommand setYesterday;
        public WCommand SetYesterday { get { return setYesterday; } }

        WCommand setWeek;
        public WCommand SetWeek { get { return setWeek; } }

        WCommand setMonth;
        public WCommand SetMonth { get { return setMonth; } }


        WCommand updateList;
        public WCommand UpdateList { get { return updateList; } }

        WCommand goNextPage;
        public WCommand GoNextPage { get { return goNextPage; } }

        WCommand goPreviousPage;
        public WCommand GoPreviousPage { get { return goPreviousPage; } }


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
