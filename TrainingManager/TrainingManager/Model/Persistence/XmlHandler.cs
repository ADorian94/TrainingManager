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

        public void SaveToXml(T workoutToSave)
        {
            string fileName = $"{workoutToSave.WorkoutId}.xml";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            if (!File.Exists(filePath))
                File.Delete(filePath);

            FileStream fileStream = File.Create(filePath);
            _xmlSerializer = new XmlSerializer(typeof(T));
            _xmlSerializer.Serialize(fileStream, workoutToSave);
        }

        public List<T> LoadWorkoutXmls()
        {
            try
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
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
    }
}
