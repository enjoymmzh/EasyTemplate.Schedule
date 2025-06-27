namespace EasyTemplate.Schedule;

public class CacheToDBSyncService : ServiceBase
{
    public CacheToDBSyncService()
    {
        Description = "缓存数据持久化服务";
    }

    private void RunWork()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    ServiceCore.WriteLine(this.GetType().Name, "开始持久化", 1);


                    ServiceCore.WriteLine(this.GetType().Name, "结束持久化", 1);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    ServiceCore.WriteLine(this.GetType().Name, ex.ToString(), 1);
                }

                Thread.Sleep(1000 * 60);
            }
        });
    }

    /// <summary>
    /// 开始工作，2分鐘執行一次
    /// </summary>
    /// <param name="reason"></param>
    protected override void StartWork(string reason)
    {
        WriteLog("业务开始……");

        try
        {
            RunWork();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            ServiceCore.WriteLine(this.GetType().Name, ex.ToString(), 1);
        }
        
        base.StartWork(reason);
    }

    /// <summary>
    /// 停止服务
    /// </summary>
    /// <param name="reason"></param>
    protected override void StopWork(string reason)
    {
        WriteLog("服务结束！{0}", reason);
        base.StopWork(reason);
    }

}
