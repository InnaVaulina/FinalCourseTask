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
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;

namespace WpfBLazorHybridClient.Functions.Contacts.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_mailUs.xaml
    /// </summary>
    public partial class UC_mailUs : UserControl
    {
        LikeMailVM _model;
        public LikeMailVM Model
        {
            get { return _model; }
        }
        public UC_mailUs(LikeMailVM model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
            addParagraphPanel.Visibility = Visibility.Collapsed;
        }

        private void addTextButton_Click(object sender, RoutedEventArgs e)
        {
            addTextButton.Visibility = Visibility.Collapsed;
            addParagraphPanel.Visibility = Visibility.Visible;
        }

        private void likeTextButton_Click(object sender, RoutedEventArgs e)
        {
            addParagraphPanel.Visibility = Visibility.Collapsed;
            addTextButton.Visibility = Visibility.Visible;
        }
    }
}
