using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Models
{
    class Queen : Piece
    {
        public Queen()
        {
            SetMovementBehaviour(new SingleFoward());
        }
    }
}
