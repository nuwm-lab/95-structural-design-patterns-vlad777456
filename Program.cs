using System;

namespace AdapterPattern
{
    /// <summary>
    /// Приклад клієнтського інтерфейсу — що хоче бачити сучасний код.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Намалювати спрайт з іменем на позиції (x,y).
        /// </summary>
        void RenderSprite(string spriteName, int x, int y);
    }

    // ------------------ "Стара" бібліотека (Legacy) ------------------
    // У реальному житті це може бути стороння DLL, код якої ви не можете змінити.
    /// <summary>
    /// Умовна стара графічна бібліотека з відмінним API.
    /// </summary>
    public class OldRenderingEngine
    {
        // Старе API приймає байтовий масив з зображенням і координати у форматі float.
        public void DrawImage(byte[] imageData, float posX, float posY)
        {
            // Симуляція роботи старого двигуна
            Console.WriteLine($"Old engine drawing image ({imageData?.Length ?? 0} bytes) at ({posX},{posY})");
        }

        // Додатковий метод — наприклад, завантаження текстури за назвою
        public byte[] LoadTextureByName(string name)
        {
            // У реальній бібліотеці тут був би код, який читає файл/ресурс.
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Texture name cannot be null or empty.");

            // Повертаємо умовні дані картинки
            return System.Text.Encoding.UTF8.GetBytes(name);
        }
    }

    // ------------------ Adapter ------------------
    /// <summary>
    /// Адаптер дозволяє використовувати OldRenderingEngine через сучасний IRenderer інтерфейс.
    /// Використовує композицію (краща практика при адаптації зовнішніх бібліотек).
    /// </summary>
    public class OldEngineAdapter : IRenderer
    {
        private readonly OldRenderingEngine _oldEngine;

        public OldEngineAdapter(OldRenderingEngine oldEngine)
        {
            _oldEngine = oldEngine ?? throw new ArgumentNullException(nameof(oldEngine));
        }

        /// <summary>
        /// Перетворює виклик RenderSprite на відповідні виклики старого API.
        /// Тут можна додавати кешування текстур, масштабування, логування тощо.
        /// </summary>
        public void RenderSprite(string spriteName, int x, int y)
        {
            if (string.IsNullOrWhiteSpace(spriteName))
                throw new ArgumentException("Sprite name cannot be null or whitespace.", nameof(spriteName));

            // Приклад простої адаптації: завантажити текстуру старим методом і намалювати її
            byte[] texture = _oldEngine.LoadTextureByName(spriteName);

            // Конвертуємо координати int -> float, тут можна додати масштабування або офсет
            float posX = x;
            float posY = y;

            _oldEngine.DrawImage(texture, posX, posY);
        }
    }

    // ------------------ Сучасна реалізація рендера (для порівняння) ------------------
    /// <summary>
    /// Сучасна реалізація IRenderer — для демонстрації, як працює код без адаптера.
    /// </summary>
    public class ModernRenderer : IRenderer
    {
        public void RenderSprite(string spriteName, int x, int y)
        {
            if (string.IsNullOrWhiteSpace(spriteName))
                throw new ArgumentException("Sprite name cannot be null or whitespace.", nameof(spriteName));

            Console.WriteLine($"Modern renderer draws '{spriteName}' at ({x},{y})");
        }
    }

    // ------------------ Client code ------------------
    public static class Program
    {
        public static void Main()
        {
            // Клієнтський код працює з IRenderer і не знає про те, що під володіє адаптером
            IRenderer modern = new ModernRenderer();
            modern.RenderSprite("hero_idle", 10, 20);

            // Використання старої бібліотеки через адаптер
            var oldEngine = new OldRenderingEngine();
            IRenderer adapted = new OldEngineAdapter(oldEngine);
            adapted.RenderSprite("enemy_attack", 50, 60);

            // Перевага: весь код, який очікує IRenderer, може працювати з адаптером
            RenderSceneWithRenderer(modern);
            RenderSceneWithRenderer(adapted);
        }

        private static void RenderSceneWithRenderer(IRenderer renderer)
        {
            // Демонстраційна сцена
            renderer.RenderSprite("tree", 5, 5);
            renderer.RenderSprite("rock", 7, 12);
        }
    }
}
