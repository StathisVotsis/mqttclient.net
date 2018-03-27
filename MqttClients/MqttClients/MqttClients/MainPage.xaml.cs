using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttClients
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            MqttClient mq1 = new MqttClient("farsala.ddns.net");

            
            byte code = mq1.Connect(Guid.NewGuid().ToString(), "evotsis", "eystbots");
            if (code.ToString() == "0")
            {
                DisplayAlert("You are connected to broker ", "farsala.ddns.net", "OK");
            }
            else
            {
                DisplayAlert("Cannot connect to broker", "Check internet connection", "OK");
            }

            //ushort msgId = mq1.Publish("test", // topic
            //Encoding.UTF8.GetBytes("MyMessageBody"), // message body
            //MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
            //true);

            ushort msgId = mq1.Subscribe(new string[] {"kg"},
                   new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            ushort msgId2 = mq1.Subscribe(new string[] { "temp" },
                   new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });


            mq1.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //Debug.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
            string st1 = Encoding.UTF8.GetString(e.Message);
            string st2 = e.Topic;
            kg.Text = st1;
            DisplayAlert(st1,st2, "OK");
        }
    }
}
