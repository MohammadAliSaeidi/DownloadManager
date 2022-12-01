using System;
using System.ComponentModel;
using System.Net;
using static UnityEngine.Debug;

namespace Chromium.Services.Download
{
	public class DownloadProgress
	{
		public readonly int downloadId;
		public readonly DownloadRequest downloadRequest;
		public event OnDownloadFinishedDelegate OnDownloadFinished;

		public bool IsDownloading { get; private set; }
		public long BytesReceived { get; private set; }
		public int ProgressPercentage { get; private set; }
		public long TotalBytesToReceive { get; private set; }

		private readonly string fileInfoStr;

		internal DownloadProgress(int downloadId, DownloadRequest downloadRequest)
		{
			this.downloadId = downloadId;
			this.downloadRequest = downloadRequest;

			// for log
			fileInfoStr = $"<b>File URL:</b> {downloadRequest.FileWebURLAddress}" +
						$"\n<b>File save path:</b> {downloadRequest.SavePath}";
		}

		internal void StartDownload()
		{
			using var webClient = new WebClient();
			webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompleteEventHandler);
			webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

			// Make sure it starts with "http://"
			var uri = new Uri(downloadRequest.FileWebURLAddress);
			string fileName = System.IO.Path.GetFileName(uri.LocalPath);

			try
			{
				webClient.DownloadFileAsync(uri, $"{downloadRequest.SavePath}/{fileName}");

				IsDownloading = true;

				Log($"<b><color=#00B149>↓</color> Downloading file: </b>{Uri.EscapeUriString(fileName)}");
			}
			catch (Exception e)
			{
				IsDownloading = false;

				Log("<color=#CF6679><b>Download was failed</b></color>" +
										$"\n<color=#CF6679><b>Error message:</b></color> {e.Message}" + fileInfoStr);
			}
		}

		public DownloadingProgressState GetDownloadingState()
		{
			Log($"<b>Download Id:</b> {downloadId}\n" +
				$"<b>Is Downloading:</b> {IsDownloading}" +
				$"<b>Bytes Received:</b> {BytesReceived}" +
				$"<b>Progress Percentage:</b> {ProgressPercentage}" +
				$"<b>Total Bytes To Receive:</b> {TotalBytesToReceive}");

			var downloadingState = new DownloadingProgressState(IsDownloading, BytesReceived, ProgressPercentage, TotalBytesToReceive);

			return downloadingState;
		}

		private void CompleteEventHandler(object sender, AsyncCompletedEventArgs e)
		{
			var downloadResult = new DownloadResult(e.Error, e.Cancelled, e.UserState);
			IsDownloading = false;
			OnDownloadFinished?.Invoke(downloadRequest, downloadResult);

			if (downloadResult.Error == null)
			{
				Log("<color=#00B149><b>Downloaded the file successfully</b></color>" +
					$"\n{fileInfoStr}");
			}
			else
			{
				Log("<color=#CF6679><b>Download was failed</b></color>" +
						$"\n<color=#CF6679><b>Error message:</b></color> {downloadResult.Error.Message}" +
						$"\n{fileInfoStr}");
			}

			if (downloadResult.Cancelled)
			{
				Log("Downloading file has been canceled" + fileInfoStr);
			}
		}

		private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			BytesReceived = e.BytesReceived;
			ProgressPercentage = e.ProgressPercentage;
			TotalBytesToReceive = e.TotalBytesToReceive;
		}
	}
}