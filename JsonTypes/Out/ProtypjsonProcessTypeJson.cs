/*TASK RP.JsonTypes*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 23, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ProtypjsonProcessTypeJson : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strTypeId { get; set; }
        public bool boolHasIt { get; set; }
        public String strClassification { get; set; }
        public bool boolIsCommon { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public ProtypjsonProcessTypeJson(
            int intPk_I,
            String strProcessTypeId_I,
            bool boolHasIt_I,
            String strClassification_I,
            bool boolIsCommon_I
            )
        {
            this.intPk = intPk_I;
            this.strTypeId = strProcessTypeId_I;
            this.boolHasIt = boolHasIt_I;
            this.strClassification = strClassification_I;
            this.boolIsCommon = boolIsCommon_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            ProtypjsonProcessTypeJson protypjson = (ProtypjsonProcessTypeJson)obj_I;
                        
            return this.strTypeId.CompareTo(protypjson.strTypeId);
        }
    }

    //==================================================================================================================
}
/*END-TASk*/
