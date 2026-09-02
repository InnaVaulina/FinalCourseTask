using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.BlazorComponents;
using WpfBLazorHybridClient.Functions.Progect.AVM;

namespace WpfBLazorHybridClient.Functions.Progect.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_ProgectItemAdd.xaml
    /// </summary>
    public partial class UC_ProgectItemAdd : UserControl
    {
        IProgectEdit model;
        public IProgectEdit Model { get { return model; } }


        public UC_ProgectItemAdd(IProgectEdit _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;

            var services = new ServiceCollection();
            services.AddWpfBlazorWebView();

            blazorWebView.HostPage = "wwwroot/editor.html";
            blazorWebView.Services = services.BuildServiceProvider();

            blazorWebView.RootComponents.Add(new RootComponent
            {
                ComponentType = typeof(ProgectEditor),
                Selector = "#app",
                Parameters = new Dictionary<string, object?>
                {
                    ["Model"] = Model
                }
            });



            Loaded += async (_, _) => {
                await blazorWebView.WebView.EnsureCoreWebView2Async();
                blazorWebView.WebView.NavigationCompleted += (_, args) =>
                {
                    if (args.IsSuccess)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LoadingIndicator.Visibility = Visibility.Collapsed;
                            blazorWebView.Visibility = Visibility.Visible;
                        });
                    }
                };
            };

        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {

                    Model.ImageFilePath = new FileInfo(openFileDialog.FileName);
                    Model.Picture = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                }
                Model.SaveImageContent(Model.ImageFilePath);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }
        }
    }
}
