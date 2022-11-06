using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalRClient
{ 
    public partial class MainWindow : Window
    {

        public String UserName { get; set; }
        public IHubProxy HubProxy { get; set; }
        const string ServerURI = "http://192.168.1.225:8080";
        public HubConnection Connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            Connection.Received += Connection_Recieved;
            HubProxy = Connection.CreateHubProxy("MyHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            //HubProxy.On<string, string>("MyHub", (name, message) => {
            //    Console.WriteLine(name + "---------------------------" + message);
            //} 
            //);
            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Unable to connect to server: Start server before connecting clients.");
                //StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI
                return;
            }

            //Show chat UI; hide login UI
            //SignInPanel.Visibility = Visibility.Collapsed;
            //ChatPanel.Visibility = Visibility.Visible;
            //ButtonSend.IsEnabled = true;
            //TextBoxMessage.Focus();
            //RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");
        }


        void Connection_Recieved(string a)
        {
            Console.WriteLine("Connection_Recieved *************************************"+a);
            ////Hide chat UI; show login UI
            //var dispatcher = Application.Current.Dispatcher;
            //dispatcher.Invoke(() => ChatPanel.Visibility = Visibility.Collapsed);
            //dispatcher.Invoke(() => ButtonSend.IsEnabled = false);
            //dispatcher.Invoke(() => StatusText.Content = "You have been disconnected.");
            //dispatcher.Invoke(() => SignInPanel.Visibility = Visibility.Visible);
        }

        void Connection_Closed()
        {
            Console.WriteLine("Connection_Closed *************************************");
            ////Hide chat UI; show login UI
            //var dispatcher = Application.Current.Dispatcher;
            //dispatcher.Invoke(() => ChatPanel.Visibility = Visibility.Collapsed);
            //dispatcher.Invoke(() => ButtonSend.IsEnabled = false);
            //dispatcher.Invoke(() => StatusText.Content = "You have been disconnected.");
            //dispatcher.Invoke(() => SignInPanel.Visibility = Visibility.Visible);
        }

        private void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            ConnectAsync();
        }
    }
}
