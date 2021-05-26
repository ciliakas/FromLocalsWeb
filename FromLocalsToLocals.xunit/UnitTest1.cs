using FromLocalsToLocals.Web.Utilities;
using System;
using Xunit;
using Xunit.Abstractions;

namespace FromLocalsToLocals.xunit
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;
        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test1()
        {
            string now = DateTime.UtcNow.ToString();
            string rez = TimeCalculator.CalcRelativeTime(now);
            //output.WriteLine(now);
            //output.WriteLine(rez);
            Assert.True(rez.Equals("0 seconds ago"));
        }
    }
}
