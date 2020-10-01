using System;

namespace GameUpdater.Core
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public struct CalculatedSegment
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005404 File Offset: 0x00003604
		public long StartPosition
		{
			get
			{
				return this.startPosition;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000541C File Offset: 0x0000361C
		public long EndPosition
		{
			get
			{
				return this.endPosition;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005434 File Offset: 0x00003634
		public CalculatedSegment(long startPos, long endPos)
		{
			this.endPosition = endPos;
			this.startPosition = startPos;
		}

		// Token: 0x04000052 RID: 82
		private long startPosition;

		// Token: 0x04000053 RID: 83
		private long endPosition;
	}
}
