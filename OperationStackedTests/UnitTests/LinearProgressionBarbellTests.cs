using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OperationStacked.Entities;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.TestLib.Adapters;

namespace OperationStackedAuth.Tests.UnitTests;

[TestFixture]
public class LinearProgressionTests
{
    private IExerciseRepository _exerciseRepository;
    private IEquipmentStackRepository _equipmentStackRepository;
    private LinearProgressionService _service;

    [SetUp]
    public void SetUp()
    {
        _exerciseRepository = Substitute.For<IExerciseRepository>();
        _service = new LinearProgressionService(_exerciseRepository,_equipmentStackRepository);
    }

    [Test]
    public async Task LinearProgressionExercise_CreatesWithValues()
    {
        var createExerciseModel = new ExerciseBuilder().WithDefaultValues().Build();
        var workoutExercise = new WorkoutExerciseBuilder().WithDefaultValues().Build();
        var linearProgressionExercise =
            new LinearProgressionExerciseBuilder().WithDefaultValues().AdaptToCreateRequest(workoutExercise.AdaptToCreateRequest());
        var createdExercise = await _service.CreateLinearProgressionExercise(linearProgressionExercise.AdaptToCreateRequest(),workoutExercise.AdaptToEntity());

        createdExercise.WorkoutExercise.Exercise.ExerciseName.Should().Be(createExerciseModel.ExerciseName);
        createdExercise.WorkoutExercise.MinimumReps.Should().Be(linearProgressionExercise.MinimumReps);
    }

    [Test]
    public async Task LinearProgressionExercise_BarbellProgressWhenMaxRepsAchieved()
    {
        var exercise = CreateExercise();
        SetupRepositoryWithExercise(exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(12, 12, 12, 12));

        status.Should().Be(ExerciseCompletedStatus.Progressed);
        nextExercise.WorkingWeight.Should().Be(exercise.ProgressedWeight());
    }

    [Test]
    public async Task LinearProgressionExercise_AttemptOneBeforeDeloadStaysTheSame()
    {
        var exercise = CreateExercise();
        SetupRepositoryWithExercise(exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(8, 9, 10, 12));

        status.Should().Be(ExerciseCompletedStatus.StayedTheSame);
        nextExercise.WorkingWeight.Should().Be(exercise.WorkingWeight);
        nextExercise.CurrentAttempt.Should().Be(0);
    }

    [Test]
    public async Task LinearProgressionExercise_FailedAttemptIncreasesFailedAttempt()
    {
        var exercise = CreateExercise();
        SetupRepositoryWithExercise(exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(7, 9, 10, 12));

        status.Should().Be(ExerciseCompletedStatus.Failed);
        nextExercise.WorkingWeight.Should().Be(exercise.WorkingWeight);
        nextExercise.CurrentAttempt.Should().Be(1);
    }
    [Test]
    public async Task LinearProgressionExercise_AttemptTwoProgresses()
    {
        var exercise = CreateExercise(2);
        SetupRepositoryWithExercise(exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(12, 12, 12, 12));

        status.Should().Be(ExerciseCompletedStatus.Progressed);
        nextExercise.WorkingWeight.Should().Be(exercise.ProgressedWeight());
    }

    [Test]
    public async Task LinearProgressionExercise_AttemptTwoFailedDeloads()
    {
        var exercise = CreateExercise(2);
        SetupRepositoryWithExercise(exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(5, 9, 10, 12));

        status.Should().Be(ExerciseCompletedStatus.Deload);
        nextExercise.WorkingWeight.Should().Be(exercise.DeloadWeight());
        nextExercise.CurrentAttempt.Should().Be(0);
    }

    // Helper methods
    private LinearProgressionExercise CreateExercise(int failedAttempts = 0)
    {
        return new LinearProgressionExerciseBuilder()
            .WithDefaultValues()
            .WithSets(4)
            .WithFailedAttempt(failedAttempts)
            .WithReps(8, 12)
            .Build().AdaptToEntity();
    }

    private void SetupRepositoryWithExercise(LinearProgressionExercise exercise)
    {
        var completeExerciseRequest = new CompleteExerciseRequest { LinearProgressionExerciseId = new Guid() };
        _exerciseRepository.GetExerciseById(completeExerciseRequest.LinearProgressionExerciseId).Returns(exercise.WorkoutExercise.Exercise);
    }

    private CompleteExerciseRequest CreateExerciseRequest(params int[] reps)
    {
        return new CompleteExerciseRequest
        {
            LinearProgressionExerciseId = new Guid(),
            Reps = reps,
            Sets = reps.Length
        };
    }
}
