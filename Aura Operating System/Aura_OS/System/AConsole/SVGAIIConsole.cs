﻿/*
* PROJECT:          Aura Operating System Development
* CONTENT:          VBE VESA Console
* PROGRAMMERS:      Valentin Charbonnier <valentinbreiz@gmail.com>
*/


using System;
using System.Drawing;
using System.Text;
using Aura_OS.System.Graphics;
using Cosmos.Debug.Kernel;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;

namespace Aura_OS.System.AConsole
{
    public class SVGAIIConsole : Console
    {
        public Color BackColor, ForeColor;
        public Font Font;

        private static uint[] Pallete = new uint[16];

        SVGAIIGraphics SVGA;

        public SVGAIIConsole()
        {
            Name = "SVGAII";
            Type = ConsoleType.Graphical;

            Pallete[0] = 0xFF000000; // Black
            Pallete[1] = 0xFF0000AB; // Darkblue
            Pallete[2] = 0xFF008000; // DarkGreen
            Pallete[3] = 0xFF008080; // DarkCyan
            Pallete[4] = 0xFF800000; // DarkRed
            Pallete[5] = 0xFF800080; // DarkMagenta
            Pallete[6] = 0xFF808000; // DarkYellow
            Pallete[7] = 0xFFC0C0C0; // Gray
            Pallete[8] = 0xFF808080; // DarkGray
            Pallete[9] = 0xFF5353FF; // Blue
            Pallete[10] = 0xFF55FF55; // Green
            Pallete[11] = 0xFF00FFFF; // Cyan
            Pallete[12] = 0xFFAA0000; // Red
            Pallete[13] = 0xFFFF00FF; // Magenta
            Pallete[14] = 0xFFFFFF55; // Yellow
            Pallete[15] = 0xFFFFFFFF; //White

            SVGA = new SVGAIIGraphics();

            mWidth = SVGA.Width;
            mHeight = SVGA.Height;

            Font = PCScreenFont.LoadFont(Convert.FromBase64String("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AAwwAAB+AgQIECBAfgAAAAAQEAAAfgIECBAgQH4AAAAAQjwAAAAAAAAAAAAAAAAAABAQAAAAAAAAAAAAAAAAAAAkGAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAM/f///8AD//9gIv//ZCL//2Ui//+gJawlriX8Jf4lGysOIv//xiVmJiUrJyv//7wA//+9AP//vgD//6YA//+oAP//uAD//5IB//8gIP//ISD//zAg//8iIf//JiD//zkg//86IP//HCAfIP//HSDuAv//HiD//0Iu//9BLs4C//8eAf//HwH//zAB//8xAf//XgH//18B//8gAKAAACABIAIgAyAEIAUgBiAHIAggCSAKIC8gXyD//yEA//8iAP//IwD//yQA//8lAP//JgD//ycAvAL//ygA//8pAP//KgBOIBci//8rAP//LADPAnUDGiD//y0ArQAQIBEgEiATIEMgEiL//y4AJCD//y8ARCAVIv//MAD//zEA//8yAP//MwD//zQA//81AP//NgD//zcA//84AP//OQD//zoANiL//zsA//88AP//PQBALv//PgD//z8A//9AAP//QQAQBJED//9CABIEkgP//0MAIQT5A///RAD//0UAFQSVA///RgD//0cA//9IAB0ElwP//0kABgTABM8EmQP//0oACAR/A///SwAaBJoDKiH//0wA//9NABwEnAP6A///TgCdA///TwAeBJ8D//9QACAEoQP//1EAGgX//1IA//9TAAUE//9UACIEpAP//1UA//9WAP//VwAcBf//WAAlBKcD//9ZAK4EpQP//1oAlgP//1sA//9cAPUp//9dAP//XgDEAsYCAyP//18A//9gAMsC7x81IP//YQAwBP//YgD//2MAQQTyA///ZAD//2UANQT//2YA//9nAP//aAD//2kAVgT//2oAWATzA///awD//2wA//9tAP//bgD//28APgS/A///cABABP//cQAbBf//cgD//3MAVQT//3QA//91AP//dgD//3cAHQX//3gARQT//3kAQwT//3oA//97AP//fAAjIv//fQD//34A3ALAH///IiAZIs8l///AAP//wQD//8IA///DAP//xADSBP//xQArIf//xgDUBP//xwD//8gAAAT//8kA///KAP//ywABBP//zAD//80A///OAP//zwAHBKoD///QABAB///RAP//0gD//9MA///UAP//1QD//9YA5gT//9cA///YAP//2QD//9oA///bAP//3AD//90A///eAPcD///fAP//kiX//6EA//+iAP//owD//6wg//+lAP//YAH//6cA//9hAf//qQD//6oA//+rAP//rAD//6QA//+uAP//rwDJAv//sADaAv//sQD//7IA//+zAP//fQH//7UAvAP//7YA//+3AIcDJyDFIjEu//9+Af//uQD//7oA//+7AP//UgH//1MB//94AasD//+/AP//ACUUIBUgryP//wIl//8MJW0l//8QJW4l//8UJXAl//8YJW8l//8cJf//JCX//ywl//80Jf//PCX//5El//+6Iz4g//+7I///vCP//70j//9QJQEl//9RJQMl//9UJQ8l//9XJRMl//9aJRcl//9dJRsl//9gJSMl//9jJSsl//9mJTMl//9pJTsl//9sJUsl//+IJf//kSH//5Mh//+QIf//kiH//+AA///hAP//4gD//+MA///kANME///lAP//5gDVBP//5wD//+gAUAT//+kA///qAP//6wBRBP//7AD//+0A///uAP//7wBXBP//8AD///EA///yAP//8wD///QA///1AP//9gDnBP//9wD///gA///5AP//+gD///sA///8AP///QD///4A+AP///8A//+0ALkCygJ0A/0fMiD//7sCvQL+HxggGyD//xkgvR+/H///NiD//zMgugLdAv//FiH//7Qg//+9IP//EQSCAf//EwSTA///FAT//xYE//8XBP//GAR2A///GQT//xsE//8fBKADDyL//yME//8kBKYD//8mBP//JwT//ygE//8pBP//KgT//ysE//8sBP//LQT//y4E//8vBP//BAT//w4E//8CBP//AwT//wkE//8KBP//CwT//wwEMB7//w8E//+QBP//DQT//zEE//8yBP//MwT//zQE//82BP//NwT//zgEdwP//zkE//86BP//OwT//zwE//89BP//PwT//0IE//9EBNUDeAL//0YE//9HBP//SAT//0kE//9KBP//SwT//0wE//9NBPYD//9OBP//TwT//1QE9QP//14E//9SBP//UwT//1kE//9aBP//WwQnAf//XAT//18E//+RBP//XQT//3oDvh///4QD//+FA///hgP//4gD//+JA///igP//4wD//+OA///jwP//5AD//+UAwYi//+YA58B9ANyBOgE//+bA0UC//+eA///owOpAREi//+oA///qQMmIf//rAP//60D//+uA///rwP//7AD//+xA///sgP//7MD//+0A58e//+1A1sC//+2A///twOeAf//uAP//7kDaQL//7oDOAH//7sD//+9A///vgP//8ED///CA///wwP//8QD///FA///xgP//8cD///IA///yQP//8oD///LA///zAP//80D///OA///AAH//wIB0AT//wQB//8CHv//BgH//wgB//8KAf//DAH//woe//8OAf//EgH//xYB//8aAf//GAH//x4e//8cAf//IAH//yIB//8kAf//JgH//ygB//8qAf//LgH//zQB//82Af//OQH//z0B//87Af//QQH//0Ae//9DAf//RwH//0UB//9KAf//TAH//1AB//9WHv//VAH//1gB//9WAf//WgH//1wB//9gHv//GAL//2oe//9kAf//GgL//2IB//9mAf//aAH//2oB//9sAf//bgH//3AB//9yAf//gB7//4Ie//90Af//hB7///Ie//92Af//eQH//3sB//8BAf//AwHRBP//BQH//wMe//8HAf//CQH//wsB//8NAf//Cx7//w8B//8RAf//EwH//xcB//8bAf//GQH//x8e//8dAf//IQH//yMB//8lAf//KQH//ysB//8vAf//NQH//zcB//86Af//PgH//zwB//9CAf//QR7//0QB//9IAf//RgH//0sB//9NAf//UQH//1ce//9VAf//WQH//1cB//9bAf//XQH//2Ee//8ZAv//ax7//2UB//8bAv//YwH//2cB//9pAf//awH//20B//9vAf//cQH//3MB//+BHv//gx7//3UB//+FHv//8x7//3cB//96Af//fAH//9gC///ZAv//xwL//9sC//8="));

            mCols = SVGA.Width / Font.Width;
            mRows = SVGA.Height / Font.Height;
            
            BackColor = Color.Black;
            ForeColor = Color.White;
            Clear();
        }

        protected int mX = 0;
        public override int X
        {
            get { return mX; }
            set
            {
                mX = value;
                UpdateCursor();
            }
        }


        protected int mY = 0;
        public override int Y
        {
            get { return mY; }
            set
            {
                mY = value;
                UpdateCursor();
            }
        }

        public static int mWidth;
        public override int Width
        {
            get { return mWidth; }
        }

        public static int mHeight;
        public override int Height
        {
            get { return mHeight; }
        }

        public static int mCols;
        public override int Cols
        {
            get { return mCols; }
        }

        public static int mRows;
        public override int Rows
        {
            get { return mRows; }
        }

        public static uint foreground = (byte)ConsoleColor.White;
        public override ConsoleColor Foreground
        {
            get { return (ConsoleColor)foreground; }
            set
            {
                foreground = (byte)global::System.Console.ForegroundColor;
                ForeColor = Color.FromArgb((int)Pallete[foreground]);
            }
        }

        public static uint background = (byte)ConsoleColor.Black;

        public override ConsoleColor Background
        {
            get { return (ConsoleColor)background; }
            set
            {
                background = (byte)global::System.Console.BackgroundColor;
                BackColor = Color.FromArgb((int)Pallete[background]);
            }
        }

        public override int CursorSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static bool cursorvisible = false;
        public override bool CursorVisible { get => cursorvisible; set => cursorvisible = value; }

        public override void Clear()
        {
            Clear(BackColor);
        }

        public override void Clear(uint color)
        {
            Clear(Color.FromArgb((int)color));
        }

        public void Clear(Color bg)
        {
            BackColor = bg;
            SVGA.Clear(bg);
            mX = 0;
            mY = 0;
            UpdateCursor();
        }

        public override void UpdateCursor()
        {
            SVGA.SVGA.Update(0, 0, (uint)SVGA.Width, (uint)SVGA.Height);
        }

        public void Scroll()
        {
            UpdateCursor();
            int h = Font.Height;
            SVGA.Copy(0, h, 0, 0, mCols * (Font.Width), SVGA.Height - h);
            for (int i = 0; i < mCols; i++) { SVGA.DrawChar(i * (Font.Width), SVGA.Height - h, ' ', ForeColor, BackColor, Font); }
            mX = 0;
            mY = mRows - 1;
            UpdateCursor();
        }

        private void DoCarriageReturn()
        {
            mX = 0;
            UpdateCursor();
        }

        public void NewLine()
        {
            UpdateCursor();
            mX = 0;
            mY++;
            if (mY >= mRows)
            {
                Scroll();
            }
            UpdateCursor();
        }

        /// <summary>
        /// Write char to the console.
        /// </summary>
        /// <param name="aChar">A char to write</param>
        public void Write(char aChar)
        {
            if (aChar == 0)
                return;

            UpdateCursor();
            SVGA.DrawChar(mX * (Font.Width), mY * (Font.Height), aChar, ForeColor, BackColor, Font);
            mX++;
            if (mX >= mCols) { NewLine(); return; }
            UpdateCursor();
        }

        public override void Write(char[] aText)
        {
            for (int i = 0; i < aText.Length; i++)
            {
                switch (aText[i])
                {
                    case LineFeed:
                        NewLine(); //DoLineFeed();
                        break;

                    case CarriageReturn:
                        DoCarriageReturn();
                        break;

                    case Tab:
                        DoTab();
                        break;

                    /* Normal characters, simply write them */
                    default:
                        Write(aText[i]);
                        break;
                }
            }
        }

        public override void Write(byte[] aText)
        {
            //throw new NotImplementedException();
        }

        private void DoTab()
        {
            Write(Space);
            Write(Space);
            Write(Space);
            Write(Space);
        }

        public override void DrawImage(ushort X, ushort Y, Bitmap image)
        {
            //graphics.canvas.DrawImage(image, X, Y);
        }
    }
}
