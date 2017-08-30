using System;

namespace CronNET.Interfaces
{
    public interface ICronSchedule
    {
        bool IsValid(string expression);
        bool IsTime(DateTime dateTime);
    }
}