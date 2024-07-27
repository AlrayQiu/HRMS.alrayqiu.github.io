using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using HRMS_Application.DataBase;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HRMS_Application.BackServer
{

    public class OnlineLogService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public OnlineLogService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            // 设置定时器，每隔1分钟执行一次
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                foreach (var mac in scope.ServiceProvider.GetRequiredService<AppDbContext>().GetMacs)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "arp";
                    process.StartInfo.Arguments = "-a";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    string[] lines = output.Split('\n');
                    foreach (string line in lines)
                    {
                        if (line.Contains(mac))
                        {
                            string[] parts = line.Split(' ');
                            string ipAddress = parts.FirstOrDefault(s => !string.IsNullOrEmpty(s));

                            using (Ping ping = new Ping())
                            {
                                PingReply reply = ping.Send(ipAddress);

                                if (reply.Status == IPStatus.Success)
                                {
                                    scope.ServiceProvider.GetRequiredService<AppDbContext>().AddTime(mac,1);
                                }
                            }
                            break;
                        }
                    }
                }
                scope.ServiceProvider.GetRequiredService<AppDbContext>().SaveChanges();
            }
            // 执行您的后台任务逻辑
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
