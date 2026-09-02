using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfBLazorHybridClient.Main.Control.BusinessControl;
using WpfBLazorHybridClient.Main.Control.AccountControl;
using WpfBLazorHybridClient.DataModel;

namespace WpfBLazorHybridClient.Main.AVM.Start
{
    public delegate void ChooseInterfaceHandler();
    public class MainWindowStartVM : INotifyPropertyChanged
    {
        

        public MainWindowStartVM() 
        {
            _userEntry = new UC_Account(new BeforeStartVM());
            _userEntry.Model.Notify += ChooseBusiness;

            content = _userEntry;
        }


        UC_Account _userEntry;
        UC_Business _userBusiness;
        
        
        UserControl content;
        public UserControl Content { get { return content; } set { content = value; OnPropertyChanged("Content"); } }

        public void ChooseAccount() 
        {
            _userEntry = new UC_Account(new BeforeStartVM());
            _userEntry.Model.Notify += ChooseBusiness;
            Content = _userEntry;           
        }

        public void ChooseBusiness(User user) 
        {
            _userBusiness = new UC_Business(user);
            _userBusiness.Model.Notify_exit += ChooseAccount;
            Content = _userBusiness;
            
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
