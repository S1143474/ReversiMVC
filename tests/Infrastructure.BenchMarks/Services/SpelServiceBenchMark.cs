using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Services;

namespace Infrastructure.BenchMarks.Services
{
    public class SpelServiceBenchMark
    {
        private readonly SpelService _spelService;

        public SpelServiceBenchMark()
        {
            _spelService = new SpelService();
        }
    }
}
