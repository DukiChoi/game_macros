using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Reflection;

namespace WindowsFormsApp2
{

    //[DllImport("user32.dll")]
    //static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);





    //keybd_event(VK_A, 0, 0, 0);
    //keybd_event(VK_A, 0, 0x02, 0);


    //keybd_event(VK_D, 0, 0, 0);
    //keybd_event(VK_D, 0, 0x02, 0);


    public partial class Form1 : Form
    {


        //private const int VK_RETURN = 0x0D;
        //private const int VK_A = 0x41;
        //private const int VK_D = 0x44;


        public Form1()
        {
            InitializeComponent();
            
        }
        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Application.ExitThread();
                    Application.Exit();
                    break;

            }
        }


        

        //버튼 1,2에서 쓰는 것
        [DllImport("user32.dll")]
        static extern int SetForegroundWindow(IntPtr targetW);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string className, string WindowTitle);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hwnd, int mod);
        //버튼 3에서만 쓰는 것
        [DllImport("user32.dll")]
        public static extern int FindWindowEx(IntPtr hWnd1, int hWnd2, string lpsz1, string lpsz2);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, string lParam);
        [DllImport("user32.dll")]
        public static extern uint PostMessage(int hwnd, int wMsg, int wParam, int lParam);
        [DllImport("ImageSearchDLL.dll")]
        private static extern IntPtr ImageSearch(int x, int y, int right, int bottom, [MarshalAs(UnmanagedType.LPStr)]string imagePath);
        [DllImport("user32")]
        public static extern int SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        
         
        //이건 아래랑 쌍인데 쓰다가 말은 것
        //raw 키입력
        [DllImport("user32.dll")]
        static extern void keybd_event(ushort bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        public const ushort VK_RIGHT = 0x27, VK_LEFT = 0x25, VK_DOWN = 0x28, VK_UP = 0x26;
        //keybd_event(VK_RIGHT, 0, 0, 0);
        //keybd_event(VK_RIGHT, 0, 0x02, 0);
        //정리: 1. 엔터는 sendkeys 2. 방향키는 keybd_event 3. 나머지 키들은 모두 SendKey함수

        
        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr a = FindWindow(null, "MapleStory");
            SetWindowPos(a, 1, 0, 0, 1280, 720, 1);
            SetForegroundWindow(a);
            UnHook();

            string jump_key = textBox4.Text;
            StreamWriter sw = new StreamWriter(new FileStream("ini\\text2.txt", FileMode.Create));


            string ini_read_yubu;
            string ini_read_key;
            string ini_read_key_before;
            int ini_read_time = 0;
            int n = 1;

            FieldInfo mykey;
            ushort Key2;
            do
            {
                Random r = new Random();
                int random_int = r.Next(0, 4);
                ////난수 생성해서 임의의 자리에 e(장비창) 열어주기

                ini_read_yubu = IniRead("시작끝여부", n.ToString(), path);
                ini_read_key = IniRead("키값", n.ToString(), path);
                ini_read_key_before = IniRead("키값", (n - 1).ToString(), path);
                //방향키, 점프키 제외한 키들 넣어주는 if문
                
                if (ini_read_yubu == "시작시각" && ini_read_key != "Right" && ini_read_key != "Left" && ini_read_key != "Up" && ini_read_key != "Down" && ini_read_key != textBox4.Text)
                {
                    mykey = typeof(Form1).GetField("DIK_"+ini_read_key.ToUpper());
                    Key2 = Convert.ToUInt16(mykey.GetValue(null));
                    SendKey(Key2);
                    //sw.WriteLine("SendKey(" + Key2.ToString() + ");");
                }

                //점프키 눌러주는 구문 두개
                else if (ini_read_yubu == "시작시각" && ini_read_key == textBox4.Text && ini_read_key_before == "Right" | ini_read_key_before == "Left" | ini_read_key_before == "Down" | ini_read_key_before == textBox4.Text)
                {
                    mykey = typeof(Form1).GetField("DIK_" + textBox.Text.ToUpper());
                    Key2 = Convert.ToUInt16(mykey.GetValue(null));
                    Keep_Pressing(Key2,2);

                    //랜덤하게 키를 누르게끔 해준다
                    //switch (random_int)
                    //{
                    //    case 1:
                    //        SendKey(DIK_K);
                    //        break;
                    //    case 2:
                    //        SendKey(DIK_E);
                    //        break;
                    //    case 3:
                    //        SendKey(DIK_I);
                    //        break;
                    //}
                    //sw.WriteLine("Keep_Pressing(" + mykey.GetValue(null) + ",2);");
                    //점프키는 두번째 누를 때에 두번 누르는게 중요함

                }
                else if (ini_read_yubu == "시작시각" && ini_read_key == textBox4.Text && ini_read_key_before != "Right" && ini_read_key_before != "Left" && ini_read_key_before != "Down" || ini_read_yubu == "시작시각" && ini_read_key == textBox4.Text && ini_read_key_before == "")
                {
                    mykey = typeof(Form1).GetField("DIK_" + textBox.Text.ToUpper());
                    Key2 = Convert.ToUInt16(mykey.GetValue(null));
                    SendKey(Key2);
                    //sw.WriteLine("SendKey(" + mykey.GetValue(null) + ");");
                }

                //방향키 눌러주는 if문
                else if (ini_read_yubu == "시작시각" && ini_read_key == "Right" | ini_read_key == "Left" | ini_read_key == "Up" | ini_read_key == "Down")
                {
                    mykey = typeof(Form1).GetField("VK_" + ini_read_key.ToUpper());
                    Key2 = Convert.ToUInt16(mykey.GetValue(null));
                    keybd_event(Key2, 0, 0, 0);

                    //랜덤하게 키를 누르게끔 해준다
                    switch (random_int)
                    {
                        case 1:
                            SendKey(DIK_K);
                            break;
                        case 2:
                            SendKey(DIK_E);
                            break;
                        case 3:
                            SendKey(DIK_I);
                            break;
                    }
                    //sw.WriteLine("keybd_event(" + mykey.GetValue(null) + ", 0, 0, 0);");
                }
                else if (ini_read_yubu == "끝시각")
                {
                    mykey = typeof(Form1).GetField("VK_" + ini_read_key.ToUpper());
                    Key2 = Convert.ToUInt16(mykey.GetValue(null));
                    keybd_event(Key2, 0, 0x02, 0);

                    //랜덤하게 키를 누르게끔 해준다
                    switch (random_int)
                    {
                        case 1:
                            SendKey(DIK_K);
                            break;
                        case 2:
                            SendKey(DIK_E);
                            break;
                        case 3:
                            SendKey(DIK_I);
                            break;
                    }
                    //sw.WriteLine("keybd_event(" + mykey.GetValue(null) + ", 0, 0x02, 0);");
                }

                //delay 넣어주는 if문

                if (IniRead("시간순서", (n + 1).ToString(), path) != "")
                {
                    ini_read_time = Convert.ToInt32(IniRead("시간순서", (n + 1).ToString(), path)) - Convert.ToInt32(IniRead("시간순서", n.ToString(), path));
                    if (ini_read_time < 0)
                        ini_read_time = ini_read_time + 60000;
                    Random r1 = new Random();
                    ini_read_time = ini_read_time + r1.Next(0, 10);
                    Thread.Sleep(ini_read_time);
                    //sw.WriteLine("Thread.Sleep(" + ini_read_time + ");");
                }

                n++;

            } while (ini_read_yubu != "");
            sw.Close();

        }



        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr a = FindWindow(null, "MapleStory");
            int c = 1;
            SetWindowPos(a, 1, 0, 0, 1280, 720, 1);
            SetForegroundWindow(a);
            SendKeys.SendWait("{Enter}");
            do
            {
                SendKeys.SendWait(textBox1.Text);
                Thread.Sleep(500);
                SendKeys.SendWait("{Enter}");
                Thread.Sleep(1000);
                SendKeys.SendWait(textBox2.Text);
                Thread.Sleep(500);
                SendKeys.SendWait("{Enter}");
                Thread.Sleep(1000);
            } while (c == 1);
        }



        private void button3_Click(object sender, EventArgs e)
        {

            IntPtr hd01 = FindWindow(null, "병근");
            ShowWindow(hd01, 9);
            SetForegroundWindow(hd01);
            //빼자int hd02 = FindWindowEx(hd01, 0, "RichEdit20W", "");

            //SendKeys.SendWait("{PRTSC}");
            //SendKeys.SendWait("^v");
            //Thread.Sleep(100);
            //SendKeys.SendWait("{Enter}");

            //빼자PostMessage(hd02, 0x0100, 0xD, 0x1C001); //엔터키

            Thread.Sleep(100);
            string[] search = UseImageSearch("*30 img\\img.png");

            //UseImageSearch("*오차값 이미지파일.png"); 로 오토핫키에서의 사용법과 동일합니다.

            if (search == null)
                return;                   //서치실패의 경우 return
            
            int[] search_ = new int[search.Length];

            for (int j = 0; j < search.Length; j++)
            {
                search_[j] = Convert.ToInt32(search[j]);
            }

            MessageBox.Show(search_[1] + "," + search_[2]);
            int w_screen = search_[1] - 12;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            UnHook();
            textBox1.Clear();
            //위처럼 텍스트박스랑 이전 루틴 정리부터 해주고.
            StreamWriter sw = new StreamWriter(new FileStream("ini\\config.ini", FileMode.Create));
            sw.Close();
            //이렇게 닫아줘야 iniwrite 가능하다
            keyHook = IntPtr.Zero;
            textBox = textBox1;
            SetHook();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UnHook();
            
            string jump_key = textBox4.Text;
            StreamWriter sw = new StreamWriter(new FileStream("ini\\text.txt", FileMode.Create));

            string ini_read_yubu;
            string ini_read_key;
            string ini_read_key_before;
            int ini_read_time=0;
            int n = 1;
            do
            {
                ini_read_yubu = IniRead("시작끝여부", n.ToString(), path);
                ini_read_key = IniRead("키값", n.ToString(), path);
                ini_read_key_before = IniRead("키값", (n - 1).ToString(), path);
                ////Send키들 넣어주는 if문

                if (ini_read_yubu == "시작시각" && ini_read_key != "Right" && ini_read_key != "Left" && ini_read_key != "Up" && ini_read_key != "Down" && ini_read_key != textBox4.Text)
                {
                    sw.WriteLine("SendKey(DIK_" + ini_read_key.ToUpper() + ");");
                }
                else if (ini_read_yubu == "시작시각" && ini_read_key == textBox4.Text && ini_read_key_before=="Right"|ini_read_key_before=="Left"| ini_read_key_before == "Down" | ini_read_key_before== textBox4.Text)
                {
                    sw.WriteLine("Keep_Pressing(DIK_" + textBox.Text.ToUpper() + ",2);");
                    //점프키는 두번째 누를 때에 두번 누르는게 중요함
                }
                else if (ini_read_yubu == "시작시각" && ini_read_key == textBox4.Text && ini_read_key_before != "Right" && ini_read_key_before != "Left" && ini_read_key_before != "Down" || ini_read_yubu == "시작시각" && ini_read_key == textBox4.Text && ini_read_key_before == "")
                {
                    sw.WriteLine("SendKey(DIK_" + textBox.Text.ToUpper() + ");");
                }



                else if (ini_read_yubu == "시작시각" && ini_read_key == "Right" | ini_read_key == "Left" | ini_read_key == "Up" | ini_read_key == "Down")
                {
                    sw.WriteLine("keybd_event(VK_" + ini_read_key.ToUpper() + ", 0, 0, 0);");
                }
                else if (ini_read_yubu == "끝시각")
                {
                    sw.WriteLine("keybd_event(VK_" + ini_read_key.ToUpper() + ", 0, 0x02, 0);");
                }

                //delay 넣어주는 if문

                if (IniRead("시간순서", (n + 1).ToString(), path) != "")
                {
                    ini_read_time = Convert.ToInt32(IniRead("시간순서", (n + 1).ToString(), path)) - Convert.ToInt32(IniRead("시간순서", n.ToString(), path));
                    if (ini_read_time < 0)
                        ini_read_time = ini_read_time + 60000;
                    //Random r2 = new Random();
                    //ini_read_time= ini_read_time + r2.Next(0, 10);
                    sw.WriteLine("Thread.Sleep(" + ini_read_time + ");");
                }

                n++;
                
            } while(ini_read_yubu != "");
            sw.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            IntPtr a = FindWindow(null, "MapleStory");
            SetWindowPos(a, 1, 0, 0, 1280, 720, 1);
            SetForegroundWindow(a);

            





        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            UnHook();
            keyHook = IntPtr.Zero;
            textBox = textBox4;
            my_SetHook();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            UnHook();
        }

        //내가만든 sendkey 누르기 함수
        public static void Keep_Pressing(ushort button, int count)
        {
            int my_count=0;
            DateTime start, end;
            start = DateTime.Now;
            end = DateTime.Now;
            //float second_spent;
            do
            {

                end = DateTime.Now;
                TimeSpan duration = end - start;
                SendKey(button);
                //second_spent = duration.Seconds * 1000 + duration.Milliseconds;
                my_count++;
                Random r = new Random();
                Thread.Sleep(r.Next(30,50));
            } while (my_count < count);
            return;
        }



        //이미지서치 함수
        public static String[] UseImageSearch(string imgPath)
        {
            int right = Screen.PrimaryScreen.WorkingArea.Right;
            int bottom = Screen.PrimaryScreen.WorkingArea.Bottom;

            IntPtr result = ImageSearch(0, 0, right, bottom, imgPath);
            String res = Marshal.PtrToStringAnsi(result);
            //이미지 서치 결과값  0번 =  결과 성공1 실패0 1,2번 = x,y 3,4번 = 이미지의 세로가로길이

            //res = 한자씩 나눠져있음


            if (res[0] == '0') //res를 이용하여 이미지여부 확인
            {
                MessageBox.Show("Not found");
                return null;
            }



            String[] data = res.Split('|'); // |로 결과 값을 조각
            int x; int y;
            int.TryParse(data[1], out x); //x좌표
            int.TryParse(data[2], out y); //y좌표

            //for(int i=0; i<5; i++)
            //  MessageBox.Show(i + "번째 " + data[i]);

            return data;
        }
        //이미지서치 함수 끝



        //key입력 함수
        public const ushort DIK_ESCAPE = 0x1, DIK_1 = 0x2, DIK_2 = 0x3, DIK_3 = 0x4, DIK_4 = 0x5, DIK_5 = 0x6, DIK_6 = 0x7, DIK_7 = 0x8, DIK_8 = 0x9,
            DIK_9 = 0xA, DIK_0 = 0xB, DIK_OEMMINUS = 0xC, DIK_OEMPLUS = 0xD, DIK_BACK = 0xE, DIK_TAB = 0xF, DIK_Q = 0x10, DIK_W = 0x11, DIK_E = 0x12,
            DIK_R = 0x13, DIK_T = 0x14, DIK_Y = 0x15, DIK_U = 0x16, DIK_I = 0x17, DIK_O = 0x18, DIK_P = 0x19, DIK_LBRACKET = 0x1A, DIK_RBRACKET = 0x1B,
            DIK_ENTER = 0x1C, DIK_LCONTROLKEY = 0x1D, DIK_A = 0x1E, DIK_S = 0x1F, DIK_D = 0x20, DIK_F = 0x21, DIK_G = 0x22, DIK_H = 0x23, DIK_J = 0x24,
            DIK_K = 0x25, DIK_L = 0x26, DIK_SEMICOLON = 0x27, DIK_APOSTROPHE = 0x28, DIK_GRAVE = 0x29, DIK_LSHIFTKEY = 0x2A, DIK_BACKSLASH = 0x2B, DIK_Z = 0x2C,
            DIK_X = 0x2D, DIK_C = 0x2E, DIK_V = 0x2F, DIK_B = 0x30, DIK_N = 0x31, DIK_M = 0x32, DIK_OEMCOMMA = 0x33, DIK_OEMPERIOD = 0x34, DIK_SLASH = 0x35,
            DIK_RSHIFTKEY = 0x36, DIK_MULTIPLY = 0x37, DIK_LMENU = 0x38, DIK_SPACE = 0x39, DIK_CAPITAL = 0x3A, DIK_F1 = 0x3B, DIK_F2 = 0x3C, DIK_F3 = 0x3D,
            DIK_F4 = 0x3E, DIK_F5 = 0x3F, DIK_F6 = 0x40, DIK_F7 = 0x41, DIK_F8 = 0x42, DIK_F9 = 0x43, DIK_F10 = 0x44, DIK_NUMLOCK = 0x45, DIK_SCROLL = 0x46,
            DIK_NUMPAD7 = 0x47, DIK_NUMPAD8 = 0x48, DIK_NUMPAD9 = 0x49, DIK_SUBTRACT = 0x4A, DIK_NUMPAD4 = 0x4B, DIK_NUMPAD5 = 0x4C, DIK_NUMPAD6 = 0x4D,
            DIK_ADD = 0x4E, DIK_NUMPAD1 = 0x4F, DIK_NUMPAD2 = 0x50, DIK_NUMPAD3 = 0x51, DIK_NUMPAD0 = 0x52, DIK_DECIMAL = 0x53, DIK_OEM_102 = 0x56, DIK_F11 = 0x57,
            DIK_F12 = 0x58, DIK_F13 = 0x64, DIK_F14 = 0x65, DIK_F15 = 0x66, DIK_KANA = 0x70, DIK_ABNT_C1 = 0x73, DIK_CONVERT = 0x79, DIK_NOCONVERT = 0x7B,
            DIK_YEN = 0x7D, DIK_ABNT_C2 = 0x7E, DIK_NUMPADEQUALS = 0x80, DIK_PREVTRACK = 0x90, DIK_AT = 0x91, DIK_COLON = 0x92, DIK_UNDERLINE = 0x93,
            DIK_KANJI = 0x94, DIK_STOP = 0x95, DIK_AX = 0x96, DIK_UNLABELED = 0x97, DIK_NEXTTRACK = 0x99, DIK_NUMPADENTER = 0x9C, DIK_RCONTROLKEY = 0x9D,
            DIK_MUTE = 0xA0, DIK_CALCULATOR = 0xA1, DIK_PLAYPAUSE = 0xA2, DIK_MEDIASTOP = 0xA4, DIK_VOLUMEDOWN = 0xAE, DIK_VOLUMEUP = 0xB0, DIK_WEBHOME = 0xB2,
            DIK_NUMPADCOMMA = 0xB3, DIK_DIVIDE = 0xB5, DIK_SYSRQ = 0xB7, DIK_RMENU = 0xB8, DIK_PAUSE = 0xC5, DIK_HOME = 0xC7, DIK_UP = 0xC8, DIK_PGUP = 0xC9,
            DIK_LEFT = 0xCB, DIK_RIGHT = 0xCD, DIK_END = 0xCF, DIK_DOWN = 0xD0, DIK_PGDN = 0xD1, DIK_INS = 0xD2, DIK_DEL = 0xD3, DIK_LWIN = 0xD8, DIK_RWIN = 0xDC,
            DIK_APPS = 0xDD, DIK_POWER = 0xDE, DIK_SLEEP = 0xDF, DIK_WAKE = 0xE3, DIK_WEBSEARCH = 0xE5, DIK_WEBFAVORITES = 0xE6, DIK_WEBREFRESH = 0xE7,
            DIK_WEBSTOP = 0xE8, DIK_WEBFORWARD = 0xE9, DIK_WEBBACK = 0xEA, DIK_MYCOMPUTER = 0xEB, DIK_MAIL = 0xEC, DIK_MEDIASELECT = 0xED, DIK_OEM1 = 0x27, 
            DIK_OEM7 = 0x28, OEMQUESTION= 0x2B;

        [Flags]
        private enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags]
        private enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008,
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        //누르는 것
        public static void SendKey(ushort key)
        {
            Input[] inputs =
            {
                new Input
                {
                    type = (int) InputType.Keyboard,
                    u = new InputUnion
                    {
                        
                        ki = new KeyboardInput
                        {
                            wVk = 0,
                            wScan = key,
                            dwFlags = (uint) (KeyEventF.KeyDown | KeyEventF.Scancode),
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            //떼는 것
            Input[] outputs =
            {
                new Input
                {
                    type = (int) InputType.Keyboard,
                    u = new InputUnion
                    {
                        ki = new KeyboardInput
                        {
                            wVk = 0,
                            wScan = key,
                            dwFlags = (uint) (KeyEventF.KeyUp | KeyEventF.Scancode),
                            dwExtraInfo = GetMessageExtraInfo()

                        }
                    }
                }
            };

            SendInput((uint)outputs.Length, outputs, Marshal.SizeOf(typeof(Input)));
        }


        private struct Input
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public readonly MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public readonly HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public readonly int dx;
            public readonly int dy;
            public readonly uint mouseData;
            public readonly uint dwFlags;
            public readonly uint time;
            public readonly IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public readonly uint uMsg;
            public readonly ushort wParamL;
            public readonly ushort wParamH;
        }

        //key 입력 함수 끝  




        //후킹 함수 시작
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;

        private delegate IntPtr LowLevelKeyProc(int nCode, IntPtr wParam, IntPtr lParam);

        //이 위로는 DLL import 등입니다.

        private static LowLevelKeyProc keyboardProc = KeyboardHookProc;
        private static LowLevelKeyProc my_KeyboardProc = my_KeyboardHookProc;
        private static IntPtr keyHook = IntPtr.Zero;
        private static IntPtr keyHookInstance = IntPtr.Zero;
        //후킹을 설정해줍니다.
        public static void SetHook()
        {
            if (keyHook == IntPtr.Zero)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    keyHook = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardProc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }
        //후킹을 해제해줍니다.
        public static void UnHook()
        {
            UnhookWindowsHookEx(keyHook);
        }

        //내 후킹 끼워넣음
        public static void my_SetHook()
        {
            if (keyHook == IntPtr.Zero)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    keyHook = SetWindowsHookEx(WH_KEYBOARD_LL, my_KeyboardProc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private static IntPtr my_KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && (int)wParam == 256)
            {
                KeysConverter converter = new KeysConverter();
                textBox.Clear();
                textBox.AppendText(converter.ConvertToInvariantString(Marshal.ReadInt32(lParam)));
            }
            return CallNextHookEx(keyHook, code, wParam, lParam);
        }

        //텍스트박스에 글을 띄워야 하는데, 후킹 메소드가 모두 static이므로 텍스트박스를 static으로 빼 줍니다.
        private static TextBox textBox;


        static int my_lParam;
        static int start, end;
        static int last = 1;
        static int last_no_banghyang = 1;
        //키보드의 후킹을 처리하는 부분. 키보드를 누르거나 떼면 이 함수가 실행됩니다.
        private static IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            //이전 if문들은 시작시각, 끝시각을 모든 버튼에 대해서 적어주는 특징이 있었음
            //또 last 초기값은 0으로 시작했어야 했다.
            //if (code >= 0 && (int)wParam == 256 && Marshal.ReadInt32(lParam) != my_lParam && last == 0)
            //{
            //    start = (DateTime.Now.Second) * 1000 + DateTime.Now.Millisecond;
            //    KeyCheck(Marshal.ReadInt32(lParam), 0, lParam);
            //}
            //else if (code >= 0 && (int)wParam == 256 && last == 1)
            //{
            //    start = (DateTime.Now.Second) * 1000 + DateTime.Now.Millisecond;
            //    KeyCheck(Marshal.ReadInt32(lParam), 0, lParam);
            //}

            //else if (code >= 0 && (int)wParam == 257)
            //{
            //    end = (DateTime.Now.Second) * 1000 + DateTime.Now.Millisecond;
            //    KeyCheck(Marshal.ReadInt32(lParam), 1, lParam);
            //}
            //Code가 0보다 클 때에만 처리해야합니다. 아닐 경우엔 메세지를 흘려보냅니다.(이유는 잘 모릅니다.)
            //wParam==256부분은, 키보드를 누르는 이벤트와 떼는 이벤트 중 누르는 이벤트만을 통과시킵니다.
            //만약 ==257로 바꿀 경우, 키보드를 땔 떼 코드가 실행됩니다.

            

            //여기는 이제 방향키'만' 끝시각이 나오게끔 만들어준 if문
            if (code >= 0 && (int)wParam == 256 && Marshal.ReadInt32(lParam) != 37 && Marshal.ReadInt32(lParam) != 38 && Marshal.ReadInt32(lParam) != 39 && Marshal.ReadInt32(lParam) != 40)
            {
                start = (DateTime.Now.Second) * 1000 + DateTime.Now.Millisecond;
                KeyCheck(Marshal.ReadInt32(lParam), 0, lParam);
                last_no_banghyang = 1;
            }
            else if (code >= 0 && (int)wParam == 256 && last == 0 && last_no_banghyang == 1 && Marshal.ReadInt32(lParam) == 37 | Marshal.ReadInt32(lParam) == 38 | Marshal.ReadInt32(lParam) == 39 | Marshal.ReadInt32(lParam) == 40)
            {
                start = (DateTime.Now.Second) * 1000 + DateTime.Now.Millisecond;
                KeyCheck(Marshal.ReadInt32(lParam), 0, lParam);
                last_no_banghyang = 0;
            }
            else if (code >= 0 && (int)wParam == 256 && last==1 && Marshal.ReadInt32(lParam) == 37 | Marshal.ReadInt32(lParam) == 38 | Marshal.ReadInt32(lParam) == 39 | Marshal.ReadInt32(lParam) == 40)
            {
                start = (DateTime.Now.Second) * 1000 + DateTime.Now.Millisecond;
                KeyCheck(Marshal.ReadInt32(lParam), 0, lParam);
                last_no_banghyang = 0;
                //last_no_banghyang은 버튼을 떼고 나서 다음 번에 키를 누를 때 전 키가 방향키가 아닐 때만 1이 되어야 하는 변수..
            }
            else if (code >= 0 && (int)wParam == 257 && Marshal.ReadInt32(lParam) == 37|Marshal.ReadInt32(lParam) == 38| Marshal.ReadInt32(lParam) == 39 | Marshal.ReadInt32(lParam) == 40)
            {
                end = (DateTime.Now.Second) * 1000 + DateTime.Now.Millisecond;
                KeyCheck(Marshal.ReadInt32(lParam), 1, lParam);
            }
            
            return CallNextHookEx(keyHook, code, wParam, lParam);
        }


        static int ini_count = 1;
        public static void KeyCheck(int keyCode, int index, IntPtr lParam)
        {
            //KeyCode를 해석해서 텍스트박스에 적어줍니다.
            
            //int duration = end - start;
            
            //현재 키값 저장
            my_lParam = Marshal.ReadInt32(lParam);

            //int second_spent = duration.Seconds* 1000 + duration.Milliseconds;
            //MessageBox.Show(duration.Seconds + "." + duration.Milliseconds);
            KeysConverter converter = new KeysConverter();
            if (index == 0)
            {
                textBox.Clear();
                textBox.AppendText("시작시각|"+converter.ConvertToInvariantString(keyCode) + "|" + start);
                IniWrite("시작끝여부", ini_count.ToString(), "시작시각", path);
                IniWrite("키값", ini_count.ToString(), converter.ConvertToInvariantString(keyCode), path);
                IniWrite("시간순서", ini_count.ToString(), start.ToString() ,path);
                
                last = 0;
            }
            if (index == 1)
            {
                textBox.Clear();
                textBox.AppendText("끝시각|"+converter.ConvertToInvariantString(keyCode) + "|" + end);
                IniWrite("시작끝여부", ini_count.ToString(), "끝시각", path);
                IniWrite("키값", ini_count.ToString(), converter.ConvertToInvariantString(keyCode), path);
                IniWrite("시간순서", ini_count.ToString(), end.ToString(), path);
                last = 1;
            }
            ini_count++;
        }

        

        public static void Hook_Save()
        {
            return;
        }

        public static void Start_Farming()
        {
            return;
        }
            


        private void Form1_Closing(object sender, EventArgs e)
        {
        //폼이 닫힐 시에는 후킹을 해제해줍니다.
            UnHook();
        }


        //후킹 함수 끝



        // ini 함수 시작
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        /*  .ini파일 읽는 함수
        * string section : 가져올 값의 키가 속해있는 섹션이름
        * string key : 가져올 값의 키이름
        * string def : 키의 값이 없을경우 기본값(default)
        * StringBuilder retVal : 가져올 값
        * int size : 가져올 값의 길이
        * string filePath : 읽어올 ini 파일경로
        * return value : 읽어들여온 데이터 길이
        */
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // ini파일경로

        static string path = "ini\\config.ini";

        // ini쓰기

        private static void IniWrite(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }
        // ini읽기
        private static string IniRead(string section, string key, string path)
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", sb, sb.Capacity, path);
            return sb.ToString();
        }
        //ini함수 끝
    }
}

