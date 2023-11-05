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
using OperationStacked.TestLib.Builders;

public class LinearProgressionMachineTests
{
    private IExerciseRepository _exerciseRepository;
    private IEquipmentStackRepository _equipmentStackRepository;
    private LinearProgressionService _service;

    [SetUp]
    public void SetUp()
    {
        _exerciseRepository = Substitute.For<IExerciseRepository>();
        _equipmentStackRepository = Substitute.For<IEquipmentStackRepository>();

        _service = new LinearProgressionService(_exerciseRepository,_equipmentStackRepository);
    }

    [Test]
    public async Task MachineDeloadIsIndex0OnFirstStackFailure()
    {
        var equipmentStack = CreateDefaultEquipmentStack();
        var exercise = CreateExercise(workingWeight: 10, failedAttempts: 2);
        
        SetupRepository(exercise, equipmentStack);

        var (nextExercise, _) = await _service.ProgressExercise(CreateExerciseRequest(7, 12, 12, 12));

        nextExercise.WorkingWeight.Should().Be(equipmentStack.GenerateStack()[0]);
    }

    [Test]
    public async Task MachineProgressesToNextStackIndex()
    {
        var equipmentStack = CreateDefaultEquipmentStack();
        var exercise = CreateExercise(workingWeight: (decimal)equipmentStack.GenerateStack()[0], failedAttempts: 2);
        
        SetupRepository(exercise, equipmentStack);

        var (nextExercise, status) = await _service.ProgressExercise(CreateExerciseRequest(12, 12, 12, 12));

        status.Should().Be(ExerciseCompletedStatus.Progressed);
        nextExercise.WorkingWeight.Should().Be(equipmentStack.GenerateStack()[nextExercise.WeightIndex]);
    }

    private void SetupRepository(LinearProgressionExercise exercise, EquipmentStack equipmentStack)
    {
        _exerciseRepository.GetExerciseById(Arg.Any<Guid>()).Returns(exercise.WorkoutExercise.Exercise);
        _equipmentStackRepository.GetEquipmentStack(exercise.WorkoutExercise.EquipmentStackId).Returns(equipmentStack);
    }

    private EquipmentStack CreateDefaultEquipmentStack() 
        => new EquipmentStackBuilder().WithDefaultValues().Build();

    private LinearProgressionExercise CreateExercise(int failedAttempts = 0, decimal workingWeight = 8)
    {
        return new LinearProgressionExerciseBuilder()
            .WithDefaultValues()
            .WithSets(4)
            .WithWorkingWeight(workingWeight)
            .WithFailedAttempt(failedAttempts)
            .WithReps(8, 12)
            .Build().AdaptToEntity();
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
