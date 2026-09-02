using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;



namespace WpfBLazorHybridClient.Functions.Contacts.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_AddAddress.xaml
    /// </summary>
    public partial class UC_AddAddress : UserControl
    {
        LikeAddressVM _model;
        public LikeAddressVM Model 
        { 
            get { return _model; }
        }
        public UC_AddAddress(LikeAddressVM model)
        {
            InitializeComponent();
            _model = model;
            DataContext = Model;
            if(Model.Address == null) 
            {
                addTextButton.Visibility = Visibility.Visible;
                addParagraphPanel.Visibility = Visibility.Collapsed;
            }
            else 
            {                 
                addTextButton.Visibility = Visibility.Collapsed;
                addParagraphPanel.Visibility = Visibility.Visible;
                showText.Visibility = Visibility.Visible;
                editText.Visibility = Visibility.Collapsed;
            }

        }

        private void addTextButton_Click(object sender, RoutedEventArgs e)
        {
            addTextButton.Visibility = Visibility.Collapsed;
            addParagraphPanel.Visibility = Visibility.Visible;

            showText.Visibility = Visibility.Collapsed;
            editText.Visibility = Visibility.Visible;
        }

        private void likeTextButton_Click(object sender, RoutedEventArgs e)
        {
            showText.Visibility = Visibility.Visible;
            editText.Visibility = Visibility.Collapsed;
        }

        private void editText_Click(object sender, RoutedEventArgs e)
        {
            showText.Visibility = Visibility.Collapsed;
            editText.Visibility = Visibility.Visible;
        }

        private void deleteTextButton_Click(object sender, RoutedEventArgs e)
        {
            addParagraphPanel.Visibility = Visibility.Collapsed;
            addTextButton.Visibility = Visibility.Visible;
        }

        private void loadMap_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {

                    Model.PictureMapFilePath = new FileInfo(openFileDialog.FileName);                   
                    Model.PictureMapBitmap = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }
        }

        
    }
}
