using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GameUpdater.Core.Extensions.Protocols
{
	// Token: 0x02000031 RID: 49
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.6.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00008EF0 File Offset: 0x000070F0
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00008F08 File Offset: 0x00007108
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00008F2A File Offset: 0x0000712A
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string ProxyAddress
		{
			get
			{
				return (string)this["ProxyAddress"];
			}
			set
			{
				this["ProxyAddress"] = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00008F3C File Offset: 0x0000713C
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00008F5E File Offset: 0x0000715E
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string ProxyUserName
		{
			get
			{
				return (string)this["ProxyUserName"];
			}
			set
			{
				this["ProxyUserName"] = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00008F70 File Offset: 0x00007170
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00008F92 File Offset: 0x00007192
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string ProxyPassword
		{
			get
			{
				return (string)this["ProxyPassword"];
			}
			set
			{
				this["ProxyPassword"] = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00008FA4 File Offset: 0x000071A4
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x00008FC6 File Offset: 0x000071C6
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string ProxyDomain
		{
			get
			{
				return (string)this["ProxyDomain"];
			}
			set
			{
				this["ProxyDomain"] = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00008FD8 File Offset: 0x000071D8
		// (set) Token: 0x060001FA RID: 506 RVA: 0x00008FFA File Offset: 0x000071FA
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		public bool UseProxy
		{
			get
			{
				return (bool)this["UseProxy"];
			}
			set
			{
				this["UseProxy"] = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00009010 File Offset: 0x00007210
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00009032 File Offset: 0x00007232
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool ProxyByPassOnLocal
		{
			get
			{
				return (bool)this["ProxyByPassOnLocal"];
			}
			set
			{
				this["ProxyByPassOnLocal"] = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00009048 File Offset: 0x00007248
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0000906A File Offset: 0x0000726A
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("80")]
		public int ProxyPort
		{
			get
			{
				return (int)this["ProxyPort"];
			}
			set
			{
				this["ProxyPort"] = value;
			}
		}

		// Token: 0x040000AD RID: 173
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
