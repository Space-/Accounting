using System.Collections.Generic;

namespace AccountingTest
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly List<Budget> _budgets;

        public BudgetRepository(List<Budget> budgets)
        {
            _budgets = budgets;
        }

        public List<Budget> GetAll()
        {
            return _budgets;
        }
    }
}