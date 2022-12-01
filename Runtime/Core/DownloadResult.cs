using System;
using System.ComponentModel;

namespace Chromium.Services.Download
{
	public class DownloadResult : AsyncCompletedEventArgs
	{
		internal DownloadResult(System.Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
		}
	}
}
