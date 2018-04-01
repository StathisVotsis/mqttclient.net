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

        public Command ConnectBroker
        {
            get
            {
                return new Command(() =>
                {
                    mq1 = new MqttClient("192.168.1.2");
                    byte code = mq1.Connect(Guid.NewGuid().ToString(), "evotsis", "eystbots");
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
            Message2 = st1;
            string st2 = e.Topic;
        }
    }
}
