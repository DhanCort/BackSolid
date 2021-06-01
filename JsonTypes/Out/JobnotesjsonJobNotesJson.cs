/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: January 05, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobnotesjsonJobNotesJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkNote { get; set; }
        public String strWisnetNote { get; set; }
        public String strOdyssey2Note { get; set; }
        public int? intnPreviousJobId { get; set; }
        public int? intnPkWorkflow { get; set; }
        public String strJobName { get; set; }
        public PronotesjsonProcessNotesJson[] arrpronotes { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public JobnotesjsonJobNotesJson(
            int intPkNote_I,
            String strWisnetNote_I,
            String strOdyssey2Note_I,
            int? intnPreviousJobId_I,
            int? intnPkWorkflow_I,
            String strJobName_I,
            PronotesjsonProcessNotesJson[] arrpronotes_I
            )
        {
            this.intPkNote = intPkNote_I;
            this.strWisnetNote = strWisnetNote_I;
            this.strOdyssey2Note = strOdyssey2Note_I;
            this.intnPreviousJobId = intnPreviousJobId_I;
            this.intnPkWorkflow = intnPkWorkflow_I;
            this.strJobName = strJobName_I;
            this.arrpronotes = arrpronotes_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
