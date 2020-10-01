using System;

namespace GameUpdater.Core.Extensions.Protocols
{
	// Token: 0x0200002E RID: 46
	internal class HttpFtpProtocolParametersSettingsProxy : IHttpFtpProtocolParameters
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00008C24 File Offset: 0x00006E24
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00008C40 File Offset: 0x00006E40
		public string ProxyAddress
		{
			get
			{
				return Settings.Default.ProxyAddress;
			}
			set
			{
				Settings.Default.ProxyAddress = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00008C50 File Offset: 0x00006E50
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00008C6C File Offset: 0x00006E6C
		public string ProxyUserName
		{
			get
			{
				return Settings.Default.ProxyUserName;
			}
			set
			{
				Settings.Default.ProxyUserName = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00008C7C File Offset: 0x00006E7C
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00008C98 File Offset: 0x00006E98
		public string ProxyPassword
		{
			get
			{
				return Settings.Default.ProxyPassword;
			}
			set
			{
				Settings.Default.ProxyPassword = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00008CA8 File Offset: 0x00006EA8
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00008CC4 File Offset: 0x00006EC4
		public string ProxyDomain
		{
			get
			{
				return Settings.Default.ProxyDomain;
			}
			set
			{
				Settings.Default.ProxyDomain = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00008CD4 File Offset: 0x00006ED4
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00008CF0 File Offset: 0x00006EF0
		public bool UseProxy
		{
			get
			{
				return Settings.Default.UseProxy;
			}
			set
			{
				Settings.Default.UseProxy = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00008D00 File Offset: 0x00006F00
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00008D1C File Offset: 0x00006F1C
		public bool ProxyByPassOnLocal
		{
			get
			{
				return Settings.Default.ProxyByPassOnLocal;
			}
			set
			{
				Settings.Default.ProxyByPassOnLocal = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00008D2C File Offset: 0x00006F2C
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00008D48 File Offset: 0x00006F48
		public int ProxyPort
		{
			get
			{
				return Settings.Default.ProxyPort;
			}
			set
			{
				Settings.Default.ProxyPort = value;
			}
		}
	}
}
