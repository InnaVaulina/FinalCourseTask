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

namespace WpfBLazorHybridClient.Functions.Contacts.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_EditSocialLink.xaml
    /// </summary>
    public partial class UC_EditSocialLink : UserControl
    {
        public UC_EditSocialLink()
        {
            InitializeComponent();
            likeTextButton.Visibility = Visibility.Collapsed;
            editText.Visibility = Visibility.Collapsed;
        }



        private void editText_Click(object sender, RoutedEventArgs e)
        {
            showText.Visibility = Visibility.Collapsed;
            editText.Visibility = Visibility.Visible;
            editTextButton.Visibility = Visibility.Collapsed;
            likeTextButton.Visibility = Visibility.Visible;
        }

        private void likeText_Click(object sender, RoutedEventArgs e)
        {
            editText.Visibility = Visibility.Collapsed;
            showText.Visibility = Visibility.Visible;
            likeTextButton.Visibility = Visibility.Collapsed;
            editTextButton.Visibility = Visibility.Visible;
        }
    }
}
