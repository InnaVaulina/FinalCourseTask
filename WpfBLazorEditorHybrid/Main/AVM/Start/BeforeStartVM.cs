using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Main.Control.AccountControl;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Main.AVM.Account;
using WpfBLazorHybridClient.Functions.Admin.AVM;

namespace WpfBLazorHybridClient.Main.AVM.Start
{

    public delegate void LogInUserHandler(User User);

    public class BeforeStartVM : INotifyPropertyChanged
    {
        //public event ChooseInterfaceHandler Notify_entry;
        public event LogInUserHandler Notify;

        AccountClientBase queryMaker;
        public BeforeStartVM()
        {
            queryMaker = new AccountClientBase();

            entry = new UC_Entry(new EntryVM(queryMaker));
            entry.Model.Notify += LogInPerformed;
            registration = new UC_Registration(new RegistrationVM(queryMaker));
            content = entry;

            selectEntryPage = new WCommand(o => { Content = entry; });
            selectRegistrationPage = new WCommand(o => { Content = registration; });
        }

        UC_Entry entry;
        UC_Registration registration;

        UserControl content;
        public UserControl Content { get { return content; } set { content = value; OnPropertyChanged("Content"); } }

        WCommand selectEntryPage;
        public WCommand SelectEntryPage { get { return selectEntryPage; } }

        WCommand selectRegistrationPage;
        public WCommand SelectRegistrationPage { get { return selectRegistrationPage; } }

        public void LogInPerformed(User user)
        {
            Notify?.Invoke(user);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
