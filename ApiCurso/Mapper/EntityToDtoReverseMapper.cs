using ApiCurso.Model;
using ApiCurso.Model.Dto.Categoria;
using ApiCurso.Model.Dto.Pelicula;
using ApiCurso.Model.Dto.Usuario;
using AutoMapper;

namespace ApiCurso.Mapper
{
    public class EntityToDtoReverseMapper : Profile
    {
        public EntityToDtoReverseMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, AddCategoriaDto>().ReverseMap();

            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<Pelicula, AddPeliculaDto>().ReverseMap();

            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioRegisterDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDataDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginResponseDto>().ReverseMap();

            CreateMap<AppUsuario, UsuarioDataDto>().ReverseMap();
        }
    }
}