using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Certify.Server.Models.CertifyApp
{
    [Table("Menu", Schema = "dbo")]
    public partial class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<RoleMenuMapping> RoleMenuMappings { get; set; }

    }
}