using System;
using TrainingManager.Data.DTO;

namespace TrainingManager.ViewModel.WorkoutManager
{
    public class PersonalRecordVM : ViewModelBase
    {
        //FIELDS
        private Guid _personalRecordGuid;
        private string _ownerUserName;
        private int _activityId;
        private int _workoutId;

        //PROPERTIES
        public double _weightOfPersonalRecord;
        public double WeightOfPersonalRecord { get => _weightOfPersonalRecord; set { _weightOfPersonalRecord = value; OnPropertyChanged(); } }

        public int _repsOfPersonalRecord;
        public int RepsOfPersonalRecord { get => _repsOfPersonalRecord; set { _repsOfPersonalRecord = value; OnPropertyChanged(); } }

        private DateTime _personalRecordDate;
        public DateTime PersonalRecordDate { get => _personalRecordDate; set { _personalRecordDate = value; OnPropertyChanged(); } }

        public PersonalRecordVM(PersonalRecordDTO _personalRecordDTO)
        {
            WeightOfPersonalRecord = _personalRecordDTO.WeightOfPersonalRecord;
            RepsOfPersonalRecord = _personalRecordDTO.RepsOfPersonalRecord;
            PersonalRecordDate = _personalRecordDTO.PersonalRecordDate;
            _personalRecordGuid = _personalRecordDTO.PersonalRecordGuid;
            _ownerUserName = _personalRecordDTO.OwnerUserName;
            _activityId = _personalRecordDTO.ActivityId;
            _workoutId = _personalRecordDTO.WorkoutId;
        }

        protected override void InitializeCommands()
        {
        }
    }
}
