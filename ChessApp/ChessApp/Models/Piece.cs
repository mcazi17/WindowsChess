using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ChessApp.Models
{
    class Piece
    {
        private MovementBehaviour movementBehaviour;
        public Image Image { get; set; }

        string move()
        {
            return movementBehaviour.move();
        }

        public void SetMovementBehaviour (MovementBehaviour movement)
        {
            this.movementBehaviour = movement;
        }

    }
}
