using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Domain
{
    public class Sale
    {
        [Key]
        public Guid SaleId { get; set; } // Primary Key
        public string SaleTitle {  get; set; }
        public DateOnly SaleDate { get; set; } // Date of the sale
        public string SaleTotalAmount { get; set; } // Total amount of the sale
        public int? Emp_Id { get; set; } // Foreign Key to Employee
        public Employee Employee { get; set; } // Navigation Property
        public int? Cl_Id { get; set; } // Foreign Key to Client
        public Client Client { get; set; } // Navigation Property
        public int Year { get; set; } // Year the project started or was completed
    }
}
