using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Models
{
    class Bishop : Piece
    {
        public Bishop()
        {
            SetMovementBehaviour(new SingleFoward());
        }
    }
}
