using Castle.DynamicProxy;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CastleAOPDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyGenerator generator = new ProxyGenerator();
            var test = generator.CreateClassProxy<TestA>(new TestInterceptor());
            Console.WriteLine($"GetResult:{test.GetResult(Console.ReadLine())}");
            test.GetResult2("test");
            Console.ReadKey();
        }
    }

    public class TestInterceptor : StandardInterceptor
    {
        private static NLog.Logger logger;

        protected override void PreProceed(IInvocation invocation)
        {
            Console.WriteLine(invocation.Method.Name + "执行前,入参：" + string.Join(",", invocation.Arguments));
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            Console.WriteLine(invocation.Method.Name + "执行中");
            try
            {
                base.PerformProceed(invocation);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine(invocation.Method.Name + "执行后，返回值：" + invocation.ReturnValue);
        }

        private void HandleException(Exception ex)
        {
            if (logger == null)
            {
                LoggingConfiguration config = new LoggingConfiguration();

                ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
                consoleTarget.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}";
                config.AddTarget("console", consoleTarget);

                LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule1);
                LogManager.Configuration = config;

                logger = LogManager.GetCurrentClassLogger(); //new NLog.LogFactory().GetCurrentClassLogger();
            }
            logger.ErrorException("error", ex);
        }
    }

    public class TestA
    {
        public virtual string GetResult(string msg)
        {
            string str = $"{DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss")}---{msg}";
            return str;
        }

        public virtual string GetResult2(string msg)
        {
            throw new Exception("throw Exception!");
        }
    }
}
