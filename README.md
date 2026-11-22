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
- **.NET 10 SDK** com as cargas de trabalho do MAUI para Windows e Android:
  ```powershell
  dotnet workload install maui-windows maui-android
  ```
- **Android SDK + Java (JDK)** — podem ser instalados pelo próprio projeto (veja abaixo)
  ou pelo Visual Studio 2022/2026 com a carga de trabalho **.NET MAUI**.
- Emulador Android (requer virtualização ativada na BIOS) **ou** celular Android com
  depuração USB ativada. Também roda como app Windows.
- Internet (o app consome as APIs em tempo real).

> Alvos do projeto: `net10.0-android` e `net10.0-windows10.0.19041.0`.

### 1. Clonar
```powershell
git clone https://github.com/SEU_USUARIO/IbgeNomes.git
cd IbgeNomes
```

### 2. Rodar no Windows
```powershell
dotnet build -f net10.0-windows10.0.19041.0
.\bin\Debug\net10.0-windows10.0.19041.0\win-x64\IbgeNomes.exe
```
> Se o caminho da pasta **não** tiver espaços, também funciona:
> `dotnet build -t:Run -f net10.0-windows10.0.19041.0`

### 3. Rodar no Android (emulador)
Instalar o Android SDK e o JDK (só na primeira vez):
```powershell
dotnet build -t:InstallAndroidDependencies -f net10.0-android "-p:AndroidSdkDirectory=C:\Android\Sdk" "-p:JavaSdkDirectory=C:\Android\jdk" "-p:AcceptAndroidSDKLicenses=True"
```

Criar o emulador Pixel com API 34 (só na primeira vez):
```powershell
$env:JAVA_HOME = "C:\Android\jdk"
C:\Android\Sdk\cmdline-tools\latest\bin\sdkmanager.bat --sdk_root=C:\Android\Sdk "emulator" "platforms;android-34" "system-images;android-34;google_apis;x86_64"
C:\Android\Sdk\cmdline-tools\latest\bin\avdmanager.bat create avd -n Pixel_API_34 -k "system-images;android-34;google_apis;x86_64" -d pixel
```

Ligar o emulador, conferir se aparece como `device` e rodar o app:
```powershell
Start-Process C:\Android\Sdk\emulator\emulator.exe -ArgumentList "-avd","Pixel_API_34"
C:\Android\Sdk\platform-tools\adb.exe devices
dotnet build -t:Run -f net10.0-android "-p:AndroidSdkDirectory=C:\Android\Sdk" "-p:JavaSdkDirectory=C:\Android\jdk"
```

### Pelo Visual Studio (alternativa)
Abra `IbgeNomes.csproj`, escolha o destino **Android Emulator** (ou *Windows Machine*)
e pressione **F5**.

### Usando o app
Escolha um **estado** → veja os nomes; escolha um **município** para filtrar;
use os botões **Homens / Mulheres / Sobrenomes** para alternar as listas.

## 🎥 Vídeo de demonstração

Link: _cole aqui o link do YouTube ou Google Drive_
