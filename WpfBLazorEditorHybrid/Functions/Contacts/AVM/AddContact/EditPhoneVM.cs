using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;


namespace WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact
{
    public class EditPhoneVM : INotifyPropertyChanged
    {
        public event DeletePhoneHandler Notify_DeletePhone;
        public event UpdatePhoneHandler Notify_UpdatePhone;

        ContactPhone phone;
        public ContactPhone Phone { get { return phone; } }
        public EditPhoneVM(ContactPhone _phone)
        {
            phone = _phone;
            text = "";


            editText = new WCommand(o =>
            {
                Text = phone.Phone; 
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                phone.Phone = text;
                OnPropertyChanged("PhoneText");
                Text = "";
                Notify_UpdatePhone?.Invoke(this);
            });

            dislikeText = new WCommand(o =>
            {
                Notify_DeletePhone?.Invoke(this);
            });

        }

        string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }

        public string PhoneText
        {
            get { return phone.Phone; }
        }

        WCommand editText;
        public WCommand EditText { get { return editText; } }

        WCommand likeText;
        public WCommand LikeText { get { return likeText; } }


        WCommand dislikeText;
        public WCommand DislikeText { get { return dislikeText; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
