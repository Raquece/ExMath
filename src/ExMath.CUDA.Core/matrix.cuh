#pragma once

void* call_matrixMultiplyFlattened(void* A, void* B, int aRows, int aCols, int bRows, int bCols, dim3 dim_grid, dim3 dim_block, int operationId);

template<typename T>
T* matrixMultiplyFlattened(T* A, T* B, int aRows, int aCols, int bRows, int bCols, dim3 dim_grid, dim3 dim_block);

template<typename T>
__global__ void CUDA_matrixMultiply(const T* A, const T* B, T* P, int width, int rows, int cols);