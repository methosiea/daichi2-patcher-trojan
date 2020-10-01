using System;

namespace GameUpdater.Core
{
	// Token: 0x02000024 RID: 36
	public class SegmentEventArgs : DownloaderEventArgs
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x00008612 File Offset: 0x00006812
		public SegmentEventArgs(Downloader d, Segment segment) : base(d)
		{
			this.segment = segment;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00008624 File Offset: 0x00006824
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000863C File Offset: 0x0000683C
		public Segment Segment
		{
			get
			{
				return this.segment;
			}
			set
			{
				this.segment = value;
			}
		}

		// Token: 0x0400009F RID: 159
		private Segment segment;
	}
}
