
using System.Collections.ObjectModel;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Client.Account;
using System.Net.Http;
using System.Text.Json;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Main.AVM.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    public delegate void ExecuteDeleteUserHandler();
    

    /*
     * управление пользователями: получение списка пользователей, регистрация нового ползователя
     */
    public class UserManagerVM
    {
        public event TabAddHandler Notify_new;
        

        AccountClientExt queryMaker;
        User user;
        

        ObservableCollection<UC_UserItem> list;

        public ObservableCollection<UC_UserItem> List { get { return list; } }


        public UserManagerVM(User _user) 
        {
            user = _user;
            queryMaker = new AccountClientExt(user);
            list = new ObservableCollection<UC_UserItem>();
            

            openAddPage = new WCommand(o =>
            {
                RegistrationVM model = new RegistrationVM(queryMaker);

                TabVM page = new TabVM()
                {
                    Header = "Регистрация пользователя",
                    Content = new UC_Registration2(model)
                };
                model.Page = page;
                model.Notify_register += DeleteUserNotify;
                model.Notify_close += page.ClosePage;
                Notify_new?.Invoke(page);
            });
        }

        

        public void SelectUsers()
        {
            list.Clear();
            HttpResponseMessage result = Task.Run(() => queryMaker.PullAdminList().GetAwaiter().GetResult()).Result;
            if ((int)result.StatusCode == 200 || (int)result.StatusCode == 201)
            {
                var jsonstream = result.Content.ReadAsStreamAsync().Result;
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var jsonstruct = JsonSerializer.DeserializeAsync<List<UserModelForDeserialization>>(jsonstream, options);

                var selectedusers = (from item in jsonstruct.Result
                                    group item by item.Id into g
                                    select new 
                                    { Id = g.Key,
                                      UserName = g.Select(i => i.UserName).First()}
                                    ).ToList();

                foreach (var user in selectedusers)
                {
                    var roles = (from item in jsonstruct.Result
                                where item.Id == user.Id
                                select new UserRole
                                {                                    
                                    Role = item.UserRole
                                }).ToList();                  

                    var rs = new ObservableCollection<UserRole>(roles);
                                    
                    List.Add(new UC_UserItem(new UserItemVM(queryMaker)
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        UserRoles = rs
                    }));
                    List.Last().Model.Notify_delete += DeleteUserNotify;
                    List.Last().Model.Notify_new += TabNotify;
                }

            }
            else
            {
                //ErrorResultMessage.Show(result);
            }

        }

        


        WCommand openAddPage;
        public WCommand OpenAddPage { get { return openAddPage; } }

        void TabNotify(TabVM page)
        {
            Notify_new?.Invoke(page);
        }

        void DeleteUserNotify() 
        {
            List.Clear();
            SelectUsers();
        }


    }
}
