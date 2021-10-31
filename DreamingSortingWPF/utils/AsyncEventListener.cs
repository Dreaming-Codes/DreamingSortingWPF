using System;
using System.Threading.Tasks;

namespace DreamingSortingWPF.utils {
    /// <summary>
    ///     Allows you to wait for an event
    /// </summary>
    public class AsyncEventListener {
        readonly Func<bool> _predicate;

        public AsyncEventListener() : this(() => true)
        {

        }

        public AsyncEventListener(Func<bool> predicate)
        {
            _predicate = predicate;
            Successfully = new Task(() => { });
        }

        public Task Successfully { get; }

        public void Listen(object sender, EventArgs eventArgs)
        {
            if (!Successfully.IsCompleted && _predicate.Invoke()) {
                Successfully.RunSynchronously();
            }
        }
    }
}