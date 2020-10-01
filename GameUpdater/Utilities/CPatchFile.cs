using System;
using System.Collections.Generic;

namespace GameUpdater.Utilities
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class CPatchFile
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004339 File Offset: 0x00002539
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00004341 File Offset: 0x00002541
		public string FileName { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000434A File Offset: 0x0000254A
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00004352 File Offset: 0x00002552
		public string FilePath { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000435B File Offset: 0x0000255B
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00004363 File Offset: 0x00002563
		public int Size { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000436C File Offset: 0x0000256C
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00004374 File Offset: 0x00002574
		public List<CFileChunk> Checksum { get; set; }

		// Token: 0x06000061 RID: 97 RVA: 0x0000437D File Offset: 0x0000257D
		public CPatchFile()
		{
			this.Checksum = new List<CFileChunk>();
		}
	}
}
