using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Entities.Models
{

#region Entity
    public partial class UserDetail : Entity
    {
public UserDetail()
{
    CartInfos = new HashSet<CartInfo>
    ();
     }

public int Id { get; set; }
public string? Account { get; set; }
public string? Email { get; set; }
public string? Name { get; set; }
public string? Password { get; set; }
public string? UpdateBy { get; set; }
public DateTime? UpdateDate { get; set; }
public string? CreateBy { get; set; }
public DateTime? CreateDate { get; set; }

public virtual ICollection<CartInfo>
    CartInfos { get; set; } = default!;
    }
#endregion

#region DTO
    public partial class UserDetailDTO
    {
public int Id { get; set; }
public string? Account { get; set; }
public string? Email { get; set; }
public string? Name { get; set; }
public string? Password { get; set; }
public string? UpdateBy { get; set; }
public DateTime? UpdateDate { get; set; }
public string? CreateBy { get; set; }
public DateTime? CreateDate { get; set; }
    }
#endregion
}
