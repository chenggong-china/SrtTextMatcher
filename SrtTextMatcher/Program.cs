using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrtTextMatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                SrtTextMatcher matcher = new SrtTextMatcher("test.srt", "test.txt", "output.srt");
            }else
            {
                SrtTextMatcher matcher = new SrtTextMatcher(args[0], args[1], args[2]);
            }
 
            Console.ReadKey();
        }
    }
}
