using IbgeNomes.ViewModels;

namespace IbgeNomes.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;

    // O ViewModel chega por injeção de dependência (registrado no MauiProgram.cs).
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InicializarAsync(); // ao abrir o app, popula o Picker de estados
    }
}
