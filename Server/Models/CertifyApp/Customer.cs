using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Certify.Server.Models.CertifyApp
{
    [Table("Customer", Schema = "dbo")]
    public partial class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Contact { get; set; }

        public string NIC { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public User User { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public User User1 { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}