using Quartz;

namespace NetJob.Job.Jobs
{
    internal class TestJob : JobBase
    {
        public override Task Run(IJobExecutionContext context)
        {

            Console.WriteLine("Hello, World! Hello First Job");


            return Task.FromResult(true);
        }
    }
}
