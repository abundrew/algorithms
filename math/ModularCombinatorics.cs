namespace algorithms.math
{
    public static class ModularCombinatorics
    {
        // ----- Modular Combinatorics -----------------------------------------
        //
        // int[][] FiF(int nmax, int modulus)
        // long Choose(int n, int r, int[][] fif, int modulus)
        // ---------------------------------------------------------------------
        public static int[][] FiF(int nmax, int modulus)
        {
            int[][] fif = new int[2][];
            fif[0] = new int[nmax + 1];
            fif[0][0] = 1;
            for (int i = 1; i <= nmax; i++) fif[0][i] = (int)(((long)fif[0][i - 1] * i) % modulus);
            long a = fif[0][nmax];
            long b = modulus;
            long p = 1;
            long q = 0;
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
            fif[1] = new int[nmax + 1];
            fif[1][nmax] = (int)(p < 0 ? p + modulus : p);
            for (int i = nmax - 1; i >= 0; i--) fif[1][i] = (int)(((long)fif[1][i + 1] * (i + 1)) % modulus);
            return fif;
        }
        public static long Choose(int n, int r, int[][] fif, int modulus)
        {
            if (n < 0 || r < 0 || r > n) return 0;
            long factn = fif[0][n];
            long invFactr = fif[1][r];
            long invFactnr = fif[1][n - r];

            long c1 = ((long)fif[0][n] * fif[1][r]) % modulus;
            long c2 = (c1 * fif[1][n - r]) % modulus;

            return ((((long)fif[0][n] * fif[1][r]) % modulus) * fif[1][n - r]) % modulus;
        }
        // ---------------------------------------------------------------------
    }
}
