namespace Services.Download
{
	public class DownloadRequest
	{
		public string FileWebURLAddress { get; set; }
		public string SavePath { get; set; }
		public DownloadPriority downloadPriority { get; set; }
		public DownloadMode downloadMode { get; set; }
	}
}
