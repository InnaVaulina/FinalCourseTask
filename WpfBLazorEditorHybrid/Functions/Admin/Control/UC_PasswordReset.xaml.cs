
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Admin.AVM;

namespace WpfBLazorHybridClient.Functions.Admin.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_PasswordReset.xaml
    /// </summary>
    public partial class UC_PasswordReset : UserControl
    {
        public UC_PasswordReset(PasswordResetVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        PasswordResetVM model;

        public PasswordResetVM Model
        {
            get { return model; }
        }
    }
}
