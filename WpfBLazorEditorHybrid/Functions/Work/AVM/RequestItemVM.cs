using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Work;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Work.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public class RequestItemVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new;

        ListTabVM tab;
        RequestExampleDM requestDM;

        public int ID { get { return requestDM.Content.ID; } }
        public string RequestIn 
        {
            get
            {
                DateTime result = DateTime.ParseExact(requestDM.Content.RequestIn, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                return result.ToString("D");
            }
        }
        public string ClientFullName { get { return requestDM.Content.FullName; } }
        public string Contact { get { return requestDM.Content.Contact; } }
        public string RequestText { get { return requestDM.Content.RequestText; } }
        public string PerformingInfo { get { return requestDM.Content.PerformingInfo; } }
        public string Status { get { return requestDM.Content.Status; } }


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
                
            }
        }

        public RequestItemVM(RequestExampleDM _requestDM, ListTabVM _tab) 
        {
            requestDM = _requestDM;
            tab = _tab;
            StatusTranslated = requestDM.Content.Status;

            editRequesteItem = new WCommand(o =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Редактировать запрос"
                };
                var _workVM = new RequestTreatingVM(requestDM, page);
                _workVM.UC_RequestItem = uC_RequestItem;
                _workVM.Notify_update += NotyfyUpdate;
                _workVM.Notify_close_page += tab.TabClose;
                page.Content = new UC_RequestTreating(_workVM);

                Notify_new?.Invoke(page);
            });
        }

        public void NotyfyUpdate()
        {
            OnPropertyChanged("RequestText");
        }

        UC_RequestItem uC_RequestItem;
        public UC_RequestItem UC_RequestItem
        {
            set { uC_RequestItem = value; }
        }


        WCommand editRequesteItem;
        public WCommand EditRequesteItem { get { return editRequesteItem; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
