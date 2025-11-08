using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDM.Core.Entities
{
    [Table("ENT_TYP", Schema = "PRDCT_01")]
    public class ProductEntityType
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("CD")]
        [Required]
        [MaxLength(64)]
        public string Code { get; set; } = string.Empty;

        [Column("NM")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("DN")]
        [MaxLength(1024)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<SourceNode> SourceNodes { get; set; } = new List<SourceNode>();
    }
}