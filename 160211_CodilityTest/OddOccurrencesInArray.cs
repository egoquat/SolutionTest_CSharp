using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class OddOccurrencesInArray
{
    public void Main()
    {
        Random rand = new Random(Environment.TickCount);

        int[] A = new int[rand.Next(2, 100000)];

        int unpaired = 0;

        for(int i = 0; i < A.Length; ++i)
        {
            unpaired = unpaired ^ A[i];
        }

        Console.WriteLine("OddOccurrencesInArray("+(unpaired)+")");
    }
}
