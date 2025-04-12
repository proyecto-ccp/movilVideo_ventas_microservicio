using Microsoft.AspNetCore.Mvc;
using Videos.Aplicacion.Comandos;
using Videos.Aplicacion.Consultas;
using Videos.Aplicacion.Dto;

namespace ServicioVideo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class VideoController : ControllerBase
    {
        private readonly IComandosVideo _comandosVideo;
        private readonly IConsultasVideo _consultasVideo;
        public VideoController(IComandosVideo comandosVideo, IConsultasVideo consultasVideo)
        {
            _comandosVideo = comandosVideo;
            _consultasVideo = consultasVideo;
        }

        [HttpPost]
        [Route("CargarVideo")]
        [ProducesResponseType(typeof(VideoOut), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 401)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> CargarVideo([FromBody] VideoIn videoIn)
        {
            try
            {
                var resultado = await _comandosVideo.CargarVideo(videoIn);

                if (resultado.Resultado != Videos.Aplicacion.Enum.Resultado.Error)
                    return Ok(resultado);
                else
                    return Problem(resultado.Mensaje, statusCode: (int)resultado.Status, title: resultado.Resultado.ToString(), type: resultado.Resultado.ToString(), instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerVideo/{id}")]
        [ProducesResponseType(typeof(VideoOut), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 401)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> ObtenerVideo(Guid id)
        {
            try
            {
                var resultado = await _consultasVideo.ObtenerVideoPorId(id);
                if (resultado.Resultado != Videos.Aplicacion.Enum.Resultado.Error)
                    return Ok(resultado);
                else
                    return Problem(resultado.Mensaje, statusCode: (int)resultado.Status, title: resultado.Resultado.ToString(), type: resultado.Resultado.ToString(), instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerVideos")]
        [ProducesResponseType(typeof(VideoListOut), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 401)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> ObtenerVideos()
        {
            try
            {
                var resultado = await _consultasVideo.ObtenerVideos();
                if (resultado.Resultado != Videos.Aplicacion.Enum.Resultado.Error)
                    return Ok(resultado);
                else
                    return Problem(resultado.Mensaje, statusCode: (int)resultado.Status, title: resultado.Resultado.ToString(), type: resultado.Resultado.ToString(), instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerVideosPorCliente/{idCliente}")]
        [ProducesResponseType(typeof(VideoListOut), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 401)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> ObtenerVideosPorCliente(Guid idCliente)
        {
            try
            {
                var resultado = await _consultasVideo.ObtenerVideosPorCliente(idCliente);
                if (resultado.Resultado != Videos.Aplicacion.Enum.Resultado.Error)
                    return Ok(resultado);
                else
                    return Problem(resultado.Mensaje, statusCode: (int)resultado.Status, title: resultado.Resultado.ToString(), type: resultado.Resultado.ToString(), instance: HttpContext.Request.Path);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
