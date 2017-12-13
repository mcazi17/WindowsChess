using ChessApp.Models;
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
using Windows.UI.Xaml.Media.Imaging;
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
        SolidColorBrush black = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 134, 187, 215));
        SolidColorBrush white = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 148, 147, 145));

        Piece queenB, queenW;
        Piece kingB, kingW;
        Piece pawnB, pawnW;
        Piece rookB, rookW;
        Piece bishopB, bishopW;
        Piece knightB, knightW;

        public MainPage()
        {
            this.InitializeComponent();
            ConfigureHub();
            CreatePieces();
            CreateGrid();
        }

       public void CreatePieces()
        {
            queenB = new Queen("b");
            queenW = new Queen("w");
            kingW = new King("w");
            kingB = new King("b");
            pawnW = new Pawn("w");
            pawnB = new Pawn("b");
            rookW = new Rook("w");
            rookB = new Rook("b");
            knightW = new Knight("w");
            knightB = new Knight("b");
            bishopW = new Bishop("w");
            bishopB = new Bishop("b");
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

                    if(x % 2 == 0 && y % 2 == 0 || x % 2 != 0 && y % 2 != 0)
                    {
                        tmpButton.Background = black;
                        if (y == 1)
                        {
                                tmpButton.Content = new Pawn("w").Image;
                        }
                    }
                    else
                    {
                        tmpButton.Background = white;
                        switch (y)
                        {
                            case 1:
                                tmpButton.Content = new Pawn("w").Image;
                                break;
                            case 6:
                                tmpButton.Content = new Pawn("b").Image;
                                break;
                            default:
                                break;

                        }
                    }

                    if (x == 0 & y == 0 || x == 7 & y == 0)
                    {
                        tmpButton.Content = new Rook("w").Image;
                    }else if (x == 0 & y == 7 || x == 7 & y == 7)
                    {
                        tmpButton.Content = new Rook("b").Image;
                    }


                    Grid.SetColumn(tmpButton, x);
                    Grid.SetRow(tmpButton, y);
                    
                    this.board.Children.Add(tmpButton);
                }
            }            
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
