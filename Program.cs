/// <summary>
/// Приклад реалізації структурного патерну «Адаптер».
/// Завдання: інтегрувати стару бібліотеку в сучасний проєкт.
/// </summary>
using System;

// ================= Стара бібліотека =================
namespace LegacyLibrary
{
    /// <summary>
    /// Стара бібліотека для рендерингу, яку потрібно адаптувати.
    /// Має свій власний інтерфейс.
    /// </summary>
    public class OldRenderingEngine
    {
        public void DrawImage(string fileName)
        {
            Console.WriteLine($"[Legacy] Малюю зображення з файлу: {fileName}");
        }

        public void DrawText(string text)
        {
            Console.WriteLine($"[Legacy] Вивід тексту: {text}");
        }
    }
}

// ================= Сучасний інтерфейс =================
namespace ModernApp
{
    /// <summary>
    /// Сучасний інтерфейс рендерингу, який очікує клієнтський код.
    /// </summary>
    public interface IRenderer
    {
        void RenderImage(string path);
        void RenderText(string content);
    }

    /// <summary>
    /// Сучасна реалізація рендерера.
    /// </summary>
    public class ModernRenderer : IRenderer
    {
        public void RenderImage(string path) => Console.WriteLine($"[Modern] Rendering image from {path}");

        public void RenderText(string content) => Console.WriteLine($"[Modern] Rendering text: {content}");
    }
}

// ================= Адаптер =================
namespace AdapterImplementation
{
    using LegacyLibrary;
    using ModernApp;

    /// <summary>
    /// Адаптер, що дозволяє використовувати стару бібліотеку через сучасний інтерфейс.
    /// </summary>
    public class OldEngineAdapter : IRenderer
    {
        private readonly OldRenderingEngine _oldEngine;

        public OldEngineAdapter(OldRenderingEngine oldEngine)
        {
            _oldEngine = oldEngine ?? throw new ArgumentNullException(nameof(oldEngine));
        }

        public void RenderImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("[Adapter] Некоректний шлях до файлу!");
                return;
            }
            _oldEngine.DrawImage(path);
        }

        public void RenderText(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("[Adapter] Некоректний текст!");
                return;
            }
            _oldEngine.DrawText(content);
        }
    }
}

// ================= Клієнтський код =================
namespace AdapterDemo
{
    using LegacyLibrary;
    using ModernApp;
    using AdapterImplementation;

    internal class Program
    {
        private static void Main()
        {
            // Використання сучасного рендерера
            IRenderer modernRenderer = new ModernRenderer();
            modernRenderer.RenderImage("hero.png");
            modernRenderer.RenderText("Hello, modern world!");

            Console.WriteLine("============================");

            // Використання старого рендерера через адаптер
            var oldEngine = new OldRenderingEngine();
            IRenderer adapter = new OldEngineAdapter(oldEngine);
            adapter.RenderImage("legacy_map.jpg");
            adapter.RenderText("Привіт зі старої бібліотеки!");
        }
    }
}
