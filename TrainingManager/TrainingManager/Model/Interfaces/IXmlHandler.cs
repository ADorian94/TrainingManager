using System.Collections.Generic;

namespace TrainingManager.Model.Interfaces
{
    public interface IXmlHandler<T>
    {
        void SaveToXml(T workoutToSave, WorkoutType workoutType);

        List<T> LoadWorkoutXmls(WorkoutType workoutType);
    }
}
