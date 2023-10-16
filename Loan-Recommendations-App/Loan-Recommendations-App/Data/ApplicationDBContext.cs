using Loan_Recommendations_App.Pages;
using Microsoft.EntityFrameworkCore;

namespace Loan_Recommendations_App.Data
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; } 
        public string PhoneNumber { get; set; } 
        public string AlternatePhoneNumber { get; set; } 
        public string Email { get; set; } 
        public string Address { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public bool IsSouthAfricanCitizen { get; set; }
        public int Gender { get; set; }
        public decimal Income { get; set; }
        public string JobTitle { get; set; }
        public string Employer { get; set; }
    }

    public class Loan
    {
        public int LoanId { get; set; }
        public string Name { get; set; } 
        public decimal Amount { get; set; } 
        public decimal InterestRate { get; set; } 
        public int MonthsToRepay { get; set; }
        public string CurrencySymbol { get; set; }
    }

    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options){ }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Loan> Loan { get; set; }
    }
}
