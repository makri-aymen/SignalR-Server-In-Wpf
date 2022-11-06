using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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

namespace SignalRHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public IDisposable SignalR { get; set; }
        const string ServerURI = "http://192.168.1.225:8080/";
        System.Timers.Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => StartServer());
            timer = new System.Timers.Timer(3000);
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        { 
            this.Dispatcher.Invoke(() => {
                //MyHub myHub = new MyHub();
                //myHub.Send("test message *******", "message --------"); 
                var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                context.Clients.All.Send( "refresh");
            });
            Console.WriteLine(" Event  : {0} ", e.SignalTime);
        }

        private void Connect_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Calls the StartServer method with Task.Run to not
        /// block the UI thread. 
        /// </summary>
        //private void ButtonStart_Click(object sender, RoutedEventArgs e)
        //{ 
        //    Task.Run(() => StartServer());
        //}

        /// <summary>
        /// Stops the server and closes the form. Restart functionality omitted
        /// for clarity.
        /// </summary>
        //private void ButtonStop_Click(object sender, RoutedEventArgs e)
        //{
        //    SignalR.Dispose();
        //    Close();
        //}

        /// <summary>
        /// Starts the server and checks for error thrown when another server is already 
        /// running. This method is called asynchronously from Button_Start.
        /// </summary>
        private void StartServer()
        {
            //try
            //{
            //this olution didnt work for me
            //HttpListener httpListener = new HttpListener();
            //httpListener.Prefixes.Clear();
            //httpListener.Prefixes.Add(ServerURI);

            //string strCmdText;
            //strCmdText = @"netsh http add urlacl url=http://+:8080/MyUri user=DOMAIN\user";
            //System.Diagnostics.Process.Start("CMD.exe", strCmdText);

            SignalR = WebApp.Start(ServerURI);
            //}
            //catch (TargetInvocationException)
            //{
            //    Console.WriteLine("A server is already running at " + ServerURI);
            //    //WriteToConsole("A server is already running at " + ServerURI);
            //    //this.Dispatcher.Invoke(() => ButtonStart.IsEnabled = true);
            //    return;
            //}
            Console.WriteLine("Server started at " + ServerURI);
            //this.Dispatcher.Invoke(() => ButtonStop.IsEnabled = true);
            //WriteToConsole("Server started at " + ServerURI);
        }
        ///This method adds a line to the RichTextBoxConsole control, using Dispatcher.Invoke if used
        /// from a SignalR hub thread rather than the UI thread.
        //public void WriteToConsole(String message)
        //{
        //    if (!(RichTextBoxConsole.CheckAccess()))
        //    {
        //        this.Dispatcher.Invoke(() =>
        //            WriteToConsole(message)
        //        );
        //        return;
        //    }
        //    RichTextBoxConsole.AppendText(message + "\r");
        //}
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.MapSignalR(); 
        }
    }

    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        public override Task OnConnected()
        { 
            Console.WriteLine("One client is connected ++++++++++++++++");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        { 
            Console.WriteLine("One client is deconnected ------------" );
            return base.OnDisconnected(stopCalled);
        }
    }
} 
