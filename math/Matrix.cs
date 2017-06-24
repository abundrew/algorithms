using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithms.math
{
    // ----- Matrix ------------------------------------------------------------
    // int Rows
    // int Cols
    // Matrix(int rows, int cols)
    // bool IsSquare()
    // double this[int row, int col]
    // Matrix GetCol(int k)
    // void SetCol(Matrix v, int k)
    // void MakeLU()
    // Matrix SolveWith(Matrix v)    
    // void MakeRref() 
    // Matrix Invert() 
    // double Det()
    // Matrix GetP()    
    // Matrix Duplicate()
    // string ToString()
    // static Matrix SubsForth(Matrix A, Matrix b) 
    // static Matrix SubsBack(Matrix A, Matrix b) 
    // static Matrix ZeroMatrix(int rows, int cols)
    // static Matrix IdentityMatrix(int rows, int cols)
    // static Matrix RandomMatrix(int rows, int cols, int dispersion)
    // static Matrix Parse(string ps)
    // static Matrix Transpose(Matrix m)  
    // static Matrix Power(Matrix m, int pow)  
    // static Matrix operator -(Matrix m)
    // static Matrix operator +(Matrix m1, Matrix m2)
    // static Matrix operator -(Matrix m1, Matrix m2)
    // static Matrix operator *(Matrix m1, Matrix m2)
    // static Matrix operator *(double n, Matrix m)
    // -------------------------------------------------------------------------
    public class Matrix
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        private double[] mat;
        private Matrix L;
        private Matrix U;
        private int[] pi;
        private double detOfP = 1;
        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            mat = new double[rows * cols];
        }
        public bool IsSquare()
        {
            return (Rows == Cols);
        }
        public double this[int row, int col]
        {
            get { return mat[row * Cols + col]; }
            set { mat[row * Cols + col] = value; }
        }
        public Matrix GetCol(int k)
        {
            Matrix m = new Matrix(Rows, 1);
            for (int i = 0; i < Rows; i++) m[i, 0] = this[i, k];
            return m;
        }
        public void SetCol(Matrix v, int k)
        {
            for (int i = 0; i < Rows; i++) this[i, k] = v[i, 0];
        }
        public void MakeLU()
        {
            if (!IsSquare()) throw new MException("The matrix is not square!");
            L = IdentityMatrix(Rows, Cols);
            U = Duplicate();

            pi = new int[Rows];
            for (int i = 0; i < Rows; i++) pi[i] = i;

            double p = 0;
            double pom2;
            int k0 = 0;
            int pom1 = 0;

            for (int k = 0; k < Cols - 1; k++)
            {
                p = 0;
                // find the row with the biggest pivot
                for (int i = k; i < Rows; i++)      
                {
                    if (Math.Abs(U[i, k]) > p)
                    {
                        p = Math.Abs(U[i, k]);
                        k0 = i;
                    }
                }
                if (p == 0) throw new MException("The matrix is singular!");

                // switch two rows in permutation matrix
                pom1 = pi[k]; pi[k] = pi[k0]; pi[k0] = pom1;    

                for (int i = 0; i < k; i++)
                {
                    pom2 = L[k, i]; L[k, i] = L[k0, i]; L[k0, i] = pom2;
                }

                if (k != k0) detOfP *= -1;
                // Switch rows in U
                for (int i = 0; i < Cols; i++)                  
                {
                    pom2 = U[k, i]; U[k, i] = U[k0, i]; U[k0, i] = pom2;
                }

                for (int i = k + 1; i < Rows; i++)
                {
                    L[i, k] = U[i, k] / U[k, k];
                    for (int j = k; j < Cols; j++)
                        U[i, j] = U[i, j] - L[i, k] * U[k, j];
                }
            }
        }
        // Function solves Ax = v in confirmity with solution vector "v"
        public Matrix SolveWith(Matrix v)                        
        {
            if (Rows != Cols) throw new MException("The matrix is not square!");
            if (Rows != v.Rows) throw new MException("Wrong number of results in solution vector!");
            if (L == null) MakeLU();

            Matrix b = new Matrix(Rows, 1);
            // switch two items in "v" due to permutation matrix
            for (int i = 0; i < Rows; i++) b[i, 0] = v[pi[i], 0];   

            Matrix z = SubsForth(L, b);
            Matrix x = SubsBack(U, z);

            return x;
        }
        // Function makes reduced echolon form
        public void MakeRref()                                    
        {
            int lead = 0;
            for (int r = 0; r < Rows; r++)
            {
                if (Cols <= lead) break;
                int i = r;
                while (this[i, lead] == 0)
                {
                    i++;
                    if (i == Rows)
                    {
                        i = r;
                        lead++;
                        if (Cols == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }
                for (int j = 0; j < Cols; j++)
                {
                    double temp = this[r, j];
                    this[r, j] = this[i, j];
                    this[i, j] = temp;
                }
                double div = this[r, lead];
                for (int j = 0; j < Cols; j++) this[r, j] /= div;
                for (int j = 0; j < Rows; j++)
                {
                    if (j != r)
                    {
                        double sub = this[j, lead];
                        for (int k = 0; k < Cols; k++) this[j, k] -= (sub * this[r, k]);
                    }
                }
                lead++;
            }
        }
        // Function returns the inverted matrix
        public Matrix Invert()                                   
        {
            if (L == null) MakeLU();

            Matrix inv = new Matrix(Rows, Cols);

            for (int i = 0; i < Rows; i++)
            {
                Matrix Ei = Matrix.ZeroMatrix(Rows, 1);
                Ei[i, 0] = 1;
                Matrix col = SolveWith(Ei);
                inv.SetCol(col, i);
            }
            return inv;
        }
        // Function for determinant
        public double Det()                         
        {
            if (L == null) MakeLU();
            double det = detOfP;
            for (int i = 0; i < Rows; i++) det *= U[i, i];
            return det;
        }
        // Function returns permutation matrix "P" due to permutation vector "pi"
        public Matrix GetP()                        
        {
            if (L == null) MakeLU();

            Matrix matrix = ZeroMatrix(Rows, Cols);
            for (int i = 0; i < Rows; i++) matrix[pi[i], i] = 1;
            return matrix;
        }
        // Function returns the copy of this matrix
        public Matrix Duplicate()
        {
            Matrix matrix = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    matrix[i, j] = this[i, j];
            return matrix;
        }
        // Function solves Ax = b for A as a lower triangular matrix
        public static Matrix SubsForth(Matrix A, Matrix b)          
        {
            if (A.L == null) A.MakeLU();
            int n = A.Rows;
            Matrix x = new Matrix(n, 1);

            for (int i = 0; i < n; i++)
            {
                x[i, 0] = b[i, 0];
                for (int j = 0; j < i; j++) x[i, 0] -= A[i, j] * x[j, 0];
                x[i, 0] = x[i, 0] / A[i, i];
            }
            return x;
        }
        // Function solves Ax = b for A as an upper triangular matrix
        public static Matrix SubsBack(Matrix A, Matrix b)           
        {
            if (A.L == null) A.MakeLU();
            int n = A.Rows;
            Matrix x = new Matrix(n, 1);

            for (int i = n - 1; i > -1; i--)
            {
                x[i, 0] = b[i, 0];
                for (int j = n - 1; j > i; j--) x[i, 0] -= A[i, j] * x[j, 0];
                x[i, 0] = x[i, 0] / A[i, i];
            }
            return x;
        }
        // Function generates the zero matrix
        public static Matrix ZeroMatrix(int rows, int cols)       
        {
            Matrix matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = 0;
            return matrix;
        }
        // Function generates the identity matrix
        public static Matrix IdentityMatrix(int rows, int cols)   
        {
            Matrix matrix = ZeroMatrix(rows, cols);
            for (int i = 0; i < Math.Min(rows, cols); i++)
                matrix[i, i] = 1;
            return matrix;
        }
        // Function generates the random matrix
        public static Matrix RandomMatrix(int rows, int cols, int dispersion)       
        {
            Random random = new Random();
            Matrix matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = random.Next(-dispersion, dispersion);
            return matrix;
        }
        // Function parses the matrix from string
        public static Matrix Parse(string ps)                        
        {
            string s = NormalizeMatrixString(ps);
            string[] rows = System.Text.RegularExpressions.Regex.Split(s, "\r\n");
            string[] nums = rows[0].Split(' ');
            Matrix matrix = new Matrix(rows.Length, nums.Length);
            try
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    nums = rows[i].Split(' ');
                    for (int j = 0; j < nums.Length; j++) matrix[i, j] = double.Parse(nums[j]);
                }
            }
            catch (FormatException) { throw new MException("Wrong input format!"); }
            return matrix;
        }
        // Function returns matrix as a string
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                    s.Append(String.Format("{0,5:E2}", this[i, j]) + " ");
                s.AppendLine();
            }
            return s.ToString();
        }
        // Matrix transpose, for any rectangular matrix
        public static Matrix Transpose(Matrix m)              
        {
            Matrix t = new Matrix(m.Cols, m.Rows);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    t[j, i] = m[i, j];
            return t;
        }
        // Power matrix to exponent
        public static Matrix Power(Matrix m, int pow)           
        {
            if (pow == 0) return IdentityMatrix(m.Rows, m.Cols);
            if (pow == 1) return m.Duplicate();
            if (pow == -1) return m.Invert();

            Matrix x;
            if (pow < 0) { x = m.Invert(); pow *= -1; }
            else x = m.Duplicate();

            Matrix ret = IdentityMatrix(m.Rows, m.Cols);
            while (pow != 0)
            {
                if ((pow & 1) == 1) ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }
        private static void SafeAplusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)         // rows
                for (int j = 0; j < size; j++)     // cols
                {
                    C[i, j] = 0;
                    if (xa + j < A.Cols && ya + i < A.Rows) C[i, j] += A[ya + i, xa + j];
                    if (xb + j < B.Cols && yb + i < B.Rows) C[i, j] += B[yb + i, xb + j];
                }
        }
        private static void SafeAminusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)         // rows
                for (int j = 0; j < size; j++)     // cols
                {
                    C[i, j] = 0;
                    if (xa + j < A.Cols && ya + i < A.Rows) C[i, j] += A[ya + i, xa + j];
                    if (xb + j < B.Cols && yb + i < B.Rows) C[i, j] -= B[yb + i, xb + j];
                }
        }
        private static void SafeACopytoC(Matrix A, int xa, int ya, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)         // rows
                for (int j = 0; j < size; j++)     // cols
                {
                    C[i, j] = 0;
                    if (xa + j < A.Cols && ya + i < A.Rows) C[i, j] += A[ya + i, xa + j];
                }
        }
        private static void AplusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // rows
                for (int j = 0; j < size; j++) C[i, j] = A[ya + i, xa + j] + B[yb + i, xb + j];
        }
        private static void AminusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // rows
                for (int j = 0; j < size; j++) C[i, j] = A[ya + i, xa + j] - B[yb + i, xb + j];
        }
        private static void ACopytoC(Matrix A, int xa, int ya, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // rows
                for (int j = 0; j < size; j++) C[i, j] = A[ya + i, xa + j];
        }
        // Smart matrix multiplication
        private static Matrix StrassenMultiply(Matrix A, Matrix B)                
        {
            if (A.Cols != B.Rows) throw new MException("Wrong dimension of matrix!");

            Matrix R;

            int msize = Math.Max(Math.Max(A.Rows, A.Cols), Math.Max(B.Rows, B.Cols));

            int size = 1; int n = 0;
            while (msize > size) { size *= 2; n++; };
            int h = size / 2;

            Matrix[,] mField = new Matrix[n, 9];

            /*
             *  8x8, 8x8, 8x8, ...
             *  4x4, 4x4, 4x4, ...
             *  2x2, 2x2, 2x2, ...
             *  . . .
             */

            int z;
            for (int i = 0; i < n - 4; i++)          // rows
            {
                z = (int)Math.Pow(2, n - i - 1);
                for (int j = 0; j < 9; j++) mField[i, j] = new Matrix(z, z);
            }

            SafeAplusBintoC(A, 0, 0, A, h, h, mField[0, 0], h);
            SafeAplusBintoC(B, 0, 0, B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 1], 1, mField); // (A11 + A22) * (B11 + B22);

            SafeAplusBintoC(A, 0, h, A, h, h, mField[0, 0], h);
            SafeACopytoC(B, 0, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 2], 1, mField); // (A21 + A22) * B11;

            SafeACopytoC(A, 0, 0, mField[0, 0], h);
            SafeAminusBintoC(B, h, 0, B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 3], 1, mField); //A11 * (B12 - B22);

            SafeACopytoC(A, h, h, mField[0, 0], h);
            SafeAminusBintoC(B, 0, h, B, 0, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 4], 1, mField); //A22 * (B21 - B11);

            SafeAplusBintoC(A, 0, 0, A, h, 0, mField[0, 0], h);
            SafeACopytoC(B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 5], 1, mField); //(A11 + A12) * B22;

            SafeAminusBintoC(A, 0, h, A, 0, 0, mField[0, 0], h);
            SafeAplusBintoC(B, 0, 0, B, h, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 6], 1, mField); //(A21 - A11) * (B11 + B12);

            SafeAminusBintoC(A, h, 0, A, h, h, mField[0, 0], h);
            SafeAplusBintoC(B, 0, h, B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 7], 1, mField); // (A12 - A22) * (B21 + B22);

            R = new Matrix(A.Rows, B.Cols);                  // result

            /// C11
            for (int i = 0; i < Math.Min(h, R.Rows); i++)          // rows
                for (int j = 0; j < Math.Min(h, R.Cols); j++)     // cols
                    R[i, j] = mField[0, 1 + 1][i, j] + mField[0, 1 + 4][i, j] - mField[0, 1 + 5][i, j] + mField[0, 1 + 7][i, j];

            /// C12
            for (int i = 0; i < Math.Min(h, R.Rows); i++)          // rows
                for (int j = h; j < Math.Min(2 * h, R.Cols); j++)     // cols
                    R[i, j] = mField[0, 1 + 3][i, j - h] + mField[0, 1 + 5][i, j - h];

            /// C21
            for (int i = h; i < Math.Min(2 * h, R.Rows); i++)          // rows
                for (int j = 0; j < Math.Min(h, R.Cols); j++)     // cols
                    R[i, j] = mField[0, 1 + 2][i - h, j] + mField[0, 1 + 4][i - h, j];

            /// C22
            for (int i = h; i < Math.Min(2 * h, R.Rows); i++)          // rows
                for (int j = h; j < Math.Min(2 * h, R.Cols); j++)     // cols
                    R[i, j] = mField[0, 1 + 1][i - h, j - h] - mField[0, 1 + 2][i - h, j - h] + mField[0, 1 + 3][i - h, j - h] + mField[0, 1 + 6][i - h, j - h];

            return R;
        }
        // A * B into C, level of recursion, matrix field
        private static void StrassenMultiplyRun(Matrix A, Matrix B, Matrix C, int l, Matrix[,] f)    
        {
            int size = A.Rows;
            int h = size / 2;

            AplusBintoC(A, 0, 0, A, h, h, f[l, 0], h);
            AplusBintoC(B, 0, 0, B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 1], l + 1, f); // (A11 + A22) * (B11 + B22);

            AplusBintoC(A, 0, h, A, h, h, f[l, 0], h);
            ACopytoC(B, 0, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 2], l + 1, f); // (A21 + A22) * B11;

            ACopytoC(A, 0, 0, f[l, 0], h);
            AminusBintoC(B, h, 0, B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 3], l + 1, f); //A11 * (B12 - B22);

            ACopytoC(A, h, h, f[l, 0], h);
            AminusBintoC(B, 0, h, B, 0, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 4], l + 1, f); //A22 * (B21 - B11);

            AplusBintoC(A, 0, 0, A, h, 0, f[l, 0], h);
            ACopytoC(B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 5], l + 1, f); //(A11 + A12) * B22;

            AminusBintoC(A, 0, h, A, 0, 0, f[l, 0], h);
            AplusBintoC(B, 0, 0, B, h, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 6], l + 1, f); //(A21 - A11) * (B11 + B12);

            AminusBintoC(A, h, 0, A, h, h, f[l, 0], h);
            AplusBintoC(B, 0, h, B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 7], l + 1, f); // (A12 - A22) * (B21 + B22);

            /// C11
            for (int i = 0; i < h; i++)          // rows
                for (int j = 0; j < h; j++)     // cols
                    C[i, j] = f[l, 1 + 1][i, j] + f[l, 1 + 4][i, j] - f[l, 1 + 5][i, j] + f[l, 1 + 7][i, j];

            /// C12
            for (int i = 0; i < h; i++)          // rows
                for (int j = h; j < size; j++)     // cols
                    C[i, j] = f[l, 1 + 3][i, j - h] + f[l, 1 + 5][i, j - h];

            /// C21
            for (int i = h; i < size; i++)          // rows
                for (int j = 0; j < h; j++)     // cols
                    C[i, j] = f[l, 1 + 2][i - h, j] + f[l, 1 + 4][i - h, j];

            /// C22
            for (int i = h; i < size; i++)          // rows
                for (int j = h; j < size; j++)     // cols
                    C[i, j] = f[l, 1 + 1][i - h, j - h] - f[l, 1 + 2][i - h, j - h] + f[l, 1 + 3][i - h, j - h] + f[l, 1 + 6][i - h, j - h];
        }
        // Stupid matrix multiplication
        private static Matrix StupidMultiply(Matrix m1, Matrix m2)                  
        {
            if (m1.Cols != m2.Rows) throw new MException("Wrong dimensions of matrix!");

            Matrix result = ZeroMatrix(m1.Rows, m2.Cols);
            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Cols; j++)
                    for (int k = 0; k < m1.Cols; k++)
                        result[i, j] += m1[i, k] * m2[k, j];
            return result;
        }
        // Matrix multiplication
        private static Matrix Multiply(Matrix m1, Matrix m2)                         
        {
            if (m1.Cols != m2.Rows) throw new MException("Wrong dimension of matrix!");
            int msize = Math.Max(Math.Max(m1.Rows, m1.Cols), Math.Max(m2.Rows, m2.Cols));
            // stupid multiplication faster for small matrices
            if (msize < 32)
            {
                return StupidMultiply(m1, m2);
            }
            // stupid multiplication faster for non square matrices
            if (!m1.IsSquare() || !m2.IsSquare())
            {
                return StupidMultiply(m1, m2);
            }
            // Strassen multiplication is faster for large square matrix 2^N x 2^N
            // NOTE because of previous checks msize == m1.cols == m1.rows == m2.cols == m2.cols
            double exponent = Math.Log(msize) / Math.Log(2);
            if (Math.Pow(2, exponent) == msize)
            {
                return StrassenMultiply(m1, m2);
            }
            else
            {
                return StupidMultiply(m1, m2);
            }
        }
        // Multiplication by constant n
        private static Matrix Multiply(double n, Matrix m)                          
        {
            Matrix r = new Matrix(m.Rows, m.Cols);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    r[i, j] = m[i, j] * n;
            return r;
        }
        private static Matrix Add(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols) throw new MException("Matrices must have the same dimensions!");
            Matrix r = new Matrix(m1.Rows, m1.Cols);
            for (int i = 0; i < r.Rows; i++)
                for (int j = 0; j < r.Cols; j++)
                    r[i, j] = m1[i, j] + m2[i, j];
            return r;
        }
        public static string NormalizeMatrixString(string matStr)
        {
            // Remove any multiple spaces
            while (matStr.IndexOf("  ") != -1)
                matStr = matStr.Replace("  ", " ");

            // Remove any spaces before or after newlines
            matStr = matStr.Replace(" \r\n", "\r\n");
            matStr = matStr.Replace("\r\n ", "\r\n");

            // If the data ends in a newline, remove the trailing newline.
            // Make it easier by first replacing \r\n’s with |’s then
            // restore the |’s with \r\n’s
            matStr = matStr.Replace("\r\n", "|");
            while (matStr.LastIndexOf("|") == (matStr.Length - 1))
                matStr = matStr.Substring(0, matStr.Length - 1);

            matStr = matStr.Replace("|", "\r\n");
            return matStr.Trim();
        }
        public static Matrix operator -(Matrix m)
        { return Matrix.Multiply(-1, m); }
        public static Matrix operator +(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, m2); }
        public static Matrix operator -(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, -m2); }
        public static Matrix operator *(Matrix m1, Matrix m2)
        { return Matrix.Multiply(m1, m2); }
        public static Matrix operator *(double n, Matrix m)
        { return Matrix.Multiply(n, m); }
    }
    public class MException : Exception
    {
        public MException(string Message) : base(Message) { }
    }
    // -------------------------------------------------------------------------
}
