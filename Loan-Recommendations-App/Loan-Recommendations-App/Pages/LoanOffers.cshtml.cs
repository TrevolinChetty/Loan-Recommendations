using Loan_Recommendations_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loan_Recommendations_App.Pages
{
    public class LoanOffers : PageModel
    {
        ApplicationDBContext _context;
        public LoanOffers(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<LoanDisplay> loanList{ get; set; }

        public class LoanDisplay
        {
            public int LoanId { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public decimal InterestRate { get; set; }
            public int MonthsToRepay { get; set; }
            public string CurrencySymbol { get; set; }
            public decimal Premium { get; set; }
        }

        public void OnGet(int CustomerId)
        {
            var customer = _context.Customer.Find(CustomerId);

            var Date = DateTime.Now;
            var age = Date.Year - customer.DateOfBirth.Year;
            if (customer.DateOfBirth.Date > Date.AddYears(-age))
                --age;

            decimal discount = (decimal)(15.00 / 100);

            loanList = (from loans in _context.Loan.ToList()
                         select new LoanDisplay()
                         {
                             Name = loans.Name,
                             Amount = loans.Amount,
                             InterestRate = loans.InterestRate,
                             MonthsToRepay = loans.MonthsToRepay,
                             //Note: All loans in this example are Interest-Only Loan Payments and the formula used is P = a(r/n) as seen on this site here https://www.thebalancemoney.com/loan-payment-calculations-315564
                             Premium = ((loans.Amount * (loans.InterestRate / 100)) / loans.MonthsToRepay) - (age < 30 ? (((loans.Amount * (loans.InterestRate / 100)) / loans.MonthsToRepay) * discount) : 0),
                             CurrencySymbol = loans.CurrencySymbol
                         }).ToList();
        }
    }
}