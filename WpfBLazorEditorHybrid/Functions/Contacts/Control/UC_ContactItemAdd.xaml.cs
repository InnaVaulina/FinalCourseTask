using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;

namespace WpfBLazorHybridClient.Functions.Contacts.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_ContactItemAdd.xaml
    /// </summary>
    public partial class UC_ContactItemAdd : UserControl
    {
        ContactAddVM _model;
        public UC_ContactItemAdd(ContactAddVM model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
        }

        public ContactAddVM Model { get { return _model; } }
    }
}
