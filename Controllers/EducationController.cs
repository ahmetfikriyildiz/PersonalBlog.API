    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PersonalBlog.API.DTOs.Commons;
    using PersonalBlog.API.DTOs.Education;
    using PersonalBlog.API.Services.Interfaces;

    namespace PersonalBlog.API.Controllers
    {
        /// <summary>
        /// Controller for managing education records
        /// </summary>
        [ApiController]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public class EducationController : ControllerBase
        {
            private readonly IEducationService _educationService;

            public EducationController(IEducationService educationService)
            {
                _educationService = educationService;
            }

            /// <summary>
            /// Get all education records
            /// </summary>
            [HttpGet]
            [ProducesResponseType(typeof(IEnumerable<EducationResponseDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<IEnumerable<EducationResponseDto>>> GetAllEducations()
            {
                var educations = await _educationService.GetAllEducationsAsync();
                return Ok(educations);
            }

            /// <summary>
            /// Get all education records paged
            /// </summary>
            [HttpGet("paged")]
            [ProducesResponseType(typeof(PagedResponse<EducationResponseDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<PagedResponse<EducationResponseDto>>> GetAllEducationsPaged([FromQuery] PaginationFilter filter)
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var response = await _educationService.GetAllEducationsPagedAsync(validFilter);
                return Ok(response);
            }

            /// <summary>
            /// Get education by ID
            /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EducationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EducationResponseDto>> GetEducationById(int id)
        {
            var education = await _educationService.GetEducationByIdAsync(id);
            if (education == null)
                return NotFound(new { message = $"Education with ID {id} not found." });
            return Ok(education);
        }

        /// <summary>
        /// Create a new education record (Admin endpoint - Requires authentication)
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(EducationResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EducationResponseDto>> CreateEducation([FromBody] CreateEducationDto dto)
        {
            var createdEducation = await _educationService.CreateEducationAsync(dto);
            return CreatedAtAction(nameof(GetEducationById), new { id = createdEducation.Id }, createdEducation);
        }

        /// <summary>
        /// Update an existing education record (Admin endpoint - Requires authentication)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(EducationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EducationResponseDto>> UpdateEducation(int id, [FromBody] UpdateEducationDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID in URL does not match ID in body." });
            
            var updatedEducation = await _educationService.UpdateEducationAsync(dto);
            return Ok(updatedEducation);
        }

        /// <summary>
        /// Delete an education record (soft delete - Admin endpoint - Requires authentication)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var deleted = await _educationService.DeleteEducationAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Education with ID {id} not found." });
            return NoContent();
        }
    }
}

