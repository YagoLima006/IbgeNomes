namespace IbgeNomes.Services;

/// <summary>
/// Erro "amigável" da camada de serviço. A tela mostra a Message ao usuário.
/// </summary>
public class IbgeApiException : Exception
{
    public IbgeApiException(string message, Exception? inner = null) : base(message, inner) { }
}
