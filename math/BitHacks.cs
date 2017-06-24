namespace algorithms.math
{
    public static class BitHacks
    {
        // ----- Bit Hacks -----------------------------------------------------
        //
        // http://grepcode.com/file/repository.grepcode.com/java/root/jdk/openjdk/6-b14/java/lang/Long.java
        //
        // ulong highestOneBit(ulong i)
        // long lowestOneBit(long i)
        // int numberOfLeadingZeros(long i)
        // int numberOfTrailingZeros(long i)
        // int bitCount(long i)
        // long rotateLeft(long i, int distance)
        // long rotateRight(long i, int distance)
        // long reverse(long i)
        // long reverseBytes(long i)
        //
        // bool is_a_power_of_2(uint v)
        // uint lexicographically_next_bit_permutation(uint v)
        // ---------------------------------------------------------------------
        public static ulong highestOneBit(ulong i)
        {
            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);
            i |= (i >> 32);
            return i - (i >> 1);
        }
        public static long lowestOneBit(long i)
        {
            return i & -i;
        }
        public static int numberOfLeadingZeros(long i)
        {
            if (i == 0) return 64;
            int n = 1;
            uint x = (uint)((ulong)i >> 32);
            if (x == 0) { n += 32; x = (uint)i; }
            if (x >> 16 == 0) { n += 16; x <<= 16; }
            if (x >> 24 == 0) { n += 8; x <<= 8; }
            if (x >> 28 == 0) { n += 4; x <<= 4; }
            if (x >> 30 == 0) { n += 2; x <<= 2; }
            n -= (int)(x >> 31);
            return n;
        }
        public static int numberOfTrailingZeros(long i)
        {
            uint x, y;
            if (i == 0) return 64;
            int n = 63;
            y = (uint)i; if (y != 0) { n = n - 32; x = y; } else x = (uint)(i >> 32);
            y = x << 16; if (y != 0) { n = n - 16; x = y; }
            y = x << 8; if (y != 0) { n = n - 8; x = y; }
            y = x << 4; if (y != 0) { n = n - 4; x = y; }
            y = x << 2; if (y != 0) { n = n - 2; x = y; }
            return n - (int)((x << 1) >> 31);
        }
        public static int bitCount(long i)
        {
            i = i - (long)(((ulong)i >> 1) & 0x5555555555555555L);
            i = (i & 0x3333333333333333L) + (long)(((ulong)i >> 2) & 0x3333333333333333L);
            i = (i + (long)((ulong)i >> 4)) & 0x0f0f0f0f0f0f0f0fL;
            i = i + (long)((ulong)i >> 8);
            i = i + (long)((ulong)i >> 16);
            i = i + (long)((ulong)i >> 32);
            return (int)i & 0x7f;
        }
        public static long rotateLeft(long i, int distance)
        {
            return (i << distance) | (long)((ulong)i >> -distance);
        }
        public static long rotateRight(long i, int distance)
        {
            return (long)((ulong)i >> distance) | (i << -distance);
        }
        public static long reverse(long i)
        {
            i = (i & 0x5555555555555555L) << 1 | (long)((ulong)i >> 1) & 0x5555555555555555L;
            i = (i & 0x3333333333333333L) << 2 | (long)((ulong)i >> 2) & 0x3333333333333333L;
            i = (i & 0x0f0f0f0f0f0f0f0fL) << 4 | (long)((ulong)i >> 4) & 0x0f0f0f0f0f0f0f0fL;
            i = (i & 0x00ff00ff00ff00ffL) << 8 | (long)((ulong)i >> 8) & 0x00ff00ff00ff00ffL;
            i = (i << 48) | ((i & 0xffff0000L) << 16) | (long)(((ulong)i >> 16) & 0xffff0000L) | (long)((ulong)i >> 48);
            return i;
        }
        public static long reverseBytes(long i)
        {
            i = (i & 0x00ff00ff00ff00ffL) << 8 | (long)((ulong)i >> 8) & 0x00ff00ff00ff00ffL;
            return (i << 48) | ((i & 0xffff0000L) << 16) | (long)(((ulong)i >> 16) & 0xffff0000L) | (long)((ulong)i >> 48);
        }
        public static bool is_a_power_of_2(uint v)
        {
            return (v & (v - 1)) == 0;
        }
        public static uint lexicographically_next_bit_permutation(uint v)
        {
            uint t = (v | (v - 1)) + 1;
            return t | (uint)((((t & -t) / (v & -v)) >> 1) - 1);
        }
        // ---------------------------------------------------------------------
    }
}
