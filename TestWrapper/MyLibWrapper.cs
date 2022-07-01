using System;
using System.Runtime.InteropServices;

namespace TestWrapper
{
    public static class MyLibWrapper
    {
        [DllImport("testSharedLibrary", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fooPublicFunction(int a);
        [DllImport("testSharedLibrary", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 ParallelSum(int a);
    }
    public class Foo : IDisposable
    {
        [DllImport("testSharedLibrary", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr createFoo();
        [DllImport("testSharedLibrary", CallingConvention = CallingConvention.Cdecl)]
        private static extern void disposeFoo(IntPtr instance);
        [DllImport("testSharedLibrary", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fooSetA(IntPtr instance, int value);
        [DllImport("testSharedLibrary", CallingConvention = CallingConvention.Cdecl)]
        private static extern int fooGetA(IntPtr instance);
        private IntPtr instance;

        public Foo() => instance = createFoo();
        public void SetA(int val) => fooSetA(instance, val);
        public int GetA() => fooGetA(instance);

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                disposeFoo(instance);
                instance = IntPtr.Zero;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Foo()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
