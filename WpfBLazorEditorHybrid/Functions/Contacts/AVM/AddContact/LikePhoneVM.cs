using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Functions.Contacts.Control;
using TTClassLibrary.Functions.Contacts;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact
{
    public delegate void DeletePhoneHandler(EditPhoneVM item);
    public delegate void UpdatePhoneHandler(EditPhoneVM item);
    public class LikePhoneVM : INotifyPropertyChanged
    {
        protected ObservableCollection<EditPhoneVM> phones;
        public ObservableCollection<EditPhoneVM> Phones
        {
            get { return phones; }
            set { phones = value; OnPropertyChanged("Phones"); }
        }

       
        protected void DeletePhone(EditPhoneVM item) 
        {
            Phones.Remove(item);
        }


        protected ContactPhone phone;
        public LikePhoneVM()
        {
            phones = new ObservableCollection<EditPhoneVM>();

            phone = new ContactPhone() 
            {
                Phone = ""
            };

            addPhonePanel = new WCommand(o =>
            {
                phone = new ContactPhone()
                {
                    Phone = ""
                };
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                phone.Phone = text;
                EditPhoneVM editPhoneVM = new EditPhoneVM(phone);
                editPhoneVM.Notify_DeletePhone += DeletePhone;
                phones.Add(editPhoneVM);
                Text = "";
            });

        }

        protected string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }


        protected WCommand addPhonePanel;
        public WCommand AddPhonePanel { get { return addPhonePanel; } }

        protected WCommand likeText;
        public WCommand LikeText { get { return likeText; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
