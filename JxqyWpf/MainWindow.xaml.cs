using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;


namespace JxqyWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyApp app = new MyApp();
        public Update upd = new Update();

        NotifyIcon appNotifyIcon = new NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();
            this.Title = app.windowTitle;
            appNotifyIcon.BalloonTipText = "JX3 Assist";
            appNotifyIcon.ShowBalloonTip(2000);
            appNotifyIcon.Text = "JX3 Assist";
            appNotifyIcon.Visible = true;
            appNotifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            appNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((obj, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    Show(obj, e);
                }
                    
            });

            StateChanged += MainWindow_StateChanged;
            
        }

        private void MainWindow_StateChanged(object obj, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                Hide(obj, e);
            }
        }

        private void Show(object obj, EventArgs e)
        {
            Visibility = Visibility.Visible;
            ShowInTaskbar = true;
            Activate();
        }

        private void Hide(object obj, EventArgs e)
        {
            ShowInTaskbar = false;
            Visibility = Visibility.Hidden;
        }
       
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            app.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            app.Stop();
        }

        private void btnSetTick_Click(object sender, RoutedEventArgs e)
        {
            int time = 50;
            if(int.TryParse(txtInTime.Text, out time))
            {
                if(time < 20 || time > 9999)
                {
                    System.Windows.MessageBox.Show("TickTime Should Between 20 and 9999");
                }
                else
                {
                    app.SetTickTime(time);
                    System.Windows.MessageBox.Show("Successful");
                } 
            }
            else
            {
                System.Windows.MessageBox.Show("TickTime Should Between 20 and 9999");
            }
            txtInTime.Text = time.ToString();
        }

        protected override void OnClosed(EventArgs e)
        {
            System.Environment.Exit(0);
            base.OnClosed(e);
        }

        private void btnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            upd.CheckUpdate();
        }
    }
}
