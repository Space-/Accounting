using System.Collections.Generic;

namespace AccountingTest
{
    public interface IBudgetRepository
    {
        List<Budget> GetAll();
    }
}