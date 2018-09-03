using Aura_OS.System.GUI.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
 namespace Aura_OS.System.GUI.UI
{
    public class WindowsManager
    {
         public static List<Window> Active_Windows { get; protected set; } = new List<Window>();
         public static void AddWindow(int sizex, int sizey, int posx, int posy, string name)
        {
            Window win = new Window();
            Util.Point position = new Util.Point((uint)posx, (uint)posy);
            Util.Point size = new Util.Point((uint)sizex, (uint)sizey);
            win.pos = position;
            win.size = size;
            win.Name = name;
            AddWindow(win);
        }
         public static void AddWindow(Window window)
        {
            Active_Windows.Add(window);
        }
         public static void ShowWindows()
        {
            foreach (Window wind in Active_Windows)
            {
                wind.Draw();
            }
        }
    }
     public class Window
    {
         public string Name;
         public Util.Point pos;
         public Util.Point size;
         public Util.Area CloseArea;
         public void Draw()
        {
            Desktop.g.FillRectangle((int)pos.X, (int)pos.Y - 21, (int)size.X, 20, Colors.AliceBlue);
            Desktop.g.DrawString(Name, (int)pos.X + 1, (int)pos.Y - 20, Colors.Black, Fonts.CFF._Pixel7_Mini_cff);
            Desktop.g.FillRectangle((int)pos.X, (int)pos.Y - 1, (int)size.X + 1, (int)size.Y + 1, Colors.White);
            Desktop.g.DrawRectangle((int)pos.X - 1, (int)pos.Y - 22, (int)size.X + 2, (int)size.Y + 22, Colors.DarkGray);
            Desktop.g.FillRectangle((int)pos.X + (int)size.X - 19, (int)pos.Y - 21, 20, 20, Colors.Red);
            Desktop.g.FillRectangle((int)pos.X + (int)size.X - 39, (int)pos.Y - 21, 20, 20, Colors.Orange);
            CloseArea.X = (int)pos.X + (int)size.X - 19;
            CloseArea.Y = (int)pos.Y - 20;
            CloseArea.XMAX = CloseArea.X + 19;
            CloseArea.YMAX = CloseArea.Y + 19;
        }
     }
     public enum WindowState
    {
         fullscreen = 0x01,
        min = 0x02,
        max = 0x03,
        windowed = 0x04,
     }
     public enum BorderStyle
    {
         None = 0x00,
        Fixed3D = 0x10,
        FixedDialog = 0x11,
        FixedSingle = 0x12,
        FixedToolWindow = 0x13,
        Sizable = 0x20,
        SizableToolWindow = 0x21
     }
}