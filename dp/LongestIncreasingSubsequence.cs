namespace algorithms.dp
{
    public static class LongestIncreasingSubsequence
    {
        // ----- Longest Increasing Subsequence --------------------------------
        //
        // -- O(nlogn)
        //
        // int[] LIS(int[] A)
        //
        // non decreasing "if (A[increasingSub[mid]] <= A[i])" ?!
        // ---------------------------------------------------------------------
        public static int[] LIS(int[] A)
        {
            int[] parent = new int[A.Length];            //Tracking the predecessors/parents of elements of each subsequence.
            int[] increasingSub = new int[A.Length + 1]; //Tracking ends of each increasing subsequence.
            int length = 0;                              //Length of longest subsequence.
            for (int i = 0; i < A.Length; i++)
            {
                //Binary search
                int low = 1;
                int high = length;
                while (low <= high)
                {
                    int mid = (low + high + 1) / 2;
                    if (A[increasingSub[mid]] < A[i])
                        low = mid + 1;
                    else
                        high = mid - 1;
                }
                int pos = low;
                //update parent/previous element for LIS
                parent[i] = increasingSub[pos - 1];
                //Replace or append
                increasingSub[pos] = i;
                //Update the length of the longest subsequence.
                if (pos > length) length = pos;
            }
            //Generate LIS by traversing parent array
            int[] lis = new int[length];
            int k = increasingSub[length];
            for (int j = length - 1; j >= 0; j--)
            {
                lis[j] = A[k];
                k = parent[k];
            }
            return lis;
        }
        // ---------------------------------------------------------------------
    }
}
