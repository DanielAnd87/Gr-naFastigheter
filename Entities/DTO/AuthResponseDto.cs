using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTO
{
    public class AuthResponseDto
    {
        public int Status { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string Access_Token { get; set; }
    }
}
