using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GameUpdater.Localization.Languages
{
	// Token: 0x02000014 RID: 20
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class de
	{
		// Token: 0x0600009E RID: 158 RVA: 0x0000489C File Offset: 0x00002A9C
		internal de()
		{
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004A2C File Offset: 0x00002C2C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				bool flag = de.resourceMan == null;
				if (flag)
				{
					de.resourceMan = new ResourceManager("GameUpdater.Localization.Languages.de", typeof(de).Assembly);
				}
				return de.resourceMan;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004A70 File Offset: 0x00002C70
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00004A87 File Offset: 0x00002C87
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return de.resourceCulture;
			}
			set
			{
				de.resourceCulture = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004A90 File Offset: 0x00002C90
		public static string ACTION_CHECKING_CLIENT_FILES
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_CHECKING_CLIENT_FILES", de.resourceCulture);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public static string ACTION_CHECKING_UPDATES
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_CHECKING_UPDATES", de.resourceCulture);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public static string ACTION_CLEANING_CLIENT_FILES
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_CLEANING_CLIENT_FILES", de.resourceCulture);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004B08 File Offset: 0x00002D08
		public static string ACTION_CLIENT_READY
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_CLIENT_READY", de.resourceCulture);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004B30 File Offset: 0x00002D30
		public static string ACTION_CONNECTING
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_CONNECTING", de.resourceCulture);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004B58 File Offset: 0x00002D58
		public static string ACTION_DOWNLOAD_PENDING
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_DOWNLOAD_PENDING", de.resourceCulture);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004B80 File Offset: 0x00002D80
		public static string ACTION_DOWNLOADING
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_DOWNLOADING", de.resourceCulture);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public static string ACTION_DOWNLOADING_PATCHER_UPDATE
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_DOWNLOADING_PATCHER_UPDATE", de.resourceCulture);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004BD0 File Offset: 0x00002DD0
		public static string ACTION_FETCHING_PATCHLIST
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_FETCHING_PATCHLIST", de.resourceCulture);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004BF8 File Offset: 0x00002DF8
		public static string ACTION_PENDING_PATCHER_UPDATE
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_PENDING_PATCHER_UPDATE", de.resourceCulture);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004C20 File Offset: 0x00002E20
		public static string ACTION_PREPARING_DOWNLOAD
		{
			get
			{
				return de.ResourceManager.GetString("ACTION_PREPARING_DOWNLOAD", de.resourceCulture);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004C48 File Offset: 0x00002E48
		public static string ERROR_CANNOT_LAUNCH_CONFIG
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_CANNOT_LAUNCH_CONFIG", de.resourceCulture);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004C70 File Offset: 0x00002E70
		public static string ERROR_CANNOT_LAUNCH_GAME
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_CANNOT_LAUNCH_GAME", de.resourceCulture);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004C98 File Offset: 0x00002E98
		public static string ERROR_CREATE_INSTALL_PATH
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_CREATE_INSTALL_PATH", de.resourceCulture);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004CC0 File Offset: 0x00002EC0
		public static string ERROR_DESERIALIZE_PATCHLIST
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_DESERIALIZE_PATCHLIST", de.resourceCulture);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004CE8 File Offset: 0x00002EE8
		public static string ERROR_DESERIALIZE_SERVERLIST
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_DESERIALIZE_SERVERLIST", de.resourceCulture);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004D10 File Offset: 0x00002F10
		public static string ERROR_MOVING_COMPLETED_FILE
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_MOVING_COMPLETED_FILE", de.resourceCulture);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004D38 File Offset: 0x00002F38
		public static string ERROR_MULTIPLE_INSTANCES_FOUND
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_MULTIPLE_INSTANCES_FOUND", de.resourceCulture);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004D60 File Offset: 0x00002F60
		public static string ERROR_UNHANDLED_EXCEPTION
		{
			get
			{
				return de.ResourceManager.GetString("ERROR_UNHANDLED_EXCEPTION", de.resourceCulture);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004D88 File Offset: 0x00002F88
		public static string INSTALL_DIR_NAME
		{
			get
			{
				return de.ResourceManager.GetString("INSTALL_DIR_NAME", de.resourceCulture);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public static string PROMPT_NEW_PATCHER_VERSION_AVAILABLE
		{
			get
			{
				return de.ResourceManager.GetString("PROMPT_NEW_PATCHER_VERSION_AVAILABLE", de.resourceCulture);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004DD8 File Offset: 0x00002FD8
		public static string PROMPT_NEW_UPDATES_AVAILABLE
		{
			get
			{
				return de.ResourceManager.GetString("PROMPT_NEW_UPDATES_AVAILABLE", de.resourceCulture);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004E00 File Offset: 0x00003000
		public static string PROMPT_REDOWNLOAD_MISSING_UPDATES
		{
			get
			{
				return de.ResourceManager.GetString("PROMPT_REDOWNLOAD_MISSING_UPDATES", de.resourceCulture);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004E28 File Offset: 0x00003028
		public static string REMAINING_TIME
		{
			get
			{
				return de.ResourceManager.GetString("REMAINING_TIME", de.resourceCulture);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004E50 File Offset: 0x00003050
		public static string TITLE
		{
			get
			{
				return de.ResourceManager.GetString("TITLE", de.resourceCulture);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004E78 File Offset: 0x00003078
		public static string TRAY_INFO_DEFAULT
		{
			get
			{
				return de.ResourceManager.GetString("TRAY_INFO_DEFAULT", de.resourceCulture);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004EA0 File Offset: 0x000030A0
		public static string TRAY_INFO_NEW_PATCHER_AVAILABLE
		{
			get
			{
				return de.ResourceManager.GetString("TRAY_INFO_NEW_PATCHER_AVAILABLE", de.resourceCulture);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004EC8 File Offset: 0x000030C8
		public static string TRAY_INFO_NEW_UPDATES_AVAILABLE
		{
			get
			{
				return de.ResourceManager.GetString("TRAY_INFO_NEW_UPDATES_AVAILABLE", de.resourceCulture);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004EF0 File Offset: 0x000030F0
		public static string TRAY_INFO_UPDATE_COMPLETED
		{
			get
			{
				return de.ResourceManager.GetString("TRAY_INFO_UPDATE_COMPLETED", de.resourceCulture);
			}
		}

		// Token: 0x0400004E RID: 78
		private static ResourceManager resourceMan;

		// Token: 0x0400004F RID: 79
		private static CultureInfo resourceCulture;
	}
}
