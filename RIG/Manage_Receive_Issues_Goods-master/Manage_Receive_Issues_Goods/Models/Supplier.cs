using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("supplier")]
public partial class Supplier
{
    [Key]
    [Column("SupplierID")]
    public int SupplierId { get; set; }

    [StringLength(100)]
    public string SupplierName { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<Schedulereceived> Schedulereceiveds { get; set; } = new List<Schedulereceived>();
}
