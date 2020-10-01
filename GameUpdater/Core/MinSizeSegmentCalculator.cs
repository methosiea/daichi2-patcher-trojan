using System;
using System.Collections.Generic;

namespace GameUpdater.Core
{
	// Token: 0x0200001E RID: 30
	public class MinSizeSegmentCalculator : ISegmentCalculator
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00007B54 File Offset: 0x00005D54
		public CalculatedSegment[] GetSegments(int segmentCount, RemoteFileInfo remoteFileInfo)
		{
			long minSegmentSize = Settings.Default.MinSegmentSize;
			long num = remoteFileInfo.FileSize / (long)segmentCount;
			while (segmentCount > 1 && num < minSegmentSize)
			{
				segmentCount--;
				num = remoteFileInfo.FileSize / (long)segmentCount;
			}
			long num2 = 0L;
			List<CalculatedSegment> list = new List<CalculatedSegment>();
			for (int i = 0; i < segmentCount; i++)
			{
				bool flag = segmentCount - 1 == i;
				if (flag)
				{
					list.Add(new CalculatedSegment(num2, remoteFileInfo.FileSize));
				}
				else
				{
					list.Add(new CalculatedSegment(num2, num2 + num));
				}
				num2 = list[list.Count - 1].EndPosition;
			}
			return list.ToArray();
		}
	}
}
