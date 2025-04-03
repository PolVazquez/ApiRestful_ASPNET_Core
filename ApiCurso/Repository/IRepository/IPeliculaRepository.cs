using ApiCurso.Model;

namespace ApiCurso.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        ICollection<Pelicula> GetPeliculas();
        ICollection<Pelicula> GetPeliculasByCategoria(int categoriaId);
        IEnumerable<Pelicula> SearchPelicula(string nombre);
        Pelicula GetPelicula(int id);
        bool ExistsPelicula(int id);
        bool ExistsPelicula(string nombre);
        bool CreatePelicula(Pelicula pelicula);
        bool UpdatePelicula(Pelicula pelicula);
        bool DeletePelicula(Pelicula pelicula);
        bool Save();
    }
}