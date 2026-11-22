using System.Collections.ObjectModel;
using System.Windows.Input;
using IbgeNomes.Models;
using IbgeNomes.Services;

namespace IbgeNomes.ViewModels;

/// <summary>
/// Liga a tela (MainPage) aos serviços. Não faz HTTP diretamente:
/// só chama ILocalidadesService e INomesService.
/// </summary>
public class MainViewModel : BaseViewModel
{
    public enum Aba { Homens, Mulheres, Sobrenomes }

    private readonly ILocalidadesService _localidades;
    private readonly INomesService _nomes;
    private CancellationTokenSource? _ctsNomes;
    private ResultadoNomes? _resultado;
    private bool _ignorarSelecaoMunicipio;

    public MainViewModel(ILocalidadesService localidades, INomesService nomes)
    {
        _localidades = localidades;
        _nomes = nomes;

        MostrarHomensCommand = new Command(() => AbaAtual = Aba.Homens);
        MostrarMulheresCommand = new Command(() => AbaAtual = Aba.Mulheres);
        MostrarSobrenomesCommand = new Command(() => AbaAtual = Aba.Sobrenomes);
        RecarregarCommand = new Command(async () => await InicializarAsync());
    }

    // ------------------------------------------------------------- dados da tela

    public ObservableCollection<Estado> Estados { get; } = new();
    public ObservableCollection<Municipio> Municipios { get; } = new();
    public ObservableCollection<NomeFrequencia> ListaAtual { get; } = new();

    public ICommand MostrarHomensCommand { get; }
    public ICommand MostrarMulheresCommand { get; }
    public ICommand MostrarSobrenomesCommand { get; }
    public ICommand RecarregarCommand { get; }

    private Estado? _estadoSelecionado;
    public Estado? EstadoSelecionado
    {
        get => _estadoSelecionado;
        set
        {
            if (SetProperty(ref _estadoSelecionado, value) && value is not null)
                _ = AoSelecionarEstadoAsync(value);
        }
    }

    private Municipio? _municipioSelecionado;
    public Municipio? MunicipioSelecionado
    {
        get => _municipioSelecionado;
        set
        {
            if (SetProperty(ref _municipioSelecionado, value) && value is not null && !_ignorarSelecaoMunicipio)
                _ = AoSelecionarMunicipioAsync(value);
        }
    }

    private Aba _abaAtual = Aba.Homens;
    public Aba AbaAtual
    {
        get => _abaAtual;
        set
        {
            if (SetProperty(ref _abaAtual, value))
            {
                OnPropertyChanged(nameof(HomensAtivo));
                OnPropertyChanged(nameof(MulheresAtivo));
                OnPropertyChanged(nameof(SobrenomesAtivo));
                AtualizarLista();
            }
        }
    }

    public bool HomensAtivo => AbaAtual == Aba.Homens;
    public bool MulheresAtivo => AbaAtual == Aba.Mulheres;
    public bool SobrenomesAtivo => AbaAtual == Aba.Sobrenomes;

    private bool _carregando;
    public bool Carregando { get => _carregando; set => SetProperty(ref _carregando, value); }

    private bool _temMunicipios;
    public bool TemMunicipios { get => _temMunicipios; set => SetProperty(ref _temMunicipios, value); }

    private string _titulo = "Selecione um estado";
    public string Titulo { get => _titulo; set => SetProperty(ref _titulo, value); }

    private string _mensagem = string.Empty;
    public string Mensagem
    {
        get => _mensagem;
        set { SetProperty(ref _mensagem, value); OnPropertyChanged(nameof(TemMensagem)); }
    }
    public bool TemMensagem => !string.IsNullOrEmpty(Mensagem);

    // ------------------------------------------------------------- fluxo

    /// <summary>Chamado quando a tela aparece: carrega os estados no Picker.</summary>
    public async Task InicializarAsync()
    {
        if (Estados.Count > 0) return;
        try
        {
            Carregando = true;
            Mensagem = string.Empty;
            foreach (var e in await _localidades.ObterEstadosAsync())
                Estados.Add(e);
        }
        catch (Exception ex)
        {
            Mensagem = $"{ex.Message} Verifique a internet e toque em Recarregar.";
        }
        finally
        {
            Carregando = false;
        }
    }

    private async Task AoSelecionarEstadoAsync(Estado estado)
    {
        // Carrega os municípios do estado (para o 2º Picker) e, em paralelo, os nomes do estado.
        var tarefaNomes = CarregarNomesAsync(estado.Id, estado.NomeExibicao);

        try
        {
            TemMunicipios = false;
            _ignorarSelecaoMunicipio = true;
            Municipios.Clear();
            MunicipioSelecionado = null;

            var lista = await _localidades.ObterMunicipiosAsync(estado.Id);
            if (EstadoSelecionado?.Id != estado.Id) return; // usuário trocou de estado no meio

            Municipios.Add(Municipio.TodoEstado());
            foreach (var m in lista) Municipios.Add(m);
            MunicipioSelecionado = Municipios[0];
            TemMunicipios = true;
        }
        catch (Exception ex)
        {
            Mensagem = ex.Message;
        }
        finally
        {
            _ignorarSelecaoMunicipio = false;
        }

        await tarefaNomes;
    }

    private Task AoSelecionarMunicipioAsync(Municipio municipio)
    {
        if (EstadoSelecionado is null) return Task.CompletedTask;

        return municipio.EhTodoEstado
            ? CarregarNomesAsync(EstadoSelecionado.Id, EstadoSelecionado.NomeExibicao)
            : CarregarNomesAsync(municipio.Id, $"{municipio.Nome} - {EstadoSelecionado.Sigla}");
    }

    /// <summary>Consulta encadeada: usa o id da localidade na API de Nomes.</summary>
    private async Task CarregarNomesAsync(int localidadeId, string descricao)
    {
        // Cancela a consulta anterior se o usuário trocou de seleção rápido.
        _ctsNomes?.Cancel();
        var cts = _ctsNomes = new CancellationTokenSource();

        try
        {
            Carregando = true;
            Mensagem = string.Empty;
            Titulo = $"Nomes mais comuns em {descricao}";
            ListaAtual.Clear();

            _resultado = await _nomes.ObterNomesAsync(localidadeId, cts.Token);
            if (cts.IsCancellationRequested) return;

            AtualizarLista();
        }
        catch (OperationCanceledException)
        {
            // troca de seleção: ignora
        }
        catch (Exception ex)
        {
            if (!cts.IsCancellationRequested) Mensagem = ex.Message;
        }
        finally
        {
            if (ReferenceEquals(cts, _ctsNomes)) Carregando = false;
        }
    }

    private void AtualizarLista()
    {
        ListaAtual.Clear();
        if (_resultado is null) return;

        var origem = AbaAtual switch
        {
            Aba.Homens => _resultado.Masculinos,
            Aba.Mulheres => _resultado.Femininos,
            _ => _resultado.Sobrenomes,
        };

        foreach (var n in origem) ListaAtual.Add(n);
    }
}
