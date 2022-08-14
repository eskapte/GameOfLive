using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace GameOfLive
{
    internal class Program
    {
        private const int WIDTH = 80;
        private const int HEIGHT = 25;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    throw new Exception("Не передан путь до файла первым аргументом");

                var gameField = new GameField(HEIGHT, WIDTH);
                var fileInput = ReadInputDataFromFile(args[0]);
                var livingCells = GetInitialLivingCells(fileInput);

                gameField.SetInitialLive(livingCells);
                Render(gameField);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
                return;
            }
        }

        private static void Render(GameField gameField)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;

                foreach (var row in gameField)
                    Console.WriteLine(row);

                Console.ResetColor();

                gameField.NewTick();
                Thread.Sleep(300);
            }
        }

        private static IEnumerable<Tuple<int, int>> GetInitialLivingCells(IEnumerable<string> data)
        {
            var inputFormat = @"^(?<row>\d+) (?<col>\d+\s*$)";

            return data.Select((x, line) =>
            {
                var regex = new Regex(inputFormat, RegexOptions.None);
                var match = regex.Match(x);

                if (!match.Success)
                    throw new Exception($"Неправильный формат(row, col) на линии {line + 1}: \"{x}\"");

                int row = int.Parse(match.Groups["row"].Value);
                var col = int.Parse(match.Groups["col"].Value);

                return new Tuple<int, int>(row, col);
            });
        }

        private static IEnumerable<string> ReadInputDataFromFile(string filePath)
        {
            try
            {
                using var reader = File.OpenText(filePath);
                var lines = reader.ReadToEnd().Split("\n");

                return lines;
            }
            catch (Exception err)
            {
                throw new Exception($"Ошибка при чтении файла: {err.Message}");
            }
        }
    }
}