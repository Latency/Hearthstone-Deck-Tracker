﻿#region

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using HearthStone.DeckTracker.Stats;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.Logging;

#endregion

namespace HearthStone.DeckTracker.Replay
{
	public static class ReplayMaker
	{
		public static string SaveToDisk(GameStats gameStats, List<string> powerLog)
		{
			try
			{
				var fileName = $"{gameStats.PlayerName}({gameStats.PlayerHero}) vs "
					+ $"{gameStats.OpponentName}({gameStats.OpponentHero}) {DateTime.Now:HHmm-ddMMyy}";

				if(!Directory.Exists(Config.Instance.ReplayDir))
					Directory.CreateDirectory(Config.Instance.ReplayDir);
				var path = Helper.GetValidFilePath(Config.Instance.ReplayDir, fileName, ".hdtreplay");
				using(var ms = new MemoryStream())
				{
					using(var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
					{
						var hsLog = archive.CreateEntry("output_log.txt");
						using(var logStream = hsLog.Open())
						using(var swLog = new StreamWriter(logStream))
							powerLog?.ForEach(swLog.WriteLine);
					}

					using(var fileStream = new FileStream(path, FileMode.Create))
					{
						ms.Seek(0, SeekOrigin.Begin);
						ms.CopyTo(fileStream);
					}
				}
				return fileName + ".hdtreplay";
			}
			catch(Exception e)
			{
				Log.Error(e);
				return null;
			}
		}
	}
}
