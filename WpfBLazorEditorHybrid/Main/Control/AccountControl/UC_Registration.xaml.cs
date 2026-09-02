
using System.Windows.Controls;
using WpfBLazorHybridClient.Main.AVM.Account;


namespace WpfBLazorHybridClient.Main.Control.AccountControl
{
    /// <summary>
    /// Логика взаимодействия для UC_Registration.xaml
    /// </summary>
    public partial class UC_Registration : UserControl
    {
        RegistrationVM model;
        public UC_Registration(RegistrationVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        public RegistrationVM Model { get { return model; } }
    }
}


