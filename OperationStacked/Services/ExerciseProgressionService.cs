using FluentResult;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services
{
    public class ExerciseProgressionService : IExerciseProgressionService
    {
        OperationStackedContext _operationStackedContext;
        public ExerciseProgressionService(OperationStackedContext operationStackedContext)
        {
            _operationStackedContext = operationStackedContext;
        }

        public async Task<Result<ExerciseCompletionResult>> CompleteExercise(CompleteExerciseRequest request)
        {
            ExerciseCompletedStatus status;
            var exercise = GetExercise(request.Id);
            int weightIndexModifier = 0;
            int attemptModifier = 0;
            //if rep target  and set count reached
            //Increase weight index
            if(exercise.SetCountReached(request.Sets) &&
                exercise.TargetRepCountReached(request.Reps))
            {
                //get next weeks exercise
                //increase weight index
                //save game
                status = ExerciseCompletedStatus.Progressed;
                weightIndexModifier++;
            }
            else if(!exercise.TargetRepCountReached(request.Reps) && 
                exercise.SetCountReached(request.Sets) && 
                exercise.WithinRepRange(request.Reps))
            {
                status = ExerciseCompletedStatus.StayedTheSame;

            }
            else if((!exercise.TargetRepCountReached(request.Reps) ||
                !exercise.SetCountReached(request.Sets)))
            {
                if(exercise.IsLastAttemptBeforeDeload())
                {
                    status = ExerciseCompletedStatus.Deload;
                    weightIndexModifier--;
                }
                else
                {
                    status = ExerciseCompletedStatus.Failed;
                    attemptModifier ++;
                }
            }
            else
            {
                status = ExerciseCompletedStatus.Failed;
                //Log to the logger here
                //We should not have got here
            }
            var nextExercise = exercise.GenerateNextExercise(weightIndexModifier,attemptModifier);
            await _operationStackedContext.LinearProgressionExercises.AddAsync(nextExercise);
            await _operationStackedContext.SaveChangesAsync();
            return Result.Create(new ExerciseCompletionResult(status, nextExercise));
        }

        private LinearProgressionExercise GetExercise(Guid id)
            => _operationStackedContext
                .LinearProgressionExercises
                .Where(x => x.Id == id)
                .FirstOrDefault();
    }

    public static class LinearProgressExtensions
    {
        public static bool SetCountReached(this LinearProgressionExercise e, int sets)
            => sets >= e.TargetSets ? true : false;
        public static bool TargetRepCountReached(this LinearProgressionExercise e, int reps)
            => reps >= e.MaximumReps ? true : false;
        public static bool WithinRepRange(this LinearProgressionExercise e, int reps)
            => Enumerable.Range(e.MinimumReps, 999).Contains(reps);
        public static bool IsLastAttemptBeforeDeload(this LinearProgressionExercise e)
            => e.CurrentAttempt >= e.AttemptsBeforeDeload;
        public static LinearProgressionExercise GenerateNextExercise(this LinearProgressionExercise e, int weightIndex, int attemptModifier)
            => new LinearProgressionExercise
            {
                MinimumReps = e.MinimumReps,
                MaximumReps = e.MaximumReps,
                TargetSets = e.TargetSets,
                StartingSets = e.TargetSets,
                CurrentSets = e.CurrentSets,
                WeightIndex = e.WeightIndex += weightIndex,
                PrimaryExercise = e.PrimaryExercise,
                StartingWeight = e.StartingWeight,
                WeightProgression = e.WeightProgression,
                AttemptsBeforeDeload = e.AttemptsBeforeDeload,
                ExerciseName = e.ExerciseName,
                Username = e.Username,
                Category = e.Category,
                Template = e.Template,
                LiftDay = e.LiftDay,
                LiftOrder = e.LiftOrder,
                LiftWeek = e.LiftWeek += 1,
                ParentId = e.Id,
                CurrentAttempt = e.CurrentAttempt += attemptModifier,
                WorkingWeight = WorkingWeight(e.WorkingWeight,weightIndex,e.WeightProgression),
                EquipmentType = e.EquipmentType,
            };

        private static decimal WorkingWeight(decimal workingWeight, int weightIndex, decimal weightProgression)
            => workingWeight + (weightIndex * weightProgression);
    }
}
