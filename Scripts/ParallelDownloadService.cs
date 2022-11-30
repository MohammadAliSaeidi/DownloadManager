using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace Services.DownloadService
{
	internal class ParallelDownloadService
	{
		private Queue<DownloadProgress> _parallelDownloadQueue;
		private int _activeParallelDownloadsCount;
		private readonly int _maxParallelDownloads;



		internal ParallelDownloadService(int maxParallelDownloads)
		{
			_parallelDownloadQueue = new Queue<DownloadProgress>();
			_maxParallelDownloads = maxParallelDownloads;
		}

		internal DownloadProgress QueueParallelDownloadRequest(DownloadRequest downloadRequest)
		{
			int downloadId = downloadRequest.GetHashCode();

			var downloadProgress = new DownloadProgress(downloadId, downloadRequest);

			AddToParallelQueue(downloadProgress);

			return downloadProgress;
		}

		#region Private Methods

		private void AddToParallelQueue(DownloadProgress downloadProgress)
		{
			_parallelDownloadQueue.Enqueue(downloadProgress);

			if (_activeParallelDownloadsCount < _maxParallelDownloads)
			{
				StartNextParallelDownload();
			}
		}

		private void StartNextParallelDownload()
		{
			if (_parallelDownloadQueue.Any())
			{
				var downloadProgress = _parallelDownloadQueue.Dequeue();
				downloadProgress.StartDownload();

				_activeParallelDownloadsCount++;

				downloadProgress.OnDownloadFinished += delegate (DownloadRequest downloadRequest, DownloadResult downloadResult)
				{
					_activeParallelDownloadsCount--;
					StartNextParallelDownload();
				};
			}
		}

		#endregion
	}
}
