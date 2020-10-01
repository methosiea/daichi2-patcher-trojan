using System;
using System.IO;

namespace GameUpdater.Core
{
	// Token: 0x0200001C RID: 28
	public interface IProtocolProvider
	{
		// Token: 0x06000156 RID: 342
		void Initialize(Downloader downloader);

		// Token: 0x06000157 RID: 343
		Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition);

		// Token: 0x06000158 RID: 344
		RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream stream);
	}
}
