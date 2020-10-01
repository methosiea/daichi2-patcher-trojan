using System;

namespace GameUpdater.Core
{
	// Token: 0x02000019 RID: 25
	public enum DownloaderState : byte
	{
		// Token: 0x0400006F RID: 111
		NeedToPrepare,
		// Token: 0x04000070 RID: 112
		Preparing,
		// Token: 0x04000071 RID: 113
		WaitingForReconnect,
		// Token: 0x04000072 RID: 114
		Prepared,
		// Token: 0x04000073 RID: 115
		Working,
		// Token: 0x04000074 RID: 116
		Pausing,
		// Token: 0x04000075 RID: 117
		Paused,
		// Token: 0x04000076 RID: 118
		Ended,
		// Token: 0x04000077 RID: 119
		EndedWithError
	}
}
