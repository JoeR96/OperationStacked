﻿// using Concise.Steps;
// using NUnit.Framework;
// using OperationStacked.TestLib;
//
// namespace OperationStackedAuth.Tests.UnitTests.RecipeTests;
//
// [TestFixture]
// [NonParallelizable]
// public class RecipeTests
// {
//     public Guid userId;
//     public WorkoutClient recipeClient;
//     
//     [OneTimeSetUp]
//     public async Task InitiateClient()
//     {
//         var (client, _userId) = await ApiClientFactory.CreateWorkoutClientAsync(true);
//         recipeClient = client;
//         userId = _userId;
//     }
//     
//     // [StepTest, Order(10)]
//     // public async Task RecipeCreatedAndPopulated()
//     // {
//     //     await "The recipe is created and returned.".__(async () =>
//     //     {
//     //         var request = new CreateRecipeRequest()
//     //         {
//     //             Name = "Egg and Onion/Potato Hash",
//     //             Steps = new List<string>()
//     //             {
//     //                 "Preheat Oven",
//     //                 "Grate Potato/Onion",
//     //                 "Drain moisture from onion/potato",
//     //                 "Cook 2 min/side",
//     //                 "Crack eggs on top and oven for 6 mins",
//     //                 "Serve"
//     //             }
//     //         };
//     //
//     //         var recipeResponse = await recipeClient.RecipeAsync(request);
//     //     });
//     }
// }