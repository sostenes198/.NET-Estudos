using System.ComponentModel.DataAnnotations;

namespace Estudos.AspNetIdentityJwt.Api.Dto
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}