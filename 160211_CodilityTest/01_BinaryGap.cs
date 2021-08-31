using System;
// you can also use other imports, for example:
// using System.Collections.Generic;

// you can write to stdout for debugging purposes, e.g.
// Console.WriteLine("this is a debug message");

class Solution
{
    public int solution(int N)
    {

        if (N <= 0)
        {
            return 0;
        }

        int cnt = 0;
        int cntMax = 0;
        bool isCounting = false;

        for (int i = 0; (1 << i) <= N && (1 << i) > 0 && i < 32; ++i)
        {
            int bit = (1 << i) & N;
            if (bit == 0)
            {
                if (true == isCounting)
                {
                    cnt++;
                }
                else
                {
                    cnt = 0;
                }
            }
            else
            {
                if (cnt > 0)
                {
                    if (cnt > cntMax)
                    {
                        cntMax = cnt;
                    }
                    cnt = 0;
                }

                isCounting = true;
            }
        }

        return cntMax;
    }
}