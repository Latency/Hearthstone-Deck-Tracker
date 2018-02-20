using System.Diagnostics;
using System.Threading.Tasks;
using HearthStone.DeckTracker.Utility.Analytics;
using HearthStone.DeckTracker.Utility.Logging;


namespace HearthStone.DeckTracker.Utility
{
	public static class ResourceMonitor
	{
		public static async void Run()
		{
			while(true)
			{
				var mem = Process.GetCurrentProcess().PrivateMemorySize64 >> 20;
				if(mem > 1024)
				{ 
					Log.Warn($"High memory usage: {mem} MB");
					Influx.OnHighMemoryUsage(mem);
					return;
				}
				await Task.Delay(60000);
			}
		}
	}
}
