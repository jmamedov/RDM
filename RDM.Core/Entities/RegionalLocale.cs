using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDM.Core.Entities
{
    [Table("RGNL_LCLE", Schema = "SHARED")]
    public class RegionalLocale
    {
        [Key]
        [Column("CD")]
        [MaxLength(64)]
        public string Code { get; set; } = string.Empty;

        [Column("NM")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("CONT_CD")]
        [Required]
        [MaxLength(64)]
        public string ContinentCode { get; set; } = string.Empty;

        [Column("DN")]
        [MaxLength(512)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<SourceNode> SourceNodes { get; set; } = new List<SourceNode>();
    }
}
