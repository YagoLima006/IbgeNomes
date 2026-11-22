# IBGE Data Insights — Nomes mais comuns por localidade

Aplicativo **.NET MAUI (C#)** que consome **duas APIs públicas do IBGE em cadeia**:

1. **API de Localidades** → popula os seletores (`Picker`) de **estado** e **município**.
2. **API de Nomes (Censo 2022)** → com o `id` da localidade escolhida, busca e exibe os
   **nomes mais comuns de homens, de mulheres e os sobrenomes** daquela região.

## 👥 Integrantes

| Nome completo | RA / Matrícula |
|---|---|
| Yago Cesar ________________ | ________ |
| ___________________________ | ________ |
| ___________________________ | ________ |

## 🔗 APIs utilizadas

| Finalidade | Endpoint |
|---|---|
| Estados | `GET https://servicodados.ibge.gov.br/api/v1/localidades/estados` |
| Municípios do estado | `GET https://servicodados.ibge.gov.br/api/v1/localidades/estados/{UF}/municipios` |
| Nomes por localidade | `GET https://servicodados.ibge.gov.br/api/v3/nomes/2022/localidade/{id_localidade}` |

Documentação oficial: <https://servicodados.ibge.gov.br/api/docs>

### Formato da resposta da API de Nomes

```json
{
  "localidade": 31,
  "mulheres":   [ { "nome": "maria", "frequencia": 1297501 }, ... ],
  "homens":     [ { "nome": "jose",  "frequencia": 528206 },  ... ],
  "sobrenomes": [ { "nome": "silva", "frequencia": 3162886 }, ... ]
}
```
Cada lista traz os 10 mais frequentes da localidade (estado ou município).

## 🧱 Arquitetura em camadas

```
IbgeNomes/
├── Models/                 ← Camada de Modelos (mapeiam o JSON)
│   ├── Estado.cs
│   ├── Municipio.cs
│   ├── NomeFrequencia.cs
│   ├── ResultadoNomes.cs
│   └── RespostaNomesLocalidade.cs  (JSON da API de Nomes)
├── Services/               ← Camada de Serviços / Integração (HttpClient + JSON)
│   ├── IbgeApi.cs              (URLs centralizadas)
│   ├── ILocalidadesService.cs / LocalidadesService.cs
│   ├── INomesService.cs / NomesService.cs
│   └── IbgeApiException.cs
│   ├── IbgeApi.cs              (URLs centralizadas)
│   ├── ILocalidadesService.cs / LocalidadesService.cs
│   ├── INomesService.cs / NomesService.cs
│   └── IbgeApiException.cs
├── ViewModels/             ← Apresentação (estado da tela, MVVM)
│   ├── BaseViewModel.cs
│   └── MainViewModel.cs
├── Views/                  ← Apresentação (UI)
│   ├── MainPage.xaml       (Pickers, botões, CollectionView)
│   └── MainPage.xaml.cs
├── MauiProgram.cs          ← Injeção de dependência (HttpClient, serviços, telas)
└── AppShell.xaml
```

**Fluxo:** `MainPage` aparece → `MainViewModel.InicializarAsync()` → `LocalidadesService`
busca os estados → usuário escolhe um estado → `LocalidadesService` busca os municípios
**e** `NomesService` busca os nomes do estado → usuário (opcionalmente) escolhe um
município → `NomesService` busca os nomes do município → `CollectionView` atualiza.

A tela **nunca** faz HTTP diretamente: ela só conversa com o ViewModel, que só conversa
com as interfaces de serviço.

## ▶️ Como executar

### Pré-requisitos
- **Visual Studio 2022 (17.12+) ou mais recente**, com a carga de trabalho
  **“Desenvolvimento de interface do usuário de aplicativo multiplataforma do .NET” (.NET MAUI)**.
- Emulador Android configurado (Android Device Manager) **ou** celular Android com
  depuração USB ativada. (Também roda como app Windows.)
- Internet (o app consome as APIs em tempo real).

### Passos
1. Clone o repositório:
   ```bash
   git clone https://github.com/SEU_USUARIO/IbgeNomes.git
   ```
2. Abra `IbgeNomes.sln` no Visual Studio.
3. Na barra superior, escolha o destino **Android Emulator** (ou *Windows Machine*).
4. Pressione **F5**.
5. No app: escolha um **estado** → veja os nomes; escolha um **município** para filtrar;
   use os botões **Homens / Mulheres / Sobrenomes** para alternar as listas.

Pela linha de comando (opcional):
```bash
dotnet build -t:Run -f net9.0-android    # troque net9.0 pela versão do seu projeto
```

## 🎥 Vídeo de demonstração

Link: _cole aqui o link do YouTube ou Google Drive_
