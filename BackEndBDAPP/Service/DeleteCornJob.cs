using BackEndBDAPP.Models;
using BackEndBDAPP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEndBDAPP.Service
{
    public class DeleteCornJob : DeletePostService
    {

        public DeleteCornJob(IScheduleConfig<DeleteCornJob> config)
           : base(config.CronExpression, config.TimeZoneInfo)
        {

        }
        public override Task DoWork(CancellationToken cancellationToken)
        {
            AutoDeletePosts.delete();
            return base.DoWork(cancellationToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
