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
            MqttClient mq1 = new MqttClient("192.168.1.2");

            try
            {
                byte code = mq1.Connect(Guid.NewGuid().ToString(), "evotsis", "eystbots");
                DisplayAlert("Alert", code.ToString(), "OK1");
                ushort msgId = mq1.Publish("test", // topic
                              Encoding.UTF8.GetBytes("MyMessageBody"), // message body
                              MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                              true);

            }
            catch (Exception)
            {
                DisplayAlert("Alert", "You have been alerted", "OK2");
            }
		}
	}
}
