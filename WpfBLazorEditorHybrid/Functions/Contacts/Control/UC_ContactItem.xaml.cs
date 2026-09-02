using System.Windows;
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Contacts.AVM;

namespace WpfBLazorHybridClient.Functions.Contacts.Control
{
    public delegate void UC_ContactItemStateChanged();
    /// <summary>
    /// Логика взаимодействия для UC_ContactItem.xaml
    /// </summary>
    public partial class UC_ContactItem : UserControl
    {
        ContactItemVM _model;
        public event UC_ContactItemStateChanged Notify_StateChanged;
        public UC_ContactItem(ContactItemVM model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
            _model.UCContactItem = this;
            serviseDescriptionBlock.Visibility = Visibility.Collapsed;
            if(_model.Contact.Address!=null && !string.IsNullOrEmpty( _model.Contact.Address.MapFileName))
            {
                addressMapIllustration.Visibility = Visibility.Visible;
            }
            
        }

        public ContactItemVM Model
        {
            get { return _model; }
            set
            {
                _model = value;
                DataContext = _model;
                _model.UCContactItem = this;
                Notify_StateChanged?.Invoke();
            }
        }

        private void arrowDownButton_Click(object sender, RoutedEventArgs e)
        {
            arrowDownButton.Visibility = Visibility.Hidden;
            arrowUpButton.Visibility = Visibility.Visible;
            serviseDescriptionBlock.Visibility = Visibility.Visible;

        }

        private void arrowUpButton_Click(object sender, RoutedEventArgs e)
        {
            arrowUpButton.Visibility = Visibility.Hidden;
            arrowDownButton.Visibility = Visibility.Visible;
            serviseDescriptionBlock.Visibility = Visibility.Collapsed;

        }
    }
}
