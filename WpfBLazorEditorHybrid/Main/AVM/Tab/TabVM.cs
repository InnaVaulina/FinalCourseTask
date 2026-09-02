using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfBLazorHybridClient.Command;

namespace WpfBLazorHybridClient.Main.AVM.Tab
{

    public delegate void TabCloseHandler(TabVM tab);
    public delegate void TabAddHandler(TabVM tab);

    public class TabVM :INotifyPropertyChanged
    {
        string header;
        UserControl content;
        public event TabCloseHandler Notify_close;
        

        public TabVM() 
        {
            closeTab = new WCommand(o => { ClosePage(this); });
        }


        public string Header { get { return header; } set { header = value; OnPropertyChanged("Header"); } }

        public UserControl Content { get { return content; } set { content = value; OnPropertyChanged("Content"); } }


        WCommand closeTab;
        public WCommand CloseTab { get { return closeTab; } }

       
        public void ClosePage(TabVM page) 
        {
            Notify_close?.Invoke(page);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
