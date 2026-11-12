using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;

namespace UltimateFlags.Configuration.Services;

internal class FlagCommandService : IFlagCommandService
{
    private readonly ILogger<FlagCommandService> _logger;

    public FlagCommandService(ILogger<FlagCommandService> logger)
    {
        _logger = logger;
    }

    public FlagResponse Create(FlagCreationRequest creationRequest)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Create)}(creationRequest)", };
    }

    public FlagResponse Update(Guid id, FlagUpdateRequest updateRequest)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Update)}(id, updateRequest)", };
    }

    public IEnumerable<FlagResponse> Delete(Guid id, Guid? concurrencyToken = null)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Delete)}(id, concurrencyToken)", };
    }

    public IEnumerable<FlagResponse> Purge(Guid id, Guid? concurrencyToken = null)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Purge)}(id, concurrencyToken)", };
    }

    public IEnumerable<FlagResponse> Restore(Guid id, Guid? concurrencyToken = null)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Restore)}(id, concurrencyToken)", };
    }

    public void Enable(Guid id)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Enable)}(id)", };
    }

    public void Enable(string key)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Enable)}(key)", };
    }

    public void Enable(string name, Guid? parentId)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Enable)}(name, parentId)", };
    }

    public void Disable(Guid id)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Disable)}(id)", };
    }

    public void Disable(string key)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Disable)}(key)", };
    }

    public void Disable(string name, Guid? parentId)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(Disable)}(name, parentId)", };
    }

    public Task<FlagResponse> CreateAsync(FlagCreationRequest creationRequest, CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(CreateAsync)}(creationRequest)", };
    }

    public Task<FlagResponse> UpdateAsync(
        Guid id,
        FlagUpdateRequest updateRequest,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(UpdateAsync)}(id, updateRequest)", };
    }

    public Task<IEnumerable<FlagResponse>> DeleteAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(DeleteAsync)}(id, concurrencyToken)", };
    }

    public Task<IEnumerable<FlagResponse>> PurgeAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(PurgeAsync)}(id, concurrencyToken)", };
    }

    public Task<IEnumerable<FlagResponse>> RestoreAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(RestoreAsync)}(id, concurrencyToken)", };
    }

    public Task EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(EnableAsync)}(id)", };
    }

    public Task EnableAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(EnableAsync)}(key)", };
    }

    public Task EnableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(EnableAsync)}(name, parentId)", };
    }

    public Task DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(DisableAsync)}(id)", };
    }

    public Task DisableAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(DisableAsync)}(key)", };
    }

    public Task DisableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandService)}.{nameof(DisableAsync)}(name, parentId)", };
    }
}
