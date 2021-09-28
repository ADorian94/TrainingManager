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

                

                //...
            }
        }

        [DataTestMethod]
        [DataRow(MethodType.Epley, "Epley method")]
        [DataRow(MethodType.Brzycki, "Brzycki method")]
        //...
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