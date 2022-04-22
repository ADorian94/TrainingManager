using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrainingManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.LogWriter;

namespace TrainingManager.ViewModel.Tests
{
    [TestClass()]
    public class WeightHistoryVMTests
    {
        private WeightHistoryVM _historyVM;
        private Mock<IApiServices> _apiService;
        private IEnumerable<WeightWorkoutDTO> _weightWorkouts;
        private readonly IEnumerable<string> _savedActivities = new List<string>() { BICEPS, TRICEPS, SQUAT, TRICEPS_PLATE };

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
                TotalWeight = 100,
                WorkoutImages = null,
                WorkoutType = WorkoutType.WeightWorkout,
                WorkoutName = WORKOUT_NAME,
                WeightExercisesDto = new List<WeightExerciseDTO>()
            } };

            _apiService.Setup(x => x.GetWeightWorkoutsAsync()).Returns(Task.FromResult(_weightWorkouts));
            _apiService.Setup(x => x.EditWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>())).Returns(Task.FromResult(true));
            _apiService.Setup(x => x.GetWeightActivitiesAsync()).Returns(Task.FromResult(_savedActivities));
            _apiService.Setup(x => x.AddWeightWorkoutAsync(It.IsAny<WeightWorkoutDTO>())).Returns(Task.FromResult(true));
            LogHandler.InitializeLogPath("./");
        }

        [TestMethod()]
        [Ignore()]
        public void WeightHistoryVMTest()
        {
            _historyVM = new WeightHistoryVM(_apiService.Object);
            _apiService.Verify(x => x.GetWeightWorkoutsAsync(), Times.Once);
            Assert.AreEqual(100, _historyVM.MovedWeightsInTheMonth);
            Assert.AreEqual(1, _historyVM.HistoryWorkoutItems.Count);
        }

        [TestMethod()]
        [Ignore()]
        public void SelectExistingWorkoutTest()
        {
            var selectedDate = DateTime.Now.ToUniversalTime();
            _historyVM = new WeightHistoryVM(_apiService.Object);
            _historyVM.WorkoutDateSelected.Execute(selectedDate);
            Assert.AreEqual(selectedDate.Date, _historyVM.NewWeightWorkout.WorkoutDate.Date);
            Assert.AreEqual(_weightWorkouts.First(), _historyVM.NewWeightWorkout);
        }

        [TestMethod()]
        [Ignore()]
        public void DeleteWeightWorkoutByStringGuidTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        [Ignore()]
        public void SearchFunctionTest()
        {
            Assert.Fail();
        }
    }
}