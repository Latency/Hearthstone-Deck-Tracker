#region

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HearthStone.DeckTracker.Enums;
using HearthStone.DeckTracker.Properties;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.Attributes;

#endregion

namespace HearthStone.DeckTracker.Controls
{
	public partial class CardMarker : INotifyPropertyChanged
	{
		private int _cardAge;
		private Visibility _cardAgeVisibility;
		private int _costReduction;
		private Visibility _costReductionVisibility;
		private BitmapImage _icon;
		private Visibility _iconVisibility;
		private ScaleTransform _scaleTransform;

		public CardMarker()
		{
			InitializeComponent();
			_scaleTransform = new ScaleTransform(1, 1);
		}

		public int CardAge
		{
			get => _cardAge;
			set
			{
				_cardAge = value;
				OnPropertyChanged();
			}
		}

		public Visibility CardAgeVisibility
		{
			get => _cardAgeVisibility;
			set
			{
				_cardAgeVisibility = value;
				OnPropertyChanged();
			}
		}

		public BitmapImage Icon
		{
			get => _icon;
			set
			{
				_icon = value;
				OnPropertyChanged();
			}
		}

		public Visibility IconVisibility
		{
			get => _iconVisibility;
			set
			{
				_iconVisibility = value;
				OnPropertyChanged();
			}
		}

		public Visibility CostReductionVisibility
		{
			get => _costReductionVisibility;
			set
			{
				_costReductionVisibility = value;
				OnPropertyChanged();
			}
		}

		public int CostReduction
		{
			get => _costReduction;
			set
			{
				_costReduction = value;
				OnPropertyChanged();
			}
		}

		public ScaleTransform ScaleTransform
		{
			get => _scaleTransform;
			set
			{
				_scaleTransform = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void UpdateIcon(CardMark mark)
		{
			if(Helper.TryGetAttribute<AssetNameAttribute>(mark, out var assetName) && assetName.Name != null)
			{
				var path = Path.Combine("/HearthstoneDeckTracker;component", assetName.Name);
				Icon = new BitmapImage(new Uri(path, UriKind.Relative));
				IconVisibility = Visibility.Visible;
			}
			else
				IconVisibility = Visibility.Collapsed;
		}

		public void UpdateCardAge(int? cardAge)
		{
			if(cardAge.HasValue)
			{
				CardAge = cardAge.Value;
				CardAgeVisibility = Visibility.Visible;
			}
			else
				CardAgeVisibility = Visibility.Collapsed;
		}

		public void UpdateCostReduction(int costReduction)
		{
			CostReduction = -costReduction;
			CostReductionVisibility = costReduction > 0 ? Visibility.Visible : Visibility.Collapsed;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
