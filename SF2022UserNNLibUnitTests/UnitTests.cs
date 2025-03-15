using SF2022User_NN_Lib;

namespace SF2022UserNNLibUnitTests
{
    [TestClass]
    public sealed class UnitTests
    {
        [TestMethod]
        public void NoAppointments_FullDayAvailable()
        {
            TimeSpan[] startTimes = { };
            int[] durations = { };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[]? result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("09:00-18:00", result[0]);
        }

        [TestMethod]
        public void SingleAppointment_CreatesTwoFreeIntervals()
        {
            TimeSpan[] startTimes = { new TimeSpan(12, 0, 0) };
            int[] durations = { 60 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[]? result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            CollectionAssert.AreEqual(new[] { "09:00-12:00", "13:00-18:00" }, result);
        }

        [TestMethod]
        public void NoFreeIntervals_TooShortGaps()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(14, 0, 0) };
            int[] durations = { 120, 240 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 130;

            string[]? result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void AppointmentAtStart_DoesNotIncludeBeforeWorkHours()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 60 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[]? result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("10:00-18:00", result[0]);
        }

        [TestMethod]
        public void AppointmentAtEnd_DoesNotIncludeAfterWorkHours()
        {
            TimeSpan[] startTimes = { new TimeSpan(17, 0, 0) };
            int[] durations = { 60 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("09:00-17:00", result[0]);
        }

        [TestMethod]
        public void BackToBackAppointments_NoAvailableIntervals()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = { 60, 60, 60 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(12, 0, 0);
            int consultationTime = 30;

            var result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void MultipleAppointments_WithGaps()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 30, 0), new TimeSpan(14, 30, 0) };
            int[] durations = { 60, 60 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            CollectionAssert.AreEqual(new[] { "09:00-09:30", "10:30-14:30", "15:30-18:00" }, result);
        }        

        [TestMethod]
        public void NullInput_ReturnsEmptyArray()
        {
            TimeSpan[] startTimes = null;
            int[] durations = null;
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void MismatchedArrays_ReturnsEmptyArray()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 60, 30 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            var result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void NegativeConsultationTime_ReturnsEmptyArray()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 60 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int consultationTime = -30;

            var result = Calculations.AvailablePeriods(startTimes, durations, begin, end, consultationTime);

            Assert.AreEqual(0, result.Length);
        }
    }
}
