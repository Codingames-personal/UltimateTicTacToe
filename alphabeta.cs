using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

class Game
{
    private const int MAX_DEPTH = 4;
    private const int SELF_ID = 1;
    private const int OTHER_ID = -1;
    private const int FULL_ID = 2;
    private static string[] inputs;
    public static int[,] boards = new int[9, 9];
    public static int[,] uboard = new int[3, 3];
    public static int[,] moves  = new int[81, 2];
    public static int validActionCount;
    // Last move lx ly
    public static int lx;
    public static int ly;
    static void Main(string[] args)
    {
        int iteration = 0;
        int x, y, ux, uy;
        int maxDepth=Game.MAX_DEPTH;
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                boards[i, j] = 0;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                uboard[i, j] = 0;
        Game.getInput();
        if ((lx, ly) != (-1, -1))
            boards[lx, ly] = Game.OTHER_ID;

        // game loop
        while (true)
        {
            // MY TURN
            
            (x, y) = Algo.AlphaBeta(uboard, boards, moves, validActionCount, maxDepth, iteration);
            (ux, uy) = (x / 3, y / 3);
            // Update boards
            boards[x, y] = Game.SELF_ID;
            uboard[ux, uy] = Board.CheckWinFromMoveBoards(boards, x, y);
            if (uboard[ux, uy] == 0 & Board.checkFull(boards, ux, uy))
                uboard[ux, uy] = Game.FULL_ID;

            Console.WriteLine($"{x} {y}");
            
            Game.getInput();            
            (ux, uy) = (lx / 3, ly / 3);
            boards[lx, ly] = Game.OTHER_ID;
            uboard[ux, uy] = Board.CheckWinFromMoveBoards(boards, lx, ly);
            if (uboard[ux, uy] == 0 & Board.checkFull(boards, ux, uy))
                uboard[ux, uy] = Game.FULL_ID;
            iteration++;            

        }
    }

    public static void getInput()
    {
        // Get the input from codingame
        inputs = Console.ReadLine().Split(' ');
        lx = int.Parse(inputs[0]);
        ly = int.Parse(inputs[1]);
        validActionCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < validActionCount; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            moves[i, 0] = int.Parse(inputs[0]);
            moves[i, 1] = int.Parse(inputs[1]);
        }
    }

}

class Board
{   
    public static bool[,] mask = new bool[9, 9];


    public static void WriteBoard(int[,] board)
    {
        for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    Console.Error.Write($"{board[i, j]} ");
                Console.Error.WriteLine();  // Passe à la ligne suivante après chaque ligne du tableau
            }
    }

    public static IEnumerable<(int x, int y)> GetMoves(int[,] uboard, int[,] boards, int lx, int ly)
    {
        int ux = lx % 3;
        int uy = ly % 3;

        if (uboard[ux, uy] == 0)
        {
            for (int i = ux*3; i < 3*(ux+1); i++)
                for (int j = uy*3; j < 3*(uy+1); j++)
                    if (boards[i, j] == 0)
                    
                        yield return (i, j);
                
        }
        else
        {
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    if (uboard[x, y] == 0)
                        for (int i = x*3; i < 3*(x+1); i++)
                            for (int j = y*3; j < 3*(y+1); j++)
                                if (boards[i, j] == 0)
                                    yield return (i, j);
        }
    }

    public static int CountAvailableCell(int[,] uboard, int[,] boards)
    {
        int c = 0;
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                if (uboard[x, y] == 0)
                    for (int i = x*3; i < 3*(x+1); i++)
                        for (int j = y*3; j < 3*(y+1); j++)
                            if (boards[i, j] == 0)
                                c++;
                    
        return c;
    }

    public static bool checkFull(int[,] boards, int ux, int uy)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (boards[3*ux+i, 3*uy+j] == 0)
                    return false;
        return true;
    }


    public static int CheckWinFromMove(int[,] board, int x, int y)
    {
        // Get the value of the cell where the move was made
        int value = board[x, y];

        // Check the row
        if (board[x, 0] == board[x, 1] && board[x, 1] == board[x, 2])
        {
            return value;
        }
        // Check the column
        if (board[0, y] == board[1, y] && board[1, y] == board[2, y])
        {
            return value;
        }
        // Check the main diagonal
        if (x == y && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            return value;
        }
        // Check the anti-diagonal
        if (x + y == 2 && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            return value;
        }
        // If no win condition is met, return 0
        return 0;
    }
    public static int CheckWinFromMoveBoards(int[,] boards, int x, int y)
    {
        // Get the value of the cell where the move was made
        int value = boards[x, y];
        int x_ = 3 * (x / 3);
        int y_ = 3 * (y / 3);

        // Check the row
        if (boards[x, y_] == boards[x, y_ + 1] && boards[x, y_ + 1] == boards[x, y_ + 2])
        {
            return value;
        }
        // Check the column
        if (boards[x_, y] == boards[x_ + 1, y] && boards[x_ + 1, y] == boards[x_ + 2, y])
        {
            return value;
        }
        // Check the main diagonal
        if (x == y && boards[x_, y_] == boards[x_ + 1, y_ + 1] && boards[x_ + 1, y_ + 1] == boards[x_ + 2, y_ + 2])
        {
            return value;
        }
        // Check the anti-diagonal
        if (x + y == 2 && boards[x_, y_ + 2] == boards[x_ + 1, y_ + 1] && boards[x_ + 1, y_ + 1] == boards[x_ + 2, y_])
        {
            return value;
        }
        // If no win condition is met, return 0
        return 0;
    }
}


class Algo
{   
    private const int SELF_ID = 1;
    private const int OTHER_ID = -1;
    private const int WIN_SCORE = 10000;
    private const int SUB_WIN_SCORE = 100;
    private const int POTENTIAL_WIN = 10;
    private const int LOW_POTENTIAL_WIN = 2;
    private const int BLOCK = 7;
    private const int LOW_BLOCK = 1;
    private static int iteration;

    private static int ScoreMove(int[,] uboard, int[,] boards, int x, int y)
    {
        int score = 0;
        int scount = 0;
        int ocount = 0;

        int ux, uy;
        
        // Score from sub win
        if (Math.Abs(Board.CheckWinFromMove(uboard, x % 3, y % 3)) == 1)
        {
            return WIN_SCORE;
        }
        if (Math.Abs(Board.CheckWinFromMoveBoards(boards, x % 3, y % 3)) == 1)
        {
            return SUB_WIN_SCORE;
        }

        (ux, uy) = (3 * (x / 3), 3 * (y / 3));

        // Score from state
        (scount, ocount) = Algo.LineStateCount(boards[x, uy], boards[x, uy + 1], boards[x, uy + 2]);
        if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
        else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;

        (scount, ocount) = Algo.LineStateCount(boards[ux, y], boards[ux + 1, y], boards[ux + 2, y]);
        if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
        else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;

        if (x == y){
            (scount, ocount) = Algo.LineStateCount(boards[ux, uy], boards[ux + 1, uy + 1], boards[ux + 2, uy + 2]);
            if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
            else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;
        }
        if ((x + y) % 3 == 8){
            (scount, ocount) = Algo.LineStateCount(boards[ux, uy + 2], boards[ux + 1, uy + 1], boards[ux + 2, uy]);
            if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
            else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;
        }
    
        return score;
    }

    public static int[,] OrderingMoves(int[,] uboard, int[,] boards, int lx, int ly)
    {
        int[,] moves = new int[81, 2];
        int validActionCount = 0;
        foreach (var (x, y) in Board.GetMoves(uboard, boards, lx, ly))
        {
            (moves[validActionCount, 0], moves[validActionCount, 1]) = (x, y);
            validActionCount++;
        }
         
        return Algo.OrderingMoves(uboard, boards, moves, validActionCount);
        
    }

    public static int[,]  OrderingMoves(int[,] uboard, int[,] boards, int[,] moves, int validActionCount)
    {
        List<int[]> orderedMoves = new List<int[]>();
        for (int i = 0; i < validActionCount; i++)
        {
            orderedMoves.Add(new int[] { moves[i, 0], moves[i, 1] });
        }

        orderedMoves.Sort((m1, m2) =>
        {
            int score1 = Algo.ScoreMove(uboard, boards, m1[0], m1[1]);
            int score2 = Algo.ScoreMove(uboard, boards, m2[0], m2[1]);
            return score2.CompareTo(score1); // Tri décroissant par score
        });
        int[,] result = new int[validActionCount, 2];
        for (int i = 0; i < validActionCount; i++)
        {
            result[i, 0] = orderedMoves[i][0];
            result[i, 1] = orderedMoves[i][1];  
        }
        
        return result;
        

    }

    private static (int, int) LineStateCount(int x, int y, int z)
    {
        int scount = 0;
        int ocount = 0;
        if (x == SELF_ID) scount++;
        else if (x == OTHER_ID) ocount++;
        if (y == SELF_ID) scount++;
        else if (y == OTHER_ID) ocount++;
        if (z == SELF_ID) scount++;
        else if (z == OTHER_ID) ocount++;
        return (scount, ocount);
    }


    public static int Evaluate(int[,] uboard, int[,] boards)
    {
        int score = 0;
        int scount = 0;
        int ocount = 0;
        
        // Score from sub win
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (uboard[i, j] < 2)
                    score += uboard[i, j];
        score *= SUB_WIN_SCORE;

        // Score from state
        for (int ux = 0; ux < 9; ux+=3)
        {
            for (int uy = 0; uy < 9; uy+=3)
                {
                for (int i = 0; i < 3; i++)
                {
                    // Check Line
                    (scount, ocount) = Algo.LineStateCount(boards[ux + i, uy], boards[ux + i, uy + 1], boards[ux + i, uy + 2]);
                    if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
                    else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;

                    // Check Row
                    (scount, ocount) = Algo.LineStateCount(boards[ux, uy + i], boards[ux + 1, uy + i], boards[ux + 2, uy + i]);
                    if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
                    else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;
                }
                    // Check first diag
                    (scount, ocount) = Algo.LineStateCount(boards[ux, uy], boards[ux + 1, uy + 1], boards[ux + 2, uy + 2]);
                    if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
                    else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;

                    // Check second diag
                    (scount, ocount) = Algo.LineStateCount(boards[ux, uy + 2], boards[ux + 1, uy + 1], boards[ux + 2, uy]);
                    if ((scount, ocount) == (2, 0)) score += Algo.POTENTIAL_WIN;
                    else if ((scount, ocount) == (1, 2)) score += Algo.BLOCK;
                }
        }

                

        return score;
    }

    public static int AlphaBetaCore(int[,] uboard, int[,] boards, int last_player, int lx, int ly, int depth, int alpha, int beta)
    {
        int ux = lx/3;
        int uy = ly/3;
        int player = -last_player;


        if (uboard[ux, uy] < 2)
        {
            int winner = Board.CheckWinFromMove(uboard, ux, uy);
            if (winner == SELF_ID){ return WIN_SCORE; }
            else if (winner == OTHER_ID){ return -WIN_SCORE; }
        }

        if (depth <= 0){
            return last_player*Algo.Evaluate(uboard, boards);
        }

        int bestScore = -WIN_SCORE*player; // set to infiny
        int[,] orderedMoves = OrderingMoves(uboard, boards, lx, ly);
        int validActionCount = orderedMoves.Length / 2;
        int x, y;
        for (int i = 0; i < validActionCount; i++)
        {
            (x, y) = (orderedMoves[i, 0], orderedMoves[i, 1]);
            boards[x, y] = player;
            (ux, uy) = (x / 3, y / 3);
            uboard[ux, uy] = Board.CheckWinFromMoveBoards(boards, x, y);
            if (player == OTHER_ID)
            {
                bestScore = Math.Min(Algo.AlphaBetaCore(uboard, boards, player, x, y, depth-1, alpha, beta), bestScore);
                // Coupure alpha
                if (alpha >= bestScore) 
                {
                    boards[x, y] = 0;
                    uboard[ux, uy] = 0; 
                    return bestScore;
                }
                beta = Math.Min(beta, bestScore);
            }
            else
            {
                bestScore = Math.Max(Algo.AlphaBetaCore(uboard, boards, player, x, y, depth-1, alpha, beta), bestScore);
                // Coupure beta
                if (beta <= bestScore)
                {
                    boards[x, y] = 0;
                    uboard[ux, uy] = 0; 
                    return bestScore;
                }
                alpha = Math.Max(alpha, bestScore);
            }
            boards[x, y] = 0;
            uboard[ux, uy] = 0;    
        }
        return bestScore;
    }

    private static int ArgMax(int[] array)
    {
        int index = 0;
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > array[index])
                index = i;
        }
        return index;
    }

    public static (int, int) AlphaBeta(int[,] uboard, int[,] boards, int[,] moves, int validActionCount, int maxDepth, int iteration)
    {
        Algo.iteration = iteration;
        int[] scores = new int[validActionCount];
        int x, y, ux, uy, index;

        int[,] orderedMoves = Algo.OrderingMoves(uboard, boards, moves, validActionCount);
        
        if (validActionCount > 9 & iteration < 25)
            maxDepth = 3;

        if (iteration > 20)
            maxDepth += 2;

         if (iteration > 30)
            maxDepth += 0;

        for (int i = 0; i < validActionCount; i++)
        {
            (x, y) = (orderedMoves[i, 0], orderedMoves[i, 1]);
            boards[x, y] = Algo.SELF_ID;
            (ux, uy) = (x / 3, y / 3);
            uboard[ux, uy] = Board.CheckWinFromMoveBoards(boards, x, y);
            scores[i] = Algo.AlphaBetaCore(uboard, boards, Algo.SELF_ID, x, y, maxDepth, int.MinValue, int.MaxValue);
            boards[x, y] = 0;
            uboard[ux, uy] = 0;    
            index = Algo.ArgMax(scores);
            Console.Error.WriteLine(scores[index]);
        
        }
        index = Algo.ArgMax(scores);
        Console.Error.WriteLine(scores[index]);
        return (orderedMoves[index, 0], orderedMoves[index, 1]);
    }
}   