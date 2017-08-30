using System;

namespace CronNET.Interfaces
{
    public interface ICronJob
    {
        void Execute(DateTime dateTime);
        void Abort();
    }
}