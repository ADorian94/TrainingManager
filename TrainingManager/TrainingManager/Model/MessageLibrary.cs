using System;
using System.Collections.Generic;

namespace TrainingManager.Model
{
    public class MessageLibrary
    {
        //FIELDS
        private readonly Dictionary<Messages, string> _messageDictionary;
        private static readonly Lazy<MessageLibrary> _instance = new Lazy<MessageLibrary>(() => new MessageLibrary());

        //PROPERTIES
        public static MessageLibrary Instance { get { return _instance.Value; } }

        private MessageLibrary()
        {
            _messageDictionary = new Dictionary<Messages, string>();
            _messageDictionary.Add(Messages.EmptyExerciseName, "Name of the exercise can't be empty.");
            _messageDictionary.Add(Messages.EmptyWorkoutName, "Name of the workout can't be empty.");
            _messageDictionary.Add(Messages.InvalidWeight, "Value of the weight must be greater than 0.");
            _messageDictionary.Add(Messages.InvalidReps, "Value of the reps must be greater than 0.");
            _messageDictionary.Add(Messages.EmptyWorkout, "The workout must contains at least one exercise.");
            _messageDictionary.Add(Messages.EmptyExercise, "The exercise must contains at least one round.");
        }

        //PUBLIC
        public string GetMessage(Messages messageEnum) => _messageDictionary[messageEnum];
    }
}
