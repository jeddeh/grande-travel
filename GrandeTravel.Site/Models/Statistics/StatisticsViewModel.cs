using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Models.Statistics
{
    public class StatisticsViewModel
    {
        public Dictionary<AustralianStateEnum, int> RegisteredUsersByState { get; set; }

        public Dictionary<AustralianStateEnum, decimal> AveragePackagePriceByState { get; set; }

        public Dictionary<int, decimal> RevenueByYear { get; set; }
    }
}