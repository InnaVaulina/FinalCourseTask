
using System.Windows.Controls;
using WpfBLazorHybridClient.Main.AVM.Account;

namespace WpfBLazorHybridClient.Functions.Admin.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_Registration2.xaml
    /// </summary>
    public partial class UC_Registration2 : UserControl
    {
        
        public UC_Registration2(RegistrationVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        RegistrationVM model;
        public RegistrationVM Model { get { return model; } }
    }
}
