using System;
using System.Security.Cryptography;
using System.Text;

namespace GameOfLife
{
    static class Program
    {
        static int Rows = 0;
        static int Columns = 0;
        static int Generations = 0;
        public static void Main()
        {
            try
            {
                Console.WriteLine("Please endter board size");
                Console.WriteLine("Rows");
                Rows = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Columns");
                Columns = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Number of Generations");

                Generations = Convert.ToInt32(Console.ReadLine());

                var grid = new Status[Rows, Columns];
                int counter = 0;

                for (var row = 0; row < Rows; row++)
                {
                    for (var column = 0; column < Columns; column++)
                    {
                        grid[row, column] = (Status)RandomNumberGenerator.GetInt32(0, 2);
                    }
                }
                while (counter <= Generations)
                {
                    Console.WriteLine(counter == 0 ? "initial state" : "Generation " + counter + " state");
                    Print(grid);
                    grid = NextGeneration(grid);
                    counter++;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
        }

        private static Status[,] NextGeneration(Status[,] currentGrid)
        {
            var nextGeneration = new Status[Rows, Columns];
            for (var row = 1; row < Rows - 1; row++)
                for (var column = 1; column < Columns - 1; column++)
                {
                    var aliveNeighbors = 0;
                    for (var i = -1; i <= 1; i++)
                    {
                        for (var j = -1; j <= 1; j++)
                        {
                            aliveNeighbors += currentGrid[row + i, column + j] == Status.Alive ? 1 : 0;
                        }
                    }
                    var currentCell = currentGrid[row, column];
                    aliveNeighbors -= currentCell == Status.Alive ? 1 : 0;
                    // Cell is lonely and dies 
                    if (currentCell.HasFlag(Status.Alive) && aliveNeighbors < 2)
                    {
                        nextGeneration[row, column] = Status.Dead;
                    }
                    // Cell dies due to over population 
                    else if (currentCell.HasFlag(Status.Alive) && aliveNeighbors > 3)
                    {
                        nextGeneration[row, column] = Status.Dead;
                    }
                    // A new cell is born 
                    else if (currentCell.HasFlag(Status.Dead) && aliveNeighbors == 3)
                    {
                        nextGeneration[row, column] = Status.Alive;
                    }
                    else
                    {
                        nextGeneration[row, column] = currentCell;
                    }
                }
            return nextGeneration;
        }

        private static void Print(Status[,] future)
        {
            var stringBuilder = new StringBuilder();
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    var cell = future[row, column];
                    stringBuilder.Append(cell == Status.Alive ? "X" : "0");
                }
                stringBuilder.Append("\n");
            }
            Console.Write(stringBuilder.ToString());
        }
    }

    public enum Status
    {
        Dead,
        Alive,
    }
}