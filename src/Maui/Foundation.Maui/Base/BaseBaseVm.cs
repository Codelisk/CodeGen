using AsyncAwaitBestPractices;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Maui.Base
{
    public class BaseBaseVm
    {
        #region LoadingCommand without parameters
        protected virtual Command LoadingCommand(Func<Task> taskFunc) => new Command(() =>
            this.LoadingAsync(taskFunc).SafeFireAndForget()
        );
        protected virtual Command BlockCommand(Func<Task> taskFunc) => new Command(() =>
            this.LoadingAsync(taskFunc, true).SafeFireAndForget()
        );
        protected virtual async Task LoadingAsync(Func<Task> taskFunc, bool blockOnly = false)
        {
            if (this.IsBusyOrLocked())
            {
                return;
            }

            this.SetBlockingAndBusy(true, blockOnly);

            try
            {
                await taskFunc.Invoke();
            }
            finally
            {
                this.SetBlockingAndBusy(false, blockOnly);
            }
        }
        #endregion
        #region LoadingCommand with parameters

        protected virtual Command LoadingCommand<T>(Func<T, Task> taskFunc) => new Command<T>((o) =>
            this.LoadingAsync(taskFunc, o).SafeFireAndForget()
        );
        protected virtual Command BlockCommand<T>(Func<T, Task> taskFunc) => new Command<T>((o) =>
            this.LoadingAsync(taskFunc, o, true).SafeFireAndForget()
        );
        protected virtual async Task LoadingAsync<T>(Func<T, Task> taskFunc, T o, bool blockOnly = false)
        {
            if (this.IsBusyOrLocked())
            {
                return;
            }

            this.SetBlockingAndBusy(true, blockOnly);

            try
            {
                await taskFunc.Invoke(o);
            }
            finally
            {
                this.SetBlockingAndBusy(false, blockOnly);
            }
        }
        #endregion
        #region Loading and Blocking Helper
        private bool IsBlocked;
        private void SetBlockingAndBusy(bool val, bool blockOnly = false)
        {
            this.IsBlocked = val;

            if (!blockOnly)
            {
                this.IsBusy = val;
            }
        }
        private bool IsBusyOrLocked()
        {
            return this.IsBlocked || this.IsBusy;
        }
        [Reactive]
        public bool IsBusy { get; set; }
        #endregion
    }
}
