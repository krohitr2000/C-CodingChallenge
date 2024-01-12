using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.dao;
using LoanManagement.entity;


namespace LoanManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILoanManagement loanmanagement = new LoanManagementImpl();

            while (true)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Welcome to loan management system");
                Console.WriteLine("------------------------------------");
                Console.WriteLine();
                Console.WriteLine("*****  MENU *****");
                Console.WriteLine();
                Console.WriteLine("1.Register");
                Console.WriteLine("2. Apply for Loan");
                Console.WriteLine("3. Calculate interest");
                Console.WriteLine("4. Get Loan Status");
                Console.WriteLine("5. Calculate EMI");
                Console.WriteLine("6. Pay EMI");
                Console.WriteLine("7. Get All Loan Details");
                Console.WriteLine("8. Get laon details by Id");
                Console.WriteLine("9. Exit");
                
                int userChoice = int.Parse(Console.ReadLine());
                switch (userChoice)
                {
                    case 1:
                        try
                        {
                            Console.Write("Enter customer ID: ");
                            int CustomerId = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter customer name: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter email address: ");
                            string emailAddress = Console.ReadLine();

                            Console.Write("Enter phone number: ");
                            string phoneNumber = Console.ReadLine();

                            Console.Write("Enter address: ");
                            string address = Console.ReadLine();

                            Console.Write("Enter credit score: ");
                            decimal creditScore = Convert.ToDecimal(Console.ReadLine());

                            Customer newCustomer = new Customer
                            {
                                CustomerId = CustomerId,
                                Name = name,
                                EmailAddress = emailAddress,
                                PhoneNumber = phoneNumber,
                                Address = address,
                                CreditScore = creditScore
                            };

                            loanmanagement.RegisterNewCustomer(newCustomer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 2:
                        try
                        {
                            Console.WriteLine("Already an existing customer? Yes/No");
                            Console.Write("Answer: ");
                            string ans = Console.ReadLine();

                            if (ans.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                            {
                                Loan loan = new Loan();

                                Console.WriteLine("\nEnter loan details:");

                                Console.Write("Customer ID: ");
                                int customerId = int.Parse(Console.ReadLine());

                                Console.Write("Principal Amount: ");
                                decimal principalAmount = decimal.Parse(Console.ReadLine());

                                Console.Write("Interest Rate: ");
                                decimal interestRate = decimal.Parse(Console.ReadLine());

                                Console.Write("Loan Term (months): ");
                                int loanTerm = int.Parse(Console.ReadLine());

                                Console.Write("Loan Loan ID: ");
                                int loanId = int.Parse(Console.ReadLine());

                                Console.Write("Loan Type (CarLoan/HomeLoan): ");
                                string loanType = Console.ReadLine();

                                loan.CustomerId = customerId;
                                loan.PrincipalAmount = principalAmount;
                                loan.InterestRate = interestRate;
                                loan.LoanTerm = loanTerm;
                                loan.LoanId = loanId;
                                loan.LoanType = loanType;

                                loanmanagement.ApplyLoan(loan);
                            }
                            else
                            {
                                Console.WriteLine("\nPlease register first, then apply for loan. Thank You!");
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 3:
                        try
                        {
                            Console.WriteLine("Enter Loan Id to calculate interest");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            decimal interest = loanmanagement.CalculateInterest(loanId);
                            Console.WriteLine("Interest amount is {0}", interest);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 4:
                        try
                        {
                            Console.WriteLine("Enter LoanId to check loan status");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            loanmanagement.LoanStatus(loanId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 5:
                        try
                        {
                            Console.WriteLine("Enter LoanId to calculate EMI");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            decimal emi = loanmanagement.CalculateEMI(loanId);
                            Console.WriteLine("EMI per month is {0}", emi);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 6:
                        try
                        {
                            Console.WriteLine("Enter LoanId to pay EMI");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter amount u want to pay");
                            decimal amount = Convert.ToDecimal(Console.ReadLine());
                            loanmanagement.LoanRepayment(loanId, amount);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 7:
                        try
                        {
                            List<Loan> loans = new List<Loan>();
                            loans = loanmanagement.GetAllLoans();
                            if (loans != null && loans.Count > 0)
                            {
                                foreach (Loan loan in loans)
                                {
                                    Console.WriteLine(loan);
                                    Console.WriteLine();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 8:
                        try
                        {
                            Console.WriteLine("Enter LoanId to get loan details");
                            int loanId = Convert.ToInt32(Console.ReadLine());
                            Loan loan = loanmanagement.GetLoanById(loanId);
                            if (loan != null)
                            {
                                Console.WriteLine(loan);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 9:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Enter valid option");
                        break;
                }
            }
        }
    }
}
