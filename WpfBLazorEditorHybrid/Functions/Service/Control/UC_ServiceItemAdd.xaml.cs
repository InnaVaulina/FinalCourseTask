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
using WpfBLazorHybridClient.BlazorComponents;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Service.AVM;

namespace WpfBLazorHybridClient.Functions.Service.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_ServiceItemAdd.xaml
    /// </summary>
    public partial class UC_ServiceItemAdd : UserControl
    {
        IServiceEdit model;
        public IServiceEdit Model { get { return model; } }

        public UC_ServiceItemAdd(IServiceEdit _model)
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
                ComponentType = typeof(ServiceEditor),
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

        
    }
}
