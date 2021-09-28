using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SB.SharedModels.Actions;
using SB.SharedModels.Booking;
using SB.WebShared.Interactors;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IDynamicsInteractor _dynamicsInteractor;

        public BookingController(IDynamicsInteractor dynamicsInteractor)
        {
            _dynamicsInteractor = dynamicsInteractor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                if (request.ApartmentId == Guid.Empty)
                {
                    return BadRequest($"{nameof(request.ApartmentId)} is empty guid");
                }

                if (request.Adults is > 5 or < 0)
                {
                    return BadRequest($"{nameof(request.Adults)} should be more than 0 and less than 5");
                }

                if (request.Children is > 5 or < 0)
                {
                    return BadRequest($"{nameof(request.Children)} should be more than 0 and less than 5");
                }

                if (request.Price is < 0)
                {
                    return BadRequest($"{nameof(request.Price)} should be more than 0");
                }

                if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    return BadRequest($"{nameof(request.PhoneNumber)} null or empty white-space");
                }

                if (string.IsNullOrWhiteSpace(request.ClientName))
                {
                    return BadRequest($"{nameof(request.ClientName)} null or empty white-space");
                }

                if (string.IsNullOrWhiteSpace(request.DateStart))
                {
                    return BadRequest($"{nameof(request.DateStart)} null or empty white-space");
                }

                if (string.IsNullOrWhiteSpace(request.DateEnd))
                {
                    return BadRequest($"{nameof(request.DateEnd)} null or empty white-space");
                }

                request.ClientName = request.ClientName.Trim();
                request.Description = request.Description.Trim();
                request.PhoneNumber = request.PhoneNumber.Trim();

                if (DateTime.TryParse(request.DateStart, out var dateStart))
                {
                    if (DateTime.TryParse(request.DateEnd, out var dateEnd))
                    {
                        if (dateStart.Date < DateTime.Today.Date)
                        {
                            return BadRequest($"{nameof(request.DateStart)} should be future");
                        }

                        if (dateStart.Date >= dateEnd.Date)
                        {
                            return BadRequest(
                                $"{nameof(request.DateStart)} should be less then {nameof(request.DateEnd)}");
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

                var response = await _dynamicsInteractor.SendAction(ActionNames.ActionTrackingNames.CreateBooking, request);

                if (response == null) return BadRequest("The request return null response");

                var createBookingResponse = JsonSerializer.Deserialize<CreateBookingResponse>(response);

                return Ok(createBookingResponse);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}