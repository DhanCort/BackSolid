/*TASK RP.JsonTypes*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 16, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EtjsonElementTypeJson : IComparable
    {
        public int intPk { get; set; }
        public String strTypeId { get; set; }
        public bool boolHasIt { get; set; }
        public String strClassification { get; set; }
        public bool? boolnUsage { get; set; }
        public bool boolIsPhysical { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            EtjsonElementTypeJson etjson = (EtjsonElementTypeJson)obj_I;
                        
            return this.strTypeId.CompareTo(etjson.strTypeId);
        }
    }

    //==================================================================================================================
}
/*END-TASk*/
