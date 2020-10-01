using System;

namespace GameUpdater.Core
{
	// Token: 0x02000018 RID: 24
	public class DownloaderEventArgs : EventArgs
	{
		// Token: 0x06000126 RID: 294 RVA: 0x00006E00 File Offset: 0x00005000
		public DownloaderEventArgs(Downloader download)
		{
			this.downloader = download;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006E11 File Offset: 0x00005011
		public DownloaderEventArgs(Downloader download, bool willStart) : this(download)
		{
			this.willStart = willStart;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00006E24 File Offset: 0x00005024
		public Downloader Downloader
		{
			get
			{
				return this.downloader;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006E3C File Offset: 0x0000503C
		public bool WillStart
		{
			get
			{
				return this.willStart;
			}
		}

		// Token: 0x0400006C RID: 108
		private Downloader downloader;

		// Token: 0x0400006D RID: 109
		private bool willStart;
	}
}
