using OperationStacked.Entities;
using OperationStacked.Models;

public class WorkoutExerciseBuilder
{
    protected WorkoutExercise _workoutExercise = new WorkoutExercise();

    public WorkoutExerciseBuilder WithDefaultValues()
    {
        _workoutExercise.Id = Guid.NewGuid();
        _workoutExercise.ExerciseId = Guid.Empty; // or some default value
        _workoutExercise.WorkoutId = Guid.Empty; // or some default value
        _workoutExercise.Template = ExerciseTemplate.LinearProgression; // or some default value
        _workoutExercise.LiftDay = 1;
        _workoutExercise.LiftOrder = 1;
        _workoutExercise.Completed = false;
        _workoutExercise.RestTimer = 0;
        return this;
    }

    public WorkoutExerciseBuilder WithExerciseId(Guid exerciseId)
    {
        _workoutExercise.ExerciseId = exerciseId;
        return this;
    }

    public WorkoutExerciseBuilder WithWorkoutId(Guid workoutId)
    {
        _workoutExercise.WorkoutId = workoutId;
        return this;
    }

    public WorkoutExerciseBuilder WithTemplate(ExerciseTemplate template)
    {
        _workoutExercise.Template = template;
        return this;
    }

    public WorkoutExerciseBuilder WithLiftDay(int liftDay)
    {
        _workoutExercise.LiftDay = liftDay;
        return this;
    }

    public WorkoutExerciseBuilder WithLiftOrder(int liftOrder)
    {
        _workoutExercise.LiftOrder = liftOrder;
        return this;
    }

    public WorkoutExerciseBuilder WithCompletionStatus(bool completed)
    {
        _workoutExercise.Completed = completed;
        return this;
    }

    public WorkoutExerciseBuilder WithRestTimer(int restTimer)
    {
        _workoutExercise.RestTimer = restTimer;
        return this;
    }

    public WorkoutExercise Build() => _workoutExercise;
}
