﻿/*
* PROJECT:          Aura Operating System Development
* CONTENT:          Command Interpreter - Cat
* PROGRAMMER(S):    Valentin Charbonnier <valentinbreiz@gmail.com>
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Aura_OS.Interpreter.Commands.Filesystem
{
    class CommandHex : ICommand
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CommandHex(string[] commandvalues) : base(commandvalues, CommandType.Filesystem)
        {
            Description = "to print an a file in hexadecimal";
        }

        /// <summary>
        /// CommandHex
        /// </summary>
        public override ReturnInfo Execute(List<string> arguments)
        {
            try
            {
                string file = arguments[0];

                if (File.Exists(Kernel.CurrentDirectory + file))
                {
                    Kernel.console.WriteLine("Offset(h)  00 01 02 03 04 05 06 07  08 09 0A 0B 0C 0D 0E 0F");
                    Kernel.console.WriteLine();
                    Kernel.console.WriteLine(Utils.Conversion.HexDump(File.ReadAllBytes(Kernel.CurrentDirectory + file)));
                }
                else
                {
                    Kernel.console.WriteLine("This file does not exist.");
                }
                return new ReturnInfo(this, ReturnCode.OK);
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Print /help information
        /// </summary>
        public override void PrintHelp()
        {
            Kernel.console.WriteLine("Usage:");
            Kernel.console.WriteLine(" - hex {file}");
        }
    }
}