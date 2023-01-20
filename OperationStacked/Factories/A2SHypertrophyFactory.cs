﻿using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Services.A2S;

namespace OperationStacked.Factories
{
    public class A2SHypertrophyFactory : ExerciseFactory<A2SHypertrophyExercise>
    {
        IA2SHypertrophyService _a2sHypertrophyService;
        public A2SHypertrophyFactory(IA2SHypertrophyService a2SHypertrophyService) 
        {
            _a2sHypertrophyService = a2SHypertrophyService;
        }

        public override bool AppliesTo(Type type) => typeof(A2SHypertrophyExercise).Equals(type);
       
        public override A2SHypertrophyExercise CreateExercise(CreateExerciseModel createExerciseModel)
        {
            CreateBaseExercise(createExerciseModel);
            _exercise.TrainingMax = _createExerciseModel.TrainingMax;
            _exercise.AuxillaryLift = _createExerciseModel.AuxillaryLift;
            _exercise.Block = _createExerciseModel.Block;
            _exercise.AmrapRepTarget = _a2sHypertrophyService.GetAmprapRepTarget(
                _createExerciseModel.Block, 
                _createExerciseModel.Week,
                _createExerciseModel.AuxillaryLift);
            _exercise.AmrapRepResult = 0;
            _exercise.Week = _createExerciseModel.Week;
            _exercise.Intensity = _a2sHypertrophyService.GetIntensity(
                _createExerciseModel.Block,
                _createExerciseModel.Week,
                _createExerciseModel.AuxillaryLift);
            _exercise.Sets = _a2sHypertrophyService.GetSets(
              _createExerciseModel.Block,
              _createExerciseModel.Week,
              _createExerciseModel.AuxillaryLift);
            _exercise.RepsPerSet = _a2sHypertrophyService.GetRepsPerSet(
                _createExerciseModel.Block,
                _createExerciseModel.Week,
                _createExerciseModel.AuxillaryLift);
            
            return _exercise;
        }
    }
}
