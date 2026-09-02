
using System.Windows;
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Admin.AVM;

namespace WpfBLazorHybridClient.Functions.Admin.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_RoleItem.xaml
    /// </summary>
    public partial class UC_RoleItem : UserControl
    {
        
        public UC_RoleItem(RoleItemVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        RoleItemVM model;

        public RoleItemVM Model { get { return model; } }

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
