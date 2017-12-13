using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ChessApp.Models
{
    class Knight : Piece
    {
        public Knight(string color)
        {
            Image = new Image();
            if (color == "w")
            {
                Image.Source = new BitmapImage(new Uri("ms-appx:///Assets/wknight.png", UriKind.Absolute));
            }
            else
            {
                Image.Source = new BitmapImage(new Uri("ms-appx:///Assets/knight.png", UriKind.Absolute));
            }
            SetMovementBehaviour(new SingleFoward());
        }
    }
}
