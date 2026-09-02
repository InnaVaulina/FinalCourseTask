using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для UC_SocialLink.xaml
    /// </summary>
    public partial class UC_SocialLink : UserControl
    {
        LikeLinkVM _model;
        public LikeLinkVM Model
        {
            get { return _model; }
        }
        public UC_SocialLink(LikeLinkVM model)
        {
            InitializeComponent();
            _model = model;
            DataContext = Model;
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

        private void loadLincIcon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {

                    Model.IconWorldNetFilePath = new FileInfo(openFileDialog.FileName);
                    Model.IconWorldNetBitmap = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }
        }
    }
}
