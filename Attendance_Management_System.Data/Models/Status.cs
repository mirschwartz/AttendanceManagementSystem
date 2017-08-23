using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public enum Status
    {
        Present = 1,
        UnexcusedAbsence,
        ExcusedAbsence,
        ExcusedLateness,
        UnexcusedLateness
    }
}
