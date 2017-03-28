using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.ComponentModel;

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
                return;
            }
        }
    }
}
