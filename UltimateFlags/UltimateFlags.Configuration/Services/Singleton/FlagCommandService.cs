using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;

namespace UltimateFlags.Configuration.Services.Singleton;

internal class FlagCommandService : IFlagCommandService
{
    private readonly ILogger<FlagCommandService> _logger;

    public FlagCommandService(ILogger<FlagCommandService> logger)
    {
        _logger = logger;
    }

    public FlagResponse Create(FlagCreationRequest creationRequest)
    {
        throw new NotSupported { Area = $"{nameof(Services.FlagCommandService)}.{nameof(Create)}(creationRequest)", };
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
        throw new NotImplementedException();
    }

    public Task<FlagResponse> UpdateAsync(
        Guid id,
        FlagUpdateRequest updateRequest,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FlagResponse>> DeleteAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FlagResponse>> PurgeAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FlagResponse>> RestoreAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task EnableAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task EnableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DisableAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DisableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
