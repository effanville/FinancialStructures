﻿namespace FinancialStructures.MathLibrary.Functions
{
    public class GaussLegendreQuadrature
    {
        public readonly int NumberValues = 18;
        public readonly double[] Abscissa = new double[]
            { 0.0021695375159141994,
              0.011413521097787704,
              0.027972308950302116,
              0.051727015600492421,
              0.082502225484340941,
              0.12007019910960293,
              0.16415283300752470,
                0.21442376986779355,
                0.27051082840644336,
                0.33199876341447887,
                0.39843234186401943,
                0.46931971407375483,
                0.54413605556657973,
                                                0.62232745288031077,
                0.70331500465597174, 0.78649910768313447,
                                                0.87126389619061517, 0.95698180152629142 };
        public readonly double[] Weights = new double[] {0.0055657196642445571,
                                                0.012915947284065419,0.020181515297735382,0.027298621498568734,
                                                0.034213810770299537,0.040875750923643261,0.047235083490265582,
                                                0.053244713977759692,0.058860144245324798,0.064039797355015485,
                                                0.068745323835736408,0.072941885005653087,0.076598410645870640,
                                                0.079687828912071670,0.082187266704339706,0.084078218979661945,
                                                0.085346685739338721,0.085983275670394821};
    }
}
