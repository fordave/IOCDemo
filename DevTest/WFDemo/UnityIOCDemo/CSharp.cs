using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityIOCDemo
{

    public class CSharp : IProgrammer
    {
        public void Working()
        {
            Console.WriteLine("programming C# ...");
        }
    }

    public class VB : IProgrammer
    {
        public void Working()
        {
            Console.WriteLine("programming VB ...");
        }
    }

    public class Java : IProgrammer
    {
        public void Working()
        {
            Console.WriteLine("programming Java ...");
        }
    }


}
