using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HideApp
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string 类名, string 程序标题);
        [DllImport("user32.dll", EntryPoint = "ShowWindowAsync")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool CloseWindow(IntPtr hwn);
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessage(IntPtr HWnd, uint Msg, int WParam, int LParam);
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const uint WM_SYSCOMMAND2 = 0x0112;
        public const uint SC_MAXIMIZE2 = 0xF030;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        /// <summary>
        /// 设置目标窗体大小，位置
        /// </summary>
        /// <param name="hWnd">目标句柄</param>
        /// <param name="x">目标窗体新位置X轴坐标</param>
        /// <param name="y">目标窗体新位置Y轴坐标</param>
        /// <param name="nWidth">目标窗体新宽度</param>
        /// <param name="nHeight">目标窗体新高度</param>
        /// <param name="BRePaint">是否刷新窗体</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }

        private const int SW_HIDE = 0;                                                          //常量，隐藏
        private const int SW_SHOWNORMAL = 1;                                                    //常量，显示，标准状态
        private const int SW_SHOWMINIMIZED = 2;                                                 //常量，显示，最小化
        private const int SW_SHOWMAXIMIZED = 3;                                                 //常量，显示，最大化
        private const int SW_SHOWNOACTIVATE = 4;                                                //常量，显示，不激活
        private const int SW_RESTORE = 9;                                                       //常量，显示，回复原状
        private const int SW_SHOWDEFAULT = 10;

        //句柄
        private int hwnd = 0;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "安图恩：周4 5 6 晚6.50集合 -9点之前 肯定结束10波结束，有时候可能多开到11 12 13波" +
"卢克修改为周日与周一，周日下午有周一下午可能也有晚上一定有团，全天大概20个团。";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ToChange(IntPtr p, bool isboolean)
        {
            try
            {
                ITaskbarList bar = (ITaskbarList)new CoTaskbarList();
                bar.HrInit();
                if (!isboolean)
                {
                    //result = ShowWindowAsync(p, SW_SHOWNORMAL);
                    bar.DeleteTab(p);
                }
                else
                {
                    //result = ShowWindowAsync(p, SW_HIDE);
                    bar.AddTab(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Event

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                hwnd = FindWindow(null, "不常用群聊"); //句柄是int32型的整数
                //Hiden
                if (hwnd != 0)
                    ToChange(new IntPtr(hwnd), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                hwnd = FindWindow(null, "不常用群聊"); //句柄是int32型的整数
                //Show
                if (hwnd != 0)
                    ToChange(new IntPtr(hwnd), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        

        //private void Form1_SizeChanged(object sender, EventArgs e)
        //{
        //    if (this.WindowState == FormWindowState.Minimized) //判断是否最小化
        //    {
        //        this.ShowInTaskbar = false; //不显示在系统任务栏
        //        notifyIcon1.Visible = true; //托盘图标可见
        //    }
        //}

        //private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    this.ShowInTaskbar = true; //显示在系统任务栏
        //    this.WindowState = FormWindowState.Normal; //还原窗体
        //    notifyIcon1.Visible = false; //托盘图标隐藏
        //}


        void exitMenu_Click(object sender, EventArgs e)
        {
            this.Dispose(true);

            Application.ExitThread();
        }

        #endregion



        private void button3_Click(object sender, EventArgs e)
        {
            MiniMizeAppication("TIM");
        }

        /// <summary>  
        /// 最小化其他应用程序  
        /// </summary>  
        /// <param name="processName"></param>  
        private void MiniMizeAppication(string processName)
        {
            Process[] processs = Process.GetProcessesByName(processName);
            if (processs != null)
            {
                foreach (Process p in processs)
                {
                    IntPtr handle = new IntPtr(FindWindow(null, p.MainWindowTitle));
                    PostMessage(handle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
                    MoveWindow(handle, 0, 0, 200, 150, false);
                    RECT rect = new RECT();
                    GetWindowRect(handle, ref rect);
                    textBox2.Text = (rect.Right - rect.Left).ToString();                        //窗口的宽度
                    textBox3.Text = (rect.Bottom - rect.Top).ToString();                   //窗口的高度
                    textBox4.Text = rect.Left.ToString();
                    textBox5.Text = rect.Top.ToString();
                }
            }
        }

        private void GetWindowSize() {

        }

        /// <summary>  
        /// 最大化其他应用程序  
        /// </summary>  
        private void button4_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("TIM");
            if (processes.Length > 0)
            {
                IntPtr handle = processes[0].MainWindowHandle;
                SendMessage(handle, WM_SYSCOMMAND2, new IntPtr(SC_MAXIMIZE2), IntPtr.Zero); // 最大化  
                SwitchToThisWindow(handle, true);   // 激活  
            }
        }

        //  只有Form_Closing事件中 e.Cancel可以用。
        //  你的是Form_Closed事件。 Form_Closed事件时窗口已关了 ，Cancel没用了；
        //  Form_Closing是窗口即将关闭时询问你是不是真的关闭才有Cancel事件

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true; // 取消关闭窗体 

                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                //this.m_cartoonForm.CartoonClose();
                this.Hide();
                return;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
