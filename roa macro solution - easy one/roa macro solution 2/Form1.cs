using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Reflection;
namespace roa_macro_solution_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Application.ExitThread();
                    Application.Exit();
                    break;

            }
        }

        [DllImport("user32.dll")]
        static extern int SetForegroundWindow(IntPtr targetW);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string className, string WindowTitle);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hwnd, int mod);
        [DllImport("user32")]
        public static extern int SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);


        string a = Cursor.Position.X.ToString();
        string b = Cursor.Position.Y.ToString();
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr a = FindWindow(null, "LOST ARK (64-bit) v.1.0.2.2");
            SetForegroundWindow(a);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_RIGHTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);

        }
    }
}
