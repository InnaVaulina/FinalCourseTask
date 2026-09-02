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
   
    public class UserVM
    {

        List<FunctionVM> functions;       
        public List<FunctionVM> Functions { get { return functions; } }

        public UserVM(User _user, ListTabVM _tabViewModel) 
        {
            functions = new List<FunctionVM>();
            UserFunctional2 userFunctional = new UserFunctional2(_tabViewModel);
            userFunctional.AddFunctionNotify += AddFunction;
            userFunctional.CreateFunctions(_user);
                       
        }

        public void AddFunction(string menuItemText, WCommand command) 
        {
            functions.Add(new FunctionVM(menuItemText, command));
        }


    }
}
