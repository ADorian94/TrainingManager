using System.Collections.ObjectModel;
using TrainingManager.Model.Navigation.MasterDetailPageItem;
using TrainingManager.View;
using TrainingManager.View.Timer.IntervallTimer;
using TrainingManager.View.WeightWorkout.WeightWorkoutNotes;

namespace TrainingManager.ViewModel
{
    public class MasterDetailNavigationPageMasterViewModel : ViewModelBase
    {
        public ObservableCollection<MasterDetailNavigationPageMenuItem> MenuItems { get; set; }

        public MasterDetailNavigationPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<MasterDetailNavigationPageMenuItem>(new[]
            {
                    new MasterDetailNavigationPageMenuItem { Id = 0, Title = "1RM Calculation", TargetType = typeof(MainPage)},
                    new MasterDetailNavigationPageMenuItem { Id = 1, Title = "Intervall timer", TargetType = typeof(IntervallTimerPage)},
                    new MasterDetailNavigationPageMenuItem { Id = 2, Title = "Exercise timer", TargetType = typeof(ExerciseTimer)},
                    new MasterDetailNavigationPageMenuItem { Id = 3, Title = "Workout notes", TargetType = typeof(CurrentWeightWorkout)},
            });
        }

        protected override void InitializeCommands()
        {
        }
    }
}
