using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MqttClients.ViewModels 
{
    public class MainZigaria : INotifyPropertyChanged
    {
        MqttClient mq1;

        private string _message;

        private string _message2;

        private string _message3;

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public string Message2
        {
            get { return _message2; }
            set { _message2 = value; OnPropertyChanged(); }
        }

        public string Message3
        {
            get { return _message3; }
            set { _message3 = value; OnPropertyChanged(); }
        }

        public Command ConnectBroker
        {
            get
            {
                return new Command(() =>
                {
                    mq1 = new MqttClient("your server");
                    byte code = mq1.Connect(Guid.NewGuid().ToString(), "user", "pass");
                    ushort msgId = mq1.Subscribe(new string[] { "zigaria" },
                    new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    if (code.ToString() == "0")
                    {
                        Message = "Connected";
                    }
                    else
                    {
                        Message = "Cannot connect";
                    }
                    mq1.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                });
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainZigaria()
        {
           
            
        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string st1 = Encoding.UTF8.GetString(e.Message);
            //Message2 = st1;
            char[] charArr = st1.ToCharArray();
            char[] thisChar1 = {' ', ' ', ' ', ' ',' '};
            char[] thisChar2 = { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
            for (int i=0;i<=4;i++)
            {
                thisChar1[i] = charArr[i];
            }
            for (int i = 5; i <8; i++)
            {
                thisChar2[i] = charArr[i];
            }
            Message2 = new string(thisChar1);
            Message3 = new string(thisChar2);

            //string st2 = e.Topic;
        }
    }
}
