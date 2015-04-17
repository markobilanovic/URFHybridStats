using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace GetURFMatches
{
	class Program
	{
		static void Main(string[] args)
		{

			TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
			TimeSpan unixTicksNow = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
			long unixTime = (long)unixTicksNow.TotalSeconds;
			long eightHoursSpan = 60 * 60 * 8;


			long unixBefore8Hours = unixTime - eightHoursSpan;

			long x = Convert.ToInt64(Convert.ToString(unixBefore8Hours).Substring(4));

			decimal t = Math.Ceiling((decimal)(unixBefore8Hours / 300));
			long k = (long)t * 300;

			//long startTime = Convert.ToInt64(string.Format("{0}{1}", Convert.ToString(unixBefore8Hours).Substring(0,4), k));

			string queryBaseFirstPart = "api.pvp.net/api/lol/";
			string queryBaseSecondPart = "/v4.1/game/ids?"; 
			string api_key = "6d866a5a-716c-4b87-8b32-7d6080aa3770";
			WebClient client = new WebClient();

			string[] regions = new string[] {"br", "eune", "euw", "kr", "lan", "las", "na", "oce", "ru", "tr"};

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			Stopwatch tenMinutesStopwatch = new Stopwatch();
			tenMinutesStopwatch.Start();
			int counterTo500 = 0;
			int globalCounter = 0;

			foreach (string region in regions)
			{
				Console.WriteLine("Starting with: " + region);
				string queryRegion = string.Format("https://{0}.", region);
				int counter = 0;
				for (long i = k; i < unixTime; i = i + 300)
				{
					
					try
					{
						counter++;
						counterTo500++;
						globalCounter++;
						if(counterTo500 >= 500)
						{
							long elapsedSeconds = tenMinutesStopwatch.ElapsedTicks / Stopwatch.Frequency;
							if (elapsedSeconds < 600 )
							{
								Thread.Sleep((int)(60000 - elapsedSeconds*100));
							}
							counterTo500 = 0;
							tenMinutesStopwatch.Reset();
						}
						if (counter >= 10 && stopwatch.ElapsedTicks / Stopwatch.Frequency < 10)
						{
							Thread.Sleep(10000);
							counter = 0;
							stopwatch.Reset();
						}
						string query = string.Format("{0}{1}{2}{3}beginDate={4}&api_key={5}",queryRegion, queryBaseFirstPart, region, queryBaseSecondPart, i, api_key);
						client.DownloadFile(query, @"D:\LoL_Challange\Data\" + region + @"\" + i + ".json");
						Console.WriteLine("Downloaded file : " + i + ".json; " + "Number: " + globalCounter);
					}
					catch (Exception e)
					{
						Console.WriteLine("Error with timestamp: " + i);
					}
				}
				Thread.Sleep(10000);
				Console.WriteLine("Finished region: " + region);
			}
			Console.WriteLine("Finished!");
			Console.WriteLine("Press any key...");
			Console.ReadKey();



			
		}
	}
}
