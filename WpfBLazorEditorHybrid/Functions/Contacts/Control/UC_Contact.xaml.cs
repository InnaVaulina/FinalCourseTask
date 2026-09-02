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
using WpfBLazorHybridClient.Functions.Contacts.AVM;

namespace WpfBLazorHybridClient.Functions.Contacts.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_Contact.xaml
    /// </summary>
    public partial class UC_Contact : UserControl
    {
        ContactVM _model;
        public UC_Contact(ContactVM model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
            
        }

        public ContactVM Model { get { return _model; } }
    }
}


