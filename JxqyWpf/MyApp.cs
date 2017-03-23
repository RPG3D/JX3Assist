using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interceptor;
using System.Windows;
using System.Threading;
using System.Runtime.InteropServices;
using System.Media;
using System.IO;

namespace JxqyWpf
{
    public class AppConfig
    {
        public AppConfig(string inFileName)
        {
            configFileName = inFileName;
        }

        public string ReadPowerKey()
        {
            ConfigFile cfg = new ConfigFile(configFileName);
            return cfg.ReadValue("Key", "PowerKey");
        }

        public void SetPowerKey(string key)
        {
            ConfigFile cfg = new ConfigFile(configFileName);
            cfg.WriteValue("Key", "PowerKey", key);
        }

        public string ReadMacroKey()
        {
            ConfigFile cfg = new ConfigFile(configFileName);
            return cfg.ReadValue("Key", "MacroKey");
        }

        public void SetMacroKey(string key)
        {
            ConfigFile cfg = new ConfigFile(configFileName);
            cfg.WriteValue("Key", "MacroKey", key);
        }

        public string ReadWindowTitle()
        {
            ConfigFile cfg = new ConfigFile(configFileName);
            return cfg.ReadValue("Key", "WindowTitle");
        }

        private string configFileName;
    }

    public class MyApp
    {
        private Input inputObj = new Input();
        private int tickTime = 50;
        private bool bShouldTick = false;
        private Object thisLock = new Object();
        private int powerKey = 41;
        private int macroKey = 66;
        private Thread fun;
        public string windowTitle = "Jx3Assist";

        
        AppConfig config = new AppConfig(Directory.GetCurrentDirectory() + "/AppConfig.ini");

        [DllImport("Kernel32")]
        public static extern void AllocConsole();
        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        public MyApp()
        {
#if DEBUG

            AllocConsole();
#endif
            inputObj.KeyboardFilterMode = KeyboardFilterMode.All;
            inputObj.Load();
            ConfigApp();

            fun = new Thread(new ThreadStart(WaitForCmd));
            fun.Start();

            Tick();
        }

        private void ConfigApp()
        {
            ///读取开始、停止按钮
            string pStr = config.ReadPowerKey();
            int pKey = 0;
            if (int.TryParse(pStr, out pKey))
            {
                SetPowerKey(pKey);
#if DEBUG
                Console.WriteLine("SetPowerKey" + pKey);
#endif
                
            }

            ///读取宏按钮
            string mStr = config.ReadMacroKey();
            int mKey = 0;
            if (int.TryParse(mStr, out mKey))
            {
                SetMacroKey(mKey);
#if DEBUG
                Console.WriteLine("SetMacroKey" + mKey);
#endif
                
            }

            ///读取窗口标题
            windowTitle = config.ReadWindowTitle();
        }

        public async void Tick()
        {
            while(true)
            {
                if(bShouldTick)
                {
                    inputObj.SendKey((Keys)macroKey, KeyState.Down);
                    await Task.Delay(5);
                    inputObj.SendKey((Keys)macroKey, KeyState.Up);
                    await Task.Delay(tickTime - 5);
                } 
                else
                {
                    await Task.Delay(250);
                }
            }
        }

        public bool IsRunning()
        {
            return bShouldTick == true;
        }

        public int SetTickTime(int inTIme)
        {
            tickTime = inTIme;
            return tickTime;
        }

        public int SetPowerKey(int inKey)
        {
            powerKey = inKey;
            return inKey;
        }

        public int SetMacroKey(int inKey)
        {
            macroKey = inKey;
            return inKey;
        }

        public void Start()
        {
            lock(thisLock)
            {
                bShouldTick = true;
            }
            SystemSounds.Hand.Play();
        }

        public void Stop()
        {
            lock (thisLock)
            {
                bShouldTick = false;
            }
            SystemSounds.Asterisk.Play();
        }

        public void WaitForCmd()
        {
            IntPtr ctx = InterceptionDriver.CreateContext();
            int device = 0;
            Stroke strk = new Stroke();
            InterceptionDriver.SetFilter(ctx, InterceptionDriver.IsKeyboard, (int)KeyboardFilterMode.KeyDown | (int)KeyboardFilterMode.KeyUp);

            int i = 0;
            while (InterceptionDriver.Receive(ctx, device = InterceptionDriver.Wait(ctx), ref strk, 1) > 0)
            {
#if DEBUG
                Console.WriteLine((int)strk.Key.Code);
#endif
                if (strk.Key.Code == (Keys)powerKey)
                {
                    ++i;
                    if(i < 2)
                    {
                        continue;
                    }
                    lock (thisLock)
                    {
                        if(IsRunning())
                        {
                            Stop();
                            
#if DEBUG
                            Console.WriteLine("Stop");
#endif
                        }
                        else
                        {
                            Start();
                            
#if DEBUG
                            Console.WriteLine("Start");
#endif
                        }  
                    }
                    i = 0;
                }
                InterceptionDriver.Send(ctx, device, ref strk, 1);
            }
        }

        public void ReadConfig(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);

            file.Seek(0, SeekOrigin.Begin);
            
            
        }
    }
}
