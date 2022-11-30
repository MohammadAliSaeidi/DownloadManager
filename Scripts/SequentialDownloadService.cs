using System.Collections.Generic;
using System.Linq;

namespace Services.Download
{
	internal class SequentialDownloadService
	{
		#region Download Priority Queues

		private Queue<DownloadProgress> _downloadPriorityQueueLow;
		private Queue<DownloadProgress> _downloadPriorityQueueNormal;
		private Queue<DownloadProgress> _downloadPriorityQueueHigh;

		#endregion


		private DownloadProgress _currentDownloadProgress;

		internal SequentialDownloadService()
		{
			_downloadPriorityQueueLow = new Queue<DownloadProgress>();
			_downloadPriorityQueueNormal = new Queue<DownloadProgress>();
			_downloadPriorityQueueHigh = new Queue<DownloadProgress>();
			_currentDownloadProgress = null;
		}

		internal DownloadProgress QueueSequentialDownloadRequest(DownloadRequest downloadRequest)
		{
			int downloadId = downloadRequest.GetHashCode();

			var downloadProgress = new DownloadProgress(downloadId, downloadRequest);

			AddToPriorityQueue(downloadProgress);

			downloadProgress.OnDownloadFinished += delegate (DownloadRequest request, DownloadResult result)
			{
				StartNextSeqDownload();
			};

			StartNextSeqDownload();

			return downloadProgress;
		}

		#region Private Methods

		private void AddToPriorityQueue(DownloadProgress downloadProgress)
		{
			switch (downloadProgress.downloadRequest.downloadPriority)
			{
				case DownloadPriority.Low:
				{
					_downloadPriorityQueueLow.Enqueue(downloadProgress);
				}
				break;

				case DownloadPriority.Normal:
				{
					_downloadPriorityQueueNormal.Enqueue(downloadProgress);
				}
				break;

				case DownloadPriority.High:
				{
					_downloadPriorityQueueHigh.Enqueue(downloadProgress);
				}
				break;
			}
		}

		private void StartNextSeqDownload()
		{
			if (_currentDownloadProgress == null || !_currentDownloadProgress.IsDownloading)
			{
				if (_downloadPriorityQueueHigh.Any())
				{
					_currentDownloadProgress = _downloadPriorityQueueHigh.Dequeue();
					_currentDownloadProgress.StartDownload();
				}

				else if (_downloadPriorityQueueNormal.Any())
				{
					_currentDownloadProgress = _downloadPriorityQueueNormal.Dequeue();
					_currentDownloadProgress.StartDownload();
				}

				else if (_downloadPriorityQueueLow.Any())
				{
					_currentDownloadProgress = _downloadPriorityQueueLow.Dequeue();
					_currentDownloadProgress.StartDownload();
				}
			}
		}

		#endregion
	}
}
