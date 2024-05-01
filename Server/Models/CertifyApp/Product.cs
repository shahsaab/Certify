using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Certify.Server.Models.CertifyApp
{
    [Table("Product", Schema = "dbo")]
    public partial class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ReceiptNo { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Pieces { get; set; }

        public double? TestingPrice { get; set; }

        public string Weight { get; set; }

        public string Karat { get; set; }

        public string StandardFineness { get; set; }

        public string AdditionalInfo { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public int? CertificateCharges { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public User User { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public User User1 { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Required]
        public string Status { get; set; }

    }
}