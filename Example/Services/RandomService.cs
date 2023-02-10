using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Services
{
    public class RandomService : IRandomService
    {
        private readonly Random _random;

        public RandomService()
        {
            _random = new Random();
        }

        public int Random(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
