using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DTools.Diagnostics
{
    public static class Performance
    {
		public static TimeSpan Time_DateTimNow(Action action)
		{
			DateTime a = DateTime.Now;
			action();
			DateTime b = DateTime.Now;
			return b - a;
		}

		public static TimeSpan Time_StopWatch(Action action)
		{
			Stopwatch watch = new Stopwatch();
			watch.Restart();
			action();
			watch.Stop();
			return watch.Elapsed;
		}
	}
}
