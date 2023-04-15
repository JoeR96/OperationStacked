using OperationStacked.Entities;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Services.A2S;

namespace OperationStacked.Factories
{
    public class A2SHypertrophyFactory : ExerciseFactory<A2SHypertrophyExercise>
    {

        IA2SHypertrophyService _a2sHypertrophyService;
        public A2SHypertrophyFactory(IA2SHypertrophyService a2SHypertrophyService, IExerciseRepository exerciseRepository) : base(exerciseRepository)
        {
            _a2sHypertrophyService = a2SHypertrophyService;
        }

        public override bool AppliesTo(Type type) => typeof(A2SHypertrophyExercise).Equals(type);
       
        public override A2SHypertrophyExercise CreateExercise(CreateExerciseModel createExerciseModel)
        {
            CreateBaseExercise(createExerciseModel);
            _exercise.TrainingMax = _createExerciseModel.TrainingMax;
            _exercise.PrimaryLift = _createExerciseModel.PrimaryLift;
            _exercise.Block = _createExerciseModel.Block;

            _exercise.WorkingWeight = _a2sHypertrophyService.GetWorkingWeight(
                _createExerciseModel.Block,
                _createExerciseModel.Week,
                _createExerciseModel.PrimaryLift,
                _createExerciseModel.TrainingMax,
                _createExerciseModel.WeightProgression);

            _exercise.AmrapRepTarget = _a2sHypertrophyService.GetAmprapRepTarget(
                _createExerciseModel.Block, 
                _createExerciseModel.Week,
                _createExerciseModel.PrimaryLift);
            _exercise.AmrapRepResult = 0;
            _exercise.Week = _createExerciseModel.Week;

            _exercise.Intensity = _a2sHypertrophyService.GetIntensity(
                _createExerciseModel.Block,
                _createExerciseModel.Week,
                _createExerciseModel.PrimaryLift);

            _exercise.Sets = _a2sHypertrophyService.GetSets(
              _createExerciseModel.Block,
              _createExerciseModel.Week,
              _createExerciseModel.PrimaryLift);

            _exercise.RepsPerSet = _a2sHypertrophyService.GetRepsPerSet(
                _createExerciseModel.Block,
                _createExerciseModel.Week,
                _createExerciseModel.PrimaryLift);
            _exercise.RoundingValue = createExerciseModel.WeightProgression;

            return _exercise;
        }

        public async override Task<(Exercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request)
        {
            var exercise = (A2SHypertrophyExercise)await _exerciseRepository.GetExerciseById(request.Id);
            exercise.Completed = true;

            await _exerciseRepository.UpdateAsync(exercise);
            ExerciseCompletedStatus status = ExerciseCompletedStatus.Active;

            int updateModifier = 0;
            Math.Clamp(updateModifier, -2, 5);

            updateModifier = (request.Reps.FirstOrDefault() - exercise.AmrapRepTarget);
            var trainingMax = UpdateTrainingMax(exercise.TrainingMax, updateModifier);
            Math.Clamp(updateModifier, -2, 5);
            var (a2sWeek, block) = _a2sHypertrophyService.GetNextWeekAndBlock(exercise.Block, exercise.Week);

            var nextWeekWorkingWeight = _a2sHypertrophyService.GetWorkingWeight(block,
                a2sWeek,
                exercise.PrimaryLift,
                trainingMax,
                exercise.RoundingValue);

            var nextExercise = exercise.GenerateNextExercise(trainingMax, nextWeekWorkingWeight, block, a2sWeek,
                _a2sHypertrophyService.GetAmprapRepTarget(block, a2sWeek, exercise.PrimaryLift),
                _a2sHypertrophyService.GetIntensity(block, a2sWeek, exercise.PrimaryLift),
                _a2sHypertrophyService.GetRepsPerSet(block, a2sWeek, exercise.PrimaryLift),
                exercise.Week, exercise.UserId);
            nextExercise.Week = a2sWeek; 
            await _exerciseRepository.InsertExercise(nextExercise);

            return (nextExercise, status);
        }
        internal decimal UpdateTrainingMax(decimal trainingMax, int updateModifier)
        {
            decimal modifier;
            switch (updateModifier)
            {
                case -2: modifier = -5m; break;
                case -1: modifier = -2m; break;
                case 0: modifier = 0m; break;
                case 1: modifier = 0.5m; break;
                case 2: modifier = 1m; break;
                case 3: modifier = 1.5m; break;
                case 4: modifier = 2m; break;
                case 5: modifier = 3m; break;
                default: modifier = 0m; break;
            }

            return trainingMax / 100 * (100 + modifier);
        }

    }
}
