using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.Services;
using TrainingManager.Model.Workouts.WeightWorkout;

namespace TrainingManager.ViewModel
{
    public class WeightDrillVM : ViewModelBase
    {
        //FIELDS
        private int _workoutId;
        private int _roundId;

        public WeightDrillVM()
        {
            DrillName = string.Empty;
            Note = string.Empty;
            WeightOfDrill = 0.0;
            Reps = 0;
            DrillDate = DateTime.Now;
        }

        public WeightDrillVM(WeightDrillVM weightDrill)
        {
            DrillName = weightDrill.DrillName;
            Note = weightDrill.Note;
            WeightOfDrill = weightDrill.WeightOfDrill;
            Reps = weightDrill.Reps;
            DrillDate = weightDrill.DrillDate;
        }

        protected override void InitializeCommands() { }

        //PROPERTIES
        private string _drillName;
        public string DrillName { get => _drillName; set { _drillName = value; OnPropertyChanged(); } }

        private string _note;
        public string Note { get => _note; set { _note = value; OnPropertyChanged(); } }

        private double _weightOfDrill;
        public double WeightOfDrill { get => _weightOfDrill; set { _weightOfDrill = value; OnPropertyChanged(); } }

        private DateTime _drillDate = DateTime.Now;
        public DateTime DrillDate { get => _drillDate; set { _drillDate = value; OnPropertyChanged(); } }

        private int _reps;
        public int Reps { get => _reps; set { _reps = value; OnPropertyChanged(); } }
    }

    public class RoundsVM : ViewModelBase
    {
        //FIELDS
        private int _workoutId;

        protected override void InitializeCommands() { }

        //PROPERTIES
        private string _roundName;
        public string RoundName { get => _roundName; set { _roundName = value; OnPropertyChanged(); } }

        private string _note;
        public string Note { get => _note; set { _note = value; OnPropertyChanged(); } }

        private int _reps;
        public int Reps { get => _reps; set { _reps = value; OnPropertyChanged(); } }

        private ObservableCollection<WeightDrillVM> _weightDrills = new ObservableCollection<WeightDrillVM>();
        public ObservableCollection<WeightDrillVM> WeightDrills { get => _weightDrills; set { _weightDrills = value; OnPropertyChanged(); } }
    }

    public class WeightWorkoutVM : ViewModelBase
    {
        protected override void InitializeCommands() { }

        //PROPERTIES
        private string _workoutName;
        public string WorkoutName { get => _workoutName; set { _workoutName = value; OnPropertyChanged(); } }

        private string _note;
        public string Note { get => _note; set { _note = value; OnPropertyChanged(); } }

        private double _totalWeight;
        public double TotalWeight { get => _totalWeight; set { _totalWeight = value; OnPropertyChanged(); } }

        private DateTime _workoutDate = DateTime.Now;
        public DateTime WorkoutDate { get => _workoutDate; set { _workoutDate = value; OnPropertyChanged(); } }

        private Data.DTO.WorkoutType _workoutType;
        public Data.DTO.WorkoutType WorkoutType { get => _workoutType; set { _workoutType = value; OnPropertyChanged(); } }

        private ObservableCollection<RoundsVM> _rounds = new ObservableCollection<RoundsVM>();
        public ObservableCollection<RoundsVM> Rounds { get => _rounds; set { _rounds = value; OnPropertyChanged(); } }

        private ObservableCollection<WeightDrillVM> _weightDrills;
        public ObservableCollection<WeightDrillVM> WeightDrills { get => _weightDrills; set { _weightDrills = value; OnPropertyChanged(); } }
    }

    public class WeightWorkoutManagerVM : ViewModelBase
    {
        //FIELDS
        private ApiServices _apiServices;
        private bool _isEdit;

        //PROPERTIES
        private WeightWorkoutVM _newWeightWorkout;
        public WeightWorkoutVM NewWeightWorkout { get => _newWeightWorkout; set { _newWeightWorkout = value; OnPropertyChanged(); } }

        private WeightDrillVM _newWeightDrill;
        public WeightDrillVM NewWeightDrill { get => _newWeightDrill; set { _newWeightDrill = value; OnPropertyChanged(); } }

        private string _workoutName;
        public string WorkoutName
        {
            get => _workoutName;
            set
            {
                _workoutName = value;
                OnPropertyChanged();
            }
        }

        private string _exerciseName;
        public string ExerciseName
        {
            get => _exerciseName;
            set
            {
                _exerciseName = value;
                OnPropertyChanged();
            }
        }

        private double _exerciseWeight;
        public double ExerciseWeight
        {
            get => _exerciseWeight;
            set
            {
                _exerciseWeight = value;
                OnPropertyChanged();
            }
        }

        private int _exerciseReps;
        public int ExerciseReps
        {
            get => _exerciseReps;
            set
            {
                _exerciseReps = value;
                OnPropertyChanged();
            }
        }

        private string _exercieNote;
        public string ExerciseNote
        {
            get => _exercieNote;
            set
            {
                _exercieNote = value;
                OnPropertyChanged();
            }
        }

        private double _totalWeight;
        public double TotalWeight
        {
            get => _totalWeight;
            set
            {
                _totalWeight = value;
                OnPropertyChanged();
            }
        }

        private WeightWorkout _todayWeightWorkout;
        public WeightWorkout TodayWeightWorkout
        {
            get => _todayWeightWorkout;
            set
            {
                _todayWeightWorkout = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WeightExercise> _weightWorkoutList;
        public ObservableCollection<WeightExercise> WeightWorkoutList
        {
            get => _weightWorkoutList;
            set
            {
                _weightWorkoutList = value;
                OnPropertyChanged();
            }
        }

        private bool _isPopUpVisible = false;
        public bool IsPopUpVisible
        {
            get => _isPopUpVisible;
            set
            {
                _isPopUpVisible = value;
                OnPropertyChanged();
            }
        }

        //COMMANDS
        public DelegateCommand SaveTodayWorkoutCommand { get; private set; }
        public DelegateCommand OpenAddWeightExerciseCommand { get; private set; }
        public DelegateCommand OpenAddPopUp { get; private set; }
        public DelegateCommand OpenAddWeightRoundCommand { get; private set; }
        public DelegateCommand AddWeightExerciseToWorkoutCommand { get; private set; }
        public DelegateCommand OpenNoteEditorCommand { get; private set; }
        public DelegateCommand SaveNoteCommand { get; private set; }
        public DelegateCommand OpenTrainingLogOpenCommand { get; private set; }
        public DelegateCommand OpenHistoryViewOpenCommand { get; private set; }
        public DelegateCommand WeightExerciseMenuSelectedCommand { get; private set; }

        //EVENTS
        public event EventHandler OpenAddWeightExercise;
        public event EventHandler OpenAddWeightRound;
        public event EventHandler OpenEditWeightExercise;
        public event EventHandler<ClosePageEventArgs> CloseAddWeightExercise;
        public event EventHandler OpenNoteEditor;
        public event EventHandler<ClosePageEventArgs> CloseNoteEditor;
        public event EventHandler OpenTrainingLog;
        public event EventHandler OpenHistoryView;
        public event EventHandler<MessageEventArgs> WeightExerciseMenuSelected;

        public WeightWorkoutManagerVM(ApiServices apiServices)
        {
            InitializeCommands();
            WeightWorkoutList = new ObservableCollection<WeightExercise>();
            NewWeightWorkout = new WeightWorkoutVM();
            NewWeightWorkout.WeightDrills = new ObservableCollection<WeightDrillVM>();
            _apiServices = apiServices;
            //SetTodayWeightWorkoutAsync();
        }

        //private async void SetTodayWeightWorkoutAsync()
        //{
        //    IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await _apiServices.GetWeightWorkoutsAsync();

        //    if (weightWorkoutDTOs != null && weightWorkoutDTOs.ToList().Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
        //    {
        //        SyncroniseTodayWeightWorkoutAsync();
        //    }
        //    else
        //        TodayWeightWorkout = new WeightWorkout()
        //        {
        //            WorkoutDate = DateTime.Now,
        //            Exercises = new List<WeightExercise>(),
        //            TotalWeight = 0.0,
        //            //WorkoutType = Data.DTO.WorkoutType.WeightWorkout,
        //            WorkoutName = DateTime.Now.Date.ToString("yyyy.MM.dd"),
        //        };
        //}

        protected override void InitializeCommands()
        {
            SaveTodayWorkoutCommand = new DelegateCommand(SaveTodayWorkoutFunctionAsync);
            OpenAddWeightExerciseCommand = new DelegateCommand(OpenAddWeightExerciseFunction);
            OpenAddPopUp = new DelegateCommand(OpenAddPopUpFunction);
            AddWeightExerciseToWorkoutCommand = new DelegateCommand(AddWeightExerciseToWorkoutFunction);
            OpenNoteEditorCommand = new DelegateCommand(OpenNoteEditorFuncton);
            OpenTrainingLogOpenCommand = new DelegateCommand(OpenTrainingLogOpenFunction);
            OpenHistoryViewOpenCommand = new DelegateCommand(OpenHistoryViewOpenFunction);
            WeightExerciseMenuSelectedCommand = new DelegateCommand(WeightExerciseMenuSelectedFunction);
            SaveNoteCommand = new DelegateCommand(SaveNoteFunction);
            OpenAddWeightRoundCommand = new DelegateCommand(OpenAddWeightRoundFunction);
        }



        private void OpenAddPopUpFunction(object obj)
        {
            IsPopUpVisible = true;
        }

        private void SaveNoteFunction(object obj) => CloseNoteEditor.Invoke(this, new ClosePageEventArgs(PageType.WightWorkout));

        //Itt kellene feltölteni az adatokat az adatbázis-ba
        //A todayworkout-on végigmegyek és feltöltöm
        //Figyelni kell, hogy van e már workout a mai naphoz.
        private async void SaveTodayWorkoutFunctionAsync(object obj)
        {
            IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await _apiServices.GetWeightWorkoutsAsync();

            if (weightWorkoutDTOs != null && weightWorkoutDTOs.ToList().Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
            {
                //ha már van a mai naphoz workout, akkor lecseréljük
            }
            else
            {
                //ha még nincs a mai naphoz workout, akkor létrehozzuk és feltöltjük
                await _apiServices.AddWeightWorkoutAsync(new WeightWorkoutDTO
                {
                    WorkoutName = TodayWeightWorkout.WorkoutName,
                    TotalWeight = TodayWeightWorkout.TotalWeight,
                    Note = TodayWeightWorkout.Note,
                    WorkoutDate = TodayWeightWorkout.WorkoutDate,
                    //WorkoutType = WebApi.Model.StructAndEnums.WorkoutType.WeightWorkout,
                    //Exercises = WeightWorkoutList.Select(x => new WeigthExerciseDTO
                    //{
                    //    ExerciseName = x.ExerciseName,
                    //    Note = x.ExerciseName,
                    //    Reps = x.Reps,
                    //    ExerciseDate = x.ExerciseDate,
                    //    WeightOfExercise = x.WeightOfExercise
                    //}).ToList()
                });
            }

            TodayWeightWorkout.TotalWeight = TotalWeight;
        }

        //Itt még csak lokálba hozzuk létre a gyakorlatokat. (Lehet, hogy nem is fogjuk lementeni)
        //A todayworkout-ba mentek
        private void AddWeightExerciseToWorkoutFunction(object obj)
        {
            //if (_isEdit)
            //{

            //}
            //else
            //{
            //    WeightWorkoutList.Add(new WeightExercise
            //    {
            //        ExerciseGuid = Guid.NewGuid(),
            //        ExerciseDate = DateTime.Now,
            //        ExerciseName = ExerciseName,
            //        Note = ExerciseNote,
            //        Reps = ExerciseReps,
            //        WeightOfExercise = ExerciseWeight
            //    });
            //}

            //UpdateTodayWeightWorkoutLocal();
            //TotalWeight = CountTotalWeightOfWorkout();
            //UpdateWeightWorkout();
            try
            {
                NewWeightWorkout.WeightDrills.Add(new WeightDrillVM(NewWeightDrill));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            CloseAddWeightExercise?.Invoke(this, new ClosePageEventArgs(PageType.WightWorkout));
        }

        private void UpdateTodayWeightWorkoutLocal()
        {
            TodayWeightWorkout.Exercises = WeightWorkoutList.ToList();
        }

        private async void SyncroniseTodayWeightWorkoutAsync()
        {
            IEnumerable<WeightWorkoutDTO> workoutsEnumerable = await _apiServices.GetWeightWorkoutsAsync();
            WeightWorkoutDTO todayWorkoutDTO = workoutsEnumerable.ToList().FirstOrDefault(x => x.WorkoutDate.Date == DateTime.Now.Date);
            TodayWeightWorkout = new WeightWorkout()
            {
                Id = todayWorkoutDTO.Id,
                WorkoutName = todayWorkoutDTO.WorkoutName,
                Note = todayWorkoutDTO.Note,
                TotalWeight = todayWorkoutDTO.TotalWeight,
                WorkoutDate = todayWorkoutDTO.WorkoutDate,
                //WorkoutType = WorkoutType.WeightWorkout
            };

            //TodayWeightWorkout.Exercises = new List<WeightExercise>(
            //todayWorkoutDTO.Exercises.Select(x => new WeightExercise
            //{
            //    Id = x.Id,
            //    ExerciseGuid = x.ExerciseGuid,
            //    ExerciseName = x.ExerciseName,
            //    Note = x.Note,
            //    Reps = x.Reps,
            //    WeightOfExercise = x.WeightOfExercise,
            //    ExerciseDate = x.ExerciseDate,
            //}));

            //TotalWeight = CountTotalWeightOfWorkout();

            if (TodayWeightWorkout.Exercises != null)
            {
                UpdateWeightWorkout();
            }
        }

        internal void DeleteExercise(string stringGuid)
        {
            //_weightWorkoutManager.DeleteExerciseFromWorkoutById(TodayWeightWorkout.WorkoutId, new Guid(stringGuid));
            SyncroniseTodayWeightWorkoutAsync();
        }

        internal void EditExercise(string stringGuid)
        {
            //WeightExercise exercise = _weightWorkoutManager.GetExerciseInWorkoutById(TodayWeightWorkout.WorkoutId, new Guid(stringGuid));
            WeightExercise exercise = WeightWorkoutList.FirstOrDefault(x => Equals(x.ExerciseGuid.ToString(), stringGuid));
            ExerciseName = exercise.ExerciseName;
            ExerciseWeight = exercise.WeightOfExercise;
            ExerciseReps = exercise.Reps;
            ExerciseNote = exercise.Note;
            _isEdit = true;
            OpenEditWeightExercise?.Invoke(this, null);
        }

        private void UpdateWeightWorkout() => WeightWorkoutList = new ObservableCollection<WeightExercise>(TodayWeightWorkout.Exercises);

        private void OpenAddWeightExerciseFunction(object obj)
        {
            NewWeightDrill = new WeightDrillVM();
            //ExerciseName = string.Empty;
            //ExerciseWeight = 0.0;
            //ExerciseReps = 0;
            //ExerciseNote = string.Empty;
            OpenAddWeightExercise?.Invoke(this, null);
            IsPopUpVisible = false;
        }

        private void OpenAddWeightRoundFunction(object obj)
        {
            OpenAddWeightRound?.Invoke(this, null);
            IsPopUpVisible = false;
        }

        private void OpenNoteEditorFuncton(object obj) => OpenNoteEditor?.Invoke(this, null);
        private void OpenTrainingLogOpenFunction(object obj) => OpenTrainingLog?.Invoke(this, null);
        private void OpenHistoryViewOpenFunction(object obj) => OpenHistoryView?.Invoke(this, null);
        private void WeightExerciseMenuSelectedFunction(object obj) => WeightExerciseMenuSelected?.Invoke(this, new MessageEventArgs(obj.ToString()));
        private double CountTotalWeightOfWorkout() => WeightWorkoutList.ToList().Aggregate(0.0, (x, y) => x += y.WeightOfExercise * y.Reps);
    }
}
