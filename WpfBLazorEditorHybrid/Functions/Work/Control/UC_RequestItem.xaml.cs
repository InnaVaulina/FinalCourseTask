using System.Windows;
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Work.AVM;

namespace WpfBLazorHybridClient.Functions.Work.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_RequestItem.xaml
    /// </summary>
    public partial class UC_RequestItem : UserControl
    {
        
        public UC_RequestItem(RequestItemVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        RequestItemVM model;
        public RequestItemVM Model { get { return model; } }

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
