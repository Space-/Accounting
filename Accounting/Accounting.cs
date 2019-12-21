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
                var diffDays = GetDiffDays(startDate, endDate);
                return diffDays * BudgetPerDayOfThisMonth(endDate);
            }
            else
            {
                var sum = 0;
                for (DateTime currentDateTime = startDate; currentDateTime.Month <= endDate.Month; currentDateTime = currentDateTime.AddMonths(1))
                {
                    if (IsFullMonth(endDate, currentDateTime))
                    {
                        var diffDays = GetTotalDaysOfThisMonth(new DateTime(currentDateTime.Year, currentDateTime.Month, 1)) * BudgetPerDayOfThisMonth(currentDateTime);
                        sum += diffDays * BudgetPerDayOfThisMonth(currentDateTime);
                    }
                    else
                    {
                        var diffDays = GetDiffDays(startDate, endDate);
                        sum += diffDays * BudgetPerDayOfThisMonth(currentDateTime);
                    }
                }

                return sum;
            }
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
            var currentMonthTotalBudget = _budgetRepository.GetAll().First(x => x.YearMonth == currentYearMonth); // $31
            var totalDaysOfThisMonth = GetTotalDaysOfThisMonth(currentDateTime);

            var budgetPerDayOfThisMonth = currentMonthTotalBudget.Amount / totalDaysOfThisMonth;
            return budgetPerDayOfThisMonth;
        }

        private int GetTotalDaysOfThisMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).Day;
        }

        private bool IsSameYearAndMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.Year == endDate.Year && startDate.Month == endDate.Month;
        }
    }
}