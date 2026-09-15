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
using TTClassLibrary.Functions.Work;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Work.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;


namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public class WorkTableVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new;

        WorkTableDM workTableDM;
        ListTabVM tabViewModel;

        ObservableCollection<UC_RequestItem> list;
        public ObservableCollection<UC_RequestItem> List { get { return list; } }

        DateTime startTime;
        public DateTime StartTime 
        {
            get { return startTime; }
            set 
            { 
                startTime = value; 
                StartTimeText = startTime.ToString("dd.MM.yyyy");
                OnPropertyChanged("StartTime");               
            }
        }

        string startTimeText;
        public string StartTimeText 
        { 
            get { return startTimeText; }
            set { startTimeText = value; OnPropertyChanged("StartTimeText"); }
        }



        DateTime endTime;
        public DateTime EndTime 
        {
            get { return endTime; }
            set 
            { 
                endTime = value;
                EndTimeText = endTime.ToString("dd.MM.yyyy");
                OnPropertyChanged("EndTime");
            }
        }

        string endTimeText;
        public string EndTimeText 
        { 
            get { return endTimeText; }
            set { endTimeText = value; OnPropertyChanged("EndTimeText"); }
        }

        string selectedStatus;
        public string SelectedStatus 
        {
            get { return selectedStatus; }
            set {  selectedStatus = value; OnPropertyChanged("SelectedStatus"); }
        }

        string desiredStatus;
        public string DesiredStatus
        {
            get { return desiredStatus; }
            set { desiredStatus = value; OnPropertyChanged("DesiredStatus"); }
        }



        public WorkTableVM(WorkTableDM _workTableDM, ListTabVM _tab)
        {
            workTableDM = _workTableDM;
            list = new ObservableCollection<UC_RequestItem>();

            StartTime = DateTime.Today;
            EndTime = DateTime.Today;
            desiredStatus = "Поступило";


            selectAll = new WCommand(o => { DesiredStatus = "Все заявки";});
            selectReceived = new WCommand(o => { DesiredStatus = "Поступило";});
            selectTakenOnWork = new WCommand(o => { DesiredStatus = "В работе";});
            selectRejected = new WCommand(o => { DesiredStatus = "Отклонена";});
            selectFinished = new WCommand(o => { DesiredStatus = "Выполнена";});
            selectCancelled = new WCommand(o => { DesiredStatus = "Отменена";});

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

            updateList = new WCommand(async _ =>
            {
                RequestRange range = new RequestRange(){ Start = StartTime, End = EndTime };
                try 
                {
                    List<RequestExampleDM> dmlist;
                    switch (desiredStatus)
                    {
                        case "Все заявки":
                            dmlist = await workTableDM.SelectAllRequests(range);
                            SetNewList(dmlist);
                            SelectedStatus = "Все заявки";
                            break;
                        case "Поступило":
                            dmlist = await workTableDM.SelectReceivedRequests(range);
                            SetNewList(dmlist);
                            SelectedStatus = "Поступило";
                            break;
                        case "В работе":
                            dmlist = await workTableDM.SelectTakenOnWorkRequests(range);
                            SetNewList(dmlist);
                            SelectedStatus = "В работе";
                            break;
                        case "Отклонена":
                            dmlist = await workTableDM.SelectRejectedRequests(range);
                            SetNewList(dmlist);
                            SelectedStatus = "Отклонена";
                            break;
                        case "Выполнена":
                            dmlist = await workTableDM.SelectFinishedRequests(range);
                            SetNewList(dmlist);
                            SelectedStatus = "Выполнена";
                            break;
                        case "Отменена":
                            dmlist = await workTableDM.SelectCancelledRequests(range);
                            SetNewList(dmlist);
                            SelectedStatus = "Отменена";
                            break;
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
            
            });

        }

        public void SetNewList(List<RequestExampleDM> dmList)
        {
            list.Clear();
            foreach (var item in dmList)
            {
                list.Add(new UC_RequestItem(new RequestItemVM(item)));
                list.Last().Model.Notify_new += TabNotify;
            }
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


        void TabNotify(TabVM page)
        {
            Notify_new?.Invoke(page);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        
    }

}
