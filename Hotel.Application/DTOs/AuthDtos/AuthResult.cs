using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.AuthDtos
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public Guid? UserId { get; set; }
        public List<string>? Errors { get; set; }

        public static AuthResult Success(Guid userId)
            => new AuthResult { Succeeded = true, UserId = userId };

        public static AuthResult Fail(IEnumerable<string> errors)
            => new AuthResult { Succeeded = false, Errors = errors.ToList() };
    }
}
