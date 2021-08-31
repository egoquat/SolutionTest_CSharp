using System;
using System.Collections.Generic;
using System.Linq;

class NodePathSimple
{
    public const int IDXNONE = -1;
    public int Idx = IDXNONE;
    public List<int> Adjs = new List<int>();
    public void AddAdj(int idx) { bool isAdded = Adjs.AddNotOverlap(idx); }
    void GetPathSimpleR(int idxTarget, ref List<int> path)
    {
        if (idxTarget == Idx)
        {
            path.Add(Idx);
            return;
        }
        if (Adjs.Count <= 0)
            return;
        for (int i = 0; i < Adjs.Count; ++i)
        {
            NodePathSimple adj = Nodes.GetOrDefault(Adjs[i]);
            if (true == path.Contains(adj.Idx)) continue;
            adj.GetPathSimpleR(idxTarget, ref path);
            if (true == path.Contains(idxTarget))
                return;
        }
    }
    NodePathSimple(int idx) { Idx = idx; }
    public static List<NodePathSimple> Nodes = new List<NodePathSimple>();
    static NodePathSimple GetNode(int idx) { return Nodes.GetOrDefault(idx); }
    public static bool GetPathSimple(int idxStart, int idxTarget, ref List<int> path)
    {
        GetNode(idxStart).GetPathSimpleR(idxTarget, ref path);
        return path.Count >= 2 && path[path.Count-1] == idxTarget;
    }
}
