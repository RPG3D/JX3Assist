using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Windows;


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

        protected UpdateWindow updWin = new UpdateWindow();

        public Update()
        {
            ConfigFile updateCfg = new ConfigFile(workPath + "/" + updateConfigName);
            serverConfigFileUrl = updateCfg.ReadValue("ServerInfo", "ConfigFileUrl");
        }

        public void CheckUpdate()
        {
            if(File.Exists(tmpFileName))
            {
                return;
            }

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
            if (localVersion == serverVersion)
            {
                MessageBox.Show("Your App is the latest version");
                return;
            }

            WebClient dlClient = new WebClient();

            string patchUrl = serverCfg.ReadValue("PatchName", "Url");
            dlClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadEnd);
            dlClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadChanged);
            dlClient.DownloadFileAsync(new Uri(patchUrl), patchFileName);

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
            MessageBox.Show("Your App is the latest version");
        }

        public bool UpdateSelf(string newFileName)
        {
            string selfName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            File.Move(selfName, selfName + "_old");
            File.Move(patchFileName, selfName);
            File.Delete(selfName + "_old");
            return true;
        }
    }
}
