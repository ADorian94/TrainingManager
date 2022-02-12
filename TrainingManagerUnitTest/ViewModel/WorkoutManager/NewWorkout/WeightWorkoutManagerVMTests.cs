using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TrainingManager.Model;
using TrainingManager.Data.DTO;
using System.Collections.ObjectModel;
using TrainingManager.Model.LogWriter;
using System.IO;

namespace TrainingManager.ViewModel.Tests
{
    [TestClass()]
    public class WeightWorkoutManagerVMTests
    {
        //FIELDS
        private WeightWorkoutManagerVM _weightWorkoutManagerVM;
        private Mock<IApiServices> _apiService;
        private IEnumerable<WeightWorkoutDTO> _weightWorkouts;

        private const string WORKOUT_NAME = "TestWorkout";
        private const string EXERCISE_NAME = "TestExercise";

        [TestInitialize]
        public void InitializeTest()
        {
            _apiService = new Mock<IApiServices>();
            _weightWorkouts = new List<WeightWorkoutDTO>() { new WeightWorkoutDTO()
            {
                WorkoutDate = DateTime.Now,
                WorkoutGuid = Guid.NewGuid(),
                Id = 1,
                Note = string.Empty,
                TotalWeight = 0,
                WorkoutImages = null,
                WorkoutType = WorkoutType.WeightWorkout,
                WorkoutName = WORKOUT_NAME,
                WeightExercisesDto = new List<WeightExerciseDTO>()
            } };

            _apiService.Setup(x => x.GetWeightWorkoutsAsync()).Returns(Task.FromResult(_weightWorkouts));
            _apiService.Setup(x => x.EditWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>())).Returns(Task.FromResult(true));
            LogHandler.InitializeLogPath("./");
        }

        [TestCleanup]
        public void CleanUpLogFiles()
        {
            if (File.Exists(Path.Combine(LogHandler.LogPathDirectory, "logs", "LiftIt.log")))
                Directory.Delete(Path.Combine(LogHandler.LogPathDirectory, "logs"), true);
        }

        //SETUP TESTS
        [TestMethod()]
        public void SetupTodayWeightWorkoutEmptyTest()
        {
            var emptyWorkout = new WeightWorkoutVM()
            {
                Note = string.Empty,
                TotalExerciseRounds = 0.0,
                TotalWeight = 0.0,
                WorkoutDate = DateTime.Now,
                WorkoutGuid = Guid.NewGuid(),
                WorkoutName = string.Empty,
                WorkoutType = WorkoutType.WeightWorkout,
                WeightExercises = new ObservableCollection<WeightExerciseVM>(),
            };

            IEnumerable<WeightWorkoutDTO> weightWorkouts = new List<WeightWorkoutDTO>();
            _apiService.Setup(x => x.GetWeightWorkoutsAsync()).Returns(Task.FromResult(weightWorkouts));
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            Assert.AreEqual(emptyWorkout, _weightWorkoutManagerVM.NewWeightWorkout);
        }

        [TestMethod()]
        public void SetupTodayWeightWorkoutTest()
        {
            var emptyWorkout = new WeightWorkoutVM()
            {
                Note = string.Empty,
                TotalExerciseRounds = 0.0,
                TotalWeight = 0.0,
                WorkoutDate = DateTime.Now,
                WorkoutGuid = Guid.NewGuid(),
                WorkoutName = WORKOUT_NAME,
                WorkoutType = WorkoutType.WeightWorkout,
                WeightExercises = new ObservableCollection<WeightExerciseVM>(),
            };

            _apiService.Setup(x => x.GetWeightWorkoutsAsync()).Returns(Task.FromResult(_weightWorkouts));
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            Assert.AreEqual(emptyWorkout, _weightWorkoutManagerVM.NewWeightWorkout);
        }

        //BOOKMARK TESTS
        [TestMethod()]
        public void WeightWorkoutBookmarkEqualsTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            Assert.IsTrue(_weightWorkoutManagerVM.WeightWorkoutBookmark == _weightWorkoutManagerVM.NewWeightWorkout);
        }

        [TestMethod()]
        public void WeightWorkoutBookmarkNotEqualsTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            var exercise = new WeightExerciseVM()
            {
                ExerciseGuid = Guid.NewGuid(),
                ExerciseName = EXERCISE_NAME,
                ExerciseNote = string.Empty,
                WeightRounds = new ObservableCollection<WeightRoundVM>()
                {
                    new WeightRoundVM()
                    {
                        WeightOfExercise = 20,
                        Reps = 12,
                        RoundGuid = Guid.NewGuid(),
                        RoundNumber = 0,
                    }
                }
            };

            exercise.CountTotalWeightOfExercise();
            _weightWorkoutManagerVM.NewWeightExercise = exercise;
            _weightWorkoutManagerVM.AddWeightExerciseToWorkoutCommand.Execute(null);
            Assert.IsFalse(_weightWorkoutManagerVM.WeightWorkoutBookmark == _weightWorkoutManagerVM.NewWeightWorkout);
        }

        [TestMethod()]
        public void WeightWorkoutBookmarkSaveTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            var exercise = new WeightExerciseVM()
            {
                ExerciseGuid = Guid.NewGuid(),
                ExerciseName = EXERCISE_NAME,
                ExerciseNote = string.Empty,
                WeightRounds = new ObservableCollection<WeightRoundVM>()
                {
                    new WeightRoundVM()
                    {
                        WeightOfExercise = 20,
                        Reps = 12,
                        RoundGuid = Guid.NewGuid(),
                        RoundNumber = 0,
                    }
                }
            };

            exercise.CountTotalWeightOfExercise();
            _weightWorkoutManagerVM.NewWeightExercise = exercise;
            _weightWorkoutManagerVM.AddWeightExerciseToWorkoutCommand.Execute(null);
            _weightWorkoutManagerVM.SaveTodayWorkoutCommand.Execute(null);
            Assert.IsTrue(_weightWorkoutManagerVM.WeightWorkoutBookmark == _weightWorkoutManagerVM.NewWeightWorkout);
        }
    }
}