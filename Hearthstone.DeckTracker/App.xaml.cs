﻿#region

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using HearthStone.DeckTracker.Errors;
using HearthStone.DeckTracker.Plugins;
using HearthStone.DeckTracker.Utility;
using HearthStone.DeckTracker.Utility.Extensions;
using HearthStone.DeckTracker.Windows;

#endregion

namespace HearthStone.DeckTracker
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private static bool _createdReport;

		private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var plugin = PluginManager.Instance.Plugins.FirstOrDefault(p => new FileInfo(p.FileName).Name.Replace(".dll", "") == e.Exception.Source);
			if(plugin != null)
			{
				var incompatibleExceptions = new[]
				{
					typeof(MissingMethodException), typeof(MissingFieldException),
					typeof(MissingMemberException), typeof(TypeLoadException)
				};
				string header;
				if(incompatibleExceptions.Any(x => e.Exception.GetType() == x))
				{
					plugin.IsEnabled = false;
					header = $"{plugin.NameAndVersion} is not compatible with HDT {Helper.GetCurrentVersion().ToVersionString()}.";
				}
				else if(plugin.UnhandledException())
				{
					plugin.IsEnabled = false;
					header = $"{plugin.NameAndVersion} threw too many exceptions and has been disabled.";
				}
				else
					header = $"{plugin.NameAndVersion} threw an exception.";
				ErrorManager.AddError(header, "Make sure you are using the latest version of the Plugin and HDT.\n\n" + e.Exception);
				e.Handled = true;
				return;
			}
			if(!_createdReport)
			{
				_createdReport = true;
				new CrashDialog(e.Exception).ShowDialog();
#if(!DEBUG)
				var date = DateTime.Now;
				var fileName = "Crash Reports\\" + $"Crash report {date.Day}{date.Month}{date.Year}-{date.Hour}{date.Minute}";

				if(!Directory.Exists("Crash Reports"))
					Directory.CreateDirectory("Crash Reports");

				using(var sr = new StreamWriter(fileName + ".txt", true))
				{
					sr.WriteLine("########## " + DateTime.Now + " ##########");
					sr.WriteLine(e.Exception);
					sr.WriteLine(Core.MainWindow.Options.OptionsTrackerLogging.TextBoxLog.Text);
				}
#endif
				e.Handled = true;
				Shutdown();
			}
			e.Handled = true;
		}
		
		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			ShutdownMode = ShutdownMode.OnExplicitShutdown;

		    var tempDir = Path.GetTempPath();
			var targetDir = Assembly.GetExecutingAssembly().Location;
			
			Console.WriteLine(@"Running Resource Generator!");
			// <path> <folder> <IsOverwriten> [msbuild]
			ResourceGenerator.Program.Run(new []{ $"{tempDir}Resources", "Tiles", "0", "msbuild" });
			Console.WriteLine($@"Sucessfully generated tiles in '{tempDir}Resources\Generated'");

		    Console.WriteLine($@"Copying Generated tiles from '{tempDir}Resources\Generated' to '{targetDir}Images\Tiles'");
			XCopy.Run($@"{tempDir}Resources\Generated", $@"{targetDir}Images\Tiles");
			
			Core.Initialize();
		}

		private void HandleManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e) => e.Handled = true;
	}
}
