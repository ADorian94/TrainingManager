using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace TrainingManager.Model
{
    public class OneRepetitionMaximumModel
    {
        private readonly List<MethodType> _possbleMethods;

        public OneRepetitionMaximumModel()
        {
            _possbleMethods = new List<MethodType>();

            foreach (MethodType method in Enum.GetValues(typeof(MethodType)))
            {
                _possbleMethods.Add(method);
            }
        }

        //ezt akarom tesztelni
        public IEnumerable<MaximumMethod> CalculateOneRepMaximums(double weight, int reps) =>
            _possbleMethods.Select(method => new MaximumMethod
            {
                MethodName = MethodToString(method),
                MaximumValue = CalculateByMethod(weight, reps, method),
                TypeOfMethod = method
            });

        private string MethodToString(MethodType methodType)
        {
            switch (methodType)
            {
                case MethodType.Epley:
                    return "Epley method";
                case MethodType.Lombardi:
                    return "Lombardi method";
                case MethodType.Mayhew:
                    return "Mayhew method";
                case MethodType.McGlothin:
                    return "McGlothin method";
                case MethodType.OConner:
                    return "O'Conner method";
                case MethodType.Wathen:
                    return "Wathen method";
                case MethodType.Brzycki:
                    return "Brzycki method";
                case MethodType.Avarage1RM:
                    return "Avarage";
                default:
                    throw new NotImplementedException();
            }
        }

        private double CalculateByMethod(double weight, int reps, MethodType methodType)
        {
            switch (methodType)
            {
                case MethodType.Epley:
                    return Math.Round(OneRepetitionMaximumClass.Epley(weight, reps), 2);
                case MethodType.Lombardi:
                    return Math.Round(OneRepetitionMaximumClass.Lombardi(weight, reps), 2);
                case MethodType.Mayhew:
                    return Math.Round(OneRepetitionMaximumClass.Mayhew(weight, reps), 2);
                case MethodType.McGlothin:
                    return Math.Round(OneRepetitionMaximumClass.McGlothin(weight, reps), 2);
                case MethodType.OConner:
                    return Math.Round(OneRepetitionMaximumClass.OConner(weight, reps), 2);
                case MethodType.Wathen:
                    return Math.Round(OneRepetitionMaximumClass.Wathen(weight, reps), 2);
                case MethodType.Brzycki:
                    return Math.Round(OneRepetitionMaximumClass.Brzycki(weight, reps), 2);
                case MethodType.Avarage1RM:
                    return Math.Round(OneRepetitionMaximumClass.Avarage1RM(weight, reps), 2);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
