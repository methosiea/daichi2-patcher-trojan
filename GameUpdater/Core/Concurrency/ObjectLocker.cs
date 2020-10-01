using System;
using System.Threading;

namespace GameUpdater.Core.Concurrency
{
	// Token: 0x02000032 RID: 50
	public class ObjectLocker : IDisposable
	{
		// Token: 0x06000201 RID: 513 RVA: 0x00009095 File Offset: 0x00007295
		public ObjectLocker(object obj)
		{
			this.obj = obj;
			Monitor.Enter(this.obj);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000090B2 File Offset: 0x000072B2
		public void Dispose()
		{
			Monitor.Exit(this.obj);
		}

		// Token: 0x040000AE RID: 174
		private object obj;
	}
}
