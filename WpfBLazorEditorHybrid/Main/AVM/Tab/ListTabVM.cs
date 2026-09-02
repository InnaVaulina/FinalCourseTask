using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfBLazorHybridClient.Main.AVM.Tab
{
    public class ListTabVM : INotifyPropertyChanged
    {
        public ListTabVM() 
        {
            list = new ObservableCollection<TabVM>();
        }


        ObservableCollection<TabVM> list;
        public ObservableCollection<TabVM> TabViewModel { get { return list; } }

        int index;
        public int SelectedIndex
        {
            get { return index; }
            set { index = value; OnPropertyChanged("SelectedIndex"); }
        }


        public void TabClose(TabVM tab)
        {
            int index = SelectedIndex;
            TabViewModel.Remove(tab);
            SelectedIndex = TabViewModel.Count > index ? index : TabViewModel.Count - 1;
        }

        public void TabAdd(string header, UserControl content)
        {
            TabVM page = new TabVM() { Header = header, Content = content };
            page.Notify_close += TabClose;
            TabViewModel.Add(page);
            SelectedIndex = TabViewModel.Count - 1;
        }

        public void TabAdd(TabVM tab)
        {
            TabVM page = tab;
            page.Notify_close += TabClose;
            TabViewModel.Add(page);
            SelectedIndex = TabViewModel.Count - 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
