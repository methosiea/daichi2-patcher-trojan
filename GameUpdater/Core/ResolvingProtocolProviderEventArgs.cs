using System;

namespace GameUpdater.Core
{
	// Token: 0x02000021 RID: 33
	public class ResolvingProtocolProviderEventArgs : EventArgs
	{
		// Token: 0x0600016F RID: 367 RVA: 0x00007E8A File Offset: 0x0000608A
		public ResolvingProtocolProviderEventArgs(IProtocolProvider provider, string url)
		{
			this.url = url;
			this.provider = provider;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007EA4 File Offset: 0x000060A4
		public string URL
		{
			get
			{
				return this.url;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00007EBC File Offset: 0x000060BC
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00007ED4 File Offset: 0x000060D4
		public IProtocolProvider ProtocolProvider
		{
			get
			{
				return this.provider;
			}
			set
			{
				this.provider = value;
			}
		}

		// Token: 0x04000087 RID: 135
		private IProtocolProvider provider;

		// Token: 0x04000088 RID: 136
		private string url;
	}
}
