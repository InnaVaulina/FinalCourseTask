using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Client.Work;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;

namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public class RequestTreatingVM: INotifyPropertyChanged
    {
        Request? request;

        WorkTableClient queryMaker;
        HttpResponseMessageDeserialize<Request> jsonSerializer;

        public int ID 
        { 
            get { return request.ID; }
        }
        public string RequestIn 
        { 
            get { return request.RequestIn.ToString("dd.MM.yyyy"); }
        }
        public string ClientFullName 
        { 
            get { return request.FullName; }
        }
        public string Contact 
        { 
            get { return request.Contact; }
            set { request.Contact = value; OnPropertyChanged("Contact"); }
        }
        public string RequestText 
        { 
            get { return request.RequestText; }
            set { request.RequestText = value; OnPropertyChanged("RequestText"); }
        }
        public string PerformingInfo 
        { 
            get { return request.PerformingInfo; }
            set { request.PerformingInfo = value; OnPropertyChanged("PerformingInfo"); }
        }
        public string Status 
        { 
            get { return request.Status; }
            set 
            { 
                request.Status = value;
                StatusTranslated = value;
            }
        }

        string statusTranslated;
        public string StatusTranslated
        {
            get { return statusTranslated; }
            set 
            {
                switch (value) 
                {
                    case "received": statusTranslated = "Поступило"; break;
                    case "taken": statusTranslated = "В работе"; break;
                    case "rejected": statusTranslated = "Отклонено"; break;
                    case "finished": statusTranslated = "Выполнено"; break;
                    case "cancelled": statusTranslated = "Отменено"; break;
                }
                OnPropertyChanged("StatusTranslated");
            }           
        }

        public RequestTreatingVM(Request _request, WorkTableClient _queryMaker) 
        {
            request = _request;
            StatusTranslated = request.Status;
            queryMaker = _queryMaker;
            jsonSerializer = new HttpResponseMessageDeserialize<Request>(queryMaker);

            changeStatusTakeOnWork = new WCommand(o => 
            {
                Status = "taken";
                PerformingInfo = $"{PerformingInfo} {DateTime.Now}: изменен статус на {Status}\n";
            });
            changeStatusRejected = new WCommand(o => 
            {
                Status = "rejected";
                PerformingInfo = $"{PerformingInfo} {DateTime.Now}: изменен статус на {Status}\n";
            });
            changeStatusFinished = new WCommand(o => 
            {
                Status = "finished";
                PerformingInfo = $"{PerformingInfo} {DateTime.Now}: изменен статус на {Status}\n";
            });
            changeStatusCancelled = new WCommand(o => 
            {
                Status = "cancelled";
                PerformingInfo = $"{PerformingInfo} {DateTime.Now}: изменен статус на {Status}\n";
            });

            updateRequest = new WCommand(o => 
            {
                Request? response = null;
                response = jsonSerializer.DeserializeResponce(UpdateRequestEx(request));
                if(response == null) return;
                request = response;
                StatusTranslated = request.Status;
                OnPropertyChanged("Contact");
                OnPropertyChanged("RequestText");
                OnPropertyChanged("PerformingInfo");
            });
            saveRequest = new WCommand(o => { SaveRequestEx(request); });
        }

        public HttpResponseMessage UpdateRequestEx(Request request)
        {
            return Task.Run(() => queryMaker.UpdateRequest(request.ID).GetAwaiter().GetResult()).Result;
        }

        public HttpResponseMessage SaveRequestEx(Request request)
        {
            return Task.Run(() => queryMaker.SaveRequest(request).GetAwaiter().GetResult()).Result;
        }



        WCommand changeStatusTakeOnWork;
        public WCommand ChangeStatusTakeOnWork { get { return changeStatusTakeOnWork; } }

        WCommand changeStatusRejected;
        public WCommand ChangeStatusRejected { get { return changeStatusRejected; } }

        WCommand changeStatusFinished;
        public WCommand ChangeStatusFinished { get { return changeStatusFinished; } }

        WCommand changeStatusCancelled;
        public WCommand ChangeStatusCancelled { get { return changeStatusCancelled; } }


        WCommand updateRequest;
        public WCommand UpdateRequest { get { return updateRequest; } }

        WCommand saveRequest;
        public WCommand SaveRequest { get { return saveRequest; } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
