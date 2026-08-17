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
    * Complete the 'downToZero' function below.
    *
    * The function is expected to return an INTEGER.
    * The function accepts INTEGER n as parameter.
    * 
    * 解題策略
    * DP:
    * 1. 建立dp[i]代表將數字i降到0所需的最少步數
    * 2. 一開始每個數字最壞的情況就是一路減1減到0
    * 3. 推演減1操作的結果，從數字i可以透過加1走到i+1，相當於從i+1減1走到i
    * 4. 推演因數暴力分解操作的結果，假設i是較大的因數，另一個較小的因數j必須小於或等於i，從數字i*j，只需花費1步就可以降到i
    * 
    * BFS:
    * 1. 從起點開始先找出能一步到達的數字並將其記錄下來(數字,步數)
    * 2. 再從一步到達的數字作為起點找到能一步到達的數字(此時為兩步)
    * 3. 以此類推直到碰到0或步數到達n(最差狀況每步都減1)
    */

    //BP
    private const int MAX = 1000001; //題目限制0~1000000
    private static int[] dp; //儲存每個數字降到0的最少步數

    static Result() //靜態建構子，只建立一次
    {
        dp = new int[MAX];

        for (int i = 0; i < MAX; i++) //初始化：最壞情況只能一路減1
        {
            dp[i] = i;
        }

        for (int i = 1; i < MAX; i++)
        {
            dp[i] = Math.Min(dp[i], dp[i - 1] + 1); //處理減1

            for (int j = 1; j <= i; j++) //處理因數分解
            {
                if (i * j >= MAX)
                {
                    break;
                }
                dp[i * j] = Math.Min(dp[i * j], dp[i] + 1);
            }
        }
    }

    public static int downToZero(int n)
    {
        return dp[n];
    }

    // BFS
    public static int downToZero(int n)
    {
        if (n == 0) //如果起點就是0，直接回傳0步
            return 0;

        Queue<(int number, int step)> queue = new Queue<(int, int)>(); //(當前數字, 已經走的步數)
        bool[] visited = new bool[n + 1]; //0~n

        queue.Enqueue((n, 0)); //起點
        visited[n] = true; //避免重複搜尋

        while (queue.Count > 0) //使用queue將同一步數全部處理完再處理加一步的數字
        {
            var (number, step) = queue.Dequeue();

            if (number == 0) 
                return step;

            int nextNumber = number - 1; //減1
            if (nextNumber >= 0 && !visited[nextNumber])
            {
                if (nextNumber == 0)  //提早檢查
                    return step + 1;

                visited[nextNumber] = true;
                queue.Enqueue((nextNumber, step + 1));
            }

            for (int i = (int)Math.Sqrt(number); i >= 2; i--) //因數分解，只需要從平方根開始往下找
            {
                if (number % i == 0) //找到因數
                {
                    int maxFactor = number / i; //取較大的因數
                    if (!visited[maxFactor])
                    {
                        visited[maxFactor] = true;
                        queue.Enqueue((maxFactor, step + 1));
                    }
                }
            }
        }

        return 0; //編譯邏輯用，理論上不會執行到這裡
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int q = Convert.ToInt32(Console.ReadLine().Trim());

        for (int qItr = 0; qItr < q; qItr++)
        {
            int n = Convert.ToInt32(Console.ReadLine().Trim());

            int result = Result.downToZero(n);

            textWriter.WriteLine(result);
        }

        textWriter.Flush();
        textWriter.Close();
    }
}