
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Main.AVM.UserVievModel;
using WpfBLazorHybridClient.DataModel;


namespace WpfBLazorHybridClient.Main.AVM.Start
{

    

    public class WorkStartVM: ListTabVM
    {
        public event ChooseInterfaceHandler Notify_exit;
        public WorkStartVM(User user):base()
        {

            _user = new UserVM(user, this);

            userExit = new WCommand(o => { Notify_exit?.Invoke(); });

        }

        UserVM _user;

        public List<FunctionVM> UserFunctions { get { return _user.Functions; } }



        WCommand userExit;
        public WCommand UserExit { get { return userExit; } }


        
    }
}
