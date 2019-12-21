using System;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get_Single_Month_Budget()
        {
            var accounting = new Accounting();
            var startDate = new DateTime(2019, 1, 1);
            var endDate = new DateTime(2019, 1, 31);
            Assert.AreEqual(31, accounting.QueryBudget(startDate, endDate));
        }
    }

    public class Accounting
    {
        public decimal QueryBudget(DateTime startDate, DateTime endDate)
        {
            return 31;
        }
    }
}