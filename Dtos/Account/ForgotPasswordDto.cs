using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
/// <summary>
/// Dto sense user's email to be sent a reset link
/// </summary>
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }
    /// <summary>
    /// This information is sent when the user submits their new password
    /// </summary>
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

}