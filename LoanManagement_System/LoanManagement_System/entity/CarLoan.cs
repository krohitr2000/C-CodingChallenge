using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.entity
{
    public class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public int CarValue { get; set; }

        public CarLoan() { }

        public CarLoan(int loanId, int customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus, string carModel, int carValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus)
        {
            CarModel = carModel;
            CarValue = carValue;
        }
    }
}
