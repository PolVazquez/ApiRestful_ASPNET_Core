using ApiCurso.Model;
using ApiCurso.Model.Dto.Usuario;

namespace ApiCurso.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int id);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginResponseDto> Login(UsuarioLoginDto dto);
        Task<Usuario> Register(UsuarioRegisterDto dto);
    }
}