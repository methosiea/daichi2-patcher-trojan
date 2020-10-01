using System;

namespace GameUpdater.Core
{
	// Token: 0x0200001B RID: 27
	public interface IMirrorSelector
	{
		// Token: 0x06000154 RID: 340
		void Init(Downloader downloader);

		// Token: 0x06000155 RID: 341
		ResourceLocation GetNextResourceLocation();
	}
}
