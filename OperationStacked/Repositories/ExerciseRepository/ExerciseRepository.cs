using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.Logger;

namespace OperationStacked.Repositories
{
    public class ExerciseRepository : RepositoryBase, IExerciseRepository
    {
        public ExerciseRepository(IDbContextFactory<OperationStackedContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<List<WorkoutExercise>> GetWorkoutExercisesByWeekAndDay(Guid userId, int week, int day)
        {
            return await _operationStackedContext.WorkoutExercises
                .Include(we => we.Exercise)
                .Include(we => we.LinearProgressionExercises) // Ensure this is a collection property
                .Where(we => we.Exercise.UserId == userId &&
                             we.LiftDay == day &&
                             we.LinearProgressionExercises.Any(lpe =>
                                 lpe.LiftWeek == week)) // Check any LinearProgressionExercise for the week
                .ToListAsync();
        }

        public async Task<(IEnumerable<WorkoutExercise> exercises, int totalCount)> GetAllWorkoutExercisesWithCount(
            Guid userId, int pageIndex, int pageSize)
        {
            try
            {
                // Count doesn't need any includes since it doesn't return the full entities
                var totalCount = await _operationStackedContext.WorkoutExercises
                    .CountAsync(we => we.Exercise.UserId == userId);

                // Query adjusted for one-to-many relationship
                var exercises = await _operationStackedContext.WorkoutExercises
                    .Include(we => we.Exercise)
                    .Include(we =>
                        we.LinearProgressionExercises) // Adjusted to include the collection of LinearProgressionExercises
                    .Where(we => we.Exercise.UserId == userId)
                    .OrderBy(we => we.WorkoutId)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (exercises, totalCount);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Exercise> GetExerciseById(Guid id) => await _operationStackedContext.Exercises
            .FirstOrDefaultAsync(x => x.Id == id);


        public async Task InsertExercise(Exercise exercise)
        {
            await _operationStackedContext.Exercises.AddAsync(exercise);
            await _operationStackedContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exercise exercise)
        {
            _operationStackedContext.Exercises.Update(exercise);
            await _operationStackedContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAllExercisesForUser(Guid userId)
        {
            var entity = _operationStackedContext.Exercises.Where(x => x.UserId == userId);

            if (!entity.Any())
            {
                return false;
            }

            _operationStackedContext.Exercises.RemoveRange(entity);
            var saveResult = await _operationStackedContext.SaveChangesAsync();

            return
                saveResult >
                0; // if one or more entities were changed (in this case, deleted), SaveChangesAsync will return a positive number
        }

        public async Task<bool> DeleteExercise(Guid exerciseId)
        {
            var entity = _operationStackedContext.Exercises.FirstOrDefault(x => x.Id == exerciseId);

            if (entity == null)
            {
                return false;
            }

            _operationStackedContext.Exercises.Remove(entity);
            var saveResult = await _operationStackedContext.SaveChangesAsync();

            return
                saveResult >
                0; // if one or more entities were changed (in this case, deleted), SaveChangesAsync will return a positive number
        }

        public async Task<LinearProgressionExercise> GetLinearProgressionExerciseByIdAsync(Guid id)
        {
            return await _operationStackedContext.LinearProgressionExercises
                .Include(lp => lp.WorkoutExercise)
                .ThenInclude(we => we.Exercise) // This line will include the Exercise table.
                .FirstOrDefaultAsync(lp => lp.Id == id);
        }

        public async Task InsertLinearProgressionExercise(LinearProgressionExercise nextExercise)
        {
            using var context = _operationStackedContext;

            await context.LinearProgressionExercises.AddAsync(nextExercise);
            await context.SaveChangesAsync();
        }

        public async Task InsertWorkoutExercise(WorkoutExercise workoutExercise)
        {
            await _operationStackedContext.WorkoutExercises.AddAsync(workoutExercise);
            await _operationStackedContext.SaveChangesAsync();
        }

        public async Task InsertExerciseHistory(ExerciseHistory history)
        {
            using var context = _operationStackedContext;
            await context.ExerciseHistory.AddAsync(history);
            await context.SaveChangesAsync();
        }

        public Task<WorkoutExercise> GetWorkoutExerciseById(Guid requestWorkoutExerciseId)
        {
            return _operationStackedContext.WorkoutExercises
                .Include(we => we.Exercise)
                .Include(we => we.LinearProgressionExercises) // Changed to the collection property
                .FirstOrDefaultAsync(we => we.Id == requestWorkoutExerciseId);
        }

        public Task<List<Exercise>> GetAllExercisesByUserId(Guid userId)
        {
            return _operationStackedContext.Exercises.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<List<ExerciseHistory>> GetExerciseHistoryByExerciseId(Guid exerciseId)
        {
            return await _operationStackedContext.ExerciseHistory.Where(e => e.ExerciseId == exerciseId).ToListAsync();
        }

        public async Task<Exercise> UpdateExerciseById(UpdateExerciseRequest request, int weightIndex = -1)
        {
            var exercise = await _operationStackedContext.Exercises.Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync();
            _operationStackedContext.Update(exercise);
            var saveResult = await _operationStackedContext.SaveChangesAsync();

            return exercise;
        }
    }
}
