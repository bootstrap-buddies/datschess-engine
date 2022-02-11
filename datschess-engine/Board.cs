using System;
using static DATSChess.MoveUtility;
using static DATSChess.Piece;
using static DATSChess.MoveFinder;

namespace DATSChess
{

    public class Piece
    {
        public PieceType _type;
        public bool hasMoved;

        public enum PieceType
        {
            None = 0,
            WhitePawn = 1,
            WhiteKnight = 2,
            WhiteBishop = 3,
            WhiteRook = 4,
            WhiteQueen = 5,
            WhiteKing = 6,
            BlackPawn = 7,
            BlackKnight = 8,
            BlackBishop = 9,
            BlackRook = 10,
            BlackQueen = 11,
            BlackKing = 12
        }

        public Piece(PieceType type)
        {
            _type = type;
            hasMoved = false;
        }

        public bool isWhite()
        {
            if (_type == PieceType.WhitePawn || _type == PieceType.WhiteKnight || _type == PieceType.WhiteBishop ||
                _type == PieceType.WhiteRook || _type == PieceType.WhiteQueen || _type == PieceType.WhiteKing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isBlack()
        {
            if (_type == PieceType.BlackPawn || _type == PieceType.BlackKnight || _type == PieceType.BlackBishop ||
                _type == PieceType.BlackRook || _type == PieceType.BlackQueen || _type == PieceType.BlackKing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isPiece()
        {
            if (_type == PieceType.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public enum Color
    {
        None = 0,
        White = 1,
        Black = 2
    }

    public class Move
    {
        public byte FromIndex;
        public byte ToIndex;
        public PieceType Promotion;

        public Move(string uciMoveNotation)
        {
            string fromSquare = uciMoveNotation.Substring(0, 2);
            string toSquare = uciMoveNotation.Substring(2, 2);
            FromIndex = Notation.ToSquare(fromSquare);
            ToIndex = Notation.ToSquare(toSquare);
            Promotion = (uciMoveNotation.Length == 5) ? Notation.ToPiece(uciMoveNotation[4]) : PieceType.None;
        }

        public override string ToString()
        {
            //result represents the move in the long algebraic notation (without piece names)
            string result = Notation.ToSquareName(FromIndex);
            result += Notation.ToSquareName(ToIndex);
            //the presence of a 5th character should mean promotion
            if (Promotion != PieceType.None)
                result += Notation.ToChar(Promotion);

            return result;
        }

        public void Adjust()
        {
            if (isUpperEdge(ToIndex))
            {
                if (Promotion != PieceType.None)
                {
                    Promotion = Notation.ToPiece(Notation.ToChar(Promotion).ToString().ToUpper());
                }
            }
        }
    }

    public class Board
    {
        public Piece[] _state = new Piece[64];
        public Color colorToMove = Color.None;

        public Board()
        {
            SetupBoard();
        }

        public Board(Board toCopy)
        {
            for (int i = 0; i < 64; i++)
            {
                _state[i] = toCopy._state[i];
            }
            colorToMove = toCopy.colorToMove;
        }

        public Piece this[int index]
        {
            get => _state[index];
            set => _state[index] = value;
        }

        public Piece this[int rank, int file]
        {
            get => _state[rank * 8 + file];
            set => _state[rank * 8 + file] = value;
        }

        public void Play(Move move)
        {
            Piece movingPiece = _state[move.FromIndex];
            if (move.Promotion != PieceType.None)
                movingPiece = new Piece(move.Promotion);

            _state[move.ToIndex] = movingPiece;

            _state[move.FromIndex] = new Piece(PieceType.None);

            movingPiece.hasMoved = true;

            if (colorToMove == Color.White)
            {
                colorToMove = Color.Black;
            }
            else if (colorToMove == Color.Black)
            {
                colorToMove = Color.White;
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }
        }

        public void SetupBoard()
        {
            //Starting position "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            //https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation

            string startingPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            colorToMove = Color.White;

            //Place pieces on board
            Array.Clear(_state, 0, 64);
            string[] positionArray = startingPosition.Split('/');
            int rank = 7;
            foreach (string row in positionArray)
            {
                int file = 0;
                foreach (char piece in row)
                {
                    if (char.IsNumber(piece))
                    {
                        int emptySquares = (int)char.GetNumericValue(piece);

                        for(int f = 0; f < emptySquares; f++)
                        {
                            _state[rank * 8 + f] = new Piece(PieceType.None);
                        }

                        file += emptySquares;
                    }
                    else
                    {
                        _state[rank * 8 + file] = new Piece(Notation.ToPiece(piece));
                        file++;
                    }
                }
                rank--;
            }

        }

        public bool IsChecked()
        {
            if (colorToMove == Color.White)
            {
                // Check Diagonals for Bishops and Queens
                if (diagonalCheck(Color.White))
                    return true;
                // Check Straights for Rooks and Queens
                if (straightCheck(Color.White))
                    return true;
                // Check Knight Jumps for Knights
                if (knightCheck(Color.White))
                    return true;
                // Check 1 Square Diagonal for Pawns
                if (pawnCheck(Color.White))
                    return true;
                // Check Surrounding Squares for King
                if (kingCheck(Color.White))
                    return true;
            }
            else if (colorToMove == Color.Black)
            {
                // Check Diagonals for Bishops and Queens
                if (diagonalCheck(Color.Black))
                    return true;
                // Check Straights for Rooks and Queens
                if (straightCheck(Color.Black))
                    return true;
                // Check Knight Jumps for Knights
                if (knightCheck(Color.Black))
                    return true;
                // Check 1 Square Diagonal for Pawns
                if (pawnCheck(Color.Black))
                    return true;
                // Check Surrounding Squares for King
                if (kingCheck(Color.Black))
                    return true;
            }
            else
            {
                throw new ArgumentException("colorToMove not set.");
            }
            
            return false;
        }

        #region Helper Functions
        private bool diagonalCheck(Color currColor)
        {
            int currPos;
            Piece currPiece;
            if (currColor == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    currPiece = _state[i];
                    currPos = i;

                    if (currPiece._type == PieceType.WhiteKing)
                    {
                        // Search NE
                        while (!isUpperEdge(i) && !isRightEdge(i))
                        {
                            i += 9;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackBishop || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search NW
                        currPos = i;
                        while (!isUpperEdge(i) && !isLeftEdge(i))
                        {
                            i += 7;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackBishop || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search SW
                        currPos = i;
                        while (!isLowerEdge(i) && !isLeftEdge(i))
                        {
                            i -= 9;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackBishop || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search SE
                        while (!isLowerEdge(i) && !isRightEdge(i))
                        {
                            i -= 7;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackBishop || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }

                        return false;
                    }
                }
            }
            else if (currColor == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    currPiece = _state[i];
                    currPos = i;

                    if (currPiece._type == PieceType.BlackKing)
                    {
                        // Search NE
                        while (!isUpperEdge(i) && !isRightEdge(i))
                        {
                            i += 9;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteBishop || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search NW
                        currPos = i;
                        while (!isUpperEdge(i) && !isLeftEdge(i))
                        {
                            i += 7;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteBishop || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;
                        currPiece = _state[i];

                        // Search SW
                        currPos = i;
                        while (!isLowerEdge(i) && !isLeftEdge(i))
                        {
                            i -= 9;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteBishop || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search SE
                        while (!isLowerEdge(i) && !isRightEdge(i))
                        {
                            i -= 7;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteBishop || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }

                        return false;
                    }
                }
            }

            return false;
        }

        private bool straightCheck(Color currColor)
        {
            int currPos;
            Piece currPiece;
            if (currColor == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    currPiece = _state[i];
                    currPos = i;

                    if (currPiece._type == PieceType.WhiteKing)
                    {
                        // Search N
                        while (!isUpperEdge(i))
                        {
                            i += 8;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackRook || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search E
                        currPos = i;
                        while (!isRightEdge(i))
                        {
                            i += 1;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackRook || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search S
                        currPos = i;
                        while (!isLowerEdge(i))
                        {
                            i -= 8;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackRook || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search W
                        while (!isLeftEdge(i))
                        {
                            i -= 1;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.BlackRook || currPiece._type == PieceType.BlackQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }

                        return false;
                    }
                }
            }
            else if (currColor == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    currPiece = _state[i];
                    currPos = i;

                    if (currPiece._type == PieceType.BlackKing)
                    {
                        // Search N
                        while (!isUpperEdge(i))
                        {
                            i += 8;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteRook || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search E
                        currPos = i;
                        while (!isRightEdge(i))
                        {
                            i += 1;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteRook || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;
                        currPiece = _state[i];

                        // Search S
                        currPos = i;
                        while (!isLowerEdge(i))
                        {
                            i -= 8;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteRook || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }
                        i = currPos;

                        // Search W
                        while (!isLeftEdge(i))
                        {
                            i -= 1;
                            currPiece = _state[i];
                            if (currPiece._type == PieceType.WhiteRook || currPiece._type == PieceType.WhiteQueen)
                            {
                                return true;
                            }
                            else if (currPiece.isBlack() || currPiece.isWhite())
                            {
                                break;
                            }
                        }

                        return false;
                    }
                }
            }

            return false;
        }

        private bool knightCheck(Color currColor)
        {
            if (currColor == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (_state[i]._type == PieceType.WhiteKing)
                    {
                        // currPos + 15: Up 2 left 1
                        if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isLeftEdge(i) && _state[i + 15]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        // currPos + 17: Up 2 right 1
                        else if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isRightEdge(i) && _state[i + 17]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        // currPos - 15: Down 2 right 1
                        else if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isRightEdge(i) && _state[i - 15]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        // currPos - 17: Down 2 left 1
                        else if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isLeftEdge(i) && _state[i - 17]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        // currPos - 6: Down 1 right 2
                        else if (!isRightEdge(i) && !isRightEdge(i + 1) && !isLowerEdge(i) && _state[i - 6]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        // currPos - 10: Down 1 left 2
                        else if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isLowerEdge(i) && _state[i - 10]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        // currPos + 6: Up 1 left 2
                        else if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isUpperEdge(i) && _state[i + 6]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        // currPos + 10: Up 1 right 2
                        else if (!isRightEdge(i) && !isRightEdge(i + 1) && !isUpperEdge(i) && _state[i + 10]._type == PieceType.BlackKnight)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else if (currColor == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (_state[i]._type == PieceType.BlackKing)
                    {
                        // currPos + 15: Up 2 left 1
                        if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isLeftEdge(i) && _state[i + 15]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        // currPos + 17: Up 2 right 1
                        else if (!isUpperEdge(i) && !isUpperEdge(i + 8) && !isRightEdge(i) && _state[i + 17]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        // currPos - 15: Down 2 right 1
                        else if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isRightEdge(i) && _state[i - 15]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        // currPos - 17: Down 2 left 1
                        else if (!isLowerEdge(i) && !isLowerEdge(i - 8) && !isLeftEdge(i) && _state[i - 17]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        // currPos - 6: Down 1 right 2
                        else if (!isRightEdge(i) && !isRightEdge(i + 1) && !isLowerEdge(i) && _state[i - 6]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        // currPos - 10: Down 1 left 2
                        else if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isLowerEdge(i) && _state[i - 10]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        // currPos + 6: Up 1 left 2
                        else if (!isLeftEdge(i) && !isLeftEdge(i - 1) && !isUpperEdge(i) && _state[i + 6]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        // currPos + 10: Up 1 right 2
                        else if (!isRightEdge(i) && !isRightEdge(i + 1) && !isUpperEdge(i) && _state[i + 10]._type == PieceType.WhiteKnight)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private bool pawnCheck(Color currColor)
        {
            if (currColor == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (_state[i]._type == PieceType.WhiteKing)
                    {
                        if (!isLeftEdge(i) && !isUpperEdge(i) && _state[i + 7]._type == PieceType.BlackPawn)
                        {
                            return true;
                        }
                        else if (!isRightEdge(i) && !isUpperEdge(i) && _state[i + 9]._type == PieceType.BlackPawn)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else if (currColor == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (_state[i]._type == PieceType.BlackKing)
                    {
                        if (!isLeftEdge(i) && !isLowerEdge(i) && _state[i - 9]._type == PieceType.WhitePawn)
                        {
                            return true;
                        }
                        else if (!isRightEdge(i) && !isLowerEdge(i) && _state[i - 7]._type == PieceType.WhitePawn)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private bool kingCheck(Color currColor)
        {
            if (currColor == Color.White)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (_state[i]._type == PieceType.WhiteKing)
                    {
                        if (!isUpperEdge(i) && _state[i + 8]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else if (!isRightEdge(i) && _state[i + 1]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else if (!isLowerEdge(i) && _state[i - 8]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else if (!isLeftEdge(i) && _state[i - 1]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else if (!isUpperEdge(i) && !isRightEdge(i) && _state[i + 9]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else if (!isUpperEdge(i) && !isLeftEdge(i) && _state[i + 7]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else if (!isLowerEdge(i) && !isRightEdge(i) && _state[i - 7]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else if (!isLowerEdge(i) && !isLeftEdge(i) && _state[i - 9]._type == PieceType.BlackKing)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else if (currColor == Color.Black)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (_state[i]._type == PieceType.BlackKing)
                    {
                        if (!isUpperEdge(i) && _state[i + 8]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else if (!isRightEdge(i) && _state[i + 1]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else if (!isLowerEdge(i) && _state[i - 8]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else if (!isLeftEdge(i) && _state[i - 1]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else if (!isUpperEdge(i) && !isRightEdge(i) && _state[i + 9]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else if (!isUpperEdge(i) && !isLeftEdge(i) && _state[i + 7]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else if (!isLowerEdge(i) && !isRightEdge(i) && _state[i - 7]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else if (!isLowerEdge(i) && !isLeftEdge(i) && _state[i - 9]._type == PieceType.WhiteKing)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

    }
}
