using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolutionTest_CSharp
{
    class A
    {
        static int _sequence = 0;
        protected int _id = 0;
        public A()
        {
            _id = _sequence++;
            Console.WriteLine("Creating A("+(_id)+ ")");
        }
        ~A()
        {
            Console.WriteLine("Destroying A(" + (_id) + ")");
        }
    }

    class B : A
    {
        public B()
        {
            Console.WriteLine("Creating B(" + (_id) + ")");
        }
        ~B()
        {
            Console.WriteLine("Destroying B(" + (_id) + ")");
        }

    }
    class C : B
    {
        public C()
        {
            Console.WriteLine("Creating C(" + (_id) + ")");
        }

        ~C()
        {
            Console.WriteLine("Destroying C(" + (_id) + ")");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            {
                C[] tableC = new C[10];
                Array.ForEach(tableC, c => { c = new C(); });

                Console.WriteLine("Objects Created ");
                Console.WriteLine("Press enter to Destroy it");
                Console.ReadLine();
                //Array.ForEach(tableC, c => { c = null; });
                tableC = null;

                Console.WriteLine("Call GC.Collect");
                GC.Collect();
            }
            
            //Test codes
            int spaceX = 0, spaceY = 0;
            Dictionary<int, int> mts = new Dictionary<int, int>();
            //mts.Any(k => { return k.Key == iTest && Math.Abs(k.Value - iTest) == 1; });
            bool isApproach = mts.Any(
                p => 
                { 
                    bool isApproachNow = ((p.Key == spaceX));
                    if (isApproachNow == true)
                    {
                        var nextL = mts.FirstOrDefault(p_ => { return (spaceY - p_.Value) <= 1; });

                        if (nextL.Equals(default(KeyValuePair<int, int>)) == true)
                        {
                            return true;
                        }
                        else 
                        {
                            return nextL.Key == spaceX;
                        }
                    }
                    else
                    {
                        return false;
                    }
                });

            //Any routines for call Object destructors.
            Console.Read();
            //Console.ReadLine();


            int n = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
            string temps = Console.ReadLine(); // the n temperatures expressed as integers ranging from -273 to 5526

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            string[] tempsStr = temps.Split(' ');
            int[] tempsNum = new int[tempsStr.Count()];
            string.IsNullOrEmpty(temps);
            

            int tempNearst = tempsNum[0];
            foreach (int temp in tempsNum)
            {
                int tnAbs = Math.Abs(tempNearst), tAbs = Math.Abs(temp);
                if (tnAbs == tAbs)
                {
                    tempNearst = (temp > 0) ? temp : tempNearst;
                }
                else if (tnAbs > tAbs)
                {
                    tempNearst = temp;
                }
            }



            //string outputDebug = "";
            //for (int i = 0; i < 10; ++i)
            //{
            //    outputDebug = outputDebug + "\nNumber:" + i;
            //}

            //System.Diagnostics.Trace.WriteLine(outputDebug);
        }
    }
}
