using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.Services;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutManagerVM : ViewModelBase
    {
        //FIELDS
        private readonly ApiServices _apiServices;

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

        //COMMANDS
        public DelegateCommand SaveTodayWorkoutCommand { get; private set; }
        public DelegateCommand OpenAddWeightExerciseCommand { get; private set; }
        public DelegateCommand OpenAddPopUp { get; private set; }
        public DelegateCommand OpenAddWeightRoundCommand { get; private set; }
        public DelegateCommand AddWeightExerciseToWorkoutCommand { get; private set; }
        public DelegateCommand AddWeightRoundToExerciseCommand { get; private set; }
        public DelegateCommand OpenNoteEditorCommand { get; private set; }
        public DelegateCommand SaveNoteCommand { get; private set; }
        public DelegateCommand OpenTrainingLogOpenCommand { get; private set; }
        public DelegateCommand OpenHistoryViewOpenCommand { get; private set; }
        public DelegateCommand WeightExerciseMenuSelectedCommand { get; private set; }

        //EVENTS
        public event EventHandler OpenAddWeightExercise;
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
            NewWeightWorkout = new WeightWorkoutVM
            {
                WeightExercises = new ObservableCollection<WeightExerciseVM>()
            };
            _apiServices = apiServices;
            SetupTodayWeightWorkoutAsync();
        }
        protected override void InitializeCommands()
        {
            SaveTodayWorkoutCommand = new DelegateCommand(SaveTodayWorkoutFunctionAsync);
            OpenAddWeightExerciseCommand = new DelegateCommand(OpenAddWeightExerciseFunction);
            AddWeightExerciseToWorkoutCommand = new DelegateCommand(AddWeightExerciseToWorkoutFunction);
            AddWeightRoundToExerciseCommand = new DelegateCommand(AddWeightRoundToExerciseFunction);
            OpenNoteEditorCommand = new DelegateCommand(OpenNoteEditorFuncton);
            OpenTrainingLogOpenCommand = new DelegateCommand(OpenTrainingLogOpenFunction);
            OpenHistoryViewOpenCommand = new DelegateCommand(OpenHistoryViewOpenFunction);
            WeightExerciseMenuSelectedCommand = new DelegateCommand(WeightExerciseMenuSelectedFunction);
            SaveNoteCommand = new DelegateCommand(SaveNoteFunction);
        }

        private async void SetupTodayWeightWorkoutAsync()
        {
            try
            {
                IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await _apiServices.GetWeightWorkoutsAsync();

                if (weightWorkoutDTOs != null && weightWorkoutDTOs.ToList().Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
                    SetupTodayWeightWorkoutDetails(weightWorkoutDTOs);
                else
                {
                    NewWeightWorkout = new WeightWorkoutVM()
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't connect to the server. {ex.Message}");
            }
        }

        private void SaveNoteFunction(object obj) => CloseNoteEditor.Invoke(this, new ClosePageEventArgs(PageType.WightWorkout));

        private async void SaveTodayWorkoutFunctionAsync(object obj)
        {
            IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await _apiServices.GetWeightWorkoutsAsync();

            if (weightWorkoutDTOs != null && weightWorkoutDTOs.ToList().Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
            {
                await _apiServices.EditWeightWorkoutAsync(new WeightWorkoutDTO
                {
                    Id = NewWeightWorkout.Id,
                    WorkoutDate = DateTime.Now,
                    TotalWeight = NewWeightWorkout.TotalWeight,
                    WorkoutName = NewWeightWorkout.WorkoutName,
                    WorkoutGuid = NewWeightWorkout.WorkoutGuid,
                    Note = NewWeightWorkout.Note,
                    WorkoutType = WorkoutType.WeightWorkout,
                    WorkoutImages = null,
                    WeightExercisesDto = new List<WeightExerciseDTO>(NewWeightWorkout.WeightExercises.Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        WeightRoundsDto = new List<WeightRoundDTO>(x.WeightRounds.Select(y => new WeightRoundDTO()
                        {
                            Reps = y.Reps,
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            WeightOfExercise = y.WeightOfExercise
                        })),
                    })),
                });
            }
            else
            {
                //EZ A RÉSZE ÚGY NÉZ KI, HOGY MŰKÖDIK
                //ha még nincs a mai naphoz workout, akkor létrehozzuk és feltöltjük
                var newWorkout = new WeightWorkoutDTO
                {
                    WorkoutDate = DateTime.Now,
                    TotalWeight = NewWeightWorkout.TotalWeight,
                    WorkoutGuid = Guid.NewGuid(),
                    WorkoutName = NewWeightWorkout.WorkoutName,
                    Note = NewWeightWorkout.Note,
                    WorkoutType = Data.DTO.WorkoutType.WeightWorkout,
                    WorkoutImages = null,
                    WeightExercisesDto = new List<WeightExerciseDTO>(NewWeightWorkout.WeightExercises.Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        WeightRoundsDto = new List<WeightRoundDTO>(x.WeightRounds.Select(y => new WeightRoundDTO()
                        {
                            Reps = y.Reps,
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            WeightOfExercise = y.WeightOfExercise
                        })),
                    })),
                };
                await _apiServices.AddWeightWorkoutAsync(newWorkout);
                NewWeightWorkout.Id = newWorkout.Id;
            }
        }

        /// <summary>
        /// Hozzá adjuk a NewWeightWorkout-hoz az új exercise-ot és kikalkuláljuk az új total weight-et.
        /// </summary>
        /// <param name="obj"></param>
        private void AddWeightExerciseToWorkoutFunction(object obj)
        {
            try
            {
                NewWeightWorkout.WeightExercises.Add(NewWeightExercise);
                NewWeightWorkout.TotalWeight = CountTotalWeightOfWorkout();
                NewWeightExercise.TotalExerciseRounds = NewWeightWorkout.WeightExercises.FirstOrDefault(x => x.ExerciseGuid == NewWeightExercise.ExerciseGuid).WeightRounds.Count;
                NewWeightWorkout.TotalExerciseRounds = NewWeightExercise.TotalExerciseRounds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            CloseAddWeightExercise?.Invoke(this, new ClosePageEventArgs(PageType.WightWorkout));
        }

        /// <summary>
        /// A szerverről lekérdezzük a mai naphoz tartozó edzést. 
        /// </summary>
        private void SetupTodayWeightWorkoutDetails(IEnumerable<WeightWorkoutDTO> workoutsFromServer)
        {
            WeightWorkoutDTO weightWorkoutDTO = workoutsFromServer.FirstOrDefault(x => x.WorkoutDate.Date == DateTime.Now.Date);

            NewWeightWorkout = new WeightWorkoutVM()
            {
                Id = weightWorkoutDTO.Id,
                Note = weightWorkoutDTO.Note,
                TotalExerciseRounds = weightWorkoutDTO.WeightExercisesDto.Count,
                TotalWeight = weightWorkoutDTO.TotalWeight,
                WorkoutDate = weightWorkoutDTO.WorkoutDate,
                WorkoutGuid = weightWorkoutDTO.WorkoutGuid,
                WorkoutName = weightWorkoutDTO.WorkoutName,
                WorkoutType = Data.DTO.WorkoutType.WeightWorkout,
                WeightExercises = new ObservableCollection<WeightExerciseVM>(),
            };

            foreach (var exercise in weightWorkoutDTO.WeightExercisesDto)
            {
                var rounds = new ObservableCollection<WeightRoundVM>();

                foreach (var round in exercise.WeightRoundsDto)
                {
                    rounds.Add(new WeightRoundVM()
                    {
                        Reps = round.Reps,
                        RoundGuid = round.RoundGuid,
                        RoundNumber = round.RoundNumber,
                        WeightOfExercise = round.WeightOfExercise,
                    });
                }

                NewWeightWorkout.WeightExercises.Add(new WeightExerciseVM()
                {
                    Note = exercise.Note,
                    ExerciseGuid = exercise.ExerciseGuid,
                    ExerciseName = exercise.ExerciseName,
                    TotalExerciseRounds = exercise.WeightRoundsDto.Count,
                    TotalExerciseWeight = exercise.TotalExerciseWeight,
                    WeightRounds = new ObservableCollection<WeightRoundVM>(rounds),
                });
            }
        }

        /// <summary>
        /// Töröljük az adott GUID-al rendelkező edzést
        /// </summary>
        /// <param name="stringGuid"></param>
        internal void DeleteExercise(string stringGuid)
        {
            try
            {
                NewWeightWorkout.TotalExerciseRounds -= NewWeightWorkout.WeightExercises.FirstOrDefault(x => x.ExerciseGuid.ToString() == stringGuid).WeightRounds.Count;
                NewWeightWorkout.WeightExercises.Remove(NewWeightWorkout.WeightExercises.Single(x => x.ExerciseGuid.ToString() == stringGuid));
                NewWeightWorkout.TotalWeight = CountTotalWeightOfWorkout();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// NOT WORKING
        /// </summary>
        /// <param name="stringGuid"></param>
        internal void EditExercise(string stringGuid)
        {
            OpenEditWeightExercise?.Invoke(this, null);
        }


        private void OpenAddWeightExerciseFunction(object obj)
        {
            NewWeightExercise = new WeightExerciseVM();
            NewWeightExercise.ExerciseName = string.Empty;
            NewWeightExercise.TotalExerciseWeight = 0.0;
            NewWeightExercise.Note = string.Empty;
            NewWeightExercise.ExerciseGuid = Guid.NewGuid();
            NewWeightExercise.WeightRounds = new ObservableCollection<WeightRoundVM>();
            NewWeightExercise.WeightRounds.Add(new WeightRoundVM()
            {
                Reps = 0,
                RoundNumber = 0,
                WeightOfExercise = 0.0,
                RoundGuid = Guid.NewGuid(),
            });
            OpenAddWeightExercise?.Invoke(this, null);
        }

        private void AddWeightRoundToExerciseFunction(object obj)
        {
            NewWeightExercise.WeightRounds.Add(new WeightRoundVM()
            {
                Reps = 0,
                RoundNumber = 0,
                WeightOfExercise = 0.0,
                RoundGuid = Guid.NewGuid(),
            });
        }

        private void OpenNoteEditorFuncton(object obj) => OpenNoteEditor?.Invoke(this, null);
        private void OpenTrainingLogOpenFunction(object obj) => OpenTrainingLog?.Invoke(this, null);
        private void OpenHistoryViewOpenFunction(object obj) => OpenHistoryView?.Invoke(this, null);
        private void WeightExerciseMenuSelectedFunction(object obj) => WeightExerciseMenuSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WeightExercises.Single(x => x.ExerciseGuid.ToString() == obj.ToString()).ExerciseName, obj.ToString()));
        //jó hát, ez kurva isten, hogy nem maradhat a vm-ben -> át kell pakolni a model rétegbe
        private double CountTotalWeightOfWorkout()
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
    }
}
