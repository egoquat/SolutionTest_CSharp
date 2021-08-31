using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class NodeNetwork
{
    public const int IDXNONE = -1;
    public int Idx = IDXNONE;
    public List<int> Adjs = new List<int>();
    public void AddAdj(int idx) { bool isAdded = Adjs.AddNotOverlap(idx); }
    void GetNetworkR(ref List<int> network)
    {
        if (Adjs.Count <= 0)
            return;
        for (int i = 0; i < Adjs.Count; ++i)
        {
            NodeNetwork adj = Nodes.GetOrDefault(Adjs[i]);
            if (true == network.Contains(adj.Idx)) continue;
            adj.GetNetworkR(ref network);
        }
    }
    NodeNetwork(int idx) { Idx = idx; }
    public static List<NodeNetwork> Nodes = new List<NodeNetwork>();
    static NodeNetwork GetNode(int idx) { return Nodes.GetOrDefault(idx); }
    public static bool GetNetwork(int idxStart, ref List<int> network)
    {
        GetNode(idxStart).GetNetworkR(ref network);
        network.Sort();
        return network.Count >= 2;
    }
}