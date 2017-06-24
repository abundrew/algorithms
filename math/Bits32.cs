namespace algorithms.math
{
    public static class Bits32
    {
        // ----- Bits32 --------------------------------------------------------
        //
        // int MarkBit(int bits, int bit)
        // int ClearBit(int bits, int bit)
        // bool IsMarked(int bits, int bit)
        // uint RemoveBit(uint bits, int bit)
        // ---------------------------------------------------------------------
        public static int MarkBit(int bits, int bit)
        {
            return bits | (1 << bit);
        }
        public static int ClearBit(int bits, int bit)
        {
            return bits & ~(1 << bit);
        }
        public static bool IsMarked(int bits, int bit)
        {
            return (bits & (1 << bit)) != 0;
        }
        static uint[] B32 = new uint[] {
            0x1, 0x2, 0x4, 0x8,
            0x10, 0x20, 0x40, 0x80,
            0x100, 0x200, 0x400, 0x800,
            0x1000, 0x2000, 0x4000, 0x8000,
            0x10000, 0x20000, 0x40000, 0x80000,
            0x100000, 0x200000, 0x400000, 0x800000,
            0x1000000, 0x2000000, 0x4000000, 0x8000000,
            0x10000000, 0x20000000, 0x40000000, 0x80000000,
        };
        static uint[] F32 = new uint[] {
            0x0,
            0x1, 0x3, 0x7, 0xF,
            0x1F, 0x3F, 0x7F, 0xFF,
            0x1FF, 0x3FF, 0x7FF, 0xFFF,
            0x1FFF, 0x3FFF, 0x7FFF, 0xFFFF,
            0x1FFFF, 0x3FFFF, 0x7FFFF, 0xFFFFF,
            0x1FFFFF, 0x3FFFFF, 0x7FFFFF, 0xFFFFFF,
            0x1FFFFFF, 0x3FFFFFF, 0x7FFFFFF, 0xFFFFFFF,
            0x1FFFFFFF, 0x3FFFFFFF, 0x7FFFFFFF, 0xFFFFFFFF
        };
        static uint RemoveBit(uint bits, int bit)
        {
            return ((bits >> 1) & (~F32[bit])) | (bits & F32[bit]);
        }
        static uint ReverseBits(uint bits, int n)
        {
            uint revs = 0;
            for (int i = 0; i < n; i++)
                if ((bits & B32[i]) == B32[i]) revs |= B32[n - 1 - i];
            return revs;
        }
        // ---------------------------------------------------------------------
    }
}
