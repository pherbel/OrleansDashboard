using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TestHost.Logger
{
    public class EventSourceLogger : ILogConsumer

    {

        private readonly ServiceEventSource eventSource;

        private readonly string pid;



        public EventSourceLogger()

        {

            this.eventSource = ServiceEventSource.Current;

            this.pid = Process.GetCurrentProcess().Id.ToString();

        }



        public void Log(

            Severity severity,

            LoggerType loggerType,

            string caller,

            string message,

            IPEndPoint myIPEndPoint,

            Exception exception,

            int eventCode = 0)

        {

            if (exception != null) eventSource.Message($"[{severity}@{myIPEndPoint}@PID:{pid}] {message}\nException: {exception}");

            else eventSource.Message($"[{severity}@{myIPEndPoint}@PID:{pid}] {message}");

        }

    }
}
