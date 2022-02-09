using System;
using static DATSChess.MoveUtility;

namespace DATSChess.Engines
{
    public class Randy
    {
        public delegate void MoveMadeCallback(string move);
        public static void Move(Board board, MoveMadeCallback MoveMade) {
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
            MoveMade(randomMove.ToString());
            //board.Play(randomMove);
        }
    }
}
