﻿using ApiCurso.Model.Dto.Pelicula.Common;

namespace ApiCurso.Model.Dto.Pelicula
{
    public class PeliculaDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public string UrlImagen { get; set; }

        public TipoClasificacion Clasificacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int IdCategoria { get; set; }
    }
}
