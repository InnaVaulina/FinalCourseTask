using System.Windows.Controls;
using WpfBLazorHybridClient.Main.AVM.Start;
using WpfBLazorHybridClient.DataModel;

namespace WpfBLazorHybridClient.Main.Control.BusinessControl
{
    /// <summary>
    /// Логика взаимодействия для UC_Business.xaml
    /// </summary>
    public partial class UC_Business : UserControl
    {
       

        WorkStartVM model;
        public UC_Business(User user)
        {
            InitializeComponent();

            model = new WorkStartVM(user);
            this.DataContext = model;
        }

        public WorkStartVM Model { get { return model; } }
    }
}
