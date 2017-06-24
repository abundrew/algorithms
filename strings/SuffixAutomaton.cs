using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.strings
{
    // ----- Suffix Automation -------------------------------------------------
    //
    // class Node
    // void PutNext(char c, Node to)
    // bool ContainsKeyNext(char c)
    // Node GetNext(char c)
    // List<string> Suffixes(char[] s)
    //
    // static SuffixAutomaton Build(char[] str)
    // Node Extend(Node last, char c)
    // SuffixAutomaton LexSort()
    // SuffixAutomaton SortTopologically()
    // long NumberOfDifferentSubstrings()
    // string ToString()
    // -------------------------------------------------------------------------
    public class SuffixAutomaton
    {
        public class Node
        {
            public int id;
            public int len;
            public char key;
            public Node link;
            public Node[] next = new Node[3];
            public Node original;
            public int np = 0;
            public int hit = 0;
            public void PutNext(char c, Node to)
            {
                to.key = c;
                if (hit << ~(c - 'a') < 0)
                {
                    for (int i = 0; i < np; i++)
                    {
                        if (next[i].key == c)
                        {
                            next[i] = to;
                            return;
                        }
                    }
                }
                hit |= 1 << c - 'a';
                if (np == next.Length)
                {
                    Node[] next2 = new Node[np * 2];
                    Array.Copy(next, next2, next.Length);
                    next = next2;
                }
                next[np++] = to;
            }
            public bool ContainsKeyNext(char c)
            {
                return hit << ~(c - 'a') < 0;
            }
            public Node GetNext(char c)
            {
                if (hit << ~(c - 'a') < 0)
                {
                    for (int i = 0; i < np; i++)
                    {
                        if (next[i].key == c) return next[i];
                    }
                }
                return null;
            }
            public List<string> Suffixes(char[] s)
            {
                List<string> list = new List<string>();
                if (id == 0) return list;
                int first = original != null ? original.len : len;
                for (int i = link.len + 1; i <= len; i++)
                {
                    list.Add(new string(s, first - i, i));
                }
                return list;
            }
        }
        public Node t0;
        public int len;
        public Node[] nodes;
        public int gen;
        public bool sortedTopologically = false;
        public bool lexsorted = false;
        private SuffixAutomaton(int n)
        {
            gen = 0;
            nodes = new Node[2 * n];
            t0 = MakeNode(0, null);
        }
        private Node MakeNode(int len, Node original)
        {
            Node node = new Node();
            node.id = gen;
            node.original = original;
            node.len = len;
            nodes[gen++] = node;
            return node;
        }
        public static SuffixAutomaton Build(char[] str)
        {
            int n = str.Length;
            SuffixAutomaton sa = new SuffixAutomaton(n);
            sa.len = str.Length;
            Node last = sa.t0;
            foreach (char c in str)
            {
                last = sa.Extend(last, c);
            }
            return sa;
        }
        public Node Extend(Node last, char c)
        {
            Node cur = MakeNode(last.len + 1, null);
            Node p;
            for (p = last; p != null && !p.ContainsKeyNext(c); p = p.link)
            {
                p.PutNext(c, cur);
            }
            if (p == null)
            {
                cur.link = t0;
            }
            else
            {
                Node q = p.GetNext(c);
                if (p.len + 1 == q.len)
                {
                    cur.link = q;
                }
                else
                {
                    Node clone = MakeNode(p.len + 1, q);
                    clone.next = new Node[q.next.Length];
                    Array.Copy(q.next, clone.next, q.next.Length);
                    clone.hit = q.hit;
                    clone.np = q.np;
                    clone.link = q.link;
                    for (; p != null && q.Equals(p.GetNext(c)); p = p.link)
                    {
                        p.PutNext(c, clone);
                    }
                    q.link = cur.link = clone;
                }
            }
            return cur;
        }
        class LexComparer : IComparer<Node>
        {
            public int Compare(Node a, Node b)
            {
                return a.key.CompareTo(b.key);
            }
        }
        public SuffixAutomaton LexSort()
        {
            for (int i = 0; i < gen; i++)
            {
                Node node = nodes[i];
                Array.Sort(node.next, 0, node.np, new LexComparer());
            }
            lexsorted = true;
            return this;
        }
        public SuffixAutomaton SortTopologically()
        {
            int[] indeg = new int[gen];
            for (int i = 0; i < gen; i++)
            {
                for (int j = 0; j < nodes[i].np; j++)
                {
                    indeg[nodes[i].next[j].id]++;
                }
            }
            Node[] sorted = new Node[gen];
            sorted[0] = t0;
            int p = 1;
            for (int i = 0; i < gen; i++)
            {
                Node cur = sorted[i];
                for (int j = 0; j < cur.np; j++)
                {
                    if (--indeg[cur.next[j].id] == 0)
                    {
                        sorted[p++] = cur.next[j];
                    }
                }
            }
            for (int i = 0; i < gen; i++) sorted[i].id = i;
            nodes = sorted;
            sortedTopologically = true;
            return this;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Node n in nodes)
            {
                if (n != null)
                {
                    sb.Append(string.Format("{{id:{0}, len:{1}, link:{2}, cloned:{3}, ",
                            n.id,
                            n.len,
                            n.link != null ? n.link.id.ToString() : "",
                            n.original != null ? n.original.id.ToString() : ""
                    ));
                    sb.Append("next:{{");
                    for (int i = 0; i < n.np; i++)
                    {
                        sb.Append(n.next[i].key + ":" + n.next[i].id + ",");
                    }
                    sb.Append("}}");
                    sb.Append("}}");
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
    }
    // -------------------------------------------------------------------------
}
