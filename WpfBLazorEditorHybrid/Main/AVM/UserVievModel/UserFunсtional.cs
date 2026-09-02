using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;

namespace WpfBLazorHybridClient.Main.AVM.UserVievModel
{
    public delegate void AddFunctionHandler(string menuItemText, WCommand command);
    public class UserFunсtional
    {
        protected ListTabVM tabViewModel;
        public event AddFunctionHandler AddFunctionNotify;

        public UserFunсtional(ListTabVM _tabViewModel) 
        {
            tabViewModel = _tabViewModel;
            workFUnit = null;
            blogFUnit = null;
            progectFUnit = null;
            serviceFUnit = null;
            contactFUnit = null;
            collectionFormFUnit = null;
        }


        WorkFUnit workFUnit;
        BlogFUnit blogFUnit;
        ProgectFUnit progectFUnit;
        ServiceFUnit serviceFUnit;
        ContactFUnit contactFUnit;
        CollectionFormFUnit collectionFormFUnit;


        public void CreateFunctions(User _user) 
        {
            foreach (var role in _user.UserRoles)
            {
                switch (role.Role) 
                {
                    case "work":
                        workFUnit = new WorkFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(workFUnit.FunctionDisplay, workFUnit.MenuCommand);
                        break;
                    case "blog":
                        blogFUnit = new BlogFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(blogFUnit.FunctionDisplay, blogFUnit.MenuCommand);
                        break;
                    case "progect":
                        progectFUnit = new ProgectFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(progectFUnit.FunctionDisplay, progectFUnit.MenuCommand);
                        break;
                    case "service":
                        serviceFUnit = new ServiceFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(serviceFUnit.FunctionDisplay, serviceFUnit.MenuCommand);
                        break;
                    case "contact":
                        contactFUnit = new ContactFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(contactFUnit.FunctionDisplay, contactFUnit.MenuCommand);
                        break;
                        case "mainpage":
                        collectionFormFUnit = new CollectionFormFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(collectionFormFUnit.FunctionDisplay, collectionFormFUnit.MenuCommand);
                        break;
                    case "admin":
                        workFUnit = new WorkFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(workFUnit.FunctionDisplay, workFUnit.MenuCommand);
                        blogFUnit = new BlogFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(blogFUnit.FunctionDisplay, blogFUnit.MenuCommand);
                        progectFUnit = new ProgectFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(progectFUnit.FunctionDisplay, progectFUnit.MenuCommand);
                        serviceFUnit = new ServiceFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(serviceFUnit.FunctionDisplay, serviceFUnit.MenuCommand);
                        contactFUnit = new ContactFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(contactFUnit.FunctionDisplay, contactFUnit.MenuCommand);
                        collectionFormFUnit = new CollectionFormFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(collectionFormFUnit.FunctionDisplay, collectionFormFUnit.MenuCommand);
                        break;
                }
            }
        }
    }

    public class UserFunctional2: UserFunсtional
    {
        public new event AddFunctionHandler AddFunctionNotify;
        public UserFunctional2(ListTabVM _tabViewModel):
            base(_tabViewModel)
        {
            userPageFUnit = null;
            userManagerFUnit = null;
        }


        UserPageFUnit userPageFUnit;
        UserManagerFUnit userManagerFUnit;

        public new void CreateFunctions(User _user) 
        {
            base.AddFunctionNotify += AddFunctionNotify;
            base.CreateFunctions(_user);
            
            foreach (var role in _user.UserRoles)
            {
                switch (role.Role)
                {
                    case "myrole":
                        userPageFUnit = new UserPageFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(userPageFUnit.FunctionDisplay, userPageFUnit.MenuCommand);
                        break;
                    case "users":
                        userManagerFUnit = new UserManagerFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(userManagerFUnit.FunctionDisplay, userManagerFUnit.MenuCommand);
                        break;
                    case "admin":
                        userPageFUnit = new UserPageFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(userPageFUnit.FunctionDisplay, userPageFUnit.MenuCommand);
                        userManagerFUnit = new UserManagerFUnit(tabViewModel, _user);
                        AddFunctionNotify?.Invoke(userManagerFUnit.FunctionDisplay, userManagerFUnit.MenuCommand);
                        break;

                }
            }
        }

    }
}
