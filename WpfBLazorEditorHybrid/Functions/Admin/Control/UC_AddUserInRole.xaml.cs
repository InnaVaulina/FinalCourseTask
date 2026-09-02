
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Admin.AVM;

namespace WpfBLazorHybridClient.Functions.Admin.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_AddUserInRole.xaml
    /// </summary>
    public partial class UC_AddUserInRole : UserControl
    {
       
        public UC_AddUserInRole(AddUserInRoleVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        AddUserInRoleVM model;
        public AddUserInRoleVM Model
        {
            get { return model; }
        }
    }
}
