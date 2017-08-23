using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string SchoolName { get; set; }
        
        [MaxLength(50), Required, Index(IsUnique = true)]
        public string UserName { get; set; }

        public string SubDomain { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}
