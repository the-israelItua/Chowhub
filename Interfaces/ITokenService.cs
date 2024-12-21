using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Models;

namespace ChowHub.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser applicationUser);
    }
}