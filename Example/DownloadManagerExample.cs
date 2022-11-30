using UnityEngine;
using Services.DownloadService;

	public class DownloadManagerExample : MonoBehaviour
	{
		[ContextMenu("Download Example File")]
		public void DownloadExampleFile()
		{
			var downloadRequest = new DownloadRequest()
			{
				downloadMode = DownloadMode.Sequential,
				downloadPriority = DownloadPriority.Normal,
				FileWebURLAddress = "https://upload.wikimedia.org/wikipedia/en/thumb/8/80/Wikipedia-logo-v2.svg/1200px-Wikipedia-logo-v2.svg.png",
				SavePath = Application.dataPath
			};
			var downloadProgress = DownloadManager.QueueDownloadRequest(downloadRequest);

			System.Threading.Tasks.Task.Run(() =>
			{
				while (downloadProgress.IsDownloading)
				{
					Debug.Log(downloadProgress.GetDownloadingState().ProgressPercentage);
					System.Threading.Thread.Sleep(500);
				}
			});
		}
	}
