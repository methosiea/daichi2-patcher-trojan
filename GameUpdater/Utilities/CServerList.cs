using System;
using System.Collections.Generic;

namespace GameUpdater.Utilities
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class CServerList
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004510 File Offset: 0x00002710
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00004518 File Offset: 0x00002718
		public string UpdaterVersion { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004521 File Offset: 0x00002721
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00004529 File Offset: 0x00002729
		public string RemoteUpdaterURL { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00004532 File Offset: 0x00002732
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000453A File Offset: 0x0000273A
		public string PatchListFile { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00004543 File Offset: 0x00002743
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000454B File Offset: 0x0000274B
		public List<string> MirrorList { get; set; }

		// Token: 0x06000079 RID: 121 RVA: 0x00004554 File Offset: 0x00002754
		public CServerList()
		{
			this.MirrorList = new List<string>();
		}
	}
}
