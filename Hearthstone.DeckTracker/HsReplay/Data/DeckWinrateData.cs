using System.Collections.Generic;


namespace HearthStone.DeckTracker.HsReplay.Data
{
	public class DeckWinrateData : HsReplayData
	{
		public double TotalWinrate { get; set; }
		public Dictionary<string, double> ClassWinrates { get; set; }
	}
}
