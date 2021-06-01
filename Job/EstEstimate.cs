/*TASK RP. CUSTOMER*/
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Alert;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez.).
//                                                          //DATE: August 07, 2020.

namespace Odyssey2Backend.Job
{
    //==================================================================================================================
    public class EstEstimate
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR.

        //--------------------------------------------------------------------------------------------------------------
        public EstEstimate(
            //                                              //To Define.
            )
        {

        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolAreAllPeriodsAddables(
            //                                              //Return true if the periods are addable.
            //                                              //    and false if any period is addable
            //                                              //    by exception with password.
            //                                              //Code Error if there is any period
            //                                              //    that is not addable.

            PsPrintShop ps_I,
            //                                              //Data to verify if are addable.
            List<EstjsonEstimationDataJson> darrestjson_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I,
            out List<int> darrintPkPeriodsByException_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List intPkPeriod by exception
            darrintPkPeriodsByException_O = new List<int>();

            bool boolAreAllPeriodsAddables = true;
            int? intnJobId;
            int? intnEstimate;
            int? intnPkWorkflow;

            EstEstimate.subValidateEstimateData(ps_I.strPrintshopId, darrestjson_I, configuration_I,
                out intnJobId, out intnPkWorkflow, out intnEstimate, ref intStatus_IO, ref strUserMessage_IO, 
                ref strDevMessage_IO);

            if (
                //                                          //darrresjson is valid.
                intStatus_IO == 200
                )
            {
                //                                          //Get PIW for this workflow.
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = context_M.ProcessInWorkflow.Where(piw =>
                    piw.intPkWorkflow == (int)intnPkWorkflow).ToList();

                //                                          //Get all period for this estimate.
                List<PerentityPeriodEntityDB> darrperentityAllPeriod = context_M.Period.Where(per => 
                    per.intJobId == intnJobId && per.intPkWorkflow == intnPkWorkflow && 
                    per.intnEstimateId == intnEstimate).ToList();

                //                                          //Get process periods.
                List<PerentityPeriodEntityDB> darrperentityProcessPeriod = darrperentityAllPeriod.Where(
                    per => per.intnPkElementElement == null && per.intnPkElementElement == null &&
                    per.intnPkCalculation != null).ToList();

                //                                          //Get ressource periods.
                List<PerentityPeriodEntityDB> darrperentityResourcePeriod = darrperentityAllPeriod.Where(
                    per => per.intnPkElementElementType != null || per.intnPkElementElement != null).ToList();

                //                                          //Verify that each Process'Period is addable.
                int intI = 0;
                /*WHILE-DO*/
                while (
                    //                                      //The status is 200.
                    intStatus_IO == 200 &&
                    //                                      //IntI is less the lenght arr.
                    intI < darrperentityProcessPeriod.Count
                    )
                {
                    //                                      //Get the PKProcessInWorkflow of the period from 
                    //                                      //    the DB.
                    int intPkPIW = darrpiwentity.FirstOrDefault(piw =>
                        piw.intPkWorkflow == intnPkWorkflow &&
                        piw.intPkProcess == darrperentityProcessPeriod[intI].intPkElement &&
                        piw.intProcessInWorkflowId == darrperentityProcessPeriod[intI].intProcessInWorkflowId).intPk;

                    //                                      //Method that verify if the period is addable.
                    PerisaddablejsonPeriodIsAddableJson perisaddablejson;
                    ProProcess.subPeriodIsAddable(ps_I.strPrintshopId, (int)intnJobId, 
                        darrperentityProcessPeriod[intI].intPk, intPkPIW, 
                        (int)darrperentityProcessPeriod[intI].intnPkCalculation, null,
                        darrperentityProcessPeriod[intI].strStartDate, darrperentityProcessPeriod[intI].strStartTime,
                        darrperentityProcessPeriod[intI].strEndDate, darrperentityProcessPeriod[intI].strEndTime,
                        true, configuration_I, out perisaddablejson, ref intStatus_IO, ref strDevMessage_IO,
                        ref strUserMessage_IO);

                    if (
                        intStatus_IO == 200
                        )
                    {
                        if (
                            //                              //Period by exception.
                            !perisaddablejson.boolIsAddableAboutRules
                        )
                        {
                            //                              //Is addable by exception.
                            boolAreAllPeriodsAddables = false;
                            darrintPkPeriodsByException_O.Add(darrperentityProcessPeriod[intI].intPk);
                        }
                    }

                    intI = intI + 1;
                }

                //                                          //Verify that each Resource'Period is addable.
                intI = 0;
                /*WHILE-DO*/
                while (
                    //                                      //The status is 200.
                    intStatus_IO == 200 &&
                    //                                      //IntI is less the lenght arr.
                    intI < darrperentityResourcePeriod.Count
                    )
                {
                    //                                      //Get the PKProcessInWorkflow of the period from 
                    //                                      //    the DB.
                    int intPkPIW = darrpiwentity.FirstOrDefault(piw =>
                        piw.intPkWorkflow == intnPkWorkflow &&
                        piw.intProcessInWorkflowId == darrperentityResourcePeriod[intI].intProcessInWorkflowId).intPk;

                    //                                      //Method that verify if the Resource'period is addable.
                    Perisaddablejson2PeriodIsAddableJson2 perisaddablejson;
                    ResResource.subPeriodIsAddable(ps_I.strPrintshopId, darrperentityResourcePeriod[intI].intPk,
                        darrperentityResourcePeriod[intI].intPkElement, null, 
                        darrperentityResourcePeriod[intI].strStartDate,
                        darrperentityResourcePeriod[intI].strStartTime, darrperentityResourcePeriod[intI].strEndDate,
                        darrperentityResourcePeriod[intI].strEndTime, (int)intnJobId, intPkPIW, true,
                        configuration_I, out perisaddablejson, ref intStatus_IO, ref strUserMessage_IO, 
                        ref strDevMessage_IO);

                    if (
                        intStatus_IO == 200
                        )
                    {
                        if (
                        //                                  //Period by exception.
                        !perisaddablejson.boolIsAddableAboutRules
                        )
                        {
                            //                              //Is addable by exception.
                            boolAreAllPeriodsAddables = false;
                            darrintPkPeriodsByException_O.Add(darrperentityResourcePeriod[intI].intPk);
                        }
                    }

                    intI = intI + 1;
                }
            }

            if (
                //                                          
                intStatus_IO != 200
                )
            {
                strUserMessage_IO = "Estimation cannot be confirmed.";
                strDevMessage_IO = "Is Not Addable.";

                EstEstimate.subDeleteEstimationData((int)intnJobId, (int)intnPkWorkflow, (int)intnEstimate);
            }
            else
            {
                if (
                    !boolAreAllPeriodsAddables
                    )
                {
                    strUserMessage_IO = "You will need a password to set the period.";
                    strDevMessage_IO = "You will need a password to set the period.";
                }
            }

            return boolAreAllPeriodsAddables;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subValidateEstimateData(
            //                                              //Valid if the arrrestjson is valid.
            //                                              //    Code 200 mean that arrrestjson is valid.
            //                                              //    ErrorCode mean that arrrestjson is not valid.

            String strPrintshopId_I,
            List<EstjsonEstimationDataJson> darrestjson_I,
            IConfiguration configuration_I,
            out int? intnJobId_O, 
            out int? intnPkWorkflow_O,
            out int? intnEstimateId_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intnJobId_O = null;
            intnEstimateId_O = null;
            intnPkWorkflow_O = null;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Nothing to confirm.";
            if (
                darrestjson_I.Count > 0
                )
            {
                //                                          //Job data.
                int intJobId = darrestjson_I[0].intJobId;
                JobjsonJobJson jobjsonJob;

                intStatus_IO = 402;
                if (
                    JobJob.boolIsValidJobId(intJobId, strPrintshopId_I, configuration_I, out jobjsonJob,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    //                                  //Establish connection.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Asign to JobId to the var out.
                    intnJobId_O = intJobId;

                    int intPkWorkflow;
                    if (
                        JobJob.boolAllProcessInWorkflowAreFromTheSameWf(darrestjson_I, context, out intPkWorkflow)
                        )
                    {
                        intnPkWorkflow_O = intPkWorkflow;

                        //                                  //Get the baseDate and baseTime.
                        int intEstimationId = darrestjson_I[0].intEstimationId;
                        intnEstimateId_O = intEstimationId;

                        EstentityEstimateEntityDB estentity = context.Estimate.FirstOrDefault(est =>
                            est.intJobId == intJobId &&
                            est.intPkWorkflow == intPkWorkflow &&
                            est.intId == intEstimationId);

                        intStatus_IO = 403;
                        strUserMessage_IO = "Estimate not found.";
                        strDevMessage_IO = "";
                        if (
                            estentity != null
                            )
                        {
                            ZonedTime ztimeBase = ZonedTimeTools.NewZonedTime(estentity.strBaseDate.ParseToDate(),
                                estentity.strBaseTime.ParseToTime());

                            intStatus_IO = 404;
                            strUserMessage_IO = "Select valid date or time.";
                            strDevMessage_IO = "";
                            if (
                                ztimeBase > ZonedTimeTools.ztimeNow
                                )
                            {
                                //                          //List of data in EstimationData table for the job.
                                List<EstdataentityEstimationDataEntityDB> darrestdataentityForJobAndPiwAndId =
                                    (
                                    from estdata in context.EstimationData
                                    join piwentity in context.ProcessInWorkflow
                                    on estdata.intPkProcessInWorkflow equals piwentity.intPk
                                    where estdata.intId == intEstimationId &&
                                    piwentity.intPkWorkflow == intPkWorkflow &&
                                    estdata.intJobId == intJobId
                                    select estdata).ToList();

                                intStatus_IO = 405;
                                strUserMessage_IO = "Data to confirm has changed, please try another estimation.";
                                strDevMessage_IO = "Data is defferent in database, probably has changed or is invalid.";
                                if (
                                    JobJob.boolValidEstimationDataInDB(darrestdataentityForJobAndPiwAndId, darrestjson_I)
                                    )
                                {
                                    intStatus_IO = 200;
                                    strUserMessage_IO = "";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteEstimationData(
            //                                              //Delete Estimate table, EstimationData and temporary 
            //                                              //    period. 

            int intJobId_I,
            int intPkWorkflow_I,
            int intEstimateId_I
            )
        {
            //                                              //Establish the connection to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List of data in EstimationData table for the job and the wf.
            List<EstdataentityEstimationDataEntityDB> darrestdataentityForJobAndPiw =
                (
                from estdata in context.EstimationData
                join piwentity in context.ProcessInWorkflow
                on estdata.intPkProcessInWorkflow equals piwentity.intPk
                where piwentity.intPkWorkflow == intPkWorkflow_I &&
                estdata.intJobId == intJobId_I &&
                estdata.intId == intEstimateId_I
                select estdata).ToList();

            //                                              //Delete estimation data for the job and wf.
            foreach (EstdataentityEstimationDataEntityDB estdata in darrestdataentityForJobAndPiw)
            {
                context.EstimationData.Remove(estdata);
            }

            //                                              //Find estimates.
            List<EstentityEstimateEntityDB> darrestentityForJObAndPiw = context.Estimate.Where(est =>
                est.intJobId == intJobId_I && est.intPkWorkflow == intPkWorkflow_I && 
                est.intId == intEstimateId_I).ToList();

            //                                              //Delete estimate for the job and workflow.
            foreach (EstentityEstimateEntityDB est in darrestentityForJObAndPiw)
            {
                context.Estimate.Remove(est);
            }

            //                                              //Get all period for this Job, wf and estimate.
            List<PerentityPeriodEntityDB> darrperentityAllPeriod = context.Period.Where(per =>
                per.intJobId == intJobId_I && per.intPkWorkflow == intPkWorkflow_I &&
                per.intnEstimateId == intEstimateId_I).ToList();

            //                                              //Delete period temporary for this estimate.
            foreach (PerentityPeriodEntityDB perentity in darrperentityAllPeriod)
            {
                context.Period.Remove(perentity);
            }

            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
