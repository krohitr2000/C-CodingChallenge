using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.dao;
using LoanManagement.entity;
using LoanManagement.exception;
using LoanManagement.util;


namespace LoanManagement.dao
{
    public class LoanManagementImpl : ILoanManagement
    {
        public void RegisterNewCustomer(Customer newCustomer)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Customer (CustomerId, Name, EmailAddress, PhoneNumber, Address, CreditScore) " +
                                         "VALUES (@CustomerId, @Name, @EmailAddress, @PhoneNumber, @Address, @CreditScore); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", newCustomer.CustomerId);
                        command.Parameters.AddWithValue("@Name", newCustomer.Name);
                        command.Parameters.AddWithValue("@EmailAddress", newCustomer.EmailAddress);
                        command.Parameters.AddWithValue("@PhoneNumber", newCustomer.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", newCustomer.Address);
                        command.Parameters.AddWithValue("@CreditScore", newCustomer.CreditScore);

                        int count = command.ExecuteNonQuery();

                        if (count > 0)
                        {
                            Console.WriteLine($"\nNew customer registered successfully. Customer ID: {newCustomer.CustomerId}");
                        }
                        else
                        {
                            throw new Exception("Error registering new customer");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering new customer: {ex.Message}");
            }
        }
        public void ApplyLoan(Loan loan)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string propertyAddress = "", carModel = "";
                    int propertyValue = 0, carValue = 0;

                    if (loan.LoanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Write("Enter Property Address: ");
                        propertyAddress = Console.ReadLine();
                        Console.Write("Enter Property Value: ");
                        propertyValue = int.Parse(Console.ReadLine());
                    }
                    else
                    {
                        Console.Write("Enter Car Model: ");
                        carModel = Console.ReadLine();
                        Console.Write("Enter Car Value: ");
                        carValue = int.Parse(Console.ReadLine());
                    }

                    loan.LoanStatus = "Pending";

                    Console.WriteLine("Do you want to apply for this loan? (Yes/No)");
                    string userInput = Console.ReadLine();

                    if (userInput.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                    {
                        string insertQuery = "INSERT INTO Loan (LoanId, CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) " +
                                             "VALUES (@LoanId, @CustomerId, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus)";

                        int status = 0;
                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@LoanId", loan.LoanId);
                            command.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
                            command.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                            command.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                            command.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                            command.Parameters.AddWithValue("@LoanType", loan.LoanType);
                            command.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);

                            status = command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }

                        if (status > 0)
                        {
                            int loanId;
                            insertQuery = "SELECT LoanId FROM Loan where CustomerId=@CustomerId AND PrincipalAmount=@PrincipalAmount";
                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
                                command.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                                loanId = Convert.ToInt32(command.ExecuteScalar());
                            }

                            if (loan.LoanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                            {
                                insertQuery = "INSERT INTO HomeLoan VALUES (@LoanId, @propertyAddress, @propertyValue)";
                                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@LoanId", loanId);
                                    command.Parameters.AddWithValue("@propertyAddress", propertyAddress);
                                    command.Parameters.AddWithValue("@propertyValue", propertyValue);
                                    int n = command.ExecuteNonQuery();
                                    if (n > 0)
                                    {
                                        Console.WriteLine("Loan application submitted successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Loan application failed.");
                                    }
                                }
                            }
                            else
                            {
                                insertQuery = "INSERT INTO CarLoan VALUES (@LoanId, @carModel, @carValue)";
                                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@LoanId", loanId);
                                    command.Parameters.AddWithValue("@carModel", carModel);
                                    command.Parameters.AddWithValue("@carValue", carValue);
                                    int n = command.ExecuteNonQuery();
                                    if (n > 0)
                                    {
                                        Console.WriteLine("Loan application submitted successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Loan application failed.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Loan application canceled by the user.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying for loan: {ex.Message}");
            }
        }

        public decimal CalculateEMI(int loanId)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string selectQuery = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LoanId", loanId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal principalAmount = Convert.ToDecimal(reader["PrincipalAmount"]);
                                decimal interestRate = Convert.ToDecimal(reader["InterestRate"]);
                                int loanTerm = Convert.ToInt32(reader["LoanTerm"]);

                                return CalculateEMI(loanId, principalAmount, interestRate, loanTerm);
                            }
                            else
                            {
                                throw new InvalidLoanException("Loan not found.");
                            }
                        }
                    }
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
        }

        public decimal CalculateEMI(int loanId, decimal principalAmount, decimal interestRate, int loanTerm)
        {
            decimal r = interestRate / 12 / 100;
            decimal n = loanTerm;
            return (principalAmount * r * (decimal)Math.Pow((double)(1 + r), (double)n)) / (decimal)(Math.Pow((double)(1 + r), (double)n) - 1);
        }

        public decimal CalculateInterest(int loanId)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();
                    string selectQuery = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LoanId", loanId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal principalAmount = Convert.ToDecimal(reader["PrincipalAmount"]);
                                decimal interestRate = Convert.ToDecimal(reader["InterestRate"]);
                                int loanTerm = Convert.ToInt32(reader["LoanTerm"]);

                                return CalculateInterest(loanId, principalAmount, interestRate, loanTerm);
                            }
                            else
                            {
                                throw new InvalidLoanException("Loan not found.");
                            }
                        }
                    }
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
                return -1;
            }
        }

        public decimal CalculateInterest(int loanId, decimal principalAmount, decimal interestRate, int loanTerm)
        {
            return (principalAmount * interestRate * loanTerm) / 12;
        }


        public List<Loan> GetAllLoans()
        {
            List<Loan> allLoans = new List<Loan>();

            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Loan";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Loan loan = new Loan
                                    {
                                        LoanId = Convert.ToInt32(reader["LoanId"]),
                                        PrincipalAmount = Convert.ToDecimal(reader["PrincipalAmount"]),
                                        InterestRate = Convert.ToDecimal(reader["InterestRate"]),
                                        LoanTerm = Convert.ToInt32(reader["LoanTerm"]),
                                        LoanType = reader["LoanType"].ToString(),
                                        LoanStatus = reader["LoanStatus"].ToString(),
                                        CustomerId = Convert.ToInt32(reader["CustomerId"])
                                    };

                                    allLoans.Add(loan);
                                }
                                return allLoans;
                            }
                            else
                            {
                                throw new InvalidLoanException("No Loans Found");
                            }
                        }
                    }
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error retrieving all loans: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all loans: {ex.Message}");
                return null;
            }
        }

        public Loan GetLoanById(int loanId)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Loan WHERE LoanId = @LoanId";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LoanId", loanId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Loan loan = new Loan
                                {
                                    LoanId = Convert.ToInt32(reader["LoanId"]),
                                    PrincipalAmount = Convert.ToDecimal(reader["PrincipalAmount"]),
                                    InterestRate = Convert.ToDecimal(reader["InterestRate"]),
                                    LoanTerm = Convert.ToInt32(reader["LoanTerm"]),
                                    LoanType = reader["LoanType"].ToString(),
                                    LoanStatus = reader["LoanStatus"].ToString(),
                                    CustomerId = Convert.ToInt32(reader["CustomerId"])
                                };

                                return loan;
                            }
                            else
                            {
                                throw new InvalidLoanException("Loan not found.");
                            }
                        }
                    }
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error retrieving loan by ID: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving loan by ID: {ex.Message}");
                return null;
            }
        }

        public void LoanRepayment(int loanId, decimal amount)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string selectQuery = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LoanId", loanId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal principalAmount = Convert.ToDecimal(reader["PrincipalAmount"]);
                                decimal interestRate = Convert.ToDecimal(reader["InterestRate"]);
                                int loanTerm = Convert.ToInt32(reader["LoanTerm"]);

                                decimal emi = CalculateEMI(loanId, principalAmount, interestRate, loanTerm);

                                int noOfEmiToPay = (int)(amount / emi);

                                if (noOfEmiToPay == 0 || amount < emi)
                                {
                                    Console.WriteLine("Payment rejected. Insufficient amount for at least one EMI.");
                                }
                                else
                                {
                                    Console.WriteLine($"Paid {noOfEmiToPay} EMIs.\n Remaining amount: {Math.Floor(amount % emi)}");
                                    UpdateLoanRepayment(loanId, noOfEmiToPay, emi);
                                    Console.WriteLine("\n Loan repayment variable updated in the database.");
                                }
                            }
                            else
                            {
                                throw new InvalidLoanException("Loan not found.");
                            }
                        }
                    }
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error processing loan repayment: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing loan repayment: {ex.Message}");
            }
        }

        private void UpdateLoanRepayment(int loanId, int noOfEmiToPay, decimal emiAmount)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string selectQuery = "SELECT PrincipalAmount FROM Loan WHERE LoanId = @LoanId";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@LoanId", loanId);

                        decimal currentPrincipalAmount = Convert.ToDecimal(selectCommand.ExecuteScalar());

                        decimal newPrincipalAmount = currentPrincipalAmount - Math.Floor((noOfEmiToPay * emiAmount));

                        string updateQuery = "UPDATE Loan SET PrincipalAmount = @NewPrincipalAmount WHERE LoanId = @LoanId";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@LoanId", loanId);
                            updateCommand.Parameters.AddWithValue("@NewPrincipalAmount", newPrincipalAmount);

                            updateCommand.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Loan repay is updated. \nNew Principal Amount: {newPrincipalAmount}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidLoanException($"Error updating loan repayment variable: {ex.Message}");
            }
        }


        public void LoanStatus(int loanId)
        {
            try
            {
                using (SqlConnection connection = DBUtil.GetDBConn())
                {
                    connection.Open();

                    string selectQuery = "SELECT l.LoanId, c.CreditScore FROM Loan l " +
                                         "INNER JOIN Customer c ON l.CustomerId = c.CustomerId " +
                                         "WHERE l.LoanId = @LoanId";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LoanId", loanId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int creditScore = Convert.ToInt32(reader["CreditScore"]);

                                string updateQuery = "UPDATE Loan SET LoanStatus = @LoanStatus WHERE LoanId = @LoanId";
                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@LoanId", loanId);

                                    if (creditScore > 650)
                                    {
                                        updateCommand.Parameters.AddWithValue("@LoanStatus", "Approved");
                                        Console.WriteLine("Loan approved!");
                                    }
                                    else
                                    {
                                        updateCommand.Parameters.AddWithValue("@LoanStatus", "Rejected");
                                        Console.WriteLine("Loan rejected due to low credit score.");
                                    }
                                    reader.Close();
                                    updateCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                throw new InvalidLoanException("Loan not found.");
                            }
                        }
                    }
                }
            }
            catch (InvalidLoanException ex)
            {
                Console.WriteLine($"Error updating loan status: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidLoanException($"Error updating loan status: {ex.Message}");
            }


        }
    }
}
