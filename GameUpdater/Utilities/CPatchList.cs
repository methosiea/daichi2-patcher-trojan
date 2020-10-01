using System;
using System.Collections.Generic;

namespace GameUpdater.Utilities
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class CPatchList
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00004393 File Offset: 0x00002593
		// (set) Token: 0x06000063 RID: 99 RVA: 0x0000439B File Offset: 0x0000259B
		public string WebsiteURL { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000043A4 File Offset: 0x000025A4
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000043AC File Offset: 0x000025AC
		public string ForumURL { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000043B5 File Offset: 0x000025B5
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000043BD File Offset: 0x000025BD
		public string SupportURL { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000043C6 File Offset: 0x000025C6
		// (set) Token: 0x06000069 RID: 105 RVA: 0x000043CE File Offset: 0x000025CE
		public string LauncherFileName { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000043D7 File Offset: 0x000025D7
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000043DF File Offset: 0x000025DF
		public List<CPatchFile> Files { get; set; }

		// Token: 0x0600006C RID: 108 RVA: 0x000043E8 File Offset: 0x000025E8
		public CPatchList()
		{
			this.Files = new List<CPatchFile>();
		}
	}
}
