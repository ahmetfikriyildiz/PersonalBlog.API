    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PersonalBlog.API.DTOs.Commons;
    using PersonalBlog.API.DTOs.Skills;
    using PersonalBlog.API.Services.Interfaces;

    namespace PersonalBlog.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public class SkillsController : ControllerBase
        {
            private readonly ISkillService _skillService;

            public SkillsController(ISkillService skillService)
            {
                _skillService = skillService;
            }

            [HttpGet]
            [ProducesResponseType(typeof(IEnumerable<SkillsResponseDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<IEnumerable<SkillsResponseDto>>> GetAllSkills()
            {
                var skills = await _skillService.GetAllSkillsAsync();
                return Ok(skills);
            }

            [HttpGet("paged")]
            [ProducesResponseType(typeof(PagedResponse<SkillsResponseDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<PagedResponse<SkillsResponseDto>>> GetAllSkillsPaged([FromQuery] PaginationFilter filter)
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var response = await _skillService.GetAllSkillsPagedAsync(validFilter);
                return Ok(response);
            }

            [HttpGet("{id}")]
        [ProducesResponseType(typeof(SkillsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SkillsResponseDto>> GetSkillById(int id)
        {
            var skill = await _skillService.GetSkillByIdAsync(id);

            if (skill == null)
                return NotFound(new { message = $"Skill with ID {id} not found." });

            return Ok(skill);
        }

        /// <summary>
        /// Create a new skill (Admin endpoint - Requires authentication)
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SkillsResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SkillsResponseDto>> CreateSkill([FromBody] CreateSkillDto dto)
        {
            var createdSkill = await _skillService.CreateSkillAsync(dto);

            return CreatedAtAction(
                nameof(GetSkillById),
                new { id = createdSkill.Id },
                createdSkill
            );
        }

        /// <summary>
        /// Update an existing skill (Admin endpoint - Requires authentication)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(SkillsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SkillsResponseDto>> UpdateSkill(int id, [FromBody] UpdateSkillDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID in URL does not match ID in body." });

            var updatedSkill = await _skillService.UpdateSkillAsync(dto);
            return Ok(updatedSkill);
        }

        /// <summary>
        /// Delete a skill (soft delete - Admin endpoint - Requires authentication)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var deleted = await _skillService.DeleteSkillAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Skill with ID {id} not found." });

            return NoContent();
        }
    }
}