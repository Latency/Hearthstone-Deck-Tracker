#region

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using HearthStone.DeckTracker.Errors;
using HearthStone.DeckTracker.HsReplay;
using HearthStone.DeckTracker.HsReplay.Enums;
using HearthStone.DeckTracker.Properties;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.Logging;

#endregion

namespace HearthStone.DeckTracker.FlyoutControls.Options.Tracker
{
	/// <summary>
	/// Interaction logic for TrackerReplays.xaml
	/// </summary>
	public partial class TrackerReplays : INotifyPropertyChanged
	{
		public TrackerReplays()
		{
			InitializeComponent();
		}

		public Visibility TextClaimVisibility => Account.Instance.Status == AccountStatus.Anonymous ? Visibility.Visible : Visibility.Collapsed;
		public Visibility ClaimErrorVisibility => Account.Instance.Status == AccountStatus.Anonymous && !string.IsNullOrEmpty(_claimUrl) ? Visibility.Visible : Visibility.Collapsed;
		public bool TextUnclaimIsEnabled => Account.Instance.Status != AccountStatus.Anonymous;
		public AccountStatus AccountStatus => Account.Instance.Status;
		public string BattleTag => Account.Instance.Status == AccountStatus.Anonymous ? string.Empty : $"({Account.Instance.Username})";
		private const string ButtonTextClaim = "Claim Account";
		private const string ButtonTextWaiting = "Waiting for HSReplay.net...";
		private string _claimUrl;

		public void Update()
		{
			OnPropertyChanged(nameof(TextClaimVisibility));
			OnPropertyChanged(nameof(TextUnclaimIsEnabled));
			OnPropertyChanged(nameof(AccountStatus));
			OnPropertyChanged(nameof(BattleTag));
			OnPropertyChanged(nameof(ClaimErrorVisibility));
		}

		private void ButtonClaimAccount_OnClick(object sender, RoutedEventArgs e) => ClaimAccount();

		internal async void ClaimAccount()
		{
			ButtonClaimAccount.Content = ButtonTextWaiting;
			ButtonClaimAccount.IsEnabled = false;
			var url = await ApiWrapper.GetClaimAccountUrl();
			if(url == null)
				return;
			if(!Helper.TryOpenUrl(url))
			{
				_claimUrl = url;
				OnPropertyChanged(nameof(ClaimErrorVisibility));
			}
			await Task.Delay(3000);
			ButtonClaimAccount.IsEnabled = true;
			await CheckForAccountUpdateAsync(AccountStatus.Registered);
			ButtonClaimAccount.Content = ButtonTextClaim;
		}

		private bool _checkingForAccountUpdate;
		internal async Task CheckForAccountUpdateAsync(AccountStatus? targetStatus = null)
		{
			if(_checkingForAccountUpdate)
				return;
			ProgressRing.IsActive = true;
			_checkingForAccountUpdate = true;
			for(var i = 0; i < 10; i++)
			{
				Log.Debug($"Checking account info... try #{i+1}");
				await ApiWrapper.UpdateAccountStatus();
				if(targetStatus == null || Account.Instance.Status == targetStatus)
				{
					Log.Debug("Found updated account status");
					Update();
					break;
				}
				await Task.Delay(5000 + (int)Math.Sqrt(i * 5000));
			}
			ProgressRing.IsActive = false;
			_checkingForAccountUpdate = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void HyperlinkBattleTag_OnClick(object sender, RoutedEventArgs e)
		{
			var url = Helper.BuildHsReplayNetUrl("account", "myaccount");
			Helper.TryOpenUrl(url);
		}

		private void ButtonCopyUrl_OnClick(object sender, RoutedEventArgs e)
		{
			if(string.IsNullOrEmpty(_claimUrl))
				return;
			try
			{
				Clipboard.SetDataObject(_claimUrl);
				ButtonCopyUrl.Content = "Copied!";
			}
			catch(Exception ex)
			{
				ErrorManager.AddError("Could not copy the URL to clipboard :(", "Here is the url: " + _claimUrl + Environment.NewLine + ex.Message);
			}
		}
	}
}
