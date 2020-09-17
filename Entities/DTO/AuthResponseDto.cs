using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTO
{
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string Message { get; set; }
        public string Access_Token { get; set; }
    }
}
