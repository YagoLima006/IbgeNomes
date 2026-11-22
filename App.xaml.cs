using Microsoft.Extensions.DependencyInjection;

namespace IbgeNomes;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = new Window(new AppShell());

#if WINDOWS
		// No Windows, abre em formato "celular" (em vez de ocupar a tela toda).
		window.Width = 450;
		window.Height = 800;
		window.MinimumWidth = 380;
		window.MinimumHeight = 600;
#endif

		return window;
	}
}