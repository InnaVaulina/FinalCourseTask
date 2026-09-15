using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Main.AVM.Tab;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Work;
using WpfBLazorHybridClient.Functions.Work.Control;

namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public class RequestItemVM
    {
        public event TabAddHandler Notify_new;

        RequestExampleDM requestDM;

        public int ID { get { return requestDM.Content.ID; } }
        public DateTime RequestIn { get { return requestDM.Content.RequestIn; } }
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

        public RequestItemVM(RequestExampleDM _requestDM) 
        {
            requestDM = _requestDM;
            StatusTranslated = requestDM.Content.Status;

            editRequesteItem = new WCommand(o =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Редактировать запрос",
                    Content = new UC_RequestTreating(new RequestTreatingVM(requestDM))
                };
                Notify_new?.Invoke(page);
            });
        }


        WCommand editRequesteItem;
        public WCommand EditRequesteItem { get { return editRequesteItem; } }
    }
}
