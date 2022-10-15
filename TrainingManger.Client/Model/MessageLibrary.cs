namespace TrainingManager.Model
{
    public class MessageLibrary
    {
        //FIELDS
        private readonly Dictionary<MessageContent, string> _messageDictionary;
        private readonly Dictionary<MessageContent, MessageType> _messageTypeDictionary;
        private static readonly Lazy<MessageLibrary> _instance = new Lazy<MessageLibrary>(() => new MessageLibrary());

        //CONSTS
        private const string WARRNING = "Warrning";
        private const string ERROR = "Error";
        private const string INFO = "Info";

        //PROPERTIES
        public static MessageLibrary Instance { get { return _instance.Value; } }

        private MessageLibrary()
        {
            _messageDictionary = new Dictionary<MessageContent, string>();
            _messageDictionary.Add(MessageContent.EmptyExerciseName, "Name of the exercise can't be empty.");
            _messageDictionary.Add(MessageContent.EmptyWorkoutName, "Name of the workout can't be empty.");
            _messageDictionary.Add(MessageContent.InvalidWeight, "Value of the weight must be greater than 0.");
            _messageDictionary.Add(MessageContent.InvalidReps, "Value of the reps must be greater than 0.");
            _messageDictionary.Add(MessageContent.MayInvalidReps, "Results may be inaccurate because the value of the reps is greater than 10.");
            _messageDictionary.Add(MessageContent.EmptyWorkout, "The workout must contains at least one exercise.");
            _messageDictionary.Add(MessageContent.EmptyExercise, "The exercise must contains at least one round.");
            _messageDictionary.Add(MessageContent.EmptyUserName, "The user name is required for login.");
            _messageDictionary.Add(MessageContent.EmptyPassword, "The password is required for login.");
            _messageDictionary.Add(MessageContent.LoginFailed, "Failed to login.");
            _messageDictionary.Add(MessageContent.RequiredFirstName, "The first name is required for the registration.");
            _messageDictionary.Add(MessageContent.RequiredUserName, "The user name is required for the registration.");
            _messageDictionary.Add(MessageContent.RequiredEmail, "The email address is required for the registration.");
            _messageDictionary.Add(MessageContent.RequiredPassword, "The password is required for the registration.");
            _messageDictionary.Add(MessageContent.RequiredConfirmPassword, "The confirm password is required for the registration.");
            _messageDictionary.Add(MessageContent.PasswordsAreNotEquals, "Passwords have to math for the registration.");
            _messageDictionary.Add(MessageContent.RegistrationFailed, "Registration failed.");
            _messageDictionary.Add(MessageContent.LoginFailedAfterRegistration, "Can't login after registration process. Try from the login page.");
            _messageDictionary.Add(MessageContent.UploadPictureMessage, "For the best result use a square image.");
            _messageDictionary.Add(MessageContent.AssesDeniedProfilePictureMessage, "Can't upload profile picture! Access denied!");
            _messageDictionary.Add(MessageContent.UnknownMuscle, "Set the muscle group for the exercise.");

            _messageTypeDictionary = new Dictionary<MessageContent, MessageType>();
            _messageTypeDictionary.Add(MessageContent.EmptyExerciseName, MessageType.Warrning);
            _messageTypeDictionary.Add(MessageContent.EmptyWorkoutName, MessageType.Warrning);
            _messageTypeDictionary.Add(MessageContent.InvalidWeight, MessageType.Warrning);
            _messageTypeDictionary.Add(MessageContent.InvalidReps, MessageType.Warrning);
            _messageTypeDictionary.Add(MessageContent.MayInvalidReps, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.EmptyWorkout, MessageType.Warrning);
            _messageTypeDictionary.Add(MessageContent.EmptyExercise, MessageType.Warrning);
            _messageTypeDictionary.Add(MessageContent.EmptyUserName, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.EmptyPassword, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.LoginFailed, MessageType.Error);
            _messageTypeDictionary.Add(MessageContent.RequiredFirstName, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.RequiredUserName, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.RequiredEmail, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.RequiredPassword, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.RequiredConfirmPassword, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.PasswordsAreNotEquals, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.RegistrationFailed, MessageType.Error);
            _messageTypeDictionary.Add(MessageContent.LoginFailedAfterRegistration, MessageType.Error);
            _messageTypeDictionary.Add(MessageContent.UploadPictureMessage, MessageType.Info);
            _messageTypeDictionary.Add(MessageContent.AssesDeniedProfilePictureMessage, MessageType.Error);
            _messageTypeDictionary.Add(MessageContent.UnknownMuscle, MessageType.Warrning);
        }

        //PUBLIC
        public string GetMessage(MessageContent messageEnum) => _messageDictionary[messageEnum];
        public string GetMessageType(MessageContent messageEnum)
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
