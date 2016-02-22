#region copyright

// Copyright 2013 Alphacloud.Net
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#endregion

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Core.Tests.Data
{
    using System.Linq;
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    internal class ChartHelperTests
    {
        [Test]
        public void GroupItemsUnderThreshold_Should_MergeItemsToNewGroup()
        {
            // total = 100
            var data = new[]
            {
                new ChartData(45),
                new ChartData(45),
                new ChartData(5),
                new ChartData(5)
            };

            ChartData[] res = ChartHelper.GroupItemsUnderThreshold(data, 7, value => new ChartData(value)).ToArray();
            res.Should().HaveCount(3);
            res[2].Percentage.Should().Be(10);
        }

        [Test]
        public void UpdatePercentage_Should_HandleZeroTotal()
        {
            var data = new[]
            {
                new ChartData(0),
                new ChartData(0)
            };

            ChartHelper.UpdatePercentage(data, d => d.Value, (item, percentage) => item.Percentage = percentage);

            data[0].Percentage.Should().Be(0.0);
            data[1].Percentage.Should().Be(0.0);
        }

        [Test]
        public void UpdatePercentage_Should_UpdatePercentage()
        {
            var data = new[]
            {
                new ChartData(1),
                new ChartData(2)
            };

            ChartHelper.UpdatePercentage(data, d => d.Value, (item, percentage) => item.Percentage = percentage);

            data[0].Percentage.Should().BeApproximately(33.333, 0.001);
            data[1].Percentage.Should().BeApproximately(66.666, 0.001);
        }

        private class ChartData : IChartData
        {
            public ChartData(double value)
            {
                Value = value;
            }

            public double Value { get; private set; }
            public double Percentage { get; set; }
        }
    }
}