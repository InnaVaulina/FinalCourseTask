using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Admin.AVM;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Contacts.Control;
using WpfBLazorHybridClient.Functions.Contacts.AVM;
using WpfBLazorHybridClient.Functions.Contacts.DM;
using WpfBLazorHybridClient.Functions.Header.AVM;
using WpfBLazorHybridClient.Functions.Header.Control;
using WpfBLazorHybridClient.Functions.Progect.AVM;
using WpfBLazorHybridClient.Functions.Progect.Control;
using WpfBLazorHybridClient.Functions.Service.AVM;
using WpfBLazorHybridClient.Functions.Service.Control;
using WpfBLazorHybridClient.Functions.UserHome.AVM;
using WpfBLazorHybridClient.Functions.UserHome.Control;
using WpfBLazorHybridClient.Functions.Work.AVM;
using WpfBLazorHybridClient.Functions.Work.Control;
using TTClassLibrary.Functions.Blog;
using WpfBLazorHybridClient.Client;
using TTClassLibrary.Functions.CollectionForm;

namespace WpfBLazorHybridClient.Main.AVM.UserVievModel
{
    public abstract class FunctionUnit
    {
        protected string functionDisplay;
        public string FunctionDisplay { get { return functionDisplay; } }

        protected ListTabVM tabViewModel;

        protected WCommand menuCommand;
        public WCommand MenuCommand { get { return menuCommand; } }

        public FunctionUnit(ListTabVM _tabViewModel) 
        {
            tabViewModel = _tabViewModel;
        }
    }


    public class WorkFUnit : FunctionUnit
    {
        UC_WorkMenu workTablePage;
        public WorkFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Рабочий стол";
            workTablePage = new UC_WorkMenu(new WorkTableVM(user));
            workTablePage.Model.Notify_new += tabViewModel.TabAdd;
            menuCommand = new WCommand(o =>
            {
                workTablePage.Model.UpdateList.Execute(null);
                tabViewModel.TabAdd(functionDisplay, workTablePage);
            });
        }
    }


   

    public class CollectionFormFUnit : FunctionUnit
    {
        UC_CollectionForm header;
        public CollectionFormFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Форма сбора заявок";

            var collectionFormDM = new EditCollectionFormDM(new CollectionFormRequestSender(new HttpRequestSender2(user)));
            header = new UC_CollectionForm(new EditCollectionFormVM(collectionFormDM));

            menuCommand = new WCommand(async o =>
            {
                await header.Model.EditCollectionFormDM.InitializeAsync();
                tabViewModel.TabAdd(functionDisplay, header);
            });
        }
    }

    

    public class BlogFUnit : FunctionUnit
    {
        UC_Blog blog;
        public BlogFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Блог";

            var rs = new TTClassLibrary.Functions.Blog.BlogRequestSenderWPF(new HttpRequestSender2(user));
            var blogListDM = new TTClassLibrary.Functions.Blog.BlogListDM(rs);
            blog = new UC_Blog(new BlogVM(blogListDM, _tabViewModel));
            blog.Model.Notify_new += tabViewModel.TabAdd;
            menuCommand = new WCommand(async _ =>
            {
                await blog.Model.InitializeAsync();
                tabViewModel.TabAdd(functionDisplay, blog);
            });
        }
    }

    public class ProgectFUnit : FunctionUnit
    {
        UC_Progects progects;
        public ProgectFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Проекты";
            var rs = new TTClassLibrary.Functions.Progect.ProgectRequestSenderWPF(new HttpRequestSender2(user));
            var progectListDM = new TTClassLibrary.Functions.Progect.ProgectListDM(rs);
            progects = new UC_Progects(new ProgectVM(progectListDM, _tabViewModel));
            progects.Model.Notify_new += tabViewModel.TabAdd;
            menuCommand = new WCommand(async _ =>
            {
                await progects.Model.InitializeAsync();
                tabViewModel.TabAdd(functionDisplay, progects);
            });
        }
    }

    public class ServiceFUnit : FunctionUnit
    {
        UC_Services services;
        public ServiceFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Услуги";
            var rs = new TTClassLibrary.Functions.Service.ServiceRequestSenderWPF(new HttpRequestSender2(user));
            var serviceListDM = new TTClassLibrary.Functions.Service.ServiceListDM(rs);
            services = new UC_Services(new ServiceVM(serviceListDM, _tabViewModel));
            services.Model.Notify_new += tabViewModel.TabAdd;
            menuCommand = new WCommand(async _ =>
            {
                await services.Model.InitializeAsync();
                tabViewModel.TabAdd(functionDisplay, services);
            });
        }
    }


    public class ContactFUnit : FunctionUnit
    {
        UC_Contact contact;
        public ContactFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Контакты";
            var contactListDM = new ContactListDM(new ContactClient(user));
            contact = new UC_Contact(new ContactVM(contactListDM, _tabViewModel));
            contact.Model.Notify_new += tabViewModel.TabAdd;

            menuCommand = new WCommand(async _ =>
            {
                await contact.Model.InitializeAsync();
                tabViewModel.TabAdd(functionDisplay, contact);
            });
        }
    }

    

    


    

    public class UserPageFUnit : FunctionUnit
    {
        UC_UserHome userHome;
        public UserPageFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Моя страница";
            userHome = new UC_UserHome(new UserHomeVM(user));
            menuCommand = new WCommand(o =>
            {
                tabViewModel.TabAdd(functionDisplay, userHome);
            });
        }
    }

    public class UserManagerFUnit : FunctionUnit
    {
        UC_UserManager userManager;
        public UserManagerFUnit(ListTabVM _tabViewModel, User user)
            : base(_tabViewModel)
        {
            functionDisplay = "Управление пользователями";
            userManager = new UC_UserManager(new UserManagerVM(user));
            userManager.Model.Notify_new += tabViewModel.TabAdd;
            menuCommand = new WCommand(o =>
            {
                userManager.Model.SelectUsers();
                tabViewModel.TabAdd(functionDisplay, userManager);
            });
        }
    }
}
