using LoanManagement.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.dao
{
    public interface ILoanManagement
    {
        void RegisterNewCustomer(Customer newCustomer);
        void ApplyLoan(Loan loan);
        decimal CalculateInterest(int loanId);
        decimal CalculateInterest(int loanId, decimal principalAmount, decimal interestRate, int loanTerm);
        void LoanStatus(int loanId);
        decimal CalculateEMI(int loanId);
        decimal CalculateEMI(int loanId, decimal principalAmount, decimal interestRate, int loanTerm);
        void LoanRepayment(int loanId, decimal amount);
        List<Loan> GetAllLoans();
        Loan GetLoanById(int loanId);
    }
}
