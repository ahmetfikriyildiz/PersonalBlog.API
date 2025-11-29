    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PersonalBlog.API.DTOs.Commons;
    using PersonalBlog.API.DTOs.Experience;
    using PersonalBlog.API.Services.Interfaces;

    namespace PersonalBlog.API.Controllers
    {
        /// <summary>
        /// Controller for managing experience records
        /// </summary>
        [ApiController]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public class ExperienceController : ControllerBase
        {
            private readonly IExperienceService _experienceService;

            public ExperienceController(IExperienceService experienceService)
            {
                _experienceService = experienceService;
            }

            /// <summary>
            /// Get all experience records
            /// </summary>
            [HttpGet]
            [ProducesResponseType(typeof(IEnumerable<ExperienceResponseDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<IEnumerable<ExperienceResponseDto>>> GetAllExperiences()
            {
                var experiences = await _experienceService.GetAllExperiencesAsync();
                return Ok(experiences);
            }

            /// <summary>
            /// Get all experience records paged
            /// </summary>
            [HttpGet("paged")]
            [ProducesResponseType(typeof(PagedResponse<ExperienceResponseDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<PagedResponse<ExperienceResponseDto>>> GetAllExperiencesPaged([FromQuery] PaginationFilter filter)
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var response = await _experienceService.GetAllExperiencesPagedAsync(validFilter);
                return Ok(response);
            }

            /// <summary>
            /// Get experience by ID
            /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExperienceResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExperienceResponseDto>> GetExperienceById(int id)
        {
            var experience = await _experienceService.GetExperienceByIdAsync(id);
            if (experience == null)
                return NotFound(new { message = $"Experience with ID {id} not found." });
            return Ok(experience);
        }

        /// <summary>
        /// Create a new experience record (Admin endpoint - Requires authentication)
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ExperienceResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ExperienceResponseDto>> CreateExperience([FromBody] CreateExperienceDto dto)
        {
            var createdExperience = await _experienceService.CreateExperienceAsync(dto);
            return CreatedAtAction(nameof(GetExperienceById), new { id = createdExperience.Id }, createdExperience);
        }

        /// <summary>
        /// Update an existing experience record (Admin endpoint - Requires authentication)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ExperienceResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ExperienceResponseDto>> UpdateExperience(int id, [FromBody] UpdateExperienceDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID in URL does not match ID in body." });
            
            var updatedExperience = await _experienceService.UpdateExperienceAsync(dto);
            return Ok(updatedExperience);
        }

        /// <summary>
        /// Delete an experience record (soft delete - Admin endpoint - Requires authentication)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var deleted = await _experienceService.DeleteExperienceAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Experience with ID {id} not found." });
            return NoContent();
        }
    }
}

