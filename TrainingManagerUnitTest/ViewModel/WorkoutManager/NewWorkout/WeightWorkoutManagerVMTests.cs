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
using System.Linq;

namespace TrainingManager.ViewModel.Tests
{
    [TestClass()]
    public class WeightWorkoutManagerVMTests
    {
        //FIELDS
        private WeightWorkoutManagerVM _weightWorkoutManagerVM;
        private Mock<IApiServices> _apiService;
        private IEnumerable<WeightWorkoutDTO> _weightWorkouts;
        private readonly IEnumerable<WeightActivityDTO> _savedActivities = new List<WeightActivityDTO>()
        {
            new WeightActivityDTO() { ActivityName = BICEPS, MainMuscleGroup = Data.Muscle.Biceps },
            new WeightActivityDTO() { ActivityName = TRICEPS, MainMuscleGroup = Data.Muscle.Triceps },
            new WeightActivityDTO() { ActivityName = SQUAT, MainMuscleGroup = Data.Muscle.Quadriceps },
            new WeightActivityDTO() { ActivityName = TRICEPS_PLATE, MainMuscleGroup = Data.Muscle.Triceps }
        };

        private const string WORKOUT_NAME = "TestWorkout";
        private const string EXERCISE_NAME = "TestExercise";
        private const string BICEPS = "Biceps";
        private const string TRICEPS = "Triceps";
        private const string TRICEPS_PLATE = "Triceps with plate";
        private const string SQUAT = "Squat";

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
            _apiService.Setup(x => x.GetWeightActivitiesAsync()).Returns(Task.FromResult(_savedActivities));
            _apiService.Setup(x => x.AddWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>())).Returns(Task.FromResult(true)); ;
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

            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            Assert.AreEqual(emptyWorkout, _weightWorkoutManagerVM.NewWeightWorkout);
            CollectionAssert.AreEqual(new List<WeightActivityDTO>(_savedActivities.OrderBy(x => x)), _weightWorkoutManagerVM.SavedActivities);
        }

        //ADD & DELETE
        [TestMethod()]
        public void AddAndDeleteExerciseTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            var id = Guid.NewGuid();

            _weightWorkoutManagerVM.NewWeightExercise = new WeightExerciseVM()
            {
                ExerciseName = EXERCISE_NAME,
                ExerciseGuid = id,
                WeightRounds = new ObservableCollection<WeightRoundVM>()
                {
                    new WeightRoundVM()
                    {
                        WeightOfExercise = 10,
                        Reps = 1
                    }
                }
            };

            _weightWorkoutManagerVM.NewWeightExercise.CountTotalWeightOfExercise();
            _weightWorkoutManagerVM.AddWeightExerciseToWorkoutCommand.Execute(null);

            Assert.AreEqual(1, _weightWorkoutManagerVM.NewWeightWorkout.WeightExercises.Count);
            Assert.AreEqual(10, _weightWorkoutManagerVM.NewWeightWorkout.TotalWeight);
            Assert.AreEqual(1, _weightWorkoutManagerVM.NewWeightExercise.TotalExerciseRounds);
            Assert.IsTrue(_weightWorkoutManagerVM.HasAnyChanges);

            _weightWorkoutManagerVM.DeleteExercise(id.ToString());
            Assert.AreEqual(0, _weightWorkoutManagerVM.NewWeightWorkout.WeightExercises.Count);
            Assert.AreEqual(0, _weightWorkoutManagerVM.NewWeightWorkout.TotalWeight);
            Assert.AreEqual(0, _weightWorkoutManagerVM.NewWeightExercise.TotalExerciseRounds);
            Assert.IsFalse(_weightWorkoutManagerVM.HasAnyChanges);
        }

        //ROUND
        [TestMethod()]
        public void AddAndDuplicateRoundToExerciseTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            _weightWorkoutManagerVM.OpenAddWeightExerciseCommand.Execute(null);
            Assert.AreEqual(1, _weightWorkoutManagerVM.NewWeightExercise.WeightRounds.Count);
            _weightWorkoutManagerVM.AddWeightRoundToExerciseCommand.Execute(null);
            Assert.AreEqual(2, _weightWorkoutManagerVM.NewWeightExercise.WeightRounds.Count);
            _weightWorkoutManagerVM.DuplicateRoundByStringGuid(_weightWorkoutManagerVM.NewWeightExercise.WeightRounds.First().RoundGuid.ToString());
            Assert.AreEqual(3, _weightWorkoutManagerVM.NewWeightExercise.WeightRounds.Count);
        }

        [TestMethod()]
        public void DeleteRoundToExerciseTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            _weightWorkoutManagerVM.OpenAddWeightExerciseCommand.Execute(null);
            Assert.AreEqual(1, _weightWorkoutManagerVM.NewWeightExercise.WeightRounds.Count);
            _weightWorkoutManagerVM.DeleteRoundByStringGuid(_weightWorkoutManagerVM.NewWeightExercise.WeightRounds.First().RoundGuid.ToString());
            Assert.AreEqual(0, _weightWorkoutManagerVM.NewWeightExercise.WeightRounds.Count);
        }

        //SAVE & EDIT
        [TestMethod()]
        public void SaveTodayWorkoutAddTest()
        {
            IEnumerable<WeightWorkoutDTO> workouts = new List<WeightWorkoutDTO>();
            _apiService.Setup(x => x.GetWeightWorkoutsAsync()).Returns(Task.FromResult(workouts));
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            _weightWorkoutManagerVM.NewWeightWorkout = new WeightWorkoutVM()
            {
                WorkoutName = WORKOUT_NAME,
                TotalWeight = 10,
                WeightExercises = new ObservableCollection<WeightExerciseVM>()
                {
                    new WeightExerciseVM()
                    {
                        ExerciseName = EXERCISE_NAME,
                        WeightRounds = new ObservableCollection<WeightRoundVM>()
                        {
                            new WeightRoundVM()
                            {
                                WeightOfExercise = 10,
                                Reps = 1
                            }
                        }
                    }
                }
            };

            _weightWorkoutManagerVM.SaveTodayWorkoutCommand.Execute(null);
            _apiService.Verify(x => x.AddWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>()), Times.Once);
            _apiService.Verify(x => x.EditWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>()), Times.Never);
        }

        [TestMethod()]
        public void SaveTodayWorkoutEditTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);
            _weightWorkoutManagerVM.NewWeightWorkout = new WeightWorkoutVM()
            {
                WorkoutName = WORKOUT_NAME,
                TotalWeight = 10,
                WeightExercises = new ObservableCollection<WeightExerciseVM>()
                {
                    new WeightExerciseVM()
                    {
                        ExerciseName = EXERCISE_NAME,
                        WeightRounds = new ObservableCollection<WeightRoundVM>()
                        {
                            new WeightRoundVM()
                            {
                                WeightOfExercise = 10,
                                Reps = 1
                            }
                        }
                    }
                }
            };

            _weightWorkoutManagerVM.SaveTodayWorkoutCommand.Execute(null);
            _apiService.Verify(x => x.EditWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>()), Times.Once);
            _apiService.Verify(x => x.AddWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>()), Times.Never);
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

        //SEARCH
        [TestMethod()]
        public void SearchFunctionTest()
        {
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiService.Object);

            //ONE STR 
            _weightWorkoutManagerVM.SearchCommand.Execute(BICEPS);
            CollectionAssert.AreEqual(new List<string>() { BICEPS }, _weightWorkoutManagerVM.SavedActivities);

            //SUB STR  
            _weightWorkoutManagerVM.SearchCommand.Execute(TRICEPS);
            CollectionAssert.AreEqual(new List<string>() { TRICEPS, TRICEPS_PLATE }, _weightWorkoutManagerVM.SavedActivities);

            //NOT FOUND
            _weightWorkoutManagerVM.SearchCommand.Execute("EMPTY");
            CollectionAssert.AreEqual(new List<string>(), _weightWorkoutManagerVM.SavedActivities);
        }
    }
}