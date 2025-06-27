# EasyTemplate.Schedule

#### 介绍
基于NewLife的windows service 服务模板

#### 软件架构
.Net8 + NewLifeAgent

#### 安装教程

1.  发布到本地文件夹
2.  运行EasyTemplate.Schedule.exe
3.  如果需要调试，就输入数字 5
4.  如果需要安装服务，就输入数字 1

#### 使用说明

1.  在Service文件夹下新增服务，增加后在构造函数中设置服务名称ServiceName，显示名称DisplayName和描述Description
2.  在RunWork方法中编写业务逻辑
3.  完成服务功能后，在Program.cs中注册服务
4.  在appsettings.json的Service.Name中切换到自己新增的服务
5.  运行调试窗口后，输入数字 5 开始调试

#### 其它

1.  范例 CacheToDBSyncService 是无定时长期执行
2.  范例 DailyStatisticsService 是定时执行