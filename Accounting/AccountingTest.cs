using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AccountingTest
{
    public class Tests
    {
        private List<Budget> _dummyBudget;
        private Accounting _accounting;

        private void GivenDummyBudget()
        {
            var janBudget = new Budget() { YearMonth = "201901", Amount = 31 };
            var febBudget = new Budget() { YearMonth = "201902", Amount = 280 };
            var marBudget = new Budget() { YearMonth = "201903", Amount = 3100 };

            _dummyBudget = new List<Budget> { janBudget, febBudget, marBudget };
        }

        private void CreateAccounting()
        {
            var budgetRepository = new BudgetRepository(_dummyBudget);
            _accounting = new Accounting(budgetRepository);
        }

        [SetUp]
        public void Setup()
        {
            GivenDummyBudget();
        }

        [Test]
        public void Get_Single_Month_Budget()
        {
            CreateAccounting();
            var startDate = new DateTime(2019, 1, 1);
            var endDate = new DateTime(2019, 1, 31);

            Assert.AreEqual(31, _accounting.QueryBudget(startDate, endDate));
        }

        [Test]
        public void Get_Two_Month_Budget()
        {
            CreateAccounting();
            var startDate = new DateTime(2019, 1, 1);
            var endDate = new DateTime(2019, 2, 28);
            Assert.AreEqual(311, _accounting.QueryBudget(startDate, endDate));
        }

        [Test]
        public void Get_Partial_Month_Budget()
        {
            CreateAccounting();
            var startDate = new DateTime(2019, 1, 15);
            var endDate = new DateTime(2019, 1, 30);
            Assert.AreEqual(16, _accounting.QueryBudget(startDate, endDate));
        }
    }
}