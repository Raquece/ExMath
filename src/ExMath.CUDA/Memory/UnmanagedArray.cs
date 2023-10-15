using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ExMath.CUDA.Memory
{
    public class UnmanagedArray<T> : UnmanagedObject<T>, IDisposable
    {
        public UnmanagedArray(IEnumerable<T> collection) : base()
        {
            if (Pointer != IntPtr.Zero)
            {
                Dispose();
            }

            var arr = collection.ToArray();
            Length = arr.Length;
            Pointer = AllocArray(arr);
        }

        public int Length { get; private set; }

        /// <summary>
        /// Gets the managed array from an unmanaged pointer.
        /// </summary>
        /// <param name="size">The number of elements in the array.</param>
        /// <returns>The managed array.</returns>
        public static T?[] FromUnmanagedPointer(IntPtr ptr, int size)
        {
            var elementSize = Marshal.SizeOf(typeof(T));
            var arr = new T?[size];

            for (int i = 0; i < size; i++)
            {
                arr[i] = Marshal.PtrToStructure<T>(IntPtr.Add(ptr, i * elementSize));
            }

            return arr;
        }

        /// <summary>
        /// Gets the managed array from an unmanaged array object.
        /// </summary>
        /// <param name="arr">The unmanaged array object type.</param>
        /// <returns>The managed array from the unmanaged array.</returns>
        public static T?[] FromUnmanagedPointer(UnmanagedArray<T> arr)
        {
            return FromUnmanagedPointer(arr.Pointer, arr.Length);
        }

        private static IntPtr AllocArray([DisallowNull] T[] array)
        {
            var elementSize = Marshal.SizeOf<T>();
            IntPtr unmanagedArray = Marshal.AllocHGlobal(array.Length * elementSize);

            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    continue;
                }

                Marshal.StructureToPtr(array[i], IntPtr.Add(unmanagedArray, i * elementSize), false);
            }

            return unmanagedArray;
        }
    }
}
