using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Tools.Threading
{
    public abstract class ThreadContainer
    {
        public ThreadContainer(string name, bool loop = false)
        {
            Thread = new Thread(Frame) { Name = name, Loop = loop };
        }

        public void Start(bool hib = false)
        {
            Thread.Start(hib);
        }
        public void Stop()
        {
            Thread.Stop();
        }
        public void Pause(bool pause)
        {
            Thread.Pause(pause);
        }
        private Thread Thread;
        protected abstract void Frame();
    }
}
