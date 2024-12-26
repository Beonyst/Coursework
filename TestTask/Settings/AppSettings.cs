#nullable enable

namespace API.Settings;

// Класс AppSettings используется для хранения настроек приложения, например, строки подключения к базе данных.
public class AppSettings
{
    // Свойство для хранения строки подключения к базе данных.
    public string ConnectionString { get; set; } = null!;  // Обязательное поле, которое не может быть null (задано с помощью аннотации).
}
