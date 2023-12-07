using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Errors;

namespace Vcc.Nolvus.Package.Errors
{
    public class ErrorHandler : IErrorHandler
    {
        #region Fields

        private int _Threshold = 0;
        private readonly object SyncRoot = new object();
        private List<FaultyMod> _FaultyMods { get; set; } = new List<FaultyMod>();
        public TaskCompletionSource<object> CancelTasks { get; set; }
        public CancellationTokenSource CancelTokenSource { get; set; }

        #endregion

        #region Properties

        public bool ThresholdReached
        {
            get
            {
                return !(ErrorsCount < _Threshold);
            }
        }

        public bool ThresholdEnabled
        {
            get
            {
                return _Threshold != 0;
            }
        }

        public int ErrorsCount
        {
            get
            {
                lock (SyncRoot)
                {
                    return _FaultyMods.Count;
                }
            }
        }

        public bool HasErrors
        {
            get
            {
                return _FaultyMods.Count > 0;
            }
        }

        public bool Cancelling
        {
            get
            {
                return CancelTokenSource.Token.IsCancellationRequested;
            }
        }

        public CancellationToken Token
        {
            get
            {
                return CancelTokenSource.Token;
            }
        }

        public List<FaultyMod> List
        {
            get
            {
                return _FaultyMods;
            }
        }

        #endregion

        public ErrorHandler(int ErrorsThreshold)
        {
            _Threshold = ErrorsThreshold;
        }

        #region Methods

        private Exception CreateException()
        {
            return new InstallationCancelledException("Installation has been cancelled due to too many errors");
        }

        public void AddFaultyMod(IInstallableElement Mod, Exception Exception)
        {
            lock (SyncRoot)
            {                
                _FaultyMods.Add(new FaultyMod()
                {
                    Mod = Mod,
                    Error = Exception
                });
            }
        }                

        public void ThrowException()
        {
            if (CancelTasks.Task.IsFaulted)
            {
                throw CancelTasks.Task.Exception.InnerException;
            }
            else
            {
                throw CreateException();
            }
            
        }

        public void CancelInstall()
        {
            ServiceSingleton.Logger.Log("Cancelling current tasks");
            CancelTokenSource.Cancel();            
        }

        public void Exit()
        {
            CancelTasks.SetException(CreateException());
        }

        public void Clear()
        {
            _FaultyMods.Clear();
        }

        #endregion
    }
}
