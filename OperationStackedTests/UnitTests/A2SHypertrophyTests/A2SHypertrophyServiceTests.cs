using FluentAssertions;
using NUnit.Framework;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Factories;
using OperationStacked.Services.A2S;

namespace OperationStackedTests.UnitTests.A2SHypertrophyTests
{
    [TestFixture]
    public class A2SHypertrophyServiceTests
    {
        IA2SHypertrophyService _service;

        [OneTimeSetUp]
        public void ClassInit()
        {
            _service = new A2SHypertrophyService();
        }

        [TestCase(100, 5, 65)]
        public void WorkingWeightRoundsToNearestRoundingValue(decimal trainingMax, decimal roundingValue, decimal expectedValue)
        {
            var e = new A2SHypertrophyExercise()
            {
                TrainingMax = trainingMax,
                RoundingValue = roundingValue,
                Intensity = 1
            };

            e.WorkingWeight = _service.GetWorkingWeight(A2SBlocks.Hypertrophy,1,false,trainingMax,roundingValue);
            e.WorkingWeight.Should().Be(expectedValue);
        }

        ////[TestCase(-3, 98)]
        //[TestCase(-2, 98)]
        //[TestCase(-1, 99)]
        //[TestCase(0, 100)]
        //[TestCase(1, 100.5)]
        //[TestCase(2, 101)]
        //[TestCase(3, 101.5)]
        //[TestCase(4, 102)]
        //[TestCase(5, 103)]
        ////[TestCase(6, 103)]

        //public void A2SHypertrophyTrainingMaxUpdates(int amrapResult, decimal expectedResult)
        //{
        //    A2SHypertrophyExercise w1 = new A2SHypertrophyExercise
        //    {
        //        TrainingMax = 100m,
        //        RoundingValue = 2.5m,
        //        AmrapRepResult = amrapResult,
        //        AmrapRepTarget = 0
        //    };

        //    A2SHypertrophyExercise w2 = new A2SHypertrophyExercise
        //    {
        //        TrainingMax = 100m,
        //        RoundingValue = 2.5m
        //    };

        //    factory.ProgressExercise(w1, w2);
        //    expectedResult.Should().Be(w2.TrainingMax);
        //}
    }
}
