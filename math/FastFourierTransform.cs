using System;
using System.Linq;

namespace algorithms.math
{
    public static class FastFourierTransform
    {
        // ----- Fast Fourier Transform ----------------------------------------
        //
        // Uses Cooley-Tukey iterative in-place algorithm with radix-2 DIT case
        // assumes no of points provided are a power of 2
        //
        // Depends on:
        // -- Complex (algorithms.math)
        //
        // void FFT(Complex[] buffer, int n, bool invert = false)
        // void FFT(Complex[] buffer, bool invert)
        // int[] FFTMultiply(int[] a, int na, int[] b, int nb)
        // int[] FFTMultiply(int[] a, int[] b)
        // ---------------------------------------------------------------------
        public static void FFT(Complex[] buffer, int n, bool invert = false)
        {
            int dig = 0;
            while ((1 << dig) < n) dig++;
            int[] rev = new int[n];
            for (int i = 0; i < n; i++)
            {
                rev[i] = (rev[i >> 1] >> 1) | ((i & 1) << (dig - 1));
                if (rev[i] > i)
                {
                    var temp = buffer[i];
                    buffer[i] = buffer[rev[i]];
                    buffer[rev[i]] = temp;
                }
            }
            for (int len = 2; len <= n; len <<= 1)
            {
                double angle = 2 * Math.PI / len;
                if (invert) angle *= -1;
                Complex wgo = new Complex(Math.Cos(angle), Math.Sin(angle));
                for (int i = 0; i < n; i += len)
                {
                    Complex w = new Complex(1);
                    for (int j = 0; j < (len >> 1); j++)
                    {
                        Complex a = buffer[i + j];
                        Complex b = w * buffer[i + j + (len >> 1)];
                        buffer[i + j] = a + b;
                        buffer[i + j + (len >> 1)] = a - b;
                        w *= wgo;
                    }
                }
            }
            if (invert)
                for (int i = 0; i < n; i++) buffer[i] /= n;
        }
        public static void FFT(Complex[] buffer, bool invert = false)
        {
            FFT(buffer, buffer.Length, invert);
        }
        public static int[] FFTMultiply(int[] a, int na, int[] b, int nb)
        {
            int sz = Math.Max(na, nb);
            int n = 1;
            while (n < sz) n <<= 1;
            n <<= 1;
            Complex[] fa = new Complex[n];
            for (int i = 0; i < na; i++) fa[i] = new Complex(a[i]);
            for (int i = na; i < n; i++) fa[i] = Complex.Zero;
            Complex[] fb = new Complex[n];
            for (int i = 0; i < nb; i++) fb[i] = new Complex(b[i]);
            for (int i = nb; i < n; i++) fb[i] = Complex.Zero;
            FFT(fa);
            FFT(fb);
            for (int i = 0; i < n; i++) fa[i] *= fb[i];
            FFT(fa, true);
            return fa.Select(p => (int)(p.Re < 0 ? p.Re - 0.5 : p.Re + 0.5)).ToArray();
        }
        public static int[] FFTMultiply(int[] a, int[] b)
        {
            return FFTMultiply(a, a.Length, b, b.Length);
        }
        // ---------------------------------------------------------------------
    }
}
