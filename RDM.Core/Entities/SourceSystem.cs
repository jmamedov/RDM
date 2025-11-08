using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDM.Core.Entities
{
    [Table("SRC_SYS", Schema = "PRDCT_01")]
    public class SourceSystem
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("PRNT_ID")]
        public long? ParentId { get; set; }

        [Column("CD")]
        [MaxLength(64)]
        public string? Code { get; set; }

        [Column("NM")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Column("DN")]
        [MaxLength(1024)]
        public string? Description { get; set; }

        [Column("SRC_PTH_TXT")]
        [MaxLength(2048)]
        public string? SourcePathText { get; set; }

        // Navigation properties
        [ForeignKey("ParentId")]
        public virtual SourceSystem? Parent { get; set; }

        public virtual ICollection<SourceSystem> Children { get; set; } = new List<SourceSystem>();
        public virtual ICollection<SourceNode> SourceNodes { get; set; } = new List<SourceNode>();
    }
}