using System;
using System.Collections.Generic;
using System.Linq;
using Holidays.Core.Extensions;

namespace Holidays.Core
{
    namespace Styrets.Core.Factories
    {
        public class HolidayFactory
        {
            private readonly int _year;

            private readonly int _currentMonth;

            private List<DateTime> Holidays { get; set; }

            public DateTime EasterDay { get { return _easterDay; } }

            private DateTime _easterDay;

            public HolidayFactory(int? currentYear, int? currentMonth)
            {
                if (!currentYear.HasValue)
                {
                    currentYear = DateTime.Now.Year;
                }

                if (!currentMonth.HasValue)
                {
                    currentMonth = DateTime.Now.Month;
                }

                Holidays = new List<DateTime>();

                _year = currentYear.Value;

                _currentMonth = currentMonth.Value;

                AddAllFixedHolidays();

                SetEasterDay();

                AddHolidaysThatAreDependentOnEasterDay();
            }

            public IEnumerable<DateTime> GetAllHolidaysIncludedWeekends()
            {
                AddAllWeekends();

                return Holidays.OrderBy(d => d.Date);
            }

            public IEnumerable<DateTime> GetAllHolidaysExcludedWeekends()
            {
                return Holidays;
            }

            private void AddHolidaysThatAreDependentOnEasterDay()
            {
                AddMaundayThursday();
                AddLongFriday();
                AddHolySaturday();
                AddEasterMonday();
                AddAscension();
                AddPentecost();
                AddPentecostEvening();
                AddMidSummerEvening();
                AddMidSummerDay();
                AddAllHallowsEve();
                AddAllHallowsDay();
            }

            private void SetEasterDay()
            {
                _easterDay = CalculateEasterDay(_year);
            }

            private void AddAllFixedHolidays()
            {
                Holidays = new List<DateTime>
            {
                new DateTime(_year, 1, 1),
                //Trettondagsafton (shouldnt be a red day)
                new DateTime(_year, 1, 5),
                new DateTime(_year, 5, 1),
                new DateTime(_year, 4, 30),
                //new DateTime(_year, 6, 1),
                new DateTime(_year, 6, 6),
                new DateTime(_year, 12, 24),
                new DateTime(_year, 12, 25),
                new DateTime(_year, 12, 26),
                new DateTime(_year, 12, 31)
            };
            }

            /// <summary>
            /// Alla helgons dag
            /// </summary>
            private void AddAllHallowsDay()
            {
                const DayOfWeek Day = DayOfWeek.Saturday;

                var firstValidDate = new DateTime(_year, 10, 31);

                if (firstValidDate.DayOfWeek == Day)
                {
                    AddDateIfNotExsist(firstValidDate);
                    return;
                }

                while (firstValidDate.DayOfWeek != Day)
                {
                    firstValidDate = firstValidDate.AddDays(1);
                }

                AddDateIfNotExsist(firstValidDate);
            }

            /// <summary>
            /// Allhelgonaafton
            /// </summary>
            private void AddAllHallowsEve()
            {
                const DayOfWeek day = DayOfWeek.Friday;

                var firstValidDate = new DateTime(_year, 10, 30);

                if (firstValidDate.DayOfWeek == day)
                {
                    AddDateIfNotExsist(firstValidDate);
                    return;
                }

                while (firstValidDate.DayOfWeek != day)
                {
                    firstValidDate = firstValidDate.AddDays(1);
                }

                AddDateIfNotExsist(firstValidDate);
            }

            /// <summary>
            /// MidsommarDagen
            /// </summary>
            private void AddMidSummerDay()
            {
                const DayOfWeek day = DayOfWeek.Saturday;

                var firstValidDate = new DateTime(_year, 6, 20);

                if (firstValidDate.DayOfWeek == day)
                {
                    AddDateIfNotExsist(firstValidDate);
                    return;
                }

                while (firstValidDate.DayOfWeek != day)
                {
                    firstValidDate = firstValidDate.AddDays(1);
                }

                AddDateIfNotExsist(firstValidDate);
            }

            /// <summary>
            /// MidsommarAfton
            /// </summary>
            private void AddMidSummerEvening()
            {
                const DayOfWeek day = DayOfWeek.Friday;

                var firstValidDate = new DateTime(_year, 6, 19);

                if (firstValidDate.DayOfWeek == day)
                {
                    AddDateIfNotExsist(firstValidDate);
                    return;
                }

                while (firstValidDate.DayOfWeek != day)
                {
                    firstValidDate = firstValidDate.AddDays(1);
                }

                AddDateIfNotExsist(firstValidDate);
            }

            /// <summary>
            /// PingstAfton
            /// </summary>
            private void AddPentecostEvening()
            {
                AddDateIfNotExsist(_easterDay.Next(DayOfWeek.Sunday).AddDays(6 * 7).AddDays(-1));
            }

            /// <summary>
            /// Pingstdagen
            /// </summary>
            private void AddPentecost()
            {
                AddDateIfNotExsist(_easterDay.Next(DayOfWeek.Sunday).AddDays(6 * 7));
            }

            /// <summary>
            /// Kristi himmelsfärdsdag
            /// </summary>
            private void AddAscension()
            {
                AddDateIfNotExsist(_easterDay.Next(DayOfWeek.Thursday).AddDays(5 * 7));
            }

            /// <summary>
            /// Annandag Påsk
            /// </summary>
            private void AddEasterMonday()
            {
                AddDateIfNotExsist(_easterDay.AddDays(1));
            }

            /// <summary>
            /// Påskafton
            /// </summary>
            private void AddHolySaturday()
            {
                AddDateIfNotExsist(_easterDay.Previous(DayOfWeek.Saturday));
            }

            /// <summary>
            /// Lång fredagen
            /// </summary>
            private void AddLongFriday()
            {
                AddDateIfNotExsist(_easterDay.Previous(DayOfWeek.Friday));
            }

            /// <summary>
            /// Skärtorsdagen
            /// </summary>
            private void AddMaundayThursday()
            {
                AddDateIfNotExsist(_easterDay.Previous(DayOfWeek.Thursday));
            }

            private void AddDateIfNotExsist(DateTime date)
            {
                if (!Holidays.Contains(date))
                {
                    Holidays.Add(date);
                }
            }

            private void AddAllWeekends()
            {
                Holidays = Holidays.Where(d => d.Month == _currentMonth).ToList();

                var firstOfYear = new DateTime(_year, _currentMonth, 1);

                var currentDay = firstOfYear;

                while (firstOfYear.Month == currentDay.Month)
                {
                    var dayOfWeek = currentDay.DayOfWeek;
                    if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                        AddDateIfNotExsist(currentDay);

                    currentDay = currentDay.AddDays(1);
                }
            }

            private static DateTime CalculateEasterDay(int year)
            {
                var g = year % 19;
                var c = year / 100;
                var h = (c - (c / 4) - ((8 * c + 13) / 25) + 19 * g + 15) % 30;
                var i = h - h / 28 * (1 - h / 28 *
                            (29 / (h + 1)) * ((21 - g) / 11));

                var day = i - ((year + year / 4 + i + 2 - c + c / 4) % 7) + 28;
                var month = 3;

                if (day <= 31)
                {
                    return new DateTime(year, month, day);
                }

                month++;
                day -= 31;

                return new DateTime(year, month, day);
            }
        }
    }
}
