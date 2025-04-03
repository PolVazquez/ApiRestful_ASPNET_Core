using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiCurso.Model.Dto.Usuario
{
    public class UsuarioDataDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}