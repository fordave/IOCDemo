using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOPDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var oUserTest1 = new User() { Name = "test2222", PassWord = "yxj" };
                var oUserTest2 = new User() { Name = "test3333", PassWord = "yxj" };
                var oUser = UserOperation.GetInstance();
                oUser.Test(oUserTest1);
                oUser.Test2(oUserTest1, oUserTest2);
            }
            catch (Exception ex)
            {
                //throw;
            }
            Console.Read();
        }
    }
}
