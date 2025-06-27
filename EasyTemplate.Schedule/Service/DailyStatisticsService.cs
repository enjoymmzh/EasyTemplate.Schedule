namespace EasyTemplate.Schedule;

public class DailyStatisticsService : ServiceBase
{
    /// <summary>
    /// 定时器
    /// </summary>
    private TimerX _timer1 { get; set; }

    public DailyStatisticsService()
    {
        Description = "每日数据统计服务";
    }

    private void RunWork(object state)
    {
        try
        {
            ServiceCore.WriteLine(this.GetType().Name, "开始统计", 1);


            ServiceCore.WriteLine(this.GetType().Name, "结束统计", 1);
        }
        catch (Exception ex)
        {
            ServiceCore.WriteLine(this.GetType().Name, ex.ToString(), 1);
        }
    }

    /// <summary>
    /// 开始工作，2分鐘執行一次
    /// </summary>
    /// <param name="reason"></param>
    protected override void StartWork(string reason)
    {
        WriteLog("业务开始……");

        //参数1：委托方法，参数2：传递的参数，参数3：开始时间，参数4：间隔时间
        _timer1 = new TimerX(RunWork, null, DateTime.Now.AddSeconds(2), 1000 * 15) { Async = false };
        
        base.StartWork(reason);
    }

    /// <summary>
    /// 停止服务
    /// </summary>
    /// <param name="reason"></param>
    protected override void StopWork(string reason)
    {
        WriteLog("服务结束！{0}", reason);
        _timer1.Dispose();
        base.StopWork(reason);
    }

}
