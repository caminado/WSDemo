using AutoMapper; 
using Microsoft.AspNetCore.Mvc;
using WSDemo.ServiceLayer;
using WSDemo.Domain.DTO;
using WSDemo.Domain;

namespace WSDemo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoldersController : ControllerBase
{
    private const int DEFAULT_PAGE_SIZE = 10;
    private readonly ILogger<FoldersController> _logger;
    private readonly IFolderItemsService _service;
    private readonly IMapper _mapper;

    public FoldersController(IFolderItemsService service, IMapper mapper, ILogger<FoldersController> logger)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string? parentId, string? pageSize, string? pageNum)
    {
        try
        {
            Guid parentIdAsGuid = Guid.Empty;
            int pageNumAsInt = 0;
            int pageSizeAsInt = DEFAULT_PAGE_SIZE;

            if (!string.IsNullOrWhiteSpace(parentId) && !Guid.TryParse(parentId , out parentIdAsGuid))
                return BadRequest("Invalid Parent Id");

            if (!string.IsNullOrWhiteSpace(pageNum) && !int.TryParse(pageNum, out pageNumAsInt))
                return BadRequest("Invalid Page number");

            if (!string.IsNullOrWhiteSpace(pageSize) && !int.TryParse(pageSize, out pageSizeAsInt))
                return BadRequest("Invalid Page size");

            var res = await _service.GetByParentIdAsync(parentIdAsGuid, pageSizeAsInt, pageNumAsInt);
            if (res == null || !res.Any())
            {
                return Ok(new List<FolderItemDto>());
            }
            return Ok(_mapper.Map<List<FolderItemDto>>( res));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling API");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out Guid idAsGuid))
                return BadRequest("Invalid id");
            var res = await _service.GetByIdAsync(idAsGuid);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FolderItemDto>(res));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling API");
            throw;
        }
    }

    [HttpPut]
    public async Task<IActionResult> Create([FromBody] FolderItemDto dto)
    {
        try
        {
            var item = _mapper.Map<FolderItem>(dto);
            var res = await _service.InsertAsync(item);
            _logger.LogInformation($"Created folder item with id {res}");
            return Ok(res);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling API");
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] FolderItemDto dto)
    {
        try
        {
            var item = _mapper.Map<FolderItem>(dto);
            await _service.UpdateAsync(item);
            _logger.LogInformation($"Updated folder item with id {dto.Id}");
            return Ok();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling API");
            throw;
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out Guid idAsGuid))
                return BadRequest("Invalid Id");
            await _service.DeleteAsync(idAsGuid);
            _logger.LogInformation($"Deleted folder item with id {id}");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling API");
            throw;
        }
    }
}
