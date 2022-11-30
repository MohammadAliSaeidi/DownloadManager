using Services.Download;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DownloadManagerExample : MonoBehaviour
{
	[SerializeField]
	private Button btn_Download;

	[SerializeField]
	private InputField inp_FileURL;

	[SerializeField]
	private Slider slider_DownloadPercentage;

	private void Start()
	{
		btn_Download.onClick.AddListener(() => DownloadImage());
	}

	private void DownloadImage()
	{
		if (inp_FileURL.text.Length > 0)
		{
			var downloadRequest = new DownloadRequest()
			{
				downloadMode = DownloadMode.Sequential,
				downloadPriority = DownloadPriority.Normal,
				FileWebURLAddress = inp_FileURL.text,
				SavePath = Application.dataPath
			};

			var downloadProgress = DownloadManager.QueueDownloadRequest(downloadRequest);

			StartCoroutine(Co_UpdateUI(downloadProgress));
		}

	}
	private IEnumerator Co_UpdateUI(DownloadProgress downloadProgress)
	{
		while (downloadProgress.IsDownloading)
		{
			slider_DownloadPercentage.value = downloadProgress.GetDownloadingState().ProgressPercentage / 100.0f;
			yield return new WaitForSeconds(0.1f);
			yield return null;
		}
		slider_DownloadPercentage.value = 1;
	}
}
