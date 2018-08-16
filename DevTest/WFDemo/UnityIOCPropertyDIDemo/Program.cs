using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace UnityIOCPropertyDIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            UnityContainer container = new UnityContainer();
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            //加载到容器
            config.Configure(container, "MyContainer");

            //返回调用者
            IUserService IUser = container.Resolve<IUserService>();
            //执行
            IUser.Display("王建");
            Console.ReadLine();
        }
    }
}
