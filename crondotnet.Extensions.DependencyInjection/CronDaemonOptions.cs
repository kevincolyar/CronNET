namespace Microsoft.Extensions.DependencyInjection
{
    internal class CronDaemonOptions
    {
        public List<JobOptions> Jobs { get; set; } = new List<JobOptions>();

    }
}