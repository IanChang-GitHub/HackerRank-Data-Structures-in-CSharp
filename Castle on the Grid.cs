using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Result
{

    /*
     * Complete the 'minimumMoves' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts following parameters:
     *  1. STRING_ARRAY grid
     *  2. INTEGER startX
     *  3. INTEGER startY
     *  4. INTEGER goalX
     *  5. INTEGER goalY
     *  
     *  解題策略:
     *  1.採用BFS策略
     *  2.在所有尋找最短路徑或最少步數的演算法中，只要每走一步的成本都一樣(無權重圖)，BFS是最佳選擇
     */

    public static int minimumMoves(List<string> grid, int startX, int startY, int goalX, int goalY)
    {
        if(startX == goalX && startY == goalY) 
            return 0;

        char[][] gridArray = new char[grid.Count][];
        for (int i = 0; i < grid.Count; i++)
        {
            gridArray[i] = grid[i].ToCharArray();
        }

        int[][] step = new int[grid.Count][];
        for (int i = 0; i < grid.Count; i++)
        {
            step[i] = new int[grid.Count];
            Array.Fill(step[i], -1); // -1代表尚未拜訪
        }

        Queue<(int row, int colume)> queue = new Queue<(int row, int colume)>();
        int[] directionRow = { -1, 1, 0, 0 }; //四個移動方向：上、下、左、右
        int[] directionColume = { 0, 0, -1, 1 };

        queue.Enqueue((startX, startY)); //初始化起點
        step[startX][startY] = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int currentStep = step[current.row][current.colume];
            for (int i = 0; i < 4; i++) //針對四個方向分別進行
            {
                int nextRow = current.row + directionRow[i];
                int nextColume = current.colume + directionColume[i];

                while (nextRow >= 0 && nextRow < grid.Count && nextColume >= 0 && nextColume < grid.Count && gridArray[nextRow][nextColume] != 'X') //只要沒撞到邊界且不是X，就繼續沿著直線前行
                {
                    if (step[nextRow][nextColume] == -1) //若這個格子是第一次走到
                    {
                        step[nextRow][nextColume] = currentStep + 1; //紀錄步數

                        if (nextRow == goalX && nextColume == goalY)
                        {
                            return step[nextRow][nextColume];
                        }

                        queue.Enqueue((nextRow, nextColume));
                    }

                    nextRow += directionRow[i]; //繼續往同一個方向前進一格
                    nextColume += directionColume[i];
                }
            }
        }
        return -1;
    }

}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int n = Convert.ToInt32(Console.ReadLine().Trim());

        List<string> grid = new List<string>();

        for (int i = 0; i < n; i++)
        {
            string gridItem = Console.ReadLine();
            grid.Add(gridItem);
        }

        string[] firstMultipleInput = Console.ReadLine().TrimEnd().Split(' ');

        int startX = Convert.ToInt32(firstMultipleInput[0]);

        int startY = Convert.ToInt32(firstMultipleInput[1]);

        int goalX = Convert.ToInt32(firstMultipleInput[2]);

        int goalY = Convert.ToInt32(firstMultipleInput[3]);

        int result = Result.minimumMoves(grid, startX, startY, goalX, goalY);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}