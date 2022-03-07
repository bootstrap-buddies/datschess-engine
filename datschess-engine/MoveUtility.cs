using System;
using System.Collections.Generic;
using static DATSChess.Piece;

namespace DATSChess
{
    public class MoveUtility
    {
        public static class Notation
        {
            public static PieceType ToPiece(char letter)
            {
                switch (letter)
                {
                    case 'P':
                        return PieceType.WhitePawn;
                    case 'N':
                        return PieceType.WhiteKnight;
                    case 'B':
                        return PieceType.WhiteBishop;
                    case 'R':
                        return PieceType.WhiteRook;
                    case 'Q':
                        return PieceType.WhiteQueen;
                    case 'K':
                        return PieceType.WhiteKing;
                    case 'p':
                        return PieceType.BlackPawn;
                    case 'n':
                        return PieceType.BlackKnight;
                    case 'b':
                        return PieceType.BlackBishop;
                    case 'r':
                        return PieceType.BlackRook;
                    case 'q':
                        return PieceType.BlackQueen;
                    case 'k':
                        return PieceType.BlackKing;
                    default:
                        throw new ArgumentException($"Piece letter {letter} not supported.");
                }
            }

            public static PieceType ToPiece(string letter)
            {
                if (letter.Length == 1)
                {
                    return ToPiece(letter.ToCharArray()[0]);
                }
                else
                {
                    throw new ArgumentException("Letter is too long.");
                }
            }

            public static char ToChar(PieceType piece)
            {
                switch (piece)

                {
                    case PieceType.WhitePawn:
                        return 'P';
                    case PieceType.WhiteKnight:
                        return 'N';
                    case PieceType.WhiteBishop:
                        return 'B';
                    case PieceType.WhiteRook:
                        return 'R';
                    case PieceType.WhiteQueen:
                        return 'Q';
                    case PieceType.WhiteKing:
                        return 'K';
                    case PieceType.BlackPawn:
                        return 'p';
                    case PieceType.BlackKnight:
                        return 'n';
                    case PieceType.BlackBishop:
                        return 'b';
                    case PieceType.BlackRook:
                        return 'r';
                    case PieceType.BlackQueen:
                        return 'q';
                    case PieceType.BlackKing:
                        return 'k';
                    case PieceType.None:
                        return ' ';
                    default:
                        throw new ArgumentException($"Piece {piece} not supported.");
                }
            }

            public static byte ToSquare(string squareNotation)
            {

                //Map letters [a..h] to [0..7] with ASCII('a') == 97
                int file = squareNotation[0] - 'a';
                //Map numbers [1..8] to [0..7] with ASCII('1') == 49
                int rank = squareNotation[1] - '1';
                int index = rank * 8 + file;

                if (index >= 0 && index <= 63)
                    return (byte)index;

                throw new ArgumentException($"The given square notation {squareNotation} does not map to a valid index between 0 and 63");
            }

            public static string ToSquareName(int squareIndex)
            {
                //This is the reverse of the ToSquareIndex()
                int rank = squareIndex / 8;
                int file = squareIndex % 8;

                //Map file [0..7] to letters [a..h] and rank [0..7] to [1..8]
                string squareNotation = $"{(char)('a' + file)}{rank + 1}";
                return squareNotation;
            }
        }

        public static class UCI
        {
            public static void BestMove(Move move)
            {
                Console.WriteLine($"bestmove {move}");
            }

            public static void BestMove(string move)
            {
                Console.WriteLine($"bestmove {move}");
            }
        }

    }
}
