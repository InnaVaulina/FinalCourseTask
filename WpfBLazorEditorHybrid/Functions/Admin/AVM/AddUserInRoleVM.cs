using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.Control;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    public class AddUserInRoleVM : INotifyPropertyChanged
    {

        public event ExecuteDeleteUserHandler Notify_change;
        public event TabCloseHandler Notify_close;



        

        AccountClientExt queryMaker;

        TabVM page;

        public TabVM Page { get { return page; } set { page = value; } }

        string id;
        public string Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

        string userName;
        public string UserName { get { return userName; } set { userName = value; OnPropertyChanged("UserName"); } }

        List<UserRole> userRoles;
       


        ObservableCollection<UC_RoleItem> list;

        public ObservableCollection<UC_RoleItem> List { get { return list; } }

        public AddUserInRoleVM(UserItemVM _user, AccountClientExt _queryMaker) 
        {
            queryMaker = _queryMaker;

            id = _user.Id;
            userName = _user.UserName;
            userRoles = _user.UserRoles.ToList();
           

            list = new ObservableCollection<UC_RoleItem>();
            for (int i = 0; i < RoleDescr.roles.Length; i++)
            {
                RoleItemVM roleItem = new RoleItemVM()
                {
                    Role = RoleDescr.roles[i],
                    RoleName = RoleDescr.functionName[i],
                    Description = RoleDescr.deskr[i]
                };
                foreach (var role in userRoles) 
                {
                    if(role.Role == RoleDescr.roles[i])
                        roleItem.RoleChecked = true;
                }
                
                list.Add(new UC_RoleItem(roleItem)); 
            }


            saveRoles = new WCommand(o=> 
            {
                SaveUserRoles();
            });
        }


        WCommand saveRoles;
        public WCommand SaveRoles { get { return saveRoles; } }


        public void SaveUserRoles() 
        {
            List<UserRole> newUserRoles = new List<UserRole>();
            foreach(var role in List) 
            {
                if (role.Model.RoleChecked == true)
                    newUserRoles.Add(new UserRole() { Role = role.Model.Role });
            }

            UserModelForAdmin usermodel = new UserModelForAdmin()
            {
                Id = this.Id,
                UserName = this.UserName,
                UserRoles = newUserRoles
            };
            HttpResponseMessage result = Task.Run(() => queryMaker.ChangeUserRole(usermodel).GetAwaiter().GetResult()).Result;
            
            if ((int)result.StatusCode == 200 || (int)result.StatusCode == 201)
            {
                MessageBox.Show($"Запрос выполнен.");
                Notify_change?.Invoke();
                Notify_close?.Invoke(page);
            }
            else
            {
                //ErrorResultMessage.Show(result);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
