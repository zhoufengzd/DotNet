using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.Simulator
{
    public sealed class RandomDate : IRandom<DateTime>
    {
        public RandomDate()
        {
        }

        public DateTime Next()
        {
            return new DateTime();
        }

        private Random _random = new Random();
    }

}
