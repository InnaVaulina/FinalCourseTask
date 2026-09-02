using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.BlazorComponents;
using System.IO;
using WpfBLazorHybridClient.Functions.Blog.AVM;

namespace WpfBLazorHybridClient.Functions.Blog.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_BlogItemAdd.xaml
    /// </summary>
    public partial class UC_BlogItemAdd : UserControl
    {
        IBlogEdit model;
        public IBlogEdit Model { get { return model; } }

        public UC_BlogItemAdd(IBlogEdit _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = Model;

            var services = new ServiceCollection();
            services.AddWpfBlazorWebView();

            blazorWebView.HostPage = "wwwroot/editor.html";
            blazorWebView.Services = services.BuildServiceProvider();

            blazorWebView.RootComponents.Add(new RootComponent
            {
                ComponentType = typeof(BlogEditor),
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
