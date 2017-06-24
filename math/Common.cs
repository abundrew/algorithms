namespace algorithms.math
{
    public static class Common
    {
        // ----- Common --------------------------------------------------------
        //
        // int gcd(int a, int b)
        // ---------------------------------------------------------------------
        public static int gcd(int a, int b)
        {
            while (b != 0) b = a % (a = b);
            return a;
        }
        // ---------------------------------------------------------------------
    }
}
