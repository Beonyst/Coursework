using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            var bootstrapper = new BootstrapperAutofac(Configuration);
            var mainWindow = new MainWindow { DataContext = bootstrapper.MainWindowViewModel };
            mainWindow.Show();
        }
    }
}
