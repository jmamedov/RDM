using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDM.Core.Entities
{
    [Table("NDE_BIL_OF_MTRL", Schema = "PRDCT_01")]
    public class NodeBillOfMaterial
    {
        [Column("HRCHY_TYP_ID")]
        [Required]
        public int HierarchyTypeId { get; set; }

        [Column("PRNT_NDE_ID")]
        [Required]
        public long ParentNodeId { get; set; }

        [Column("CHLD_NDE_ID")]
        [Required]
        public long ChildNodeId { get; set; }

        // Navigation properties
        [ForeignKey("HierarchyTypeId")]
        public virtual HierarchyType HierarchyType { get; set; } = null!;

        [ForeignKey("ParentNodeId")]
        public virtual SourceNode ParentNode { get; set; } = null!;

        [ForeignKey("ChildNodeId")]
        public virtual SourceNode ChildNode { get; set; } = null!;
    }
}