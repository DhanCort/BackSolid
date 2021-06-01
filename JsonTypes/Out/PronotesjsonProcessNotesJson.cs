/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: March 05, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PronotesjsonProcessNotesJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public String strProcessName { get; set; }
        public int? intnPreviousJobId { get; set; }
        public int? intnPkWorkflow { get; set; }
        public String strJobName { get; set; }
        public NoteprojsonNoteProcessJson[] arrnotes { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PronotesjsonProcessNotesJson(
            String strProcessName_I,
            int? intnPreviousJobId_I,
            int? intnPkWorkflow_I,
            String strJobName_I,
            NoteprojsonNoteProcessJson[] arrnotes_I
            )
        {
            this.strProcessName = strProcessName_I;
            this.arrnotes = arrnotes_I;
            this.intnPreviousJobId = intnPreviousJobId_I;
            this.intnPkWorkflow = intnPkWorkflow_I;
            this.strJobName = strJobName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
