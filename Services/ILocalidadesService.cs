using IbgeNomes.Models;

namespace IbgeNomes.Services;

public interface ILocalidadesService
{
    Task<List<Estado>> ObterEstadosAsync(CancellationToken ct = default);
    Task<List<Municipio>> ObterMunicipiosAsync(int estadoId, CancellationToken ct = default);
}
