using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TTClassLibrary.Functions.Admin;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Functions.Admin.AVM;
using WpfBLazorHybridClient.Main.AVM.Account;
using WpfBLazorHybridClient.Main.Control.AccountControl;
using TTClassLibrary.Functions.Authorization;

namespace WpfBLazorHybridClient.Main.AVM.Start
{

    public delegate void LogInUserHandler(User User);

    public class BeforeStartVM : INotifyPropertyChanged
    {
        public event LogInUserHandler Notify;

        public BeforeStartVM()
        {
            var rs = new HttpRequestSender2(new User());

            var regVM = new RegistrationVM(new RegistrationDM(rs));
            regVM.Notify_register += RegistrationPerformed;
            registration = new UC_Registration(regVM);

            var entryVM = new EntryVM(new EntryDM(rs));
            entryVM.Notify += LogInPerformed;
            entry = new UC_Entry(entryVM);

            var resetPasswordVM = new ResetUserPasswordVM(new ResetPasswordDM(rs));
            resetPasswordVM.Notify_register += RegistrationPerformed;
            resetPassword = new UC_ResetPassword(resetPasswordVM);

            content = entry;

            selectEntryPage = new WCommand(o => { Content = entry; });
            selectRegistrationPage = new WCommand(o => { Content = registration; });
            selectPasswordResetPage = new WCommand(o => { Content = resetPassword; });
        }

        UC_Entry entry;
        UC_Registration registration;
        UC_ResetPassword resetPassword;

        UserControl content;
        public UserControl Content { get { return content; } set { content = value; OnPropertyChanged("Content"); } }

        WCommand selectEntryPage;
        public WCommand SelectEntryPage { get { return selectEntryPage; } }

        WCommand selectRegistrationPage;
        public WCommand SelectRegistrationPage { get { return selectRegistrationPage; } }

        WCommand selectPasswordResetPage;
        public WCommand SelectPasswordResetPage { get { return selectPasswordResetPage; } }

        public void LogInPerformed(User user)
        {
            Notify?.Invoke(user);
        }

        public void RegistrationPerformed()
        {
            Content = entry;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
