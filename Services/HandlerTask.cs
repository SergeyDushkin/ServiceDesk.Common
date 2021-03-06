using System;
using System.Threading.Tasks;
using NLog;

namespace servicedesk.Common.Services
{
    public class HandlerTask : IHandlerTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IHandler _handler;
        private readonly Action _run;
        private readonly Func<Task> _runAsync;
        private Action _always;
        private Func<Task> _alwaysAsync;
        private Action _onSuccess;
        private Action<Logger> _onSuccessWithLogger;
        private Func<Task> _onSuccessAsync;
        private Func<Logger, Task> _onSuccessWithLoggerAsync;
        private Action<Exception> _onError;
        private Action<Exception, Logger> _onErrorWithLogger;
        private Func<Exception, Task> _onErrorAsync;
        private Func<Exception, Logger, Task> _onErrorWithLoggerAsync;
        private bool _propagateException = true;
        private bool _executeOnError = true;

        public HandlerTask(IHandler handler, Action run)
        {
            _handler = handler;
            _run = run;
        }

        public HandlerTask(IHandler handler, Func<Task> runAsync)
        {
            _handler = handler;
            _runAsync = runAsync;
        }

        public IHandlerTask Always(Action always)
        {
            _always = always;

            return this;
        }

        public IHandlerTask Always(Func<Task> always)
        {
            _alwaysAsync = always;

            return this;
        }

        public IHandlerTask OnError(Action<Exception> onError, bool propagateException = false)
        {
            _onError = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Action<Exception, Logger> onError, bool propagateException = false)
        {
            _onErrorWithLogger = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Func<Exception, Task> onError, bool propagateException = false)
        {
            _onErrorAsync = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnError(Func<Exception, Logger, Task> onError, bool propagateException = false)
        {
            _onErrorWithLoggerAsync = onError;
            _propagateException = propagateException;

            return this;
        }

        public IHandlerTask OnSuccess(Action<Logger> onSuccess)
        {
            _onSuccessWithLogger = onSuccess;

            return this;
        }
        public IHandlerTask OnSuccess(Action onSuccess)
        {
            _onSuccess = onSuccess;

            return this;
        }

        public IHandlerTask OnSuccess(Func<Task> onSuccess)
        {
            _onSuccessAsync = onSuccess;

            return this;
        }
        public IHandlerTask OnSuccess(Func<Logger, Task> onSuccess)
        {
            _onSuccessWithLoggerAsync = onSuccess;

            return this;
        }

        public IHandlerTask PropagateException()
        {
            _propagateException = true;

            return this;
        }

        public IHandlerTask DoNotPropagateException()
        {
            _propagateException = false;

            return this;
        }

        public IHandler Next() => _handler;

        public void Execute()
        {
            try
            {
                _run();
                _onSuccessWithLogger?.Invoke(Logger);
                _onSuccess?.Invoke();
            }
            catch (Exception exception)
            {
                var executeOnError = _executeOnError || exception == null;
                if(executeOnError)
                {
                    _onErrorWithLogger?.Invoke(exception, Logger);
                    _onError?.Invoke(exception);
                }
                if(_propagateException)
                {
                    throw;
                }
            }
            finally
            {
                _always?.Invoke();
            }
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await _runAsync();
                if(_onSuccessAsync != null)
                {
                    await _onSuccessAsync();
                }

                if(_onSuccessWithLoggerAsync != null)
                {
                    await _onSuccessWithLoggerAsync(Logger);
                }
            }
            catch (Exception exception)
            {                
                var executeOnError = _executeOnError || exception == null;
                if(executeOnError)
                {
                    _onErrorWithLogger?.Invoke(exception, Logger);
                    if (_onErrorWithLoggerAsync != null)
                    {
                        await _onErrorWithLoggerAsync(exception, Logger);
                    }
                    _onError?.Invoke(exception);
                    if (_onErrorAsync != null)
                    {
                        await _onErrorAsync(exception);
                    }
                }
                if(_propagateException)
                {
                    throw;
                }
            }
            finally
            {
                if (_alwaysAsync != null)
                {
                    await _alwaysAsync();
                }
            }
        }
    }
}