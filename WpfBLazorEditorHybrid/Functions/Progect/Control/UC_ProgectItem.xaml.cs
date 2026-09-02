using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;
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
using WpfBLazorHybridClient.BlazorComponents;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Progect.AVM;

namespace WpfBLazorHybridClient.Functions.Progect.Control
{

    /// <summary>
    /// Логика взаимодействия для UC_ProgectItem.xaml
    /// </summary>
    public partial class UC_ProgectItem : UserControl
    {
        ProgectItemVM model;
        
        public ProgectItemVM Model
        {
            get { return model; }
            set
            {
                model = value;
                DataContext = model;
            }
        }

        public UC_ProgectItem(ProgectItemVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }
    }
}
