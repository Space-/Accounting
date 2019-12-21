using System;
using System.Linq;

namespace AccountingTest
{
    public class Accounting
    {
        private readonly BudgetRepository _budgetRepository;

        public Accounting(BudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public decimal QueryBudget(DateTime startDate, DateTime endDate)
        {
            if (IsSameYearAndMonth(startDate, endDate))
            {
                return GetTotalBudgetInThisPeriod(startDate, endDate);
            }

            var sum = 0;
            for (DateTime currentDateTime = startDate; currentDateTime.Month <= endDate.Month; currentDateTime = currentDateTime.AddMonths(1))
            {
                sum += IsFullMonth(endDate, currentDateTime)
                    ? GetTotalBudgetInThisPeriod(new DateTime(currentDateTime.Year, currentDateTime.Month, 1), currentDateTime.AddMonths(1).AddDays(-1))
                    : GetTotalBudgetInThisPeriod(startDate, endDate);
            }

            return sum;
        }

        private int GetTotalBudgetInThisPeriod(DateTime startDate, DateTime endDate)
        {
            return GetDiffDays(startDate, endDate) * BudgetPerDayOfThisMonth(endDate);
        }

        private int GetDiffDays(DateTime startDate, DateTime endDate)
        {
            return endDate.Date.Day - startDate.Date.Day + 1;
        }

        private bool IsFullMonth(DateTime endDate, DateTime currentDateTime)
        {
            return endDate.Month > currentDateTime.Month;
        }

        private int BudgetPerDayOfThisMonth(DateTime currentDateTime)
        {
            var currentYearMonth = string.Concat(currentDateTime.Year, currentDateTime.Month.ToString().PadLeft(2, '0')); // 201901
            var currentMonthTotalBudget = _budgetRepository.GetAll().First(x => x.YearMonth == currentYearMonth);

            var budgetPerDayOfThisMonth = currentMonthTotalBudget.Amount / GetTotalDaysOfThisMonth(currentDateTime);
            return budgetPerDayOfThisMonth;
        }

        private int GetTotalDaysOfThisMonth(DateTime currentDateTime)
        {
            return DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);
        }

        private bool IsSameYearAndMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.Year == endDate.Year && startDate.Month == endDate.Month;
        }
    }
}