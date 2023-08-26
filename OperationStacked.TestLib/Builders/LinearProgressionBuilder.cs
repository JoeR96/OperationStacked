using OperationStacked.Entities;

public class LinearProgressionExerciseBuilder : ExerciseBuilder<LinearProgressionExercise, LinearProgressionExerciseBuilder>
{
    public LinearProgressionExerciseBuilder WithDefaultValues()
    {
        base.WithDefaultValues();

        _exercise.MinimumReps = 8;
        _exercise.MaximumReps = 12;
        _exercise.Sets = 3;
        _exercise.WeightIndex = 0;
        _exercise.PrimaryExercise = true;
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
}