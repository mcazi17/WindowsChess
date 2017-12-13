using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Models
{
    class Knight : Piece
    {
        public Knight()
        {
            SetMovementBehaviour(new SingleFoward());
        }
    }
}
