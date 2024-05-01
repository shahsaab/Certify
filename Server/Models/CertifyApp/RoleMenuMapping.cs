using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Certify.Server.Models.CertifyApp
{
    [Table("RoleMenuMapping", Schema = "dbo")]
    public partial class RoleMenuMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; }

        [Required]
        public int MenuId { get; set; }

        public Menu Menu { get; set; }

    }
}