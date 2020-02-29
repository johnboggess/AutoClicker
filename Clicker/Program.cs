using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
namespace Clicker
{
    class Program
    {
        static Controller ctrl;
        static Dictionary<string, Variable> Variables = new Dictionary<string, Variable>();
        static Dictionary<string, int> JumpPoints = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            ctrl = new Controller();
            string input;
            while (true)
            {
                input = Console.ReadLine();
                ParseCommand(input);
            }
        }

        static bool ParseCommand(string cmd)
        {
            string[] cmds = cmd.Split(';');
            
            if (cmd[0] == '@' || cmd[0] == '#')
            {
                return true;
            }

            for (int x=0;x<cmds.Length;x+=1)
            {
                if (cmds[x].Contains("%") && cmds[x].Contains ("$"))
                {
                    if (Variables.ContainsKey(cmds[x].Remove(cmds[x].Length-1, 1)) == false)
                    {
                        Console.WriteLine("Unable to find variable " + cmds[x]);
                        return false;
                    }
                    cmds[x] = Variables[cmds[x].Remove(cmds[x].Length-1, 1)].value;
                }
            }
            
            if (cmds[0].ToLower() == "getpos")
            {
                #region GetPos
                if (cmds.Length == 1)
                {
                    getPos("");
                }
                else
                {
                    getPos(cmds[1]);
                }
                return true;
                #endregion
            }

            if (cmds[0].ToLower() == "getcolor")
            {
                #region GetPos
                Console.WriteLine(ctrl.color(ctrl.getMousePosition()[0], ctrl.getMousePosition()[1]));
                return true;
                #endregion
            }

            if (cmds[0].ToLower() == "run")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("Enter Script name");
                    return false;
                }
                else
                {
                    Console.WriteLine("Press Shift to stop program");
                    Run(cmds[1]);
                    Console.WriteLine("Done");
                    return true;
                }
            }

            if (cmds[0].ToLower() == "window")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No window given");
                    return false;
                }

                return ctrl.getWindow(cmds[1]);
            }

            if (cmds[0].ToLower() == "beep")
            {
                ctrl.beep = true;
                return true;
            }

            if (cmds[0].ToLower() == "move")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No position given to move mouse to.");
                    return false;
                }
                else
                {
                    ctrl.Move(new Point("("+cmds[1]+","+cmds[2]+")"));
                    return true;
                }
            }

            if (cmds[0].ToLower() == "moverelative")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No position given to move mouse to.");
                    return false;
                }
                else
                {
                    ctrl.MoveRelative(new Point("(" + cmds[1] + "," + cmds[2] + ")"));
                    return true;
                }
            }

            if (cmds[0].ToLower() == "movewindow")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No position given to move mouse to.");
                    return false;
                }
                else
                {
                    ctrl.MoveWindowRelative(new Point("(" + cmds[1] + "," + cmds[2] + ")"));
                    return true;
                }
            }

            if (cmds[0].ToLower() == "movenatural")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No position given to move mouse to.");
                    return false;
                }
                else
                {
                    ctrl.MoveNatural(new Point("(" + cmds[1] + "," + cmds[2] + ")"));
                    return true;
                }
            }

            if (cmds[0].ToLower() == "movewindownatural")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No position given to move mouse to.");
                    return false;
                }
                else
                {
                    ctrl.MoveWindowNatural(new Point("(" + cmds[1] + "," + cmds[2] + ")"));
                    return true;
                }
            }

            #region Left Button
            if (cmds[0].ToLower() == "leftdown")
            {
                ctrl.LeftDown();
                return true;
            }
            if (cmds[0].ToLower() == "leftup")
            {
                ctrl.LeftUp();
                return true;
            }
            if (cmds[0].ToLower() == "leftclick")
            {
                ctrl.LeftClick();
                return true;
            }
            if (cmds[0].ToLower() == "doubleleftclick")
            {
                ctrl.LeftClickDouble();
                return true;
            }
            #endregion

            #region Right Button
            if (cmds[0].ToLower() == "rightdown")
            {
                ctrl.RightDown();
                return true;
            }
            if (cmds[0].ToLower() == "rightup")
            {
                ctrl.RightUp();
                return true;
            }
            if (cmds[0].ToLower() == "rightclick")
            {
                ctrl.RightClick();
                return true;
            }
            if (cmds[0].ToLower() == "doublerightclick")
            {
                ctrl.RightClickDouble();
                return true;
            }
            #endregion

            #region Middle Button
            if (cmds[0].ToLower() == "middledown")
            {
                ctrl.MiddleDown();
                return true;
            }
            if (cmds[0].ToLower() == "middleup")
            {
                ctrl.MiddleUp();
                return true;
            }
            if (cmds[0].ToLower() == "middleclick")
            {
                ctrl.RightClick();
                return true;
            }
            if (cmds[0].ToLower() == "doublemiddleclick")
            {
                ctrl.MiddleClickDouble();
                return true;
            }
            #endregion

            if (cmds[0].ToLower() == "randomtime")
            {
                if (cmds.Length > 2)
                {
                    if (cmds[1].Any(a=> Char.IsDigit(a) !=true ))
                    {
                        Console.WriteLine("randomtime must be an integer.");
                        return false;
                    }
                    if (cmds[2].Any(a => Char.IsDigit(a) != true))
                    {
                        Console.WriteLine("randomtime must be an integer.");
                        return false;
                    }
                    ctrl.sleeptimemin = int.Parse(cmds[1]);
                    ctrl.sleeptimemax = int.Parse(cmds[2]);
                    return true;
                }
                return true;
            }

            if (cmds[0].ToLower() == "randomdistance")
            {
                if (cmds.Length != 1)
                {
                    if (cmds[1].Any(a => Char.IsDigit(a) != true))
                    {
                        Console.WriteLine("randomdist must be an integer.");
                        return false;
                    }
                    ctrl.randomdist = int.Parse(cmds[1]);
                    return true;
                }
                return true;
            }

            if (cmds[0].ToLower() == "type")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No text given to write");
                    return false;
                }
                System.Windows.Forms.SendKeys.SendWait(cmds[1]);
                return true;

            }

            if (cmds[0].ToLower() == "goto")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("No line number given");
                    return false;
                }
                if(cmds[1][0] == '@')
                {
                    if (JumpPoints.ContainsKey(cmds[1]) == false)
                    {
                        Console.WriteLine("Cant find " + cmds[1]);
                        return false;
                    }
                    cmds[1] = JumpPoints[cmds[1]].ToString();
                }
                ctrl.currentLine = int.Parse(cmds[1])-1;
                //Subtract one becuase the for loop adds one right after commands is processed
                return true;

            }

            if (cmds[0].ToLower() == "var")
            {//Command;name;value;
                if (cmds.Length == 1 || cmds.Length == 2)
                {
                    Console.WriteLine("No variable name/value given");
                    return false;
                }
                Variable v = new Variable(cmds[1], cmds[2]);
                if (v.type == null)
                {
                    Console.WriteLine("invalid value " + cmds[2] + ". Value must be an integer");
                    return false;
                }
                else
                {
                    Variables.Add(cmds[1], v);
                }
                return true;
            }

            if (cmds[0].ToLower() == "console")
            {
                if (cmds.Length == 1)
                {
                    Console.WriteLine("");
                    return true;
                }
                Console.WriteLine(cmds[1]);
                return true;
            }

            if (cmds[0].ToLower() == "editvar")
            {//command;name;operator;value
                if (cmds.Length == 1 || cmds.Length == 2 || cmds.Length == 3)
                {
                    Console.WriteLine("No variable/operation/value given");
                    return false;
                }

                if (Variables.ContainsKey(cmds[1]) == false)
                {
                    Console.WriteLine("Unable to find variable " + cmds[1]);
                    return false;
                }
                int a = 0;
                switch (cmds[2])
                {
                    case "=":
                        Variables[cmds[1]].value = cmds[3];
                        break;
                    case "+":
                        a = int.Parse(Variables[cmds[1]].value) + int.Parse(cmds[3]);
                        Variables[cmds[1]].value = a.ToString();
                        break;
                    case "-":
                        a = int.Parse(Variables[cmds[1]].value) - int.Parse(cmds[3]);
                        Variables[cmds[1]].value = a.ToString();
                        break;
                }
                return true;
            }
     
            if (cmds[0] == "if")
            {//command;name;operator;value
                if (cmds.Length<4)
                {
                    Console.WriteLine("Invalid if");
                    return false;
                }

                switch (cmds[2])
                {
                    case "==":
                        if (!(int.Parse(cmds[1]) == int.Parse(cmds[3])))
                        {
                            ctrl.currentLine += 1;
                        }
                        break;
                    case "!=":
                        if (!(int.Parse(cmds[1]) != int.Parse(cmds[3])))
                        {
                            ctrl.currentLine += 1;
                        }
                        break;
                    case "<":
                        if (!(int.Parse(cmds[1]) < int.Parse(cmds[3])))
                        {
                            ctrl.currentLine += 1;
                        }
                        break;
                    case ">":
                        if (!(int.Parse(cmds[1]) > int.Parse(cmds[3])))
                        {
                            ctrl.currentLine += 1;
                        }
                        break;
                    case "<=":
                        if (! (int.Parse(cmds[1]) <= int.Parse(cmds[3])))
                        {
                            ctrl.currentLine += 1;
                        }
                        break;
                    case ">=":
                        if (!(int.Parse(cmds[1]) >= int.Parse(cmds[3])))
                        {
                            ctrl.currentLine += 1;
                        }
                        break;
                }
                return true;

            }

            if (cmds[0] == "resolution")
            {
                if (cmds.Length != 3)
                {
                    return false;
                }
                ctrl.res_x = float.Parse(cmds[1]);
                ctrl.res_y = float.Parse(cmds[2]);
                return true;

            }

            if (cmds[0] == "findcolor")
            {//command;color;varx;vary;rect
                string resultx = "999999999";
                string resulty = "999999999";
                if (cmds.Length != 8)
                {
                    return false;
                }

                if (Variables.ContainsKey(cmds[2]) == false)
                {
                    Console.WriteLine("Unable to find variable " + cmds[1]);
                    return false;
                }
                if (Variables.ContainsKey(cmds[3]) == false)
                {
                    Console.WriteLine("Unable to find variable " + cmds[1]);
                    return false;
                }

                Rect rect = new Rect();
                rect.fromString("("+cmds[4]+","+cmds[5]+","+cmds[6]+","+cmds[7]+")");

                for(int x=rect.Left; x<=rect.Right;x+=1)
                {
                    for (int y = rect.Top; y <= rect.Bottom; y += 1)
                    {
                        if (uint.Parse(cmds[1]) == ctrl.color(x,y))
                        {
                            resultx = x.ToString();
                            resulty = y.ToString();
                            x = rect.Right + 1;
                            break;
                        }
                    }
                }
                Variables[cmds[2]].value = resultx;
                Variables[cmds[3]].value = resulty;
                return true;
            }

            if (cmds[0] == "addressgetvalue")
            {//command;var;address

                if (cmds.Length != 3)
                {
                    return false;
                }

                if (Variables.ContainsKey(cmds[1]) == false)
                {
                    Console.WriteLine("Unable to find variable " + cmds[1]);
                    return false;
                }
                int addr = Convert.ToInt32(cmds[2].Substring(2), 16);

                Variables[cmds[1]].value = ctrl.AddressGetValue(addr).ToString();
                return true;
            }

            if (cmds[0].ToLower() == "holdkey")
            {//command;key;ms
                if (cmds.Length != 3)
                {
                    return true;
                }
                if (cmds[2].Any(a => Char.IsDigit(a) != true))
                {
                    Console.WriteLine("wait time must be an integer.");
                    return false;
                }
                ctrl.KeyHold(cmds[1], cmds[2]);
                return true;
            }

            if (cmds[0].ToLower() == "wait")
            {
                if (cmds.Length == 1)
                {
                    return true;
                }
                if (cmds[1].Any(a => Char.IsDigit(a) != true))
                {
                    Console.WriteLine("wait time must be an integer.");
                    return false;
                }
                ctrl.SleepCommand(int.Parse(cmds[1]));
                return true;
            }

            return false;
        }

        static void getPos(string window)
        {
            if (string.IsNullOrEmpty(window) == true)
            {
                Console.WriteLine("x: " + System.Windows.Forms.Cursor.Position.X);
                Console.WriteLine("y: " + System.Windows.Forms.Cursor.Position.Y);
                return;
            }
            if (ctrl.getWindow(window) == false)
            {
                Console.WriteLine("Cant find window " +window);
                return;
            }
            Console.WriteLine("x: " + (System.Windows.Forms.Cursor.Position.X-ctrl.rect.Left));
            Console.WriteLine("y: " + (System.Windows.Forms.Cursor.Position.Y-ctrl.rect.Top));
        }

        static void Run(string name)
        {
            string[] data = System.IO.File.ReadAllLines(name);

            //Find Jump Points
            for (ctrl.currentLine = 0; ctrl.currentLine < data.Length; ctrl.currentLine++)
            {
                if (string.IsNullOrEmpty(data[ctrl.currentLine]))
                {
                    data[ctrl.currentLine] = "#";
                }
                else if (data[ctrl.currentLine][0] == '@')
                {
                    JumpPoints.Add(data[ctrl.currentLine], ctrl.currentLine);
                }
            }

            Variables.Add("%currentline", new Variable("%currentline", ctrl.currentLine.ToString()));
            if (System.IO.File.Exists(name) == false)
            {
                Console.WriteLine("Cant find file " + name);
                return;
            }

            for (ctrl.currentLine = 0; ctrl.currentLine < data.Length; ctrl.currentLine++)
            {
                Variables["%currentline"].value = ctrl.currentLine.ToString();
                if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift)
                {
                    Console.WriteLine("Stopped");
                    break;
                }
                if (ParseCommand(data[ctrl.currentLine]) == false)
                {
                    Console.WriteLine("Unable to run command: " + data[ctrl.currentLine]);
                    break;
                }
                ctrl.Sleep();
            }
            Variables.Clear();
            JumpPoints.Clear();
        }
    }

    struct Point
    {
        public int x;
        public int y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(string pos)
        {
            pos = pos.Remove(0, 1);
            pos = pos.Remove(pos.Length - 1, 1);
            string[] poss = pos.Split(',');
            this.x = Int32.Parse(poss[0]);
            this.y = Int32.Parse(poss[1]);
        }
    }

    public struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Width;
        public int Height;
        public void setUp()
        {
            this.Width = Right - Left;
            this.Height = Bottom - Top;
        }

        public void fromString(string str)
        {
            str = str.Remove(0, 1);
            str = str.Remove(str.Length - 1, 1);
            string[] newstr = new string[4];
            newstr = str.Split(',');
            this.Left = int.Parse(newstr[0]);
            this.Top = int.Parse(newstr[1]);
            this.Right = int.Parse(newstr[2]);
            this.Bottom = int.Parse(newstr[3]);
        }
    }

    class Controller
    {
        #region init
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private const int MOUSE_MOVE = 0x0001;
        private const int MOUSE_LEFTDOWN = 0x0002;
        private const int MOUSE_LEFTUP = 0x0004;
        private const int MOUSE_RIGHTDOWN = 0x0008;
        private const int MOUSE_RIGHTUP = 0x0010;
        private const int MOUSE_MIDDLEDOWN = 0x0020;
        private const int MOUSE_MIDDLEUP = 0x0040;
        private const int MOUSE_ABSOLUTE = 0x8000;

        const int PROCESS_WM_READ = 0x0010;

        public int x;
        public int y;
        public int speed=5;
        public double direction;

        public int currentLine = 0;

        public bool beep = false;

        public int sleeptimemin = 100;
        public int sleeptimemax = 200;
        public int randomdist = 5;

        public float res_x = 1920.0f;
        public float res_y = 1080.0f;

        public IntPtr WindowHandle;
        public IntPtr processHandle;
        public Rect rect;
        #endregion

        #region Mouse

        public uint color(int x,int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            return pixel;
        }

        public int[] getMousePosition()
        {
            int[] result = new int[2];
            result[0] = System.Windows.Forms.Cursor.Position.X;
            result[1] = System.Windows.Forms.Cursor.Position.Y;
            return result;
        }

        #region Move Mouse Functions

        public void Move(int x, int y)
        {
            Random rng = new Random();
            if (randomdist != 0)
            {
                x += rng.Next(0, (randomdist * 2) + 1) - randomdist;
                y += rng.Next(0, (randomdist * 2) + 1) - randomdist;
            }
            int[] move = ScreenToDll(x+1, y+1, 1920, 1080);
            this.x = move[0];
            this.y = move[1];
            updateMousePosition();
        }

        public void Move(Point pt)
        {
            Random rng = new Random();
            if (randomdist != 0)
            {
                pt.x += rng.Next(0, (randomdist * 2) + 1) - randomdist;
                pt.y += rng.Next(0, (randomdist * 2) + 1) - randomdist;
            }
            int[] move = ScreenToDll(pt.x+1, pt.y+1, 1920, 1080);
            this.x = move[0];
            this.y = move[1];
            updateMousePosition();
        }

        public void MoveRelative(int x, int y)
        {
            Move(x + getMousePosition()[0], y + getMousePosition()[1]);
        }

        public void MoveRelative(Point pt)
        {
            Console.WriteLine((x + getMousePosition()[0]) + "," + (y + getMousePosition()[1]));
            Move(pt.x + getMousePosition()[0], pt.y + getMousePosition()[1]);
        }

        public void MoveWindowRelative(int x, int y)
        {
            Move(x+rect.Left,y+rect.Top);
        }

        public void MoveWindowRelative(Point pt)
        {
            Move(pt.x + rect.Left, pt.y + rect.Top);
        }

        public void MoveNatural(int targ_x, int targ_y)
        {
            int rdholder = randomdist;
            randomdist = 2;
            int next_x = 0;
            int next_y = 0;
            Random rng = new Random();
            int[] move = ScreenToDll(x, y, 1920, 1080);
            if (randomdist != 0)
            {
                targ_x += rng.Next(0, (randomdist * 2) + 1) - randomdist;
                targ_y += rng.Next(0, (randomdist * 2) + 1) - randomdist;
            }
            int mx = getMousePosition()[0];
            int my = getMousePosition()[1];
            float length = (float)Math.Sqrt( Math.Pow(mx - targ_x,2) + Math.Pow(my - targ_y, 2));
            float start_length = length;
            double angle = -Math.Atan2(targ_y-my, targ_x-mx);
            double rndangle;
            while (length > 3)
            {
                if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift)
                {
                    break;
                }

                rndangle = ((((double)(new Random(DateTime.Now.Millisecond).Next(0, (int)Remap(length, start_length, 0, 0, 1)))) * 2) - 1) / 100;
                direction = angle + rndangle;
                next_x = (int)(Math.Cos(direction) * speed);
                next_y = -(int)(Math.Sin(direction) * speed);
                MoveRelative(next_x, next_y);
                mx = getMousePosition()[0];
                my = getMousePosition()[1];
                length = (float)Math.Sqrt(Math.Pow(mx - targ_x, 2) + Math.Pow(my - targ_y, 2));
                angle = -Math.Atan2(targ_y - my, targ_x - mx);
                SleepCommand(1);
            }
            randomdist = rdholder;
        }

        public void MoveNatural(Point pt)
        {
            MoveNatural(pt.x, pt.y);
        }

        public void MoveWindowNatural(int targ_x, int targ_y)
        {
            MoveNatural(rect.Left + targ_x, rect.Top + targ_y);
        }

        public void MoveWindowNatural(Point pt)
        {
            MoveWindowNatural(pt.x, pt.y);
        }

        #endregion

        #region Left Mouse Functions
        public void LeftDown()
        {
            mouse_event(MOUSE_LEFTDOWN, this.x, this.y, 0, 0);
            Beep();
        }

        public void LeftUp()
        {
            mouse_event(MOUSE_LEFTUP, this.x, this.y, 0, 0);
            Beep();
        }

        public void LeftClick()
        {
            LeftDown();
            SleepCommand(new Random().Next(100,500));
            LeftUp();
        }

        public void LeftClickDouble()
        {
            LeftDown();
            SleepCommand(new Random().Next(25, 50));
            LeftUp();
            SleepCommand(new Random().Next(25, 50));
            LeftDown();
            SleepCommand(new Random().Next(25, 50));
            LeftUp();
        }
        #endregion

        #region Right Mouse Functions
        public void RightDown()
        {
            mouse_event(MOUSE_RIGHTDOWN, this.x, this.y, 0, 0);
            Beep();
        }

        public void RightUp()
        {
            mouse_event(MOUSE_RIGHTUP, this.x, this.y, 0, 0);
            Beep();
        }

        public void RightClick()
        {
            RightDown();
            SleepCommand(new Random().Next(100, 500));
            RightUp();
        }

        public void RightClickDouble()
        {
            RightDown();
            SleepCommand(new Random().Next(25, 50));
            RightUp();
            SleepCommand(new Random().Next(25, 50));
            RightDown();
            SleepCommand(new Random().Next(25, 50));
            RightUp();
        }

        #endregion

        #region Middle Mouse Functions
        public void MiddleDown()
        {
            mouse_event(MOUSE_MIDDLEDOWN, this.x, this.y, 0, 0);
            Beep();
        }

        public void MiddleUp()
        {
            mouse_event(MOUSE_MIDDLEUP, this.x, this.y, 0, 0);
            Beep();
        }

        public void MiddleClick()
        {
            MiddleDown();
            SleepCommand(new Random().Next(100, 500));
            MiddleUp();
        }

        public void MiddleClickDouble()
        {
            MiddleDown();
            SleepCommand(new Random().Next(25, 50));
            MiddleUp();
            SleepCommand(new Random().Next(25, 50));
            MiddleDown();
            SleepCommand(new Random().Next(25, 50));
            MiddleUp();
        }

        #endregion

        #region Key Presses

        public void KeyHold(string key, string ms)
        {
            int totalDuration = 0;
            while (totalDuration < int.Parse(ms))
            {
                keybd_event(Convert.ToByte(key.Substring(2), 16), 0, 0, 0);
                keybd_event(Convert.ToByte(key.Substring(2), 16), 0, 2, 0);
                System.Threading.Thread.Sleep(1);
                totalDuration += 1;
            }
        }

        #endregion

        private int[] ScreenToDll(int x, int y, int resolution_x, int resolution_y)
        {
            int mappedX = (int)Remap(x, 0.0f, this.res_x, 0, 65535);
            int mappedY = (int)Remap(y, 0.0f, this.res_y, 0, 65535);
            int[] result = new int[2];
            result[0] = mappedX;
            result[1] = mappedY;
            return result;
        }

        static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        private void updateMousePosition()
        {
            mouse_event(MOUSE_MOVE|MOUSE_ABSOLUTE, this.x, this.y, 0, 0);
        }
        #endregion

        public bool getWindow(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                return false;
            }
            this.WindowHandle = processes[0].MainWindowHandle;
            this.processHandle = OpenProcess(PROCESS_WM_READ, false, processes[0].Id);
            this.rect = new Rect();
            GetWindowRect(WindowHandle, ref rect);
            this.rect.setUp();
            return true;
        }

        public void windowUpdatePosition()
        {
            GetWindowRect(WindowHandle, ref rect);
            this.rect.setUp();
        }

        public void Beep()
        {
            if (this.beep == true)
            {
                Console.Beep();
            }
        }

        public void Sleep()
        {
            Random rng = new Random();
            System.Threading.Thread.Sleep(rng.Next(this.sleeptimemin,this.sleeptimemax));
        }

        public void SleepCommand(int miliseconds)
        {
            System.Threading.Thread.Sleep(miliseconds);
        }

        public int AddressGetValue(int address)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)this.processHandle, address, buffer, buffer.Length, ref bytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }
    }

    class Variable
    {
        public string value;
        public string name;
        public string type;
        public Variable(string name,string value)
        {
            this.value = value;
            if (value.All(a=>Char.IsDigit(a)))
            {
                type = "int";
            }
            else
            {
                type = null;
            }
        }

        public bool equals(string value)
        {
            return (int.Parse(this.value) == int.Parse(value));
        }

        public bool lessThan(string value)
        {
            return (int.Parse(this.value) < int.Parse(value));
        }

        public bool greaterThan(string value)
        {
            return (int.Parse(this.value) > int.Parse(value));
        }

        public bool lessThanOrEqual(string value)
        {
            return (int.Parse(this.value) <= int.Parse(value));
        }

        public bool greaterThanOrEqual(string value)
        {
            return (int.Parse(this.value) >= int.Parse(value));
        }
    }
}
