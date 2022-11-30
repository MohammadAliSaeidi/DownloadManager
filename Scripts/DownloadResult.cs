using System;
using System.ComponentModel;

namespace Services.DownloadService
{
	public class DownloadResult : AsyncCompletedEventArgs
	{
		public DownloadResult(System.Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
		}
	}
}
