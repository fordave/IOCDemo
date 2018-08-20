using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace UnityIOCDemo
{
    class Program
    {
        private static IUnityContainer container = null;
        static void Main(string[] args)
        {
            RegisterContainer();

            var programmer = container.Resolve<IProgrammer>("CSharp");
            var programmer1 = container.Resolve<IProgrammer>("VB");
            var programmer2 = container.Resolve<IProgrammer>("Java");
            programmer.Working();
            programmer1.Working();
            programmer2.Working();

            Console.ReadKey();
        }
        private static void RegisterContainer()
        {
            container = new UnityContainer();
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            config.Configure(container, "Programmer");
        }
    }
}
