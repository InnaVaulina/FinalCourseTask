using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Authorization;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Main.AVM.Account
{
    public delegate void ExecuteRegistrationUserHandler();
    public class RegistrationVM : INotifyPropertyChanged
    {        
        public event ExecuteRegistrationUserHandler Notify_register;

        RegistrationDM registrationDM;


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
                    if(string.IsNullOrEmpty(LoginProp))
                    {
                        MessageBox.Show("Поле Логин должно быть заполнено.");
                        return;
                    }

                    if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                    {
                        MessageBox.Show("Поля Пароль и Подтверждение пароля должны быть заполнены.");
                        return;
                    }

                    if (Password != ConfirmPassword)
                    {
                        MessageBox.Show("Пароль и Подтверждение пароля не совпадают.");
                        return;
                    }

                    await registrationDM.RegisterNewUser();
                    MessageBox.Show($"Запрос выполнен.");
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
