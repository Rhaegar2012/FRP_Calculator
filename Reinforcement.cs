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
        public struct Ce_notExposed
        {
            public const double Carbon = 1.0;
            public const double Glass = 0.8;
            public const double Aramid = 0.9;
        }
        public struct Ce_exposed
        {
            public const double Carbon = 0.9;
            public const double Glass = 0.7;
            public const double Aramid = 0.8;
        }

        private double Ce { get; set; }
        public double AssignCe(string ExposureCondition,string FiberType)
        {
            if(ExposureCondition=="Not exposed to earth/water")
            {
                switch (FiberType)
                {
                    case "Carbon":
                        Ce = Ce_notExposed.Carbon;
                        break;
                    case "Glass":
                        Ce = Ce_notExposed.Glass;
                        break;
                    case "Aramid":
                        Ce = Ce_notExposed.Aramid;
                        break;
                    default:
                        Ce = 0;
                        break;
                }
            }
            else
            {
                switch (FiberType)
                {
                    case "Carbon":
                        Ce = Ce_exposed.Carbon;
                        break;
                    case "Glass":
                        Ce=Ce_exposed.Glass;
                        break;
                    case "Aramid":
                        Ce = Ce_exposed.Aramid;
                        break;
                    default:
                        Ce = 0;
                        break;
                        
                }
            }
            Console.WriteLine("Ce " + Ce);
            return Ce;
        }
        public double AssignFFu(string rebarSize,string ExposureCondition, string FiberType)
        {
            Ce = AssignCe(ExposureCondition, FiberType);
            double returnValue;
            switch (rebarSize)
            {

                case "No 2":

                    return returnValue = Ce * (double)Fy.BarNo2;

                case "No 3":
                    return returnValue = Ce * (double)Fy.BarNo3;

                case "No 4":
                    return returnValue = Ce * (double)Fy.BarNo4;

                case "No 5":
                    return returnValue = Ce * (double)Fy.BarNo5;

                case "No 6":
                    return returnValue = Ce * (double)Fy.BarNo6;

                case "No 7":
                    return returnValue = Ce * (double)Fy.BarNo7;

                case "No 8":
                    return returnValue = Ce * (double)Fy.BarNo8;

                case "No 9":
                    return returnValue = Ce * (double)Fy.BarNo9;

                case "No 10":
                    return returnValue = Ce * (double)Fy.BarNo10;
                default:
                    return returnValue = 0;

            }

        }
    }
}
