namespace Services.DownloadService
{
	public static class DownloadManager
	{
		private static SequentialDownloadService _sequentialDownloadService;
		private static ParallelDownloadService _parallelDownloadService;

		static DownloadManager()
		{
			_sequentialDownloadService = new SequentialDownloadService();
			_parallelDownloadService = new ParallelDownloadService(2);
		}

		public static DownloadProgress QueueDownloadRequest(DownloadRequest downloadRequest)
		{
			DownloadProgress downloadProgress;

			if (downloadRequest.downloadMode == DownloadMode.Sequential)
			{
				downloadProgress = _sequentialDownloadService.QueueSequentialDownloadRequest(downloadRequest);
				return downloadProgress;
			}

			else if (downloadRequest.downloadMode == DownloadMode.Parallel)
			{
				downloadProgress = _parallelDownloadService.QueueParallelDownloadRequest(downloadRequest);
				return downloadProgress;
			}

			return null;
		}
	}
}
