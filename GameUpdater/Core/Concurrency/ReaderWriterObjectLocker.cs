using System;
using System.Threading;

namespace GameUpdater.Core.Concurrency
{
	// Token: 0x02000033 RID: 51
	public class ReaderWriterObjectLocker
	{
		// Token: 0x06000203 RID: 515 RVA: 0x000090C1 File Offset: 0x000072C1
		public ReaderWriterObjectLocker()
		{
			this.locker = new ReaderWriterLock();
			this.writerReleaser = new ReaderWriterObjectLocker.WriterReleaser(this);
			this.readerReleaser = new ReaderWriterObjectLocker.ReaderReleaser(this);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000090F0 File Offset: 0x000072F0
		public IDisposable LockForRead()
		{
			this.locker.AcquireReaderLock(-1);
			return this.readerReleaser;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00009118 File Offset: 0x00007318
		public IDisposable LockForWrite()
		{
			this.locker.AcquireWriterLock(-1);
			return this.writerReleaser;
		}

		// Token: 0x040000AF RID: 175
		private ReaderWriterLock locker;

		// Token: 0x040000B0 RID: 176
		private IDisposable writerReleaser;

		// Token: 0x040000B1 RID: 177
		private IDisposable readerReleaser;

		// Token: 0x0200003D RID: 61
		private class BaseReleaser
		{
			// Token: 0x0600022A RID: 554 RVA: 0x00009FE1 File Offset: 0x000081E1
			public BaseReleaser(ReaderWriterObjectLocker locker)
			{
				this.locker = locker;
			}

			// Token: 0x040000BF RID: 191
			protected ReaderWriterObjectLocker locker;
		}

		// Token: 0x0200003E RID: 62
		private class ReaderReleaser : ReaderWriterObjectLocker.BaseReleaser, IDisposable
		{
			// Token: 0x0600022B RID: 555 RVA: 0x00009FF2 File Offset: 0x000081F2
			public ReaderReleaser(ReaderWriterObjectLocker locker) : base(locker)
			{
			}

			// Token: 0x0600022C RID: 556 RVA: 0x00009FFD File Offset: 0x000081FD
			public void Dispose()
			{
				this.locker.locker.ReleaseReaderLock();
			}
		}

		// Token: 0x0200003F RID: 63
		private class WriterReleaser : ReaderWriterObjectLocker.BaseReleaser, IDisposable
		{
			// Token: 0x0600022D RID: 557 RVA: 0x00009FF2 File Offset: 0x000081F2
			public WriterReleaser(ReaderWriterObjectLocker locker) : base(locker)
			{
			}

			// Token: 0x0600022E RID: 558 RVA: 0x0000A011 File Offset: 0x00008211
			public void Dispose()
			{
				this.locker.locker.ReleaseWriterLock();
			}
		}
	}
}
