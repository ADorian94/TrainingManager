using System.Collections.Generic;

namespace TrainingManager.Model.Interfaces
{
    public interface IXmlHandler<T>
    {
        void SaveToXml(T workoutToSave);

        List<T> LoadWorkoutXmls();
    }
}
