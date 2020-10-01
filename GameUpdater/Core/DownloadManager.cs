using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GameUpdater.Core.Concurrency;

namespace GameUpdater.Core
{
	// Token: 0x0200001A RID: 26
	public class DownloadManager
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00006E54 File Offset: 0x00005054
		public static DownloadManager Instance
		{
			get
			{
				return DownloadManager.instance;
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600012B RID: 299 RVA: 0x00006E6C File Offset: 0x0000506C
		// (remove) Token: 0x0600012C RID: 300 RVA: 0x00006EA4 File Offset: 0x000050A4
		// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler BeginAddBatchDownloads;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600012D RID: 301 RVA: 0x00006EDC File Offset: 0x000050DC
		// (remove) Token: 0x0600012E RID: 302 RVA: 0x00006F14 File Offset: 0x00005114
		// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler EndAddBatchDownloads;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600012F RID: 303 RVA: 0x00006F4C File Offset: 0x0000514C
		// (remove) Token: 0x06000130 RID: 304 RVA: 0x00006F84 File Offset: 0x00005184
		// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<DownloaderEventArgs> DownloadEnded;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000131 RID: 305 RVA: 0x00006FBC File Offset: 0x000051BC
		// (remove) Token: 0x06000132 RID: 306 RVA: 0x00006FF4 File Offset: 0x000051F4
		// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<DownloaderEventArgs> DownloadAdded;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000133 RID: 307 RVA: 0x0000702C File Offset: 0x0000522C
		// (remove) Token: 0x06000134 RID: 308 RVA: 0x00007064 File Offset: 0x00005264
		// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<DownloaderEventArgs> DownloadRemoved;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000709C File Offset: 0x0000529C
		public ReadOnlyCollection<Downloader> Downloads
		{
			get
			{
				return this.downloads.AsReadOnly();
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000136 RID: 310 RVA: 0x000070BC File Offset: 0x000052BC
		public int TotalActiveDownloads
		{
			get
			{
				int num = 0;
				using (this.LockDownloadList(false))
				{
					for (int i = 0; i < this.Downloads.Count; i++)
					{
						bool flag = this.Downloads[i].IsWorking();
						if (flag)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00007134 File Offset: 0x00005334
		public double TotalDownloadRate
		{
			get
			{
				double num = 0.0;
				using (this.LockDownloadList(false))
				{
					for (int i = 0; i < this.Downloads.Count; i++)
					{
						bool flag = this.Downloads[i].State == DownloaderState.Working;
						if (flag)
						{
							num += this.Downloads[i].Rate;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000071C8 File Offset: 0x000053C8
		public double TotalDownloadSize
		{
			get
			{
				double num = 0.0;
				using (this.LockDownloadList(false))
				{
					for (int i = 0; i < this.Downloads.Count; i++)
					{
						bool flag = this.Downloads[i].State == DownloaderState.Working;
						if (flag)
						{
							num += (double)this.Downloads[i].FileSize;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000725C File Offset: 0x0000545C
		public double TotalDownloadedBytes
		{
			get
			{
				double num = 0.0;
				using (this.LockDownloadList(false))
				{
					for (int i = 0; i < this.Downloads.Count; i++)
					{
						bool flag = this.Downloads[i].State == DownloaderState.Working || this.Downloads[i].State == DownloaderState.Ended;
						if (flag)
						{
							num += (double)this.Downloads[i].Transfered;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00007308 File Offset: 0x00005508
		public int CompletedDownloads
		{
			get
			{
				int num = 0;
				using (this.LockDownloadList(false))
				{
					for (int i = 0; i < this.Downloads.Count; i++)
					{
						bool flag = this.Downloads[i].State == DownloaderState.Ended;
						if (flag)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00007384 File Offset: 0x00005584
		public int TotalDownloads
		{
			get
			{
				return this.Downloads.Count;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000073A4 File Offset: 0x000055A4
		public int FailedDownloads
		{
			get
			{
				int num = 0;
				using (this.LockDownloadList(false))
				{
					for (int i = 0; i < this.Downloads.Count; i++)
					{
						bool flag = this.Downloads[i].State == DownloaderState.EndedWithError;
						if (flag)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00007420 File Offset: 0x00005620
		public int AddBatchCount
		{
			get
			{
				return this.addBatchCount;
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007438 File Offset: 0x00005638
		private void downloader_StateChanged(object sender, EventArgs e)
		{
			Downloader downloader = (Downloader)sender;
			bool flag = downloader.State == DownloaderState.Ended || downloader.State == DownloaderState.EndedWithError;
			if (flag)
			{
				this.OnDownloadEnded((Downloader)sender);
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007478 File Offset: 0x00005678
		public IDisposable LockDownloadList(bool lockForWrite)
		{
			IDisposable result;
			if (lockForWrite)
			{
				result = this.downloadListSync.LockForWrite();
			}
			else
			{
				result = this.downloadListSync.LockForRead();
			}
			return result;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000074A9 File Offset: 0x000056A9
		public void RemoveDownload(int index)
		{
			this.RemoveDownload(this.downloads[index]);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000074C0 File Offset: 0x000056C0
		public void RemoveDownload(Downloader downloader)
		{
			bool flag = downloader.State != DownloaderState.NeedToPrepare || downloader.State != DownloaderState.Ended || downloader.State != DownloaderState.Paused;
			if (flag)
			{
				downloader.Pause();
			}
			using (this.LockDownloadList(true))
			{
				this.downloads.Remove(downloader);
			}
			this.OnDownloadRemoved(downloader);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007538 File Offset: 0x00005738
		public void ClearEnded()
		{
			using (this.LockDownloadList(true))
			{
				for (int i = this.downloads.Count - 1; i >= 0; i--)
				{
					bool flag = this.downloads[i].State == DownloaderState.Ended;
					if (flag)
					{
						Downloader d = this.downloads[i];
						this.downloads.RemoveAt(i);
						this.OnDownloadRemoved(d);
					}
				}
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000075CC File Offset: 0x000057CC
		public void ClearAll()
		{
			using (this.LockDownloadList(true))
			{
				this.downloads.Clear();
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007610 File Offset: 0x00005810
		public void PauseAll()
		{
			using (this.LockDownloadList(false))
			{
				for (int i = 0; i < this.Downloads.Count; i++)
				{
					this.Downloads[i].Pause();
				}
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007674 File Offset: 0x00005874
		public void StartAll()
		{
			using (this.LockDownloadList(false))
			{
				int num = 0;
				while (num < this.Downloads.Count && this.TotalActiveDownloads < Settings.Default.SimultaneousDownloadCount)
				{
					this.Downloads[num].Start();
					num++;
				}
			}
			bool flag = this.Downloads.Count == 0;
			if (flag)
			{
				this.OnDownloadEnded(null);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007708 File Offset: 0x00005908
		public void StartNextDownload()
		{
			using (this.LockDownloadList(false))
			{
				int num = 0;
				while (num < this.Downloads.Count && this.TotalActiveDownloads <= Settings.Default.SimultaneousDownloadCount)
				{
					bool flag = !this.Downloads[num].IsWorking() && this.Downloads[num].State != DownloaderState.Ended && this.Downloads[num].State != DownloaderState.EndedWithError;
					if (flag)
					{
						this.Downloads[num].Start();
					}
					num++;
				}
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000077D0 File Offset: 0x000059D0
		public Downloader Add(ResourceLocation rl, ResourceLocation[] mirrors, string localFile, int segments, bool autoStart)
		{
			Downloader downloader = new Downloader(rl, mirrors, localFile, segments);
			this.Add(downloader, autoStart);
			return downloader;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000077F8 File Offset: 0x000059F8
		public Downloader Add(string remoteFilePath, List<string> mirrors, string localFile, int segments, bool autoStart)
		{
			List<ResourceLocation> list = new List<ResourceLocation>();
			bool flag = mirrors.Count > 1;
			if (flag)
			{
				foreach (string str in mirrors.GetRange(1, mirrors.Count - 1))
				{
					list.Add(ResourceLocation.FromURL(str + remoteFilePath));
				}
			}
			Downloader downloader = new Downloader(ResourceLocation.FromURL(mirrors[0] + remoteFilePath), list.ToArray(), localFile, segments);
			this.Add(downloader, autoStart);
			return downloader;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000078B0 File Offset: 0x00005AB0
		public Downloader Add(ResourceLocation rl, ResourceLocation[] mirrors, string localFile, List<Segment> segments, RemoteFileInfo remoteInfo, int requestedSegmentCount, bool autoStart, DateTime createdDateTime)
		{
			Downloader downloader = new Downloader(rl, mirrors, localFile, segments, remoteInfo, requestedSegmentCount, createdDateTime);
			this.Add(downloader, autoStart);
			return downloader;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000078E0 File Offset: 0x00005AE0
		public void Add(Downloader downloader, bool autoStart)
		{
			downloader.StateChanged += this.downloader_StateChanged;
			using (this.LockDownloadList(true))
			{
				this.downloads.Add(downloader);
			}
			this.OnDownloadAdded(downloader, autoStart);
			if (autoStart)
			{
				downloader.Start();
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000794C File Offset: 0x00005B4C
		public virtual void OnBeginAddBatchDownloads()
		{
			this.addBatchCount++;
			bool flag = this.BeginAddBatchDownloads != null;
			if (flag)
			{
				this.BeginAddBatchDownloads(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000798C File Offset: 0x00005B8C
		public virtual void OnEndAddBatchDownloads()
		{
			this.addBatchCount--;
			bool flag = this.EndAddBatchDownloads != null;
			if (flag)
			{
				this.EndAddBatchDownloads(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000079CC File Offset: 0x00005BCC
		protected virtual void OnDownloadEnded(Downloader d)
		{
			bool flag = this.DownloadEnded != null;
			if (flag)
			{
				this.DownloadEnded(this, new DownloaderEventArgs(d));
				this.StartNextDownload();
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007A04 File Offset: 0x00005C04
		protected virtual void OnDownloadAdded(Downloader d, bool willStart)
		{
			bool flag = this.DownloadAdded != null;
			if (flag)
			{
				this.DownloadAdded(this, new DownloaderEventArgs(d, willStart));
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007A38 File Offset: 0x00005C38
		protected virtual void OnDownloadRemoved(Downloader d)
		{
			bool flag = this.DownloadRemoved != null;
			if (flag)
			{
				this.DownloadRemoved(this, new DownloaderEventArgs(d));
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007A68 File Offset: 0x00005C68
		public void SwapDownloads(int idx, bool isThreadSafe)
		{
			if (isThreadSafe)
			{
				this.InternalSwap(idx);
			}
			else
			{
				using (this.LockDownloadList(true))
				{
					this.InternalSwap(idx);
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007AB8 File Offset: 0x00005CB8
		private void InternalSwap(int idx)
		{
			int count = this.downloads.Count;
			Downloader item = this.downloads[idx];
			Downloader item2 = this.downloads[idx - 1];
			this.downloads.RemoveAt(idx);
			this.downloads.RemoveAt(idx - 1);
			this.downloads.Insert(idx - 1, item);
			this.downloads.Insert(idx, item2);
		}

		// Token: 0x0400007D RID: 125
		private static DownloadManager instance = new DownloadManager();

		// Token: 0x0400007E RID: 126
		private List<Downloader> downloads = new List<Downloader>();

		// Token: 0x0400007F RID: 127
		private int addBatchCount;

		// Token: 0x04000080 RID: 128
		private ReaderWriterObjectLocker downloadListSync = new ReaderWriterObjectLocker();
	}
}
