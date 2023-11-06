using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Analysis.GC;
using System.IO;


namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string test = "наименование исследования мскт грудной клетки " +
                "заключение на севере нативных мскт органов грудной клетки есть конструкция средства в высоком разрешении определяется" +
                "Значение структурно не смещено трахея проходима бронхи 1 3 порядка проходимы грудной отдел аорты и канальные артерии с мелкими концернальными" +
                "Лимфатические узлы толщиной до 6 7 мм не увеличены диафрагма обычно расположены" +
                "Жидкости в приварных полостях не выявлено" +
                "Трудоднев позвоночника без костно зубных изменений умеренная декоративная линия позвоночника заключение" +
                "Врач савельева татьяна вячеславовна";

            var name = test.IndexOf("наимено");
            var zakl = test.IndexOf("заклю");

            var mass = test.AsSpan().Slice(name, zakl);
            Console.WriteLine(mass.ToString());

            //BenchmarkRunner.Run<Benchy>();

            //var name = test.IndexOf("наимено");
            //var zakl = test.IndexOf("заклю");
            //var text = test.Substring(name, zakl);


            //var mass = test.Split(" ").AsSpan();
            //var ind = mass.index("исследов");

            //var word = test.IndexOf("исследов");
            //var wordPlusSpace = test.IndexOf(" ", ind);


            //Console.WriteLine(space);
            //Console.WriteLine(ind);
            //Console.WriteLine(indspace);

            //var name = test.AsSpan().Slice(indspace,);
            //Console.Write(name.ToString());
        }

        [MemoryDiagnoser]
        [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
        [RankColumn]
        public class Benchy
        {
            public const string test = "наименование исследования мскт грудной клетки " +
                "заключение на севере нативных мскт органов грудной клетки есть конструкция средства в высоком разрешении определяется" +
                "Значение структурно не смещено трахея проходима бронхи 1 3 порядка проходимы грудной отдел аорты и канальные артерии с мелкими концернальными" +
                "Лимфатические узлы толщиной до 6 7 мм не увеличены диафрагма обычно расположены" +
                "Жидкости в приварных полостях не выявлено" +
                "Трудоднев позвоночника без костно зубных изменений умеренная декоративная линия позвоночника заключение" +
                "Врач савельева татьяна вячеславовна";

            [Benchmark]
            public string UseIndexes()
            {
                var name = test.IndexOf("наимено");
                var zakl = test.IndexOf("заклю");
                var text = test.Substring(name, zakl);

                return text;
            }

            [Benchmark]
            public string UseSplit()
            {
                var name = test.IndexOf("наимено");
                var zakl = test.IndexOf("заклю");

                var mass = test.AsSpan().Slice(name, zakl);
                return mass.ToString();
            }
        }
    }
}