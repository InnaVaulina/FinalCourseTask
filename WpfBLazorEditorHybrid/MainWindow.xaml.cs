
using System.Windows;
using WpfBLazorHybridClient.Main.AVM.Start;

namespace WpfBLazorHybridClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainWindowStartVM model;

        public MainWindow()
        {
            InitializeComponent();
            model = new MainWindowStartVM();
            DataContext = model;
            
        }
    }
}
