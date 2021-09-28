using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SB.SharedModels.Actions;
using SB.SharedModels.Apartments;
using SB.WebShared.Interactors;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly IDynamicsInteractor _dynamicsInteractor;

        public ApartmentController(IDynamicsInteractor dynamicsInteractor)
        {
            _dynamicsInteractor = dynamicsInteractor;
        }

        [HttpGet]
        public async Task<IActionResult> GetApartments([FromQuery] GetAvailableApartmentsRequest request)
        {
            try
            {
                GetAvailableApartmentsResponse apartmentsResponse;
                string response;

                if (string.IsNullOrWhiteSpace(request.DateStart) && string.IsNullOrWhiteSpace(request.DateEnd))
                {
                    response = await _dynamicsInteractor.SendAction(ActionNames.ActionTrackingNames.GetAllApartments, request);

                    if (response == null) return BadRequest("The request return null response");

                    apartmentsResponse = JsonSerializer.Deserialize<GetAvailableApartmentsResponse>(response);

                    return Ok(apartmentsResponse);
                }

                if (string.IsNullOrWhiteSpace(request.DateStart))
                {
                    return BadRequest($"{nameof(request.DateStart)} null or empty white-space");
                }
                if (string.IsNullOrWhiteSpace(request.DateEnd))
                {
                    return BadRequest($"{nameof(request.DateEnd)} null or empty white-space");
                }
                if (DateTime.TryParse(request.DateStart, out var dateStart))
                {
                    if (DateTime.TryParse(request.DateEnd, out var dateEnd))
                    {
                        if (dateStart.Date < DateTime.Today.Date)
                        {
                            return BadRequest($"{nameof(request.DateStart)} should be future");
                        }
                        if (dateStart.Date > dateEnd.Date)
                        {
                            return BadRequest($"{nameof(request.DateStart)} should be less then {nameof(request.DateEnd)}");
                        }
                    }
                    else
                    {
                        return BadRequest($"{nameof(request.DateEnd)} not parse to DateTime");
                    }
                }
                else
                {
                    return BadRequest($"{nameof(request.DateStart)} not parse to DateTime");
                }

                response = await _dynamicsInteractor.SendAction(ActionNames.ActionTrackingNames.GetAvailableApartments, request);

                if (response == null) return BadRequest("The request return null response");

                apartmentsResponse = JsonSerializer.Deserialize<GetAvailableApartmentsResponse>(response);

                return Ok(apartmentsResponse);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("price")]
        public async Task<IActionResult> GetPriceIfApartmentTypeAvailable([FromQuery] GetPriceIfApartmentTypeAvailableRequest request)
        {
            try
            {
                if (request.ApartmentTypeId == Guid.Empty)
                {
                    return BadRequest($"{nameof(request.ApartmentTypeId)} is empty guid");
                }
                if (string.IsNullOrWhiteSpace(request.DateStart))
                {
                    return BadRequest($"{nameof(request.DateStart)} null or empty white-space");
                }
                if (string.IsNullOrWhiteSpace(request.DateEnd))
                {
                    return BadRequest($"{nameof(request.DateEnd)} null or empty white-space");
                }
                if (DateTime.TryParse(request.DateStart, out var dateStart))
                {
                    if (DateTime.TryParse(request.DateEnd, out var dateEnd))
                    {
                        if (dateStart.Date < DateTime.Today.Date)
                        {
                            return BadRequest($"{nameof(request.DateStart)} should be future");
                        }
                        if (dateStart.Date > dateEnd.Date)
                        {
                            return BadRequest($"{nameof(request.DateStart)} should be less then {nameof(request.DateEnd)}");
                        }
                    }
                    else
                    {
                        return BadRequest($"{nameof(request.DateEnd)} not parse to DateTime");
                    }
                }
                else
                {
                    return BadRequest($"{nameof(request.DateStart)} not parse to DateTime");
                }

                var response = await _dynamicsInteractor.SendAction(ActionNames.ActionTrackingNames.GetPriceIfApartmentTypeAvailable, request);

                if (!string.IsNullOrWhiteSpace(response) && int.TryParse(response, out var price))
                {
                    return Ok(price);
                }

                return Ok(null);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartment(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest($"{nameof(id)} is empty Guid");
                }

                var response = await _dynamicsInteractor.SendAction(ActionNames.ActionTrackingNames.GetApartment, id.ToString());

                if (response == null) return NotFound();

                var apartmentsResponse = JsonSerializer.Deserialize<Apartment>(response);

                return Ok(apartmentsResponse);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}