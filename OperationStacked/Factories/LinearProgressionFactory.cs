using Microsoft.EntityFrameworkCore;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Factories
{
    public class LinearProgressionFactory : ExerciseFactory<LinearProgressionExercise>
    {
        public LinearProgressionFactory(IExerciseRepository exerciseRepository) : base(exerciseRepository)
        {
        }

        public override bool AppliesTo(Type type) => typeof(LinearProgressionExercise).Equals(type);

        public override LinearProgressionExercise CreateExercise(CreateExerciseModel createExerciseModel)
        {
            CreateBaseExercise(createExerciseModel);
            _exercise.MinimumReps = _createExerciseModel.MinimumReps;
            _exercise.MaximumReps = _createExerciseModel.MaximumReps;
            _exercise.TargetSets = _createExerciseModel.TargetSets;
            _exercise.WeightIndex = _createExerciseModel.WeightIndex;
            _exercise.PrimaryExercise = _createExerciseModel.PrimaryExercise;
            _exercise.StartingWeight = _createExerciseModel.StartingWeight;
            _exercise.WeightProgression = _createExerciseModel.WeightProgression;
            _exercise.AttemptsBeforeDeload = _createExerciseModel.AttemptsBeforeDeload;


            return _exercise;
        }

        public async override Task<(Exercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request)
        {
            var exercise = (LinearProgressionExercise)await _exerciseRepository.GetExerciseById(request.Id);
            exercise.Completed = true;
            await _exerciseRepository.UpdateAsync(exercise);
            ExerciseCompletedStatus status = ExerciseCompletedStatus.Active;
            int weightIndexModifier = 0;
            int attemptModifier = 0;
            //if rep target  and set count reached
            //Increase weight index
            if (exercise.SetCountReached(request.Sets) &&
                exercise.TargetRepCountReached(request.Reps) &&
                exercise.WithinRepRange(request.Reps))
            {
                //get next weeks exercise
                //increase weight index
                //save game
                status = ExerciseCompletedStatus.Progressed;
                weightIndexModifier++;
            }
            else if (!exercise.TargetRepCountReached(request.Reps) &&
                exercise.SetCountReached(request.Sets) &&
                exercise.WithinRepRange(request.Reps))
            {
                status = ExerciseCompletedStatus.StayedTheSame;

            }
            else if (!exercise.TargetRepCountReached(request.Reps) ||
                !exercise.SetCountReached(request.Sets))
            {
                if (exercise.IsLastAttemptBeforeDeload())
                {
                    status = ExerciseCompletedStatus.Deload;
                    weightIndexModifier--;
                }
                else
                {
                    status = ExerciseCompletedStatus.Failed;
                    attemptModifier++;
                }
            }
            else
            {
                status = ExerciseCompletedStatus.Failed;
                //Log to the logger here
                //We should not have got here
            }
            var nextExercise = exercise.GenerateNextExercise( weightIndexModifier, attemptModifier);
            
            await _exerciseRepository.InsertExercise(nextExercise); 
            return (nextExercise, status);
        }
    }
}
