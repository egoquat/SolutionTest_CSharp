using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        protected const float SpeedG = -3.7111f;
        protected const float PowerMax = 4.0f;
        protected const float SpeedMaxLanding = 40.0f;

        protected const float Epsilon = 0.0001f;

        private const float DegToRad = (float)Math.PI / 180.0f;
        private const float RadToDeg = 180.0f / (float)Math.PI;

        public static float EpsilonZero(float f) { return Math.Abs(f) < Epsilon ? 0.0f : f; }

        class Vector
        {
            private float _x;
            private float _y;

            public float X
            {
                get { return _x; }
                set { _x = value; }
            }

            public float Y
            {
                get { return _y; }
                set { _y = value; }
            }

            public int Length()
            {
                return (int)Math.Round((Math.Sqrt(_x * _x + _y * _y)));
            }

            public float LengthF()
            {
                return (float)(Math.Sqrt(_x * _x + _y * _y));
            }

            public Vector Normalize()
            {
                float length = Length();
                _x = _x / length;
                _y = _y / length;

                return this;
            }

            public Vector Rotate(float degree)
            {
                float rad = degree * DegToRad;
                float cos = (float)Math.Cos(rad);
                float sin = (float)Math.Sin(rad);

                _x = cos * _x - sin * _y;
                _y = sin * _x + cos * _y;

                return this;
            }

            public static Vector operator +(Vector a, Vector b)
            {
                return new Vector(a.X + b.X, a.Y + b.Y);
            }

            public static Vector operator -(Vector a, Vector b)
            {
                return new Vector(a.X - b.X, a.Y - b.Y);
            }

            public static Vector operator *(Vector a, Vector b)
            {
                return new Vector(a.X * b.X, a.Y * b.Y);
            }

            public static Vector operator *(Vector a, float f)
            {
                return new Vector(a.X * f, a.Y * f);
            }

            public static Vector operator /(Vector a, float f)
            {
                return new Vector(a.X / f, a.Y / f);
            }

            public static Vector operator /(Vector a, int i)
            {
                return new Vector(a.X / i, a.Y / i);
            }

            public static Vector Normalize(Vector v)
            {
                return v.Normalize();
            }

            public static float Dot(Vector a, Vector b)
            {
                Vector a_ = a.Normalize();
                Vector b_ = b.Normalize();

                return a_.X * b_.X + a_.Y * b_.Y;
            }

            public static Vector Middle(Vector a, Vector b)
            {
                return new Vector((a.X + b.X) * 0.5f, (a.Y + b.Y) * 0.5f);
            }

            public static float GetAngleDegBetween(Vector a, Vector b)
            {
                return (float)(Math.Acos(Dot(a, b) / (a.Length() * b.Length())) * RadToDeg);
            }

            public Vector Set(float x, float y)
            {
                _x = x; _y = y;
                return this;
            }

            public override string ToString()
            {
                return "("+_x+","+_y+")";
            }

            public Vector(float x, float y)
            {
                Set(x, y);
            }

            public Vector() { }
        }

        static List<Vector> GetPosTargetLanding(List<Vector> listLand)
        {
            Vector vReturn = new Vector();
            Vector pPrevious = new Vector(float.MinValue, float.MinValue);
            List<Vector> sameLevel = new List<Vector>();
            foreach (Vector p in listLand)
            {
                if (p.Y == pPrevious.Y)
                {
                    sameLevel.Add(pPrevious);
                    sameLevel.Add(p);
                    break;
                }

                pPrevious.Set(p.X, p.Y);
            }

            if (sameLevel.Count() != 2)
            {
                Console.Error.WriteLine("No Level//(sameLevel.Count() < 1)");
                return sameLevel;
            }
            else
            {
                vReturn.Set(sameLevel[0].X + sameLevel[1].X * 0.5f, sameLevel[0].Y);
                Console.Error.WriteLine("No Level//(sameLevel.Count() < 1)");
                return sameLevel;
            }
        }

        static void CalcAnglePower( int powerCurrent,
                                    int angleCurrent,
                                    int fuelCurrent,
                                    Vector speedCurrent, 
                                    Vector posCurrent, 
                                    List<Vector> targetLevel,
                                    out int powerTarget, out int angleTarget )
        {
            Vector speedCurr = new Vector(speedCurrent.X, speedCurrent.Y);
            Vector posCurr = new Vector(posCurrent.X, posCurrent.Y);
            Vector posTarget = targetLevel[0] + targetLevel[1];
            Vector dirUp = (targetLevel[0] - targetLevel[1]).Rotate(90.0f).Normalize();
            bool isIn = false;
            bool isOverSpeedLanding = false;
            posTarget.Set(posTarget.X / 2.0f, posTarget.Y / 2.0f);

            bool isCrossOver = false;

            float fuelCurr = fuelCurrent;

            Console.Error.WriteLine("CalcAngle//powerCurrent(" + (powerCurrent) + ")/angleCurrent(" + angleCurrent + ")");

            float speedX = speedCurrent.X, speedY = speedCurrent.Y;

            //@ Key = speed each fall, Value = distance from land.
            Dictionary<int, int> collectFall = new Dictionary<int, int>();

            int seq = 0;
            while (posCurr.Y > 0 && fuelCurr > 0 && isCrossOver == false)
            {
                ++seq;
                fuelCurr = fuelCurr - powerCurrent;

                if (fuelCurr < 0)
                {
                    break;
                }

                speedY = speedY + ((EpsilonZero((float)Math.Cos(DegToRad * angleCurrent)) * powerCurrent) + SpeedG);
                speedX = speedX + (EpsilonZero((float)Math.Cos(DegToRad * (90.0f - angleCurrent))) * powerCurrent);

                speedCurr.Set(speedX, speedY);

                posCurr.Set(posCurr.X + speedCurr.X, posCurr.Y + speedCurr.Y);

                collectFall.Add((int)Math.Round(speedY), (int)Math.Round(posCurr.Y));

                Console.Error.WriteLine("CalcAngle(" + (seq.ToString("00")) + ")//angle(" + (angleCurrent) + "," + EpsilonZero((float)Math.Cos(DegToRad * (90.0f - angleCurrent)))
                                            + ")/speedCurr" + (speedCurr) 
                                            + "/pos" + posCurr
                                            + "/posTarget" + (posTarget) 
                                            + "/dirUp" + (dirUp) + "");

                Vector dirPos = posCurr - posTarget;
                if (Vector.Dot(dirUp, dirPos) < 0)  //Cross Over Land.
                {
                    if (speedCurr.Y > SpeedMaxLanding)
                    {
                        isOverSpeedLanding = false;
                    }
                    else
                    {
                        isOverSpeedLanding = true;
                    }

                    Console.Error.WriteLine("CalcAngle//CrossOver");
                    isCrossOver = true;
                    Vector dirIn = targetLevel[1] - targetLevel[0];
                    Vector dirInPos = posCurr - targetLevel[0];
                    isIn = Vector.Dot(dirIn, dirInPos) > 0? true : false;

                    dirIn = targetLevel[0] - targetLevel[1];
                    dirInPos = posCurr - targetLevel[1];
                    isIn |= Vector.Dot(dirIn, dirInPos) > 0 ? true : false;
                }
            }

            Console.Error.WriteLine("CalcAnglePower//isIn("+(isIn)+")/fuel("+(fuelCurr)+")/speedCurr.Y("+(speedCurr.Y)+")");

            if (false == isOverSpeedLanding)
            {
                
                float speedStable = SpeedMaxLanding - 30;
                float speedPowerG = (SpeedG + PowerMax);
                
            }

            powerTarget = powerCurrent;
            angleTarget = angleCurrent;
        }

        static void Main(string[] args)
        {
            string[] inputs;
            int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.

            string outputTest01 = "N(" + surfaceN + ")/";

            List<Vector> listLands = new List<Vector>();

            for (int i = 0; i < surfaceN; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
                int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.

                listLands.Add(new Vector(landX, landY));

                outputTest01 += "/(" + landX + "," + landY + ")";
            }

            Console.Error.WriteLine(outputTest01);

            Vector pos = new Vector();
            Vector speed = new Vector();
            Vector DirDown = new Vector(0, -1);

            int speedTarget = 3;
            int angleTarget = 0;

            // game loop
            while (true)
            {
                inputs = Console.ReadLine().Split(' ');
                int X = int.Parse(inputs[0]);
                int Y = int.Parse(inputs[1]);
                int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                pos.Set(X, Y);
                speed.Set(hSpeed, vSpeed);
                
                angleTarget = 6;    //@TEST

                CalcAnglePower(power, rotate, fuel, speed, pos, listLands, out speedTarget, out angleTarget);

                Console.Error.WriteLine(    "rotate(" + (rotate) 
                                        + ")//power(" + (power) 
                                        + "//pos" + pos);

                Console.WriteLine(angleTarget + " " + speedTarget); // 2 integers: rotate power. rotate is the desired rotation angle (should be 0 for level 1), power is the desired thrust power (0 to 4).
            }
        }

    }
}
