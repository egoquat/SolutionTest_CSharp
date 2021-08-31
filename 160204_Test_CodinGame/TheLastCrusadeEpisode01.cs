using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public enum DIR
{
    TOP, BOTTOM, RIGHT, LEFT, NONE
};

struct Point
{
    public int X;
    public int Y;

    public bool IsSame(Point other) { return X == other.X && Y == other.Y; }

    public Point(int x, int y) { X = x; Y = y; }
}

class Way
{
    private DIR _in;
    private DIR _out;

    public DIR GetIn
    {
        get { return _in; }
    }

    public DIR GetOut
    {
        get { return _out; }
    }

    public bool IsIn(DIR in_)
    {
        return _in == in_;
    }

    public Way(DIR in_, DIR out_)
    {
        _in = in_;
        _out = out_;
    }

    public static Way N(DIR in_, DIR out_)
    {
        return new Way(in_, out_);
    }
}

class Node
{
    private Way[] _ways = null;
    private Dictionary<DIR, Node> _adjacents = null;
    private Point _pnt;

    public String ToStr { get { string output = "Node:(" + _pnt.X + "," + _pnt.Y + ")"; return output; } }

    public Point Point { get { return _pnt; } }

    public bool IsInstanceNode { get; private set; }

    public Way[] GetWays { get { return _ways; } }

    public DIR[] GetOuts
    {
        get
        {
            List<DIR> outs = null;
            if (null != _ways && 0 < _ways.Length)
            {
                outs = new List<DIR>();
                for (int i = 0; i < _ways.Length; ++i)
                {
                    Way w = _ways[i];
                    outs.Add(w.GetOut);
                }
            }

            return (null != outs && 0 < outs.Count) ? outs.ToArray() : null;
        }
    }

    public DIR[] GetComes
    {
        get
        {
            List<DIR> comes = null;
            if (null != _ways && 0 < _ways.Length)
            {
                comes = new List<DIR>();
                for (int i = 0; i < _ways.Length; ++i)
                {
                    Way w = _ways[i];
                    comes.Add(w.GetIn);
                }
            }

            return (null != comes && 0 < comes.Count) ? comes.ToArray() : null;
        }
    }

    private bool IsLinkable(DIR dirFrom, DIR dirTo)
    {
        if (Map.LINKS.ContainsKey(dirFrom) == false)
            return false;
        return Map.LINKS[dirFrom] == dirTo;
    }

    private DIR GetLocateAdj(Node adj)
    {
        if (adj._pnt.X == _pnt.X && Math.Abs(adj._pnt.Y - _pnt.Y) == 1)
            return (_pnt.Y - adj._pnt.Y) <= -1 ? DIR.TOP : DIR.BOTTOM;

        if (adj._pnt.Y == _pnt.Y)
            return (_pnt.X - adj._pnt.X) <= -1 ? DIR.LEFT : DIR.RIGHT;

        return DIR.NONE;
    }

    private DIR GetLinkAdj(Node adj)
    {
        DIR[] dirFroms = GetOuts;
        DIR[] dirTos = adj.GetComes;
        if (null == dirFroms || null == dirTos)
            return DIR.NONE;

        DIR dirResult = DIR.NONE;
        foreach (DIR dirFrom in dirFroms)
        {
            foreach (DIR dirTo in dirTos)
            {
                if (true == IsLinkable(dirFrom, dirTo))
                {
                    dirResult = dirFrom;
                    break;
                }
            }

            if (DIR.NONE != dirResult)
                break;
        }

        return dirResult;
    }

    private void AddAdjIfLinkable(Node adj)
    {
        if (null == adj || true == Point.IsSame(adj.Point))
            return;

        DIR dirAdj = GetLocateAdj(adj);
        if (DIR.NONE == dirAdj)
            return;

        DIR dirAdjLinked = GetLinkAdj(adj);
        if (dirAdj != dirAdjLinked)
        {
            Console.Error.WriteLine("skip Addadj Node:" + ToStr + "/Adj:" + adj.ToStr + "/dirAdjLinked:" + dirAdjLinked);
            return;
        }

        Console.Error.WriteLine("Addadj Node:" + ToStr + "/Adj:" + adj.ToStr + "/dirAdjLinked:" + dirAdjLinked + "/cnd adj:" + _adjacents.Count);
        if (_adjacents.ContainsKey(dirAdjLinked))
        {
            Console.Error.WriteLine("ERROR:add Node:" + ToStr + "/Adj:" + adj.ToStr + "/dir:" + dirAdjLinked + "/Exist:" + _adjacents[dirAdjLinked].ToStr + "/Same:" + (adj == _adjacents[dirAdjLinked]));
        }
        else
        {
            _adjacents.Add(dirAdjLinked, adj);
        }
    }

    public void SetAdjacents(Node[,] nodeMaps)
    {
        for (int w = 0; w < nodeMaps.GetLength(0); ++w)
        {
            for (int h = 0; h < nodeMaps.GetLength(1); ++h)
            {
                AddAdjIfLinkable(nodeMaps[w, h]);
            }
        }
    }

    public void Init(int x, int y)
    {
        _pnt = new Point(x, y);
        _adjacents = new Dictionary<DIR, Node>();
    }

    public Node(params Way[] ways)
    {
        _ways = ways;
    }

    public Node(Node nodeRsc)
    {
        if (null != nodeRsc._ways && 1 <= nodeRsc._ways.Length)
        {
            _ways = new Way[nodeRsc._ways.Length];
            nodeRsc._ways.CopyTo(_ways, 0);
        }

        if (null != nodeRsc._adjacents && 1 <= nodeRsc._adjacents.Count)
        {
            _adjacents = new Dictionary<DIR, Node>(nodeRsc._adjacents);
        }

        _pnt = new Point(nodeRsc._pnt.X, nodeRsc._pnt.Y);
    }

    public static Node NewRsc(params Way[] ways)
    {
        return new Node(ways);
    }

    public static Node NewPath(Node nodeRsc)
    {
        return new Node(nodeRsc);
    }
}

public static class Map
{
    enum STATUSNODE
    {
        NONE = -1, START, WAY, EXIT,
    }

    private static Dictionary<int, Node> _Nodes = new Dictionary<int, Node>();
    private static Node[,] _maps = null;
    private static int[,] _mapWays = null;
    private static Point _pntExit;

    public static readonly Dictionary<DIR, DIR> LINKS = new Dictionary<DIR, DIR>()
    {
        {DIR.LEFT, DIR.RIGHT},
        {DIR.RIGHT, DIR.LEFT},
        {DIR.TOP, DIR.BOTTOM},
        {DIR.BOTTOM, DIR.TOP},
    };

    public static void Init()
    {
        _Nodes.Add(0, Node.NewRsc());
        _Nodes.Add(1, Node.NewRsc(Way.N(DIR.LEFT, DIR.BOTTOM), Way.N(DIR.TOP, DIR.BOTTOM), Way.N(DIR.RIGHT, DIR.BOTTOM)));
        _Nodes.Add(2, Node.NewRsc(Way.N(DIR.LEFT, DIR.RIGHT), Way.N(DIR.RIGHT, DIR.LEFT)));
        _Nodes.Add(3, Node.NewRsc(Way.N(DIR.TOP, DIR.BOTTOM)));
        _Nodes.Add(4, Node.NewRsc(Way.N(DIR.TOP, DIR.LEFT), Way.N(DIR.RIGHT, DIR.BOTTOM)));
        _Nodes.Add(5, Node.NewRsc(Way.N(DIR.TOP, DIR.RIGHT), Way.N(DIR.LEFT, DIR.BOTTOM)));
        _Nodes.Add(6, Node.NewRsc(Way.N(DIR.TOP, DIR.RIGHT), Way.N(DIR.TOP, DIR.LEFT), Way.N(DIR.LEFT, DIR.RIGHT), Way.N(DIR.RIGHT, DIR.LEFT)));
        _Nodes.Add(7, Node.NewRsc(Way.N(DIR.TOP, DIR.BOTTOM), Way.N(DIR.RIGHT, DIR.BOTTOM)));
        _Nodes.Add(8, Node.NewRsc(Way.N(DIR.LEFT, DIR.BOTTOM), Way.N(DIR.RIGHT, DIR.BOTTOM)));
        _Nodes.Add(9, Node.NewRsc(Way.N(DIR.LEFT, DIR.BOTTOM), Way.N(DIR.TOP, DIR.BOTTOM)));
        _Nodes.Add(10, Node.NewRsc(Way.N(DIR.TOP, DIR.LEFT)));
        _Nodes.Add(11, Node.NewRsc(Way.N(DIR.TOP, DIR.RIGHT)));
        _Nodes.Add(12, Node.NewRsc(Way.N(DIR.RIGHT, DIR.BOTTOM)));
        _Nodes.Add(13, Node.NewRsc(Way.N(DIR.LEFT, DIR.BOTTOM)));
    }

    private static Node GetNode(Point point)
    {
        if (_maps.GetLength(0) <= point.X || 0 > point.X)
        {
            Console.Error.WriteLine("@ERROR/(_maps.GetLength(0) <= point.X || 0 > point.X)");
            return null;
        }

        if (_maps.GetLength(1) <= point.Y || 0 > point.Y)
        {
            Console.Error.WriteLine("@ERROR/(_maps.GetLength(1) <= point.Y || 0 > point.Y)");
            return null;
        }

        return _maps[point.X, point.Y];
    }

    public static void BuildMap(int[,] mapIndexs, int exitW)
    {
        _maps = new Node[mapIndexs.GetLength(0), mapIndexs.GetLength(1)];
        _pntExit = new Point(exitW, mapIndexs.GetLength(1) - 1);

        for (int w = 0; w < mapIndexs.GetLength(0); ++w)
        {
            for (int h = 0; h < mapIndexs.GetLength(1); ++h)
            {
                int index = mapIndexs[w, h];
                Console.Error.WriteLine("BuildMap/w:" + w + "/h:" + h + "/index:" + index);
                Node nodeRsc = _Nodes[index];

                Node nodePath = Node.NewPath(nodeRsc);
                nodePath.Init(w, h);
                _maps[w, h] = nodePath;
            }
        }

        foreach (Node n in _maps)
        {
            n.SetAdjacents(_maps);
        }
    }
}

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {
        int[,] nodeMaps = null;
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int W = int.Parse(inputs[0]); // number of columns.
        int H = int.Parse(inputs[1]); // number of rows.

        Console.Error.WriteLine("W:" + W + "/H:" + H);

        Map.Init();

        nodeMaps = new int[W, H];
        for (int i = 0; i < H; i++)
        {
            string LINE = Console.ReadLine(); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
            string[] nodes = LINE.Split(' ');
            Console.Error.WriteLine("LINE:" + LINE + "/W:" + W + "/H:" + H + "/h:" + i + "/line nodes:" + nodes.Length);

            for (int j = 0; j < nodes.Length; ++j)
            {
                nodeMaps[j, i] = int.Parse(nodes[j]);
                Console.Error.WriteLine("\t nodeMaps[" + j + "," + i + "]:" + nodeMaps[j, i]);
            }
        }

        int x_Exit = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).
        Console.Error.WriteLine("nodeMaps/w count:" + nodeMaps.GetLength(0) + "/y count:" + nodeMaps.GetLength(1) + "x_Exit:" + x_Exit);

        Map.BuildMap(nodeMaps, x_Exit);

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int XI = int.Parse(inputs[0]);
            int YI = int.Parse(inputs[1]);
            string POS = inputs[2];

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");


            // One line containing the X Y coordinates of the room in which you believe Indy will be on the next turn.
            Console.WriteLine("0 0");
        }
    }
}