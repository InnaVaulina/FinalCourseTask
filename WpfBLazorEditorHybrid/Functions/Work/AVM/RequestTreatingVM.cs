using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Work;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public class RequestTreatingVM: INotifyPropertyChanged
    {
        RequestExampleDM requestDM;

        public int ID 
        { 
            get { return requestDM.Content.ID; }
        }
        public string RequestIn 
        { 
            get { return requestDM.Content.RequestIn.ToString("dd.MM.yyyy"); }
        }
        public string ClientFullName 
        { 
            get { return requestDM.Content.FullName; }
        }
        public string Contact 
        { 
            get { return requestDM.Content.Contact; }
            set { requestDM.Content.Contact = value; OnPropertyChanged("Contact"); }
        }
        public string RequestText 
        { 
            get { return requestDM.Content.RequestText; }
            set { requestDM.Content.RequestText = value; OnPropertyChanged("RequestText"); }
        }
        public string PerformingInfo 
        { 
            get { return requestDM.Content.PerformingInfo; }
            set { requestDM.Content.PerformingInfo = value; OnPropertyChanged("PerformingInfo"); }
        }
        public string Status 
        { 
            get { return requestDM.Content.Status; }
            set 
            { 
                requestDM.Content.Status = value;
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

        public RequestTreatingVM(RequestExampleDM _requestDM) 
        {
            requestDM = _requestDM;
            StatusTranslated = requestDM.Content.Status;

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

            updateRequest = new WCommand(async _ => 
            {
                try
                {
                    var response = await requestDM.UpdateRequestEx();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<Request>();
                    var newcontent = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newcontent != null)
                    {
                        MessageBox.Show("Запрос выполнен успешно");
                        //Notify_add?.Invoke(newcontent);
                        //Notify_close_page?.Invoke(page);
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


            saveRequest = new WCommand(async _ =>
            {
                try 
                {
                    var response = await requestDM.SaveRequestEx();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<Request>();
                    var newcontent = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newcontent != null)
                    {
                        MessageBox.Show("Запрос выполнен успешно");
                        //Notify_add?.Invoke(newcontent);
                        //Notify_close_page?.Invoke(page);
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
