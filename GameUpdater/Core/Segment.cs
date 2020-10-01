using System;
using System.IO;

namespace GameUpdater.Core
{
	// Token: 0x02000023 RID: 35
	public class Segment
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008194 File Offset: 0x00006394
		// (set) Token: 0x06000188 RID: 392 RVA: 0x000081AC File Offset: 0x000063AC
		public int CurrentTry
		{
			get
			{
				return this.currentTry;
			}
			set
			{
				this.currentTry = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000189 RID: 393 RVA: 0x000081B8 File Offset: 0x000063B8
		// (set) Token: 0x0600018A RID: 394 RVA: 0x000081D0 File Offset: 0x000063D0
		public SegmentState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				switch (this.state)
				{
				case SegmentState.Connecting:
				case SegmentState.Paused:
				case SegmentState.Finished:
				case SegmentState.Error:
					this.rate = 0.0;
					this.left = TimeSpan.Zero;
					break;
				case SegmentState.Downloading:
					this.BeginWork();
					break;
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00008234 File Offset: 0x00006434
		public DateTime LastErrorDateTime
		{
			get
			{
				return this.lastErrorDateTime;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000824C File Offset: 0x0000644C
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00008264 File Offset: 0x00006464
		public Exception LastError
		{
			get
			{
				return this.lastError;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.lastErrorDateTime = DateTime.Now;
				}
				else
				{
					this.lastErrorDateTime = DateTime.MinValue;
				}
				this.lastError = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000082A0 File Offset: 0x000064A0
		// (set) Token: 0x0600018F RID: 399 RVA: 0x000082B8 File Offset: 0x000064B8
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000082C4 File Offset: 0x000064C4
		// (set) Token: 0x06000191 RID: 401 RVA: 0x000082DC File Offset: 0x000064DC
		public long InitialStartPosition
		{
			get
			{
				return this.initialStartPosition;
			}
			set
			{
				this.initialStartPosition = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000082E8 File Offset: 0x000064E8
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00008300 File Offset: 0x00006500
		public long StartPosition
		{
			get
			{
				return this.startPosition;
			}
			set
			{
				this.startPosition = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000830C File Offset: 0x0000650C
		public long Transfered
		{
			get
			{
				return this.StartPosition - this.InitialStartPosition;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000832C File Offset: 0x0000652C
		public long TotalToTransfer
		{
			get
			{
				bool flag = this.EndPosition > 0L;
				long result;
				if (flag)
				{
					result = this.EndPosition - this.InitialStartPosition;
				}
				else
				{
					result = 0L;
				}
				return result;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00008360 File Offset: 0x00006560
		public long MissingTransfer
		{
			get
			{
				bool flag = this.EndPosition > 0L;
				long result;
				if (flag)
				{
					result = this.EndPosition - this.StartPosition;
				}
				else
				{
					result = 0L;
				}
				return result;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00008394 File Offset: 0x00006594
		public double Progress
		{
			get
			{
				bool flag = this.EndPosition > 0L;
				double result;
				if (flag)
				{
					result = (double)this.Transfered / (double)this.TotalToTransfer * 100.0;
				}
				else
				{
					result = 0.0;
				}
				return result;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000083DC File Offset: 0x000065DC
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000083F4 File Offset: 0x000065F4
		public long EndPosition
		{
			get
			{
				return this.endPosition;
			}
			set
			{
				this.endPosition = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00008400 File Offset: 0x00006600
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00008418 File Offset: 0x00006618
		public Stream OutputStream
		{
			get
			{
				return this.outputStream;
			}
			set
			{
				this.outputStream = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00008424 File Offset: 0x00006624
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000843C File Offset: 0x0000663C
		public Stream InputStream
		{
			get
			{
				return this.inputStream;
			}
			set
			{
				this.inputStream = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00008448 File Offset: 0x00006648
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00008460 File Offset: 0x00006660
		public string CurrentURL
		{
			get
			{
				return this.currentURL;
			}
			set
			{
				this.currentURL = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000846C File Offset: 0x0000666C
		public double Rate
		{
			get
			{
				bool flag = this.State == SegmentState.Downloading;
				double result;
				if (flag)
				{
					this.IncreaseStartPosition(0L);
					result = this.rate;
				}
				else
				{
					result = 0.0;
				}
				return result;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000084A8 File Offset: 0x000066A8
		public TimeSpan Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000084C0 File Offset: 0x000066C0
		public void BeginWork()
		{
			this.start = this.startPosition;
			this.lastReception = DateTime.Now;
			this.started = true;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000084E4 File Offset: 0x000066E4
		public void IncreaseStartPosition(long size)
		{
			lock (this)
			{
				DateTime now = DateTime.Now;
				this.startPosition += size;
				bool flag2 = this.started;
				if (flag2)
				{
					TimeSpan timeSpan = now - this.lastReception;
					bool flag3 = timeSpan.TotalSeconds != 0.0;
					if (flag3)
					{
						this.rate = (double)(this.startPosition - this.start) / timeSpan.TotalSeconds;
						bool flag4 = this.rate > 0.0;
						if (flag4)
						{
							this.left = TimeSpan.FromSeconds((double)this.MissingTransfer / this.rate);
						}
						else
						{
							this.left = TimeSpan.MaxValue;
						}
					}
				}
				else
				{
					this.start = this.startPosition;
					this.lastReception = now;
					this.started = true;
				}
			}
		}

		// Token: 0x0400008F RID: 143
		private long startPosition;

		// Token: 0x04000090 RID: 144
		private int index;

		// Token: 0x04000091 RID: 145
		private string currentURL;

		// Token: 0x04000092 RID: 146
		private long initialStartPosition;

		// Token: 0x04000093 RID: 147
		private long endPosition;

		// Token: 0x04000094 RID: 148
		private Stream outputStream;

		// Token: 0x04000095 RID: 149
		private Stream inputStream;

		// Token: 0x04000096 RID: 150
		private Exception lastError;

		// Token: 0x04000097 RID: 151
		private SegmentState state;

		// Token: 0x04000098 RID: 152
		private bool started;

		// Token: 0x04000099 RID: 153
		private DateTime lastReception = DateTime.MinValue;

		// Token: 0x0400009A RID: 154
		private DateTime lastErrorDateTime = DateTime.MinValue;

		// Token: 0x0400009B RID: 155
		private double rate;

		// Token: 0x0400009C RID: 156
		private long start;

		// Token: 0x0400009D RID: 157
		private TimeSpan left = TimeSpan.Zero;

		// Token: 0x0400009E RID: 158
		private int currentTry;
	}
}
