using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancala
{
    public class Trace
    {
        public static bool ON = false;            // default - no output here
        public static int TraceDetailLevel = 10;  // default trace level set high (10)
        /*
         * print only if priority is higher (bigger number) than TraceDetailLevel
         */
        public static void println(String str, int priority)
        {
            if (ON && priority >= TraceDetailLevel)
            {
                Console.WriteLine(str);
            }
        }
        public static void println(String str, int priority, int depth)
        {
            if (ON && priority >= TraceDetailLevel && depth == 4)
            {
                Console.WriteLine(str);
            }
        }
    }
}
