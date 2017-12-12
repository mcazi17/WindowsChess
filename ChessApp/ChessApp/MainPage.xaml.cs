using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ChessApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IHubProxy _hub;

        public MainPage()
        {
            this.InitializeComponent();
            ConfigureHub();
            CreateGrid();
        }

        private void CreateGrid()
        {
            int ButtonWidth = 50;
            int ButtonHeight = 50;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Button tmpButton = new Button
                    {
                        Width = ButtonWidth,
                        Height = ButtonHeight
                    };

                    if(x % 2 == 0 && y % 2 == 0)
                    {
                        tmpButton.Foreground = 
                    }

                    Grid.SetColumn(tmpButton, y);
                    Grid.SetRow(tmpButton, x);
                    this.board.Children.Add(tmpButton);
                }
            }

            //Button button = new Button
            //{
            //    Height = ButtonHeight,
            //    Width = ButtonWidth
            //};

            //for(int x = 1; x <= 64; x++)
            //{
            //    board.Children.Add(button);
            //}
            
        }

        //Hello world button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(TextBoxMessage.Text.ToString());
        }

        //Send message to signal R service
        private void SendMessage(string msg)
        {
            _hub.Invoke("SendAction", msg);
        }


        //Signal R connection
        public void ConfigureHub()
        {
            string url = @"http://xpesdcompany-001-site3.ctempurl.com/apis/ChessNotificationsService/signalr/hubs";
            var connection = new HubConnection(url);
            _hub = connection.CreateHubProxy("chesshub");

            _hub.On("ActionRequested", result =>
            {
                var x = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    lstMessages.Items.Add(result);
                    lstMessages.ScrollIntoView(lstMessages.Items[lstMessages.Items.Count - 1]);
                    
                });
            });
            connection.Start().Wait();
        }
    }
}
