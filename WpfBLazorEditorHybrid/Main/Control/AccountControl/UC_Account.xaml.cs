using System.Windows.Controls;
using WpfBLazorHybridClient.Main.AVM.Start;

namespace WpfBLazorHybridClient.Main.Control.AccountControl
{
    /// <summary>
    /// Логика взаимодействия для UC_Account.xaml
    /// </summary>
    public partial class UC_Account : UserControl
    {
       
        public UC_Account(BeforeStartVM _model)
        {
            InitializeComponent();
            model = _model;
            this.DataContext = model;
        }

        BeforeStartVM model;
        public BeforeStartVM Model { get { return model; } }
    }
}
