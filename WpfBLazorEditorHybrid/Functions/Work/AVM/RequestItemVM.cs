using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Client.Work;
using WpfBLazorHybridClient.Functions.Work.Control;

namespace WpfBLazorHybridClient.Functions.Work.AVM
{
    public class RequestItemVM
    {
        public event TabAddHandler Notify_new;

        WorkTableClient queryMaker;

        Request request;

        public int ID { get { return request.ID; } }
        public DateTime RequestIn { get { return request.RequestIn; } }
        public string ClientFullName { get { return request.FullName; } }
        public string Contact { get { return request.Contact; } }
        public string RequestText { get { return request.RequestText; } }
        public string PerformingInfo { get { return request.PerformingInfo; } }
        public string Status { get { return request.Status; } }


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

        public RequestItemVM(Request _request, WorkTableClient _queryMaker) 
        {
            request = _request;
            StatusTranslated = request.Status;
            queryMaker = _queryMaker;



            editRequesteItem = new WCommand(o =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Редактировать запрос",
                    Content = new UC_RequestTreating(new RequestTreatingVM(request,queryMaker))
                };
                Notify_new?.Invoke(page);
            });
        }


        WCommand editRequesteItem;
        public WCommand EditRequesteItem { get { return editRequesteItem; } }
    }
}
