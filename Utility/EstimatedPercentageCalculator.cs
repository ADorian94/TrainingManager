using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class EstimatedPercentageCalculator
    {
        private static Lazy<EstimatedPercentageCalculator> Lazy => new Lazy<EstimatedPercentageCalculator>(() => new EstimatedPercentageCalculator(), true);
        public static EstimatedPercentageCalculator Instance => Lazy.Value;

        private EstimatedPercentageCalculator()
        {

        }

        public double CalculateEstimated1RMPercentage(double originalWeight, double rpe, double reps, double percentage)
        {
            double exraReps = 10 - rpe;
            reps += exraReps;
            percentage /= 100; 

            return (originalWeight / (1 - ((reps * 2.5) / 100))) * percentage;
        }
    }
}
