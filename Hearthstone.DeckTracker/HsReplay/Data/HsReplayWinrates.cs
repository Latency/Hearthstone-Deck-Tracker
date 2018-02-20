using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinrateData = System.Collections.Generic.Dictionary<string, HearthStone.DeckTracker.HsReplay.Data.DeckWinrateData>;

namespace HearthStone.DeckTracker.HsReplay.Data
{
	public class HsReplayWinrates : JsonCache<Dictionary<string, DeckWinrateData>>
	{
		private bool _cleaned;
		private WinrateData _data;
		private async Task<WinrateData> GetData() => _data ?? (_data = await LoadFromDisk() ?? new WinrateData());

		public HsReplayWinrates() : base("hsreplay_winrates.cache")
		{
		}

		public async Task<DeckWinrateData> Get(string shortId, bool wild)
		{
			var data = await GetData();
			if(data.TryGetValue(shortId, out var deck) && !deck.IsStale)
				return deck;
			if(!_cleaned)
				Cleanup();
			deck = await ApiWrapper.GetDeckWinrates(shortId, wild) ?? NoDataFallback;
			_data[shortId] = deck;
			await WriteToDisk(data);
			return deck;
		}

		private void Cleanup()
		{
			var stale = _data.Where(x => x.Value.IsStale).ToList();
			foreach(var s in stale)
				_data.Remove(s.Key);
			_cleaned = true;
		}

		public DeckWinrateData NoDataFallback => new DeckWinrateData
		{
			ClientTimeStamp = DateTime.Now.Subtract(TimeSpan.FromHours(23.5))
		};
	}
}
