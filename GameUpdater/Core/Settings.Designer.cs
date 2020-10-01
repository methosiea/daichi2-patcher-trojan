using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GameUpdater.Core
{
	// Token: 0x02000027 RID: 39
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00008744 File Offset: 0x00006944
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000875C File Offset: 0x0000695C
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000877E File Offset: 0x0000697E
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("200000")]
		public long MinSegmentSize
		{
			get
			{
				return (long)this["MinSegmentSize"];
			}
			set
			{
				this["MinSegmentSize"] = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00008794 File Offset: 0x00006994
		// (set) Token: 0x060001AF RID: 431 RVA: 0x000087B6 File Offset: 0x000069B6
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("30")]
		public int MinSegmentLeftToStartNewSegment
		{
			get
			{
				return (int)this["MinSegmentLeftToStartNewSegment"];
			}
			set
			{
				this["MinSegmentLeftToStartNewSegment"] = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000087CC File Offset: 0x000069CC
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x000087EE File Offset: 0x000069EE
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("3")]
		public int RetryDelay
		{
			get
			{
				return (int)this["RetryDelay"];
			}
			set
			{
				this["RetryDelay"] = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00008804 File Offset: 0x00006A04
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00008826 File Offset: 0x00006A26
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("2")]
		public int MaxRetries
		{
			get
			{
				return (int)this["MaxRetries"];
			}
			set
			{
				this["MaxRetries"] = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000883C File Offset: 0x00006A3C
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000885E File Offset: 0x00006A5E
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("1")]
		public int MaxSegments
		{
			get
			{
				return (int)this["MaxSegments"];
			}
			set
			{
				this["MaxSegments"] = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00008874 File Offset: 0x00006A74
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00008896 File Offset: 0x00006A96
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string DownloadFolder
		{
			get
			{
				return (string)this["DownloadFolder"];
			}
			set
			{
				this["DownloadFolder"] = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000088A8 File Offset: 0x00006AA8
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x000088CA File Offset: 0x00006ACA
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("3")]
		public int SimultaneousDownloadCount
		{
			get
			{
				return (int)this["SimultaneousDownloadCount"];
			}
			set
			{
				this["SimultaneousDownloadCount"] = value;
			}
		}

		// Token: 0x040000A9 RID: 169
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
