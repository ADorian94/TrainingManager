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
        Avarage1RM
    }

    public enum IntervallTimerStates
    {
        IntervallTimerStopped,
        IntervallTimerStarted,
        IntervallTimerPaused
    }

    public enum WorkoutType
    {
        IntervallWorkout,
        WeightWorkout,
    }

    public static class UsedFolders
    {
        public const string INTERVALL = "Intervall";
        public const string WEIGHT = "Weight";
    }
}
