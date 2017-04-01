using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Diagnostics;

namespace JxqyWpf
{
    public class Update
    {
        protected string updateConfigName = "Update.ini";
        protected string workPath = System.Environment.CurrentDirectory;
        protected string localVersion;
        protected string serverVersion;
        protected string tmpFileName = "tmp";
        protected string serverConfigFileUrl;
        protected string patchFileName = "Patch.tmp";
        protected bool isChecking = false;

        protected UpdateWindow updWin = new UpdateWindow();

        public Update()
        {
            ConfigFile updateCfg = new ConfigFile(workPath + "/" + updateConfigName);
            serverConfigFileUrl = updateCfg.ReadValue("ServerInfo", "ConfigFileUrl");

            if (File.Exists("old_"))
            {
                File.Delete("old_");
            }
        }

        public void CheckUpdate()
        {
            if(isChecking == true)
            {
                return;
            }

            isChecking = true;


            ConfigFile updateCfg = new ConfigFile(workPath + "/" + updateConfigName);
            localVersion = updateCfg.ReadValue("AppInfo", "AppVersion");
            WebClient dlClient = new WebClient();

            dlClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnCheckUpdateEnd);
            dlClient.DownloadFileAsync(new Uri(serverConfigFileUrl), tmpFileName);

            return;
        }

        public void OnCheckUpdateEnd(object sender, AsyncCompletedEventArgs e)
        {
            ConfigFile serverCfg = new ConfigFile(workPath + "/" + tmpFileName);
            serverVersion = serverCfg.ReadValue("AppInfo", "AppVersion");
            if (localVersion == serverVersion)
            {
                isChecking = false;
                if (File.Exists(tmpFileName))
                {
                    File.Delete(tmpFileName);
                }
                MessageBox.Show("Your App is the latest version");
                return;
            }

            WebClient dlClient = new WebClient();

            string patchUrl = serverCfg.ReadValue("PatchName", "Url");
            dlClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadEnd);
            dlClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadChanged);

            if (File.Exists(patchFileName))
            {
                File.Delete(patchFileName);
            }
            dlClient.DownloadFileAsync(new Uri(patchUrl), patchFileName);

            if (File.Exists(tmpFileName))
            {
                File.Delete(tmpFileName);
            }

            updWin.Show();
        }

        public void OnDownloadChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            updWin.ChangePrograss(e.ProgressPercentage);
        }

        public void OnDownloadEnd(object sender, AsyncCompletedEventArgs e)
        {
            updWin.Close();
            UpdateSelf(patchFileName);
            
        }

        public void UpdateSelf(string newFileName)
        {
            string selfName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            if (File.Exists("old_"))
            {
                File.Delete("old_");
            }

            File.Move(selfName, "old_");
            File.Copy(patchFileName, selfName);

            ConfigFile updateCfg = new ConfigFile(workPath + "/" + updateConfigName);
            updateCfg.WriteValue("AppInfo", "AppVersion", serverVersion);
            MessageBox.Show("Updating sucessful.App is restarting.");

            if (File.Exists(patchFileName))
            {
                File.Delete(patchFileName);
            }

            Process.Start(selfName);
            System.Environment.Exit(0);
        }
    }
}
