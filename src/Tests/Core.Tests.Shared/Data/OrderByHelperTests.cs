#region copyright

// Copyright 2013-2016 Alphacloud.Net
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
// ReSharper disable HeapView.BoxingAllocation
namespace Core.Tests.Data
{
    using System;
    using System.Linq;
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class OrderByHelperTests
    {
        [Test]
        public void CanSortByMultiProperty()
        {
            var data = new[] {
                new Simple(2, "A"),
                new Simple(1, "B"),
                new Simple(3, "B")
            };

            data.OrderBy("Name DESC, Id ASC")
                .Should()
                .ContainInOrder(new Simple(1, "B"), new Simple(3, "B"), new Simple(2, "A"));
        }


        [Test]
        public void CanSortByNestedProperties()
        {
            var data = new[] {
                new Aggregate(new Simple(2)),
                new Aggregate(new Simple(1))
            };

            data.OrderBy("Nested.Id")
                .Should()
                .ContainInOrder(new Aggregate(new Simple(1)), new Aggregate(new Simple(2)));
        }


        [Test]
        public void CanSortBySingleProperty()
        {
            var data = new[] {
                new Simple(2),
                new Simple(1),
                new Simple(3)
            };

            var res = data.OrderBy("Id");
            res.Should().ContainInOrder(new Simple(1), new Simple(2), new Simple(3));
        }


        [Test]
        public void CanSortBySinglePropertyDescending()
        {
            var data = new[] {
                new Simple(2),
                new Simple(1),
                new Simple(3)
            };

            var res = data.OrderBy("Id DESC");
            res.Should().ContainInOrder(new Simple(3), new Simple(2), new Simple(1));
        }


        [Test]
        public void FailOnNullNestedProps()
        {
            var data = new[] {
                new Aggregate(new Simple(2)),
                new Aggregate(null)
            };
            Action order = () => data.OrderBy("Nested.Id").ToArray();
            order.ShouldThrow<NullReferenceException>();
        }

        #region Nested type: Aggregate

        class Aggregate : IEquatable<Aggregate>
        {
            public Aggregate(Simple nested)
            {
                Nested = nested;
            }


            public Simple Nested { get; }

            #region Equality members

            public bool Equals(Aggregate other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }
                if (ReferenceEquals(this, other))
                {
                    return true;
                }
                return Equals(Nested, other.Nested);
            }


            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }
                var other = obj as Aggregate;
                return other != null && Equals(other);
            }


            public override int GetHashCode()
            {
                return Nested != null ? Nested.GetHashCode() : 0;
            }

            #endregion
        }

        #endregion

        #region Nested type: Simple

        class Simple : IEquatable<Simple>
        {
            public Simple(int id)
            {
                Id = id;
                Name = string.Empty;
            }


            public Simple(int id, string name)
            {
                Id = id;
                Name = name;
            }


            public int Id { get; }

            public string Name { get; }


            public override string ToString()
            {
                return "{0}.{1}".ApplyArgs(Id, Name);
            }

            #region Equality members

            public bool Equals(Simple other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }
                if (ReferenceEquals(this, other))
                {
                    return true;
                }
                return Id == other.Id && Name == other.Name;
            }


            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }
                var other = obj as Simple;
                return other != null && Equals(other);
            }


            public override int GetHashCode()
            {
                return Id;
            }

            #endregion
        }

        #endregion
    }
}