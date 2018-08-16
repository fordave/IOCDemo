using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityIOCMethodDIDemo
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
        
        public IUserDao IUserDao { get; set; }

        /// <summary>
        /// 方法注入-加[InjectionMethod]属性
        /// </summary>
        /// <param name="IUserDal"></param>
        [InjectionMethod]
        public void SetInjection(IUserDao IUserDal)
        {
            IUserDao = IUserDal;
        }
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
