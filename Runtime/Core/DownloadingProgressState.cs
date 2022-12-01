namespace Chromium.Services.Download
{
	public class DownloadingProgressState
	{
		public readonly bool IsDownloading;
		public readonly long BytesReceived;
		public readonly int ProgressPercentage;
		public readonly long TotalBytesToReceive;

		internal DownloadingProgressState(bool isDownloading, long bytesReceived, int progressPercentage, long totalBytesToReceive)
		{
			IsDownloading = isDownloading;
			BytesReceived = bytesReceived;
			ProgressPercentage = progressPercentage;
			TotalBytesToReceive = totalBytesToReceive;
		}
	}
}
