using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IbgeNomes.ViewModels;

/// <summary>
/// Implementa INotifyPropertyChanged para a tela atualizar sozinha quando um valor muda.
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? nome = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));

    protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string? nome = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor)) return false;
        campo = valor;
        OnPropertyChanged(nome);
        return true;
    }
}
