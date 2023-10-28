using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.Logger;

namespace OperationStacked.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private OperationStackedContext _operationStackedContext;
        private readonly IDbContextFactory<OperationStackedContext> _contextFactory;
        private readonly ICloudWatchLogger _logger;

        public ExerciseRepository( IDbContextFactory<OperationStackedContext> contextFactory, ICloudWatchLogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _operationStackedContext = contextFactory.CreateDbContext();
        }

        public async Task<List<WorkoutExercise>> GetWorkoutExercisesByWeekAndDay(Guid userId, int week, int day)
        {
            return await _operationStackedContext.WorkoutExercises
                .Include(we => we.Exercise) 
                .Include(we => we.LinearProgressionExercise) 
                .Where(we => we.Exercise.UserId == userId &&  
                             we.LinearProgressionExercise.LiftWeek == week &&
                             we.LiftDay == day)
                .ToListAsync();
        }




        public async Task<List<LinearProgressionExercise>> GetAllExercisesAndUpdateCompletedReps(Guid userId)
        {
            try
            {
                await _logger.LogMessageAsync("Starting to fetch and update exercises!");

                // Fetch all exercises for the given userId
                var exercises = await _operationStackedContext.LinearProgressionExercises
                    .Where(x => x.WorkoutExercise.Exercise.UserId  == userId)
                    .ToListAsync();

                var random = new Random();  // Create a random object to generate random numbers

                foreach (var exercise in exercises)
                {

                        // Generate randomized set values between minimum and maximum reps
                        var setsValues = Enumerable.Range(0, exercise.Sets)
                            .Select(_ => random.Next(exercise.MinimumReps, exercise.MaximumReps + 1))
                            .ToArray();
                }

                // Update the changes in the context
                _operationStackedContext.UpdateRange(exercises);
                await _operationStackedContext.SaveChangesAsync();

                return exercises;  // Return the updated exercises
            }
            catch (Exception e)
            {
                await _logger.LogMessageAsync($"Error {e}");
                throw;
            }
        }
        public async Task<(IEnumerable<WorkoutExercise> exercises, int totalCount)> GetAllWorkoutExercisesWithCount(Guid userId, int pageIndex, int pageSize)
        {
            try
            {
                var totalCount = await _operationStackedContext.WorkoutExercises
                    .CountAsync(we => we.Exercise.UserId == userId);
    
                var exercises = await _operationStackedContext.WorkoutExercises
                    .Include(we => we.Exercise)
                    .Include(we => we.LinearProgressionExercise)
                    .Where(we => we.Exercise.UserId == userId)
                    .OrderBy(we => we.WorkoutId)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (exercises, totalCount);
            }
            catch (Exception e)
            {
                await _logger.LogMessageAsync($"Error {e}");
                throw;
            }
        }


        public async Task<Exercise> GetExerciseById(Guid id) => await _operationStackedContext.Exercises
        .FirstOrDefaultAsync(x => x.Id == id);




        public async Task InsertExercise(Exercise nextExercise)
        {
            await _operationStackedContext.Exercises.AddAsync(nextExercise);
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

            return saveResult > 0; // if one or more entities were changed (in this case, deleted), SaveChangesAsync will return a positive number
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

            return saveResult > 0; // if one or more entities were changed (in this case, deleted), SaveChangesAsync will return a positive number
        }

        public async Task<EquipmentStackResponse> InsertEquipmentStack(CreateEquipmentStackRequest equipmentStack)
        {
            var e = new EquipmentStack()
            {
                IncrementCount = equipmentStack.IncrementCount,
                EquipmentStackKey = equipmentStack.EquipmentStackKey,
                IncrementValue = equipmentStack.IncrementValue,
                InitialIncrements = equipmentStack.InitialIncrements,
                UserID = equipmentStack.UserID,
                StartWeight = equipmentStack.StartWeight,
                
            };
            _operationStackedContext = await _contextFactory.CreateDbContextAsync();
            _operationStackedContext.EquipmentStacks.Add(e);
            await _operationStackedContext.SaveChangesAsync();
            return new EquipmentStackResponse(e);
        }

        public async Task<EquipmentStack> GetEquipmentStack(Guid equipmentStackId)
        {
            try 
            {
                EquipmentStack equipmentStack;
                using (var context = _contextFactory.CreateDbContext())
                {
                    // use 'context' here
                    equipmentStack = await context.EquipmentStacks.FirstOrDefaultAsync(x => x.Id == equipmentStackId);
                }
                return equipmentStack;
            }
            catch (Exception e)
            {
                // Log or print the exception
                Console.WriteLine(e);
                throw; // rethrowing the exception
            }
        }
        
        public async Task<List<EquipmentStack>> GetAllEquipmentStacks(Guid userId)
        {
            try 
            {
                List<EquipmentStack> equipmentStack;
                using (var context = _contextFactory.CreateDbContext())
                {
                    equipmentStack = await context.EquipmentStacks.Where(x => x.UserID == userId).ToListAsync();
                }
                return equipmentStack;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw; 
            }
        }

        public Task<LinearProgressionExercise> GetLinearProgressionExerciseById(Guid id)
        {
            return _operationStackedContext.LinearProgressionExercises.Where(lp => lp.Id == id).FirstOrDefaultAsync();
        }

        public async Task InsertLinearProgressionExercise(LinearProgressionExercise nextExercise)
        {
            await _operationStackedContext.LinearProgressionExercises.AddAsync(nextExercise);
        }

        public async Task InsertWorkoutExercise(WorkoutExercise workoutExercise)
        {
            await _operationStackedContext.WorkoutExercises.AddAsync(workoutExercise);
        }

        public async Task InsertExerciseHistory(ExerciseHistory history)
        {
            await _operationStackedContext.ExerciseHistory.AddAsync(history);
        }


        public async Task<bool> DeleteEquipmentStack(Guid equipmentStackId)
        {
            if (equipmentStackId == null)
            {
                throw new ArgumentNullException(nameof(equipmentStackId));
            }

            var equipmentStack = await _operationStackedContext.EquipmentStacks.FirstOrDefaultAsync(x => x.Id == equipmentStackId);

            if (equipmentStack == null)
            {
                throw new KeyNotFoundException($"No EquipmentStack was found with the ID {equipmentStackId}");
            }

            _operationStackedContext.EquipmentStacks.Remove(equipmentStack);
            var saveResult = await _operationStackedContext.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<Exercise> UpdateExerciseById(UpdateExerciseRequest request,int weightIndex = -1)
        {
            var exercise = await _operationStackedContext.Exercises.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            _operationStackedContext.Update(exercise);
            var saveResult = await _operationStackedContext.SaveChangesAsync();

            return exercise;
        }
    }
}
