using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.TestLib.Adapters;

[TestFixture]
public class LinearProgressionDumbbellTests
{
    private IExerciseRepository _repository;
    private LinearProgressionService _service;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IExerciseRepository>();
        _service = new LinearProgressionService(_repository);
    }
    
    [Test]
    public async Task LinearProgressionExercise_DumbbellProgressesBy1KGIfUnder10KG ()
    {
        var exercise = CreateExercise();
        SetupRepositoryWithExercise(exercise.WorkoutExercise.Exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(12, 12, 12, 12));

        status.Should().Be(ExerciseCompletedStatus.Progressed);
        nextExercise.WorkingWeight.Should().Be(9);
    }
    
    [Test]
    public async Task LinearProgressionExercise_DumbbellProgressesBy2KGIfOver10KG ()
    {
        var exercise = CreateExercise(workingWeight:10);
        SetupRepositoryWithExercise(exercise.WorkoutExercise.Exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(12, 12, 12, 12));

        status.Should().Be(ExerciseCompletedStatus.Progressed);
        nextExercise.WorkingWeight.Should().Be(12);
    }
    [Test]
    public async Task LinearProgressionExercise_DumbbellDeloadsBy1KGIf10KGorUnder ()
    {
        var exercise = CreateExercise(failedAttempts:2,workingWeight:10);
        SetupRepositoryWithExercise(exercise.WorkoutExercise.Exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(7, 12, 12, 12));

        status.Should().Be(ExerciseCompletedStatus.Deload);
        nextExercise.WorkingWeight.Should().Be(9);
    }
    
    [Test]
    public async Task LinearProgressionExercise_DumbbellDeloadsBy2KGIfOver10KG ()
    {
        var exercise = CreateExercise(workingWeight:12,failedAttempts:2);
        SetupRepositoryWithExercise(exercise.WorkoutExercise.Exercise);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(7, 12, 12, 12));

        status.Should().Be(ExerciseCompletedStatus.Deload);
        nextExercise.WorkingWeight.Should().Be(10);
    }

    private LinearProgressionExercise CreateExercise(int failedAttempts = 0,int workingWeight = 8)
    {
        return new LinearProgressionExerciseBuilder()
            .WithDefaultValues()
            .WithSets(4)
            .WithWorkingWeight(workingWeight)
            .WithFailedAttempt(failedAttempts)
            .WithReps(8, 12)
            .Build().AdaptToEntity();
    }

    private void SetupRepositoryWithExercise(Exercise exercise)
    {
        var completeExerciseRequest = new CompleteExerciseRequest { LinearProgressionExerciseId = new Guid() };
        _repository.GetExerciseById(completeExerciseRequest.LinearProgressionExerciseId).Returns(exercise);
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
