using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WpfBLazorHybridClient.BlazorComponents;
using WpfBLazorHybridClient.Functions.Service.AVM;

namespace WpfBLazorHybridClient.Functions.Service.Control
{ 

    /// <summary>
    /// Логика взаимодействия для UC_ServiceItem.xaml
    /// </summary>
    public partial class UC_ServiceItem : UserControl
    {
        ServiceItemVM model;

        public ServiceItemVM Model
        {
            get { return model; }
            set
            {
                model = value;
                DataContext = model;
            }
        }

        public UC_ServiceItem(ServiceItemVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;

            arrowUpButton.Visibility = Visibility.Hidden;
            arrowDownButton.Visibility = Visibility.Visible;
            serviseDescriptionBlock.Visibility = Visibility.Collapsed;
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
