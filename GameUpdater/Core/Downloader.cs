#define ADDON

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using GameUpdater.Core.Concurrency;

namespace GameUpdater.Core
{
	// Token: 0x02000017 RID: 23
	public class Downloader
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00005448 File Offset: 0x00003648
		private Downloader(ResourceLocation rl, ResourceLocation[] mirrors, string localFile)
		{
			this.threads = new List<Thread>();
			this.resourceLocation = rl;
			bool flag = mirrors == null;
			if (flag)
			{
				this.mirrors = new List<ResourceLocation>();
			}
			else
			{
				this.mirrors = new List<ResourceLocation>(mirrors);
			}
			this.localFile = localFile;
			this.extentedProperties = new Dictionary<string, object>();
			this.defaultDownloadProvider = rl.BindProtocolProviderInstance(this);
			this.segmentCalculator = new MinSizeSegmentCalculator();
			this.MirrorSelector = new SequentialMirrorSelector();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000054D5 File Offset: 0x000036D5
		public Downloader(ResourceLocation rl, ResourceLocation[] mirrors, string localFile, int segmentCount) : this(rl, mirrors, localFile)
		{
			this.SetState(DownloaderState.NeedToPrepare);
			this.createdDateTime = DateTime.Now;
			this.requestedSegmentCount = segmentCount;
			this.segments = new List<Segment>();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005508 File Offset: 0x00003708
		public Downloader(ResourceLocation rl, ResourceLocation[] mirrors, string localFile, List<Segment> segments, RemoteFileInfo remoteInfo, int requestedSegmentCount, DateTime createdDateTime) : this(rl, mirrors, localFile)
		{
			bool flag = segments.Count > 0;
			if (flag)
			{
				this.SetState(DownloaderState.Prepared);
			}
			else
			{
				this.SetState(DownloaderState.NeedToPrepare);
			}
			this.createdDateTime = createdDateTime;
			this.remoteFileInfo = remoteInfo;
			this.requestedSegmentCount = requestedSegmentCount;
			this.segments = segments;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000E6 RID: 230 RVA: 0x00005564 File Offset: 0x00003764
		// (remove) Token: 0x060000E7 RID: 231 RVA: 0x0000559C File Offset: 0x0000379C
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler Ending;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000E8 RID: 232 RVA: 0x000055D4 File Offset: 0x000037D4
		// (remove) Token: 0x060000E9 RID: 233 RVA: 0x0000560C File Offset: 0x0000380C
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler InfoReceived;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000EA RID: 234 RVA: 0x00005644 File Offset: 0x00003844
		// (remove) Token: 0x060000EB RID: 235 RVA: 0x0000567C File Offset: 0x0000387C
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler StateChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000EC RID: 236 RVA: 0x000056B4 File Offset: 0x000038B4
		// (remove) Token: 0x060000ED RID: 237 RVA: 0x000056EC File Offset: 0x000038EC
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler<SegmentEventArgs> RestartingSegment;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000EE RID: 238 RVA: 0x00005724 File Offset: 0x00003924
		// (remove) Token: 0x060000EF RID: 239 RVA: 0x0000575C File Offset: 0x0000395C
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler<SegmentEventArgs> SegmentStoped;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000F0 RID: 240 RVA: 0x00005794 File Offset: 0x00003994
		// (remove) Token: 0x060000F1 RID: 241 RVA: 0x000057CC File Offset: 0x000039CC
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler<SegmentEventArgs> SegmentStarting;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000F2 RID: 242 RVA: 0x00005804 File Offset: 0x00003A04
		// (remove) Token: 0x060000F3 RID: 243 RVA: 0x0000583C File Offset: 0x00003A3C
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler<SegmentEventArgs> SegmentStarted;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000F4 RID: 244 RVA: 0x00005874 File Offset: 0x00003A74
		// (remove) Token: 0x060000F5 RID: 245 RVA: 0x000058AC File Offset: 0x00003AAC
#if !ADDON
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		public event EventHandler<SegmentEventArgs> SegmentFailed;

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000058E4 File Offset: 0x00003AE4
		public Dictionary<string, object> ExtendedProperties
		{
			get
			{
				return this.extentedProperties;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000058FC File Offset: 0x00003AFC
		public ResourceLocation ResourceLocation
		{
			get
			{
				return this.resourceLocation;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005914 File Offset: 0x00003B14
		public List<ResourceLocation> Mirrors
		{
			get
			{
				return this.mirrors;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000592C File Offset: 0x00003B2C
		public long FileSize
		{
			get
			{
				bool flag = this.remoteFileInfo == null;
				long result;
				if (flag)
				{
					result = 0L;
				}
				else
				{
					result = this.remoteFileInfo.FileSize;
				}
				return result;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000595C File Offset: 0x00003B5C
		public DateTime CreatedDateTime
		{
			get
			{
				return this.createdDateTime;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00005974 File Offset: 0x00003B74
		public int RequestedSegments
		{
			get
			{
				return this.requestedSegmentCount;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000598C File Offset: 0x00003B8C
		public string LocalFile
		{
			get
			{
				return this.localFile;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000059A4 File Offset: 0x00003BA4
		public double Progress
		{
			get
			{
				int count = this.segments.Count;
				bool flag = count > 0;
				double result;
				if (flag)
				{
					double num = 0.0;
					for (int i = 0; i < count; i++)
					{
						num += this.segments[i].Progress;
					}
					result = num / (double)count;
				}
				else
				{
					result = 0.0;
				}
				return result;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005A10 File Offset: 0x00003C10
		public double Rate
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < this.segments.Count; i++)
				{
					num += this.segments[i].Rate;
				}
				return num;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005A60 File Offset: 0x00003C60
		public long Transfered
		{
			get
			{
				long num = 0L;
				for (int i = 0; i < this.segments.Count; i++)
				{
					num += this.segments[i].Transfered;
				}
				return num;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005AA8 File Offset: 0x00003CA8
		public TimeSpan Left
		{
			get
			{
				bool flag = this.Rate == 0.0;
				TimeSpan result;
				if (flag)
				{
					result = TimeSpan.MaxValue;
				}
				else
				{
					double num = 0.0;
					for (int i = 0; i < this.segments.Count; i++)
					{
						num += (double)this.segments[i].MissingTransfer;
					}
					result = TimeSpan.FromSeconds(num / this.Rate);
				}
				return result;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005B24 File Offset: 0x00003D24
		public List<Segment> Segments
		{
			get
			{
				return this.segments;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005B3C File Offset: 0x00003D3C
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005B54 File Offset: 0x00003D54
		public Exception LastError
		{
			get
			{
				return this.lastError;
			}
			set
			{
				this.lastError = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005B60 File Offset: 0x00003D60
		public DownloaderState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005B78 File Offset: 0x00003D78
		public bool IsWorking()
		{
			DownloaderState downloaderState = this.State;
			return downloaderState == DownloaderState.Preparing || downloaderState == DownloaderState.WaitingForReconnect || downloaderState == DownloaderState.Working;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005BA0 File Offset: 0x00003DA0
		public RemoteFileInfo RemoteFileInfo
		{
			get
			{
				return this.remoteFileInfo;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005BB8 File Offset: 0x00003DB8
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00005BD0 File Offset: 0x00003DD0
		public string StatusMessage
		{
			get
			{
				return this.statusMessage;
			}
			set
			{
				this.statusMessage = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005BDC File Offset: 0x00003DDC
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00005BF4 File Offset: 0x00003DF4
		public ISegmentCalculator SegmentCalculator
		{
			get
			{
				return this.segmentCalculator;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					throw new ArgumentNullException("value");
				}
				this.segmentCalculator = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005C20 File Offset: 0x00003E20
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005C38 File Offset: 0x00003E38
		public IMirrorSelector MirrorSelector
		{
			get
			{
				return this.mirrorSelector;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					throw new ArgumentNullException("value");
				}
				this.mirrorSelector = value;
				this.mirrorSelector.Init(this);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005C6E File Offset: 0x00003E6E
		private void SetState(DownloaderState value)
		{
			this.state = value;
			this.OnStateChanged();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005C7F File Offset: 0x00003E7F
		private void StartToPrepare()
		{
			this.mainThread = new Thread(new ParameterizedThreadStart(this.StartDownloadThreadProc));
			this.mainThread.Start(this.requestedSegmentCount);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005CB0 File Offset: 0x00003EB0
		private void StartPrepared()
		{
			this.mainThread = new Thread(new ThreadStart(this.RestartDownload));
			this.mainThread.Start();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005CD8 File Offset: 0x00003ED8
		protected virtual void OnRestartingSegment(Segment segment)
		{
			bool flag = this.RestartingSegment != null;
			if (flag)
			{
				this.RestartingSegment(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005D0C File Offset: 0x00003F0C
		protected virtual void OnSegmentStoped(Segment segment)
		{
			bool flag = this.SegmentStoped != null;
			if (flag)
			{
				this.SegmentStoped(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005D40 File Offset: 0x00003F40
		protected virtual void OnSegmentFailed(Segment segment)
		{
			bool flag = this.SegmentFailed != null;
			if (flag)
			{
				this.SegmentFailed(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005D74 File Offset: 0x00003F74
		protected virtual void OnSegmentStarting(Segment segment)
		{
			bool flag = this.SegmentStarting != null;
			if (flag)
			{
				this.SegmentStarting(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005DA8 File Offset: 0x00003FA8
		protected virtual void OnSegmentStarted(Segment segment)
		{
			bool flag = this.SegmentStarted != null;
			if (flag)
			{
				this.SegmentStarted(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005DDC File Offset: 0x00003FDC
		protected virtual void OnStateChanged()
		{
			bool flag = this.StateChanged != null;
			if (flag)
			{
				this.StateChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005E0C File Offset: 0x0000400C
		protected virtual void OnEnding()
		{
			bool flag = this.Ending != null;
			if (flag)
			{
				this.Ending(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005E3C File Offset: 0x0000403C
		protected virtual void OnInfoReceived()
		{
			bool flag = this.InfoReceived != null;
			if (flag)
			{
				this.InfoReceived(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005E6C File Offset: 0x0000406C
		public IDisposable LockSegments()
		{
			return new ObjectLocker(this.segments);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005E8C File Offset: 0x0000408C
		public void WaitForConclusion()
		{
			bool flag = !this.IsWorking() && this.mainThread != null && this.mainThread.IsAlive;
			if (flag)
			{
				this.mainThread.Join(TimeSpan.FromSeconds(1.0));
			}
			while (this.IsWorking())
			{
				Thread.Sleep(100);
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005EF0 File Offset: 0x000040F0
		public void Pause()
		{
			bool flag = this.state == DownloaderState.Preparing || this.state == DownloaderState.WaitingForReconnect;
			if (flag)
			{
				this.Segments.Clear();
				this.mainThread.Abort();
				this.mainThread = null;
				this.SetState(DownloaderState.NeedToPrepare);
			}
			else
			{
				bool flag2 = this.state == DownloaderState.Working;
				if (flag2)
				{
					this.SetState(DownloaderState.Pausing);
					while (!this.AllWorkersStopped(5))
					{
					}
					List<Thread> list = this.threads;
					List<Thread> obj = list;
					lock (obj)
					{
						this.threads.Clear();
					}
					this.mainThread.Abort();
					this.mainThread = null;
					bool flag4 = this.RemoteFileInfo != null && !this.RemoteFileInfo.AcceptRanges;
					if (flag4)
					{
						this.Segments[0].StartPosition = 0L;
					}
					this.SetState(DownloaderState.Paused);
				}
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006004 File Offset: 0x00004204
		public void Start()
		{
			bool flag = this.state == DownloaderState.NeedToPrepare;
			if (flag)
			{
				this.SetState(DownloaderState.Preparing);
				this.StartToPrepare();
			}
			else
			{
				bool flag2 = this.state != DownloaderState.Preparing && this.state != DownloaderState.Pausing && this.state != DownloaderState.Working && this.state != DownloaderState.WaitingForReconnect;
				if (flag2)
				{
					this.SetState(DownloaderState.Preparing);
					this.StartPrepared();
				}
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006070 File Offset: 0x00004270
		private void AllocLocalFile()
		{
			FileInfo fileInfo = new FileInfo(this.LocalFile);
			bool flag = !Directory.Exists(fileInfo.DirectoryName);
			if (flag)
			{
				Directory.CreateDirectory(fileInfo.DirectoryName);
			}
			using (FileStream fileStream = new FileStream(this.LocalFile, FileMode.Create, FileAccess.Write))
			{
				fileStream.SetLength(Math.Max(this.FileSize, 0L));
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000060EC File Offset: 0x000042EC
		private void StartDownloadThreadProc(object objSegmentCount)
		{
			this.SetState(DownloaderState.Preparing);
			int segmentCount = Math.Min((int)objSegmentCount, Settings.Default.MaxSegments);
			Stream inputStream = null;
			int num = 0;
			for (;;)
			{
				this.lastError = null;
				bool flag = this.state == DownloaderState.Pausing;
				if (flag)
				{
					break;
				}
				this.SetState(DownloaderState.Preparing);
				num++;
				try
				{
					this.remoteFileInfo = this.defaultDownloadProvider.GetFileInfo(this.ResourceLocation, out inputStream);
				}
				catch (ThreadAbortException)
				{
					this.SetState(DownloaderState.NeedToPrepare);
					return;
				}
				catch (Exception ex)
				{
					this.lastError = ex;
					bool flag2 = num < Settings.Default.MaxRetries;
					if (flag2)
					{
						this.SetState(DownloaderState.WaitingForReconnect);
						Thread.Sleep(TimeSpan.FromSeconds((double)Settings.Default.RetryDelay));
						continue;
					}
					this.lastError = ex;
					this.SetState(DownloaderState.EndedWithError);
					return;
				}
				goto IL_CC;
			}
			this.SetState(DownloaderState.NeedToPrepare);
			return;
			IL_CC:
			try
			{
				this.lastError = null;
				this.StartSegments(segmentCount, inputStream);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				this.lastError = ex2;
				this.SetState(DownloaderState.EndedWithError);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000623C File Offset: 0x0000443C
		private void StartSegments(int segmentCount, Stream inputStream)
		{
			this.OnInfoReceived();
			this.AllocLocalFile();
			bool flag = !this.remoteFileInfo.AcceptRanges;
			CalculatedSegment[] array;
			if (flag)
			{
				array = new CalculatedSegment[]
				{
					new CalculatedSegment(0L, this.remoteFileInfo.FileSize)
				};
			}
			else
			{
				array = this.SegmentCalculator.GetSegments(segmentCount, this.remoteFileInfo);
			}
			List<Thread> list = this.threads;
			List<Thread> obj = list;
			lock (obj)
			{
				this.threads.Clear();
			}
			List<Segment> list2 = this.segments;
			List<Segment> obj2 = list2;
			lock (obj2)
			{
				this.segments.Clear();
			}
			for (int i = 0; i < array.Length; i++)
			{
				Segment segment = new Segment();
				bool flag4 = i == 0;
				if (flag4)
				{
					segment.InputStream = inputStream;
				}
				segment.Index = i;
				segment.InitialStartPosition = array[i].StartPosition;
				segment.StartPosition = array[i].StartPosition;
				segment.EndPosition = array[i].EndPosition;
				this.segments.Add(segment);
			}
			this.RunSegments();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000063C0 File Offset: 0x000045C0
		private void RestartDownload()
		{
			int num = 0;
			Stream stream;
			RemoteFileInfo fileInfo;
			try
			{
				for (;;)
				{
					this.lastError = null;
					this.SetState(DownloaderState.Preparing);
					num++;
					try
					{
						fileInfo = this.defaultDownloadProvider.GetFileInfo(this.ResourceLocation, out stream);
						break;
					}
					catch (Exception ex)
					{
						this.lastError = ex;
						bool flag = num >= Settings.Default.MaxRetries;
						if (flag)
						{
							return;
						}
						this.SetState(DownloaderState.WaitingForReconnect);
						Thread.Sleep(TimeSpan.FromSeconds((double)Settings.Default.RetryDelay));
					}
				}
			}
			finally
			{
				this.SetState(DownloaderState.Prepared);
			}
			try
			{
				bool flag2 = !fileInfo.AcceptRanges || fileInfo.LastModified > this.RemoteFileInfo.LastModified || fileInfo.FileSize != this.RemoteFileInfo.FileSize;
				if (flag2)
				{
					this.remoteFileInfo = fileInfo;
					this.StartSegments(this.RequestedSegments, stream);
				}
				else
				{
					bool flag3 = stream != null;
					if (flag3)
					{
						stream.Dispose();
					}
					this.RunSegments();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				this.lastError = ex2;
				this.SetState(DownloaderState.EndedWithError);
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000651C File Offset: 0x0000471C
		private void RunSegments()
		{
			this.SetState(DownloaderState.Working);
			using (FileStream fileStream = new FileStream(this.LocalFile, FileMode.Open, FileAccess.Write))
			{
				for (int i = 0; i < this.Segments.Count; i++)
				{
					this.Segments[i].OutputStream = fileStream;
					this.StartSegment(this.Segments[i]);
				}
				while (!this.AllWorkersStopped(1000) || this.RestartFailedSegments())
				{
				}
			}
			for (int j = 0; j < this.Segments.Count; j++)
			{
				bool flag = this.Segments[j].State == SegmentState.Error;
				if (flag)
				{
					this.SetState(DownloaderState.EndedWithError);
					return;
				}
			}
			bool flag2 = this.State != DownloaderState.Pausing;
			if (flag2)
			{
				this.OnEnding();
			}
			this.SetState(DownloaderState.Ended);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000662C File Offset: 0x0000482C
		private bool RestartFailedSegments()
		{
			bool result = false;
			double num = 0.0;
			for (int i = 0; i < this.Segments.Count; i++)
			{
				bool flag = this.Segments[i].State == SegmentState.Error && this.Segments[i].LastErrorDateTime != DateTime.MinValue && (Settings.Default.MaxRetries == 0 || this.Segments[i].CurrentTry < Settings.Default.MaxRetries);
				if (flag)
				{
					result = true;
					TimeSpan timeSpan = DateTime.Now - this.Segments[i].LastErrorDateTime;
					bool flag2 = timeSpan.TotalSeconds >= (double)Settings.Default.RetryDelay;
					if (flag2)
					{
						Segment segment = this.Segments[i];
						int currentTry = segment.CurrentTry;
						segment.CurrentTry = currentTry + 1;
						this.StartSegment(this.Segments[i]);
						this.OnRestartingSegment(this.Segments[i]);
					}
					else
					{
						num = Math.Max(num, (double)(Settings.Default.RetryDelay * 1000) - timeSpan.TotalMilliseconds);
					}
				}
			}
			Thread.Sleep((int)num);
			return result;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006790 File Offset: 0x00004990
		private void StartSegment(Segment newSegment)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(this.SegmentThreadProc));
			thread.Start(newSegment);
			List<Thread> list = this.threads;
			List<Thread> obj = list;
			lock (obj)
			{
				this.threads.Add(thread);
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000067F8 File Offset: 0x000049F8
		private bool AllWorkersStopped(int timeOut)
		{
			bool flag = true;
			List<Thread> list = this.threads;
			List<Thread> obj = list;
			Thread[] array;
			lock (obj)
			{
				array = this.threads.ToArray();
			}
			foreach (Thread thread in array)
			{
				bool flag3 = thread.Join(timeOut);
				flag = (flag && flag3);
				bool flag4 = flag3;
				if (flag4)
				{
					list = this.threads;
					List<Thread> obj2 = list;
					lock (obj2)
					{
						this.threads.Remove(thread);
					}
				}
			}
			return flag;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000068CC File Offset: 0x00004ACC
		private void SegmentThreadProc(object objSegment)
		{
			Segment segment = (Segment)objSegment;
			segment.LastError = null;
			try
			{
				bool flag = segment.EndPosition > 0L && segment.StartPosition >= segment.EndPosition;
				if (flag)
				{
					segment.State = SegmentState.Finished;
					this.OnSegmentStoped(segment);
				}
				else
				{
					byte[] array = new byte[4096];
					segment.State = SegmentState.Connecting;
					this.OnSegmentStarting(segment);
					bool flag2 = segment.InputStream == null;
					if (flag2)
					{
						ResourceLocation nextResourceLocation = this.MirrorSelector.GetNextResourceLocation();
						IProtocolProvider protocolProvider = nextResourceLocation.BindProtocolProviderInstance(this);
						while (nextResourceLocation != this.ResourceLocation)
						{
							Stream stream;
							RemoteFileInfo fileInfo = protocolProvider.GetFileInfo(nextResourceLocation, out stream);
							bool flag3 = stream != null;
							if (flag3)
							{
								stream.Dispose();
							}
							bool flag4 = fileInfo.FileSize == this.remoteFileInfo.FileSize && fileInfo.AcceptRanges == this.remoteFileInfo.AcceptRanges;
							if (flag4)
							{
								break;
							}
							List<ResourceLocation> list = this.mirrors;
							List<ResourceLocation> obj = list;
							lock (obj)
							{
								this.mirrors.Remove(nextResourceLocation);
							}
							nextResourceLocation = this.MirrorSelector.GetNextResourceLocation();
							protocolProvider = nextResourceLocation.BindProtocolProviderInstance(this);
						}
						segment.InputStream = protocolProvider.CreateStream(nextResourceLocation, segment.StartPosition, segment.EndPosition);
						segment.CurrentURL = nextResourceLocation.URL;
					}
					else
					{
						segment.CurrentURL = this.resourceLocation.URL;
					}
					using (segment.InputStream)
					{
						this.OnSegmentStarted(segment);
						segment.State = SegmentState.Downloading;
						segment.CurrentTry = 0;
						for (;;)
						{
							long num = (long)segment.InputStream.Read(array, 0, array.Length);
							bool flag6 = segment.EndPosition > 0L && segment.StartPosition + num > segment.EndPosition;
							if (flag6)
							{
								num = segment.EndPosition - segment.StartPosition;
								bool flag7 = num <= 0L;
								if (flag7)
								{
									break;
								}
							}
							Stream outputStream = segment.OutputStream;
							Stream obj2 = outputStream;
							lock (obj2)
							{
								segment.OutputStream.Position = segment.StartPosition;
								segment.OutputStream.Write(array, 0, (int)num);
							}
							segment.IncreaseStartPosition(num);
							bool flag9 = segment.EndPosition > 0L && segment.StartPosition >= segment.EndPosition;
							if (flag9)
							{
								goto Block_21;
							}
							bool flag10 = this.state == DownloaderState.Pausing;
							if (flag10)
							{
								goto Block_22;
							}
							bool flag11 = num <= 0L;
							if (flag11)
							{
								goto Block_23;
							}
						}
						segment.StartPosition = segment.EndPosition;
						goto IL_2E4;
						Block_21:
						segment.StartPosition = segment.EndPosition;
						goto IL_2E4;
						Block_22:
						segment.State = SegmentState.Paused;
						Block_23:
						IL_2E4:
						bool flag12 = segment.State == SegmentState.Downloading;
						if (flag12)
						{
							segment.State = SegmentState.Finished;
							this.AddNewSegmentIfNeeded();
						}
					}
					this.OnSegmentStoped(segment);
				}
			}
			catch (Exception ex)
			{
				segment.State = SegmentState.Error;
				segment.LastError = ex;
				this.OnSegmentFailed(segment);
			}
			finally
			{
				segment.InputStream = null;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006CA4 File Offset: 0x00004EA4
		private void AddNewSegmentIfNeeded()
		{
			List<Segment> list = this.segments;
			List<Segment> obj = list;
			lock (obj)
			{
				for (int i = 0; i < this.segments.Count; i++)
				{
					Segment segment = this.segments[i];
					bool flag2 = segment.State == SegmentState.Downloading && segment.Left.TotalSeconds > (double)Settings.Default.MinSegmentLeftToStartNewSegment && segment.MissingTransfer / 2L >= Settings.Default.MinSegmentSize;
					if (flag2)
					{
						long num = segment.MissingTransfer / 2L;
						Segment segment2 = new Segment();
						segment2.Index = this.segments.Count;
						segment2.StartPosition = segment.StartPosition + num;
						segment2.InitialStartPosition = segment2.StartPosition;
						segment2.EndPosition = segment.EndPosition;
						segment2.OutputStream = segment.OutputStream;
						segment.EndPosition -= num;
						this.segments.Add(segment2);
						this.StartSegment(segment2);
						break;
					}
				}
			}
		}

		// Token: 0x0400005C RID: 92
		private string localFile;

		// Token: 0x0400005D RID: 93
		private int requestedSegmentCount;

		// Token: 0x0400005E RID: 94
		private ResourceLocation resourceLocation;

		// Token: 0x0400005F RID: 95
		private List<ResourceLocation> mirrors;

		// Token: 0x04000060 RID: 96
		private List<Segment> segments;

		// Token: 0x04000061 RID: 97
		private Thread mainThread;

		// Token: 0x04000062 RID: 98
		private List<Thread> threads;

		// Token: 0x04000063 RID: 99
		private RemoteFileInfo remoteFileInfo;

		// Token: 0x04000064 RID: 100
		private DownloaderState state;

		// Token: 0x04000065 RID: 101
		private DateTime createdDateTime;

		// Token: 0x04000066 RID: 102
		private Exception lastError;

		// Token: 0x04000067 RID: 103
		private Dictionary<string, object> extentedProperties = new Dictionary<string, object>();

		// Token: 0x04000068 RID: 104
		private IProtocolProvider defaultDownloadProvider;

		// Token: 0x04000069 RID: 105
		private ISegmentCalculator segmentCalculator;

		// Token: 0x0400006A RID: 106
		private IMirrorSelector mirrorSelector;

		// Token: 0x0400006B RID: 107
		private string statusMessage;
	}
}
