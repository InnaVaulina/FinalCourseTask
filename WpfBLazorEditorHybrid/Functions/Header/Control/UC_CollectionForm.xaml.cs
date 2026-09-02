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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfBLazorHybridClient.Functions.Header.AVM;
using WpfBLazorHybridClient.BlazorComponents;

namespace WpfBLazorHybridClient.Functions.Header.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_CollectionForm.xaml
    /// </summary>
    public partial class UC_CollectionForm : UserControl
    {
        EditCollectionFormVM model;
        public EditCollectionFormVM Model { get { return model; } }
        public UC_CollectionForm(EditCollectionFormVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = Model;

            var services = new ServiceCollection();
            services.AddWpfBlazorWebView();

            blazorWebView.HostPage = "wwwroot/header.html";
            blazorWebView.Services = services.BuildServiceProvider();

            blazorWebView.RootComponents.Add(new RootComponent
            {
                ComponentType = typeof(HeaderEditor),
                Selector = "#app",
                Parameters = new Dictionary<string, object?>
                {
                    ["Model"] = Model
                }
            });



            Loaded += async (_, _) =>
            {
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
                    Model.SaveImageContent(new FileInfo(openFileDialog.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }
        }
    }
}

