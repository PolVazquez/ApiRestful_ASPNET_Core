using ApiCurso.Model;
using ApiCurso.Model.Dto.Pelicula;
using ApiCurso.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCurso.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController(IPeliculaRepository peliculaRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPeliculaRepository _peliculasRepository = peliculaRepository;

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddPelicula([FromBody] AddPeliculaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest(ModelState);

            if (_peliculasRepository.ExistsPelicula(dto.Nombre))
            {
                ModelState.AddModelError("", $"La pelicula {dto.Nombre} ya existe");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(dto);
            if (!_peliculasRepository.CreatePelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {pelicula.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { id = pelicula.Id }, pelicula);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePelicula(int id)
        {
            if (!_peliculasRepository.ExistsPelicula(id))
            {
                return NotFound($"No se encontró una película con el ID {id}");
            }

            var pelicula = _peliculasRepository.GetPelicula(id);
            if (!_peliculasRepository.DeletePelicula(pelicula))
            {
                ModelState.AddModelError("", "Error al eliminar la película.");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int id)
        {
            var pelicula = _peliculasRepository.GetPelicula(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            var peliculaDto = _mapper.Map<PeliculaDto>(pelicula);
            return Ok(peliculaDto);
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _peliculasRepository.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDto);
        }

        [AllowAnonymous]
        [HttpGet("GetPeliculasByCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPeliculasByCategoria(int categoriaId)
        {
            var peliculas = _peliculasRepository.GetPeliculasByCategoria(categoriaId);
            if (peliculas == null || peliculas.Count == 0)
            {
                return NotFound($"No se encontraron películas para la categoría con ID {categoriaId}");
            }

            var peliculasDto = _mapper.Map<List<PeliculaDto>>(peliculas);
            return Ok(peliculasDto);
        }

        [AllowAnonymous]
        [HttpGet("SearchPelicula/{nombre}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchPelicula(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre de búsqueda no puede estar vacío.");
            }

            var peliculas = _peliculasRepository.SearchPelicula(nombre);
            var peliculasDto = _mapper.Map<List<PeliculaDto>>(peliculas);

            return Ok(peliculasDto);
        }

        [HttpPatch("{id:int}", Name = "UpdatePatchPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchPelicula(int id, [FromBody] PeliculaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null || dto.Id != id)
                return BadRequest(ModelState);

            var peliculaExistente = _peliculasRepository.GetPelicula(id);
            if (peliculaExistente == null)
                return NotFound($"No se encontró la película {dto.Id}");

            var pelicula = _mapper.Map<Pelicula>(dto);
            if (!_peliculasRepository.UpdatePelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePutPelicula(int id, [FromBody] PeliculaDto dto)
        {
            if (!ModelState.IsValid || dto == null || id != dto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_peliculasRepository.ExistsPelicula(id))
            {
                return NotFound($"No se encontró una película con el ID {id}");
            }

            var pelicula = _mapper.Map<Pelicula>(dto);
            if (!_peliculasRepository.UpdatePelicula(pelicula))
            {
                ModelState.AddModelError("", "Error al actualizar la película.");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok(pelicula);
        }
    }
}