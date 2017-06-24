
namespace algorithms.structures
{
    // ----- Linked List Array ------------------------------------------------------
    //
    // int M
    // int Current
    // int Count
    // LinkedListArray(int m)
    // void First()
    // void Next()
    // bool Add()
    // bool Remove()
    // -------------------------------------------------------------------------
    public class LinkedListArray
    {
        int[] list = null;
        int first = 0;
        int prev = 0;
        int[] heap = null;
        int ixh = 0;
        int len = 0;
        public int M { get; private set; }
        public int Current { get; private set; }
        public int Count { get { return M - len; } }
        public LinkedListArray(int m)
        {
            M = m;
            list = new int[M];
            heap = new int[M];
            for (int i = 0; i < M; i++) heap[i] = i;
            ixh = 0;
            len = M;
            first = -1;
            prev = -1;
            Current = -1;
        }
        public void First()
        {
            prev = first;
            Current = first;
        }
        public void Next()
        {
            if (Current >= 0)
            {
                Current = list[Current];
                if (list[first] != Current) prev = list[prev];
            }
        }
        public bool Add()
        {
            if (len == 0) return false;
            int nix = heap[ixh++];
            ixh %= M;
            len--;
            if (Current >= 0)
            {
                list[nix] = list[Current];
                list[Current] = nix;
            }
            else
            {
                list[nix] = first;
                first = nix;
            }
            prev = Current;
            Current = nix;
            return true;
        }
        public bool Remove()
        {
            if (Current < 0) return false;
            int ix = list[Current];
            if (Current == first)
            {
                first = list[ix];
                Current = first;
                prev = first;
            }
            else
            {
                list[prev] = list[Current];
                Current = list[Current];
            }
            heap[(ixh + len) % M] = ix;
            len++;
            return true;
        }
    }
    // -------------------------------------------------------------------------
}
