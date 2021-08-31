using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TapeEquilibrium
{
    public void Main()
    {
        Random rand = new Random(Environment.TickCount);

        string txt = "";

        string input = "1";
        int sequence = 0;
        do
        {
            //int N = rand.Next(2, 100000);
            int N = 2;
            string line = "";
            int[] A = new int[N];

            line += sequence + "/N(" + N + ")" + "\n";
            for (int i = 0; i < A.Length; ++i)
            {
                A[i] = rand.Next(-10, 10);
                line += A[i] + ", ";
            }
            line += "\n";

            //@ Start.
            long right = 0, left = 0;
            uint differMin = uint.MaxValue;
            for (int i = 0; i < A.Length; ++i )
            {
                right += A[i];
            }

            uint differ = 0;
            for (int i = 0; i < A.Length; ++i )
            {
                left += A[i];
                right -= A[i];
                differ = (uint)Math.Abs(left - right);

                if (differMin > differ)
                {
                    differMin = differ;
                }
            }

            //@ End.

            line += "differ(" + differ + ")\n";

            Console.WriteLine(line);
            txt += line;

        } while (sequence++ < 10);


        // Write the string to a file.
        System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\" + "TapeEquilibrium.txt", false);
        file.WriteLine(txt);

        file.Close();

        List<int> list = new List<int>();
        int countnumber = list.Count();
        

        Console.WriteLine("END_________________________________________");
        Console.ReadLine();
    }
}

