namespace HearthStone.DeckTracker.Plugins
{
	public class PluginSettings
	{
		public string FileName;
		public bool IsEnabled;
		public string Name;

		public PluginSettings()
		{
		}

		internal PluginSettings(PluginWrapper p)
		{
			FileName = p.RelativeFilePath;
			Name = p.Name;
			IsEnabled = p.IsEnabled;
		}
	}
}
