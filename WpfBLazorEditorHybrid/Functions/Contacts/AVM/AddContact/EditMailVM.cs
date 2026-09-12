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
    public class EditMailVM : INotifyPropertyChanged
    {
        public event DeleteMailHandler Notify_DeleteMail;
        public event UpdateMailHandler Notify_UpdateMail;

        ContactMail mail;
        public ContactMail Mail { get { return mail; } }
        public EditMailVM(ContactMail _mail)
        {
            mail = _mail;
            text = "";

            editText = new WCommand(o =>
            {
                Text = mail.Mail;
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                mail.Mail = text;
                OnPropertyChanged("MailText");
                Text = "";
                Notify_UpdateMail?.Invoke(this);
            });

            dislikeText = new WCommand(o =>
            {
                Notify_DeleteMail?.Invoke(this);
            });

        }

        string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }


        public string MailText
        {
            get { return mail.Mail; }
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
