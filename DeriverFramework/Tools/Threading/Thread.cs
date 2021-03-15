using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DF.Tools.Threading
{
    public class Thread
    {
        public Thread(Action action)
        {
            Action = action;
        }
        
        public string Name { get; set; }
        public bool Loop { get; set; }

        private System.Threading.Thread MainThread;
        private Action Action;
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private ManualResetEvent _pauseEvent = new ManualResetEvent(true);

        public bool Start(bool hibernated = false)
        {
            MainThread = new System.Threading.Thread(new ThreadStart(ThreadStart)) { Name = Name };
            if (!hibernated)
            {
                MainThread.Start();
            }
            else
            {
                _shutdownEvent.Reset();
                _pauseEvent.Reset();
            }
            return true;
        }
        public async Task<bool> Sleep(int timeout)
        {
            _pauseEvent.Reset();
            await Task.Delay(timeout);
            _pauseEvent.Set();

            if (_shutdownEvent.WaitOne(0))
                return false;
            else
                return true;
        }
        public void Pause(bool pause)
        {
            if (pause)
            {
                _pauseEvent.Reset();
            }
            else
            {
                _pauseEvent.Set();
            }
        }
        public void Stop()
        {
            _shutdownEvent.Set();
            _pauseEvent.Set();
        }
        private void ThreadStart()
        {
            if (Loop)
            {
                while (true)
                {
                    _pauseEvent.WaitOne(Timeout.Infinite);

                    if (_shutdownEvent.WaitOne(0))
                        break;

                    Action?.Invoke();
                }
            }
            else
            {
                if (_shutdownEvent.WaitOne(0))
                {
                    _pauseEvent.WaitOne(Timeout.Infinite);
                }
                else
                {
                    Action?.Invoke();
                }
            }
        }
    }
}
