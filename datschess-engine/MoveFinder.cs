using System;
using System.Collections.Generic;
using static DATSChess.MoveUtility;
using static DATSChess.Piece;

namespace DATSChess
{
    public class MoveFinder
    {
        public static List<Move> GetLegalMoves(Board board)
        {
            List<Move> legalMoves = new List<Move>();


            legalMoves.AddRange(GetPawnMoves(board));
            legalMoves.AddRange(GetBishopMoves(board));
            legalMoves.AddRange(GetKnightMoves(board));
            legalMoves.AddRange(GetRookMoves(board));
            legalMoves.AddRange(GetQueenMoves(board));
            legalMoves.AddRange(GetKingMoves(board));
            
            return FilterLegalMoves(board, legalMoves);
        }

        public static List<Move> FilterLegalMoves(Board board, List<Move> moveList)
        {
            List<Move> toReturn = new List<Move>();
            bool moveAllowsCheck;

            foreach (Move move in moveList)
            {
                moveAllowsCheck = PlayTestMove(board, move);
                if (!moveAllowsCheck)
                {
                    toReturn.Add(move);
                }
            }

            return toReturn;
        }

        #region Piece Moves

        public static List<Move> GetPawnMoves(Board board)
        {
            List<Move> pawnMoves = new List<Move>();

            if (board.colorToMove == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.WhitePawn)
                    {
                        // Forward moves
                        if (!board._state[i].hasMoved && board._state[i + 8]._type == PieceType.None && board._state[i + 16]._type == PieceType.None)
                        {
                            pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 16)));
                        }
                        if (!isUpperEdge(i) && board._state[i + 8]._type == PieceType.None)
                        {
                            if (isUpperEdge(i + 8))
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 8) + "Q"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 8) + "R"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 8) + "N"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 8) + "B"));
                            }
                            else
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 8)));
                            }
                        }
                        // Diagonal Captures
                        if (!isLeftEdge(i) && board._state[i + 7]._type != PieceType.None && !board._state[i + 7].isWhite())
                        {
                            if (isUpperEdge(i + 7))
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 7) + "Q"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 7) + "R"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 7) + "N"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 7) + "B"));
                            }
                            else
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 7)));
                            }
                        }
                        if (!isRightEdge(i) && board._state[i + 9]._type != PieceType.None && !board._state[i + 9].isWhite())
                        {
                            if (isUpperEdge(i + 8))
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 9) + "Q"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 9) + "R"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 9) + "N"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 9) + "B"));
                            }
                            else
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 9)));
                            }
                        }
                        // En passant
                        //if ()
                        //{

                        //}
                    }
                }
            }
            else if (board.colorToMove == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.BlackPawn)
                    {
                        // Forward moves
                        if (!board._state[i].hasMoved && board._state[i - 8]._type == PieceType.None && board._state[i - 16]._type == PieceType.None)
                        {
                            pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 16)));
                        }
                        if (!isLowerEdge(i) && board._state[i - 8]._type == PieceType.None)
                        {
                            if (isLowerEdge(i - 8))
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 8) + "q"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 8) + "r"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 8) + "n"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 8) + "b"));
                            }
                            else
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 8)));
                            }
                        }
                        // Diagonal Captures
                        if (!isRightEdge(i) && board._state[i - 7]._type != PieceType.None && board._state[i - 7].isWhite())
                        {
                            if (isLowerEdge(i - 8))
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 7) + "q"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 7) + "r"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 7) + "n"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 7) + "b"));
                            }
                            else
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 7)));
                            }
                        }
                        if (!isLeftEdge(i) && board._state[i - 9]._type != PieceType.None && board._state[i - 9].isWhite())
                        {
                            if (isLowerEdge(i - 8))
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 9) + "q"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 9) + "r"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 9) + "n"));
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 9) + "b"));
                            }
                            else
                            {
                                pawnMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 9)));
                            }
                        }
                        // En passant
                        //if ()
                        //{

                        //}
                    }
                }
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }

            return pawnMoves;
        }

        public static List<Move> GetBishopMoves(Board board)
        {
            List<Move> bishopMoves = new List<Move>();
            int currPos;
            bool stopSearch = false;

            if (board.colorToMove == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.WhiteBishop)
                    {
                        currPos = i;
                        // Search NE
                        while (!isUpperEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i += 9;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search NW
                        stopSearch = false;
                        currPos = i;
                        while (!isUpperEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i += 7;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SW
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i -= 9;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SE
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i -= 7;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                    }
                }
            }
            else if (board.colorToMove == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.BlackBishop)
                    {
                        currPos = i;
                        // Search NE
                        while (!isUpperEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i += 9;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search NW
                        stopSearch = false;
                        currPos = i;
                        while (!isUpperEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i += 7;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SW
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i -= 9;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SE
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i -= 7;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                bishopMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                    }
                }
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }

            return bishopMoves;
        }

        public static List<Move> GetKnightMoves(Board board)
        {
            List<Move> knightMoves = new List<Move>();
            int currPos;

            if (board.colorToMove == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.WhiteKnight)
                    {
                        currPos = i;

                        // At max, knight has 8 moves

                        // currPos + 15: Up 2 left 1
                        if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isLeftEdge(i) && !board._state[i + 15].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 15)));
                        }
                        // currPos + 17: Up 2 right 1
                        if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isRightEdge(i) && !board._state[i + 17].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 17)));
                        }
                        // currPos - 15: Down 2 right 1
                        if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isRightEdge(i) && !board._state[i - 15].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 15)));
                        }
                        // currPos - 17: Down 2 left 1
                        if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isLeftEdge(i) && !board._state[i - 17].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 17)));
                        }
                        // currPos - 6: Down 1 right 2
                        if (!isRightEdge(i) && !isRightEdge(i + 1) && !isLowerEdge(i) && !board._state[i - 6].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 6)));
                        }
                        // currPos - 10: Down 1 left 2
                        if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isLowerEdge(i) && !board._state[i - 10].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 10)));
                        }
                        // currPos + 6: Up 1 left 2
                        if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isUpperEdge(i) && !board._state[i + 6].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 6)));
                        }
                        // currPos + 10: Up 1 right 2
                        if (!isRightEdge(i) && !isRightEdge(i + 1) && !isUpperEdge(i) && !board._state[i + 10].isWhite())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 10)));
                        }
                    }
                }
            }
            else if (board.colorToMove == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.BlackKnight)
                    {
                        currPos = i;

                        // At max, knight has 8 moves

                        // currPos + 15: Up 2 left 1
                        if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isLeftEdge(i) && !board._state[i + 15].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 15)));
                        }
                        // currPos + 17: Up 2 right 1
                        if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isRightEdge(i) && !board._state[i + 17].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 17)));
                        }
                        // currPos - 15: Down 2 right 1
                        if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isRightEdge(i) && !board._state[i - 15].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 15)));
                        }
                        // currPos - 17: Down 2 left 1
                        if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isLeftEdge(i) && !board._state[i - 17].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 17)));
                        }
                        // currPos - 6: Down 1 right 2
                        if (!isRightEdge(i) && !isRightEdge(i + 1) && !isLowerEdge(i) && !board._state[i - 6].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 6)));
                        }
                        // currPos - 10: Down 1 left 2
                        if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isLowerEdge(i) && !board._state[i - 10].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i - 10)));
                        }
                        // currPos + 6: Up 1 left 2
                        if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isUpperEdge(i) && !board._state[i + 6].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 6)));
                        }
                        // currPos + 10: Up 1 right 2
                        if (!isRightEdge(i) && !isRightEdge(i + 1) && !isUpperEdge(i) && !board._state[i + 10].isBlack())
                        {
                            knightMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i + 10)));
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }

            return knightMoves;
        }

        public static List<Move> GetRookMoves(Board board)
        {
            List<Move> rookMoves = new List<Move>();
            int currPos;
            bool stopSearch = false;

            if (board.colorToMove == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.WhiteRook)
                    {
                        currPos = i;
                        // Search N
                        while (!isUpperEdge(i) && !stopSearch)
                        {
                            i += 8;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search E
                        stopSearch = false;
                        currPos = i;
                        while (!isRightEdge(i) && !stopSearch)
                        {
                            i += 1;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search S
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !stopSearch)
                        {
                            i -= 8;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search W
                        stopSearch = false;
                        currPos = i;
                        while (!isLeftEdge(i) && !stopSearch)
                        {
                            i -= 1;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                    }
                }
            }
            else if (board.colorToMove == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.BlackRook)
                    {
                        currPos = i;
                        // Search N
                        while (!isUpperEdge(i) && !stopSearch)
                        {
                            i += 8;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search E
                        stopSearch = false;
                        currPos = i;
                        while (!isRightEdge(i) && !stopSearch)
                        {
                            i += 1;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search S
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !stopSearch)
                        {
                            i -= 8;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search W
                        stopSearch = false;
                        currPos = i;
                        while (!isLeftEdge(i) && !stopSearch)
                        {
                            i -= 1;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                rookMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                    }
                }
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }

            return rookMoves;
        }

        public static List<Move> GetQueenMoves(Board board)
        {
            List<Move> queenMoves = new List<Move>();
            int currPos = 0;
            bool stopSearch = false;

            if (board.colorToMove == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.WhiteQueen)
                    {
                        #region Diagonal Moves
                        currPos = i;
                        // Search NE
                        while (!isUpperEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i += 9;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search NW
                        stopSearch = false;
                        currPos = i;
                        while (!isUpperEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i += 7;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SW
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i -= 9;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SE
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i -= 7;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        #endregion

                        #region Straight Moves
                        currPos = i;
                        // Search N
                        while (!isUpperEdge(i) && !stopSearch)
                        {
                            i += 8;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search E
                        stopSearch = false;
                        currPos = i;
                        while (!isRightEdge(i) && !stopSearch)
                        {
                            i += 1;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search S
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !stopSearch)
                        {
                            i -= 8;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search W
                        stopSearch = false;
                        currPos = i;
                        while (!isLeftEdge(i) && !stopSearch)
                        {
                            i -= 1;
                            if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        #endregion
                    }
                }
            }
            else if (board.colorToMove == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.BlackQueen)
                    {
                        #region Diagonal Moves
                        currPos = i;
                        // Search NE
                        while (!isUpperEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i += 9;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search NW
                        stopSearch = false;
                        currPos = i;
                        while (!isUpperEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i += 7;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SW
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isLeftEdge(i) && !stopSearch)
                        {
                            i -= 9;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        // Search SE
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !isRightEdge(i) && !stopSearch)
                        {
                            i -= 7;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        #endregion

                        #region Straight Moves
                        currPos = i;
                        // Search N
                        while (!isUpperEdge(i) && !stopSearch)
                        {
                            i += 8;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search E
                        stopSearch = false;
                        currPos = i;
                        while (!isRightEdge(i) && !stopSearch)
                        {
                            i += 1;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search S
                        stopSearch = false;
                        currPos = i;
                        while (!isLowerEdge(i) && !stopSearch)
                        {
                            i -= 8;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;

                        // Search W
                        stopSearch = false;
                        currPos = i;
                        while (!isLeftEdge(i) && !stopSearch)
                        {
                            i -= 1;
                            if (board._state[i].isPiece() && board._state[i].isWhite())
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                                stopSearch = true;
                            }
                            else if (board._state[i].isPiece() && !board._state[i].isWhite())
                            {
                                stopSearch = true;
                            }
                            else
                            {
                                queenMoves.Add(new Move(Notation.ToSquareName(currPos) + Notation.ToSquareName(i)));
                            }
                        }
                        i = currPos;
                        #endregion
                    }
                }
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }

            return queenMoves;
        }

        public static List<Move> GetKingMoves(Board board)
        {
            List<Move> kingMoves = new List<Move>();

            if (board.colorToMove == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.WhiteKing) 
                    {
                        if (!isUpperEdge(i) && !(i + 8 > 63) && !board._state[i + 8].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 8)));
                        }
                        if (!isRightEdge(i) && !(i + 1 > 63) && !board._state[i + 1].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 1)));
                        }
                        if (!isLowerEdge(i) && !(i - 8 < 0) && !board._state[i - 8].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 8)));
                        }
                        if (!isLeftEdge(i) && !(i - 1 < 0) && !board._state[i - 1].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 1)));
                        }
                        if (!isUpperEdge(i) && !isRightEdge(i) && !(i + 9 > 63) && !board._state[i + 9].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 9)));
                        }
                        if (!isLowerEdge(i) && !isRightEdge(i) && !(i - 7 < 0) && !board._state[i - 7].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 7)));
                        }
                        if (!isLowerEdge(i) && !isLeftEdge(i) && !(i - 9 < 0) && !board._state[i - 9].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 9)));
                        }
                        if (!isUpperEdge(i) && !isLeftEdge(i) && !(i + 7 > 63) && !board._state[i + 7].isWhite())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 7)));
                        }
                    }
                }
            }
            else if (board.colorToMove == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (board._state[i]._type == PieceType.BlackKing)
                    {
                        if (!isUpperEdge(i) && !(i + 8 > 63) && !board._state[i + 8].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 8)));
                        }
                        if (!isRightEdge(i) && !(i + 1 > 63) && !board._state[i + 1].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 1)));
                        }
                        if (!isLowerEdge(i) && !(i - 8 < 0) && !board._state[i - 8].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 8)));
                        }
                        if (!isLeftEdge(i) && !(i - 1 < 0) && !board._state[i - 1].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 1)));
                        }
                        if (!isUpperEdge(i) && !isRightEdge(i) && !(i + 9 > 63) && !board._state[i + 9].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 9)));
                        }
                        if (!isLowerEdge(i) && !isRightEdge(i) && !(i - 7 < 0) && !board._state[i - 7].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 7)));
                        }
                        if (!isLowerEdge(i) && !isLeftEdge(i) && !(i - 9 < 0) && !board._state[i - 9].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i - 9)));
                        }
                        if (!isUpperEdge(i) && !isLeftEdge(i) && !(i + 7 > 63) && !board._state[i + 7].isBlack())
                        {
                            kingMoves.Add(new Move(Notation.ToSquareName(i) + Notation.ToSquareName(i + 7)));
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }

            return kingMoves;
        }

        #endregion

        #region Helper Functions
        public static bool isUpperEdge(int index)
        {
            if (index >= 56 && index <= 63) // Top Rank
            {
                return true;
            }
            return false;
        }

        public static bool isLowerEdge(int index)
        {
            if (index >= 0 && index <= 7) // Bottom Rank
            {
                return true;
            }
            return false;
        }
        public static bool isLeftEdge(int index)
        {
            if (index % 8 == 0) // A File
            {
                return true;
            }
            return false;
        }
        public static bool isRightEdge(int index)
        {
            if ((index + 1) % 8 == 0) // H File 
            {
                return true;
            }
            return false;
        }

        public static bool PlayTestMove(Board board, Move move)
        {
            Board testBoard = new Board(board);
            testBoard.Play(move);
            if (testBoard.colorToMove == Color.White) 
            {
                testBoard.colorToMove = Color.Black;
            }
            else if (testBoard.colorToMove == Color.Black)
            {
                testBoard.colorToMove = Color.White;
            }
            return testBoard.IsChecked();
        }

        //public static void Print(Board board)
        //{
        //    Console.WriteLine("  A B C D E F G H");
        //    Console.WriteLine("  ---------------");
        //    for (int rank = 7; rank >= 0; rank--)
        //    {
        //        Console.Write($"{rank + 1}|");
        //        for (int file = 0; file < 8; file++)
        //        {
        //            Piece piece = board[rank, file];
        //            Print(piece);
        //            //Console.Write((rank * 8 + file) + " ");
        //        }
        //        Console.WriteLine();
        //    }
        //}

        //private static void Print(Piece piece)
        //{
        //    Console.Write(Notation.ToChar(piece._type));
        //    Console.Write(' ');
        //}
    }
    #endregion

}
