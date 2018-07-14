using System;

namespace ArmChair.Utils.Logging
{
    using System.Diagnostics;

    public interface ILogger
    {
        void Log(Func<string> message);
    }


    public class NullLogger : ILogger
    {
        public void Log(Func<string> message)
        {
        }
    }
}
