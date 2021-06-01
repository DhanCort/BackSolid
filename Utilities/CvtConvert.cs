/*TASK RP.*/
using Odyssey2Backend.JsonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;

//                                                          //AUTHOR: Towa (CLGA-Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: March 31, 2020.

namespace Odyssey2Backend.Utilities
{
    //=================================================================================================================
    public static class CvtConvert
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR

        public static List<EqvjsoninEquivalenceJsonInternal> darreqvjsoninEquivalences;

        //--------------------------------------------------------------------------------------------------------------
        static CvtConvert()
        {
            //                                              //Init List.
            darreqvjsoninEquivalences = new List<EqvjsoninEquivalenceJsonInternal>();

            //                                              //Add all equivalences.

            //                                              //Length equivalences.
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "km", 1, "km"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "km", 1000, "m"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "km", 100000, "cm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "km", 1093.61, "yd"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "km", 3280.84, "ft"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "km", 39370.1, "in"));

            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "ft", 1, "ft"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "ft", 304800, "µm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "ft", 304.8, "mm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "ft", 30.48, "cm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "ft", 12, "in"));

            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "m", 1, "m"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "m", 100, "cm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "m", 1000, "mm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "m", 3.28084, "ft"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "m", 39.3701, "in"));

            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "in", 1, "in"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "in", 2.54, "cm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "in", 25.4, "mm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "in", 25400, "µm"));

            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "cm", 1, "cm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "cm", 10, "mm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "cm", 10000, "µm"));

            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "mm", 1, "mm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "mm", 1000, "µm"));

            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "µm", 1, "µm"));
            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "µm", 1000, "nm"));

            darreqvjsoninEquivalences.Add(new EqvjsoninEquivalenceJsonInternal(1, "nm", 1, "nm"));
        }

        public static double to(
            //                                              //convert input (25 cm) to ?(toConvertUnit).

            double numInputValue_I, 
            String strInputUnit_I, 
            String strToConvertUnit_I
            )
        {
            double numResConvert = -1;

            //                                              //Get the equivalences.
            EqvjsoninEquivalenceJsonInternal eqvjsoninEquivalence = darreqvjsoninEquivalences.FirstOrDefault(eqv =>
                (eqv.strEqvUnitTop == strInputUnit_I && eqv.strEqvUnitBottom == strToConvertUnit_I) 
                    || 
                (eqv.strEqvUnitTop == strToConvertUnit_I && eqv.strEqvUnitBottom == strInputUnit_I)
                );

            if (
                //                                          //The equivalences exist.
                eqvjsoninEquivalence != null
                )
            {
                if (
                    //                                      //valueTop          : 1 in           ?? 
                    //                                      //                    ----     =    -----
                    //                                      //valueBottom       : 2.54 cm       20 cm (strInputUnit)
                    strInputUnit_I == eqvjsoninEquivalence.strEqvUnitBottom
                    )
                {
                    numResConvert = (numInputValue_I * eqvjsoninEquivalence.numEqvTop) / 
                        eqvjsoninEquivalence.numEqvBottom;
                }
                else
                {
                    numResConvert = (numInputValue_I * eqvjsoninEquivalence.numEqvBottom) /
                        eqvjsoninEquivalence.numEqvTop;
                }
            }

            return numResConvert;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
