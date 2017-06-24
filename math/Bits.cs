namespace algorithms.math
{
    public static class Bits
    {
        // ----- Bits ----------------------------------------------------------
        //
        // int[] BitsArray(int nbits)
        // void MarkBit(int[] bits, int bit)
        // void ClearBit(int[] bits, int bit)
        // bool IsMarked(int[] bits, int bit)
        //
        // void MarkBit(ref int bits, int bit)
        // void ClearBit(ref int bits, int bit)
        // bool IsMarked(ref int bits, int bit)
        // ---------------------------------------------------------------------
        public static int[] BitsArray(int nbits)
        {
            return new int[(nbits - 1) / 32 + 1];
        }
        public static void MarkBit(int[] bits, int bit)
        {
            bits[bit / 32] |= (1 << (bit % 32));
        }
        public static void ClearBit(int[] bits, int bit)
        {
            bits[bit / 32] &= ~(1 << (bit % 32));
        }
        public static bool IsMarked(int[] bits, int bit)
        {
            return (bits[bit / 32] & (1 << (bit % 32))) != 0;
        }
        public static void MarkBit(ref int bits, int bit)
        {
            bits |= (1 << bit);
        }
        public static void ClearBit(ref int bits, int bit)
        {
            bits &= ~(1 << bit);
        }
        public static bool IsMarked(ref int bits, int bit)
        {
            return (bits & (1 << bit)) != 0;
        }
        // ---------------------------------------------------------------------
    }
}
