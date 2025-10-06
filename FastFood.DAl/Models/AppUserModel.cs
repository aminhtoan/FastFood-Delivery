using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.DAL.Models
{
    public class AppUserModel : IdentityUser
    {
        public string FullName { get; set; }        
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
