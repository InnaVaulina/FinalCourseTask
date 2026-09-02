using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Work.Control;
using WpfBLazorHybridClient.Client.Work;
using WpfBLazorHybridClient.Command;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfBLazorHybridClient.Client;

namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public class WorkTableVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new;

        WorkTableClient queryMaker;
        HttpResponseMessageDeserialize<List<Request>> jsonSerializer;

        ListTabVM tabViewModel;

        User user;
        public User User { set { user = value; } }

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



        public WorkTableVM(User _user)
        {
            user = _user;

            queryMaker = new WorkTableClient(user);
            jsonSerializer = new HttpResponseMessageDeserialize<List<Request>>(queryMaker);
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

            updateList = new WCommand(o =>
            {
                RequestRange range = new RequestRange(){ Start = StartTime, End = EndTime };
                switch (desiredStatus)
                {
                    case "Все заявки":                      
                        NewList(jsonSerializer.DeserializeResponce(SelectAllRequests(range)));
                        SelectedStatus = "Все заявки";
                        break;
                    case "Поступило":
                        NewList(jsonSerializer.DeserializeResponce(SelectReceivedRequests(range)));
                        SelectedStatus = "Поступило";
                        break;
                    case "В работе":
                        NewList(jsonSerializer.DeserializeResponce(SelectTakenOnWorkRequests(range)));
                        SelectedStatus = "В работе";
                        break;
                    case "Отклонена":
                        NewList(jsonSerializer.DeserializeResponce(SelectRejectedRequests(range)));
                        SelectedStatus = "Отклонена";
                        break;
                    case "Выполнена":
                        NewList(jsonSerializer.DeserializeResponce(SelectFinishedRequests(range)));
                        SelectedStatus = "Выполнена";
                        break;
                    case "Отменена":
                        NewList(jsonSerializer.DeserializeResponce(SelectCancelledRequests(range)));
                        SelectedStatus = "Отменена";
                        break;
                }
            });

        }



        public void NewList(List<Request>? requestList)
        {
            if(requestList == null)
            {
                return;
            }
            list.Clear();
            foreach (var item in requestList)
            {
                list.Add(new UC_RequestItem(new RequestItemVM(item, queryMaker)));
                list.Last().Model.Notify_new += TabNotify;
            }
        }

        public HttpResponseMessage SelectAllRequests(RequestRange range)
        {
            return Task.Run(() => queryMaker.GetAllRequests(range).GetAwaiter().GetResult()).Result;
        }

        public HttpResponseMessage SelectReceivedRequests(RequestRange range)
        {
            return Task.Run(() => queryMaker.GetReceivedRequests(range).GetAwaiter().GetResult()).Result;
        }

        public HttpResponseMessage SelectTakenOnWorkRequests(RequestRange range)
        {
            return Task.Run(() => queryMaker.GetTakenOnWorkRequests(range).GetAwaiter().GetResult()).Result;
        }

        public HttpResponseMessage SelectRejectedRequests(RequestRange range)
        {
            return Task.Run(() => queryMaker.GetRejectedRequests(range).GetAwaiter().GetResult()).Result;
        }

        public HttpResponseMessage SelectFinishedRequests(RequestRange range)
        {
            return Task.Run(() => queryMaker.GetFinishedRequests(range).GetAwaiter().GetResult()).Result;
        }

        public HttpResponseMessage SelectCancelledRequests(RequestRange range)
        {
            return Task.Run(() => queryMaker.GetCancelledRequests(range).GetAwaiter().GetResult()).Result;
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
