using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRec.CoreEngine
{
    public interface IRecommender
    {
        double GetCorrelation(int[] baseData, int[] otherData);
    }

    public interface IDataPreprocessor
    {
        void Preprocess(ref int[] baseData, ref int[] otherData);
    }

    public class DataPreprocessor : IDataPreprocessor
    {
        public void Preprocess(ref int[] baseData, ref int[] otherData)
        {
            int maxLength = Math.Max(baseData.Length, otherData.Length);
            Array.Resize(ref baseData, maxLength);
            Array.Resize(ref otherData, maxLength);

            for (int i = 0; i < maxLength; i++)
            {
                if (baseData[i] == 0 || otherData[i] == 0)
                {
                    baseData[i] = 1;
                    otherData[i] = 1;
                }
            }
        }
    }

    public class PearsonRecommender : IRecommender
    {
        private readonly IDataPreprocessor _preprocessor;

        private PearsonRecommender(IDataPreprocessor preprocessor)
        {
            _preprocessor = preprocessor;
        }

        public double GetCorrelation(int[] baseData, int[] otherData)
        {
            _preprocessor.Preprocess(ref baseData, ref otherData);
            try {
                int n = baseData.Length;
                double sumBase = 0, sumOther = 0, sumBaseSq = 0, sumOtherSq = 0, pSum = 0;

                for (int i = 0; i < n; i++)
                {
                    sumBase += baseData[i];
                    sumOther += otherData[i];
                    sumBaseSq += Math.Pow(baseData[i], 2);
                    sumOtherSq += Math.Pow(otherData[i], 2);
                    pSum += baseData[i] * otherData[i];
                }

                double num = pSum - (sumBase * sumOther / n);
                double den = Math.Sqrt((sumBaseSq - Math.Pow(sumBase, 2) / n) * (sumOtherSq - Math.Pow(sumOther, 2) / n));

                if (den == 0)
                    throw new InvalidOperationException("Denominator is zero, correlation cannot be calculated.");

                return num / den;
            }
            catch (InvalidOperationException e)
            {
               
            }
            return 0;
        }


        public static PearsonRecommender Create(IDataPreprocessor preprocessor)
        {
            return new PearsonRecommender(preprocessor);
        }
    }

    public static class RecommenderFactory
    {
        public static IRecommender CreateRecommender()
        {
            IDataPreprocessor preprocessor = new DataPreprocessor();
            return PearsonRecommender.Create(preprocessor);
        }
    }
   
  
    //        int[] baseData = { 1, 2, 3, 4, 5 };
    //        int[] otherData = { 5, 4, 3, 2, 1 };

    
}
