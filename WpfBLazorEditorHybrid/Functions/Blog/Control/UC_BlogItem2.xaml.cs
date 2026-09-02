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
    /// Логика взаимодействия для UC_BlogItem2.xaml
    /// </summary>
    public partial class UC_BlogItem2 : UserControl
    {
        BlogItemVM model;
        public BlogItemVM Model
        {
            get { return model; }
            set
            {
                model = value;
                DataContext = model;
            }
        }

        public UC_BlogItem2(BlogItemVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        
    }
}
