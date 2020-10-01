using System;

namespace GameUpdater
{
	// Token: 0x02000003 RID: 3
	public enum EState
	{
		// Token: 0x04000006 RID: 6
		NONE,
		// Token: 0x04000007 RID: 7
		SERVER_LIST,
		// Token: 0x04000008 RID: 8
		CHECK_PATCHER,
		// Token: 0x04000009 RID: 9
		UPDATE_PATCHER,
		// Token: 0x0400000A RID: 10
		FETCH_PATCHLIST,
		// Token: 0x0400000B RID: 11
		CHECK_FILES,
		// Token: 0x0400000C RID: 12
		PREPARE_DOWNLOAD,
		// Token: 0x0400000D RID: 13
		DOWNLOAD_FILES,
		// Token: 0x0400000E RID: 14
		CLEAR_FILES,
		// Token: 0x0400000F RID: 15
		CHECK_UPDATE,
		// Token: 0x04000010 RID: 16
		COMPLETE
	}
}
