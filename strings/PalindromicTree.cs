using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.strings
{
    public class PalindromicTree
    {
        const int MAXN = 105000;

        struct Node
        {
            public int[] Next { get; set; }
            public int Len { get; set; }
            public int SuffLink { get; set; }
            public int Num { get; set; }
        };

        int len;
        char[] s = new char[MAXN];
        Node[] tree = new Node[MAXN];
        int num;            // node 1 - root with len -1, node 2 - root with len 0
        int suff;           // max suffix palindrome
        long ans;

        bool addLetter(int pos)
        {
            int cur = suff, curlen = 0;
            int let = s[pos] - 'a';

            while (true)
            {
                curlen = tree[cur].len;
                if (pos - 1 - curlen >= 0 && s[pos - 1 - curlen] == s[pos])
                    break;
                cur = tree[cur].sufflink;
            }
            if (tree[cur].next[let])
            {
                suff = tree[cur].next[let];
                return false;
            }

            num++;
            suff = num;
            tree[num].len = tree[cur].len + 2;
            tree[cur].next[let] = num;

            if (tree[num].len == 1)
            {
                tree[num].sufflink = 2;
                tree[num].num = 1;
                return true;
            }

            while (true)
            {
                cur = tree[cur].sufflink;
                curlen = tree[cur].len;
                if (pos - 1 - curlen >= 0 && s[pos - 1 - curlen] == s[pos])
                {
                    tree[num].sufflink = tree[cur].next[let];
                    break;
                }
            }

            tree[num].num = 1 + tree[tree[num].sufflink].num;

            return true;
        }

        void initTree()
        {
            num = 2; suff = 2;
            tree[1].len = -1; tree[1].sufflink = 1;
            tree[2].len = 0; tree[2].sufflink = 1;
        }

    }
}
