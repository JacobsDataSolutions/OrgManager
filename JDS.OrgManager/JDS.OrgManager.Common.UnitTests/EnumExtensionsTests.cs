using JDS.OrgManager.Common.Diagnostics;
using JDS.OrgManager.Common.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

#pragma warning disable xUnit1026 // Theory methods should use all of their parameters

namespace JDS.OrgManager.Common.UnitTests
{
    public class EnumExtensionsTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { TestEnum.None, Array.Empty<TestEnum>() },
                new object[] { TestEnum.One, new TestEnum[] { TestEnum.One } },
                new object[] { TestEnum.One | TestEnum.Two, new TestEnum[] { TestEnum.One, TestEnum.Two } },
                new object[] { TestEnum.One | TestEnum.Two | TestEnum.Three | TestEnum.Four | TestEnum.Five | TestEnum.Six | TestEnum.Seven, new TestEnum[] { TestEnum.One, TestEnum.Two, TestEnum.Three, TestEnum.Four, TestEnum.Five, TestEnum.Six, TestEnum.Seven } },
                new object[] { TestEnum.Two, new TestEnum[] { TestEnum.Two } },
                new object[] { TestEnum.Three | TestEnum.Seven, new TestEnum[] { TestEnum.Three, TestEnum.Seven } },
                new object[] { TestEnum.Fourteen | TestEnum.Fifteen, new TestEnum[] { TestEnum.Fourteen, TestEnum.Fifteen } },
                new object[] { TestEnum.Six | TestEnum.Sixteen, new TestEnum[] { TestEnum.Six, TestEnum.Sixteen } }
            };

        public static IEnumerable<object[]> Data2 =>
            new List<object[]>
            {
                new object[] { TestEnum2.One, new TestEnum2[] { TestEnum2.One } },
                new object[] { TestEnum2.One | TestEnum2.Two, new TestEnum2[] { TestEnum2.One, TestEnum2.Two } },
            };

        public EnumExtensionsTests(ITestOutputHelper testOutputHelper) => this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

        [Theory]
        [MemberData(nameof(Data))]
        public void EnumerateFlagValues_WorksAsExpected(TestEnum testValue, TestEnum[] expectedResult)
        {
            var result = testValue.EnumerateFlagValues();
            var zip = result.Zip(expectedResult).ToArray();
            Assert.Equal(expectedResult.Length, result.Length);
            Assert.True(zip.All(x => x.First == x.Second));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EnumerateFlagValues2_WorksAsExpected(TestEnum testValue, TestEnum[] expectedResult)
        {
            var result = testValue.EnumerateFlagValues2();
            var zip = result.Zip(expectedResult).ToArray();
            Assert.Equal(expectedResult.Length, result.Length);
            Assert.True(zip.All(x => x.First == x.Second));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EnumerateFlagValues_TimeIt(TestEnum testValue, TestEnum[] _)
        {
            TestEnum[]? result = null;
            var (min, max, average) = MethodTimer.TimeIt(() => result = testValue.EnumerateFlagValues(), 1000);
            testOutputHelper.WriteLine($"Min: {min}, Max: {max}, Avg: {average}");
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EnumerateFlagValues2_TimeIt(TestEnum testValue, TestEnum[] _)
        {
            TestEnum[]? result = null;
            var (min, max, average) = MethodTimer.TimeIt(() => result = testValue.EnumerateFlagValues2(), 1000);
            testOutputHelper.WriteLine($"Min: {min}, Max: {max}, Avg: {average}");
        }

        // Running this test demonstrates that flags enums which do NOT have None = 0 defined will break.
        //[Theory]
        //[MemberData(nameof(Data2))]
        //public void EnumerateFlagValues2_WorksAsExpected(TestEnum2 testValue, TestEnum2[] expectedResult)
        //{
        //    var result = testValue.EnumerateFlagValues();
        //    var zip = result.Zip(expectedResult).ToArray();
        //    Assert.Equal(expectedResult.Length, result.Length);
        //    Assert.True(zip.All(x => x.First == x.Second));
        //}
    }

    [Flags]
    public enum TestEnum
    {
        None = 0,
        One = 0x01,
        Two = 0x02,
        Three = 0x04,
        Four = 0x08,
        Five = 0x10,
        Six = 0x20,
        Seven = 0x40,
        Eight = 0x80,
        Nine = 0x100,
        Ten = 0x200,
        Eleven = 0x400,
        Twelve = 0x800,
        Thirteen = 0x1000,
        Fourteen = 0x2000,
        Fifteen = 0x4000,
        Sixteen = 0x8000,
    }

    [Flags]
    public enum TestEnum2
    {
        One = 0x01,
        Two = 0x02,
        Three = 0x04,
        Four = 0x08,
        Five = 0x10,
        Six = 0x20,
        Seven = 0x40,
        Eight = 0x80,
        Nine = 0x100,
        Ten = 0x200,
        Eleven = 0x400,
        Twelve = 0x800,
        Thirteen = 0x1000,
        Fourteen = 0x2000,
        Fifteen = 0x4000,
        Sixteen = 0x8000,
    }
}
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
