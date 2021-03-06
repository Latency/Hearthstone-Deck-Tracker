﻿#region

using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using HearthStone.DeckTracker.Plugins;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.Extensions;
using HearthStone.DeckTracker.Utility.Logging;
using HearthStone.DeckTracker.Windows;

#endregion

namespace HearthStone.DeckTracker.FlyoutControls.Options.Tracker
{
	/// <summary>
	/// Interaction logic for TrackerPlugins.xaml
	/// </summary>
	public partial class TrackerPlugins : UserControl
	{
		public TrackerPlugins()
		{
			InitializeComponent();
		}

		public void Load()
		{
			ListBoxPlugins.ItemsSource = PluginManager.Instance.Plugins;
			if(ListBoxPlugins.Items.Count > 0)
				ListBoxPlugins.SelectedIndex = 0;
			else
				GroupBoxDetails.Visibility = Visibility.Hidden;
		}

		private void ListBoxPlugins_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
		{
			(ListBoxPlugins.SelectedItem as PluginWrapper)?.OnButtonPress();
		}

		private void ButtonAvailablePlugins_OnClick(object sender, RoutedEventArgs e) => Helper.TryOpenUrl(@"https://github.com/HearthSim/Hearthstone-Deck-Tracker/wiki/Available-Plugins");

		private void ButtonOpenPluginsFolder_OnClick(object sender, RoutedEventArgs e)
		{
			var dir = PluginManager.PluginDirectory;
			if(!dir.Exists)
			{
				try
				{
					dir.Create();
				}
				catch(Exception)
				{
					MessageDialogs.ShowMessage(Core.MainWindow, "Error",
												$"Plugins directory was not found and can not be created. Please manually create a folder called 'Plugins' under {dir}.").Forget();
				}
			}
			Helper.TryOpenUrl(dir.FullName);
		}

		private void DockPanel_Drop(object sender, DragEventArgs e)
		{
			var dir = PluginManager.PluginDirectory.FullName;
			var prevCount = PluginManager.Instance.Plugins.Count;
			try
			{
				if(e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					
					var plugins = 0;
					var droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
					if(droppedFiles == null) 
						return;
					foreach(var pluginPath in droppedFiles)
					{
						if(pluginPath.EndsWith(".dll"))
						{
							File.Copy(pluginPath, Path.Combine(dir, Path.GetFileName(pluginPath)), true);
							plugins++;
						}
						else if(pluginPath.EndsWith(".zip"))
						{
							var path = Path.Combine(dir, Path.GetFileNameWithoutExtension(pluginPath));
							ZipFile.ExtractToDirectory(pluginPath, path);
							if (Directory.GetDirectories(path).Length == 1 && Directory.GetFiles(path).Length == 0)
							{
								Directory.Delete(path, true);
								ZipFile.ExtractToDirectory(pluginPath, dir);
							}
							plugins++;
						}
					}
					if(plugins <= 0) 
						return;
					PluginManager.Instance.LoadPlugins(PluginManager.Instance.SyncPlugins());
					if(prevCount == 0)
					{
						ListBoxPlugins.SelectedIndex = 0;
						GroupBoxDetails.Visibility = Visibility.Visible;
					}
				}
			}
			catch(Exception ex)
			{
				Log.Error(ex);
				MessageDialogs.ShowMessage(Core.MainWindow, "Error Importing Plugin", $"Please import manually to {dir}.").Forget();
			}
		}
	}
}
