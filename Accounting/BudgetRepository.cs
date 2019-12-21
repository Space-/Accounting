using System.Collections.Generic;

namespace AccountingTest
{
    public class BudgetRepository
    {
        private List<Budget> _budgets;

        public BudgetRepository(List<Budget> budgets)
        {
            _budgets = budgets;
        }

        public List<Budget> GetAll()
        {
            return this._budgets;
        }
    }
}