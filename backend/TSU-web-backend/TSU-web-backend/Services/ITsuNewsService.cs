using TSU_web_backend.Dtos;

namespace TSU_web_backend.Services;

public interface ITsuNewsService
{
    Task<IReadOnlyList<ExternalNewsItemDto>> GetLatestNewsAsync(CancellationToken cancellationToken = default);
}
