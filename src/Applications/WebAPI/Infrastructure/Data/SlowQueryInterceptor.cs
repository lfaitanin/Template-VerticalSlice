using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace WebAPI.Infrastructure.Data
{
    public class SlowQueryInterceptor : DbCommandInterceptor
    {
        private const int _slowQueryThreshold = 300; //MS
        public override DbDataReader ReaderExecuted(DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result)
        {
            if(eventData.Duration.TotalMilliseconds > _slowQueryThreshold)
            {
                Console.WriteLine($"Slow query ({eventData.Duration.TotalMilliseconds} ms): {command.CommandText}");
            }

            return base.ReaderExecuted(command, eventData, result);
        }
    }
}
