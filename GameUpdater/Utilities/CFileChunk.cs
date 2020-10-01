using System;

namespace GameUpdater.Utilities
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class CFileChunk
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000042FD File Offset: 0x000024FD
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00004305 File Offset: 0x00002505
		public int StartIndex { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0000430E File Offset: 0x0000250E
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00004316 File Offset: 0x00002516
		public int Size { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000056 RID: 86 RVA: 0x0000431F File Offset: 0x0000251F
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00004327 File Offset: 0x00002527
		public string Hash { get; set; }
	}
}
