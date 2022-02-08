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

    public enum Messages
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
    }
}
