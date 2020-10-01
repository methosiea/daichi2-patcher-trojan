using System;
using System.Diagnostics;

namespace GameUpdater.Core.Instrumentation
{
	// Token: 0x02000028 RID: 40
	public class MyStopwatch : IDisposable
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000489C File Offset: 0x00002A9C
		public MyStopwatch(string name)
		{
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000028E4 File Offset: 0x00000AE4
		public void Dispose()
		{
		}

		// Token: 0x040000AA RID: 170
		private Stopwatch internalStopwatch;

		// Token: 0x040000AB RID: 171
		private string name;
	}
}
