using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogging _logger;
        private readonly ApplicatioDbContext _db;
        public VillaAPIController(ApplicatioDbContext db)
        {
            // _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            //_logger.Log("Get All Villas", "information");
            return Ok(_db.Villas.ToList());
        }
        [HttpGet("{id:int}", Name = "GetVilla")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200, Type =typeof(VillaDto))]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                //_logger.Log("Get Villad Error with " + id, "error");
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<VillaDto>> CreateVilla([FromBody] VillaDto villaDTO)
        {
            if (villaDTO == null)
            {
                return BadRequest();
            }
            if (_db.Villas.FirstOrDefault(x => x.Name.Trim().ToLower() == villaDTO.Name.Trim().ToLower()) != null)
            {
                ModelState.AddModelError("CustomeError", "Villa already Exist!");
                return BadRequest(ModelState);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = new()
            {
                Name = villaDTO.Name,
                Amenity = villaDTO.Amenity,
                Detail = villaDTO.Detail,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Id = villaDTO.Id
            };
            _db.Villas.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("id:int", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var resp = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (resp == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(resp);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("id:int", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateVilla(int id, [FromBody] VillaDto villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            Villa model = new()
            {
                Name = villaDTO.Name,
                Amenity = villaDTO.Amenity,
                Detail = villaDTO.Detail,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();
        }


        [HttpPatch("id:int", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaDto villaDTO = new()
            {
                Name = villa.Name,
                Amenity = villa.Amenity,
                Detail = villa.Detail,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = new()
            {
                Name = villaDTO.Name,
                Amenity = villaDTO.Amenity,
                Detail = villaDTO.Detail,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
               
            };
            _db.Villas.Update(model);
            _db.SaveChanges();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}