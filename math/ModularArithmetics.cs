namespace algorithms.math
{
    public static class ModularArithmetics
    {
        // ----- Modular Arithmetics -------------------------------------------
        //
        // long FastFib(long n, int modulus)
        // long Multiply(long a, long b, int modulus)
        // long Divide(long a, long b, int modulus)
        // long Power(long a, long b, int modulus)
        // long Choose(long n, int r, int modulus)
        // ---------------------------------------------------------------------
        // Fast doubling algorithm
        public static long FastFib(long n, int modulus)
        {
            int k = n > int.MaxValue ? 63 : 31;

            long a = 0;
            long b = 1;
            for (int i = k; i >= 0; i--)
            {
                long d = a * (b * 2 - a + modulus);
                long e = a * a + b * b;
                a = d % modulus;
                b = e % modulus;
                if ((((ulong)n >> i) & 1) != 0)
                {
                    long c = a + b;
                    a = b;
                    b = c % modulus;
                }
            }
            return a;
        }
        public static long Multiply(long a, long b, int modulus)
        {
            return (a * b) % modulus;
        }
        public static long Divide(long a, long b, int modulus)
        {
            return (a * Inverse(b, modulus)) % modulus;
        }
        public static long Power(long a, long b, int modulus)
        {
            long res = 1;
            while (b > 0)
            {
                if ((b & 1) == 1) res = (res * a) % modulus;
                a = (a * a) % modulus;
                b >>= 1;
            }
            return res;
        }
        public static long Choose(long n, int r, int modulus)
        {
            long choose = 1;
            while (true)
            {
                if (r == 0) break;
                long N = n % modulus;
                int R = r % modulus;
                if (N < R) return 0;

                for (int i = 0; i < R; i++)
                    choose = (choose * (N - i)) % modulus;
                long factR = 1;
                for (int i = 0; i < R; i++)
                    factR = (factR * (i + 1)) % modulus;
                choose = Divide(choose, factR, modulus);
                if (choose < 0) choose += modulus;
                n /= modulus;
                r /= modulus;
            }
            return choose;
        }
        static long Inverse(long a, int modulus)
        {
            long b = modulus;
            long p = 1, q = 0;
            while (b > 0)
            {
                long c = a / b;
                long d = a;
                a = b;
                b = d % b;
                d = p;
                p = q;
                q = d - c * q;
            }
            return p < 0 ? p + modulus : p;
        }
        // ---------------------------------------------------------------------
    }
}
