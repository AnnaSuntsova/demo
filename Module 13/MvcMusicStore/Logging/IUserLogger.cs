using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcMusicStore.Logging
{
    public interface IUserLogger
    {
        void Error(string message);
        void Info(string message);
        void Debug(string message);
        void Trace(string message);
        void Warn(string message);
        void Fatal(string message);
    }
}
