#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <stdio.h>

#include "matrix.cuh"

extern "C"
{
    __declspec(dllexport) void* MatrixMultiply(void* A, void* B, int aRows, int aCols, int bRows, int bCols, int dim_grid_x, int dim_grid_y, int dim_grid_z, int dim_block_x, int dim_block_y, int dim_block_z, int operationId)
    {
        return call_matrixMultiplyFlattened(A, B, aRows, aCols, bRows, bCols, dim3(dim_grid_x, dim_grid_y, dim_grid_z), dim3(dim_block_x, dim_block_y, dim_block_z), operationId);
    }
}