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
        public int HotKey { get; set; }

        public int MacroKey { get; set; }
    }

    public class MyApp
    {
        private Input inputObj = new Input();
        private int tickTime = 50;
        private bool bShouldTick = false;
        private Object thisLock = new Object();
        private Thread fun;

        AppConfig config = new AppConfig();

        [DllImport("Kernel32")]
        public static extern void AllocConsole();
        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        public MyApp()
        {
            inputObj.KeyboardFilterMode = KeyboardFilterMode.All;
            inputObj.Load();

            fun = new Thread(new ThreadStart(WaitForCmd));
            fun.Start();

#if DEBUG

            AllocConsole();
#endif
            Tick();
        }
        
        public async void Tick()
        {
            while(true)
            {
                if(bShouldTick)
                {
                    inputObj.SendKey(Keys.F8, KeyState.Down);
                    await Task.Delay(5);
                    inputObj.SendKey(Keys.F8, KeyState.Up);
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

        public int SetKey(int inKey)
        {
            //key = inKey;
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
                if (strk.Key.Code == Keys.Tilde)
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
