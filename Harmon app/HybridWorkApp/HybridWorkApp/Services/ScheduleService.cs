using System;
using System.Collections.Generic;
using System.Linq;
using HybridWorkApp.Models;

namespace HybridWorkApp.Services
{
    public class ScheduleService
    {
        private List<ScheduleItem> _items = new();

        public ScheduleService(List<ScheduleItem>? items = null)
        {
            if (items != null) _items = items;
        }

        public void Update(List<ScheduleItem> items)
        {
            _items = items ?? new List<ScheduleItem>();
        }

        // Simple business logic: higher remote days increases "balance" but too many reduces collaboration score.
        public double CalculateBalanceIndex()
        {
            if (_items == null || !_items.Any()) return 50.0;
            var total = _items.Count;
            var remote = _items.Count(i => i.Type == ScheduleType.Remote);
            var pres = _items.Count(i => i.Type == ScheduleType.Presential);
            var hybrid = _items.Count(i => i.Type == ScheduleType.Hybrid);

            // base score from remote ratio
            double remoteRatio = (double)remote / total;
            double hybridRatio = (double)hybrid / total;
            double presRatio = (double)pres / total;

            // prefer balance: best when remoteRatio around 0.4 and presRatio around 0.4
            double balanceScore = 100 - (Math.Abs(remoteRatio - 0.4) * 100 + Math.Abs(presRatio - 0.4) * 100) * 0.5;

            // collaboration penalty if remote too high
            if (remoteRatio > 0.7) balanceScore -= (remoteRatio - 0.7) * 100 * 0.6;

            // hybrid bonus
            balanceScore += hybridRatio * 10;

            // clamp
            if (balanceScore < 0) balanceScore = 0;
            if (balanceScore > 100) balanceScore = 100;
            return balanceScore;
        }
    }
}
