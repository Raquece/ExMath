using ExMath.CUDA.Memory;
using ExMath.Matrix;
using System.Runtime.InteropServices;

namespace ExMath.CUDA.Matrix
{
    public static class CudaMatrix
    {
        public static CudaMatrix<T> ToCudaMatrix<T>(this Matrix<T> matrix)
            where T : struct
        {
            CudaMatrix<T> m = new(matrix.Rows, matrix.Columns)
            {
                Data = matrix.Data
            };

            return m;
        }

        [DllImport("ExMath.CUDA.Core.dll")]
        internal static extern IntPtr MatrixMultiply(IntPtr A, IntPtr B, int aRows, int aCols, int bRows, int bCols, int dim_grid_x, int dim_grid_y, int dim_grid_z, int dim_block_x, int dim_block_y, int dim_block_z, int operationId);
    }

    [Serializable]
    public class CudaMatrix<T> : Matrix<T>
        where T : struct
    {
        public CudaMatrix(int rows, int columns, T def = default) : base(rows, columns, def)
        { }

        /// <summary>
        /// Performs the dot product of this matrix with another using CUDA.
        /// </summary>
        /// <param name="other">The other matrix to perform the product with.</param>
        /// <param name="dim_grid_x">The number of blocks a grid has in the X dimension.</param>
        /// <param name="dim_grid_y">The number of blocks a grid has in the Y dimension.</param>
        /// <param name="dim_grid_z">The number of blocks a grid has in the Z dimension.</param>
        /// <param name="dim_block_x">The number of threads a block has in the X dimension.</param>
        /// <param name="dim_block_y">The number of threads a block has in the Y dimension.</param>
        /// <param name="dim_block_z">The number of threads a block has in the Z dimension.</param>
        public void CudaMultiply(Matrix<T> other, int dim_grid_x = 16, int dim_grid_y = 16, int dim_grid_z = 1, int dim_block_x = 8, int dim_block_y = 8, int dim_block_z = 1)
        {
            T[] aFlattened = Data.Cast<T>().ToArray();
            T[] bFlattened = other.Data.Cast<T>().ToArray();

            using UnmanagedArray<T> aarr = new(aFlattened);
            using UnmanagedArray<T> barr = new(bFlattened);

            IntPtr p = CudaMatrix.MatrixMultiply(aarr.Pointer, barr.Pointer,
                Rows, Columns, other.Rows, other.Columns,
                dim_grid_x, dim_grid_y, dim_grid_z,
                dim_block_x, dim_block_y, dim_block_z,
                GetOperationId());

            var arrflat = UnmanagedArray<T>.FromUnmanagedPointer(p, Rows * other.Columns);

            Data = CudaMatrix<T>.Fold(arrflat, other.Columns, Rows);
        }

        /// <summary>
        /// Performs the dot product of this matrix with another using CUDA.
        /// </summary>
        /// <param name="a">The first matrix to perform the product on..</param>
        /// <param name="b">The other matrix to perform the product with.</param>
        /// <param name="dim_grid_x">The number of blocks a grid has in the X dimension.</param>
        /// <param name="dim_grid_y">The number of blocks a grid has in the Y dimension.</param>
        /// <param name="dim_grid_z">The number of blocks a grid has in the Z dimension.</param>
        /// <param name="dim_block_x">The number of threads a block has in the X dimension.</param>
        /// <param name="dim_block_y">The number of threads a block has in the Y dimension.</param>
        /// <param name="dim_block_z">The number of threads a block has in the Z dimension.</param>
        /// <returns>The new matrix with the dot product applied.</returns>
        public static CudaMatrix<T> CudaMultiply(Matrix<T> a, Matrix<T> b, int dim_grid_x = 16, int dim_grid_y = 16, int dim_grid_z = 1, int dim_block_x = 8, int dim_block_y = 8, int dim_block_z = 1)
        {
            T[] aFlattened = a.Data.Cast<T>().ToArray();
            T[] bFlattened = b.Data.Cast<T>().ToArray();

            using UnmanagedArray<T> aarr = new(aFlattened);
            using UnmanagedArray<T> barr = new(bFlattened);

            IntPtr p = CudaMatrix.MatrixMultiply(aarr.Pointer, barr.Pointer,
                a.Rows, a.Columns, b.Rows, b.Columns,
                dim_grid_x, dim_grid_y, dim_grid_z,
                dim_block_x, dim_block_y, dim_block_z,
                GetOperationId());

            var arrflat = UnmanagedArray<T>.FromUnmanagedPointer(p, a.Rows * b.Columns);

            return new CudaMatrix<T>(a.Rows, b.Columns)
            {
                Data = CudaMatrix<T>.Fold(arrflat, b.Columns, a.Rows)
            };
        }

        private static T[,] Fold(T[] array, int cols, int rows)
        {
            T[,] matrix = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = array[i * cols + j];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets the operation ID from a matrix data type.
        /// </summary>
        /// <returns>The operation ID.</returns>
        /// <exception cref="ArgumentException">Thrown if data type is invalid for CUDA matrix operations.</exception>
        private static int GetOperationId()
        {
            T obj = default;

            return obj switch
            {
                double => 1,
                int => 2,
                _ => throw new ArgumentException("Type of matrix not supported for CUDA operations."),
            };
        }
    }
}
