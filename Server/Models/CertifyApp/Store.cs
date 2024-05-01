using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Certify.Server.Models.CertifyApp
{
    [Table("Store", Schema = "dbo")]
    public partial class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Address { get; set; }

        [Required]
        public string Contact1 { get; set; }

        public string Contact2 { get; set; }

        public string Email { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public User User { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public User User1 { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool Status { get; set; }

        public ICollection<User> Users { get; set; }

    }
}