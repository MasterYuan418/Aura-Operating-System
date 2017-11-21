﻿using System;
using Aura_OS.HAL;
using static Cosmos.Core.INTs;
using System.Collections.Generic;
using XSharp;

namespace Aura_OS.Core
{
    class MSDOS_Syscalls : Driver
    {
        public override bool Init()
        {
            Name = "MSDOS API";
            SetIntHandler(0x48, SWI); //ints.setinthandler
            return true;
        }

        public static int x = Console.CursorLeft;
        public static int y = Console.CursorTop;

        public unsafe static void SWI(ref IRQContext aContext)
        {
            if (aContext.Interrupt == 0x48)
            {
                //Console.WriteLine("'" + aContext.EAX + "'");
                if (aContext.EAX == 0x01)
                {
                    //Console.WriteLine("EAX is 0x01");
                    uint ptr = aContext.ESI;
                    byte* dat = (byte*)(ptr + System.Executable.COM.ProgramAddress);
                    for (int i = 0; dat[i] != 0; i++)
                    {
                        if ((char)dat[i] == 0x0A)
                            Console.WriteLine("\n");
                        else
                            Console.Write((char)dat[i]);
                    }
                }
                else if (aContext.EAX == 0x02)
                {
                    Console.Clear();
                }
                else if (aContext.EAX == 0x03)
                {
                    uint xesi = aContext.ESI;
                    uint yedi = aContext.EDI;
                    Console.SetCursorPosition((int)xesi, (int)yedi);
                }
                else if (aContext.EAX == 0x31)
                {
                    Console.SetCursorPosition(x, y);
                }
                else if (aContext.EAX == 0x04) // Read from line
                {
                    uint ptr = aContext.ESI;
                    byte* dat = (byte*)(ptr + System.Executable.COM.ProgramAddress);

                    string input = "";

                    for (int i = 0; dat[i] != 0; i++)
                    {
                        input = input + (char)dat[i];
                    }

                    Console.Write(input);
                    string output = Console.ReadLine();

                    uint ptr2 = aContext.EDI;
                    byte* dat2 = (byte*)(ptr2 + System.Executable.COM.ProgramAddress);

                    List<byte> list = new List<byte>();

                    foreach (char charr in output)
                    {
                        list.Add(System.Utils.Convert.StringToByte(charr));
                    }

                    byte[] test = list.ToArray();

                    for (int i = 0; i < test.Length; i++)
                    {
                        dat2[i] = test[i];
                    }

                    aContext.EDI = (uint)dat2 - System.Executable.COM.ProgramAddress;

                    uint ptr3 = aContext.EDI;
                    byte* dat3 = (byte*)(ptr3 + System.Executable.COM.ProgramAddress);

                    string input3 = "";

                    for (int i = 0; dat3[i] != 0; i++)
                    {
                        input3 = input3 + (char)dat3[i];
                    }

                }
            }

        }
    }
}
