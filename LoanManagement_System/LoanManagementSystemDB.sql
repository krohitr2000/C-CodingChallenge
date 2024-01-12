IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LoanManagementSystemDB')
CREATE DATABASE LoanManagementSystemDB;

USE LoanManagementSystemDB;


-- Create Customer table
CREATE TABLE Customer (
    CustomerId INT PRIMARY KEY,
    Name VARCHAR(255),
    EmailAddress VARCHAR(255),
    PhoneNumber VARCHAR(20),
    Address VARCHAR(255),
    CreditScore Decimal
);
ALTER TABLE Customer
ADD CreditScore DECIMAL;
-- Create Loan table
CREATE TABLE Loan (
    LoanId INT PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Customer(CustomerId),
    PrincipalAmount DECIMAL(18, 2),
    InterestRate DECIMAL(5, 2),
    LoanTerm INT,
    LoanType VARCHAR(50),
    LoanStatus VARCHAR(50)
);


-- Create HomeLoan table (inherits from Loan)
CREATE TABLE HomeLoan (
    LoanId INT PRIMARY KEY,
    PropertyAddress VARCHAR(255),
    PropertyValue INT,
    FOREIGN KEY (LoanId) REFERENCES Loan(LoanId)
);

-- Create CarLoan table (inherits from Loan)
CREATE TABLE CarLoan (
    LoanId INT PRIMARY KEY,
    CarModel VARCHAR(255),
    CarValue INT,
    FOREIGN KEY (LoanId) REFERENCES Loan(LoanId)
);

-- Sample data for Customer table
INSERT INTO Customer (CustomerId, Name, EmailAddress, PhoneNumber, Address, CreditScore)
VALUES
(101, 'John Doe', 'john.doe@example.com', '1234567890', '123 Main St', 750),
(102, 'Alice Smith', 'alice.smith@example.com', '9876543210', '456 Oak Ave', 800),
(103, 'Bob Johnson', 'bob.johnson@example.com', '1112233445', '789 Pine Rd', 700),
(104, 'Emily Davis', 'emily.davis@example.com', '5556667777', '321 Elm Ln', 820),
(105, 'Michael Wilson', 'michael.wilson@example.com', '9998887777', '654 Birch Blvd', 680);

-- Sample data for Loan table
INSERT INTO Loan (LoanId, CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus)
VALUES
(1, 101, 2000.00, 12.5, 24, 'HomeLoan', 'Pending'),
(2, 102, 5000.00, 10.0, 36, 'CarLoan', 'Approved'),
(3, 103, 10000.00, 8.0, 48, 'PersonalLoan', 'Rejected'),
(4, 104, 1500.00, 15.0, 12, 'HomeLoan', 'Approved'),
(5, 105, 8000.00, 9.5, 24, 'CarLoan', 'Pending');

-- Sample data for HomeLoan table
INSERT INTO HomeLoan (LoanId, PropertyAddress, PropertyValue)
VALUES
(1, '123 Oak St', 250000),
(4, '456 Maple Ave', 180000);

-- Sample data for CarLoan table
INSERT INTO CarLoan (LoanId, CarModel, CarValue)
VALUES
(2, 'Toyota Camry', 30000),
(5, 'Honda Civic', 25000);


	SELECT * FROM Loan;
	select * from CarLoan
	SELECT * FROM Customer;