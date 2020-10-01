using System;

namespace GameUpdater.Core
{
	// Token: 0x02000020 RID: 32
	[Serializable]
	public class RemoteFileInfo
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007DE8 File Offset: 0x00005FE8
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00007E00 File Offset: 0x00006000
		public string MimeType
		{
			get
			{
				return this.mimeType;
			}
			set
			{
				this.mimeType = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007E0C File Offset: 0x0000600C
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00007E24 File Offset: 0x00006024
		public bool AcceptRanges
		{
			get
			{
				return this.acceptRanges;
			}
			set
			{
				this.acceptRanges = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007E30 File Offset: 0x00006030
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00007E48 File Offset: 0x00006048
		public long FileSize
		{
			get
			{
				return this.fileSize;
			}
			set
			{
				this.fileSize = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007E54 File Offset: 0x00006054
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00007E6C File Offset: 0x0000606C
		public DateTime LastModified
		{
			get
			{
				return this.lastModified;
			}
			set
			{
				this.lastModified = value;
			}
		}

		// Token: 0x04000083 RID: 131
		private bool acceptRanges;

		// Token: 0x04000084 RID: 132
		private long fileSize;

		// Token: 0x04000085 RID: 133
		private DateTime lastModified = DateTime.MinValue;

		// Token: 0x04000086 RID: 134
		private string mimeType;
	}
}
