using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TTClassLibrary.Functions.Authorization;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Main.AVM.Account
{
    public class ResetUserPasswordVM : INotifyPropertyChanged
    {

        public event ExecuteRegistrationUserHandler Notify_register;
        ResetPasswordDM resetPasswordDM;

        public ResetUserPasswordVM(ResetPasswordDM _resetPasswordDM)
        {
            resetPasswordDM = _resetPasswordDM;

            saveNewPassword = new WCommand(async _ =>
            {
                await CatchExeption.ExecuteWithCatchAsync(async () =>
                {
                    if (string.IsNullOrEmpty(LoginProp))
                    {
                        MessageBox.Show("Поле Логин должно быть заполнено.");
                        return;
                    }

                    if (string.IsNullOrEmpty(Token))
                    {
                        MessageBox.Show("Поле Токен должно быть заполнено.");
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

                    await resetPasswordDM.ResetUserPassword();
                    MessageBox.Show($"Запрос выполнен.");
                    Notify_register?.Invoke();
                });
            });
        }

        public string LoginProp
        {
            get { return resetPasswordDM.ResetPasswordModel.LoginProp; }
            set { resetPasswordDM.ResetPasswordModel.LoginProp = value; OnPropertyChanged("LoginProp"); }
        }

        public string Token
        {
            get { return resetPasswordDM.ResetPasswordModel.Token; }
            set { resetPasswordDM.ResetPasswordModel.Token = value; OnPropertyChanged("Token"); }
        }
        public string Password
        {
            get { return resetPasswordDM.ResetPasswordModel.NewPassword; }
            set { resetPasswordDM.ResetPasswordModel.NewPassword = value; OnPropertyChanged("Password"); }
        }

        string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }


        WCommand saveNewPassword;
        public WCommand ChangePassword { get { return saveNewPassword; } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
