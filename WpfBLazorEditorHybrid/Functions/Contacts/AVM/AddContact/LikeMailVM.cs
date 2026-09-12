using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact;
using TTClassLibrary.Functions.Contacts;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact
{
    public delegate void DeleteMailHandler(EditMailVM item);
    public delegate void UpdateMailHandler(EditMailVM item);
    public class LikeMailVM : INotifyPropertyChanged
    {

        protected ObservableCollection<EditMailVM> mails;
        public ObservableCollection<EditMailVM> Mails
        {
            get { return mails; }
            set { mails = value; OnPropertyChanged("Mails"); }
        }

        
        protected void DeleteMail(EditMailVM item) 
        {
            Mails.Remove(item);
        }


        protected ContactMail mail;
        

        public LikeMailVM()
        {
            mails = new ObservableCollection<EditMailVM>();

            mail = new ContactMail() 
            {
                Mail = ""
            };

            addMailPanel = new WCommand(o =>
            {
                mail = new ContactMail()
                {
                    Mail = ""
                };
            });

            likeText = new WCommand(o =>
            {
                if (text == null || text.Trim() == "")
                {
                    MessageBox.Show("Текст не заполнен!");
                    return;
                }

                mail.Mail = text;
                EditMailVM editMailVM = new EditMailVM(mail);
                editMailVM.Notify_DeleteMail += DeleteMail;
                mails.Add(new EditMailVM(mail));
                Text = "";
            });

        }

        protected string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }

        protected WCommand addMailPanel;
        public WCommand AddMailPanel { get { return addMailPanel; } }


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
