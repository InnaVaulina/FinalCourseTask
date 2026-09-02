using System;
using System.Collections.Generic;
using System.Windows.Controls;
using WpfBLazorHybridClient.Functions.Work.AVM;

namespace WpfBLazorHybridClient.Functions.Work.Control
{
    /// <summary>
    /// Логика взаимодействия для UC_WorkMenu.xaml
    /// </summary>
    public partial class UC_WorkMenu : UserControl
    {
        
        public UC_WorkMenu(WorkTableVM _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
            endDate.DisplayDateEnd = DateTime.Today;           
        }


        WorkTableVM model;
        public WorkTableVM Model { get { return model; } }

       
    }
}
