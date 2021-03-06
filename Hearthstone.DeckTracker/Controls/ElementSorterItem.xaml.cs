﻿#region

using System;
using System.Windows;
using HearthStone.DeckTracker.Enums;

#endregion

namespace HearthStone.DeckTracker.Controls
{
	/// <summary>
	/// Interaction logic for ElementSorterItem.xaml
	/// </summary>
	public partial class ElementSorterItem
	{
		private readonly bool _initialized;
		private readonly bool _isPlayerList;
		private readonly Action<bool> _setConfigValue;
		public DeckPanel Panel { get; }

		public ElementSorterItem(DeckPanel panel, bool isChecked, Action<bool> setConfigValue, bool isPlayerList)
		{
			InitializeComponent();
			Panel = panel;
			CheckBox.IsChecked = isChecked;
			_setConfigValue = setConfigValue;
			_isPlayerList = isPlayerList;
			_initialized = true;
		}

		private void ButtonUp_OnClick(object sender, RoutedEventArgs e)
		{
			if(_isPlayerList)
			{
				Core.MainWindow.Options.OptionsOverlayPlayer.ElementSorterPlayer.MoveItem(this, SortDirection.Up);
				Core.Overlay.UpdatePlayerLayout();
				Core.Windows.PlayerWindow.UpdatePlayerLayout();
			}
			else
			{
				Core.MainWindow.Options.OptionsOverlayOpponent.ElementSorterOpponent.MoveItem(this, SortDirection.Up);
				Core.Overlay.UpdateOpponentLayout();
				Core.Windows.OpponentWindow.UpdateOpponentLayout();
			}
		}

		private void ButtonDown_OnClick(object sender, RoutedEventArgs e)
		{
			if(_isPlayerList)
			{
				Core.MainWindow.Options.OptionsOverlayPlayer.ElementSorterPlayer.MoveItem(this, SortDirection.Down);
				Core.Overlay.UpdatePlayerLayout();
				Core.Windows.PlayerWindow.UpdatePlayerLayout();
			}
			else
			{
				Core.MainWindow.Options.OptionsOverlayOpponent.ElementSorterOpponent.MoveItem(this, SortDirection.Down);
				Core.Overlay.UpdateOpponentLayout();
				Core.Windows.OpponentWindow.UpdateOpponentLayout();
			}
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if(!_initialized)
				return;
			_setConfigValue(true);
			Config.Save();
			Core.Overlay.Update(false);
			if(_isPlayerList)
			{
				Core.Windows.PlayerWindow.Update();
				Core.Windows.PlayerWindow.UpdatePlayerLayout();
			}
			else
			{
				Core.Windows.OpponentWindow.Update();
				Core.Windows.OpponentWindow.UpdateOpponentLayout();
			}
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if(!_initialized)
				return;
			_setConfigValue(false);
			Config.Save();
			Core.Overlay.Update(false);
			if(_isPlayerList)
			{
				Core.Windows.PlayerWindow.Update();
				Core.Windows.PlayerWindow.UpdatePlayerLayout();
			}
			else
			{
				Core.Windows.OpponentWindow.Update();
				Core.Windows.OpponentWindow.UpdateOpponentLayout();
			}
		}
	}
}
