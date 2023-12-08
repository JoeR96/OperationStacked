using OperationStacked.DTOs;
using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseRetrievalService
{
    public class ExerciseRetrievalService : IExerciseRetrievalService
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseRetrievalService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<GetWorkoutResult> GetWorkout(Guid userId, int week, int day, bool completed)
        {
            var exercises = await _exerciseRepository.GetWorkoutExercisesByWeekAndDay(userId, week, day);
            var exerciseDtos = exercises.Select(we => MapToWorkoutExerciseDto(we)).ToList();
            return new GetWorkoutResult(exerciseDtos);
        }

        public async Task<GetWorkoutResult> GetAllWorkouts(Guid userId, int pageIndex, int pageSize)
        {
            var (exercises, totalCount) = await _exerciseRepository.GetAllWorkoutExercisesWithCount(userId, pageIndex, pageSize);

            var exerciseDtos = exercises.Select(we => MapToWorkoutExerciseDto(we)).ToList();

            return new GetWorkoutResult(exerciseDtos, totalCount);
        }

        private WorkoutExerciseDto MapToWorkoutExerciseDto(WorkoutExercise we)
        {
            return new WorkoutExerciseDto
            {
                Id = we.Id,
                WorkoutId = we.WorkoutId,
                ExerciseId = we.ExerciseId,
                Exercise = MapToExerciseDto(we.Exercise),
                LinearProgressionExercises = we.LinearProgressionExercises.Select(lpe => MapToLinearProgressionExerciseDto(lpe)).ToList(),
                Template = we.Template,
                LiftDay = we.LiftDay,
                LiftOrder = we.LiftOrder,
                Completed = we.Completed,
                RestTimer = we.RestTimer,
                MinimumReps = we.MinimumReps,
                MaximumReps = we.MaximumReps,
                Sets = we.Sets,
                WeightProgression = we.WeightProgression,
                AttemptsBeforeDeload = we.AttemptsBeforeDeload,
                EquipmentStackId = we.EquipmentStackId
            };
        }

        private ExerciseDto MapToExerciseDto(Exercise e)
        {
            return new ExerciseDto
            {
                Id = e.Id,
                ExerciseName = e.ExerciseName,
                Category = e.Category,
                EquipmentType = e.EquipmentType,
                UserId = e.UserId
            };
        }

        private LinearProgressionExerciseDto MapToLinearProgressionExerciseDto(LinearProgressionExercise lpe)
        {
            return new LinearProgressionExerciseDto
            {
                WorkoutExerciseId = lpe.WorkoutExerciseId,
                Id = lpe.Id,
                CurrentAttempt = lpe.CurrentAttempt,
                ParentId = lpe.ParentId,
                LiftWeek = lpe.LiftWeek,
                WorkingWeight = lpe.WorkingWeight,
                WeightIndex = lpe.WeightIndex
            };
        }

    }
}
