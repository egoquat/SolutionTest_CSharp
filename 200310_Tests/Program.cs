using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Solution01
{
    static bool IsLowerCase(char c)
    {
        return c >= 97 && c <= 122;
    }
    public string solution(string s)
    {
        string answer = "";
        List<char> lowers = new List<char>();
        List<char> uppers = new List<char>();
        for (int i = 0; i < s.Length; ++i)
        {
            char c = s[i];
            if (true == IsLowerCase(c))
            {
                lowers.Add(c);
            }
            else
            {
                uppers.Add(c);
            }
        }
        lowers.Sort((a, b) => { return a == b ? 0 : a > b ? -1 : 1; });
        uppers.Sort((a, b) => { return a == b ? 0 : a > b ? -1 : 1; });
        answer = new string(lowers.ToArray());
        answer = answer + new string(uppers.ToArray());
        Console.WriteLine(answer);
        return answer;
    }
}
public class Solution02
{
    public long solution(long n)
    {
        long iter = n;
        List<int> digits = new List<int>();
        while (iter >= 1)
        {
            long remain = iter % 10;
            iter = iter / 10;
            digits.Add((int)remain);
        }

        digits.Sort((a, b) => { return a == b ? 0 : a < b ? -1 : 1; });
        long answer = 0;
        for (int i = 0; i < digits.Count; ++i)
        {
            answer = answer + (digits[i] * ((long)Math.Pow(10, i)));
        }
        Console.WriteLine(answer);
        return answer;
    }
}
class Solution3
{
    static void GetSubsetsR(int[] nums, int index, List<int> subset, ref List<List<int>> subsets)
    {
        if (subset.Count == 3)
        {
            subsets.Add(subset.ToList());
            //Console.WriteLine("subsets.Add:" + index + "/" + subset.String());
            return;
        }

        for (int i = index; i < nums.Length; i++)
        {
            subset.Add(nums[i]);
            GetSubsetsR(nums, i + 1, subset, ref subsets);
            subset.RemoveAt(subset.Count - 1);
        }

        return;
    }
    static List<List<int>> GetSubsets(int[] nums)
    {
        List<int> subset = new List<int>();
        List<List<int>> subsets = new List<List<int>>();

        int index = 0;
        GetSubsetsR(nums, index, subset, ref subsets);

        return subsets;
    }
    static bool IsPrime(List<int> subset)
    {
        int num = 0;
        for (int i = 0; i < subset.Count; ++i) { num += subset[i]; }
        Console.WriteLine(">> num:" + num + "/subset:" + subset.ToString());
        if (num <= 1) return false;
        if (num == 2) return true;
        if (num == 3) return true;
        int sqrt = (int)Math.Sqrt(num);
        for (int i = 2; i <= sqrt; ++i)
        {
            if (num % i == 0)
            {
                Console.WriteLine(">> " + num + "/false");
                return false;
            }
        }
        Console.WriteLine(">> " + num + "/true");
        return true;
    }
    static int GetCountPrimes(int[] nums)
    {
        List<List<int>> subsets = GetSubsets(nums);
        Console.WriteLine(">> subsets:" + subsets.Count);
        int count = 0;
        for (int i = 0; i < subsets.Count; ++i)
        {
            if (true == IsPrime(subsets[i]))
            {
                ++count;
            }
        }
        return count;
    }
    public int solution(int[] nums)
    {
        int answer = GetCountPrimes(nums);
        return answer;
    }
}
public class Solution5
{
    struct CPy
    {
        int Idx;
        int Py;
        CPy(int idx, int py) { Idx = idx; Py = py; }
    }
    struct CPs
    {
        int IdxOrder;
        List<CPy> Pys;
    }
    public void Order(int[] ps, ref List<int> orders)
    {
        List<CPy> pys = new List<CPy>();
        for (int i = 0; i < ps.Length; ++i)
        {

        }
    }
    public int solution(int[] priorities, int location)
    {
        int answer = 0;
        return answer;
    }
}

class Solution6
{
    public bool GetPow(int num, int limit, out int teledOut, out int remainOut)
    {
        if (num >= limit)
        {
            teledOut = 0; remainOut = 0; return false;
        }
        int count = 1;
        while (true)
        {
            long powed = (long)Math.Pow(num, count);
            if (powed > (long)limit)
            {
                teledOut = count - 1;
                remainOut = (int)((long)limit - (long)Math.Pow(num, teledOut)); break;
            }
        }
        return true;
    }
    public int solution(int n)
    {
        int sqrted = (int)Math.Sqrt(n) + 1;
        int consume = 0, remain = 0, teled = 0;
        for (int i = 0; i < sqrted; ++i)
        {

        }
        int answer = 0;
        System.Console.WriteLine("Hello C#");

        return answer;
    }
}

class Solution7
{
    int GetGCD(int x, int y)
    {
        int r = 0, a, b;
        a = (x > y) ? x : y;
        b = (x < y) ? x : y;

        r = b;
        while (a % b != 0)
        {
            r = a % b;
            a = b;
            b = r;
        }
        return r;
    }
    int FindGCD(int[] arr, int n)
    {
        int result = arr[0];
        for (int i = 1; i < n; i++)
        {
            result = GetGCD(arr[i], result);

            if (result == 1)
            {
                return 1;
            }
        }

        return result;
    }
    int GetGCD2(int a, int b)
    {
        if (a == 0)
            return b;
        return GetGCD2(b % a, a);
    }
    int FindLCM(int[] nums)
    {
        int ans = nums[0];
        for (int i = 1; i < nums.Length; i++)
            ans = (((nums[i] * ans)) /
                    (GetGCD2(nums[i], ans)));

        return ans;
    }
}

class Solution8
{
    bool IsRule(string[] words, int idx)
    {
        if (0 == idx) return true;
        string word = words[idx];
        string wordPrev = words[idx - 1];
        if (word[0] != wordPrev[wordPrev.Length - 1]) return false;
        if (Array.IndexOf(words, word) < idx) return false;
        return true;
    }
    public int[] solution(int n, string[] words)
    {
        int numInvalid = 0;
        for (int i = 0; i < words.Length; ++i)
        {
            if (IsRule(words, i)) continue;
            numInvalid = i + 1; break;
        }
        int numSeq = numInvalid % n;
        int numCount = (int)Math.Ceiling((float)numInvalid / (float)n);
        if (0 != numInvalid && 0 == numSeq) numSeq = n;
        System.Console.WriteLine("numInvalid:" + numInvalid + "/numSeq:" + numSeq + "/numCount:" + numCount);
        int[] answer = new int[2] { numSeq, numCount };
        return answer;
    }
}

public class Solution10Ing
{
    int GetCount(int[] heights, List<int> stack, int idx)
    {
        int count = 0, h = heights[idx];
        for (int i = idx - 1; i >= 0; --i)
        {
            int curr = heights[i];
            if (curr > h) { count++; }
            else if (curr < h) { count = count + stack[i] + 1; break; }
            else { count = count + stack[i]; break; }
        }
        stack.Add(count);
        return count;
    }
    public int[] solution(int[] heights)
    {
        List<int> stack = new List<int>();

        int[] answer = new int[] { };
        return answer;
    }
}

public class Solution11
{
    public string solution(string s)
    {
        List<int> nums = s.Split(' ').Select(Int32.Parse).ToList();
        nums.Sort((a, b) => { return a == b ? 0 : a < b ? -1 : 1; });
        string answer = "" + nums[0] + " " + nums[nums.Count - 1];
        return answer;
    }
}

public class Solution12
{
    enum DIR { U, D, L, R };
    class Node
    {
        public int X = 0, Y = 0;
        public Node[] Adjs = new Node[4];
        public bool[] IsVisited = new bool[4];
        public int DirIdx(DIR dir) { return (int)dir; }
        public Node(int x, int y) { X = x; Y = y; }
        public void Init()
        {
            //int x=X+(w-1);
        }
        static Node[] Nodes = new Node[11 * 11];
        static int GetIdx(int x, int y) { return ((y + 5) * 11) + (x + 5); }
        static void GetIdx(int idx, out int x, out int y)
        {
            x = idx % 11 - 5; y = idx / 11 - 5;
            Console.WriteLine("GetIdx(" + idx + ")/x:" + x + "y:" + y);
        }
        static Node GetNode(int x, int y) { return Nodes[GetIdx(x, y)]; }
    }
    public int solution(string dirs)
    {
        int answer = 0;
        return answer;
    }
}

public class Solution13
{
    class Node
    {
        public int Fold = 0;
        public Node(int fold) { Fold = fold; }
    }
    void FoldR(Node s, int depthTarget, int depth, int folded, ref LinkedList<Node> buffs)
    {
        if (depthTarget <= depth)
            return;
        //buffs.AddAfter(s, new Node(0));
    }
    public int[] solution(int n)
    {
        LinkedList<Node> buffs = new LinkedList<Node>();
        buffs.AddLast(new Node(0)); buffs.AddLast(new Node(1));
        FoldR(buffs.First(), n, 0, 0, ref buffs);
        

        int[] answer = new int[] { };
        return answer;
    }
}

class Solution14
{
    public int solution(int n, int[] stations, int w)
    {
        int last = 0, counting = 0, range = w * 2 + 1;
        for (int i = 0; i < stations.Length; ++i)
        {
            int curr = stations[i] - w, blank = curr - last, numRemain = blank % range, numAdd = blank / range;
            numAdd = numAdd + (numRemain >= 1 ? 1 : 0);
            Console.WriteLine(" numAdd:" + numAdd);
            counting = counting + numAdd;
            last = curr + w;
        }
        int numAddLast = (n - last) / range;
        Console.WriteLine(" numAddLast:" + numAddLast);
        counting = counting + numAddLast;
        int answer = counting;
        return answer;
    }
}

public class Solution15
{
    class Node
    {
        int Idx = -1;
        List<int> Adjs = new List<int>();
        public Node(int idx)
        {
            Idx = idx;
            string word = Words[idx];
            for(int i = 0; i < Words.Length; ++i)
            {
                if (idx == i) continue;
                string w = Words[i];
                if (1 == GetDiff(word, w))
                {
                    Adjs.Add(i);
                }
            }
        }
        int GetRuled(string a, string[] words, ref List<int> except)
        {
            for (int i = 0; i < words.Length; ++i)
            {
                if (words[i] == a || except.Contains(i) || GetDiff(words[i], a) != 1) continue;
                except.Add(i);
                return i;
            }
            return -1;
        }
        static int GetDiff(string a, string b)
        {
            int countDiff = 0;
            for (int i = 0; i < a.Count(); ++i)
            {
                if (a[i] != b[i]) { ++countDiff; }
            }
            return countDiff;
        }
        public static void Init(string begin, string target, string[] words)
        {
            Begin = begin; Target = target; Words = words;
            for (int i = 0; i < words.Length; ++i)
            {

            }
        }
        static List<Node> Nodes = new List<Node>();
        static string[] Words = null;
        static string Begin = null, Target = null;
    }
    public int solution(string begin, string target, string[] words)
    {
        if (false == words.Contains(target))
            return 0;

        Node.Init(begin, target, words);
        List<int> stackList = new List<int>();
        return stackList.Count();
    }
}

class Solution20
{
    public int[] solution(int[,] v)
    {
        int minX = 1 << 30; int maxX = -(1 << 30);
        int minY = 1 << 30; int maxY = -(1 << 30);
        for (int i = 0; i < v.GetLength(0); ++i)
        {
            if (v[i, 0] < minX) minX = v[i, 0];
            if (v[i, 0] > maxX) maxX = v[i, 0];
            if (v[i, 1] < minY) minY = v[i, 1];
            if (v[i, 1] > maxY) maxY = v[i, 1];
        }

        int X = minX * 2 + maxX * 2;
        int Y = minY * 2 + maxY * 2;
        for (int i = 0; i < v.GetLength(0); ++i)
        {
            X = X - v[i, 0];
            Y = Y - v[i, 1];
        }

        int[] answer = { X, Y };
        System.Console.WriteLine("minX" + minX + "/minY" + minY + "/maxX" + maxX + "/maxY" + maxY +"/X:"+X + "/Y:"+Y);
        return answer;
    }
}



class Program
{
    static void Main(string[] args)
    {
        //Solution15 sol = new Solution15();
        //string[] words = { "hot", "dot", "dog", "lot", "log", "cog"};
        //sol.solution("hit", "cog", words);

        Solution20 sol = new Solution20();
        int[,] tests = { { 1, 4 }, { 3, 4 }, { 3, 10 } };
        sol.solution(tests);        
    }
}
