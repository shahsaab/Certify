using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Certify.Server.Models.CertifyApp
{
    [Table("User", Schema = "dbo")]
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int StoreId { get; set; }

        public Store Store { get; set; }

        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public User User1 { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public User User2 { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool Status { get; set; }

        public ICollection<Customer> Customers { get; set; }

        public ICollection<Customer> Customers1 { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Product> Products1 { get; set; }

        public ICollection<Store> Stores { get; set; }

        public ICollection<Store> Stores1 { get; set; }

        public ICollection<User> Users1 { get; set; }

        public ICollection<User> Users2 { get; set; }

    }
}