namespace Estudos.AspNetIdentityJwt.Api.Dto
{
    public class UpdateUserRoleDto
    {
        public string Email { get; set; }
        public bool Delete { get; set; }
        public string Role { get; set; }
    }
}