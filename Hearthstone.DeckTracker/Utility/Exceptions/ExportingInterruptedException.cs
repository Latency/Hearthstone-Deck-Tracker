using System;


namespace HearthStone.DeckTracker.Utility.Exceptions
{
	public class ExportingInterruptedException : Exception
	{
		public ExportingInterruptedException(string message) : base(message)
		{
		}
	}
}
