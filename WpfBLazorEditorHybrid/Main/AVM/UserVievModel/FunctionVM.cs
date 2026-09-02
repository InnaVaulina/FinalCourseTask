using WpfBLazorHybridClient.Command;


namespace WpfBLazorHybridClient.Main.AVM.UserVievModel
{
    public class FunctionVM
    {

        string functionDisplay;
        public string FunctionDisplay { get { return functionDisplay; } }


        WCommand menuCommand;
        public WCommand MenuCommand { get { return menuCommand; } }


        public FunctionVM(string _functionDisplay, WCommand _menuCommand) 
        {
            functionDisplay = _functionDisplay;
            menuCommand = _menuCommand;
        }

    }
}
