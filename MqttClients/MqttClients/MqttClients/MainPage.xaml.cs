using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace MqttClients
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            MqttClient mq1 = new MqttClient("farsala.ddns.net");

            try
            {
                byte code = mq1.Connect(Guid.NewGuid().ToString(), "evotsis", "eystbots");
                DisplayAlert("Alert", code.ToString(), "OK1");
                
            }
            catch (Exception)
            {
                DisplayAlert("Alert", "You have been alerted", "OK2");
            }
		}
	}
}
