using System;
using System.Collections.Generic;

namespace GameUpdater.Core
{
	// Token: 0x02000026 RID: 38
	public class SequentialMirrorSelector : IMirrorSelector
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00008646 File Offset: 0x00006846
		public void Init(Downloader downloader)
		{
			this.queryMirrorCount = 0;
			this.downloader = downloader;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008658 File Offset: 0x00006858
		public ResourceLocation GetNextResourceLocation()
		{
			bool flag = this.downloader.Mirrors == null || this.downloader.Mirrors.Count == 0;
			ResourceLocation result;
			if (flag)
			{
				result = this.downloader.ResourceLocation;
			}
			else
			{
				List<ResourceLocation> mirrors = this.downloader.Mirrors;
				List<ResourceLocation> obj = mirrors;
				ResourceLocation resourceLocation;
				lock (obj)
				{
					bool flag3 = this.queryMirrorCount >= this.downloader.Mirrors.Count;
					if (flag3)
					{
						this.queryMirrorCount = 0;
						resourceLocation = this.downloader.ResourceLocation;
					}
					else
					{
						List<ResourceLocation> mirrors2 = this.downloader.Mirrors;
						int num = this.queryMirrorCount;
						this.queryMirrorCount = num + 1;
						resourceLocation = mirrors2[num];
					}
				}
				result = resourceLocation;
			}
			return result;
		}

		// Token: 0x040000A7 RID: 167
		private Downloader downloader;

		// Token: 0x040000A8 RID: 168
		private int queryMirrorCount;
	}
}
