using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BinaryGab
{
    public void Main()
    {
        Random rand = new Random(Environment.TickCount);

        string txt = "";

        string input = "1";
        int sequence = 0;
        do
        {
            int number = 0;
            string outputBin = "";

            //Console.WriteLine("Input any positive integer:");
            //input = Console.ReadLine();

            //if (false == int.TryParse(input, out number))
            //{
            //    break;
            //}

            //number = rand.Next() % 100000;
            number = int.MaxValue - 1;

            int cnt = 0;
            int cntMax = 0;
            bool isCounting = false;

            for (int i = 0; (1 << i) <= number && (1 << i) > 0 && i < 32; ++i)
            {
                int bit = (1 << i) & number;
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

                outputBin = ((0 == bit) ? "0" : "1") + outputBin;
            }

            string line = number + "//Binary(" + (outputBin) + ")//(" + (cntMax) + ")";
            Console.WriteLine(line);
            txt += line + "\n";
        } while (sequence++ < 1);


        // Write the string to a file.
        System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\" + "binarytest.txt", false);
        file.WriteLine(txt);

        file.Close();

        Console.WriteLine("END_________________________________________");
        Console.ReadLine();
    }
}
