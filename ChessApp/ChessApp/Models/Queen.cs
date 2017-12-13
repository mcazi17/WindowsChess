using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ChessApp.Models
{
    class Queen : Piece
    {
        

        public Queen(string color)
        {
            Image = new Image();
            if (color == "w")
            {
                Image.Source = new BitmapImage(new Uri("ms-appx:///Assets/wqueen.png", UriKind.Absolute));
            }
            else {
                Image.Source = new BitmapImage(new Uri("ms-appx:///Assets/queen.png", UriKind.Absolute));
            }
            SetMovementBehaviour(new SingleFoward());
        }

    }
}
