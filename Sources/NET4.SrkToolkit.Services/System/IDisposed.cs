
namespace System
{
    public interface IDisposed : IDisposable
    {
        bool IsDisposed { get; }
    }
}
