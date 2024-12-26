using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для приложения.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Конфигурация приложения.
        /// </summary>
        public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Метод, который вызывается при старте приложения.
        /// Инициализирует конфигурацию, контейнер зависимостей и главное окно.
        /// </summary>
        /// <param name="e">Параметры события старта.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создание конфигурации из файла appsettings.json.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Устанавливает базовый путь для поиска конфигурационных файлов.
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); // Добавляет конфигурацию из файла JSON.
            
            // Строит конфигурацию.
            Configuration = builder.Build();

            // Устанавливает режим завершения приложения после закрытия последнего окна.
            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            // Инициализация Bootstrapper (контейнера зависимостей).
            var bootstrapper = new BootstrapperAutofac(Configuration);

            // Создание главного окна и привязка ViewModel.
            var mainWindow = new MainWindow { DataContext = bootstrapper.MainWindowViewModel };
            
            // Показывает главное окно приложения.
            mainWindow.Show();
        }
    }
}
