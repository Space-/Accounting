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
            var totalBudget = 0;
            for (DateTime currentDateTime = startDate; IsEndDateEqualOrGreaterThanCurrentDate(endDate, currentDateTime); currentDateTime = currentDateTime.AddMonths(1))
            {
                totalBudget += IsFullMonth(endDate, currentDateTime)
                    ? GetTotalBudgetInThisPeriod(new DateTime(currentDateTime.Year, currentDateTime.Month, 1), currentDateTime.AddMonths(1).AddDays(-1))
                    : GetTotalBudgetInThisPeriod(currentDateTime, endDate);
            }

            return totalBudget;
        }

        private static bool IsEndDateEqualOrGreaterThanCurrentDate(DateTime endDate, DateTime currentDateTime)
        {
            return DateTime.Compare(endDate, currentDateTime) >= 0;
        }

        private int GetTotalBudgetInThisPeriod(DateTime startDate, DateTime endDate)
        {
            DateTime lastDate = IsSameYearAndMonth(startDate, endDate) ? endDate : GetLastDateOfThisMonth(startDate);
            //            var lastDate = GetLastDateOfThisMonth(endDate);

            if (startDate.Year == endDate.Year)
            {
                if (startDate.Month == endDate.Month)
                {
                    return GetDiffDays(startDate, lastDate) * BudgetPerDayOfThisMonth(endDate)
                        + 0 * BudgetPerDayOfThisMonth(endDate);
                }
                else
                {
                    return GetDiffDays(startDate, lastDate) * BudgetPerDayOfThisMonth(startDate)
                           + endDate.Day * BudgetPerDayOfThisMonth(endDate);
                }
            }
            else
            {
                return GetDiffDays(startDate, lastDate) * BudgetPerDayOfThisMonth(startDate)
                       + endDate.Day * BudgetPerDayOfThisMonth(endDate);
            }
        }

        private static DateTime GetLastDateOfThisMonth(DateTime startDate)
        {
            return new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
        }

        private int GetDiffDays(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }

        private bool IsFullMonth(DateTime endDate, DateTime currentDateTime)
        {
            return endDate.Month > currentDateTime.Month;
        }

        private int BudgetPerDayOfThisMonth(DateTime currentDateTime)
        {
            var currentYearMonth = currentDateTime.ToString("yyyyMM"); // 201901
            var currentMonthTotalBudget = _budgetRepository.GetAll().FirstOrDefault(x => x.YearMonth == currentYearMonth);
            if (currentMonthTotalBudget != null)
            {
                var budgetPerDayOfThisMonth = currentMonthTotalBudget.Amount / GetTotalDaysOfThisMonth(currentDateTime);

                return budgetPerDayOfThisMonth;
            }

            return 0;
        }

        private static bool DataNotExistInRepository(Budget currentMonthTotalBudget)
        {
            return currentMonthTotalBudget == null;
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