using System;
using System.IO;
using System.Linq;

namespace GameUpdater.Utilities
{
	// Token: 0x0200000D RID: 13
	internal class FileSystem
	{
		// Token: 0x0600007D RID: 125 RVA: 0x0000457C File Offset: 0x0000277C
		public static bool DeleteFile(string filename)
		{
			bool result;
			try
			{
				File.Delete(filename);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000045B4 File Offset: 0x000027B4
		public static void DeleteFiles(string path, string pattern)
		{
			foreach (FileInfo fileInfo in (from p in new DirectoryInfo(path).GetFiles("*.tmp")
			where p.Extension == ".tmp"
			select p).ToArray<FileInfo>())
			{
				try
				{
					fileInfo.Attributes = FileAttributes.Normal;
					File.Delete(fileInfo.FullName);
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004644 File Offset: 0x00002844
		public static void Rename(string oldPath, string newPath)
		{
			try
			{
				bool flag = File.Exists(newPath);
				if (flag)
				{
					File.Delete(newPath);
				}
				File.Move(oldPath, newPath);
			}
			catch (Exception)
			{
				Log.Write(string.Format("    Error renaming tmp file {0} to {1}", oldPath, newPath), true);
			}
		}
	}
}
