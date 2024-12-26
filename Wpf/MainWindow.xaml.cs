using System.Windows;

namespace Wpf
{
    /// <summary>
    /// Класс MainWindow представляет главное окно приложения.
    /// Логика взаимодействия описана в соответствующем XAML-файле.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор класса MainWindow.
        /// Инициализирует компоненты пользовательского интерфейса, 
        /// описанные в файле MainWindow.xaml.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); // Вызывает метод для инициализации всех элементов интерфейса.
        }
    }
}
