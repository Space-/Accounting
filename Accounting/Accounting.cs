using System;
using System.ComponentModel;
using System.Linq;

namespace Tests
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
                // 1/15 ~ 1/31
                var diffDays = endDate.Date.Day - startDate.Date.Day + 1; // 16 days
                return diffDays * BudgetPerDayOfThisMonth(endDate); // 16 * ($31 / 31day)
            }
            else
            {
                var sum = 0;
                for (DateTime currentDateTime = startDate; currentDateTime.Month <= endDate.Month; currentDateTime.AddMonths(1))
                {
                    if (IsFullMonth(endDate, currentDateTime))
                    {
                        sum += GetTotalDaysOfThisMonth(new DateTime(currentDateTime.Year, currentDateTime.Month, 1)) * BudgetPerDayOfThisMonth(currentDateTime);
                    }
                    else
                    {
                        var diffDays = endDate.Date.Day - startDate.Date.Day + 1; // 16 days
                        sum += diffDays * BudgetPerDayOfThisMonth(currentDateTime); // 16 * ($31 / 31day)
                    }
                }

                return sum;
            }
        }

        private static bool IsFullMonth(DateTime endDate, DateTime currentDateTime)
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

        private static int GetTotalDaysOfThisMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).Day;
        }

        private static bool IsSameYearAndMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.Year == endDate.Year && startDate.Month == endDate.Month;
        }
    }
}