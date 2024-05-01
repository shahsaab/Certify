using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Certify.Server.Models.CertifyApp
{
    [Table("Role", Schema = "dbo")]
    public partial class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool Status { get; set; }

        public ICollection<RoleMenuMapping> RoleMenuMappings { get; set; }

        public ICollection<User> Users { get; set; }

    }
}