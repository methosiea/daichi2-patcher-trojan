using System;

namespace GameUpdater.Core
{
	// Token: 0x0200001D RID: 29
	public interface ISegmentCalculator
	{
		// Token: 0x06000159 RID: 345
		CalculatedSegment[] GetSegments(int segmentCount, RemoteFileInfo fileSize);
	}
}
