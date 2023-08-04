using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Entities.Models
{

#region Entity
    public partial class UserDetail : Entity
    {
public UserDetail()
{}

public int Id { get; set; }
public string? Account { get; set; }
public string? Name { get; set; }
public string? Password { get; set; }
    }
#endregion

#region DTO
    public partial class UserDetailDTO
    {
public int Id { get; set; }
public string? Account { get; set; }
public string? Name { get; set; }
public string? Password { get; set; }
    }
#endregion
}
