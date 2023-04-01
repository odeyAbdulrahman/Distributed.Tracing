using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distributed.Tracing.Services
{
    public interface ICounter
    {
        Task<int> SetCounter();
        Task<int> GetCounter();
    }
    public class CounterService : ICounter
    {
        private readonly IDistributedCache Cache;

        public CounterService(IDistributedCache cache)
        {
            Cache = cache;
        }

        public async Task<int> SetCounter()
        {
            string key = "counter";
            await Cache.SetAsync(key, BitConverter.GetBytes(1));
            return 1;
        }
        public async Task<int> GetCounter()
        {
            string key = "counter";
            byte[] value = await Cache.GetAsync(key);

            if (value == null)
            {
                value = BitConverter.GetBytes(1);
                await Cache.SetAsync(key, value);
                return 1;

            }
            else
            {
                int count = BitConverter.ToInt32(value);
                count++;
                value = BitConverter.GetBytes(count);
                await Cache.SetAsync(key, value);
                return count;
            }
        }
    }
}
