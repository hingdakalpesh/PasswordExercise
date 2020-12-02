using PasswordExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordExercise
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(PasswordRequirements requirements);
    }
}
