using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.math
{
    public static class Combinatorics
    {
        // ----- Combinatorics -------------------------------------------------
        //
        // IEnumerable<T[]> AllFor<T>(T[] array)
        // IEnumerable<int[]> Combinations(int m, int n)
        // https://en.wikipedia.org/wiki/Heap%27s_algorithm
        // void Heap<T>(T[] A, Action doPermutation)
        // ---------------------------------------------------------------------
        public static IEnumerable<T[]> Permutations<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                yield return new T[0];
            }
            else
            {
                for (int pick = 0; pick < array.Length; ++pick)
                {
                    T item = array[pick];
                    int i = -1;
                    T[] rest = System.Array.FindAll<T>(
                        array, delegate (T p) { return ++i != pick; }
                    );
                    foreach (T[] restPermuted in Permutations(rest))
                    {
                        i = -1;
                        yield return System.Array.ConvertAll<T, T>(
                            array,
                            delegate (T p)
                            {
                                return ++i == 0 ? item : restPermuted[i - 1];
                            }
                        );
                    }
                }
            }
        }
        public static IEnumerable<int[]> Combinations(int m, int n)
        {
            int[] result = new int[m];
            Stack<int> stack = new Stack<int>();
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();

                while (value < n)
                {
                    result[index++] = ++value;
                    stack.Push(value);

                    if (index == m)
                    {
                        yield return result;
                        break;
                    }
                }
            }
        }
        public static void Heap<T>(T[] A, Action doPermutation)
        {
            int N = A.Length;
            int[] C = new int[N];
            doPermutation();
            int i = 0;
            while (i < N)
                if (C[i] < i)
                {
                    int j = i % 2 == 0 ? 0 : C[i];
                    T t = A[j]; A[j] = A[i]; A[i] = t;
                    doPermutation();
                    C[i]++;
                    i = 0;
                }
                else
                {
                    C[i] = 0;
                    i++;
                }
        }
        // ---------------------------------------------------------------------
    }
}
