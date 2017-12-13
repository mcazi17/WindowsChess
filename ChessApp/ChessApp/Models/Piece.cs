using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Models
{
    class Piece
    {
        private MovementBehaviour movementBehaviour;

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
