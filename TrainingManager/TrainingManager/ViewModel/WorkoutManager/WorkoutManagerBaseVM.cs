using System;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    public abstract class WorkoutManagerBaseVM : ViewModelBase
    {
        //FIELDS
        protected IApiServices ApiServices;

        //PROPERTIES
        private WeightWorkoutVM _newWeightWorkout;
        public WeightWorkoutVM NewWeightWorkout { get => _newWeightWorkout; set { _newWeightWorkout = value; OnPropertyChanged(); } }

        private WeightExerciseVM _newWeightExercise;
        public WeightExerciseVM NewWeightExercise { get => _newWeightExercise; set { _newWeightExercise = value; OnPropertyChanged(); } }

        private WeightRoundVM _newWeightRound;
        public WeightRoundVM NewWeightRound { get => _newWeightRound; set { _newWeightRound = value; OnPropertyChanged(); } }

        private string _workoutName;
        public string WorkoutName { get => _workoutName; set { _workoutName = value; OnPropertyChanged(); } }

        private string _exerciseName;
        public string ExerciseName { get => _exerciseName; set { _exerciseName = value; OnPropertyChanged(); } }

        private double _weightOfExercise;
        public double WeightOfExercise { get => _weightOfExercise; set { _weightOfExercise = value; OnPropertyChanged(); } }

        private int _reps;
        public int Reps { get => _reps; set { _reps = value; OnPropertyChanged(); } }

        private Guid _exerciseGuid;
        public Guid ExerciseGuid { get => _exerciseGuid; set { _exerciseGuid = value; OnPropertyChanged(); } }

        private string _exercieNote;
        public string ExerciseNote { get => _exercieNote; set { _exercieNote = value; OnPropertyChanged(); } }

        private double _totalWeight;
        public double TotalWeight { get => _totalWeight; set { _totalWeight = value; OnPropertyChanged(); } }

        private double _totalExerciseWeight;
        public double TotalExerciseWeight { get => _totalExerciseWeight; set { _totalExerciseWeight = value; OnPropertyChanged(); } }

        private double _totalExerciseRounds;
        public double TotalExerciseRounds { get => _totalExerciseRounds; set { _totalExerciseRounds = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _savedActivities;
        public ObservableCollection<string> SavedActivities { get => _savedActivities; set { _savedActivities = value; OnPropertyChanged(); } }

        //COMMANDS
        public DelegateCommand SaveTodayWorkoutCommand { get; private set; }
        public DelegateCommand OpenAddWeightExerciseCommand { get; private set; }
        public DelegateCommand AddWeightExerciseToWorkoutCommand { get; private set; }
        public DelegateCommand AddWeightRoundToExerciseCommand { get; private set; }
        public DelegateCommand OpenNoteEditorCommand { get; private set; }
        public DelegateCommand SaveNoteCommand { get; private set; }
        public DelegateCommand WeightExerciseMenuSelectedCommand { get; private set; }
        public DelegateCommand SavedActivitiySelected { get; private set; }
        public DelegateCommand ExerciseRoundSelectedCommand { get; private set; }

        //EVENTS
        public event EventHandler OpenAddWeightExercise;
        public event EventHandler OpenEditWeightExercise;
        public event EventHandler<ClosePageEventArgs> CloseAddWeightExercise;
        public event EventHandler OpenNoteEditor;
        public event EventHandler<ClosePageEventArgs> CloseNoteEditor;
        public event EventHandler<MessageEventArgs> WeightExerciseMenuSelected;
        public event EventHandler<string> SavedWeightActivitySelected;
        public event EventHandler<string> ExerciseRoundSelected;
        public event EventHandler WorkoutSaved;

        protected override void InitializeCommands()
        {
            SaveTodayWorkoutCommand = new DelegateCommand(SaveTodayWorkoutFunctionAsync);
            OpenAddWeightExerciseCommand = new DelegateCommand(OpenAddWeightExerciseFunction);
            AddWeightExerciseToWorkoutCommand = new DelegateCommand(AddWeightExerciseToWorkoutFunction);
            AddWeightRoundToExerciseCommand = new DelegateCommand(AddWeightRoundToExerciseFunction);
            OpenNoteEditorCommand = new DelegateCommand(OpenNoteEditorFuncton);
            WeightExerciseMenuSelectedCommand = new DelegateCommand(WeightExerciseMenuSelectedFunction);
            SavedActivitiySelected = new DelegateCommand(SavedActivitiySelectedFunction);
            SaveNoteCommand = new DelegateCommand(SaveNoteFunction);
            ExerciseRoundSelectedCommand = new DelegateCommand(ExerciseRoundSelectedFunction);
        }

        //ABSTRACT FUNCTIONS
        protected abstract void SaveTodayWorkoutFunctionAsync(object obj);
        public abstract void RefreshWorkouts(object sender, EventArgs e);

        //PROTECTED FUNCTIONS
        //protected void SendExceptionPopUp(object obj, MessageEventArgs args) => ExceptionAllert?.Invoke(obj, args);
        protected void InvokeWorkoutSavedEvent(object obj, EventArgs args) => WorkoutSaved?.Invoke(obj, args);

        protected double CountTotalWeightOfWorkout()
        {
            double sumWorkoutWeight = 0.0;

            for (int i = 0; i < NewWeightWorkout.WeightExercises.Count; i++)
            {
                double sumExerciseWeight = 0.0;

                foreach (var round in NewWeightWorkout.WeightExercises[i].WeightRounds)
                {
                    sumExerciseWeight += round.Reps * round.WeightOfExercise;
                }

                sumWorkoutWeight += sumExerciseWeight;
                NewWeightWorkout.WeightExercises[i].TotalExerciseWeight = sumExerciseWeight;
            }

            return sumWorkoutWeight;
        }

        /// <summary>
        /// Töröljük az adott GUID-al rendelkező edzést
        /// </summary>
        /// <param name="stringGuid"></param>
        public void DeleteExercise(string stringGuid)
        {
            try
            {
                NewWeightWorkout.TotalExerciseRounds -= NewWeightWorkout.WeightExercises.FirstOrDefault(x => x.ExerciseGuid.ToString() == stringGuid).WeightRounds.Count;
                NewWeightWorkout.WeightExercises.Remove(NewWeightWorkout.WeightExercises.Single(x => x.ExerciseGuid.ToString() == stringGuid));
                NewWeightWorkout.TotalWeight = CountTotalWeightOfWorkout();
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        protected void RecalculateRoundWeight(object sender, double e)
        {
            NewWeightExercise.CountTotalWeightOfExercise();
            TotalExerciseWeight = NewWeightExercise.TotalExerciseWeight;
        }

        protected async void SetupActivitiesAsync()
        {
            try
            {
                SavedActivities = new ObservableCollection<string>(await ApiServices.GetWeightActivitiesAsync());
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        //PUBLIC 
        public void DeleteRoundByStringGuid(string e)
        {
            NewWeightExercise.WeightRounds.Remove(NewWeightExercise.WeightRounds.Single(x => x.RoundGuid.ToString() == e));
            RecalculateRoundWeight(this, 0.0);
        }

        public void DuplicateRoundByStringGuid(string e)
        {
            WeightRoundVM originalRound = NewWeightExercise.WeightRounds.Single(x => x.RoundGuid.ToString() == e);

            var round = new WeightRoundVM()
            {
                Reps = originalRound.Reps,
                RoundNumber = 0,
                WeightOfExercise = originalRound.WeightOfExercise,
                RoundGuid = Guid.NewGuid()
            };

            round.RoundWeightChanged += RecalculateRoundWeight;
            NewWeightExercise.WeightRounds.Add(round);
        }

        /// <summary>
        /// Kiválasztunk egy gyakorlatot az edzésből, amit szerkeszteni fogunk.
        /// </summary>
        /// <param name="stringGuid"></param>
        public void EditExercise(string stringGuid)
        {
            WeightExerciseVM exerciseForEdit = NewWeightWorkout.WeightExercises.Single(x => x.ExerciseGuid.ToString() == stringGuid);
            NewWeightExercise = exerciseForEdit;
            NewWeightExercise.CountTotalWeightOfExercise();
            TotalExerciseWeight = NewWeightExercise.TotalExerciseWeight;
            OpenEditWeightExercise?.Invoke(this, null);
        }

        //PRIVATES
        private void ExerciseRoundSelectedFunction(object obj) => ExerciseRoundSelected?.Invoke(this, (string)obj);
        private void SaveNoteFunction(object obj) => CloseNoteEditor?.Invoke(this, new ClosePageEventArgs(PageType.WightWorkout));

        private void AddWeightExerciseToWorkoutFunction(object obj)
        {
            try
            {
                if (!IsExerciseReadyToAdd())
                    return;

                if (NewWeightWorkout.WeightExercises.Any(x => x.ExerciseGuid == NewWeightExercise.ExerciseGuid))
                    NewWeightWorkout.WeightExercises.Remove(NewWeightExercise);

                NewWeightWorkout.WeightExercises.Add(NewWeightExercise);
                NewWeightWorkout.TotalWeight = CountTotalWeightOfWorkout();
                NewWeightExercise.TotalExerciseRounds = NewWeightWorkout.WeightExercises.FirstOrDefault(x => x.ExerciseGuid == NewWeightExercise.ExerciseGuid).WeightRounds.Count;
                NewWeightWorkout.TotalExerciseRounds = NewWeightExercise.TotalExerciseRounds;
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }

            CloseAddWeightExercise?.Invoke(this, new ClosePageEventArgs(PageType.WightWorkout));
        }

        private bool IsExerciseReadyToAdd()
        {
            if (string.IsNullOrEmpty(NewWeightExercise.ExerciseName))
            {
                SendPopUpMessage(Messages.EmptyExerciseName);
                return false;
            }

            if (NewWeightExercise.TotalExerciseWeight <= 0)
            {
                SendPopUpMessage(Messages.EmptyExercise);
                return false;
            }

            if (NewWeightExercise.WeightRounds.Any(x => x.WeightOfExercise <= 0))
            {
                SendPopUpMessage(Messages.InvalidWeight);
                return false;
            }

            if (NewWeightExercise.WeightRounds.Any(x => x.Reps <= 0))
            {
                SendPopUpMessage(Messages.InvalidReps);
                return false;
            }

            return true;
        }

        protected bool IsReadyReadyToSave()
        {
            if (string.IsNullOrEmpty(NewWeightWorkout.WorkoutName))
            {
                SendPopUpMessage(Messages.EmptyWorkoutName);
                return false;
            }

            if (NewWeightWorkout.TotalWeight <= 0)
            {
                SendPopUpMessage(Messages.EmptyWorkout);
                return false;
            }

            return true;
        }

        private void OpenAddWeightExerciseFunction(object obj)
        {
            NewWeightExercise = new WeightExerciseVM();
            NewWeightExercise.ExerciseName = string.Empty;
            NewWeightExercise.TotalExerciseWeight = 0.0;
            NewWeightExercise.ExerciseNote = string.Empty;
            NewWeightExercise.ExerciseGuid = Guid.NewGuid();
            NewWeightExercise.WeightRounds = new ObservableCollection<WeightRoundVM>();
            var round = new WeightRoundVM()
            {
                Reps = 0,
                RoundNumber = 0,
                WeightOfExercise = 0.0,
                RoundGuid = Guid.NewGuid()
            };
            round.RoundWeightChanged += RecalculateRoundWeight;
            NewWeightExercise.WeightRounds.Add(round);
            TotalExerciseWeight = 0.0;
            OpenAddWeightExercise?.Invoke(this, EventArgs.Empty);
        }

        private void AddWeightRoundToExerciseFunction(object obj)
        {
            var round = new WeightRoundVM()
            {
                Reps = 0,
                RoundNumber = 0,
                WeightOfExercise = 0.0,
                RoundGuid = Guid.NewGuid()
            };

            round.RoundWeightChanged += RecalculateRoundWeight;
            NewWeightExercise.WeightRounds.Add(round);
        }

        private void OpenNoteEditorFuncton(object obj) => OpenNoteEditor?.Invoke(this, null);
        private void WeightExerciseMenuSelectedFunction(object obj) => WeightExerciseMenuSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WeightExercises.Single(x => x.ExerciseGuid.ToString() == obj.ToString()).ExerciseName, obj.ToString()));
        private void SavedActivitiySelectedFunction(object obj)
        {
            NewWeightExercise.ExerciseName = (string)obj;
            ExerciseName = (string)obj;
            SavedWeightActivitySelected?.Invoke(this, (string)obj);
        }
    }
}
