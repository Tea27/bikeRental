﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bikeRental.Core.Identity;
public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
    //public ApplicationRole(string roleName)
    //{
    //    this.Name = roleName;
    //    this.NormalizedName = roleName.ToUpper();
    //}
}
