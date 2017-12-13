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
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Core;
using Windows.Devices.Sensors;

namespace ChessApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IHubProxy _hub;
        SolidColorBrush black = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 229, 229, 229));
        SolidColorBrush white = new SolidColorBrush(Windows.UI.Colors.White);

        Piece queenB, queenW;
        Piece kingB, kingW;
        Piece pawnB, pawnW;
        Piece rookB, rookW;
        Piece bishopB, bishopW;
        Piece knightB, knightW;
        LightSensor mylightSensor;   

        public MainPage()
        {
            this.InitializeComponent();
            ConfigureHub();
            CreatePieces();
            CreateGrid();
            StartLightSensor();
            
        }

        public async void StartLightSensor()
        {
            mylightSensor = LightSensor.GetDefault();
            mylightSensor.ReportInterval = 15;
            mylightSensor.ReadingChanged += MylightSensor_ReadingChanged;
        }

        private void MylightSensor_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            LightSensorReading read = args.Reading;
            string values = String.Format("Light Sensor Reading:\t{0}\t{1}", args.Reading.Timestamp.ToString(), args.Reading.IlluminanceInLux.ToString());
            txtLuxValue.Text = values;
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
                        tmpButton.Background = white;
                    }
                    else
                    {
                        tmpButton.Background = black;
                    }

                    switch (y)
                    {
                        case 0:
                            switch (x)
                            {
                                case 0:
                                case 7:
                                    tmpButton.Content = new Rook("w").Image;
                                    break;
                                case 1:
                                case 6:
                                    tmpButton.Content = new Knight("w").Image;
                                    break;
                                case 2:
                                case 5:
                                    tmpButton.Content = new Bishop("w").Image;
                                    break;
                                case 3:
                                    tmpButton.Content = new Queen("w").Image;
                                    break;
                                case 4:
                                    tmpButton.Content = new King("w").Image;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 1:
                            tmpButton.Content = new Pawn("w").Image;
                            break;
                        case 6:
                            tmpButton.Content = new Pawn("b").Image;
                            break;
                        case 7:
                            switch (x)
                            {
                                case 0:
                                case 7:
                                    tmpButton.Content = new Rook("b").Image;
                                    break;
                                case 1:
                                case 6:
                                    tmpButton.Content = new Knight("b").Image;
                                    break;
                                case 2:
                                case 5:
                                    tmpButton.Content = new Bishop("b").Image;
                                    break;
                                case 3:
                                    tmpButton.Content = new Queen("b").Image;
                                    break;
                                case 4:
                                    tmpButton.Content = new King("b").Image;
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }

                    

                    Grid.SetColumn(tmpButton, x);
                    Grid.SetRow(tmpButton, y);
                    
                    this.board.Children.Add(tmpButton);
                }
            }            
        }

        private void OpenCamera(object sender, RoutedEventArgs e)
        {
            TakePicture();
        }


        public async void TakePicture()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(120, 120);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                return;
            }

            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
            BitmapPixelFormat.Bgra8, 
            BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            ProfilePic.Source = bitmapSource;
        }

        private void Send(object sender, RoutedEventArgs e)
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

        private void CommandBar_Opening(object sender, object e)
        {
            CommandBar cb = sender as CommandBar;
            if (cb != null) cb.Background.Opacity = 1.0;
        }

        private void CommandBar_Closing(object sender, object e)
        {
            CommandBar cb = sender as CommandBar;
            if (cb != null) cb.Background.Opacity = 0.5;
        }
    }
}
