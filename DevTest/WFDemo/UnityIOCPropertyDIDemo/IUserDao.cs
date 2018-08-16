using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityIOCPropertyDIDemo
{
    interface IUserDao
    {
        void Display(string mes);
    }

    interface IUserService
    {
        void Display(string mes);
    }


    public class UserDaoImpl : IUserDao
    {
        public void Display(string mes)
        {
            Console.WriteLine(mes);
        }
    }
    class UserImpl : IUserService
    {      
        //只需要在对象成员前面加上[Dependency]，
        //就是把构造函数去掉，成员对象上面加[Dependency]注入
        [Dependency]
        public IUserDao IUserDao { get; set; }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="mes"></param>
        public void Display(string mes)
        {
            IUserDao.Display(mes);
        }



    }
}
