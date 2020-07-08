using System;

namespace Utility
{
    public static class OneRepetitionMaximumClass
    {
        public static double Epley(double weight, int reps) => weight * (1 + (reps / 30.0));
        public static double Brzycki(double weight, int reps) => weight * (36.0 / (37.0 - reps));
        public static double McGlothin(double weight, int reps) => (100.0 * weight) / (101.3 - 2.67123 * reps);
        public static double Lombardi(double weight, int reps) => (weight * (Math.Pow(reps, 0.10)));
        public static double Mayhew(double weight, int reps) => (100.0 * weight) / (52.2 + 41.9 * Math.Pow(Math.E, -0.055 * reps));
        public static double OConner(double weight, int reps) => weight * (1 + (reps / 40.0));
        public static double Wathen(double weight, int reps) => (100.0 * weight) / (48.8 + 53.8 * Math.Pow(Math.E, -0.075 * reps));

        public static double Avarage1RM(double weight, int reps) =>
            (Epley(weight, reps) + Brzycki(weight, reps) + McGlothin(weight, reps) + Lombardi(weight, reps) + Mayhew(weight, reps)
            + OConner(weight, reps) + Wathen(weight, reps)) / 7;
    }
}
