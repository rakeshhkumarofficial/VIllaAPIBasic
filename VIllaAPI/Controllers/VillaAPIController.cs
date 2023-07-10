using log4net.Config;
using log4net.Core;
using log4net;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using VIllaAPI.Models.Dtos;
using VIllaAPI.CustomLogging;

namespace VIllaAPI.Controllers
{
    [Route("/api/Villa")]
    [ApiController]
    public class VillaAPIController : Controller
    {
        // default logger
        //private readonly ILogger<VillaAPIController> _logger;

        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        // custom logging
        //private readonly ILogging _logger;

        //public VillaAPIController(ILogging logger)
        //{
        //    _logger = logger;
        //}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            // default logger
            // _logger.LogInformation("Getting all the Villas");

            // custom logging
            //_logger.Log("Getting all the Villas", "");

            return Ok(VillaStore.VillaList); // 200
        }

        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200, Type=typeof(VillaDto)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if(id == 0)
            {
                // default
                // _logger.LogError("Get Villa Error With Id " + id);

                // custom
                //_logger.Log("Villa Not Exist ", "error");
                return BadRequest(); //400
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists");
                return BadRequest(ModelState);
            }
            if(villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDto.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDto);
            // return Ok(villaDto);
            // when we are working with put we generally return created at route
            // it produces at status code of 201
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto); // 201
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            //  example of log4net
            //try
            //{
            //    int x = div(50);
            //}
            //catch(Exception ex)
            //{
            //    LogError(ex.Message);
            //    throw;
            //}
            if (id == 0)
            {
                return BadRequest(); // 400
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound(); // 404
            }
            VillaStore.VillaList.Remove(villa);
            return NoContent(); // 204
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id , [FromBody]VillaDto villaDto)
        {
            if(villaDto== null || villaDto.Id != id)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if(villa== null)
            {
                return NotFound();
            }
            villa.Name = villaDto.Name;
            villa.Sqft = villaDto.Sqft;
            villa.Occupancy = villaDto.Occupancy;
            return NoContent();

        }

        //[HttpPatch("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult UpdatePartialResult(int id , JsonPatchDocument<VillaDto> patchDto)
        //{
        //    if(patchDto == null || id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
        //    if(villa== null)
        //    {
        //        return NotFound();
        //    }
        //    patchDto.ApplyTo(villa,ModelState);
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return NoContent();
        //}

        // log4Net
        //private void LogError(string message)
        //{
        //    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        //    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        //    ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
        //    _logger.Info(message);
        //}
        //private int div(int x)
        //{
        //    return x / 0;
        //}
    }
}
