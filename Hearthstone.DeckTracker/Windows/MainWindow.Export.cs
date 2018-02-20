#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using HearthStone.Database;
using HearthStone.Database.Enums;
using HearthStone.DeckTracker.Errors;
using HearthStone.DeckTracker.Hearthstone;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.Extensions;
using HearthStone.DeckTracker.Utility.Logging;
using MahApps.Metro.Controls.Dialogs;
using Clipboard = System.Windows.Clipboard;

#endregion

namespace HearthStone.DeckTracker.Windows
{
	public partial class MainWindow
	{
		public void ShowExportFlyout(Deck deck)
		{
			DeckExportFlyout.Deck = deck.GetSelectedDeckVersion();
			FlyoutDeckExport.IsOpen = true;
		}

		public void ShowScreenshotFlyout()
		{
			DeckScreenshotFlyout.Deck = DeckPickerList.SelectedDecks.FirstOrDefault() ?? DeckList.Instance.ActiveDeck;
			FlyoutDeckScreenshot.IsOpen = true;
		}

		public void ShowDeckHistoryFlyout()
		{
			var deck = DeckPickerList.SelectedDecks.FirstOrDefault() ?? DeckList.Instance.ActiveDeck;
			if(!deck?.HasVersions ?? true)
				return;
			DeckHistoryFlyout.Deck = deck;
			FlyoutDeckHistory.IsOpen = true;
		}

		public async Task SaveOrUploadScreenshot(PngBitmapEncoder pngEncoder, string proposedFileName)
		{
			if(pngEncoder != null)
			{
				var saveOperation = await this.ShowScreenshotUploadSelectionDialog();
				if(saveOperation.Cancelled)
					return;
				var tmpFile = new FileInfo(Path.Combine(Config.Instance.DataDir, $"tmp{DateTime.Now.ToFileTime()}.png"));
				var fileName = saveOperation.SaveLocal
					               ? Helper.ShowSaveFileDialog(Helper.RemoveInvalidFileNameChars(proposedFileName), "png") : tmpFile.FullName;
				if(fileName != null)
				{
					string imgurUrl = null;
					using(var ms = new MemoryStream())
					using(var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
					{
						pngEncoder.Save(ms);
						ms.WriteTo(fs);
						if(saveOperation.Upload)
						{
							var controller = await DialogManager.ShowProgressAsync(this, "Uploading...", "");
							imgurUrl = await Imgur.Upload(Config.Instance.ImgurClientId, ms, proposedFileName);
							await controller.CloseAsync();
						}
					}

					if(imgurUrl != null)
					{
						await this.ShowSavedAndUploadedFileMessage(saveOperation.SaveLocal ? fileName : null, imgurUrl);
						Log.Info("Uploaded screenshot to " + imgurUrl);
					}
					else
						await this.ShowSavedFileMessage(fileName);
					Log.Info("Saved screenshot to: " + fileName);
				}
				if(tmpFile.Exists)
				{
					try
					{
						tmpFile.Delete();
					}
					catch(Exception ex)
					{
						Log.Error(ex);
					}
				}
			}
		}

		internal async void SaveDecksToDisk(IEnumerable<Deck> decks)
		{
			var selectedDecks = DeckPickerList.SelectedDecks;
			if (selectedDecks.Count > 1)
			{
				if(selectedDecks.Count > 10)
				{
					var result = await
						DialogManager.ShowMessageAsync(this, "Exporting multiple decks!", $"You are about to export {selectedDecks.Count} decks. Are you sure?",
											  MessageDialogStyle.AffirmativeAndNegative);
					if(result != MessageDialogResult.Affirmative)
						return;
				}
				var dialog = new FolderBrowserDialog();
				if(dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return;
				foreach(var deck in DeckPickerList.SelectedDecks)
				{
					//Helper.GetValidFilePath avoids overwriting files and properly handles duplicate deck names
					var saveLocation = Path.Combine(dialog.SelectedPath, Helper.GetValidFilePath(dialog.SelectedPath, deck.Name, "xml"));
					XmlManager<Deck>.Save(saveLocation, deck.GetSelectedDeckVersion());
					Log.Info($"Saved {deck.GetSelectedDeckVersion().GetDeckInfo()} to file: {saveLocation}");
				}
				await this.ShowSavedFileMessage(dialog.SelectedPath);

			}
			else if(selectedDecks.Count > 0)
			{
				var deck = selectedDecks.First();
				var fileName = Helper.ShowSaveFileDialog(Helper.RemoveInvalidFileNameChars(deck.Name), "xml");
				if(fileName == null)
					return;
				XmlManager<Deck>.Save(fileName, deck.GetSelectedDeckVersion());
				await this.ShowSavedFileMessage(fileName);
				Log.Info($"Saved {deck.GetSelectedDeckVersion().GetDeckInfo()} to file: {fileName}");
			}
		}

		internal void ExportIdsToClipboard(Deck deck)
		{
			if(deck == null)
				return;
			Clipboard.SetDataObject(Helper.DeckToIdString(deck.GetSelectedDeckVersion()));
			MessageDialogs.ShowMessage(this, "", "copied ids to clipboard").Forget();
			Log.Info("Copied " + deck.GetSelectedDeckVersion().GetDeckInfo() + " to clipboard");
		}

		internal async void ExportCardNamesToClipboard(Deck deck)
		{
			if(deck == null || !deck.GetSelectedDeckVersion().Cards.Any())
			{
				MessageDialogs.ShowMessage(this, "", LocUtil.Get("ShowMessage_CopyCardNames_NoCards")).Forget();
				return;
			}

			try
			{
				var selectedLanguage = await MessageDialogs.ShowSelectLanguageDialog(this);
				if(!selectedLanguage.IsCanceled)
				{
					Enum.TryParse(selectedLanguage.SelectedLanguage, out Locale myLang);
					var names = deck.GetSelectedDeckVersion().Cards.ToSortedCardList()
								.Select(c => (Cards.GetFromDbfId(c.DbfIf).GetLocName(myLang)) + (c.Count > 1 ? " x " + c.Count : ""))
								.Aggregate((c, n) => c + Environment.NewLine + n);

					Clipboard.SetDataObject(names);
					Log.Info("Copied " + deck.GetDeckInfo() + " names to clipboard");
				}
			}
			catch(Exception ex)
			{
				Log.Error(ex);
				ErrorManager.AddError("Error copying card names", LocUtil.Get("ShowMessage_CopyCardNames_Error"));
			}
		}

		internal async void ExportDeckFromWeb()
		{
			var result = await ImportDeckFromUrl();
			if(result.WasCancelled)
				return;
			if(result.Deck != null)
				ShowExportFlyout(result.Deck);
			else
				await DialogManager.ShowMessageAsync(this, "No deck found", "Could not find a deck on" + Environment.NewLine + result.Url);
		}
	}
}
