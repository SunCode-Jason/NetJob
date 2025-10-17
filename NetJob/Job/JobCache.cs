namespace NetJob.Job;

/// <summary>
/// 任务日志表
/// </summary>
public class TasksLog
{
    /// <summary>
    /// 任务ID
    /// </summary>
    public long JobId { get; set; }
    /// <summary>
    /// 任务耗时
    /// </summary>
    public double TotalTime { get; set; }
    /// <summary>
    /// 执行结果(0-失败 1-成功)
    /// </summary>
    public bool RunResult { get; set; }
    /// <summary>
    /// 运行时间
    /// </summary>
    public DateTime RunTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }
    /// <summary>
    /// 执行参数
    /// </summary>
    public string RunPars { get; set; }
    /// <summary>
    /// 异常信息
    /// </summary>
    public string ErrMessage { get; set; }
    /// <summary>
    /// 异常堆栈
    /// </summary>
    public string ErrStackTrace { get; set; }
    /// <summary>
    /// 创建ID
    /// </summary>
    public int? CreateId { get; set; }
    /// <summary>
    /// 创建者
    /// </summary>
    public string CreateBy { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 修改ID
    /// </summary>
    public int? ModifyId { get; set; }
    /// <summary>
    /// 修改者
    /// </summary>
    public string ModifyBy { get; set; }
    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModifyTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 任务分组
    /// </summary>
    public string JobGroup { get; set; }
}


public static class JobCache
{
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, TasksQz> cacheJobData = new System.Collections.Concurrent.ConcurrentDictionary<string, TasksQz>();

    private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, List<TasksLog>> cacheLogData = new System.Collections.Concurrent.ConcurrentDictionary<string, List<TasksLog>>();

    /// <summary>
    ///  获取数据
    /// </summary>
    /// <param name="key">缓存的Key</param>
    /// <returns></returns>
    public static T? Get<T>(string key) where T : class
    {
        switch (typeof(T).Name)
        {
            case "TasksQz":
                {
                    if (cacheJobData.TryGetValue(key, out TasksQz cache))
                    {
                        var result = cache as T;
                        return result;
                    }
                }
                break;
            case "List<TasksLog>":
                {
                    if (cacheLogData.TryGetValue(key, out List<TasksLog> cache))
                    {
                        var result = cache as T;
                        return result;
                    }
                }
                break;
            default:
                break;
        }

        return default;
    }

    /// <summary>
    ///  设置数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">缓存的Key</param>
    /// <param name="value">缓存的数据</param>
    /// <param name="expMinute">缓存的过期时间</param>
    /// <returns></returns>
    public static bool Set<T>(string key, dynamic value, int expMinute = 5)
    {
        try
        {
            switch (typeof(T).Name)
            {
                case "TasksQz":
                    {
                        cacheJobData.TryRemove(key, out _);
                        cacheJobData.TryAdd(key, value);
                    }
                    break;
                case "TasksLog":
                    {
                        var logList = Get<List<TasksLog>>(key) ?? [];
                        logList.Add(value);
                        cacheLogData.TryRemove(key, out _);
                        cacheLogData.TryAdd(key, logList);
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

}
