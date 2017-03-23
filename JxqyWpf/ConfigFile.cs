using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace JxqyWpf
{
    public class ConfigFile
    {
        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string defValue, StringBuilder retValue, int size, string filePath);


        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);



        public ConfigFile(string inFileName)
        {
            this.fileName = inFileName;
        }

        public void WriteValue(string section, string key, string value)
        {
            if(File.Exists(fileName))
            {
                WritePrivateProfileString(section, key, value, fileName);
            }
            else
            {
                ///Do something.
            }
        }

        public string ReadValue(string section, string key)
        {
            if(File.Exists(fileName))
            {
                StringBuilder ret = new StringBuilder(255);
                long i = GetPrivateProfileString(section, key, "Default", ret, 255, fileName);
                return ret.ToString();
            }
            else
            {
                ///Wrong.
                return "NotExist";
            }   
        }

        public bool SetFileName(string inFIleName)
        {
            this.fileName = inFIleName;
            return true;
        }

        public string GetFileName()
        {
            return fileName;
        }

        protected string fileName;
    }
}
