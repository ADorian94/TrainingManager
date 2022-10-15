using TrainingManager.Data;

namespace TrainingManager.Model
{
    public struct MaximumMethod
    {
        public string MethodName { get; set; }
        public double MaximumValue { get; set; }
        public MethodType TypeOfMethod { get; set; }
    }

    public enum MethodType
    {
        Epley,
        Brzycki,
        McGlothin,
        Lombardi,
        Mayhew,
        OConner,
        Wathen,
        Avarage1RM,
    }

    public enum MessageType
    {
        Warrning,
        Info,
        Error,
    }

    public enum MessageContent
    {
        EmptyExerciseName,
        EmptyWorkoutName,
        InvalidWeight,
        InvalidReps,
        MayInvalidReps,
        EmptyExercise,
        EmptyWorkout,
        EmptyUserName,
        EmptyPassword,
        LoginFailed,
        RequiredFirstName,
        RequiredUserName,
        RequiredEmail,
        RequiredPassword,
        RequiredConfirmPassword,
        PasswordsAreNotEquals,
        RegistrationFailed,
        LoginFailedAfterRegistration,
        UploadPictureMessage,
        AssesDeniedProfilePictureMessage,
        UnknownMuscle,
    }

    public struct MaterialColorName
    {
        public MaterialColors Color { get; set; }
        public string Name { get; set; }
    }
}
