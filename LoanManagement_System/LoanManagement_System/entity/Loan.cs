using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LoanManagement.entity
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int CustomerId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        public Loan() { }

        public Loan(int loanId, int customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus)
        {
            LoanId = loanId;
            CustomerId = customer;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }

        public override string ToString()
        {
            return $"Loan ID \t:\t{LoanId}\nCustomer ID \t:\t{CustomerId}\nPrincipalAmount :\t{PrincipalAmount}\nInterest Rate \t:\t{InterestRate}\nLoan Term \t:\t{LoanTerm}\nLoan Type \t:\t{LoanType}\nLaon Status \t:\t{LoanStatus}";
        }
    }
}
