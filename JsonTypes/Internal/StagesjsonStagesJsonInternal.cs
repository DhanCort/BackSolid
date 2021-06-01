/*TASK RP.JSON*/
using System;
using System.Collections.Generic;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: September 30, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class StagesjsonStagesJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPrintshopId { get; set; }
        public bool? boolnUnsubmitted { get; set; }
        public bool? boolnInEstimating { get; set; }
        public bool? boolnWaitingForPriceApproval { get; set; }
        public bool? boolnPending { get; set; }
        public bool? boolnInProgress { get; set; }
        public bool? boolnCompleted { get; set; }
        public bool? boolnWaitingForPayment { get; set; }
        public bool? boolnAll { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public StagesjsonStagesJsonInternal(
            int intPrintshopId_I,
            bool? boolnUnsubmitted_I,
            bool? boolnInEstimating_I,
            bool? boolnWaitingForPriceApproval_I,
            bool? boolnPendingStage_I,
            bool? boolnInProgressStage_I,
            bool? boolnCompletedStage_I,
            bool? boolnNotPaid_I, 
            bool? boolnAll_I
            )
        {
            this.intPrintshopId = intPrintshopId_I;
            this.boolnUnsubmitted = boolnUnsubmitted_I;
            this.boolnInEstimating = boolnInEstimating_I;
            this.boolnWaitingForPriceApproval = boolnWaitingForPriceApproval_I;
            this.boolnPending = boolnPendingStage_I;
            this.boolnInProgress = boolnInProgressStage_I;
            this.boolnCompleted = boolnCompletedStage_I;
            this.boolnWaitingForPayment = boolnNotPaid_I;
            this.boolnAll = boolnAll_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
