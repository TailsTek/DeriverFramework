using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Tools.Threading
{
    public class ThreadManager
    {
        public ThreadManager()
        {
            Threads = new List<Thread>();
        }
        public List<Thread> Threads { get; private set; }
        public Thread CreateNewThread(string name, Action action)
        {
            var thread = new Thread(action) { Name = name, };
            Threads.Add(thread);
            return thread;
        }
    }
}
