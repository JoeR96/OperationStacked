using Microsoft.EntityFrameworkCore;
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


        public ExerciseRepository( IDbContextFactory<OperationStackedContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _operationStackedContext = contextFactory.CreateDbContext();
        }

        public async Task<List<Exercise>> GetExercises(Guid userId, int week, int day, bool completed)
        {
            var logger = await CloudWatchLogger.GetLoggerAsync();

            try
            {
                logger.LogMessageAsync("Got workout");
                return completed
                    ? await _operationStackedContext.Exercises
                        .Where(x => x.LiftDay == day && x.LiftWeek == week && x.UserId == userId)
                        .ToListAsync()
                    : await _operationStackedContext.Exercises
                        .Where(x => x.LiftDay == day && x.LiftWeek == week && x.UserId == userId && x.Completed == false)
                        .ToListAsync();
            }
            catch (Exception e)
            {
                logger.LogMessageAsync($"Error {e}");
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

        public async Task<bool> UpdateExerciseById(Guid exerciseId, decimal weight,int weightIndex = -1)
        {
            var exercise = await _operationStackedContext.Exercises.Where(x => x.Id == exerciseId).FirstOrDefaultAsync();
            _operationStackedContext.Update(exercise);
            var saveResult = await _operationStackedContext.SaveChangesAsync();

            return saveResult > 0;
        }
    }
}
