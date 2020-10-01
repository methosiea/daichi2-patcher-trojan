using System;

namespace GameUpdater.Core.Extensions.Protocols
{
	// Token: 0x02000030 RID: 48
	public interface IHttpFtpProtocolParameters
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001E2 RID: 482
		// (set) Token: 0x060001E3 RID: 483
		string ProxyAddress { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001E4 RID: 484
		// (set) Token: 0x060001E5 RID: 485
		string ProxyUserName { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001E6 RID: 486
		// (set) Token: 0x060001E7 RID: 487
		string ProxyPassword { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001E8 RID: 488
		// (set) Token: 0x060001E9 RID: 489
		string ProxyDomain { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001EA RID: 490
		// (set) Token: 0x060001EB RID: 491
		bool UseProxy { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001EC RID: 492
		// (set) Token: 0x060001ED RID: 493
		bool ProxyByPassOnLocal { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001EE RID: 494
		// (set) Token: 0x060001EF RID: 495
		int ProxyPort { get; set; }
	}
}
