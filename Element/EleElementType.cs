/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 10, 2019. 

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public class EletemElementType : EtElementTypeAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public EletemElementType(
            //                                              //Primary key of the type.
            int intPk_I,
            //                                              //Specific type of when it is a XJDF type, it 
            //                                              //      can be empty string when this is a printshop 
            //                                              //      type.
            String strXJDFTypeId_I,
            //                                              //Added by: XJDFX.X, MI4P or printshop id.
            String strAddedBy_I,
            //                                              //Modified by: XJDFX.X, MI4P or printshop id
            int? intPkPrintshop_I,
            //                                              //Custom type id.
            String strCustomTypeId_I,
            //                                              //Element Classification.
            String strClassification_I
            )
            : base(intPk_I, strXJDFTypeId_I, strAddedBy_I, intPkPrintshop_I, strCustomTypeId_I, 
                  EtElementTypeAbstract.strElement, strClassification_I)
        {
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/