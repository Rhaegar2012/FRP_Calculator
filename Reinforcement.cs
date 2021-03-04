using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRP_Calculator_V0._0
{
    class Reinforcement
    {
        //Enumeration for FRP modulus of elasticity;
        public enum MOE
        {
            MOE=6700
        }

        //Enumeration for FRP yield Stress
        public enum Fy
        {
            BarNo2=110,
            BarNo3=110,
            BarNo4=100,
            BarNo5=95,
            BarNo6=90,
            BarNo7=90,
            BarNo8=80,
            BarNo9=75,
            BarNo10=70

        }
        public double AssignFFu(string rebarSize)
        {
            double returnValue;
            switch (rebarSize)
            {

                case "No 2":

                    return returnValue = 0.9 * (double)Fy.BarNo2;

                case "No 3":
                    return returnValue = 0.9 * (double)Fy.BarNo3;

                case "No 4":
                    return returnValue = 0.9 * (double)Fy.BarNo4;

                case "No 5":
                    return returnValue = 0.9 * (double)Fy.BarNo5;

                case "No 6":
                    return returnValue = 0.9 * (double)Fy.BarNo6;

                case "No 7":
                    return returnValue = 0.9 * (double)Fy.BarNo7;

                case "No 8":
                    return returnValue = 0.9 * (double)Fy.BarNo8;

                case "No 9":
                    return returnValue = 0.9 * (double)Fy.BarNo9;

                case "No 10":
                    return returnValue = 0.9 * (double)Fy.BarNo10;
                default:
                    return returnValue = 0;

            }

        }
    }
}
