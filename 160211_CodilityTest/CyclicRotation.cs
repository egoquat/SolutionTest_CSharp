using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CyclicRotation
{
    public void Main()
    {
        Random rand = new Random(Environment.TickCount);

        string txt = "";

        string input = "1";
        int sequence = 0;
        do
        {
            //int K = rand.Next(0, 100);
            int K = 5;
            //int lengthArr = rand.Next(0, 100);
            int lengthArr = 9;
            string line = "";
            int[] A = new int[lengthArr];

            line += sequence + "/Shift("+ K +")" + "\n";
            for(int i = 0; i < A.Length; ++ i)
            {
                A[i] = rand.Next(-1000, 1000);
                line += A[i].ToString() + ",";
            }

            line += "\n";

            //Console.WriteLine("Input any positive integer:");
            //input = Console.ReadLine();

            //if (false == int.TryParse(input, out K))
            //{
            //    break;
            //}

            int[] ArrR = new int[A.Length];
            for(int i = 0; i < A.Length; ++i)
            {
                int idxK = (i + K) % A.Length;
                ArrR[idxK] = A[i];
            }


            for (int i = 0; i < ArrR.Length; ++i )
            {
                line += ArrR[i].ToString() + ",";
            }

            line += "\n";
            
            Console.WriteLine(line);
            txt += line;
            
        } while (sequence++ < 10);


        // Write the string to a file.
        System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\" + "CyclicRotation.txt", false);
        file.WriteLine(txt);

        file.Close();

        Console.WriteLine("END_________________________________________");
        Console.ReadLine();
    }
}
