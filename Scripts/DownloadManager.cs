using System.Collections.Generic;

namespace Services.DownloadService
{
	/// <summary>
	/// Download manager for downloading files asynchronously from url
	/// </summary>
	public static class DownloadManager
	{
		public static List<DownloadProgress> DownloadProgressList { get; private set; } = new List<DownloadProgress>();

		private const int MAX_PARALELL_DOWNLOADS = 5;
		private static readonly SequentialDownloadService _sequentialDownloadService;
		private static readonly ParallelDownloadService _parallelDownloadService;

		static DownloadManager()
		{
			_sequentialDownloadService = new SequentialDownloadService();
			_parallelDownloadService = new ParallelDownloadService(MAX_PARALELL_DOWNLOADS);
		}

		public static DownloadProgress QueueDownloadRequest(DownloadRequest downloadRequest)
		{
			DownloadProgress downloadProgress = null;

			if (downloadRequest.downloadMode == DownloadMode.Sequential)
			{
				downloadProgress = _sequentialDownloadService.QueueSequentialDownloadRequest(downloadRequest);
			}

			else if (downloadRequest.downloadMode == DownloadMode.Parallel)
			{
				downloadProgress = _parallelDownloadService.QueueParallelDownloadRequest(downloadRequest);
			}

			if (downloadProgress != null)
			{
				DownloadProgressList.Add(downloadProgress);
				downloadProgress.OnDownloadFinished += delegate { DownloadProgressList.Remove(downloadProgress); };
			}

			return downloadProgress;
		}
	}
}
