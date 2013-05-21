namespace Alphacloud.Common.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    ///     Handles percentage calculations.
    /// </summary>
    [PublicAPI]
    public static class ChartHelper
    {
        public static double UpdatePercentage<TEntity>(ICollection<TEntity> data, Func<TEntity, double> valueSelector,
            Action<TEntity, double> updatePercentage)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (valueSelector == null) throw new ArgumentNullException("valueSelector");
            if (updatePercentage == null) throw new ArgumentNullException("updatePercentage");

            var total = data.Sum(valueSelector);
            if (total == 0.0)
            {
                foreach (var entity in data)
                {
                    updatePercentage(entity, 0.0);
                }
            }
            else
            {
                foreach (var entity in data)
                {
                    updatePercentage(entity, valueSelector(entity) / total * 100);
                }
            }

            return total;
        }

        /// <summary>
        ///     Claculate and update percentage for each item.
        /// </summary>
        /// <typeparam name="T">Chart item type.</typeparam>
        /// <param name="source">Source values.</param>
        /// <returns>Total value for source items.</returns>
        public static double UpdatePercentage<T>(ICollection<T> source)
            where T : IChartData
        {
            return UpdatePercentage(source, item => item.Value,
                (item, percentage) => item.Percentage = percentage);
        }

        /// <summary>
        ///     Calculate percentage and group items below threshold into new item.
        /// </summary>
        /// <typeparam name="T">Chart item type.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="threshold">Percentage threshold.</param>
        /// <param name="getGroupedItemsContainer">Create container for data below threshold callback.</param>
        /// <returns>Grouped items.</returns>
        public static IList<T> GroupItemsUnderThreshold<T>(IList<T> source, double threshold,
            Func<double, T> getGroupedItemsContainer)
            where T : IChartData
        {
            if (source == null) throw new ArgumentNullException("source");
            if (getGroupedItemsContainer == null) throw new ArgumentNullException("getGroupedItemsContainer");
            if (threshold <= 0)
                throw new ArgumentException("Percentage threshold should be positive value.", "threshold");

            UpdatePercentage(source);

            // if nothing to merge, just return existing collection
            if (!source.Any(item => item.Percentage < threshold))
                return source;

            var mergedList = new List<T>(source.Where(item => item.Percentage >= threshold));
            double percentageAboveThreshold = mergedList.Sum(item => item.Percentage);


            var groupedTotal = source.Where(item => item.Percentage < threshold).Sum(t => t.Value);
            // merge items below threshold
            var groupedItem = getGroupedItemsContainer(groupedTotal);
            groupedItem.Percentage = 100.0 - percentageAboveThreshold;

            mergedList.Add(groupedItem);
            return mergedList;
        }
    }
}