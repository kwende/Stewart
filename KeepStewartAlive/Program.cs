using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeepStewartAlive
{
    class Program
    {
        static void Main(string[] args)
        {
            for(;;)
            {
                Process[] procsNamedStewart = Process.GetProcessesByName("Stewart"); 

                if(procsNamedStewart.Length == 0)
                {
                    Process.Start("Stewart.exe"); 
                }

                Thread.Sleep(1000); 
            }
        }
    }
}
