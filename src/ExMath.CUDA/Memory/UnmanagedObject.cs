using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.CUDA.Memory
{
    public class UnmanagedObject<T> : IDisposable
    {
        public IntPtr Pointer { get; protected set; }

        public UnmanagedObject([DisallowNull] T o)
        {
            if (Pointer != IntPtr.Zero)
            {
                DestroyObject();
            }

            Pointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
            Marshal.StructureToPtr(o, Pointer, false);
        }

        protected UnmanagedObject() { }
        
        ~UnmanagedObject()
        {
            DestroyObject();
        }

        public void Dispose()
        {
            DestroyObject();
            GC.SuppressFinalize(this);
        }

        private void DestroyObject()
        {
            if (Pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Pointer);
                Pointer = IntPtr.Zero;
            }
        }
    }
}
