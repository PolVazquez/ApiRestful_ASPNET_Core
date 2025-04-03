﻿using ApiCurso.Data;
using ApiCurso.Model;
using ApiCurso.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiCurso.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly ApplicationDbContext _db;

        public PeliculaRepository(ApplicationDbContext db)
            => _db = db;

        public bool CreatePelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.UtcNow;
            _db.Peliculas.Add(pelicula);

            return Save();
        }

        public bool DeletePelicula(Pelicula pelicula)
        {
            _db.Remove(_db.Peliculas.Find(pelicula.Id));

            return Save();
        }

        public bool ExistsPelicula(int id)
            => _db.Peliculas.Any(p => p.Id == id);

        public bool ExistsPelicula(string nombre)
            => _db.Peliculas.Any(p => p.Nombre.ToLower().Trim() == nombre.ToLower().Trim());

        public Pelicula GetPelicula(int id)
            => _db.Peliculas.FirstOrDefault(p => p.Id == id);

        public ICollection<Pelicula> GetPeliculas()
            => _db.Peliculas.OrderBy(p => p.Nombre).ToList();

        public ICollection<Pelicula> GetPeliculasByCategoria(int categoriaId)
            => _db.Peliculas.Include(p => p.Categoria).Where(p => p.IdCategoria == categoriaId).ToList();

        public bool Save()
            => _db.SaveChanges() >= 0;

        public IEnumerable<Pelicula> SearchPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _db.Peliculas;
            if (!string.IsNullOrWhiteSpace(nombre))
                query = _db.Peliculas.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre));

            return query.ToList();
        }

        public bool UpdatePelicula(Pelicula pelicula)
        {
            pelicula.FechaActualizacion = DateTime.UtcNow;
            _db.Peliculas.Update(pelicula);

            return Save();
        }
    }
}