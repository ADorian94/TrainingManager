using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TrainingManager.Model.Tests
{
    [TestClass()]
    public class OneRepetitionMaximumModelTests
    {
        //komment
        //FIELDS
        private OneRepetitionMaximumModel _repetitionMaximumModel;
        private const double ZERO_WEIGHT = 0.0;
        private const int ZERO_REPS = 0;

        [TestInitialize]
        public void Initialize()
        {
            _repetitionMaximumModel = new OneRepetitionMaximumModel();
        }

        [TestMethod()]
        public void MethodTypeLengthTest()
        {
            List<MaximumMethod> result = new List<MaximumMethod>(_repetitionMaximumModel.CalculateOneRepMaximums(ZERO_WEIGHT, ZERO_REPS));
            Assert.AreEqual(GetLenthOfMethodTypeEnum(), result.Count);
        }

        [DataTestMethod]
        [DataRow(0.0, 0)]
        [DataRow(80.0, 8)]
        [DataRow(95.5, 6)]
        public void MethodTypeValueTest(double weightOfExercise, int repsOfExercise)
        {
            List<MaximumMethod> result = new List<MaximumMethod>(_repetitionMaximumModel.CalculateOneRepMaximums(weightOfExercise, repsOfExercise));

            foreach (var item in result)
            {
                if (item.TypeOfMethod == MethodType.Epley)
                    Assert.AreEqual(Math.Round(weightOfExercise * (1 + (repsOfExercise / 30.0)), 2), item.MaximumValue);

                if (item.TypeOfMethod == MethodType.Brzycki)
                    Assert.AreEqual(Math.Round(weightOfExercise * (36.0 / (37.0 - repsOfExercise)), 2), item.MaximumValue);

                if (item.TypeOfMethod == MethodType.McGlothin)
                    Assert.AreEqual(Math.Round((weightOfExercise * 100.0) / (101.3 - 2.67123 * repsOfExercise), 2), item.MaximumValue);

                if (item.TypeOfMethod == MethodType.Lombardi)
                    Assert.AreEqual(Math.Round(weightOfExercise * (Math.Pow(repsOfExercise, 0.10)), 2), item.MaximumValue);

                if (item.TypeOfMethod == MethodType.Mayhew)
                    Assert.AreEqual(Math.Round((100.0 * weightOfExercise) / (52.2 + 41.9 * Math.Pow(Math.E, -0.055 * repsOfExercise)), 2), item.MaximumValue);

                if (item.TypeOfMethod == MethodType.OConner)
                    Assert.AreEqual(Math.Round(weightOfExercise * (1 + (repsOfExercise / 40.0)), 2), item.MaximumValue);

                if (item.TypeOfMethod == MethodType.Wathen)
                    Assert.AreEqual(Math.Round((100.0 * weightOfExercise) / (48.8 + 53.8 * Math.Pow(Math.E, -0.075 * repsOfExercise)), 2), item.MaximumValue);
            }
        }

        [DataTestMethod]
        [DataRow(MethodType.Epley, "Epley method")]
        [DataRow(MethodType.Brzycki, "Brzycki method")]
        [DataRow(MethodType.McGlothin, "McGlothin method")]
        [DataRow(MethodType.Lombardi, "Lombardi method")]
        [DataRow(MethodType.Mayhew, "Mayhew method")]
        [DataRow(MethodType.OConner, "O'Conner method")]
        [DataRow(MethodType.Wathen, "Wathen method")]
        public void MethodTypeNameTest(MethodType type, string name)
        {
            List<MaximumMethod> result = new List<MaximumMethod>(_repetitionMaximumModel.CalculateOneRepMaximums(ZERO_WEIGHT, ZERO_REPS));

            foreach (var item in result)
            {
                if (item.TypeOfMethod == type)
                    Assert.AreEqual(name, item.MethodName);
            }
        }

        private int GetLenthOfMethodTypeEnum()
        {
            int length = 0;

            foreach (MethodType method in Enum.GetValues(typeof(MethodType)))
            {
                ++length;
            }

            return length;
        }
    }
}