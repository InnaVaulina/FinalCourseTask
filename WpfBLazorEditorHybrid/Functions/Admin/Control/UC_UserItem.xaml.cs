
using System.Windows;
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Admin.AVM;

namespace WpfBLazorHybridClient.Functions.Admin.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_UserItem.xaml
    /// </summary>
    public partial class UC_UserItem : UserControl
    {
        public UC_UserItem(UserItemVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        UserItemVM model;

        public UserItemVM Model { get { return model; } }


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
