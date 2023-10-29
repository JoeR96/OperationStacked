using OperationStacked.TestLib;

public class LinearProgressionExerciseBuilder
{
    protected LinearProgressionExercise _exercise = new LinearProgressionExercise();

    public LinearProgressionExerciseBuilder WithDefaultValues()
    {
        _exercise.MinimumReps = 8;
        _exercise.MaximumReps = 12;
        _exercise.Sets = 3;
        _exercise.WeightIndex = 0;
        _exercise.WeightProgression = 2.50M;
        _exercise.AttemptsBeforeDeload = 2;
        _exercise.CurrentAttempt = 1;
        _exercise.EquipmentStackId = Guid.NewGuid();
        return this;
    }


    public LinearProgressionExerciseBuilder WithWeightProgression(decimal weightProgression)
    {
        _exercise.WeightProgression = weightProgression;
        return this;
    }

    public LinearProgressionExerciseBuilder WithReps(int minReps, int maxReps)
    {
        _exercise.MinimumReps = minReps;
        _exercise.MaximumReps = maxReps;
        return this;
    }

    public LinearProgressionExerciseBuilder WithSets(int targetSets)
    {
        _exercise.Sets = targetSets;
        return this;
    }

    public LinearProgressionExerciseBuilder WithFailedAttempt(int failedAttempt)
    {
        _exercise.CurrentAttempt = failedAttempt;
        return this;
    }

    public LinearProgressionExercise Build() => _exercise;

    public LinearProgressionExerciseBuilder WithWorkingWeight(decimal workingWeight)
    {
        _exercise.WorkingWeight = workingWeight;
        return this;
    }

    public CreateLinearProgressionExerciseRequest AdaptToCreateRequest(CreateWorkoutExerciseRequest workoutExercise)
    {
        return new CreateLinearProgressionExerciseRequest
        {
            MinimumReps = _exercise.MinimumReps,
            MaximumReps = _exercise.MaximumReps,
            TargetSets = _exercise.Sets,
            WeightIndex = _exercise.WeightIndex,
            WeightProgression = _exercise.WeightProgression,
            AttemptsBeforeDeload = _exercise.AttemptsBeforeDeload,
            EquipmentType = workoutExercise.Exercise.EquipmentType,
            // Assuming you have a method to create or get an existing EquipmentStackKey
            EquipmentStackKey = GetOrCreateEquipmentStackKey(_exercise.EquipmentStackId),
            // Assuming you have a method to create or get an existing EquipmentStack
            EquipmentStack = GetOrCreateEquipmentStack(_exercise.EquipmentStackId),
            WorkoutExercise = workoutExercise
        };
    }

    // Placeholder for methods to create or retrieve EquipmentStackKey and EquipmentStack
    private EquipmentStackKey GetOrCreateEquipmentStackKey(Guid equipmentStackId)
    {
        // Implement the logic to create or retrieve the EquipmentStackKey
        return new EquipmentStackKey(); // Replace with actual implementation
    }

    private CreateEquipmentStackRequest GetOrCreateEquipmentStack(Guid equipmentStackId)
    {
        // Implement the logic to create or retrieve the EquipmentStack
        return new CreateEquipmentStackRequest(); // Replace with actual implementation
    }
}
