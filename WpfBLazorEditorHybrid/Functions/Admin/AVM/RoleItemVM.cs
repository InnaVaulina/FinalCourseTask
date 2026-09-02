
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    public class RoleItemVM : INotifyPropertyChanged
    {

        string role;
        public string Role { get { return role; } set { role = value; } }

        string roleName;
        public string RoleName { get { return roleName; } set { roleName = value; OnPropertyChanged("RoleName"); } }

        string deskr;
        public string Description { get { return deskr; } set { deskr = value; OnPropertyChanged("Description"); } }

        bool roleChecked;
        public bool RoleChecked { get { return roleChecked; } set { roleChecked = value; OnPropertyChanged("RoleChecked"); } }

        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
