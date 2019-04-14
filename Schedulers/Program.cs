using System;
using System.Linq;
using System.Collections.Generic;

namespace Schedulers
{
    class Result
    {
        public static List<string> findSchedulers(int workHours, int dayHours, string pattern)
        {
            var positionsToFill = getPositionsToFill(pattern);
            var patterns = new List<string>();

            for (int i = 0; i < positionsToFill.Count(); i++)
            {
                var position = positionsToFill[i];
                for (int hour = dayHours; hour >= 0; hour--)
                {
                    pattern = updatePattern(pattern, position, hour);
                    if (i < positionsToFill.Count() - 1)
                    {
                        patterns.AddRange(findSchedulers(workHours, dayHours, pattern));
                    } else
                    {
                        patterns.Add(pattern);
                    }
                }
            }

            var result = patterns.Where(a => getTotalHoursAlreadySet(a) == workHours).Distinct().ToList();
            return result.OrderBy(a => a).ToList();
        }

        private static string updatePattern(string pattern, int position, int hour)
        {
            return pattern.Substring(0, position) + hour.ToString() + pattern.Substring(position + 1);
        }

        private static int getTotalHoursAlreadySet(string pattern)
        {
            int hours = 0;

            hours = pattern.ToCharArray().Select(a =>
            {
                int number = 0;
                int.TryParse(a.ToString(), out number);
                return number;
            }).Sum();

            return hours;
        }

        private static List<int> getPositionsToFill(string pattern)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < pattern.Length; i++)
                if (pattern[i] == '?') result.Add(i);

            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Tests> patterns = new List<Tests>()
            {
                new Tests(24, 4, "08??840"),
                new Tests(56, 8, "???8???"),
                new Tests(3 , 2, "??2??00"),
                new Tests(15 , 5, "???????")
            };

            foreach (var item in patterns)
            {
                List<string> results = Result.findSchedulers(item.workHours, item.dayHours, item.pattern);

                Console.WriteLine("Pattern: " + item.pattern);
                Console.WriteLine("--------------------------");
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }

                Console.WriteLine("--------------------------");
            }

            Console.ReadLine();
        }
    }

    class Tests
    {
        public Tests(int work, int day, string pattern)
        {
            this.workHours = work;
            this.dayHours = day;
            this.pattern = pattern;
        }

        public string pattern { get; set; }
        public int workHours { get; set; }
        public int dayHours { get; set; }
    }
}
