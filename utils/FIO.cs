using System;
using System.IO;
using System.Text;

namespace algorithms.utils
{
    // ----- FastIO ------------------------------------------------------------
    //
    // FIO()
    // FIO(Stream stdin)
    // void Close()
    // void Dispose()
    // string ReadToEnd()
    // string ReadLine()
    // string ReadToken()
    // int ReadInt()
    // long ReadLong()
    // decimal ReadDecimal()
    // double ReadDouble()
    // int[] ReadIntArr(int n)
    // long[] ReadLongArr(int n)
    // decimal[] ReadDecimalArr(int n)
    // double[] ReadDoubleArr(int n)
    // void Write(string value)
    // void WriteLine(string value)
    // -------------------------------------------------------------------------
    public class FIO: IDisposable
    {
        const int BUFFER_SIZE = 4096;
        byte[] byteBuffer;
        char[] charBuffer;
        int byteBufferSize;
        int charBufferSize;
        int charPos;
        int charLen;
        int byteLen;
        Stream stm = null;
        StreamWriter sw = null;
        public FIO()
        {
            stm = Console.OpenStandardInput();
            sw = new StreamWriter(Console.OpenStandardOutput());
            Init();
        }
        public FIO(Stream stdin)
        {
            stm = stdin;
            sw = new StreamWriter(Console.OpenStandardOutput());
            Init();
        }
        public void Close()
        {
            Dispose();
        }
        public void Dispose()
        {
            try
            {
                stm.Close();
                sw.Dispose();
            }
            finally
            {
                if (stm != null)
                {
                    stm = null;
                    byteBuffer = null;
                    charBuffer = null;
                    charPos = 0;
                    charLen = 0;
                }
            }
        }
        void Init()
        {
            byteBufferSize = BUFFER_SIZE;
            byteBuffer = new byte[byteBufferSize];
            charBufferSize = Encoding.UTF8.GetMaxCharCount(byteBufferSize);
            charBuffer = new char[charBufferSize];
            charPos = 0;
            charLen = 0;
            byteLen = 0;
        }
        int ReadBuffer()
        {
            charLen = 0;
            charPos = 0;
            byteLen = 0;
            do
            {
                byteLen = stm.Read(byteBuffer, 0, byteBuffer.Length);
                if (byteLen == 0) return charLen;
                charLen += Encoding.UTF8.GetChars(byteBuffer, 0, byteLen, charBuffer, charLen);
            } while (charLen == 0);
            return charLen;
        }
        public string ReadToEnd()
        {
            StringBuilder sb = new StringBuilder(charLen - charPos);
            do
            {
                sb.Append(charBuffer, charPos, charLen - charPos);
                charPos = charLen;
                ReadBuffer();
            } while (charLen > 0);
            return sb.ToString();
        }
        public string ReadLine()
        {
            if (charPos == charLen)
            {
                if (ReadBuffer() == 0) return null;
            }
            StringBuilder sb = null;
            do
            {
                int i = charPos;
                do
                {
                    char ch = charBuffer[i];
                    if (ch == '\r' || ch == '\n')
                    {
                        string s;
                        if (sb != null)
                        {
                            sb.Append(charBuffer, charPos, i - charPos);
                            s = sb.ToString();
                        }
                        else
                        {
                            s = new string(charBuffer, charPos, i - charPos);
                        }
                        charPos = i + 1;
                        if (ch == '\r' && (charPos < charLen || ReadBuffer() > 0))
                        {
                            if (charBuffer[charPos] == '\n') charPos++;
                        }
                        return s;
                    }
                    i++;
                } while (i < charLen);
                i = charLen - charPos;
                if (sb == null) sb = new StringBuilder(i + 80);
                sb.Append(charBuffer, charPos, i);
            } while (ReadBuffer() > 0);
            return sb.ToString();
        }
        public string ReadToken()
        {
            if (charPos == charLen)
            {
                if (ReadBuffer() == 0) return null;
            }
            StringBuilder sb = null;
            do
            {
                int i = charPos;
                do
                {
                    char ch = charBuffer[i];
                    if (ch == '\r' || ch == '\n' || ch == ' ')
                    {
                        string s;
                        if (sb != null)
                        {
                            sb.Append(charBuffer, charPos, i - charPos);
                            s = sb.ToString();
                        }
                        else
                        {
                            s = new string(charBuffer, charPos, i - charPos);
                        }
                        charPos = i + 1;
                        if (ch == '\r' && (charPos < charLen || ReadBuffer() > 0))
                        {
                            if (charBuffer[charPos] == '\n' || charBuffer[charPos] == ' ') charPos++;
                        }
                        if (!string.IsNullOrEmpty(s)) return s;
                        i = charPos;
                    }
                    i++;
                } while (i < charLen);
                i = charLen - charPos;
                if (sb == null) sb = new StringBuilder(i + 80);
                sb.Append(charBuffer, charPos, i);
            } while (ReadBuffer() > 0);
            return sb.ToString();
        }
        long ReadLongToken()
        {
            long y = 0;
            bool none = true;
            if (charPos == charLen)
            {
                if (ReadBuffer() == 0) return 0;
            }
            do
            {
                int i = charPos;
                do
                {
                    char ch = charBuffer[i];
                    if (ch == '\r' || ch == '\n' || ch == ' ')
                    {
                        for (int j = 0; j < (i - charPos); j++)
                        {
                            y = y * 10 + (charBuffer[j + charPos] - '0');
                            none = false;
                        }
                        charPos = i + 1;
                        if (ch == '\r' && (charPos < charLen || ReadBuffer() > 0))
                        {
                            if (charBuffer[charPos] == '\n' || charBuffer[charPos] == ' ') charPos++;
                        }
                        if (!none) return y;
                        i = charPos;
                    }
                    i++;
                } while (i < charLen);
                i = charLen - charPos;
                for (int j = 0; j < i; j++)
                {
                    y = y * 10 + (charBuffer[j + charPos] - '0');
                    none = false;
                }
            } while (ReadBuffer() > 0);
            return y;
        }
        public int ReadInt()
        {
            return (int)ReadLongToken();
        }
        public long ReadLong()
        {
            return ReadLongToken();
        }
        public decimal ReadDecimal()
        {
            string s = ReadToken();
            long n = 0;
            int decimalPosition = s.Length;
            for (int k = 0; k < s.Length; k++)
            {
                char c = s[k];
                if (c == '.')
                    decimalPosition = k + 1;
                else
                    n = (n * 10) + (c - '0');
            }
            return new decimal((int)n, (int)(n >> 32), 0, false, (byte)(s.Length - decimalPosition));
        }
        public double ReadDouble()
        {
            return (double)ReadDecimal();
        }
        public int[] ReadIntArr(int n)
        {
            int[] arr = new int[n];
            for (int i = 0; i < n; i++) arr[i] = (int)ReadLongToken();
            return arr;
        }
        public long[] ReadLongArr(int n)
        {
            long[] arr = new long[n];
            for (int i = 0; i < n; i++) arr[i] = ReadLongToken();
            return arr;
        }
        public decimal[] ReadDecimalArr(int n)
        {
            decimal[] arr = new decimal[n];
            for (int i = 0; i < n; i++) arr[i] = ReadDecimal();
            return arr;
        }
        public double[] ReadDoubleArr(int n)
        {
            double[] arr = new double[n];
            for (int i = 0; i < n; i++) arr[i] = ReadDouble();
            return arr;
        }
        public void Write(string value)
        {
            sw.Write(value);
        }
        public void WriteLine(string value)
        {
            sw.WriteLine(value);
        }
    }
    // -------------------------------------------------------------------------
}
