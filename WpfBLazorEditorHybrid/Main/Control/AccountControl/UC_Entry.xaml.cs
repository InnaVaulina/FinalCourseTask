using System.Windows.Controls;
using WpfBLazorHybridClient.Main.AVM.Account;

namespace WpfBLazorHybridClient.Main.Control.AccountControl
{
    /// <summary>
    /// Логика взаимодействия для UC_Entry.xaml
    /// </summary>
    public partial class UC_Entry : UserControl
    {
        
        public UC_Entry(EntryVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        EntryVM model;
        public EntryVM Model { get { return model; } }
    }
}
