using System;
using static DATSChess.MoveUtility;

namespace DATSChess.Engines
{
    public class Randy
    {
        public static void Move(Board board) {
            Random rand = new Random();
            int index;
            Move randomMove;
            Move[] legalMoves;

            // Get Legal Moves
            legalMoves = MoveFinder.GetLegalMoves(board).ToArray();

            // Pick Random Move
            index = rand.Next(legalMoves.Length);
            randomMove = legalMoves[index];
            System.Threading.Thread.Sleep(250);

            UCI.BestMove(randomMove);
            //board.Play(randomMove);
        }
    }
}
