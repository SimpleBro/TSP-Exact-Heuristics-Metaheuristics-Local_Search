using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.Miscellaneous
{
    internal class ILogger
    {
        public string time = String.Empty;
        public string domain = String.Empty;
        public string method = String.Empty;
        public string status = String.Empty;
        public string message = String.Empty;
        public string execution = String.Empty;
        readonly public string iLoggerType = typeof(ILogger).Name;

        public ILogger(string domain, string method, string status, string execution, string message)
        {
            DateTime dt = DateTime.Now;
            this.time = dt.ToString("HH:mm:ss.ffffff");
            this.domain = domain;
            this.method = method;
            this.status = status;
            this.execution = execution;
            this.message = message;
        }
    }
}
