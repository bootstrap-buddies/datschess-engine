using DATSChess.Engines;
using System;
using System.IO;
using System.IO.Pipes;
using static DATSChess.Engines.Randy;
using static DATSChess.MoveUtility;

namespace DATSChess
{
    class Program
    {
        public static Board _board = new Board();
        static string pipeWriteHandle;
        static string pipeReadHandle;
        private static void Main(string[] args)
        {
            if (args != null && args.Length >= 2)
            {
                // Get read and write pipe handles
                // Note: Roles are now reversed from how the other process is passing the handles in
                pipeWriteHandle = args[0];
                pipeReadHandle = args[1];

            }

            // This if statement checks if the program is ran from web socket process
            // If these fields are null, then simply run the logic as normal
            // Else setup the pipe connection to receive any incoming messages
            if (pipeWriteHandle == null || pipeReadHandle == null)
            {
                byte[] inputBuffer = new byte[1024];
                Stream inputStream = Console.OpenStandardInput(inputBuffer.Length);
                Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
                while (true)
                {
                    string[] tokens = Console.ReadLine().Trim().Split();
                    switch (tokens[0])
                    {
                        case "uci":
                            Console.WriteLine("uciok");
                            break;
                        case "isready":
                            //_board.SetupBoard();
                            Console.WriteLine("readyok");
                            break;
                        case "position":
                            UciPosition(tokens);
                            Print(_board);
                            break;
                        case "go":
                            UciGo(tokens);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                setupPipeListener();
            }
        }

        private static void UciPosition(string[] tokens)
        {
            int firstMove = Array.IndexOf(tokens, "moves") + 1;
            if (firstMove == 0)
            {
                return;
            }

            _board.SetupBoard();

            for (int i = firstMove; i < tokens.Length; i++)
            {
                Move move = new Move(tokens[i]);
                move.Adjust();
                _board.Play(move);
            }
        }

        private static void UciGo(string[] tokens)
        {
            MoveMadeCallback callback = MoveMade;
            // Call Engine to make move.
            Randy.Move(_board, callback);
        }

        // Callback function that receives a move to this class
        public static void MoveMade(string move)
        {
            if (pipeWriteHandle != null)
            {
                sendMessage(pipeWriteHandle, move);
            }
        }

        public static void Print(Board board)
        {
            Console.WriteLine("  A B C D E F G H");
            Console.WriteLine("  ---------------");

            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank + 1}|");
                for (int file = 0; file < 8; file++)
                {
                    Piece piece = board[rank, file];
                    Print(piece);
                    //Console.Write((rank * 8 + file) + " ");
                }
                Console.WriteLine();
            }
        }

        private static void Print(Piece piece)
        {
            Console.Write(Notation.ToChar(piece._type));
            Console.Write(' ');
        }

        public static void setupPipeListener()
        {
            Console.WriteLine("[CLIENT]:" + " Starting pipe listener...");
            using (var pipeRead = new AnonymousPipeClientStream(PipeDirection.In, pipeReadHandle))
            {
                try
                {

                    // Get message from other process
                    using (var sr = new StreamReader(pipeRead))
                    {
                        string temp;

                        // Wait for 'sync message' from the other process
                        do
                        {
                            temp = sr.ReadLine();
                        } while (temp == null || !temp.StartsWith("SYNC"));

                        // Read until 'end message' from the server
                        while ((temp = sr.ReadLine()) != null && !temp.StartsWith("END"))
                        {
                            string[] tokens = temp.Trim().Split();
                            switch (tokens[0])
                            {
                                case "position":
                                    UciPosition(tokens);
                                    break;
                                case "go":
                                    UciGo(tokens);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO Exception handling/logging
                    throw;
                }
            }
        }

        public static void sendMessage(string pipeWriteHandle, string msg)
        {
            using (var pipeWrite = new AnonymousPipeClientStream(PipeDirection.Out, pipeWriteHandle))
            {
                try
                {
                    // Send value to calling process
                    using (var sw = new StreamWriter(pipeWrite))
                    {
                        sw.AutoFlush = true;
                        // Send a 'sync message' and wait for the calling process to receive it
                        sw.WriteLine("SYNC");
                        pipeWrite.WaitForPipeDrain();

                        sw.WriteLine(msg);
                        sw.WriteLine("END");
                    }
                }
                catch (Exception ex)
                {
                    //TODO Exception handling/logging
                    throw;
                }
            }
        }
    }
}
