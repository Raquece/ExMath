#include <iostream>
#include <stdexcept>
#include <cuda.h>
#include <cuda_runtime.h>

/// @brief Kernel method for performing the matrix dot product.
/// @tparam T Type of data in the matrices.
/// @param A Matrix A.
/// @param B Matrix B.
/// @param P Product matrix.
/// @param aRows Number of rows in A.
/// @param aCols Number of columns in A.
/// @param bRows Number of rows in B.
/// @param bCols Number of columns in B.
template<typename T>
__global__ void CUDA_matrixMultiply(T* A, T* B, T* P, int aRows, int aCols, int bRows, int bCols)
{
    int width = aCols; // aCols and bRows are the same

    int row = blockIdx.y * blockDim.y + threadIdx.y;
    int col = blockIdx.x * blockDim.x + threadIdx.x;

    // Grid stride loop to ensure all necessary data is processed, even
    // in abscence of enough threads.
    for (int j = row; j < aRows; j += blockDim.y * gridDim.y)
    {
        if (j < aRows)
        {
            for (int i = col; i < bCols; i += blockDim.x * gridDim.y)
            {
                if (i < bCols)
                {
                    int pIndex = j * bCols + i;

                    T sum = 0;
                    for (int k = 0; k < width; k++)
                    {
                        sum += A[j * aCols + k] * B[k * bCols + i];
                    }
                    
                    P[pIndex] = sum;
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            break;
        }
    }
}

/// @brief Host code for performing matrix mulitplication on flattened arrays.
/// @tparam T Type of data in the matrices.
/// @param A Matrix A.
/// @param B Matrix B.
/// @param aRows Number of rows in A.
/// @param aCols Number of columns in A.
/// @param bRows Number of rows in B.
/// @param bCols Number of columns in B.
/// @param dim_grid The dim size of the grid.
/// @param dim_block The dim size of the blocks.
/// @return The resultant flattened array.
template<typename T>
T* matrixMultiplyFlattened(T* A, T* B, int aRows, int aCols, int bRows, int bCols, dim3 dim_grid, dim3 dim_block)
{
    int aSize = aRows * aCols;
    int bSize = bRows * aCols;
    int pRows = aRows;
    int pCols = bCols;
    int pSize = pRows * pCols;

    T* P_flattened, *d_A, *d_B, *d_P;

    P_flattened = (T*)malloc(sizeof(T) * pSize);

    cudaMalloc(&d_A, sizeof(T) * aSize);
    cudaMalloc(&d_B, sizeof(T) * bSize);
    cudaMalloc(&d_P, sizeof(T) * pSize);

    cudaMemcpy(d_A, A, aSize * sizeof(T), cudaMemcpyHostToDevice);
    cudaMemcpy(d_B, B, bSize * sizeof(T), cudaMemcpyHostToDevice);

    CUDA_matrixMultiply<<<dim_grid, dim_block>>>(d_A, d_B, d_P, aRows, aCols, bRows, bCols);

    cudaDeviceSynchronize();

    cudaMemcpy(P_flattened, d_P, pSize * sizeof(T), cudaMemcpyDeviceToHost);

    cudaFree(d_A);
    cudaFree(d_B);
    cudaFree(d_P);

    return P_flattened;
}

/// @brief Exposes the interface to be exported in the DLL to call different types of matrix multiplication methods depending on the data type of the matrices.
/// @param A Matrix A.
/// @param B Matrix B.
/// @param aRows Number of rows in A.
/// @param aCols Number of columns in A.
/// @param bRows Number of rows in B.
/// @param bCols Number of columns in B.
/// @param dim_grid The dim size of the grid.
/// @param dim_block The dim size of the blocks.
/// @return The resultant flattened array.
void* call_matrixMultiplyFlattened(void* A, void* B, int aRows, int aCols, int bRows, int bCols, dim3 dim_grid, dim3 dim_block, int operationId)
{
    void *o;

    switch (operationId)
    {
    case 1:
        o = matrixMultiplyFlattened((double*)A, (double*)B, aRows, aCols, bRows, bCols, dim_grid, dim_block);
        break;
    case 2:
        o = matrixMultiplyFlattened((int*)A, (int*)B, aRows, aCols, bRows, bCols, dim_grid, dim_block);
        break;
    default:
        break;
    }

    return o;
}