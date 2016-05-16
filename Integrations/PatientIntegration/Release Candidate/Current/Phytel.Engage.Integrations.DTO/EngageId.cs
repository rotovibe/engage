using System;
using System.Collections.Generic;
using System.Threading;

namespace Phytel.Engage.Integrations.DTO
{
    public static class EngageId
    {
        public static string New()
        {
            const string charList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Thread.Sleep(2); // throttle the thread for more randomness

            var t = DateTime.UtcNow;
            var charArray = charList.ToCharArray();
            var result = new Stack<char>();

            var length = charArray.Length;

            var dgit = 1000000000000L +
                       t.Millisecond * 1000000000L +
                       t.DayOfYear * 1000000L +
                       t.Hour * 10000L +
                       t.Minute * 100L +
                       t.Second;

            while (dgit != 0)
            {
                result.Push(charArray[dgit % length]);
                dgit /= length;
            }

            return new string(result.ToArray());
        }
    }
}
