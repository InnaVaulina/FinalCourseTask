
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Admin.AVM;

namespace WpfBLazorHybridClient.Functions.Admin.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_UserManager.xaml
    /// </summary>
    public partial class UC_UserManager : UserControl
    {

        public UC_UserManager(UserManagerVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        UserManagerVM model;
        public UserManagerVM Model 
        {
            get { return model; }
        }
    }
}
