using ApiCurso.Data;
using ApiCurso.Model;
using ApiCurso.Model.Dto.Usuario;
using ApiCurso.Repository.IRepository;
using ApiCurso.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCurso.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretApi;
        public UsuarioRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretApi = configuration.GetValue<string>("ApiSettings:secret");
        }

        public Usuario GetUsuario(int id)
            => _db.Usuarios.FirstOrDefault(u => u.Id == id);

        public ICollection<Usuario> GetUsuarios()
            => _db.Usuarios.OrderBy(u => u.NombreUsuario).ToList();

        public bool IsUniqueUser(string usuario)
            => _db.Usuarios.All(u => u.NombreUsuario != usuario);

        public async Task<UsuarioLoginResponseDto> Login(UsuarioLoginDto dto)
        {
            // Buscar el usuario en la base de datos por nombre de usuario
            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario.ToLower() == dto.NombreUsuario.ToLower());

            // Verificar si el usuario existe
            if (usuario == null)
            {
                return new UsuarioLoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                }; // Usuario no encontrado
            }

            // Comparar la contraseña ingresada con la almacenada
            bool isPasswordValid = PasswordHasher.VerifyPassword(dto.Password, usuario.Password);

            if (!isPasswordValid)
            {
                return null; // Contraseña incorrecta
            }

            // Si todo está bien, devolver los datos del usuario autenticado
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role),
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretApi)), SecurityAlgorithms.HmacSha256Signature)
            };

            // Crear el token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Devolver el token y los datos del usuario
            return new UsuarioLoginResponseDto()
            {
                Token = tokenString,
                Usuario = new UsuarioDataDto()
                {
                    Id = usuario.Id,
                    UserName = usuario.NombreUsuario,
                    Name = usuario.Nombre,
                    Role = usuario.Role
                }
            };
        }


        public async Task<Usuario> Register(UsuarioRegisterDto dto)
        {
            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Nombre = dto.Nombre,
                Password = PasswordHasher.HashPassword(dto.Password),
                Role = dto.Role
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            return usuario;
        }
    }
}