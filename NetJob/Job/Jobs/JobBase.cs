using Quartz;

namespace NetJob.Job.Jobs;

public class JobBase : IJob
{
    /// <summary>
    /// 执行指定任务
    /// </summary>
    /// <param name="context"></param>
    /// <param name="action"></param>
    public async Task<string> ExecuteJob(IJobExecutionContext context, Func<Task> func)
    {
        //记录Job
        TasksLog tasksLog = new TasksLog();
        //JOBID
        int jobid = Convert.ToInt32(context.JobDetail.Key.Name);
        //JOB组名
        string groupName = context.JobDetail.Key.Group;
        //日志
        tasksLog.JobId = jobid;
        tasksLog.RunTime = DateTime.Now;
        string jobHistory = $"【{tasksLog.RunTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行开始】【Id：{jobid}，组别：{groupName}】";
        try
        {
            await func();//执行任务
            tasksLog.EndTime = DateTime.Now;
            tasksLog.RunResult = true;
            jobHistory += $"，【{tasksLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行成功】";

            JobDataMap jobPars = context.JobDetail.JobDataMap;
            tasksLog.RunPars = jobPars.GetString("JobParam");
        }
        catch (Exception ex)
        {
            tasksLog.EndTime = DateTime.Now;
            tasksLog.RunResult = false;
            //JobExecutionException e2 = new JobExecutionException(ex);
            //true  是立即重新执行任务 
            //e2.RefireImmediately = true;
            tasksLog.ErrMessage = ex.Message;
            tasksLog.ErrStackTrace = ex.StackTrace;
            jobHistory += $"，【{tasksLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行失败:{ex.Message}】";
        }
        finally
        {
            tasksLog.TotalTime = Math.Round((tasksLog.EndTime - tasksLog.RunTime).TotalSeconds, 3);
            jobHistory += $"(耗时:{tasksLog.TotalTime}秒)";

            var model = JobCache.Get<TasksQz>(jobid.ToString());
            if (model != null)
            {
                JobCache.Set<TasksLog>(jobid.ToString(), tasksLog);
                model.RunTimes += 1;
                if (model.TriggerType == Enum.JobTriggerTypeEnum.Simple) model.CycleHasRunTimes += 1;
                if (model.TriggerType == Enum.JobTriggerTypeEnum.Simple && model.CycleRunTimes != 0 && model.CycleHasRunTimes >= model.CycleRunTimes) model.IsStart = false;// 循环完善,当循环任务完成后,停止该任务,防止下次启动再次执行
                var separator = "<br>";
                // 这里注意数据库字段的长度问题，超过限制，会造成数据库remark不更新问题。

                List<string> GetTopDataBySeparator(string content, string separator, int top, bool isDesc = false)
                {
                    if (string.IsNullOrEmpty(content))
                    {
                        return new List<string>() { };
                    }

                    if (string.IsNullOrEmpty(separator))
                    {
                        throw new ArgumentException("message", nameof(separator));
                    }

                    var dataArray = content.Split(separator).Where(d => !string.IsNullOrEmpty(d)).ToArray();
                    if (isDesc)
                    {
                        Array.Reverse(dataArray);
                    }

                    if (top > 0)
                    {
                        dataArray = dataArray.Take(top).ToArray();
                    }

                    return dataArray.ToList();
                }

                model.Remark =
                    $"{jobHistory}{separator}" + string.Join(separator, GetTopDataBySeparator(model.Remark, separator, 9));

                JobCache.Set<TasksQz>(jobid.ToString(), model);
            }
        }

        return jobHistory;
    }

    /// <summary>
    /// 但是调用其他类方法 上下文ContextID就不一样
    /// </summary>
    /// <param name="context"></param>
    public async Task Execute(IJobExecutionContext context)
    {
        var executeLog = await ExecuteJob(context, async () => await Run(context));
    }

    /// <summary>
    ///  任务执行
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual async Task Run(IJobExecutionContext context)
    {
        await Task.FromResult(true);
    }
}
