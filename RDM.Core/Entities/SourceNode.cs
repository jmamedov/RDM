using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDM.Core.Entities
{


    [Table("SRC_NDE", Schema = "PRDCT_01")]
public class SourceNode
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("SRC_ID")]
    [Required]
    public long SourceId { get; set; }

    [Column("ENT_TYP_ID")]
    [Required]
    public long EntityTypeId { get; set; }

    [Column("LCLE_CD")]
    [Required]
    [MaxLength(64)]
    public string LocaleCode { get; set; } = "en-US";

    [Column("CD")]
    [Required]
    [MaxLength(64)]
    public string Code { get; set; } = string.Empty;

    [Column("NM")]
    [Required]
    [MaxLength(1024)]
    public string Name { get; set; } = string.Empty;

    [Column("DN")]
    [MaxLength(2024)]
    public string? Description { get; set; }

    // Navigation properties
    [ForeignKey("SourceId")]
    public virtual SourceSystem SourceSystem { get; set; } = null!;

    [ForeignKey("EntityTypeId")]
    public virtual ProductEntityType EntityType { get; set; } = null!;

    [ForeignKey("LocaleCode")]
    public virtual RegionalLocale Locale { get; set; } = null!;

    public virtual ICollection<NodeBillOfMaterial> ParentRelationships { get; set; } = new List<NodeBillOfMaterial>();
    public virtual ICollection<NodeBillOfMaterial> ChildRelationships { get; set; } = new List<NodeBillOfMaterial>();
}
}

