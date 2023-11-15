using System.Xml.Linq;
using System.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace HAMovies
{
    internal class Program
    {
        // Класс, представляющий фильм
        class Movie
        {
            public string Title { get; set; }
            public string Genre { get; set; }
            public int Year { get; set; }
            public double Rating { get; set; }
        }
        static void Main(string[] args)
        {
            // Чтение файла movies.xml
            string filePath = "movies.xml"; //кидает unhandled exception
            XDocument doc = XDocument.Load(filePath);

            // Получение списка фильмов
            List<Movie> movies = doc.Descendants("movie")
                                    .Select(m => new Movie
                                    {
                                        Title = m.Element("title")?.Value,
                                        Genre = m.Element("genre")?.Value,
                                        Year = int.Parse(m.Element("year")?.Value),
                                        Rating = double.Parse(m.Element("rating")?.Value ?? "0")
                                    })
                                    .ToList();

            // Сортировка фильмов по названию
            var sortedMovies = movies.OrderBy(m => m.Title);

            // Вывод отсортированного списка фильмов
            Console.WriteLine("Список фильмов:");
            foreach (var movie in sortedMovies)
            {
                Console.WriteLine(movie.Title);
            }

            // Получение списка уникальных жанров фильмов
            var uniqueGenres = movies.Select(m => m.Genre).Distinct();

            // Вывод списка уникальных жанров фильмов
            Console.WriteLine("\nУникальные жанры фильмов:");
            foreach (var genre in uniqueGenres)
            {
                Console.WriteLine(genre);
            }

            // Подсчет общего количества фильмов
            int totalMovies = movies.Count;

            // Вывод общего количества фильмов
            Console.WriteLine($"\nОбщее количество фильмов: {totalMovies}");

            // Поиск фильма с самым высоким рейтингом
            var highestRatedMovie = movies.OrderByDescending(m => m.Rating).FirstOrDefault();

            // Вывод названия и рейтинга фильма с самым высоким рейтингом
            Console.WriteLine($"\nСамый высокий рейтинг: {highestRatedMovie.Rating}");
            Console.WriteLine($"Фильм с самым высоким рейтингом: {highestRatedMovie.Title}");

            // Сортировка фильмов по году выпуска, начиная с самых новых
            var sortedByYear = movies.OrderByDescending(m => m.Year);

            // Вывод списка фильмов, отсортированных по году выпуска
            Console.WriteLine($"\nСписок фильмов, отсортированных по году выпуска (начиная с самых новых):");
            foreach (var movie in sortedByYear)
            {
                Console.WriteLine($"{movie.Title} ({movie.Year})");
            }

            // Поиск фильмов, выпущенных в последние 5 лет
            int currentYear = DateTime.Now.Year;
            var recentMovies = movies.Where(m => m.Year >= currentYear - 5);

            // Вывод списка фильмов, выпущенных в последние 5 лет
            Console.WriteLine($"\nСписок фильмов, выпущенных в последние 5 лет:");
            foreach (var movie in recentMovies)
            {
                Console.WriteLine($"{movie.Title} ({movie.Year})");
            }

            // Вычисление среднего рейтинга всех фильмов
            double averageRating = movies.Average(m => m.Rating);

            // Вывод среднего рейтинга
            Console.WriteLine($"\nСредний рейтинг всех фильмов: {averageRating}");

            // Поиск фильмов, выпущенных после 2010 года с рейтингом выше 8
            var highRatedMovies = movies.Where(m => m.Year > 2010 && m.Rating > 8);

            // Вывод списка фильмов, выпущенных после 2010 года с рейтингом выше 8
            Console.WriteLine($"\nФильмы, выпущенные после 2010 года с рейтингом выше 8:");
            foreach (var movie in highRatedMovies)
            {
                Console.WriteLine($"{movie.Title} ({movie.Year})");
            }

            // Сохранение результатов в файл output.json
            string json = JsonConvert.SerializeObject(movies, Formatting.Indented);
            File.WriteAllText("output.json", json);
        }
    }
}