using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyProductManager.Models
{
    [Table("Categories")]
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}