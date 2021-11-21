using System;
using System.Collections.Generic;

namespace TrainingManager.Model
{
    public class MessageLibrary
    {
        //FIELDS
        private readonly Dictionary<Messages, string> _messageDictionary;
        private readonly Dictionary<Messages, MessageType> _messageTypeDictionary;
        private static readonly Lazy<MessageLibrary> _instance = new Lazy<MessageLibrary>(() => new MessageLibrary());

        //CONSTS
        private const string WARRNING = "Warrning";
        private const string ERROR = "Error";
        private const string INFO = "Info";

        //PROPERTIES
        public static MessageLibrary Instance { get { return _instance.Value; } }

        private MessageLibrary()
        {
            _messageDictionary = new Dictionary<Messages, string>();
            _messageDictionary.Add(Messages.EmptyExerciseName, "Name of the exercise can't be empty.");
            _messageDictionary.Add(Messages.EmptyWorkoutName, "Name of the workout can't be empty.");
            _messageDictionary.Add(Messages.InvalidWeight, "Value of the weight must be greater than 0.");
            _messageDictionary.Add(Messages.InvalidReps, "Value of the reps must be greater than 0.");
            _messageDictionary.Add(Messages.MayInvalidReps, "Results may be inaccurate because the value of the reps is greater than 10.");
            _messageDictionary.Add(Messages.EmptyWorkout, "The workout must contains at least one exercise.");
            _messageDictionary.Add(Messages.EmptyExercise, "The exercise must contains at least one round.");
            _messageDictionary.Add(Messages.EmptyUserName, "The user name is required for login.");
            _messageDictionary.Add(Messages.EmptyPassword, "The password is required for login.");
            _messageDictionary.Add(Messages.LoginFailed, "Failed to login.");
            _messageDictionary.Add(Messages.RequiredFirstName, "The first name is required for the registration.");
            _messageDictionary.Add(Messages.RequiredUserName, "The user name is required for the registration.");
            _messageDictionary.Add(Messages.RequiredEmail, "The email address is required for the registration.");
            _messageDictionary.Add(Messages.RequiredPassword, "The password is required for the registration.");
            _messageDictionary.Add(Messages.RequiredConfirmPassword, "The confirm password is required for the registration.");
            _messageDictionary.Add(Messages.PasswordsAreNotEquals, "Passwords have to math for the registration.");
            _messageDictionary.Add(Messages.RegistrationFailed, "Registration failed.");
            _messageDictionary.Add(Messages.LoginFailedAfterRegistration, "Can't login after registration process. Try from the login page.");

            _messageTypeDictionary = new Dictionary<Messages, MessageType>();
            _messageTypeDictionary.Add(Messages.EmptyExerciseName, MessageType.Warrning);
            _messageTypeDictionary.Add(Messages.EmptyWorkoutName, MessageType.Warrning);
            _messageTypeDictionary.Add(Messages.InvalidWeight, MessageType.Warrning);
            _messageTypeDictionary.Add(Messages.InvalidReps, MessageType.Warrning);
            _messageTypeDictionary.Add(Messages.MayInvalidReps, MessageType.Info);
            _messageTypeDictionary.Add(Messages.EmptyWorkout, MessageType.Warrning);
            _messageTypeDictionary.Add(Messages.EmptyExercise, MessageType.Warrning);
            _messageTypeDictionary.Add(Messages.EmptyUserName, MessageType.Info);
            _messageTypeDictionary.Add(Messages.EmptyPassword, MessageType.Info);
            _messageTypeDictionary.Add(Messages.LoginFailed, MessageType.Error);
            _messageTypeDictionary.Add(Messages.RequiredFirstName, MessageType.Info);
            _messageTypeDictionary.Add(Messages.RequiredUserName, MessageType.Info);
            _messageTypeDictionary.Add(Messages.RequiredEmail, MessageType.Info);
            _messageTypeDictionary.Add(Messages.RequiredPassword, MessageType.Info);
            _messageTypeDictionary.Add(Messages.RequiredConfirmPassword, MessageType.Info);
            _messageTypeDictionary.Add(Messages.PasswordsAreNotEquals, MessageType.Info);
            _messageTypeDictionary.Add(Messages.RegistrationFailed, MessageType.Error);
            _messageTypeDictionary.Add(Messages.LoginFailedAfterRegistration, MessageType.Error);
        }

        //PUBLIC
        public string GetMessage(Messages messageEnum) => _messageDictionary[messageEnum];
        public string GetMessageType(Messages messageEnum)
        {
            switch (_messageTypeDictionary[messageEnum])
            {
                case MessageType.Warrning:
                    return WARRNING;
                case MessageType.Error:
                    return ERROR;
                case MessageType.Info:
                    return INFO;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
