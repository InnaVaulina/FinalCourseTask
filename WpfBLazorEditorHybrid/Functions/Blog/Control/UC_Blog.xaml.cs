using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfBLazorHybridClient.Functions.Blog.AVM;

namespace WpfBLazorHybridClient.Functions.Blog.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_Blog.xaml
    /// </summary>
    public partial class UC_Blog : UserControl
    {
        BlogVM model;
        public BlogVM Model { get { return model; } }
        public UC_Blog(BlogVM model)
        {
            InitializeComponent();
            this.model = model;
            DataContext = model;
        }

        

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPanel.Visibility = Visibility.Visible;
            FilterButton.Visibility = Visibility.Hidden;
        }

        private void FilterHideButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPanel.Visibility = Visibility.Hidden;
            FilterButton.Visibility = Visibility.Visible;
        }
    }
}
