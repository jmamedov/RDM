using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDM.Core.Entities
{
    [Table("HRCHY_TYP", Schema = "PRDCT_01")]
    public class HierarchyType
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

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
        public virtual ICollection<NodeBillOfMaterial> NodeBillOfMaterials { get; set; } = new List<NodeBillOfMaterial>();
    }
}