using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Workouts;

namespace TrainingManager.Model.Persistence
{
    public class XmlHandler<T, U> : IXmlHandler<T> where T : WorkoutBase<U> where U : ExerciseBase
    {
        private XmlSerializer _xmlSerializer;
        private const string TRAINING_MANAGER = "TrainingManager";

        public XmlHandler()
        {

        }

        public void SaveToXml(T workoutToSave, WorkoutType workoutType)
        {
            string folder = GetFolderByWorkoutType(workoutType);
            string fileName = $@"{folder}\{workoutToSave.WorkoutId}.xml";

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folder)))
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folder));

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            FileStream fileStream = File.Create(filePath);
            _xmlSerializer = new XmlSerializer(typeof(T));
            _xmlSerializer.Serialize(fileStream, workoutToSave);
        }

        public List<T> LoadWorkoutXmls(WorkoutType workoutType)
        {
            try
            {
                string folder = workoutType == WorkoutType.IntervallWorkout ? "Intervall" : "Weight";
                string[] files = Directory.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folder));
                StreamReader fileStream;
                _xmlSerializer = new XmlSerializer(typeof(T));
                List<T> loadedWorkout = new List<T>();

                foreach (var file in files)
                {
                    //File.Delete(file);
                    fileStream = new StreamReader(file);
                    loadedWorkout.Add((T)_xmlSerializer.Deserialize(fileStream));
                }

                return loadedWorkout;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        private string GetFolderByWorkoutType(WorkoutType workoutType)
        {
            switch (workoutType)
            {
                case WorkoutType.IntervallWorkout:
                    return UsedFolders.INTERVALL;
                case WorkoutType.WeightWorkout:
                    return UsedFolders.WEIGHT;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
