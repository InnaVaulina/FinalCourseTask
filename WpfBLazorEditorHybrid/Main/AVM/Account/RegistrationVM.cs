using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Admin;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Main.AVM.Account
{
    public delegate void ExecuteDeleteUserHandler();
    public class RegistrationVM : INotifyPropertyChanged
    {        
        public event TabCloseHandler Notify_close;
        public event ExecuteDeleteUserHandler Notify_register;

        RegistrationDM registrationDM;

        TabVM page;
        public TabVM Page { get { return page; } set { page = value; } }

        public string LoginProp
        {
            get { return registrationDM.RegisterModel.LoginProp; }
            set { registrationDM.RegisterModel.LoginProp = value; OnPropertyChanged("LoginProp"); }
        }

        public string Password
        {
            get { return registrationDM.RegisterModel.Password; }
            set { registrationDM.RegisterModel.Password = value; OnPropertyChanged("Password"); }
        }
        public string ConfirmPassword
        {
            get { return registrationDM.RegisterModel.ConfirmPassword; }
            set { registrationDM.RegisterModel.ConfirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }


        public RegistrationVM(RegistrationDM _registrationDM) 
        {
            registrationDM = _registrationDM;

            regUser = new WCommand(async _ =>
            {
                await CatchExeption.ExecuteWithCatchAsync(async () =>
                {
                    if (LoginProp != "" && Password != "" && ConfirmPassword != "")
                        await registrationDM.RegisterNewUser();
                    MessageBox.Show($"Запрос выполнен.");
                    Notify_close?.Invoke(page);
                    Notify_register?.Invoke();
                });
            });
        }


        WCommand regUser;
        public WCommand RegUser { get { return regUser; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }



 
}
