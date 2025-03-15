namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        public static string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, 
            TimeSpan endWorkingTime, int consultationTime)
        {
            try
            {
                if (startTimes == null || durations == null)
                    throw new ArgumentNullException("Входные массивы не могут быть пустыми");
                if (startTimes.Length != durations.Length)
                    throw new ArgumentException("Массивы времени начала и длительности должны иметь одинаковую длину");
                if (consultationTime <= 0)
                    throw new ArgumentException("Время консультации должно быть больше нуля");
                if (beginWorkingTime >= endWorkingTime)
                    throw new ArgumentException("Время начала работы должно быть раньше времени окончания");

                List<string> FreeTimeIntervals = new List<string>();
                List<(TimeSpan start, TimeSpan end)> BeginningAndEndWorkingDay = new List<(TimeSpan, TimeSpan)>();

                for (int i = 0; i < startTimes.Length; i++)
                {
                    BeginningAndEndWorkingDay.Add((startTimes[i], startTimes[i].Add(TimeSpan.FromMinutes(durations[i]))));
                }

                BeginningAndEndWorkingDay = BeginningAndEndWorkingDay.OrderBy(slot => slot.start).ToList();

                TimeSpan currentStart = beginWorkingTime;

                foreach ((TimeSpan start, TimeSpan end) slot in BeginningAndEndWorkingDay)
                {
                    if (currentStart.Add(TimeSpan.FromMinutes(consultationTime)) <= slot.start)
                    {
                        FreeTimeIntervals.Add($"{currentStart:hh\\:mm}-{slot.start:hh\\:mm}");
                    }

                    if (currentStart < slot.end)
                    {
                        currentStart = slot.end;
                    }
                }

                if (currentStart.Add(TimeSpan.FromMinutes(consultationTime)) <= endWorkingTime)
                {
                    FreeTimeIntervals.Add($"{currentStart:hh\\:mm}-{endWorkingTime:hh\\:mm}");
                }

                return FreeTimeIntervals.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return Array.Empty<string>();
            }
        }
    }
}
