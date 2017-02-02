
namespace SrkToolkit.Xaml.Internal
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    public class DisposableEventHandler : IDisposable
    {
        public static IDisposable FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler) where TEventArgs : EventArgs
        {
            if (addHandler == null)
            {
                throw new ArgumentNullException("addHandler");
            }
            if (removeHandler == null)
            {
                throw new ArgumentNullException("removeHandler");
            }

            return Observable.Create<EventPattern<TEventArgs>>(delegate(IObserver<EventPattern<TEventArgs>> observer)
            {
                EventHandler<TEventArgs> eventHandler = delegate(object sender, TEventArgs eventArgs)
                {
                    observer.OnNext(new EventPattern<TEventArgs>(sender, eventArgs));
                };
                TDelegate d = (TDelegate)((object)Delegate.CreateDelegate(typeof(TDelegate), eventHandler, typeof(EventHandler<TEventArgs>).GetMethod("Invoke")));
                addHandler.Invoke(d);
                return Disposable.Create(delegate
                {
                    removeHandler.Invoke(d);
                });
            });
        }
    }
}
