using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDM.Core.Entities
{
    [Table("NDE_BIL_OF_MTRL", Schema = "PROD04")]
    public class NodeBillOfMaterial
    {
        [Column("HRCHY_TYP_ID")]
        [Required]
        public long HierarchyTypeId { get; set; }

        [Column("PRNT_NDE_ID")]
        [Required]
        public long ParentNodeId { get; set; }

        [Column("CHLD_NDE_ID")]
        [Required]
        public long ChildNodeId { get; set; }

        // Navigation properties
        [ForeignKey("HierarchyTypeId")]
        public virtual ProductEntityType HierarchyType { get; set; } = null!;

        [ForeignKey("ParentNodeId")]
        public virtual SourceNode ParentNode { get; set; } = null!;

        [ForeignKey("ChildNodeId")]
        public virtual SourceNode ChildNode { get; set; } = null!;
    }
}