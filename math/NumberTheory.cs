using System;
using System.Collections.Generic;

namespace algorithms.math
{
    public static class NumberTheory
    {
        // ----- Number Theory -------------------------------------------------
        //
        // Depends on:
        // -- ModularArithmetics (algorithms.math)
        //
        // List<int> Primes(int limit)
        // Dictionary<int, int> PrimeFactors(long n, List<int> primes)
        // long Phi(long n, List<int> primes)
        // bool IsPrimitiveRoot(long prime, long root, List<int> primes)
        // ---------------------------------------------------------------------
        public static List<int> Primes(int limit)
        {
            List<int> primes = new List<int>();
            for (int k = 2; k <= limit; k++)
            {
                bool prime = true;
                int rk = (int)Math.Sqrt(k);
                foreach (int p in primes)
                {
                    if (rk < p) break;
                    if (k % p == 0)
                    {
                        prime = false;
                        break;
                    }
                }
                if (prime) primes.Add(k);
            }
            return primes;
        }
        public static Dictionary<int, int> PrimeFactors(long n, List<int> primes)
        {
            Dictionary<int, int> pk = new Dictionary<int, int>();
            int nr = (int)Math.Sqrt(n);
            foreach (int p in primes)
            {
                if (n == 1 || p > nr) break;
                if (n % p == 0)
                {
                    pk[p] = 0;
                    while (n % p == 0)
                    {
                        pk[p]++;
                        n /= p;
                    }
                }
            }
            if (n > 1) pk[(int)n] = 1;
            return pk;
        }
        public static long Phi(long n, List<int> primes)
        {
            long phi = n;
            foreach (int p in PrimeFactors(n, primes).Keys)
                phi = phi / p * (p - 1);
            return phi;
        }
        public static bool IsPrimitiveRoot(long prime, long root, List<int> primes)
        {
            foreach (int p in PrimeFactors(prime - 1, primes).Keys)
                if (ModularArithmetics.Power(root, prime / p, (int)prime) == 1) return false;
            return true;
        }
        // ---------------------------------------------------------------------
        static bool Suspect(long b, int t, long u, long n)
        {
            long prod = 1;
            while (u > 0)
            {
                if ((u & 1) == 1) prod = (prod * b) % n;
                b = (b * b) % n;
                u /= 2;
            }
            if (prod == 1) return true;
            for (int i = 0; i < t; i++)
            {
                if (prod == n - 1) return true;
                prod = (prod * prod) % n;
            }
            return false;
        }
        // up to 2^32-1
        public static bool IsPrime(long n)
        {
            long k = n - 1;
            int t = 0;
            while (k % 2 == 0) { t++; k /= 2; }
            if (n > 2 && n % 2 == 0) return false;
            if (n > 3 && n % 3 == 0) return false;
            if (n > 5 && n % 5 == 0) return false;
            if (n > 7 && n % 7 == 0) return false;
            return Suspect(61, t, k, n) && Suspect(7, t, k, n) && Suspect(2, t, k, n);
        }
        // up to 3*10^14
        public static bool IsPrime314(long n)
        {
            if (n < 20 && (n == 2 || n == 3 || n == 5 || n == 7 || n == 11 || n == 13 || n == 17 || n == 19)) return true;

            long k = n - 1;
            int t = 0;
            while (k % 2 == 0) { t++; k /= 2; }
            if (n > 2 && n % 2 == 0) return false;
            if (n > 3 && n % 3 == 0) return false;
            if (n > 5 && n % 5 == 0) return false;
            if (n > 7 && n % 7 == 0) return false;
            return Suspect(19, t, k, n) && Suspect(17, t, k, n) && Suspect(13, t, k, n) && Suspect(11, t, k, n) && Suspect(7, t, k, n) && Suspect(5, t, k, n) && Suspect(3, t, k, n) && Suspect(2, t, k, n);
        }
        // ---------------------------------------------------------------------
    }
}
