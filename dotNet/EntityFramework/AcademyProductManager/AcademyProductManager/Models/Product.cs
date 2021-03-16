using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyProductManager.Models
{
    [Table("Products")]
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string MadeInCountry { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [Column("Tags", TypeName = "jsonb")]
        public string[] Tags { get; set; }

        [Column("Seller", TypeName = "jsonb")]
        public Seller? Seller { get; set; }

        public Guid? CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
