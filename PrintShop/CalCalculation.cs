/*TASK RP. CALCULATION*/
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Job;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;
using Odyssey2Backend.Customer;

//                                                          //AUTHOR: Towa (CLGA-Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 21, 2019.

namespace Odyssey2Backend.PrintShop
{
    //==================================================================================================================
    public class CalCalculation : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        public const String strBase = "B";
        public const String strPerUnit = "PU";
        public const String strPerQuantity = "PQ";
        public const String strPerQuantityBase = "BQ";
        public const String strProfit = "P";

        public const String strByProduct = "BPROD";
        public const String strByIntent = "BI";
        public const String strByProcess = "BPROC";
        public const String strByResource = "BRES";


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //                                                  //Primary key in the database.
        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }

        private String strUnit_Z;
        public String strUnit { get { return this.strUnit_Z; } }

        private double? numnQuantity_Z;
        public double? numnQuantity { get { return this.numnQuantity_Z; } }

        private double? numnBlock_Z;
        public double? numnBlock { get { return this.numnBlock_Z; } }

        private bool boolIsEnable_Z;
        public bool boolIsEnable { get { return this.boolIsEnable_Z; } }

        private double? numnCost_Z;
        public double? numnCost { get { return this.numnCost_Z; } }

        private int? intnHours_Z;
        public int? intnHours { get { return this.intnHours_Z; } }

        private int? intnMinutes_Z;
        public int? intnMinutes { get { return this.intnMinutes_Z; } }

        private int? intnSeconds_Z;
        public int? intnSeconds { get { return this.intnSeconds_Z; } }

        private String strValue_Z;
        public String strValue { get { return this.strValue_Z; } }

        private String strAscendants_Z;
        public String strAscendants { get { return this.strAscendants_Z; } }

        private String strDescription_Z;
        public String strDescription { get { return this.strDescription_Z; } }

        private double? numnProfit_Z;
        public double? numnProfit { get { return this.numnProfit_Z; } }

        private String strByX_Z;
        public String strByX { get { return this.strByX_Z; } }

        private String strStartDate_Z;
        public String strStartDate { get { return this.strStartDate_Z; } }

        private String strStartTime_Z;
        public String strStartTime { get { return this.strStartTime_Z; } }

        private String strEndDate_Z;
        public String strEndDate { get { return this.strEndDate_Z; } }

        private String strEndTime_Z;
        public String strEndTime { get { return this.strEndTime_Z; } }

        private int? intnPkProductTypeBelongsTo_Z;
        public int? intnPkProductTypeBelongsTo { get { return this.intnPkProductTypeBelongsTo_Z; } }

        private int? intnPkProcessElementBelongsTo_Z;
        public int? intnPkProcessElementBelongsTo { get { return this.intnPkProcessElementBelongsTo_Z; } }

        private int? intnPkResourceElementBelongsTo_Z;
        public int? intnPkResourceElementBelongsTo { get { return this.intnPkResourceElementBelongsTo_Z; } }

        private int? intnPkWorkflowBelongsTo_Z;
        public int? intnPkWorkflowBelongsTo { get { return this.intnPkWorkflowBelongsTo_Z; } }

        private int? intnProcessInWorkflowId_Z;
        public int? intnProcessInWorkflowId { get { return this.intnProcessInWorkflowId_Z; } }

        private int? intnPkElementElementTypeBelongsTo_Z;
        public int? intnPkElementElementTypeBelongsTo { get { return this.intnPkElementElementTypeBelongsTo_Z; } }

        private int? intnPkElementElementBelongsTo_Z;
        public int? intnPkElementElementBelongsTo { get { return this.intnPkElementElementBelongsTo_Z; } }

        private String strCalculationType_Z;
        public String strCalculationType { get { return this.strCalculationType_Z; } }

        private double? numnNeeded_Z;
        public double? numnNeeded { get { return this.numnNeeded_Z; } }

        private double? numnPerUnits_Z;
        public double? numnPerUnits { get { return this.numnPerUnits_Z; } }

        private double? numnMin_Z;
        public double? numnMin { get { return this.numnMin_Z; } }

        private double? numnQuantityWaste_Z;
        public double? numnQuantityWaste { get { return this.numnQuantityWaste_Z; } }

        private double? numnPercentWaste_Z;
        public double? numnPercentWaste { get { return this.numnPercentWaste_Z; } }

        private int? intnPkQFromElementElementTypeBelongsTo_Z;
        public int? intnPkQFromElementElementTypeBelongsTo { get { return this.intnPkQFromElementElementTypeBelongsTo_Z; } }

        private int? intnPkQFromElementElementBelongsTo_Z;
        public int? intnPkQFromElementElementBelongsTo { get { return this.intnPkQFromElementElementBelongsTo_Z; } }

        private int? intnPkQFromResourceElementBelongsTo_Z;
        public int? intnPkQFromResourceElementBelongsTo { get { return this.intnPkQFromResourceElementBelongsTo_Z; } }

        private int? intnPkAccount_Z;
        public int? intnPkAccount { get { return this.intnPkAccount_Z; } }

        private bool? boolnFromThickness_Z;
        public bool? boolnFromThickness { get { return this.boolnFromThickness_Z; } }

        private bool? boolnIsBlock_Z;
        public bool? boolnIsBlock { get { return this.boolnIsBlock_Z; } }

        private bool? boolnByArea_Z;
        public bool? boolnByArea { get { return this.boolnByArea_Z; } }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DYNAMIC VARIABLES.

        //                                                  //Group of the calculation.
        public int? intnGroup_Z;
        public int? intnGroup
        {
            get
            {
                this.subGetGroupFromDB(out this.intnGroup_Z);
                return this.intnGroup_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //SUPPORT METHODS FOR DYNAMIC VARIABLES. 

        //--------------------------------------------------------------------------------------------------------------
        private void subGetGroupFromDB(

            out int? intnGroup_O
            )
        {
            //                                              //Initialize with null.
            intnGroup_O = null;
            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all relations for this.
            GpcalentityGroupCalculationEntityDB gpetentity =
                context.GroupCalculation.FirstOrDefault(gpetentity =>
                gpetentity.intPkCalculation == this.intPk);

            if (
                gpetentity != null
                )
            {
                intnGroup_O = gpetentity.intId;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR.

        //--------------------------------------------------------------------------------------------------------------
        public CalCalculation(
            int intPK_I,
            String strUnit_I,
            double? numnQuantity_I,
            double? numnCost_I,
            int? intnHours_I,
            int? intnMinutes_I,
            int? intnSeconds_I,
            double? numnBlock_I,
            bool boolIsEnable_I,
            String strValue_I,
            String strAscendantElements_I,
            String strDescription_I,
            double? numnProfit_I,
            int? intnProductTypeBelongsTo_I,
            int? intnProcessTypeBelongsTo_I,
            int? intnResourceElementBelongsTo_I,
            String strCalculationType_I,
            String strByX_I,
            String strStarDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            double? numnNeeded_I,
            double? numnPerUnits_I,
            double? numnMin_I,
            double? numnQuantityWaste_I,
            double? numnPercentWaste_I,
            int? intnPkWorkflowBelongsTo_I,
            int? intnProcessInWorkflowId_I,
            int? intnPkElementElementTypeBelongsTo_I,
            int? intnPkElementElementBelongsTo_I,
            int? intnPkQFromElementElementTypeBelongsTo_I,
            int? intnPkQFromElementElementBelongsTo_I,
            int? intnPkQFromResourceElementBelongsTo_I,
            int? intnPkAccount_I,
            bool? boolnFromThickness_I,
            bool? boolnIsBlock_I,
            bool? boolnByArea_I
            )
        {
            this.intPk_Z = intPK_I;
            this.strUnit_Z = strUnit_I;
            this.numnQuantity_Z = numnQuantity_I;
            this.numnCost_Z = numnCost_I;
            this.intnHours_Z = intnHours_I;
            this.intnMinutes_Z = intnMinutes_I;
            this.intnSeconds_Z = intnSeconds_I;
            this.numnBlock_Z = numnBlock_I;
            this.boolIsEnable_Z = boolIsEnable_I;
            this.strValue_Z = strValue_I;
            this.strAscendants_Z = strAscendantElements_I;
            this.strDescription_Z = strDescription_I;
            this.numnProfit_Z = numnProfit_I;
            this.intnPkProductTypeBelongsTo_Z = intnProductTypeBelongsTo_I;
            this.intnPkProcessElementBelongsTo_Z = intnProcessTypeBelongsTo_I;
            this.intnPkResourceElementBelongsTo_Z = intnResourceElementBelongsTo_I;
            this.strCalculationType_Z = strCalculationType_I;
            this.strByX_Z = strByX_I;
            this.strStartDate_Z = strStarDate_I;
            this.strStartTime_Z = strStartTime_I;
            this.strEndDate_Z = strEndDate_I;
            this.strEndTime_Z = strEndTime_I;
            this.numnNeeded_Z = numnNeeded_I;
            this.numnPerUnits_Z = numnPerUnits_I;
            this.numnMin_Z = numnMin_I;
            this.numnQuantityWaste_Z = numnQuantityWaste_I;
            this.numnPercentWaste_Z = numnPercentWaste_I;
            this.intnPkWorkflowBelongsTo_Z = intnPkWorkflowBelongsTo_I;
            this.intnProcessInWorkflowId_Z = intnProcessInWorkflowId_I;
            this.intnPkElementElementTypeBelongsTo_Z = intnPkElementElementTypeBelongsTo_I;
            this.intnPkElementElementBelongsTo_Z = intnPkElementElementBelongsTo_I;
            this.intnPkQFromElementElementTypeBelongsTo_Z = intnPkQFromElementElementTypeBelongsTo_I;
            this.intnPkQFromElementElementBelongsTo_Z = intnPkQFromElementElementBelongsTo_I;
            this.intnPkQFromResourceElementBelongsTo_Z = intnPkQFromResourceElementBelongsTo_I;
            this.intnPkAccount_Z = intnPkAccount_I;
            this.boolnFromThickness_Z = boolnFromThickness_I;
            this.boolnIsBlock_Z = boolnIsBlock_I;
            this.boolnByArea_Z = boolnByArea_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddCalculation(
            //                                              //To add a calculation to de DB.

            PsPrintShop ps_I,
            int? intnJobId_I,
            String strUnit_I,
            double? numnQuantity_I,
            double? numnCost_I,
            double? numnBlock_I,
            bool boolIsEnable_I,
            String strValue_I,
            String strAscendantElements_I,
            String strDescription_I,
            double? numnProfit_I,
            String strCalculationType_I,
            GpcondjsonGroupConditionJson gpcondCondition_I,
            int? intnPkProduct_I,
            int? intnPkProcess_I,
            int? intnPkResourceI_I,
            int? intnPkProcessInWorkflow_I,
            int? intnPkEleetOrEleeleI_I,
            bool? boolnIsEleetI_I,
            double? numnNeeded_I,
            double? numnPerUnits_I,
            String strByX_I,
            double? numnMin_I,
            double? numnQuantityWaste_I,
            double? numnPercentWaste_I,
            int? intnPkEleetOrEleeleO_I,
            bool? boolnIsEleetO_I,
            int? intnPkResourceO_I,
            IConfiguration configuration_I,
            int? intnPkAccount_I,
            bool? boolnFromThickness_I,
            bool? boolnIsBlock_I,
            bool? boolnByArea_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int? intnPkProcess = intnPkProcess_I;
            bool boolValid = false;

            int? intnPkElementElementTypeI = null;
            int? intnPkElementElementI = null;

            int? intnPkElementElementTypeO = null;
            int? intnPkElementElementO = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //To easy code.
            PiwentityProcessInWorkflowEntityDB piwentity = null;

            /*CASE*/
            if (
                //                                          //Calculation from wf, for a res in an IO (option 1).
                (intnPkProcessInWorkflow_I != null) &&
                (boolnIsEleetI_I != null) &&
                (intnPkEleetOrEleeleI_I != null) &&
                (intnPkResourceI_I != null)
                )
            {
                //                                          //Get the piw.
                piwentity = context.ProcessInWorkflow.FirstOrDefault(piw => piw.intPk == intnPkProcessInWorkflow_I);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Process in workflow not found.";
                if (
                    piwentity != null
                    )
                {
                    intStatus_IO = 402;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "boolEleetI or boolEleetO is null.";

                    if (
                        (boolnIsEleetI_I != null &&
                        boolnIsEleetO_I != null) ||
                        (boolnIsEleetI_I != null &&
                        strCalculationType_I == CalCalculation.strPerQuantityBase)
                        )
                    {
                        //                                  //Assign Value eleetI or eleeleI.
                        intnPkElementElementTypeI = boolnIsEleetI_I == true ? intnPkEleetOrEleeleI_I : null;
                        intnPkElementElementI = boolnIsEleetI_I == true ? null : intnPkEleetOrEleeleI_I;

                        //                                  //Assign Value eleetO or eleeleO.
                        intnPkElementElementTypeO = boolnIsEleetO_I == true ? intnPkEleetOrEleeleO_I : null;
                        intnPkElementElementO = boolnIsEleetO_I == true ? null : intnPkEleetOrEleeleO_I;

                        //                                  //Get the Eleet Input.
                        EleetentityElementElementTypeEntityDB eleetentityI = context.ElementElementType.FirstOrDefault(
                            eleet => eleet.intPk == intnPkElementElementTypeI);

                        //                                  //Get the Eleele Input.
                        EleeleentityElementElementEntityDB eleeleentityI = context.ElementElement.FirstOrDefault(
                            eleele => eleele.intPk == intnPkElementElementI);

                        //                                  //Get the Eleet Output.
                        EleetentityElementElementTypeEntityDB eleetentityO = context.ElementElementType.FirstOrDefault(
                            eleet => eleet.intPk == intnPkElementElementTypeO);

                        //                                  //Get the Eleele Output.
                        EleeleentityElementElementEntityDB eleeleentityO = context.ElementElement.FirstOrDefault(
                            eleele => eleele.intPk == intnPkElementElementO);

                        intStatus_IO = 402;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "EleetI or EleeleI or EleetO or EleeleO not found";
                        if (
                            //                              //Valid eleetoreleele input.
                            (((eleetentityI != null ||
                            eleeleentityI != null)) &&
                            //                              //Valid eleetoreleele output.
                            ((eleetentityO != null ||
                            eleeleentityO != null)))
                            )
                        {

                            intStatus_IO = 403;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "EleetOrEleeleI is not an input.";
                            if (
                                //                          //EleetOrEleeleI is an input.
                                ((eleetentityI != null) && (eleetentityI.boolUsage)) ||
                                ((eleeleentityI != null) && (eleeleentityI.boolUsage))
                                )
                            {
                                IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(
                                    io => io.intPkWorkflow == piwentity.intPkWorkflow &&
                                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                    io.intnPkElementElementType == intnPkElementElementTypeI &&
                                    io.intnPkElementElement == intnPkElementElementI);

                                intStatus_IO = 403;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Piw needs to be normal or EleetOrEleeleI needs to be input " +
                                    "without link.";
                                if (
                                    //                      //Piw is normal.
                                    (!piwentity.boolIsPostProcess) ||
                                    //                      //Piw is post process but io has link.
                                    ((ioentity == null) || (ioentity.strLink == null))
                                    )
                                {
                                    intStatus_IO = 404;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "EleetI or EleeleI or EleetO or EleeleO process and " +
                                        "PrcessInWorkflow process are not the same.";
                                    if (
                                        //                  //Valid that eleet eleele input or output, it is from 
                                        //                  //   same process.
                                        (piwentity.intPkProcess == (boolnIsEleetI_I == true ?
                                        eleetentityI.intPkElementDad : eleeleentityI.intPkElementDad)) &&
                                        (piwentity.intPkProcess == (boolnIsEleetO_I == true ?
                                        eleetentityO.intPkElementDad : eleeleentityO.intPkElementDad))
                                        )
                                    {
                                        //                  //GetResource Output.
                                        ResResource resO = ResResource.resFromDB(intnPkResourceO_I, false);

                                        intStatus_IO = 405;
                                        strUserMessage_IO = "Something is wrong.";
                                        strDevMessage_IO = "Resource output is not found.";
                                        if (
                                            resO != null
                                            )
                                        {
                                            //              //List Of process inputs.
                                            List<IofrmpiwjsonIOFromPIWJson> darrioinfrmpiwjsonIosFromPIW =
                                                new List<IofrmpiwjsonIOFromPIWJson>();

                                            ProdtypProductType.subGetProcessInputs(intnJobId_I, ps_I.strPrintshopId,
                                                piwentity.intPk, configuration_I, (int)intnPkEleetOrEleeleI_I,
                                                (bool)boolnIsEleetI_I, (int)intnPkResourceI_I,
                                                out darrioinfrmpiwjsonIosFromPIW, ref intStatus_IO,
                                                ref strUserMessage_IO, ref strDevMessage_IO);

                                            if (
                                                //          //Valid that is not the same IO.
                                                !(boolnIsEleetI_I == boolnIsEleetO_I &&
                                                intnPkEleetOrEleeleI_I == intnPkEleetOrEleeleO_I &&
                                                intnPkResourceI_I == intnPkResourceO_I) &&
                                                //          //Valid the status.
                                                intStatus_IO == 200 &&
                                                //          //Valid that eleetO a resourceO exist in any process input.
                                                darrioinfrmpiwjsonIosFromPIW.Exists(ioinfrmpiw =>
                                                ioinfrmpiw.boolIsEleet == boolnIsEleetO_I &&
                                                ioinfrmpiw.intnPkEleetOrEleele == intnPkEleetOrEleeleO_I &&
                                                ioinfrmpiw.intnPkResource == intnPkResourceO_I)
                                                )
                                            {
                                                boolValid = true;
                                            }
                                            else
                                            {
                                                intStatus_IO = 406;
                                                strUserMessage_IO = "Something is wrong.";
                                                strDevMessage_IO = "Can be the same IO or the InputIO is not belong to " +
                                                    "other Input IO for this process.";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (
                            //                              //It is a Job Quantity.
                            intnPkEleetOrEleeleO_I == null &&
                            intnPkResourceO_I == null
                            )
                        {
                            boolValid = true;
                        }
                    }
                }
            }
            else if (
                //                                          //Calculation from wf, for a process (option 2).
                (intnPkProcessInWorkflow_I != null) &&
                (boolnIsEleetI_I == null) &&
                (intnPkEleetOrEleeleI_I == null)
                )
            {
                //                                          //Get the piw.
                piwentity = context.ProcessInWorkflow.FirstOrDefault(piw => piw.intPk == intnPkProcessInWorkflow_I);

                intStatus_IO = 431;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Process in workflow not found.";
                if (
                    piwentity != null
                    )
                {
                    boolValid = true;
                    intnPkProcess = piwentity.intPkProcess;
                }
            }
            else if (
                //                                          //Calculation not from wf (option 3).
                (intnPkProcessInWorkflow_I == null) &&
                (boolnIsEleetI_I == null) &&
                (intnPkEleetOrEleeleI_I == null)
                )
            {
                boolValid = true;
            }
            else
            {
                intStatus_IO = 430;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Calculation by workflow invalid.";
            }
            /*END-CASE*/

            if (
                boolValid
                )
            {
                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Calculation type invalid.";

                bool boolWorkflowIsBase = false;
                if (
                    //                                      //PIW could come null if calculation does not come from
                    //                                      //      workflow (by product, by process , etc).
                    piwentity != null
                    )
                {
                    //                                      //Find workflow.
                    WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                        wf.intPk == piwentity.intPkWorkflow);

                    if (
                        //                                  //Verify if workflow is base.
                        wfentity.intnPkProduct == null
                        )
                    {
                        boolWorkflowIsBase = true;
                    }

                    if (
                        //                                  //Verify if is an estimate workflow 
                        wfentity.intnPkProduct == null && wfentity.intnJobId != null
                        )
                    {
                        //                                  //Override conditions
                        gpcondCondition_I = null;
                    }
                }

                //                                          //To easy code.
                ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(intnPkProduct_I);
                ProProcess pro = ProProcess.proFromDB(intnPkProcess);
                ResResource resI = ResResource.resFromDB(intnPkResourceI_I, false);

                /*CASE*/
                if (
                    //                                      //It is a profit calculation.
                    strCalculationType_I == CalCalculation.strProfit
                    )
                {
                    CalCalculation.subAddAProfitCalculation(prodtyp, pro, resI, strAscendantElements_I,
                        strValue_I, numnProfit_I, strByX_I, strDescription_I, boolIsEnable_I, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO);
                }
                else if (
                    //                                      //It is a base calculation.
                    strCalculationType_I == CalCalculation.strBase
                    )
                {
                    CalCalculation.subAddBaseCalculation(ps_I, prodtyp, intnPkProduct_I, pro, intnPkProcess, resI,
                        strAscendantElements_I, strValue_I, numnCost_I, strByX_I,
                        strDescription_I, boolIsEnable_I, gpcondCondition_I, intnPkProcessInWorkflow_I, 
                        intnPkElementElementTypeI, intnPkElementElementI, intnPkAccount_I, boolWorkflowIsBase, 
                        ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                }
                else if (
                    //                                      //It is a per quantity calculation.
                    strCalculationType_I == CalCalculation.strPerQuantity
                    )
                {
                    CalCalculation.subAddPerQuantityCalculation(ps_I, prodtyp, intnPkProduct_I, pro, intnPkProcess,
                        resI, intnPkResourceI_I, intnPkResourceO_I, strDescription_I, boolIsEnable_I, numnCost_I,
                        numnQuantity_I, numnNeeded_I, strUnit_I, numnPerUnits_I, strAscendantElements_I, strValue_I, 
                        strByX_I, numnMin_I, intnPkProcessInWorkflow_I, intnPkElementElementTypeI, intnPkElementElementI,
                        intnPkElementElementTypeO, intnPkElementElementO, numnBlock_I, gpcondCondition_I, 
                        numnQuantityWaste_I, numnPercentWaste_I, intnPkAccount_I, boolWorkflowIsBase, 
                        boolnFromThickness_I, boolnIsBlock_I, boolnByArea_I, ref intStatus_IO, ref strUserMessage_IO, 
                        ref strDevMessage_IO);
                }
                else if (
                   //                                       //It is a per quantity base calculation.
                   strCalculationType_I == CalCalculation.strPerQuantityBase
                   )
                {
                    CalCalculation.subAddPerQuantityBaseCalculation(ps_I, prodtyp, intnPkProduct_I, numnNeeded_I,
                         intnPkProcess, strByX_I, strDescription_I, boolIsEnable_I, gpcondCondition_I, 
                         intnPkProcessInWorkflow_I, intnPkResourceI_I, intnPkElementElementTypeI, 
                         intnPkElementElementI, intnPkAccount_I, boolWorkflowIsBase, ref intStatus_IO, 
                         ref strUserMessage_IO,  ref strDevMessage_IO);
                }
                /*END-CASE*/
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddAProfitCalculation(
            ProdtypProductType prodtyp_I,
            ProProcess pro_I,
            ResResource res_I,
            String strAscendantElements_I,
            String strValue_I,
            double? numnProfit_I,
            String strByX_I,
            String strDescription_I,
            bool boolIsEnable_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "For the profit calculation you need: A product and strBy = ByProduct. The " +
                "process, resource type, resource, ascendants and value must be null.";
            if (
                //                                          //Must have a product. Must not have a process, a resource 
                //                                          //      type, a resource, ascendants, value for the 
                //                                          //      attribute for the orden form.
                (prodtyp_I != null) && (pro_I == null) && (res_I == null) &&
                (strAscendantElements_I == null) && (strValue_I == null) &&
                //                                          //From byProduct screen.
                (strByX_I == CalCalculation.strByProduct)
                )
            {
                intStatus_IO = 403;
                strUserMessage_IO = "The profit should be greater than zero.";
                strDevMessage_IO = "";
                if (
                    numnProfit_I > 0
                    )
                {
                    CalentityCalculationEntityDB calentity = new CalentityCalculationEntityDB
                    {
                        intnPkProduct = prodtyp_I.intPk,
                        strCalculationType = CalCalculation.strProfit,
                        strDescription = strDescription_I,
                        boolIsEnable = boolIsEnable_I,
                        numnProfit = numnProfit_I,
                        strByX = strByX_I,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString()
                    };
                    context.Calculation.Add(calentity);
                    context.SaveChanges();

                    CalCalculation.subDisableOtherProfitCalculations(calentity);
                    intStatus_IO = 200;
                    strUserMessage_IO = "Success";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddBaseCalculation(
            PsPrintShop ps_I,
            ProdtypProductType prodtyp_I,
            int? intnPkProduct_I,
            ProProcess pro_I,
            int? intnPkProcess_I,
            ResResource res_I,
            String strAscendantElements_I,
            String strValue_I,
            double? numnCost_I,
            String strByX_I,
            String strDescription_I,
            bool boolIsEnable_I,
            GpcondjsonGroupConditionJson gpcondCondition_I,
            int? intnPkProcessInWorkflow_I,
            int? intnPkElementElementType_I,
            int? intnPkElementElement_I,
            int? intnPkAccount_I,
            bool boolWorkflowIsBase_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            int intPkAccount = intnPkAccount_I != null ? (int)intnPkAccount_I :
                ps_I.intGetExpensePkAccount(context);
            bool boolAccountValid = AccAccounting.boolIsExpenseValid(intPkAccount, context);

            bool boolIsValid = false;
            intStatus_IO = 404;
            strUserMessage_IO = "";
            strDevMessage_IO = "The strBy and the product do not match for a valid calculation.";
            /*CASE*/
            if (
                //                                          //ByProduct.
                strByX_I == CalCalculation.strByProduct
                )
            {
                intStatus_IO = 405;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProduct base calculation you need: A product and strBy = ByProduct. " +
                    "Process, resource type, resource, ascendants and value must be null.";
                if (
                    (prodtyp_I != null) && (pro_I == null) && (res_I == null) &&
                    (strAscendantElements_I == null) && (strValue_I == null)
                    )
                {
                    intStatus_IO = 422;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Account not valid.";
                    if (
                        boolAccountValid
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }
            else if (
                //                                          //ByProcess.
                (strByX_I == CalCalculation.strByProcess) &&
                (prodtyp_I != null || boolWorkflowIsBase_I)
                )
            {
                intStatus_IO = 407;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProcess base calculation you need: A product, a process and strBy = " +
                    "ByProcess. Resource type, resource, ascendants and value must be null.";
                if (
                    (pro_I != null) && (res_I == null) &&
                    (strAscendantElements_I == null) && (strValue_I == null)
                    )
                {
                    intStatus_IO = 423;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Account not valid.";
                    if (
                        boolAccountValid
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }
            else if (
                //                                          //ByProcess default.
                (strByX_I == CalCalculation.strByProcess) &&
                (prodtyp_I == null)
                )
            {
                intStatus_IO = 421;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProcess default per quantity calculation you need: A process and strBy = " +
                    "ByProcess. Product, resource type, resource, ascendants and value must be null.";
                if (
                    (res_I == null) && (strAscendantElements_I == null) && (strValue_I == null)
                    )
                {
                    intStatus_IO = 423;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Account not valid.";
                    if (
                        boolAccountValid
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }

            /*END-CASE*/

            if (
                boolIsValid
                )
            {
                intStatus_IO = 408;
                strUserMessage_IO = "The cost should be greater than zero.";
                strDevMessage_IO = "";
                if (
                    //                                      //The cost is greater than zero.
                    numnCost_I > 0
                    )
                {
                    intStatus_IO = 409;
                    strUserMessage_IO = "The condition to apply is invalid. Please verify it.";
                    strDevMessage_IO = "";
                    if (
                       Tools.boolValidConditionList(gpcondCondition_I)
                       )
                    {
                        PiwentityProcessInWorkflowEntityDB piwentity =
                                context.ProcessInWorkflow.FirstOrDefault(piw =>
                                piw.intPk == intnPkProcessInWorkflow_I);

                        int? intnPkWorkflow = null;
                        int? intnProcessInWorkflowId = null;
                        if (
                            piwentity != null
                            )
                        {
                            intnPkWorkflow = piwentity.intPkWorkflow;
                            intnProcessInWorkflowId = piwentity.intProcessInWorkflowId;
                        }

                        //                      //Add to db.
                        CalentityCalculationEntityDB calentity = new CalentityCalculationEntityDB
                        {
                            intnPkProduct = intnPkProduct_I,
                            intnPkProcess = intnPkProcess_I,
                            strCalculationType = CalCalculation.strBase,
                            strDescription = strDescription_I,
                            boolIsEnable = boolIsEnable_I,
                            numnCost = numnCost_I,
                            strByX = strByX_I,
                            intnPkWorkflow = intnPkWorkflow,
                            intnProcessInWorkflowId = intnProcessInWorkflowId,
                            intnPkElementElementType = intnPkElementElementType_I,
                            intnPkElementElement = intnPkElementElement_I,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            intnPkAccount = intPkAccount
                        };

                        context.Calculation.Add(calentity);
                        context.SaveChanges();

                        Tools.subAddCondition(calentity.intPk, null, null, null, gpcondCondition_I, context);

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddPerQuantityCalculation(
            PsPrintShop ps_I,
            ProdtypProductType prodtyp_I,
            int? intnPkProduct_I,
            ProProcess pro_I,
            int? intnPkProcess_I,
            ResResource resI_I,
            int? intnPkResourceI_I,
            int? intnPkResourceO_I,
            String strDescription_I,
            bool boolIsEnable_I,
            double? numnCost_I,
            double? numnQuantity_I,
            double? numnNeeded_I,
            String strUnit_I,
            double? numnPerUnits_I,
            String strAscendantElements_I,
            String strValue_I,
            String strByX_I,
            double? numnMin_I,
            int? intnPkProcessInWorkflow_I,
            int? intnPkElementElementTypeI_I,
            int? intnPkElementElementI_I,
            int? intnPkElementElementTypeO_I,
            int? intnPkElementElementO_I,
            double? numnBlock_I,
            GpcondjsonGroupConditionJson gpcondCondition_I,
            double? numnQuantityWaste_I,
            double? numnPercentWaste_I,
            int? intnPkAccount_I,
            bool boolWorkflowIsBase_I,
            bool? boolnFromThickness_I,
            bool? boolnIsBlock_I,
            bool? boolnByArea_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            int intPkAccount = intnPkAccount_I != null ? (int)intnPkAccount_I :
                ps_I.intGetExpensePkAccount(context);
            bool boolAccountValid = AccAccounting.boolIsExpenseValid(intPkAccount, context);

            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == intnPkProcessInWorkflow_I);

            bool? boolnByAreaToSave = null;
            bool boolIsValid = false;
            double? numnQuantityWaste = null;
            double? numnPercentWaste = null;
            intStatus_IO = 417;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The strBy, product, process, resource type and resource do not match for a valid " +
                "calculation.";

            /*CASE*/
            if (
                //                                          //ByProduct.
                strByX_I == CalCalculation.strByProduct
                )
            {
                intStatus_IO = 418;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProduct per quantity calculation you need: A product and strBy = " +
                    "ByProduct. Process, resource type, resource, ascendants and value must be null.";
                if (
                    (prodtyp_I != null) && (pro_I == null) && (resI_I == null) &&
                    (strAscendantElements_I == null) && (strValue_I == null)
                   )
                {
                    intStatus_IO = 416;
                    strUserMessage_IO = "The minimum amount to produce should be an integer.";
                    strDevMessage_IO = "";
                    if (
                        ((int)numnMin_I / numnMin_I) == 1
                        )
                    {
                        intStatus_IO = 433;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Account not valid.";
                        if (
                            boolAccountValid
                            )
                        {
                            boolIsValid = true;
                        }
                    }
                }
            }
            else if (
                //                                          //ByIntent.
                strByX_I == CalCalculation.strByIntent
                )
            {
                intStatus_IO = 419;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByIntent per quantity calculation you need: A product, strBy = ByIntent, " +
                    "ascendants and value. Process, resource type and resource must be null.";
                if (
                    (prodtyp_I != null) && (pro_I == null) && (resI_I == null) &&
                    (strAscendantElements_I != null) && (strValue_I != null)
                    )
                {
                    intStatus_IO = 416;
                    strUserMessage_IO = "The minimum amount to produce should be an integer.";
                    strDevMessage_IO = "";
                    if (
                        ((int)numnMin_I / numnMin_I) == 1
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }
            else if (
                //                                          //ByProcess.
                (strByX_I == CalCalculation.strByProcess) &&
                (prodtyp_I != null || boolWorkflowIsBase_I)
                )
            {
                intStatus_IO = 420;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProcess per quantity calculation you need: A product, process and " +
                    "strBy = ByProcess. Resource type, resource, ascendants and value must be null.";
                if (
                    (pro_I != null) && (resI_I == null) &&
                    (strAscendantElements_I == null) && (strValue_I == null)
                    )
                {
                    intStatus_IO = 426;
                    strUserMessage_IO = "The number of units of measurement needed should be greater than zero.";
                    strDevMessage_IO = "";
                    if (
                        numnNeeded_I > 0
                        )
                    {
                        intStatus_IO = 427;
                        strUserMessage_IO = "The number of units produced by the number of units of measurement needed" +
                            " should be greater than zero.";
                        strDevMessage_IO = "";
                        if (
                            numnPerUnits_I > 0
                            )
                        {
                            //                              //Validate Unit not only have numbers.
                            bool boolIsParseableToInt = strUnit_I.IsParsableToInt();
                            bool boolIsParseableToNum = strUnit_I.IsParsableToNum();

                            intStatus_IO = 429;
                            strUserMessage_IO = "Unit of Measurement cannot start with a number.";
                            if (
                                !boolIsParseableToInt &&
                                !boolIsParseableToNum
                                )
                            {
                                intStatus_IO = 434;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Account not valid.";
                                if (
                                    boolAccountValid
                                    )
                                {
                                    boolIsValid = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (
                //                                          //ByProcess default.
                (strByX_I == CalCalculation.strByProcess) && (prodtyp_I == null)
                )
            {
                intStatus_IO = 421;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByProcess default per quantity calculation you need: A process and strBy = " +
                    "ByProcess. Product, resource type, resource, ascendants and value must be null.";
                if (
                    (resI_I == null) && (strAscendantElements_I == null) && (strValue_I == null)
                    )
                {
                    //                                      //Validate Unit not only have numbers.
                    bool boolIsParseableToInt = strUnit_I.IsParsableToInt();
                    bool boolIsParseableToNum = strUnit_I.IsParsableToNum();

                    intStatus_IO = 432;
                    strUserMessage_IO = "Unit of Measurement cannot start with a number.";
                    if (
                        !boolIsParseableToInt &&
                        !boolIsParseableToNum
                        )
                    {
                        intStatus_IO = 434;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Account not valid.";
                        if (
                            boolAccountValid
                            )
                        {
                            boolIsValid = true;
                        }
                    }
                }
            }
            else if (
                //                                          //ByResource.
                (strByX_I == CalCalculation.strByResource) &&
                (prodtyp_I != null || boolWorkflowIsBase_I)
                )
            {
                intStatus_IO = 424;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "For a ByResource per quantity calculation you need: A product, physical resourceI" +
                    " physical resourceO or null resourceO and strBy = ByResource. Process, ascendants and value must " +
                    "be null.";

                if (
                    (pro_I == null) && (resI_I != null) &&
                    (RestypResourceType.boolIsPhysical(resI_I.restypBelongsTo.strClassification)) &&
                    (strAscendantElements_I == null) && (strValue_I == null)
                    )
                {
                    //                                      //Get resourceI data.
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == intnPkResourceI_I);

                    //                                      //To easy code.
                    EtentityElementTypeEntityDB etentityRestypI = context.ElementType.FirstOrDefault(et =>
                        et.intPk == eleentity.intPkElementType);

                    bool boolResIIsPaper = (etentityRestypI.strCustomTypeId == ResResource.strComponent) ||
                        (etentityRestypI.strCustomTypeId == ResResource.strMedia);

                    //                                      //Find Qfrom IO.
                    IoentityInputsAndOutputsEntityDB ioentityQfromRes = context.InputsAndOutputs.FirstOrDefault(
                        io => io.intPkWorkflow == piwentity.intPkWorkflow &&
                        io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                        io.intnPkElementElementType == intnPkElementElementTypeO_I &&
                        io.intnPkElementElement == intnPkElementElementO_I);

                    bool boolResOIsSizeBearer = false;
                    if (
                        ioentityQfromRes != null
                        )
                    {
                        if (
                            ioentityQfromRes.boolnSize == true
                            )
                        {
                            boolResOIsSizeBearer = true;
                        }
                    }

                    //                                  //Get resource entity.
                    EleentityElementEntityDB eleentityQFrom = context.Element.FirstOrDefault(ele =>
                        ele.intPk == intnPkResourceO_I);

                    //                                  //Find resource type.
                    RestypResourceType restypeResourceTypeO =
                        (RestypResourceType)EtElementTypeAbstract.etFromDB(eleentityQFrom.intPkElementType);

                    if (
                            restypeResourceTypeO.strCustomTypeId == ResResource.strMedia ||
                            restypeResourceTypeO.strCustomTypeId == ResResource.strComponent                            
                        )
                    {
                        boolnByAreaToSave = boolnByArea_I == null ? false : boolnByArea_I;
                    }

                    bool boolPerUnitsCanBeNull = false;
                    bool boolNeededCanBeNull = false;
                    if (
                        //                                  //ResI is paper and ResO (QFrom) is size bearer
                        boolResIIsPaper && boolResOIsSizeBearer
                        )
                    {
                        numnPerUnits_I = null;
                        boolPerUnitsCanBeNull = true;
                        boolNeededCanBeNull = boolResOIsSizeBearer;
                    }

                    if (
                        boolnFromThickness_I == true
                        )
                    {
                        numnNeeded_I = (boolnFromThickness_I == true || resI_I.boolMediaRoll()) ? numnNeeded_I : 1;
                        numnPerUnits_I = null;
                        boolPerUnitsCanBeNull = true;
                    }

                    intStatus_IO = 426;
                    strUserMessage_IO = "The number of units of measurement needed should be greater than zero.";
                    strDevMessage_IO = "";
                    if (
                        numnNeeded_I > 0 ||
                        boolNeededCanBeNull
                        )
                    {
                        intStatus_IO = 427;
                        strUserMessage_IO = "The number of units produced by the number of units of measurement " +
                            "needed should be greater than zero.";
                        strDevMessage_IO = "";
                        if (
                            boolPerUnitsCanBeNull ||
                            numnPerUnits_I > 0
                            )
                        {
                            bool boolContinueCheck = true;

                            if (
                                //                          //Need and perUnit Can be null.
                                boolNeededCanBeNull &&
                                boolPerUnitsCanBeNull
                                ) 
                            {
                                //                          //Get the paper transformation.
                                PatransPaperTransformationEntityDB patrans = context.PaperTransformation.FirstOrDefault(
                                    patrans => patrans.intPkProcessInWorkflow == piwentity.intPk &&
                                    patrans.intnPkElementElementTypeI == intnPkElementElementTypeI_I &&
                                    patrans.intnPkElementElementI == intnPkElementElementI_I &&
                                    patrans.intPkResourceI == intnPkResourceI_I);

                                if (
                                    //                      //Paper transformation exist.
                                    patrans != null
                                    )
                                {
                                    //                      //The paper transformation should exist
                                    //                      //    can continue.
                                    boolContinueCheck = true;
                                }
                                else
                                {
                                    boolContinueCheck = false;
                                }
                            }

                            intStatus_IO = 428;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Not found the paper transformation or it is necesariy data less the" +
                                "needed.";
                            if (
                                boolContinueCheck
                                )
                            {
                                intStatus_IO = 430;
                                strUserMessage_IO = "Waste cannot be less than 0";
                                strDevMessage_IO = "";
                                if (
                                    ((numnQuantityWaste_I == null) ||
                                    ((numnQuantityWaste_I != null) &&
                                    (numnQuantityWaste_I >= 0)))
                                    &&
                                    ((numnPercentWaste_I == null) ||
                                    ((numnPercentWaste_I != null) &&
                                    (numnPercentWaste_I >= 0)))
                                    )
                                {
                                    //                      //Get resource output data.
                                    EleentityElementEntityDB eleentityOutput = context.Element.FirstOrDefault(ele =>
                                        ele.intPk == intnPkResourceO_I);

                                    //                      //To easy code.
                                    EtentityElementTypeEntityDB etentityRestypOutput = context.ElementType.FirstOrDefault(et =>
                                        et.intPk == eleentityOutput.intPkElementType);


                                    bool boolResOIsMedia = etentityRestypOutput.strCustomTypeId == ResResource.strMedia;
                                    bool boolResOIsComponent = etentityRestypOutput.strCustomTypeId == ResResource.strComponent;

                                    //                      //Get resResource.
                                    ResResource resResource = ResResource.resFromDB(intnPkResourceO_I, false);
                                    bool boolIsRoll = resResource.boolMediaRoll();

                                    intStatus_IO = 431;
                                    strUserMessage_IO = "From Thickness only can be for Qfrom Media-not roll or Component.";
                                    strDevMessage_IO = "Thickness only can be ResQfrom media or component.";
                                    if (
                                        boolnFromThickness_I == null || boolnFromThickness_I == false
                                        ||
                                        //                  //ResOutput has a component or media
                                        //                  //    for receive fromThickness(true).
                                        boolnFromThickness_I == true && boolResOIsComponent
                                        ||
                                        boolnFromThickness_I == true && boolResOIsMedia && !boolIsRoll
                                        )
                                    {
                                        bool boolContinue = true;
                                        if (
                                            //              //Cal perUnits by Thickness.        
                                            boolnFromThickness_I == true
                                            )
                                        {
                                            //              //Verify if the resources has thickness and lift.
                                            boolContinue = CalCalculation.boolResHasThicknessAndLiftOrHeight(
                                                eleentity, eleentityOutput, ref intStatus_IO, ref strUserMessage_IO,
                                                ref strDevMessage_IO, context);
                                        }

                                        //                          //If calculation still valid and is media or component, we
                                        //                          //      have to verify if the value for boolByArea is
                                        //                          //      valid, checking if the resource has AreaUnit.
                                        if (
                                            boolContinue &&
                                            (boolResOIsMedia || boolResOIsComponent)
                                            )
                                        {   
                                            boolContinue = 
                                                (boolnByArea_I == null || !(bool)boolnByArea_I) ? true :
                                                CalCalculation.boolnByAreaIsValid(eleentityOutput.intPk,
                                                boolResOIsComponent, boolnByArea_I);
                                        }

                                        if (
                                            boolContinue == true
                                            )
                                        {
                                            boolIsValid = true;

                                            /*CASE*/
                                            if (
                                                (numnQuantityWaste_I != null) &&
                                                (numnPercentWaste_I == null)
                                                )
                                            {
                                                numnQuantityWaste = numnQuantityWaste_I;
                                                numnPercentWaste = 0;
                                            }
                                            else if (
                                                (numnQuantityWaste_I == null) &&
                                                (numnPercentWaste_I == null)
                                                )
                                            {
                                                numnQuantityWaste = 0;
                                                numnPercentWaste = 0;
                                            }
                                            else if (
                                               (numnQuantityWaste_I == null) &&
                                               (numnPercentWaste_I != null)
                                               )
                                            {
                                                numnQuantityWaste = 0;
                                                numnPercentWaste = numnPercentWaste_I;
                                            }
                                            else if (
                                                (numnQuantityWaste_I != null) &&
                                                (numnPercentWaste_I != null)
                                                )
                                            {
                                                numnQuantityWaste = numnQuantityWaste_I;
                                                numnPercentWaste = numnPercentWaste_I;
                                            }
                                            /*END-CASE*/
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            /*END-CASE*/

            if (
                boolIsValid
                )
            {
                intStatus_IO = 408;
                strUserMessage_IO = "The cost should be greater than zero.";
                strDevMessage_IO = "";
                if (
                    ((strByX_I == CalCalculation.strByResource) && (prodtyp_I != null || boolWorkflowIsBase_I)) ||
                    (numnCost_I > 0)
                    )
                {
                    intStatus_IO = 428;
                    strUserMessage_IO = "The quantity should be greater than zero.";
                    strDevMessage_IO = "";
                    if (
                        ((strByX_I == CalCalculation.strByResource) && (prodtyp_I != null || boolWorkflowIsBase_I)) ||
                        (numnQuantity_I > 0)
                        )
                    {
                        intStatus_IO = 415;
                        strUserMessage_IO = "The minimum amount should be greater than zero.";
                        strDevMessage_IO = "";
                        if (
                            (numnMin_I == null) || (numnMin_I > 0)
                            )
                        {
                            intStatus_IO = 409;
                            strUserMessage_IO = "The condition to apply is invalid. Please verify it.";
                            strDevMessage_IO = "";
                            if (
                               Tools.boolValidConditionList(gpcondCondition_I)
                               )
                            {
                                int? intnPkWorkflow = null;
                                int? intnProcessInWorkflowId = null;
                                if (
                                    piwentity != null
                                    )
                                {
                                    intnPkWorkflow = piwentity.intPkWorkflow;
                                    intnProcessInWorkflowId = piwentity.intProcessInWorkflowId;
                                }

                                bool boolCycleIsCreated = false;
                                if (
                                    strByX_I == CalCalculation.strByResource
                                    )
                                {
                                    List<CalentityCalculationEntityDB> darrcalentity =
                                        context.Calculation.Where(cal =>
                                        cal.intnPkProduct == intnPkProduct_I &&
                                        cal.intnPkWorkflow == intnPkWorkflow &&
                                        cal.intnProcessInWorkflowId == intnProcessInWorkflowId &&
                                        cal.intnPkResource == intnPkResourceO_I).ToList();

                                    boolCycleIsCreated = CalCalculation.boolCycleIsCreatedRecursive(
                                        darrcalentity, (int)intnPkResourceI_I);
                                }

                                intStatus_IO = 431;
                                strUserMessage_IO = "Cycle is created with this calculation.";
                                strDevMessage_IO = "";
                                if (
                                    !boolCycleIsCreated
                                    )
                                {
                                    if (
                                        numnBlock_I != null && numnBlock_I > 0
                                        )
                                    {
                                        //                  //Calculate Min to use according ByBlock´s value.
                                        numnMin_I = ResResource.numnGetMinToUse(numnMin_I, numnBlock_I);
                                        strDevMessage_IO = "numnMin was calculated using the value of " +
                                            "numnBlock";
                                    }

                                    CalentityCalculationEntityDB calentity =
                                        new CalentityCalculationEntityDB
                                        {
                                            intnPkProduct = intnPkProduct_I,
                                            intnPkProcess = intnPkProcess_I,
                                            intnPkResource = intnPkResourceI_I,
                                            intnPkQFromResource = intnPkResourceO_I,
                                            strCalculationType = CalCalculation.strPerQuantity,
                                            strDescription = strDescription_I,
                                            boolIsEnable = boolIsEnable_I,
                                            numnCost = numnCost_I,
                                            numnQuantity = numnQuantity_I,
                                            strUnit = strUnit_I,
                                            numnMin = numnMin_I,
                                            numnBlock = numnBlock_I,
                                            strAscendants = strAscendantElements_I,
                                            strValue = strValue_I,
                                            numnNeeded = numnNeeded_I,
                                            numnPerUnits = numnPerUnits_I,
                                            strByX = strByX_I,
                                            intnPkWorkflow = intnPkWorkflow,
                                            intnProcessInWorkflowId = intnProcessInWorkflowId,
                                            intnPkElementElementType = intnPkElementElementTypeI_I,
                                            intnPkElementElement = intnPkElementElementI_I,
                                            intnPkQFromElementElementType = intnPkElementElementTypeO_I,
                                            intnPkQFromElementElement = intnPkElementElementO_I,
                                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                            numnQuantityWaste = numnQuantityWaste,
                                            numnPercentWaste = numnPercentWaste,
                                            intnPkAccount = intPkAccount,
                                            boolnFromThickness = (boolnFromThickness_I == true ? boolnFromThickness_I :
                                            null),
                                            boolnIsBlock = boolnIsBlock_I == null ? false : boolnIsBlock_I,
                                            boolnByArea = boolnFromThickness_I == true ? null : boolnByAreaToSave
                                        };
                                    context.Calculation.Add(calentity);
                                    context.SaveChanges();

                                    Tools.subAddCondition(calentity.intPk, null, null, null, gpcondCondition_I,
                                        context);

                                    //                      //Assign the new PkCal to its temporary paper 
                                    //                      //      transformation.
                                    CalCalculation.subAddPkCalculationToPaperTransformation(calentity,
                                        intnPkProcessInWorkflow_I);

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolResHasThicknessAndLiftOrHeight(
            //                                              //Verify that the resource Input has thickness and
            //                                              //      ResourceOutput has lift or heigth.
            //                                              //      Device - must have lift.
            //                                              //      MiscConsumable - must have height.

            //                                              //It is the resource.
            EleentityElementEntityDB eleentityInput_I,
            //                                              //It is the resource of the QFrom.
            EleentityElementEntityDB eleentityOutput_I,
            //                                              //strMessage.
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            Odyssey2Context context_M
            )
        {
            bool boolHasThickness = false;
            bool boolHasLiftOrHeight = false;

            //                                              //Attributes for Resource.

            List<AttrentityAttributeEntityDB> darrattrResource =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == eleentityInput_I.intPkElementType
                 select attrentity).ToList();

            EtentityElementTypeEntityDB etentityInput = context_M.ElementType.FirstOrDefault(et =>
                et.intPk == eleentityInput_I.intPkElementType);

            /*CASE*/
            if (
                etentityInput.strCustomTypeId == ResResource.strDevice
                )
            {
                //                                          //Get the Lift and LiftUnit attributes.
                AttrentityAttributeEntityDB attrentityLift = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "Lift" || a.strCustomName == "XJDFLift");

                AttrentityAttributeEntityDB attrentityLiftUnit = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "LiftUnit" || a.strCustomName == "XJDFLiftUnit");

                if (
                    attrentityLift != null && attrentityLiftUnit != null
                    )
                {
                    //                                      //Get the Lift value.
                    ValentityValueEntityDB valentityLift = context_M.Value.FirstOrDefault(val =>
                        val.intPkElement == eleentityInput_I.intPk &&
                        val.intPkAttribute == attrentityLift.intPk);

                    //                                      //Get the LiftUnit value.
                    ValentityValueEntityDB valentityLiftUnit = context_M.Value.FirstOrDefault(val =>
                        val.intPkElement == eleentityInput_I.intPk &&
                        val.intPkAttribute == attrentityLiftUnit.intPk);

                    //                                      //Validate the lift.
                    boolHasLiftOrHeight = valentityLift != null && valentityLiftUnit != null ? true : false;
                }
            }
            else if (
                etentityInput.strCustomTypeId == ResResource.strMiscConsumable
                )
            {
                //                                          //Get the Height and HeightUnit attributes.
                AttrentityAttributeEntityDB attrentityHeight = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "Height" || a.strCustomName == "XJDFHeight");

                AttrentityAttributeEntityDB attrentityHeightUnit = darrattrResource.FirstOrDefault(a =>
                    a.strXJDFName == "HeightUnit" || a.strCustomName == "XJDFHeightUnit");

                if (
                    attrentityHeight != null && attrentityHeightUnit != null
                    )
                {
                    //                                      //Get the Height value.
                    ValentityValueEntityDB valentityHeight = context_M.Value.FirstOrDefault(val =>
                        val.intPkElement == eleentityInput_I.intPk &&
                        val.intPkAttribute == attrentityHeight.intPk);

                    //                                      //Get the HeightUnit value.
                    ValentityValueEntityDB valentityHeightUnit = context_M.Value.FirstOrDefault(val =>
                        val.intPkElement == eleentityInput_I.intPk &&
                        val.intPkAttribute == attrentityHeightUnit.intPk);

                    //                                      //Validate the Height.
                    boolHasLiftOrHeight = valentityHeight != null && valentityHeightUnit != null ? true : false;
                }
            }
            else
            { 
                //                                          //Do nothing.
            }

            //                                              //Attributes for Resource Qfrom.
            List<AttrentityAttributeEntityDB> darrattrResourceQFrom =
                (from attrentity in context_M.Attribute
                 join attretentity in context_M.AttributeElementType
                 on attrentity.intPk equals attretentity.intPkAttribute
                 where attretentity.intPkElementType == eleentityOutput_I.intPkElementType
                 select attrentity).ToList();

            EtentityElementTypeEntityDB etentityOutput = context_M.ElementType.FirstOrDefault(et =>
                et.intPk == eleentityOutput_I.intPkElementType);

            /*CASE*/
            if (
                etentityOutput.strCustomTypeId == ResResource.strMedia
                )
            {
                //                                          //Get the Thickness and ThicknessUnit attributes.
                AttrentityAttributeEntityDB attrentityThickness = darrattrResourceQFrom.FirstOrDefault(a =>
                    a.strXJDFName == "Thickness" || a.strCustomName == "XJDFThickness");

                AttrentityAttributeEntityDB attrentityThicknessUnit = darrattrResourceQFrom.FirstOrDefault(a =>
                    a.strXJDFName == "ThicknessUnit" || a.strCustomName == "XJDFThicknessUnit");

                if (
                    attrentityThickness != null && attrentityThicknessUnit != null
                    )
                {
                    //                                      //Get the Thickness value.
                    ValentityValueEntityDB valentityThickness = context_M.Value.FirstOrDefault(val =>
                        val.intPkElement == eleentityOutput_I.intPk &&
                        val.intPkAttribute == attrentityThickness.intPk);

                    //                                      //Get the ThicknessUnit value.
                    ValentityValueEntityDB valentityThicknessUnit = context_M.Value.FirstOrDefault(val =>
                        val.intPkElement == eleentityOutput_I.intPk &&
                        val.intPkAttribute == attrentityThicknessUnit.intPk);

                    //                                      //Validate the lift.
                    boolHasThickness = valentityThickness != null && valentityThicknessUnit != null ? true : false;
                }
            }
            else if (
                etentityOutput.strCustomTypeId == ResResource.strComponent
                )
            {
                //                                          //The thickness will be propagated.
                boolHasThickness = true;
            }
            else
            {
                //Do nothing.
            }

            strDevMessage_IO = "";
            intStatus_IO = 409;
            /*CASE*/
            if (
                boolHasLiftOrHeight == false &&
                boolHasThickness == true
                )
            {
                strUserMessage_IO = "Lift or Height for resource must be set.";
            }
            else if (
                boolHasLiftOrHeight == true &&
                boolHasThickness == false
                )
            {
                strUserMessage_IO = "Thickness for resource must be set.";
            }
            else if (
                boolHasLiftOrHeight ==  false &&
                boolHasThickness == false
                )
            {
                strUserMessage_IO = "Thickness and Lift or Height for resource must be set.";
            }
            /*END-CASE*/

            return boolHasThickness && boolHasLiftOrHeight;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddPerQuantityBaseCalculation(
            PsPrintShop ps_I,
            ProdtypProductType prodtyp_I,
            int? intnPkProduct_I,
            double? numnNeeded_I,
            int? intnPkProcess_I,
            String strByX_I,
            String strDescription_I,
            bool boolIsEnable_I,
            GpcondjsonGroupConditionJson gpcondCondition_I,
            int? intnPkProcessInWorkflow_I,
            int? intnPkResourceI_I,
            int? intnPkElementElementType_I,
            int? intnPkElementElement_I,
            int? intnPkAccount_I,
            bool boolWorkflowIsBase_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            int intPkAccount = intnPkAccount_I != null ? (int)intnPkAccount_I :
                ps_I.intGetExpensePkAccount(context);
            bool boolAccountValid = AccAccounting.boolIsExpenseValid(intPkAccount, context);

            bool boolIsValid = false;
            intStatus_IO = 404;
            strUserMessage_IO = "";
            strDevMessage_IO = "The strBy and the product do not match for a valid calculation.";
            if (
                //                                          //ByResource.
                (strByX_I == CalCalculation.strByResource) &&
                (prodtyp_I != null || boolWorkflowIsBase_I)
                )
            {
                intStatus_IO = 405;
                strUserMessage_IO = "";
                strDevMessage_IO = "Account not valid.";
                if (
                    //                                      //Valid account.
                    boolAccountValid
                    )
                {
                    boolIsValid = true;
                }
            }

            if (
                boolIsValid
                )
            {
                intStatus_IO = 408;
                strUserMessage_IO = "Quantity should not be less than 0.";
                strDevMessage_IO = "";
                if (
                    //                                      //The cost is greater than zero.
                    numnNeeded_I >= 0
                    )
                {
                    intStatus_IO = 409;
                    strUserMessage_IO = "The condition to apply is invalid. Please verify it.";
                    strDevMessage_IO = "";
                    if (
                       Tools.boolValidConditionList(gpcondCondition_I)
                       )
                    {
                        PiwentityProcessInWorkflowEntityDB piwentity =
                                context.ProcessInWorkflow.FirstOrDefault(piw =>
                                piw.intPk == intnPkProcessInWorkflow_I);

                        int? intnPkWorkflow = null;
                        int? intnProcessInWorkflowId = null;
                        if (
                            piwentity != null
                            )
                        {
                            intnPkWorkflow = piwentity.intPkWorkflow;
                            intnProcessInWorkflowId = piwentity.intProcessInWorkflowId;
                        }

                        //                                  //Add to db.
                        CalentityCalculationEntityDB calentity = new CalentityCalculationEntityDB
                        {
                            intnPkProduct = intnPkProduct_I,
                            intnPkProcess = intnPkProcess_I,
                            strCalculationType = CalCalculation.strPerQuantityBase,
                            strDescription = strDescription_I,
                            boolIsEnable = boolIsEnable_I,
                            numnNeeded = numnNeeded_I,
                            strByX = strByX_I,
                            intnPkWorkflow = intnPkWorkflow,
                            intnProcessInWorkflowId = intnProcessInWorkflowId,
                            intnPkResource = intnPkResourceI_I,
                            intnPkElementElementType = intnPkElementElementType_I,
                            intnPkElementElement = intnPkElementElement_I,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            intnPkAccount = intPkAccount
                        };

                        context.Calculation.Add(calentity);
                        context.SaveChanges();

                        Tools.subAddCondition(calentity.intPk, null, null, null, gpcondCondition_I, context);

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subAddPkCalculationToPaperTransformation(
            //                                              //Find temporary paper transformation created on a specific
            //                                              //      IO and assign it the PkCalculation that was added.

            CalentityCalculationEntityDB calentityCalculation_I,
            int? intnPkProcessInWorkflow_I
            )
        {
            if (
                //                                          //Only per quantity by resource calculations can have 
                //                                          //      paper transformations.
                calentityCalculation_I.strCalculationType == CalCalculation.strPerQuantity &&
                calentityCalculation_I.strByX == CalCalculation.strByResource &&
                intnPkProcessInWorkflow_I != null
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find Piw.
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intnPkProcessInWorkflow_I);

                if (
                    piwentity != null
                    )
                {
                    //                                      //Find temporary paper transformation based on eleet, 
                    //                                      //      eleele and PkPiw. Only inputs can have 
                    //                                      //      paper transformations.
                    PatransPaperTransformationEntityDB patransentity = context.PaperTransformation.FirstOrDefault(
                        paper => paper.intPkProcessInWorkflow == intnPkProcessInWorkflow_I && paper.boolTemporary &&
                        paper.intnPkElementElementTypeI == calentityCalculation_I.intnPkElementElementType &&
                        paper.intnPkElementElementI == calentityCalculation_I.intnPkElementElement &&
                        paper.intPkResourceI == calentityCalculation_I.intnPkResource);

                    if (
                        patransentity != null
                        )
                    {
                        patransentity.intnPkCalculationOwn = calentityCalculation_I.intPk;
                        patransentity.boolTemporary = false;

                        //                                  //Find otherside of the link.
                        List<IoentityInputsAndOutputsEntityDB> darrioentity =
                            CalCalculation.darrioentityVerifyIfIOHasLink(piwentity,
                            patransentity.intnPkElementElementTypeO, patransentity.intnPkElementElementO);

                        foreach (IoentityInputsAndOutputsEntityDB ioentityPropagated in darrioentity)
                        {
                            //                              //Find piw.
                            PiwentityProcessInWorkflowEntityDB piwentityFromPropagated =
                                context.ProcessInWorkflow.FirstOrDefault(piw =>
                                piw.intPkWorkflow == ioentityPropagated.intPkWorkflow &&
                                piw.intProcessInWorkflowId == ioentityPropagated.intnProcessInWorkflowId);

                            if (
                                piwentityFromPropagated != null
                                )
                            {
                                //                          //Find paper trans.
                                PatransPaperTransformationEntityDB patransentityPropagated =
                                    context.PaperTransformation.FirstOrDefault(paper =>
                                    paper.intPkProcessInWorkflow == piwentityFromPropagated.intPk &&
                                    paper.intnPkElementElementTypeI ==
                                    ioentityPropagated.intnPkElementElementType &&
                                    paper.intnPkElementElementI == ioentityPropagated.intnPkElementElement &&
                                    paper.intPkResourceI == patransentity.intnPkResourceO &&
                                    paper.intnPkCalculationLink == null &&
                                    paper.intnPkCalculationOwn == null);

                                if (
                                    patransentityPropagated != null
                                    )
                                {
                                    patransentityPropagated.intnPkCalculationLink = calentityCalculation_I.intPk;
                                }
                            }
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolCycleIsCreatedRecursive(
            //                                              //Valid if it creates a cycle in the calculations
            //                                              //Return true if the cycle is created

            List<CalentityCalculationEntityDB> darrcalentity_I,
            int intPkResourceI_I
            )
        {
            bool boolCycleIsCreated = false;

            if (
                darrcalentity_I.Count == 0
                )
            {
            }
            else
            {
                int intI = 0;
                /*WHILE-DO*/
                while (
                    intI < darrcalentity_I.Count && !boolCycleIsCreated
                    )
                {
                    CalentityCalculationEntityDB calentity = darrcalentity_I[intI];

                    CalCalculation.subCycleIsCreated(calentity, intPkResourceI_I, ref boolCycleIsCreated);

                    intI = intI + 1;
                }
            }

            return boolCycleIsCreated;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subCycleIsCreated(
            //                                              //Return true if the cycle is created

            CalentityCalculationEntityDB calentity_I,
            int intPkResourceI_I,
            ref bool boolCycleIsCreated_IO
            )
        {
            /*CASE*/
            if (
                calentity_I.intnPkQFromResource == null
                )
            {
                boolCycleIsCreated_IO = false;
            }
            else if (
                calentity_I.intnPkQFromResource == intPkResourceI_I
                )
            {
                boolCycleIsCreated_IO = true;
            }
            else
            {
                Odyssey2Context context = new Odyssey2Context();

                List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(cal =>
                    cal.intnPkProduct == calentity_I.intnPkProduct &&
                    cal.intnPkWorkflow == calentity_I.intnPkWorkflow &&
                    cal.intnProcessInWorkflowId == calentity_I.intnProcessInWorkflowId &&
                    cal.intnPkResource == calentity_I.intnPkQFromResource).ToList();

                boolCycleIsCreated_IO = CalCalculation.boolCycleIsCreatedRecursive(darrcalentity, intPkResourceI_I);
            }
            /*END-CASE*/
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDelete(
            int intPk_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();


            //                                              //Search if the cal exists.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(calentity =>
                calentity.intPk == intPk_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Calculation not found.";
            if (
                calentity != null
                )
            {
                if (
                    CalCalculation.boolCalculationNeedsToBeCopied(calentity.intnPkProduct, calentity.intnPkWorkflow,
                        calentity.strStartDate, calentity.strStartTime, calentity.strByX)
                )
                {
                    //                                      //Set end date and time for the calculation.
                    calentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                    calentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                    context.Calculation.Update(calentity);
                }
                else
                {
                    //                                      //Delete conditions.
                    Tools.subDeleteCondition(calentity.intPk, null, null, null, context);

                    //                                      //calculation is not used by a job in progress/completed.

                    //                                      //Find all periods associated to this cal from´piwfj.
                    List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                        per.intnPkCalculation == calentity.intPk).ToList();

                    foreach (PerentityPeriodEntityDB perentity in darrperentity)
                    {
                        //                                  //Delete Period.
                        context.Period.Remove(perentity);
                    }

                    //                                      //Find all paper transformations related to this cal.
                    List<PatransPaperTransformationEntityDB> darrpatransentity = context.PaperTransformation.Where(
                        paper => paper.intnPkCalculationOwn == calentity.intPk ||
                        paper.intnPkCalculationLink == calentity.intPk).ToList();

                    foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                    {
                        //                                  //Delete paper transformations.
                        context.PaperTransformation.Remove(patransentity);
                    }

                    context.Calculation.Remove(calentity);
                }

                context.SaveChanges();

                intStatus_IO = 200;
                strUserMessage_IO = " ";
                strDevMessage_IO = " ";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteCalculationsWhenUnitChange(
            //                                              //Delete calculation when the unit of measurement
            //                                              //          is change.

            int intPkResource_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get the calculations associated with the resource
            //                                              //      not edited or deleted wich means column EndDate
            //                                              //      null.
            List<CalentityCalculationEntityDB> darrcalentity = context_M.Calculation.Where(cal =>
                cal.intnPkResource == intPkResource_I && cal.strEndDate == null).ToList();

            if (
                //                                          //There is at least one calculation.
                darrcalentity.Count() > 0
                )
            {
                //                                          //Set each calculation as deleted (column 
                //                                          //      end date no null).
                foreach (CalentityCalculationEntityDB calentityToDelete in darrcalentity)
                {
                    //                                      //Verify if it's necessary to create a copy.
                    if (
                        //                                  //Calculation needs to be copy.
                        CalCalculation.boolCalculationNeedsToBeCopied(calentityToDelete.intnPkProduct,
                            calentityToDelete.intnPkWorkflow, calentityToDelete.strStartDate,
                            calentityToDelete.strStartTime, calentityToDelete.strByX)
                        )
                    {
                        calentityToDelete.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                        calentityToDelete.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                        context_M.Calculation.Update(calentityToDelete);

                        CalentityCalculationEntityDB calentityToAdd = new CalentityCalculationEntityDB
                        {
                            intnPkProduct = calentityToDelete.intnPkProduct,
                            intnPkProcess = calentityToDelete.intnPkProcess,
                            intnPkResource = calentityToDelete.intnPkResource,
                            strCalculationType = calentityToDelete.strCalculationType,
                            strDescription = calentityToDelete.strDescription,
                            boolIsEnable = calentityToDelete.boolIsEnable,
                            numnCost = calentityToDelete.numnCost,
                            numnQuantity = calentityToDelete.numnQuantity,
                            strUnit = calentityToDelete.strUnit,
                            numnMin = calentityToDelete.numnMin,
                            numnBlock = calentityToDelete.numnBlock,
                            strAscendants = calentityToDelete.strAscendants,
                            strValue = calentityToDelete.strValue,
                            numnNeeded = calentityToDelete.numnNeeded,
                            numnPerUnits = calentityToDelete.numnPerUnits,
                            strByX = calentityToDelete.strByX,
                            intnPkWorkflow = calentityToDelete.intnPkWorkflow,
                            intnProcessInWorkflowId = calentityToDelete.intnProcessInWorkflowId,
                            intnPkElementElementType = calentityToDelete.intnPkElementElementType,
                            intnPkElementElement = calentityToDelete.intnPkElementElement,
                            strStartDate = calentityToDelete.strEndDate,
                            strStartTime = calentityToDelete.strEndTime,
                            numnQuantityWaste = calentityToDelete.numnQuantityWaste,
                            numnPercentWaste = calentityToDelete.numnPercentWaste,
                            intnHours = calentityToDelete.intnHours,
                            intnMinutes = calentityToDelete.intnMinutes,
                            intnSeconds = calentityToDelete.intnSeconds
                        };
                        context_M.Calculation.Add(calentityToAdd);
                        context_M.SaveChanges();

                        //                                  //Get calculation condition.
                        GpcondjsonGroupConditionJson gpcondition =  
                            Tools.gpcondjsonGetCondition(calentityToDelete.intPk, null, null, null);

                        //                                  //Create conditions for the new cal added.
                        Tools.subAddCondition(calentityToAdd.intPk, null, null, null, gpcondition, context_M);                        
                    }
                    else
                    {
                        //                                  //Find all periods associated to this cal from´piwfj.
                        List<PerentityPeriodEntityDB> darrperentity = context_M.Period.Where(per =>
                            per.intnPkCalculation == calentityToDelete.intPk).ToList();

                        foreach (PerentityPeriodEntityDB perentity in darrperentity)
                        {
                            //                              //Delete Period.
                            context_M.Period.Remove(perentity);
                        }

                        //                                  //Find all paper transformations related to this cal.
                        List<PatransPaperTransformationEntityDB> darrpatransentity = context_M.PaperTransformation.Where(
                            paper => paper.intnPkCalculationOwn == calentityToDelete.intPk ||
                            paper.intnPkCalculationLink == calentityToDelete.intPk).ToList();

                        foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                        {
                            //                              //Delete paper transformations.
                            context_M.PaperTransformation.Remove(patransentity);
                        }

                        //                                  //Just delete the calculation.
                        context_M.Calculation.Remove(calentityToDelete);
                    }
                }
                context_M.SaveChanges();
            }
        }
         
        //--------------------------------------------------------------------------------------------------------------
        public static void subModify(
            PsPrintShop ps_I,
            CalCalculation calNew_I,
            GpcondjsonGroupConditionJson gpcondCondition_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Search if the cal exist.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(calentity =>
                calentity.intPk == calNew_I.intPk);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No calculation found.";
            if (
                //                                          //Cal found.
                calentity != null
                )
            {
                //                                          //Find the condition.
                ColcondentityCalculationOrLinkConditionEntityDB colcondentity = context.CalculationOrLinkCondition.
                    FirstOrDefault(colcond => colcond.intnPkCalculation == calentity.intPk &&
                    colcond.intnPkLinkNode == null);

                //                                          //Assing general expense account if calculation does not
                //                                          //      contain one.
                int intPkAccount = calNew_I.intnPkAccount != null ? (int)calNew_I.intnPkAccount :
                    ps_I.intGetExpensePkAccount(context);
                bool boolAccountValid = AccAccounting.boolIsExpenseValid(intPkAccount, context);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Calculation type invalid.";
                /*CASE*/
                if (
                    //                                      //Profit calculation.
                    calNew_I.strCalculationType == CalCalculation.strProfit
                    )
                {
                    if (
                        (calNew_I.numnProfit != null) &&
                        (calNew_I.numnProfit > 0)
                        )
                    {
                        if (
                            CalCalculation.boolCalculationNeedsToBeCopied(calentity.intnPkProduct,
                                calentity.intnPkWorkflow, calentity.strStartDate, calentity.strStartTime,
                                calentity.strByX)
                            )
                        {
                            //                              //Create story point.
                            //                              //Set end date and time.
                            calentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                            calentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                            context.Calculation.Update(calentity);

                            //                              //Add calculation edited to db.
                            CalentityCalculationEntityDB calentityToAdd = new CalentityCalculationEntityDB
                            {
                                intnPkProduct = calentity.intnPkProduct,
                                strCalculationType = CalCalculation.strProfit,
                                strDescription = calNew_I.strDescription,
                                boolIsEnable = calNew_I.boolIsEnable,
                                numnProfit = calNew_I.numnProfit,
                                strByX = calNew_I.strByX,
                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString()
                            };
                            context.Calculation.Add(calentityToAdd);
                        }
                        else
                        {
                            //                              //calculation is not used by a job in progress/completed.
                            calentity.strDescription = calNew_I.strDescription;
                            calentity.boolIsEnable = calNew_I.boolIsEnable;
                            calentity.numnProfit = calNew_I.numnProfit;
                            calentity.strByX = calNew_I.strByX;
                        }
                        context.SaveChanges();

                        if (
                            calNew_I.boolIsEnable
                            )
                        {
                            CalCalculation.subDisableOtherProfitCalculations(calentity);
                        }
                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
                else if (
                    //                                      //Base calculation.
                    calNew_I.strCalculationType == CalCalculation.strBase
                    )
                {
                    if (
                        (calNew_I.numnCost != null) &&
                        (calNew_I.numnCost > 0) &&
                        Tools.boolValidConditionList(gpcondCondition_I)
                        )
                    {
                        intStatus_IO = 406;
                        strUserMessage_IO = "Account not valid.";
                        strDevMessage_IO = "Account not valid.";
                        if (
                            boolAccountValid
                            )
                        {
                            int intPkCalculation = calentity.intPk;
                            bool boolCopied = CalCalculation.boolCalculationNeedsToBeCopied(calentity.intnPkProduct,
                                    calentity.intnPkWorkflow, calentity.strStartDate, calentity.strStartTime,
                                    calentity.strByX);
                            if (
                                boolCopied
                                )
                            {
                                //                      //Set end date and time.
                                calentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                                calentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                                context.Calculation.Update(calentity);

                                //                      //Add calculation edited to db.
                                CalentityCalculationEntityDB calentityToAdd = new CalentityCalculationEntityDB
                                {
                                    intnPkProduct = calentity.intnPkProduct,
                                    intnPkProcess = calentity.intnPkProcess,
                                    strCalculationType = CalCalculation.strBase,
                                    strDescription = calNew_I.strDescription,
                                    boolIsEnable = calNew_I.boolIsEnable,
                                    numnCost = calNew_I.numnCost,
                                    strByX = calNew_I.strByX,
                                    intnPkWorkflow = calentity.intnPkWorkflow,
                                    intnProcessInWorkflowId = calentity.intnProcessInWorkflowId,
                                    intnPkElementElementType = calentity.intnPkElementElementType,
                                    intnPkElementElement = calentity.intnPkElementElement,
                                    strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                    strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                    intnPkAccount = intPkAccount
                                };
                                context.Calculation.Add(calentityToAdd);
                                context.SaveChanges();

                                intPkCalculation = calentityToAdd.intPk;

                                if (
                                    gpcondCondition_I == null
                                    )
                                {
                                    //                                  //Get calculation condition.
                                    gpcondCondition_I =
                                        Tools.gpcondjsonGetCondition(calentity.intPk, null, null, null);
                                }   

                                Tools.subAddCondition(intPkCalculation, null, null, null, gpcondCondition_I, context);

                            }
                            else
                            {
                                calentity.strDescription = calNew_I.strDescription;
                                calentity.boolIsEnable = calNew_I.boolIsEnable;
                                calentity.numnCost = calNew_I.numnCost;
                                calentity.strByX = calNew_I.strByX;
                                calentity.intnPkAccount = intPkAccount;

                                context.Calculation.Update(calentity);
                                context.SaveChanges();

                                if (
                                    gpcondCondition_I != null
                                    )
                                {
                                    //                              //Delete the condition and add the new condition.
                                    Tools.subDeleteCondition(calentity.intPk, null, null, null, context);

                                    Tools.subAddCondition(intPkCalculation, null, null, null, gpcondCondition_I, context);
                                }
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
                else if (
                    //                                      //Per quantity calculation.
                    calNew_I.strCalculationType == CalCalculation.strPerQuantity
                    )
                {
                    if (
                        (((calNew_I.numnCost > 0) && (calNew_I.numnQuantity > 0)) ||
                        (calNew_I.strByX == CalCalculation.strByResource)) &&
                        Tools.boolValidConditionList(gpcondCondition_I)
                        )
                    {
                        if (
                            (calNew_I.strByX == CalCalculation.strByProcess) ||
                            (calNew_I.strByX == CalCalculation.strByResource) ||
                            (((calNew_I.strByX == CalCalculation.strByProduct) ||
                            (calNew_I.strByX == CalCalculation.strByIntent)) &&
                            (((int)calNew_I.numnMin / calNew_I.numnMin) == 1))
                            )
                        {
                            //                              //To easy code.
                            double? numnNeeded = calNew_I.numnNeeded;
                            double? numnPerUnits = calNew_I.numnPerUnits;

                            bool boolIsValidCalculation = true;
                            double? numnQuantityWaste = null;
                            double? numnPercentWaste = null;

                            if (
                                //                          //Calculation per quantity by resource.
                                calNew_I.strByX == CalCalculation.strByResource
                                )
                            {
                                intStatus_IO = 403;
                                strUserMessage_IO = "Waste cannot be less than 0";
                                strDevMessage_IO = "";
                                if (
                                    //                      //Correct values in waste.
                                    ((calNew_I.numnQuantityWaste >= 0) &&
                                    (calNew_I.numnPercentWaste >= 0)) ||
                                    ((calNew_I.numnQuantityWaste >= 0) &&
                                    (calNew_I.numnPercentWaste == null)) ||
                                    ((calNew_I.numnQuantityWaste == null) &&
                                    (calNew_I.numnPercentWaste >= 0)) ||
                                    ((calNew_I.numnQuantityWaste == null) &&
                                    (calNew_I.numnPercentWaste == null))
                                    )
                                {
                                    //                      //If waste = null then waste = 0 or just normal value.
                                    numnQuantityWaste = calNew_I.numnQuantityWaste == null ? 0 :
                                        calNew_I.numnQuantityWaste;
                                    numnPercentWaste = calNew_I.numnPercentWaste == null ? 0 :
                                        calNew_I.numnPercentWaste;
                                }
                                else
                                {
                                    //                      //Incorrect values.
                                    boolIsValidCalculation = false;
                                }

                                if (
                                    //                      //Valid that is not the same IO.
                                    calentity.intnPkElementElementType ==
                                    calNew_I.intnPkQFromElementElementTypeBelongsTo &&
                                    calentity.intnPkElementElement ==
                                    calNew_I.intnPkQFromElementElementBelongsTo
                                    )
                                {
                                    intStatus_IO = 405;
                                    strUserMessage_IO = "Invalid Quantity From.";
                                    strDevMessage_IO = "Can be the same IO or the InputIO is not belong to " +
                                        "other Input IO for this process.";

                                    boolIsValidCalculation = false;
                                }

                                //                          //Get resourceO data.
                                EleentityElementEntityDB eleentityOutput = context.Element.FirstOrDefault(ele =>
                                    ele.intPk == calNew_I.intnPkQFromResourceElementBelongsTo);

                                //                          //To easy code.
                                EtentityElementTypeEntityDB etentityRestypOutput = context.ElementType.FirstOrDefault(et =>
                                    et.intPk == eleentityOutput.intPkElementType);

                                bool boolResOIsPaper = (etentityRestypOutput.strCustomTypeId == ResResource.strComponent) ||
                                    (etentityRestypOutput.strCustomTypeId == ResResource.strMedia);

                                if (
                                    (calNew_I.boolnFromThickness == null ||
                                    calNew_I.boolnFromThickness == false
                                    )
                                    ||
                                    (
                                    //                      //ResOutput has a component or media
                                    //                      //    for receive fromThickness(true).
                                    calNew_I.boolnFromThickness == true && calNew_I.numnNeeded != null &&
                                    calNew_I.numnPerUnits == null
                                    )
                                    )
                                {
                                    //                      //Do not something. 
                                }
                                else
                                {
                                    intStatus_IO = 405;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "Thickness only can be ResQfrom media or component.";
                                    boolIsValidCalculation = false;
                                }

                                if (
                                    boolIsValidCalculation
                                    )
                                {
                                    List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(cal =>
                                        cal.intnPkProduct == calentity.intnPkProduct &&
                                        cal.intnPkWorkflow == calentity.intnPkWorkflow &&
                                        cal.intnProcessInWorkflowId == calentity.intnProcessInWorkflowId &&
                                        cal.intnPkResource == calNew_I.intnPkQFromResourceElementBelongsTo).ToList();

                                    bool boolCycleIsCreated = CalCalculation.boolCycleIsCreatedRecursive(darrcalentity,
                                        (int)calentity.intnPkResource);

                                    if (boolCycleIsCreated)
                                    {
                                        intStatus_IO = 404;
                                        strUserMessage_IO = "Cycle is created with this calculation.";
                                        strDevMessage_IO = "";
                                        boolIsValidCalculation = false;
                                    }
                                }

                                //                          //Get resourceI data.
                                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                                    ele.intPk == calentity.intnPkResource);

                                //                          //To easy code.
                                EtentityElementTypeEntityDB etentityRestyp = context.ElementType.FirstOrDefault(et =>
                                    et.intPk == eleentity.intPkElementType);

                                bool boolResIIsPaper = (etentityRestyp.strCustomTypeId == ResResource.strComponent) ||
                                    (etentityRestyp.strCustomTypeId == ResResource.strMedia);

                                //                          //Find Qfrom IO.
                                IoentityInputsAndOutputsEntityDB ioentityQfromRes = context.InputsAndOutputs.FirstOrDefault(
                                    io => io.intPkWorkflow == calentity.intnPkWorkflow &&
                                    io.intnProcessInWorkflowId == calentity.intnProcessInWorkflowId &&
                                    io.intnPkElementElementType == calentity.intnPkQFromElementElementType &&
                                    io.intnPkElementElement == calentity.intnPkQFromElementElement);

                                bool boolResOIsSizeBearer = false;
                                if (
                                    ioentityQfromRes != null
                                    )
                                {
                                    if (
                                        ioentityQfromRes.boolnSize == true
                                        )
                                    {
                                        boolResOIsSizeBearer = true;
                                    }
                                }

                                if (
                                    //                      //ResI is paper and ResO (QFrom) is size bearer
                                    boolResIIsPaper && boolResOIsSizeBearer
                                    )
                                {
                                    numnNeeded = 1;
                                    numnPerUnits = null;
                                }

                                //                          //If calculation still valid and is media or component, we
                                //                          //      have to verify if the value for boolByArea is
                                //                          //      valid, checking if the resource has AreaUnit.
                                if (
                                    boolIsValidCalculation && boolResOIsPaper
                                    )
                                {
                                    //                      //To easy code.
                                    bool boolComponent = etentityRestyp.strCustomTypeId == ResResource.strComponent ?
                                        true : false;

                                    boolIsValidCalculation =
                                        (calNew_I.boolnByArea == null || !(bool)calNew_I.boolnByArea) ? true : 
                                        CalCalculation.boolnByAreaIsValid(eleentityOutput.intPk,
                                        boolComponent, calNew_I.boolnByArea);
                                }
                            }

                            //                              //For PQ byprocess and byproduct consider account.
                            if (
                                (calNew_I.strByX == CalCalculation.strByProcess) ||
                                (calNew_I.strByX == CalCalculation.strByProduct)
                                )
                            {
                                if (
                                    !boolAccountValid
                                    )
                                {
                                    intStatus_IO = 407;
                                    strUserMessage_IO = "Account not valid.";
                                    strDevMessage_IO = "Account not valid.";
                                    boolIsValidCalculation = false;
                                }
                            }

                            intStatus_IO = 407;
                            strUserMessage_IO = "Something is wrong";
                            strDevMessage_IO = "Invalid data.";
                            if (
                                //                           //Only if everything is correct the data will be saved.
                                boolIsValidCalculation
                                )
                            {
                                intStatus_IO = 200;
                                strUserMessage_IO = "Success.";
                                strDevMessage_IO = "";

                                double? numnMin = calNew_I.numnMin;
                                double? numnBlock = calNew_I.numnBlock;
                                if (
                                    numnBlock != null && numnBlock > 0
                                    )
                                {
                                    //                      //Calculate Min to use according ByBlock´s value.
                                    numnMin = ResResource.numnGetMinToUse(numnMin, numnBlock);
                                }

                                int intPkCalculation = calentity.intPk;
                                bool boolCopied = CalCalculation.boolCalculationNeedsToBeCopied(calentity.intnPkProduct,
                                    calentity.intnPkWorkflow, calentity.strStartDate, calentity.strStartTime,
                                    calentity.strByX);
                                if (
                                    boolCopied
                                    )
                                {
                                    //                  //Create story point. 
                                    //                  //Set end date and time.
                                    calentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                                    calentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                                    context.Calculation.Update(calentity);

                                    //                  //Add calculation edited to db.
                                    CalentityCalculationEntityDB calentityToAdd = new CalentityCalculationEntityDB
                                    {
                                        intnPkProduct = calentity.intnPkProduct,
                                        intnPkProcess = calentity.intnPkProcess,
                                        intnPkResource = calentity.intnPkResource,
                                        strCalculationType = CalCalculation.strPerQuantity,
                                        strDescription = calNew_I.strDescription,
                                        boolIsEnable = calNew_I.boolIsEnable,
                                        numnCost = calNew_I.numnCost,
                                        numnQuantity = calNew_I.numnQuantity,
                                        strUnit = calNew_I.strUnit,
                                        numnMin = numnMin,
                                        numnQuantityWaste = numnQuantityWaste,
                                        numnPercentWaste = numnPercentWaste,
                                        numnBlock = numnBlock,
                                        strAscendants = calNew_I.strAscendants,
                                        strValue = calNew_I.strValue,
                                        numnNeeded = calNew_I.numnNeeded,
                                        numnPerUnits = (calNew_I.boolnFromThickness == true ? null :
                                        calNew_I.numnPerUnits),
                                        strByX = calNew_I.strByX,
                                        intnPkWorkflow = calentity.intnPkWorkflow,
                                        intnProcessInWorkflowId = calentity.intnProcessInWorkflowId,
                                        intnPkElementElementType = calentity.intnPkElementElementType,
                                        intnPkElementElement = calentity.intnPkElementElement,
                                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                        intnPkQFromElementElementType =
                                            calNew_I.intnPkQFromElementElementTypeBelongsTo,
                                        intnPkQFromElementElement = calNew_I.intnPkQFromElementElementBelongsTo,
                                        intnPkQFromResource = calNew_I.intnPkQFromResourceElementBelongsTo,
                                        intnPkAccount = intPkAccount,
                                        boolnFromThickness = calNew_I.boolnFromThickness,
                                        boolnIsBlock = (calNew_I.strByX == CalCalculation.strByResource ?
                                            (calNew_I.boolnIsBlock == null ? false : calNew_I.boolnIsBlock)
                                            : null),
                                        boolnByArea = calNew_I.boolnByArea

                                    };
                                    context.Calculation.Add(calentityToAdd);
                                    context.SaveChanges();


                                    intPkCalculation = calentityToAdd.intPk;

                                    if (
                                        gpcondCondition_I == null
                                        )
                                    {
                                        //                  //Get calculation condition.
                                        gpcondCondition_I =
                                            Tools.gpcondjsonGetCondition(calentity.intPk, null, null, null);
                                    }

                                    Tools.subAddCondition(intPkCalculation, null, null, null, gpcondCondition_I, context);
                                }
                                else
                                {
                                    //                      //calculation is not used by a job in
                                    //                      //      progress/completed.
                                    calentity.strDescription = calNew_I.strDescription;
                                    calentity.boolIsEnable = calNew_I.boolIsEnable;
                                    calentity.numnCost = calNew_I.numnCost;
                                    calentity.numnQuantity = calNew_I.numnQuantity;
                                    calentity.strUnit = calNew_I.strUnit;
                                    calentity.numnMin = numnMin;
                                    calentity.numnQuantityWaste = numnQuantityWaste;
                                    calentity.numnPercentWaste = numnPercentWaste;
                                    calentity.numnBlock = numnBlock;
                                    calentity.numnNeeded = numnNeeded;
                                    calentity.numnPerUnits = (calNew_I.boolnFromThickness == true ? null : numnPerUnits);
                                    calentity.intnPkQFromElementElementType =
                                    calNew_I.intnPkQFromElementElementTypeBelongsTo;
                                    calentity.intnPkQFromElementElement =
                                    calNew_I.intnPkQFromElementElementBelongsTo;
                                    calentity.intnPkQFromResource = calNew_I.intnPkQFromResourceElementBelongsTo;
                                    calentity.intnPkAccount = intPkAccount;
                                    calentity.boolnFromThickness = calNew_I.boolnFromThickness;
                                    calentity.boolnIsBlock = (calNew_I.strByX == CalCalculation.strByResource ?
                                            (calNew_I.boolnIsBlock == null ? false : calNew_I.boolnIsBlock)
                                            : null);
                                    calentity.boolnByArea = calNew_I.boolnByArea;

                                    context.Calculation.Update(calentity);
                                    context.SaveChanges();

                                    if (
                                        calentity.intnPkWorkflow != null
                                        )
                                    {
                                        //                  //Update paperTransformation.
                                        CalCalculation.subUpdatePaperWhenUpdateCal(calentity);
                                    }

                                    if (
                                        gpcondCondition_I != null
                                    )
                                    {
                                        //                  //Delete the condition and add the new condition.
                                        Tools.subDeleteCondition(calentity.intPk, null, null, null, context);

                                        Tools.subAddCondition(intPkCalculation, null, null, null, gpcondCondition_I, context);
                                    }
                                }
                                
                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
                else if (
                    //                                      //Base quantity calculation.
                    calNew_I.strCalculationType == CalCalculation.strPerQuantityBase
                    )
                {
                    if (
                        Tools.boolValidConditionList(gpcondCondition_I)
                        )
                    {
                        if (
                            (calNew_I.strByX == CalCalculation.strByResource)
                            )
                        {
                            intStatus_IO = 408;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Needed must be greater than 0.";

                            if (
                               calNew_I.numnNeeded > 0
                               )
                            {
                                intStatus_IO = 409;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "The calculations are not the same type.";
                                if (
                                    calNew_I.strCalculationType == calentity.strCalculationType
                                    )
                                {

                                    int intPkCalculation = calentity.intPk;
                                    bool boolCopied = CalCalculation.boolCalculationNeedsToBeCopied(calentity.intnPkProduct,
                                        calentity.intnPkWorkflow, calentity.strStartDate, calentity.strStartTime,
                                            calentity.strByX);
                                    if (
                                        boolCopied
                                        )
                                    {
                                        //              //Create story point. 
                                        //              //Set end date and time.
                                        calentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                                        calentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                                        context.Calculation.Update(calentity);

                                        //              //Add calculation edited to db.
                                        CalentityCalculationEntityDB calentityToAdd =
                                            new CalentityCalculationEntityDB
                                            {
                                                intnPkProduct = calentity.intnPkProduct,
                                                intnPkResource = calentity.intnPkResource,
                                                strCalculationType = CalCalculation.strPerQuantityBase,
                                                strDescription = calNew_I.strDescription,
                                                boolIsEnable = calNew_I.boolIsEnable,
                                                numnNeeded = calNew_I.numnNeeded,
                                                strByX = calNew_I.strByX,
                                                intnPkWorkflow = calentity.intnPkWorkflow,
                                                intnProcessInWorkflowId = calentity.intnProcessInWorkflowId,
                                                intnPkElementElementType = calentity.intnPkElementElementType,
                                                intnPkElementElement = calentity.intnPkElementElement,
                                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString()
                                            };

                                        context.Calculation.Add(calentityToAdd);
                                        context.SaveChanges();

                                        intPkCalculation = calentityToAdd.intPk;

                                        if (
                                            gpcondCondition_I == null
                                            )
                                        {
                                            //              //Get calculation condition.
                                            gpcondCondition_I =
                                                Tools.gpcondjsonGetCondition(calentity.intPk, null, null, null);
                                        }

                                        Tools.subAddCondition(intPkCalculation, null, null, null, gpcondCondition_I, context);
                                    }
                                    else
                                    {
                                        //              //calculation is not used by a job in progress/completed.
                                        calentity.strDescription = calNew_I.strDescription;
                                        calentity.boolIsEnable = calNew_I.boolIsEnable;
                                        calentity.numnNeeded = calNew_I.numnNeeded;

                                        context.Calculation.Update(calentity);
                                        context.SaveChanges();

                                        if (
                                            gpcondCondition_I != null
                                            )
                                        {
                                            //              //Delete the condition and add the new condition.
                                            Tools.subDeleteCondition(calentity.intPk, null, null, null, context);

                                            Tools.subAddCondition(intPkCalculation, null, null, null, gpcondCondition_I, context);
                                        }
                                        
                                    }

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
                /*END-CASE*/
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolCalculationNeedsToBeCopied(
           //                                              //Returns true if a calculation is in a pending job or 
           //                                              //      completed.

           int? intnPkProduct_I,
           int? intnPkWorkflow_I,
           String strStartDate_I,
           String strStartTime_I,
           String strByX_I
           )
        {
            bool boolCalNeedsToBeCopied = false;

            //                                              //List where workflows are stored.
            List<WfentityWorkflowEntityDB> darrwfentity = new List<WfentityWorkflowEntityDB>();

            //                                              //Calculation start time.
            ZonedTime ztimecalStartDate = ZonedTimeTools.NewZonedTime(strStartDate_I.ParseToDate(),
                strStartTime_I.ParseToTime());

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            if (
                //                                          //Only Calculation ByProduct.
                strByX_I == CalCalculation.strByProduct &&
                //                                          //Find jobs with PkProduct.
                intnPkProduct_I > 0 &&
                intnPkWorkflow_I == null
                )
            {
                //                                          //Find product´s workflows.
                IQueryable<WfentityWorkflowEntityDB> setwfentity = context.Workflow.Where(wf =>
                    wf.intnPkProduct == intnPkProduct_I && wf.boolDeleted == false);
                darrwfentity = setwfentity.ToList();
            }

            if (
                //                                          //Find jobs with PkWorkflow.
                intnPkWorkflow_I > 0 &&
                intnPkProduct_I > 0
                )
            {
                //                                          //Find workflow.
                WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intnPkWorkflow_I && wf.boolDeleted == false);
                darrwfentity.Add(wfentity);
            }

            //                                              //Iterate over each workflow.
            int intJ = 0;
            /*WHILE-DO*/
            while (
                 intJ < darrwfentity.Count &&
                !boolCalNeedsToBeCopied
                )
            {
                //                                          //Find workflow´s jobs.
                IQueryable<JobentityJobEntityDB> setjobentity = context.Job.Where(job =>
                    job.intPkWorkflow == darrwfentity[intJ].intPk);
                List<JobentityJobEntityDB> darrjobentity = setjobentity.ToList();

                //                                          //Iterate over each job.
                int intK = 0;
                /*WHILE-DO*/
                while (
                     intK < darrjobentity.Count &&
                    !boolCalNeedsToBeCopied
                    )
                {
                    //                                      //Job start date.
                    ZonedTime ztimejobStartDate = ZonedTimeTools.NewZonedTime(darrjobentity
                        [intK].strStartDate.ParseToDate(), darrjobentity[intK].strStartTime.ParseToTime());

                    if (
                        //                                  //Job started after cal´s start date.
                        ztimejobStartDate >= ztimecalStartDate
                    )
                    {
                        boolCalNeedsToBeCopied = true;
                    }

                    intK = intK + 1;
                }

                intJ = intJ + 1;
            }

            return boolCalNeedsToBeCopied;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdatePaperWhenUpdateCal(
           //                                               //Update the register for a calculation in 
           //                                               //      PaperTransformation table.

           CalentityCalculationEntityDB calentity_I
           )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get piw entity.
            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw
                => piw.intPkWorkflow == calentity_I.intnPkWorkflow &&
                piw.intProcessInWorkflowId == calentity_I.intnProcessInWorkflowId);

            //                                              //Get paperTransformation.
            PatransPaperTransformationEntityDB patransEntityOwn = context.PaperTransformation.FirstOrDefault(pa
                => pa.intnPkCalculationOwn == calentity_I.intPk);

            if (
                //                                          //Calculation has paperTransformation associated.
                patransEntityOwn != null
                )
            {
                //                                          //Verify if the Paper was modified.
                PatransPaperTransformationEntityDB patransentityOwnNew = context.PaperTransformation.FirstOrDefault(pa
                    => pa.intnPkCalculationOwn == null &&
                    pa.intPkProcessInWorkflow == patransEntityOwn.intPkProcessInWorkflow &&
                    pa.intPkResourceI == patransEntityOwn.intPkResourceI &&
                    pa.intnPkResourceO == patransEntityOwn.intnPkResourceO &&
                    pa.intnPkElementElementTypeI == patransEntityOwn.intnPkElementElementTypeI &&
                    pa.intnPkElementElementI == patransEntityOwn.intnPkElementElementI &&
                    pa.intnPkElementElementTypeO == patransEntityOwn.intnPkElementElementTypeO &&
                    pa.intnPkElementElementO == patransEntityOwn.intnPkElementElementO &&
                    pa.boolTemporary == true);

                if (
                    //                                      //This is the calculation we need to keep.
                    patransentityOwnNew != null
                    )
                {
                    patransentityOwnNew.intnPkCalculationOwn = patransEntityOwn.intnPkCalculationOwn;
                    patransentityOwnNew.boolTemporary = false;
                    context.PaperTransformation.Update(patransentityOwnNew);

                    //                                      //Remove propagate transformation.
                    //                                      //Get all paper register where te pkCal was propagate.
                    List<PatransPaperTransformationEntityDB> darrpatransPropToDelete =
                        context.PaperTransformation.Where(pa =>
                        pa.intnPkCalculationLink == patransEntityOwn.intnPkCalculationOwn).ToList();

                    foreach (PatransPaperTransformationEntityDB patransToDelete in darrpatransPropToDelete)
                    {
                        context.PaperTransformation.Remove(patransToDelete);
                    }

                    //                                      //Delete obsolote paper for the calculation.
                    context.PaperTransformation.Remove(patransEntityOwn);

                    //                                  //Verify if the output IO has link. It has, create a 
                    //                                  //      register for the other side of the link
                    List<IoentityInputsAndOutputsEntityDB> darrioentity =
                        CalCalculation.darrioentityVerifyIfIOHasLink(piwentity,
                        patransentityOwnNew.intnPkElementElementTypeO, patransentityOwnNew.intnPkElementElementO);

                    foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
                    {
                        //                              //Get piwentity
                        PiwentityProcessInWorkflowEntityDB piwentityOut =
                            context.ProcessInWorkflow.FirstOrDefault(piw =>
                            piw.intPkWorkflow == ioentity.intPkWorkflow &&
                            piw.intProcessInWorkflowId == ioentity.intnProcessInWorkflowId);

                        //                              //Get all link's register.
                        List<PatransPaperTransformationEntityDB> darrpatrans = context.PaperTransformation.Where(pa
                            => pa.boolTemporary == false && pa.intPkProcessInWorkflow == piwentityOut.intPk &&
                            pa.intPkResourceI == patransentityOwnNew.intnPkResourceO && pa.intnPkResourceO == null &&
                            pa.intnPkElementElementTypeI == ioentity.intnPkElementElementType &&
                            pa.intnPkElementElementI == ioentity.intnPkElementElement &&
                            pa.intnPkElementElementTypeO == null && pa.intnPkElementElementO == null &&
                            pa.intnPkCalculationLink == null).ToList();

                        if (
                            //
                            darrpatrans.Count() == 2
                            )
                        {
                            PatransPaperTransformationEntityDB patransToDelete = darrpatrans[0];
                            darrpatrans.Remove(patransToDelete);
                            context.PaperTransformation.Remove(patransToDelete);
                        }

                        PatransPaperTransformationEntityDB patransToUpdate = darrpatrans[0];
                        patransToUpdate.numWidthI = patransentityOwnNew.numWidthO;
                        patransToUpdate.numnHeightI = patransentityOwnNew.numHeightO;
                        patransToUpdate.numWidthO = 0.0;
                        patransToUpdate.numHeightO = 0.0;
                        patransToUpdate.intnPkCalculationLink = patransentityOwnNew.intnPkCalculationOwn;
                        context.PaperTransformation.Update(patransentityOwnNew);
                    }

                    context.SaveChanges();
                }
            }
            else
            {
                CalCalculation.subAddPkCalculationToPaperTransformation(calentity_I, piwentity.intPk);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDisableOtherProfitCalculations(
            CalentityCalculationEntityDB calentity_I
            )
        {
            if (
                calentity_I.boolIsEnable
                )
            {
                Odyssey2Context context = new Odyssey2Context();
                IQueryable<CalentityCalculationEntityDB> setcalentity = context.Calculation.Where(cal =>
                cal.strCalculationType == CalCalculation.strProfit && cal.intnPkProduct == calentity_I.intnPkProduct);
                List<CalentityCalculationEntityDB> darrcalentity = setcalentity.ToList();

                foreach (CalentityCalculationEntityDB cal in darrcalentity)
                {
                    if (
                            cal.intPk != calentity_I.intPk
                        )
                    {
                        cal.boolIsEnable = false;
                        context.SaveChanges();
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolnByAreaIsValid(
           //                                               //Returns true if its a PQ ByRes calculation and:
           //                                               //QFromRes is Media or Component, has AreaUnit and the
           //                                               //      value for boolnArea != null.

           int intPkResource_I,
           bool boolComponent_I,
           bool? boolnByArea_I
           )
        {
            bool boolnByAreaIsValid;

            //                                              //Get AreaUnit
            String strAreaUnit = ProdtypProductType.strGetAreaUnit(intPkResource_I, boolComponent_I);

            //                                              //True when the resources has areaUnit and is a valid value
            //                                              //      fot boolByArea (true or false, not null).
            boolnByAreaIsValid = (strAreaUnit != null && boolnByArea_I != null) ? true : false;


            return boolnByAreaIsValid;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGroup(
            int[] arrintPk_I,
            //                                              //Status:
            //                                              //      0 - Group added sucessfully
            //                                              //      1 - At leats one calculations is already group.
            //                                              //      2 - Calculations can not be group.
            //                                              //      3 - At least one calculation was not found.
            //                                              //      4 - Only one resource.
            //                                              //      5 - No calculations to group.
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Select at least two calculations to group.";
            strDevMessage_IO = "";
            if (
                //                                          //There are not more than one cal selected.
                arrintPk_I.Length > 1
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                int intI = 0;
                bool boolNotFound = false;
                bool boolCanNotBeGroup = false;
                bool boolCalAlreadyGroup = false;
                bool boolHasNotCost = false;
                bool boolIsByWf = false;

                //                                          //To easy code.
                ResResource resLast = ResResource.resFromDB(CalCalculation.calGetFromDb(arrintPk_I[0]).
                    intnPkResourceElementBelongsTo, false);

                //                                          //Get the first type.
                int intPkTypeLast = resLast.restypBelongsTo.intPk;

                //                                          //Get the first inherited.
                int? intnPkTemplateLast = null;
                if (
                    resLast.resinherited != null
                    )
                {
                    intnPkTemplateLast = resLast.resinherited.intPk;
                }

                /*UNTIL-DO*/
                while (!(
                    (intI >= arrintPk_I.Length) ||
                    boolNotFound ||
                    boolCanNotBeGroup ||
                    boolCalAlreadyGroup ||
                    boolHasNotCost ||
                    boolIsByWf
                    ))
                {
                    //                                  //Search if the cal exist.
                    CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(calentity =>
                        calentity.intPk == arrintPk_I[intI]);
                    boolNotFound = calentity == null;

                    if (
                        //                              //Cal exists.
                        !boolNotFound
                        )
                    {
                        //                              //Verify cal by wf.
                        if (
                            (calentity.intnPkWorkflow != null) &&
                            ((calentity.intnPkElementElementType != null) ||
                            (calentity.intnPkElementElement != null))
                            )
                        {
                            boolIsByWf = true;
                        }
                        //                                  //Check if the cal has a group.
                        CalCalculation cal = CalCalculation.calGetFromDb(calentity.intPk);
                        boolCalAlreadyGroup = cal.intnGroup != null;

                        //                                  //To easy code.
                        ResResource res = ResResource.resFromDB(cal.intnPkResourceElementBelongsTo, false);

                        //                                  //Chek if the cal has cost.
                        //                                  //Get the cost.                        
                        CostentityCostEntityDB costentity = res.costentityCurrent;
                        boolHasNotCost = costentity == null;

                        //                                  //Get the type for this.
                        int intPkType = res.restypBelongsTo.intPk;

                        //                                  //Get the inherited for this.
                        int? intnPkTemplate = null;
                        if (
                            res.resinherited != null
                            )
                        {
                            intnPkTemplate = res.resinherited.intPk;
                        }

                        boolCanNotBeGroup = (intPkTypeLast != intPkType) || (intnPkTemplateLast != intnPkTemplate);

                        intPkTypeLast = intPkType;
                        intnPkTemplateLast = intnPkTemplate;
                    }
                    intI = intI + 1;
                }

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "At least one calculation was not found.";
                if (
                    !boolNotFound
                    )
                {
                    intStatus_IO = 406;
                    strUserMessage_IO = "Calculation by Workflow cannot be grouped.";
                    strDevMessage_IO = "Calculation by Workflow cannot be grouped.";
                    if (
                        !boolIsByWf
                        )
                    {
                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "At least one calculation already has a group.";
                        if (
                            !boolCalAlreadyGroup
                            )
                        {
                            intStatus_IO = 404;
                            strUserMessage_IO = "The resources to group must be from the same type and template.";
                            strDevMessage_IO = "";
                            if (
                                !boolCanNotBeGroup
                                )
                            {
                                intStatus_IO = 405;
                                strUserMessage_IO = "The resources to group must have a cost specified.";
                                strDevMessage_IO = "";
                                if (
                                    !boolHasNotCost
                                    )
                                {
                                    int intMax = 0;
                                    if (
                                        context.GroupCalculation.Count() > 0
                                        )
                                    {
                                        intMax = context.GroupCalculation.Max(gpcalentity => gpcalentity.intId);
                                    }

                                    foreach (int intPk in arrintPk_I)
                                    {
                                        GpcalentityGroupCalculationEntityDB gpcalentity = new GpcalentityGroupCalculationEntityDB
                                        {
                                            intPkCalculation = intPk,
                                            intId = intMax + 1
                                        };
                                        context.GroupCalculation.Add(gpcalentity);
                                        context.SaveChanges();
                                    }

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUngroup(
            int intGroupId_I,
            out int intStatus_O
            )
        {
            intStatus_O = 1;

            Odyssey2Context context = new Odyssey2Context();
            //                                              //Search the group
            IQueryable<GpcalentityGroupCalculationEntityDB> setgpcalentity =
                context.GroupCalculation.Where(gpcalentity => gpcalentity.intId == intGroupId_I);
            List<GpcalentityGroupCalculationEntityDB> darrgpcalentity = setgpcalentity.ToList();
            if (
                darrgpcalentity.Count > 0
                )
            {
                //Remove all calculation from that group
                foreach (GpcalentityGroupCalculationEntityDB gpcalentity in darrgpcalentity)
                {
                    context.GroupCalculation.Remove(gpcalentity);
                    context.SaveChanges();
                }
                intStatus_O = 0;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddProcessTime(
            //                                              //Add time calculation to DB.

            int? intPkProduct_I,
            int? intnHours_I,
            int? intnMinutes_I,
            int? intnSeconds_I,
            String strUnit_I,
            double? numnQuantity_I,
            bool boolIsEnable_I,
            String strDescription_I,
            String strCalculationType_I,
            int? intnPkProcess_I,
            int? intnPkProcessInWorkflow_I,
            double? numnNeeded_I,
            double? numnPerUnits_I,
            String strByX_I,
            GpcondjsonGroupConditionJson gpcondition_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "It is not a calculation by process.";
            if (
                strByX_I == CalCalculation.strByProcess
                )
            {
                int? intnPkProduct = (intPkProduct_I > 0) ? intPkProduct_I : null;
                //                                          //Find product.
                ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(intnPkProduct);

                int? intnPkProcess = intnPkProcess_I;
                int? intnProcessInWorkflowId = null;
                int? intnPkWorkflow = null;

                PiwentityProcessInWorkflowEntityDB piwentity = null;
                WfentityWorkflowEntityDB wfentity = null;
                if (
                    //                                      //From Workflow.
                    intnPkProcessInWorkflow_I != null
                    )
                {
                    //                                      //Get process in workflow.
                    piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intnPkProcessInWorkflow_I);

                    if (
                        piwentity != null
                        )
                    {
                        intnProcessInWorkflowId = piwentity.intProcessInWorkflowId;
                        intnPkWorkflow = piwentity.intPkWorkflow;

                        wfentity = context_M.Workflow.FirstOrDefault(wf => wf.intPk == intnPkWorkflow);
                    }
                }

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Product not found or wf is not a wf base.";
                if (
                    //                                      //Product null because it belongs to wf base.
                    ((prodtyp == null) && (wfentity != null) && (wfentity.intnPkProduct == null)) ||
                    //                                      //Product found (it comes from a wf).
                    ((prodtyp != null) && (wfentity != null) && (wfentity.intnPkProduct != null)) ||
                    //                                      //It is a default calculation.
                    ((prodtyp == null) && (wfentity == null))
                    )
                {
                    //                                      //Transform time to 0 if null.
                    int intHours_I = intnHours_I == null ? 0 : (int)intnHours_I;
                    int intMinutes_I = intnMinutes_I == null ? 0 : (int)intnMinutes_I;
                    int intSeconds_I = intnSeconds_I == null ? 0 : (int)intnSeconds_I;

                    intStatus_IO = 403;
                    strUserMessage_IO = "Set correct values to time.";
                    strDevMessage_IO = "One time value migth be 0, below 0 or greater than 59.";
                    if (
                          intHours_I >= 0 &&
                          intMinutes_I >= 0 &&
                          intMinutes_I <= 59 &&
                          intSeconds_I >= 0 &&
                          intSeconds_I <= 59 &&
                          (intHours_I > 0 ||
                          intMinutes_I > 0 ||
                          intSeconds_I > 0)
                          )
                    {
                        intStatus_IO = 406;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Process not found.";
                        if (
                            //                              //Find process.
                            (intnPkProcess != null) &&
                            (ProProcess.proFromDB(intnPkProcess) != null)
                            )
                        {
                            intStatus_IO = 407;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Some values are wrong.";
                            if (
                                //                          //It is a base calculation.
                                (strCalculationType_I == CalCalculation.strBase) &&
                                (numnNeeded_I == null) &&
                                (numnQuantity_I == null) &&
                                (numnPerUnits_I == null) &&
                                (strUnit_I == null) &&
                                (strDescription_I != null)
                                )
                            {
                                intStatus_IO = 401;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Invalid CalculationAnd";
                                if (
                                    Tools.boolValidConditionList(gpcondition_I)
                                    )
                                {
                                    CalCalculation.subBaseTimeCalculation(null, intHours_I, intMinutes_I, intSeconds_I,
                                        intnPkProduct, intnPkProcess, intnPkWorkflow, intnProcessInWorkflowId,
                                        strDescription_I, strCalculationType_I, strByX_I, boolIsEnable_I,
                                        gpcondition_I, context_M, ref intStatus_IO, ref strUserMessage_IO,
                                        ref strDevMessage_IO);
                                }
                            }
                            else if (
                                //                          //It is a per quantity calculation.
                                (strCalculationType_I == CalCalculation.strPerQuantity) &&
                                (numnNeeded_I != null) &&
                                (numnQuantity_I != null) &&
                                (numnPerUnits_I != null) &&
                                (strUnit_I != null) &&
                                (strDescription_I != null)
                                )
                            {
                                //                          //Validate Unit not only have numbers.
                                bool boolIsParseableToInt = strUnit_I.IsParsableToInt();
                                bool boolIsParseableToNum = strUnit_I.IsParsableToNum();

                                intStatus_IO = 408;
                                strUserMessage_IO = "Unit of Measurement cannot start with a number.";
                                if (
                                    !boolIsParseableToInt &&
                                    !boolIsParseableToNum
                                    )
                                {
                                    intStatus_IO = 409;
                                    strUserMessage_IO = "Set correct value to Per Units, Quantity or Nedeed.";
                                    strDevMessage_IO = "One value is below 0 or is 0.";
                                    if (
                                        (numnPerUnits_I > 0) &&
                                        (numnQuantity_I > 0) &&
                                        (numnNeeded_I > 0)
                                        )
                                    {
                                        intStatus_IO = 401;
                                        strUserMessage_IO = "Something is wrong.";
                                        strDevMessage_IO = "Invalid CalculationAnd";
                                        if (
                                            Tools.boolValidConditionList(gpcondition_I)
                                            )
                                        {
                                            CalCalculation.subPerQuantityTimeCalculation(null, intHours_I, intMinutes_I,
                                                intSeconds_I, numnPerUnits_I, intnPkProduct, intnPkProcess,
                                                intnPkWorkflow, intnProcessInWorkflowId, numnQuantity_I, numnNeeded_I,
                                                strDescription_I, strCalculationType_I, strByX_I, strUnit_I,
                                                boolIsEnable_I, gpcondition_I, context_M, ref intStatus_IO,
                                                ref strUserMessage_IO, ref strDevMessage_IO);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subModifyProcessTime(

            CalCalculation calNew_I,
            GpcondjsonGroupConditionJson gpcondCondition_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Search if the cal exist.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(calentity =>
                calentity.intPk == calNew_I.intPk);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No calculation found.";
            if (
                //                                          //Cal found.
                calentity != null
                )
            {
                //                                          //Find the condition.
                ColcondentityCalculationOrLinkConditionEntityDB colcondentity = context.CalculationOrLinkCondition.
                    FirstOrDefault(colcond => colcond.intnPkCalculation == calentity.intPk &&
                    colcond.intnPkLinkNode == null);

                //                                      //Transform time to 0 if null.
                int intHoursNew = calNew_I.intnHours == null ? 0 : (int)calNew_I.intnHours;
                int intMinutesNew = calNew_I.intnMinutes == null ? 0 : (int)calNew_I.intnMinutes;
                int intSecondsNew = calNew_I.intnSeconds == null ? 0 : (int)calNew_I.intnSeconds;

                intStatus_IO = 402;
                strUserMessage_IO = "Set correct values to time.";
                strDevMessage_IO = "One time value migth be 0, below 0 or greater than 59.";
                if (
                      intHoursNew >= 0 &&
                      intMinutesNew >= 0 &&
                      intMinutesNew <= 59 &&
                      intSecondsNew >= 0 &&
                      intSecondsNew <= 59 &&
                      (intHoursNew > 0 ||
                      intMinutesNew > 0 ||
                      intSecondsNew > 0)
                      )
                {
                    int? intnPkProcess = calentity.intnPkProcess;
                    int? intnProcessInWorkflowId = calentity.intnProcessInWorkflowId;
                    int? intnPkWorkflow = calentity.intnPkWorkflow;
                    int? intnPkProduct = calentity.intnPkProduct;

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Process not found.";
                    if (
                        //                              //Find process.
                        (intnPkProcess != null) &&
                        (ProProcess.proFromDB(intnPkProcess) != null)
                        )
                    {
                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Some values are wrong.";
                        if (
                            //                          //It is a base calculation.
                            (calentity.strCalculationType == CalCalculation.strBase) &&
                            (calentity.numnNeeded == null) &&
                            (calentity.numnQuantity == null) &&
                            (calentity.numnPerUnits == null) &&
                            (calentity.strUnit == null) &&
                            (calNew_I.strDescription != null)
                            )
                        {
                            int intPkCalculation = calentity.intPk;
                            bool boolCopied = CalCalculation.boolCalculationNeedsToBeCopied(calentity.intnPkProduct,
                                    calentity.intnPkWorkflow, calentity.strStartDate, calentity.strStartTime,
                                    calentity.strByX);
                            if (
                                boolCopied
                                )
                            {
                                //                          //Set end date and time for the calculation.
                                calentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                                calentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                                context.Calculation.Update(calentity);

                                if (
                                    !calNew_I.boolIsEnable
                                    )
                                {
                                    List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                                        per.intnPkCalculation == calentity.intPk).ToList();

                                    foreach (PerentityPeriodEntityDB perentity in darrperentity)
                                    {
                                        JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                                            job.intJobID == perentity.intJobId);
                                        if (
                                            jobentity == null
                                            )
                                        {
                                            context.Period.Remove(perentity);
                                        }
                                    }
                                    context.SaveChanges();
                                }

                                CalCalculation.subBaseTimeCalculation(calentity, intHoursNew, intMinutesNew, intSecondsNew,
                                intnPkProduct, intnPkProcess, intnPkWorkflow, intnProcessInWorkflowId,
                                calNew_I.strDescription, calentity.strCalculationType, calentity.strByX,
                                calNew_I.boolIsEnable, gpcondCondition_I, context, ref intStatus_IO, 
                                ref strUserMessage_IO, ref strDevMessage_IO);
                            }
                            else
                            {
                                //                      //calculation is not used by a job in progress/completed.
                                calentity.intnHours = intHoursNew;
                                calentity.intnMinutes = intMinutesNew;
                                calentity.intnSeconds = intSecondsNew;
                                calentity.boolIsEnable = calNew_I.boolIsEnable;
                                calentity.strDescription = calNew_I.strDescription;
                                calentity.strCalculationType = calNew_I.strCalculationType;
                                calentity.strByX = calNew_I.strByX;

                                context.SaveChanges();

                                if (
                                    !calNew_I.boolIsEnable
                                    )
                                {
                                    List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                                        per.intnPkCalculation == calentity.intPk).ToList();

                                    foreach (PerentityPeriodEntityDB perentity in darrperentity)
                                    {
                                        context.Period.Remove(perentity);
                                    }
                                    context.SaveChanges();
                                }

                                if (
                                    gpcondCondition_I != null
                                    )
                                {
                                    //                              //Delete the condition and add the new condition.
                                    Tools.subDeleteCondition(calentity.intPk, null, null, null, context);

                                    Tools.subAddCondition(intPkCalculation, null, null, null, gpcondCondition_I, context);
                                }
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                        else if (
                            //                          //It is a per quantity calculation.
                            (calentity.strCalculationType == CalCalculation.strPerQuantity) &&
                            (calNew_I.numnNeeded != null) &&
                            (calNew_I.numnQuantity != null) &&
                            (calNew_I.numnPerUnits != null) &&
                            (calNew_I.strUnit != null) &&
                            (calNew_I.strDescription != null)
                            )
                        {
                            //                          //Validate Unit not only have numbers.
                            bool boolIsParseableToInt = calNew_I.strUnit.IsParsableToInt();
                            bool boolIsParseableToNum = calNew_I.strUnit.IsParsableToNum();

                            intStatus_IO = 405;
                            strUserMessage_IO = "Unit of Measurement cannot start with a number.";
                            if (
                                !boolIsParseableToInt &&
                                !boolIsParseableToNum
                                )
                            {
                                intStatus_IO = 406;
                                strUserMessage_IO = "Set correct value to Per Units, Quantity or Nedeed.";
                                strDevMessage_IO = "One value is below 0 or is 0.";
                                if (
                                    (calNew_I.numnPerUnits > 0) &&
                                    (calNew_I.numnQuantity > 0) &&
                                    (calNew_I.numnNeeded > 0)
                                    )
                                {
                                    if (
                                        CalCalculation.boolCalculationNeedsToBeCopied(calentity.intnPkProduct,
                                            calentity.intnPkWorkflow, calentity.strStartDate, calentity.strStartTime,
                                            calentity.strByX)
                                    )
                                    {
                                        //              //Set end date and time for the calculation.
                                        calentity.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                                        calentity.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                                        context.Calculation.Update(calentity);

                                        if (
                                            !calNew_I.boolIsEnable
                                            )
                                        {
                                            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                                                per.intnPkCalculation == calentity.intPk).ToList();
                                            foreach (PerentityPeriodEntityDB perentity in darrperentity)
                                            {
                                                JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                                                    job.intJobID == perentity.intJobId);
                                                if (
                                                    jobentity == null
                                                    )
                                                {
                                                    context.Period.Remove(perentity);
                                                }
                                            }
                                            context.SaveChanges();
                                        }

                                        CalCalculation.subPerQuantityTimeCalculation(calentity, intHoursNew, intMinutesNew,
                                        intSecondsNew, calNew_I.numnPerUnits, intnPkProduct, intnPkProcess,
                                        intnPkWorkflow, intnProcessInWorkflowId, calNew_I.numnQuantity,
                                        calNew_I.numnNeeded, calNew_I.strDescription, calentity.strCalculationType,
                                        calentity.strByX, calNew_I.strUnit, calNew_I.boolIsEnable, gpcondCondition_I, 
                                        context, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                                    }
                                    else
                                    {
                                        //              //calculation is not used by a job in progress/completed.

                                        calentity.intnHours = intHoursNew;
                                        calentity.intnMinutes = intMinutesNew;
                                        calentity.intnSeconds = intSecondsNew;
                                        calentity.numnPerUnits = calNew_I.numnPerUnits;
                                        calentity.numnQuantity = calNew_I.numnQuantity;
                                        calentity.numnNeeded = calNew_I.numnNeeded;
                                        calentity.strDescription = calNew_I.strDescription;
                                        calentity.strUnit = calNew_I.strUnit;
                                        calentity.boolIsEnable = calNew_I.boolIsEnable;

                                        if (
                                            !calNew_I.boolIsEnable
                                            )
                                        {
                                            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                                                per.intnPkCalculation == calentity.intPk).ToList();

                                            foreach (PerentityPeriodEntityDB perentity in darrperentity)
                                            {
                                                context.Period.Remove(perentity);
                                            }
                                            context.SaveChanges();
                                        }

                                        if (
                                            gpcondCondition_I != null
                                            )
                                        {
                                            //                              //Delete the condition and add the new condition.
                                            Tools.subDeleteCondition(calentity.intPk, null, null, null, context);

                                            Tools.subAddCondition(calentity.intPk, null, null, null, gpcondCondition_I, context);
                                        }
                                    }
                                    context.SaveChanges();

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subBaseTimeCalculation(
            CalentityCalculationEntityDB calentity_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            int? intPkProduct_I,
            int? intnPkProcess_I,
            int? intnPkWorkflow_I,
            int? intnProcessInWorkflowId_I,
            String strDescription_I,
            String strCalculationType_I,
            String strByX_I,
            bool boolIsEnable_I,
            GpcondjsonGroupConditionJson gpcondition_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                          //Create entity.
            CalentityCalculationEntityDB calentityToAdd = new CalentityCalculationEntityDB
            {
                intnHours = intHours_I,
                intnMinutes = intMinutes_I,
                intnSeconds = intSeconds_I,
                boolIsEnable = boolIsEnable_I,
                strDescription = strDescription_I,
                strCalculationType = strCalculationType_I,
                strByX = strByX_I,
                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                intnPkProduct = intPkProduct_I,
                intnPkProcess = intnPkProcess_I,
                intnPkWorkflow = intnPkWorkflow_I,
                intnProcessInWorkflowId = intnProcessInWorkflowId_I
            };
            //                                          //Add entity to DB.
            context_M.Calculation.Add(calentityToAdd);
            context_M.SaveChanges();

            if (
                gpcondition_I == null
                )
            {
                if (
                   calentity_I != null
                   )
                {
                    //                                  //Get calculation condition.
                    gpcondition_I =
                        Tools.gpcondjsonGetCondition(calentity_I.intPk, null, null, null);
                }
            }

            //                                          //Add conditions to necessary tables.
            Tools.subAddCondition(calentityToAdd.intPk, null, null, null, gpcondition_I, context_M);

            intStatus_IO = 200;
            strUserMessage_IO = "Success.";
            strDevMessage_IO = "";

        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subPerQuantityTimeCalculation(

            CalentityCalculationEntityDB calentity_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            double? numnPerUnits_I,
            int? intPkProduct_I,
            int? intnPkProcess_I,
            int? intnPkWorkflow_I,
            int? intnProcessInWorkflowId_I,
            double? numnQuantity_I,
            double? numnNeeded_I,
            String strDescription_I,
            String strCalculationType_I,
            String strByX_I,
            String strUnit_I,
            bool boolIsEnable_I,
            GpcondjsonGroupConditionJson gpcondition_I,
             Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                          //Create entity.
            CalentityCalculationEntityDB calentityToAdd = new CalentityCalculationEntityDB
            {
                strUnit = strUnit_I,
                numnQuantity = numnQuantity_I,
                numnNeeded = numnNeeded_I,
                numnPerUnits = numnPerUnits_I,
                intnHours = intHours_I,
                intnMinutes = intMinutes_I,
                intnSeconds = intSeconds_I,
                boolIsEnable = boolIsEnable_I,
                strDescription = strDescription_I,
                strCalculationType = strCalculationType_I,
                strByX = strByX_I,
                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                intnPkProduct = intPkProduct_I,
                intnPkProcess = intnPkProcess_I,
                intnPkWorkflow = intnPkWorkflow_I,
                intnProcessInWorkflowId = intnProcessInWorkflowId_I
            };
            //                                          //Add entity to DB.
            context_M.Calculation.Add(calentityToAdd);
            context_M.SaveChanges();

            if (
                gpcondition_I == null
                )
            {
                if (
                   calentity_I != null
                   )
                {
                    //                                  //Get calculation condition.
                    gpcondition_I =
                        Tools.gpcondjsonGetCondition(calentity_I.intPk, null, null, null);
                }
            }

            //                                          //Add conditions to necessary tables.
            Tools.subAddCondition(calentityToAdd.intPk, null, null, null, gpcondition_I, context_M);

            intStatus_IO = 200;
            strUserMessage_IO = "Success.";
            strDevMessage_IO = "";

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetTransform(
            //                                              //Method set a transformation calculation in the DB.

            //                                              //Data to add.
            int? intnPk_I,
            int intPkProcessInWorkflow_I,
            int intPkEleetOrEleeleI_I,
            bool boolIsEleetI_I,
            int intPkEleetOrEleeleO_I,
            bool boolIsEleetO_I,
            double numNeeded_I,
            double numPerUnit_I,
            int intPkResourceI_I,
            int intPkResourceO_I,
            GpcondjsonGroupConditionJson gpcondjson_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref string strUserMessage_IO,
            ref string strDevMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //To easy code.
            int? intnPkElementElementTypeI = null;
            int? intnPkElementElementI = intPkEleetOrEleeleI_I;
            if (
                boolIsEleetI_I == true
                )
            {
                intnPkElementElementTypeI = intPkEleetOrEleeleI_I;
                intnPkElementElementI = null;
            }

            bool boolIsOutput = false;
            int? intnPkElementElementTypeO = null;
            int? intnPkElementElementO = null;
            if (
                boolIsEleetO_I == true
                )
            {
                intnPkElementElementTypeO = intPkEleetOrEleeleO_I;
                boolIsOutput = !(context.ElementElementType.FirstOrDefault(eleet =>
                eleet.intPk == intPkEleetOrEleeleO_I).boolUsage);
            }
            else
            {
                intnPkElementElementO = intPkEleetOrEleeleO_I;
                boolIsOutput = !(context.ElementElement.FirstOrDefault(eleet =>
                eleet.intPk == intPkEleetOrEleeleO_I).boolUsage);
            }

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "EleetOrEleeleO needs to be an output.";
            if (
                boolIsOutput
                )
            {
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Piw not found.";
                if (
                    piwentity != null
                    )
                {
                    IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                        io.intPkWorkflow == piwentity.intPkWorkflow &&
                        io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                        io.intnPkElementElementType == intnPkElementElementTypeO &&
                        io.intnPkElementElement == intnPkElementElementO);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Piw needs to be post process or normal but io needs to have not link.";
                    if (
                        //                                  //Piw is a post process.
                        (piwentity.boolIsPostProcess) ||
                        //                                  //Piw is normal but io has not link.
                        ((ioentity == null) || (ioentity.strLink == null))
                        )
                    {
                        if (
                            //                              //The resource to set is valid for the io.
                            (ProdtypProductType.boolDataValid(piwentity.intPkWorkflow, piwentity.intProcessInWorkflowId,
                                intPkResourceI_I, intPkEleetOrEleeleI_I, boolIsEleetI_I, ref intStatus_IO, 
                                ref strUserMessage_IO, ref strDevMessage_IO)) && 
                                (ProdtypProductType.boolDataValid(piwentity.intPkWorkflow, 
                                piwentity.intProcessInWorkflowId, intPkResourceO_I, intPkEleetOrEleeleO_I, 
                                boolIsEleetO_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO))
                            )
                        {
                            intStatus_IO = 404;
                            strUserMessage_IO = "The condition is invalid. Please verify it.";
                            strDevMessage_IO = "";
                            if (
                                Tools.boolValidConditionList(gpcondjson_I)
                               )
                            {
                                TrfcalentityTransformCalculationEntityDB trfcalentity = context.TransformCalculation
                                    .FirstOrDefault(trfcal => trfcal.intPk == intnPk_I);
                                if (
                                    //                      //Update transform calculation
                                    trfcalentity != null
                                    )
                                {
                                    int intPkWorkflow = context.ProcessInWorkflow.FirstOrDefault(piw => 
                                    piw.intPk == trfcalentity.intPkProcessInWorkflow).intPkWorkflow;

                                    if (
                                        CalCalculation.boolTransformCalculationNeedsToBeCopied(intPkWorkflow,
                                            trfcalentity.strStartDate, trfcalentity.strStartTime)
                                        )
                                    {
                                        //                  //Create story point.
                                        //                  //Set end date and end time.
                                        trfcalentity.strEndDate = ZonedTimeTools.ztimeNow.Date.ToString();
                                        trfcalentity.strEndTime = ZonedTimeTools.ztimeNow.Time.ToString();

                                        context.TransformCalculation.Update(trfcalentity);

                                        //                  //Add calculation edited to db.
                                        TrfcalentityTransformCalculationEntityDB trfcalentityToAdd =
                                            new TrfcalentityTransformCalculationEntityDB
                                            {
                                                intPkProcessInWorkflow = intPkProcessInWorkflow_I,
                                                numNeeded = numNeeded_I,
                                                numPerUnits = numPerUnit_I,
                                                intPkResourceI = intPkResourceI_I,
                                                intPkResourceO = intPkResourceO_I,
                                                strStartDate = ZonedTimeTools.ztimeNow.Date.ToString(),
                                                strStartTime = ZonedTimeTools.ztimeNow.Time.ToString(),
                                                intnPkElementElementTypeI = intnPkElementElementTypeI,
                                                intnPkElementElementI = intnPkElementElementI,
                                                intnPkElementElementTypeO = intnPkElementElementTypeO,
                                                intnPkElementElementO = intnPkElementElementO
                                            };

                                        context.TransformCalculation.Add(trfcalentityToAdd);

                                        //                  //Add the given condition to the new transform calculation.
                                        Tools.subAddCondition(null, null, null, trfcalentityToAdd.intPk, gpcondjson_I, 
                                            context);
                                    }
                                    else
                                    {
                                        trfcalentity.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
                                        trfcalentity.numNeeded = numNeeded_I;
                                        trfcalentity.numPerUnits = numPerUnit_I;
                                        trfcalentity.intPkResourceI = intPkResourceI_I;
                                        trfcalentity.intPkResourceO = intPkResourceO_I;
                                        trfcalentity.strStartDate = ZonedTimeTools.ztimeNow.Date.ToString();
                                        trfcalentity.strStartTime = ZonedTimeTools.ztimeNow.Time.ToString();
                                        trfcalentity.intnPkElementElementTypeI = intnPkElementElementTypeI;
                                        trfcalentity.intnPkElementElementI = intnPkElementElementI;
                                        trfcalentity.intnPkElementElementTypeO = intnPkElementElementTypeO;
                                        trfcalentity.intnPkElementElementO = intnPkElementElementO;

                                        context.TransformCalculation.Update(trfcalentity);

                                        //                  //Delete the last condition.
                                        Tools.subDeleteCondition(null, null, null, trfcalentity.intPk, context_M);

                                        //                  //Add the new condition.
                                        Tools.subAddCondition(null, null, null, trfcalentity.intPk, gpcondjson_I, 
                                            context_M);
                                    }

                                    context.SaveChanges();

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "";
                                    strDevMessage_IO = "";
                                }
                                else
                                {
                                    //                          //Add new transform calculation

                                    trfcalentity = new TrfcalentityTransformCalculationEntityDB
                                    {
                                        intPkProcessInWorkflow = intPkProcessInWorkflow_I,
                                        numNeeded = numNeeded_I,
                                        numPerUnits = numPerUnit_I,
                                        intPkResourceI = intPkResourceI_I,
                                        intPkResourceO = intPkResourceO_I,
                                        strStartDate = ZonedTimeTools.ztimeNow.Date.ToString(),
                                        strStartTime = ZonedTimeTools.ztimeNow.Time.ToString(),
                                        intnPkElementElementTypeI = intnPkElementElementTypeI,
                                        intnPkElementElementI = intnPkElementElementI,
                                        intnPkElementElementTypeO = intnPkElementElementTypeO,
                                        intnPkElementElementO = intnPkElementElementO
                                    };

                                    context.TransformCalculation.Add(trfcalentity);
                                    context.SaveChanges();

                                    //                      //Add the condition.
                                    Tools.subAddCondition(null, null, null, trfcalentity.intPk, gpcondjson_I, 
                                        context_M);

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
        private static bool boolTransformCalculationNeedsToBeCopied(
            //                                              //Returns true if a calculation is in a inprogress job or 
            //                                              //      completed.

            int intPkWorkflow_I,
            String strStartDate_I,
            String strStartTime_I
            )
        {
            bool boolCalNeedsToBeCopied = false;

            //                                              //Calculation start time.
            ZonedTime ztimecalStartDate = ZonedTimeTools.NewZonedTime(strStartDate_I.ParseToDate(),
                strStartTime_I.ParseToTime());

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();
            //                                              //Find workflow.
            WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                wf.intPk == intPkWorkflow_I && wf.boolDeleted == false);

            if (
                wfentity != null
                )
            {
                //                                          //Find workflow´s jobs.
                IQueryable<JobentityJobEntityDB> setjobentity = context.Job.Where(job =>
                    job.intPkWorkflow == wfentity.intPk);
                List<JobentityJobEntityDB> darrjobentity = setjobentity.ToList();

                //                                          //Iterate over each job.
                int intK = 0;
                /*WHILE-DO*/
                while (
                     intK < darrjobentity.Count &&
                    !boolCalNeedsToBeCopied
                    )
                {
                    //                                      //Job start date.
                    ZonedTime ztimejobStartDate = ZonedTimeTools.NewZonedTime(darrjobentity
                        [intK].strStartDate.ParseToDate(), darrjobentity[intK].strStartTime.ParseToTime());

                    if (
                        //                                  //Job started after cal´s start date.
                        ztimejobStartDate >= ztimecalStartDate
                    )
                    {
                        boolCalNeedsToBeCopied = true;
                    }

                    intK = intK + 1;
                }
            }

            return boolCalNeedsToBeCopied;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteTransform(
            //                                              //Delete one transform calculation.

            int intPk_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Get transform calculation.
            TrfcalentityTransformCalculationEntityDB trfcalentityToDelete = context_M.TransformCalculation.
                FirstOrDefault(trf => trf.intPk == intPk_I);

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Transform calculation does not exist.";
            if (
                //                                          //Transform calculation exists.
                trfcalentityToDelete != null
                )
            {
                intStatus_IO = 403;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "There is a history point.";
                if (
                    //                                      //There isn't a history point.
                    trfcalentityToDelete.strEndDate == null &&
                    trfcalentityToDelete.strEndTime == null
                    )
                {
                    //                                      //Get the workflow.
                    int intPkWorkflow = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == trfcalentityToDelete.intPkProcessInWorkflow).intPkWorkflow;

                    if (
                        CalCalculation.boolTransformCalculationNeedsToBeCopied(intPkWorkflow,
                            trfcalentityToDelete.strStartDate, trfcalentityToDelete.strStartTime)
                        )
                    {
                        //                                  //Create a history point.
                        //                                  //Set end date and end time.
                        trfcalentityToDelete.strEndDate = ZonedTimeTools.ztimeNow.Date.ToString();
                        trfcalentityToDelete.strEndTime = ZonedTimeTools.ztimeNow.Time.ToString();

                        context_M.TransformCalculation.Update(trfcalentityToDelete);
                    }
                    else
                    {
                        Tools.subDeleteCondition(null, null, null, trfcalentityToDelete.intPk, context_M);
                        //                                  //Remove the transform calculation.
                        context_M.TransformCalculation.Remove(trfcalentityToDelete);
                    }

                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeletePaperTransformation(
            //                                              //Finds and deletes an specific paper transformation 
            //                                              //      from DB.
            //                                              //If there is a temporary paper transformation previously
            //                                              //      propagated, it is deleted.

            int intPkPaTrans_I,
            int? intnPkCalculation_I,
            bool boolFromClose_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Main Pk is 0 or a negative number.";
            if (
                intPkPaTrans_I > 0
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find paper trans.
                PatransPaperTransformationEntityDB patransentity = context.PaperTransformation.FirstOrDefault(paper =>
                    paper.intPk == intPkPaTrans_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Paper transformation not found.";
                if (
                    patransentity != null
                    )
                {
                    //                                      //Find piw.
                    PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                        piw => piw.intPk == patransentity.intPkProcessInWorkflow);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Process in workflow not found.";
                    if (
                        piwentity != null
                        )
                    {
                        List<PatransPaperTransformationEntityDB> darrpatransentityToDelete =
                           new List<PatransPaperTransformationEntityDB>();
                        if (
                            patransentity.boolTemporary &&
                            //                              //It is from modal and printer confirm (yes) to delete
                            //                              //all the paper tranformation for that IO.
                            !boolFromClose_I
                            )
                        {
                            //                              //Find no temporary if exist.
                            PatransPaperTransformationEntityDB patransentityNoTemporary =
                                context.PaperTransformation.FirstOrDefault(paper =>
                                paper.intnPkElementElementTypeI == patransentity.intnPkElementElementTypeI &&
                                paper.intnPkElementElementI == patransentity.intnPkElementElementI &&
                                paper.intPkProcessInWorkflow == patransentity.intPkProcessInWorkflow &&
                                paper.intPkResourceI == patransentity.intPkResourceI &&
                                paper.intnPkCalculationOwn == intnPkCalculation_I);
                            if (
                                patransentityNoTemporary != null
                                )
                            {
                                darrpatransentityToDelete.Add(patransentityNoTemporary);
                            }
                        }
                        else
                        {
                            darrpatransentityToDelete.Add(patransentity);
                        }

                        //                                  //Find temporary paper trans in the input io.
                        List<PatransPaperTransformationEntityDB> darrpatransentityTemporary =
                            context.PaperTransformation.Where(paper =>
                            paper.intnPkElementElementTypeI == patransentity.intnPkElementElementTypeI &&
                            paper.intnPkElementElementI == patransentity.intnPkElementElementI &&
                            paper.intPkProcessInWorkflow == patransentity.intPkProcessInWorkflow &&
                            paper.intPkResourceI == patransentity.intPkResourceI &&
                            paper.intnPkCalculationOwn == null &&
                            paper.intnPkCalculationLink == null &&
                            paper.boolTemporary).ToList();

                        darrpatransentityToDelete.AddRange(darrpatransentityTemporary);

                        foreach (PatransPaperTransformationEntityDB patransentityTemp in darrpatransentityToDelete)
                        {
                            //                              //Delete temporary ones.
                            context.PaperTransformation.Remove(patransentityTemp);
                        }

                        //                                  //Find IO output that might contain a link.
                        IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                            io.intnPkElementElementType == patransentity.intnPkElementElementTypeO &&
                            io.intnPkElementElement == patransentity.intnPkElementElementO &&
                            io.intPkWorkflow == piwentity.intPkWorkflow &&
                            io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                            io.intnPkResource == patransentity.intnPkResourceO);

                        if (
                            ioentity != null &&
                            //                              //There is a link.
                            ioentity.strLink != null
                            )
                        {
                            //                              //Get other(s) side of the link.
                            List<IoentityInputsAndOutputsEntityDB> darrioentity =
                                CalCalculation.darrioentityVerifyIfIOHasLink(piwentity,
                                patransentity.intnPkElementElementTypeO, patransentity.intnPkElementElementO);

                            foreach (IoentityInputsAndOutputsEntityDB ioentityOtherSide in darrioentity)
                            {
                                //                          //Find piw.
                                PiwentityProcessInWorkflowEntityDB piwentityOtherSide =
                                    context.ProcessInWorkflow.FirstOrDefault(piw =>
                                    piw.intPkWorkflow == ioentityOtherSide.intPkWorkflow &&
                                    piw.intProcessInWorkflowId == ioentityOtherSide.intnProcessInWorkflowId);

                                if (
                                    piwentityOtherSide != null
                                    )
                                {
                                    List<PatransPaperTransformationEntityDB> darrpatransentityOthersideToDelete =
                                        new List<PatransPaperTransformationEntityDB>();

                                    /*CASE*/
                                    if (
                                        //                  //Delete temporaries.
                                        patransentity.boolTemporary &&
                                        intnPkCalculation_I == null
                                        )
                                    {
                                        //                  //Find otherside's paper transformations that was 
                                        //                  //      propagated.
                                        List<PatransPaperTransformationEntityDB> darrpatransentityOthersideTemporary =
                                            context.PaperTransformation.Where(patrans =>
                                            patrans.intnPkElementElementTypeI ==
                                            ioentityOtherSide.intnPkElementElementType &&
                                            patrans.intnPkElementElementI == ioentityOtherSide.intnPkElementElement &&
                                            patrans.intnPkCalculationOwn == null &&
                                            patrans.intPkProcessInWorkflow == piwentityOtherSide.intPk &&
                                            patrans.intPkResourceI == patransentity.intnPkResourceO &&
                                            patrans.intnPkCalculationLink == null
                                            ).ToList();

                                        darrpatransentityOthersideToDelete.AddRange(
                                            darrpatransentityOthersideTemporary);
                                    }
                                    else if (
                                        //                  //Delete no temporary.
                                        !patransentity.boolTemporary &&
                                        intnPkCalculation_I != null
                                        )
                                    {
                                        PatransPaperTransformationEntityDB patransentityOthersideToDelete =
                                            context.PaperTransformation.FirstOrDefault(patrans =>
                                            patrans.intnPkElementElementTypeI ==
                                            ioentityOtherSide.intnPkElementElementType &&
                                            patrans.intnPkElementElementI == ioentityOtherSide.intnPkElementElement &&
                                            patrans.intnPkCalculationOwn == null &&
                                            patrans.intPkProcessInWorkflow == piwentityOtherSide.intPk &&
                                            patrans.intPkResourceI == patransentity.intnPkResourceO &&
                                            patrans.intnPkCalculationLink == intnPkCalculation_I);


                                        if (
                                            patransentityOthersideToDelete != null
                                            )
                                        {
                                            darrpatransentityOthersideToDelete.Add(patransentityOthersideToDelete);
                                        }
                                    }
                                    else if (
                                        //                  //Close. Se arrepintio de lo que mofico.
                                        //                  //Delete no temporary and temporaries.
                                        patransentity.boolTemporary &&
                                        intnPkCalculation_I != null
                                        //                  //
                                        )
                                    {
                                        if (
                                            //              //It is from modal and printer confirm (yes) to delete
                                            //              //all the paper tranformation for that IO.
                                            !boolFromClose_I
                                            )
                                        {
                                            PatransPaperTransformationEntityDB patransentityOthersideToDelete =
                                                context.PaperTransformation.FirstOrDefault(patrans =>
                                                patrans.intnPkElementElementTypeI ==
                                                ioentityOtherSide.intnPkElementElementType &&
                                                patrans.intnPkElementElementI ==
                                                ioentityOtherSide.intnPkElementElement &&
                                                patrans.intnPkCalculationOwn == null &&
                                                patrans.intPkProcessInWorkflow == piwentityOtherSide.intPk &&
                                                patrans.intPkResourceI == patransentity.intnPkResourceO &&
                                                patrans.intnPkCalculationLink == intnPkCalculation_I);

                                            if (
                                                patransentityOthersideToDelete != null
                                                )
                                            {
                                                darrpatransentityOthersideToDelete.Add(patransentityOthersideToDelete);
                                            }
                                        }
                                        List<PatransPaperTransformationEntityDB> darrpatransentityOthersideTemporary =
                                            context.PaperTransformation.Where(patrans =>
                                            patrans.intnPkElementElementTypeI ==
                                            ioentityOtherSide.intnPkElementElementType &&
                                            patrans.intnPkElementElementI == ioentityOtherSide.intnPkElementElement &&
                                            patrans.intnPkCalculationOwn == null &&
                                            patrans.intPkProcessInWorkflow == piwentityOtherSide.intPk &&
                                            patrans.intPkResourceI == patransentity.intnPkResourceO &&
                                            patrans.intnPkCalculationLink == null
                                            ).ToList();

                                        darrpatransentityOthersideToDelete.AddRange(
                                            darrpatransentityOthersideTemporary);
                                    }

                                    foreach (PatransPaperTransformationEntityDB patransentityToDelete in
                                        darrpatransentityOthersideToDelete)
                                    {
                                        context.PaperTransformation.Remove(patransentityToDelete);
                                    }
                                }
                            }
                        }

                        context.SaveChanges();

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSavePaperTransformation(
            //                                              //Save a temporary paper transformation by creating a 
            //                                              //      register in paperTransformation table.

            int? intnPkPaTrans_I,
            int? intnPkCalculation_I,
            double numWidth_I,
            double? numnHeigth_I,
            double numCutWidth_I,
            double numCutHeight_I,
            bool boolCut_I,
            double? numnMarginTop_I,
            double? numnMarginBottom_I,
            double? numnMarginLeft_I,
            double? numnMarginRight_I,
            double? numnVerticalGap_I,
            double? numnHorizontalGap_I,
            String strUnit_I,
            int intPkEleetOrEleeleI_I,
            bool boolIsEleetI_I,
            int intPkResourceI_I,
            int intPkEleetOrEleeleO_I,
            bool boolIsEleetO_I,
            int intPkResourceO_I,
            int intPkProcessInWorkflow_I,
            bool boolIsOptimized_I,
            out Patransjson2PaperTransformationJson2 patransjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {            
            patransjson_O = null;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Paper dimensions, Margins and gaps should not be less than 0.";
            if (
                 ((numnMarginTop_I != null && numnMarginTop_I >= 0) || numnMarginTop_I == null) &&
                 ((numnMarginBottom_I != null && numnMarginBottom_I >= 0) || numnMarginBottom_I == null) &&
                 ((numnMarginLeft_I != null && numnMarginLeft_I >= 0) || numnMarginLeft_I == null) &&
                 ((numnMarginRight_I != null && numnMarginRight_I >= 0) || numnMarginRight_I == null) &&
                 ((numnVerticalGap_I != null && numnVerticalGap_I >= 0) || numnVerticalGap_I == null) &&
                 ((numnHorizontalGap_I != null && numnHorizontalGap_I >= 0) || numnHorizontalGap_I == null)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Validate Workflow.
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Process in workflow not valid.";
                if (
                    piwentity != null
                    )
                {
                    //                                      //To easy code.
                    int? intnPkElementElementTypeI = boolIsEleetI_I ? intPkEleetOrEleeleI_I : (int?)null;
                    int? intnPkElementElementI = !boolIsEleetI_I ? intPkEleetOrEleeleI_I : (int?)null;
                    int? intnPkElementElementTypeO = boolIsEleetO_I ? intPkEleetOrEleeleO_I : (int?)null;
                    int? intnPkElementElementO = !boolIsEleetO_I ? intPkEleetOrEleeleO_I : (int?)null;

                    //                                      //Validate IOs.
                    EleetentityElementElementTypeEntityDB eleetentityI = null;
                    EleeleentityElementElementEntityDB eleeleentityI = null;
                    if (
                        intnPkElementElementTypeI != null
                        )
                    {
                        eleetentityI = context.ElementElementType.FirstOrDefault(eleet =>
                            eleet.intPk == intnPkElementElementTypeI && eleet.boolUsage == true);
                    }
                    else
                    {
                        eleeleentityI = context.ElementElement.FirstOrDefault(ele =>
                            ele.intPk == intnPkElementElementI && ele.boolUsage == true);
                    }

                    EleetentityElementElementTypeEntityDB eleetentityO = null;
                    EleeleentityElementElementEntityDB eleeleentityO = null;
                    if (
                        intnPkElementElementTypeO != null
                        )
                    {
                        eleetentityO = context.ElementElementType.FirstOrDefault(eleet =>
                            eleet.intPk == intnPkElementElementTypeO && eleet.boolUsage == false);
                    }
                    else
                    {
                        eleeleentityO = context.ElementElement.FirstOrDefault(ele =>
                            ele.intPk == intnPkElementElementO && ele.boolUsage == false);
                    }

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The IO is not valid.";
                    if (
                        //                                  //It is a Type or Template input IO and is used as input.
                        (eleetentityI != null || eleeleentityI != null) &&
                        //                                  //It is a Type or Template output IO and is used as Output.
                        (eleetentityO != null || eleeleentityO != null)
                        )
                    {
                        //                                  //Resource belongs to IO (input side).
                        //                                  //Validate the resource.

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Resource not valid.";
                        if (
                            (CalCalculation.boolResourceInIOIsValid(piwentity, intPkResourceI_I,
                                intnPkElementElementTypeI, intnPkElementElementI)) &&
                            (CalCalculation.boolResourceInIOIsValid(piwentity, intPkResourceO_I,
                                intnPkElementElementTypeO, intnPkElementElementO))
                            )
                        {
                            //                              //Get resResource.
                            ResResource resResource = ResResource.resFromDB(intPkResourceI_I, false);
                            bool boolIsRoll = resResource.boolMediaRoll();

                            intStatus_IO = 405;
                            strUserMessage_IO = resResource.boolMediaRoll() ? "For a roll media resource length has to" +
                                " be null" : "For a sheet media resource length has to be greather than 0";
                            strDevMessage_IO = "Resource not valid.";
                            if (
                                //                          //If media resource is roll type, heigthI prop must be null
                                //                          //      otherwise, heigth have to be different to null.
                                (boolIsRoll && numnHeigth_I == null) ||
                                (!boolIsRoll && (numnHeigth_I != null && numnHeigth_I > 0))
                                )
                            {
                                intStatus_IO = 405;
                                strUserMessage_IO = "For Media Roll you cannot add Fold-PaperTransformation.";
                                strDevMessage_IO = "Resource not valid.";
                                if (
                                    boolIsRoll && boolCut_I 
                                    ||
                                    !boolIsRoll
                                    )
                                {
                                    //                              //To know if the IO input is a postSize IO.
                                    bool boolIsIOInputPostSize = CalCalculation.boolIsPostSize(intPkEleetOrEleeleI_I,
                                    boolIsEleetI_I, piwentity, null, null);

                                    if (
                                        boolIsIOInputPostSize
                                        )
                                    {
                                        numWidth_I = 0;
                                        numnHeigth_I = 0;
                                    }

                                    //                              //Find Qfrom IO.
                                    IoentityInputsAndOutputsEntityDB ioentityQfromRes = context.InputsAndOutputs.FirstOrDefault(
                                        io => io.intPkWorkflow == piwentity.intPkWorkflow &&
                                        io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                        io.intnPkElementElementType == intnPkElementElementTypeO &&
                                        io.intnPkElementElement == intnPkElementElementO);

                                    bool boolIOQFromIsSizeBearer = false;
                                    if (
                                        ioentityQfromRes != null
                                        )
                                    {
                                        if (
                                            ioentityQfromRes.boolnSize == true
                                            )
                                        {
                                            boolIOQFromIsSizeBearer = true;
                                        }
                                    }

                                    double numCutWidthToCalculateFolderFactor = numCutWidth_I;
                                    double numCutHeightToCalculateFolderFactor = numCutHeight_I;
                                    bool boolCutDimensionsCanBeZero = false;
                                    if (
                                        //                          //ResO (QFrom) is size bearer
                                        boolIOQFromIsSizeBearer
                                        )
                                    {
                                        boolCutDimensionsCanBeZero = true;
                                        numCutWidth_I = 0;
                                        numCutHeight_I = 0;
                                    }

                                    intStatus_IO = 406;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "Paper dimensions, Margins and gaps should be greater than 0.";
                                    if (
                                        //                          //Original size
                                        (boolIsIOInputPostSize ||
                                        (!boolIsRoll && numWidth_I > 0 && numnHeigth_I > 0) ||
                                        (boolIsRoll && numWidth_I > 0 && numnHeigth_I == null)) &&
                                        //                          //Finished size
                                        (boolCutDimensionsCanBeZero ||
                                        (numCutWidth_I > 0 && numCutHeight_I > 0))
                                        )
                                    {
                                        //                          //To fill and return.
                                        int intPkPaTrans;

                                        if (
                                            intnPkPaTrans_I == null
                                            )
                                        {
                                            intPkPaTrans = -1;

                                            //                      //Create register in PaperTransformation.
                                            CalCalculation.subCreatePaperTransformationRegister(numWidth_I, numnHeigth_I,
                                                numCutWidth_I, numCutHeight_I, numCutWidthToCalculateFolderFactor,
                                                numCutHeightToCalculateFolderFactor, boolCut_I, numnMarginTop_I, numnMarginBottom_I,
                                                numnMarginLeft_I, numnMarginRight_I, numnVerticalGap_I, numnHorizontalGap_I,
                                                strUnit_I, intnPkElementElementTypeI, intnPkElementElementI, intPkResourceI_I,
                                                intnPkElementElementTypeO, intnPkElementElementO, intPkResourceO_I,
                                                intPkProcessInWorkflow_I, piwentity, boolIsOptimized_I, out intPkPaTrans,
                                                ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                                        }
                                        else
                                        {
                                            //                      //Update register in PaperTransformation.
                                            CalCalculation.subUpdatePaperTransformationRegister((int)intnPkPaTrans_I,
                                                intnPkCalculation_I, numWidth_I, numnHeigth_I, numCutWidth_I, numCutHeight_I,
                                                numCutWidthToCalculateFolderFactor, numCutHeightToCalculateFolderFactor,
                                                boolCut_I, numnMarginTop_I, numnMarginBottom_I, numnMarginLeft_I,
                                                numnMarginRight_I, numnVerticalGap_I, numnHorizontalGap_I, intPkResourceI_I,
                                                intnPkElementElementTypeO, intnPkElementElementO, intPkResourceO_I,
                                                strUnit_I, piwentity, boolIsOptimized_I, out intPkPaTrans, ref intStatus_IO,
                                                ref strUserMessage_IO, ref strDevMessage_IO);
                                        }

                                        //                          //Data to get Back.
                                        patransjson_O = (intPkPaTrans == -1) ? null :
                                            new Patransjson2PaperTransformationJson2(intPkPaTrans);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolResourceInIOIsValid(
            //                                              //Verify if the given resource is the one setted in 
            //                                              //      the specific IO (as input).

            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int intPkResource_I,
            int? intnPkElementElementTypeI_I,
            int? intnPkElementElementI_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                io.intnPkElementElementType == intnPkElementElementTypeI_I &&
                io.intnPkElementElement == intnPkElementElementI_I);

            bool boolIsValidPkResource = false;
            if (
                //                                          //IO exists in InputsAndOutputs.
                ioentity != null
                )
            {
                if (
                    //                                      //Resource is not in a group.
                    ioentity.intnPkResource != null
                    )
                {
                    boolIsValidPkResource = (
                        ioentity.intnPkResource == intPkResource_I
                        );
                }
                else
                {
                    //                                      //Resource in group.
                    //                                      //Verify if the resource exists in the group.
                    List<GpresentityGroupResourceEntityDB> darrgpresentity = context.GroupResource.Where(gp
                        => gp.intPkWorkflow == piwentity_I.intPkWorkflow &&
                        gp.intId == ioentity.intnGroupResourceId).ToList();

                    foreach (GpresentityGroupResourceEntityDB gpres in darrgpresentity)
                    {
                        if (
                            gpres.intPkResource == intPkResource_I
                            )
                        {
                            boolIsValidPkResource = true;
                        }
                    }
                }
            }

            if (
                //                                          //Resource was not found in table IO.
                !boolIsValidPkResource
                )
            {
                //                                          //Search in table IOJ.
                IojentityInputsAndOutputsForAJobEntityDB iojentity =
                    context.InputsAndOutputsForAJob.FirstOrDefault(ioj =>
                   ioj.intPkProcessInWorkflow == piwentity_I.intPk &&
                   ioj.intnPkElementElementType == intnPkElementElementTypeI_I &&
                   ioj.intnPkElementElement == intnPkElementElementI_I);

                if (
                    iojentity != null
                    )
                {
                    boolIsValidPkResource = (
                        iojentity.intPkResource == intPkResource_I
                        );
                }

            }
            return boolIsValidPkResource;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCreatePaperTransformationRegister(
            //                                              //Create the register in table paperTransformation.

            double numWidth_I,
            double? numnHeigth_I,
            double numCutWidth_I,
            double numCutHeight_I,
            double numCutWidthToCalculateFolderFactor_I,
            double numCutHeightToCalculateFolderFactor_I,
            bool boolCut_I,
            double? numnMarginTop_I,
            double? numnMarginBottom_I,
            double? numnMarginLeft_I,
            double? numnMarginRight_I,
            double? numnVerticalGap_I,
            double? numnHorizontalGap_I,
            String strUnit_I,
            int? intnPkElementElementTypeI_I,
            int? intnPkElementElementI_I,
            int intPkResourceI_I,
            int? intnPkElementElementTypeO_I,
            int? intnPkElementElementO_I,
            int intPkResourceO_I,
            int intPkProcessInWorkflow_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            bool boolIsOptimized_I,
            out int intPkPaTrans_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            intPkPaTrans_O = -1;

            int intFoldFactorCalculated;
            if (
                //                                          //It is cuts.
                boolCut_I
                )
            {
                intFoldFactorCalculated = 1;
                intStatus_IO = 200;
            }
            else
            {
                //                                          //It is folding.
                CutdatajsonCutDataJson cutdatajson;
                CalCalculation.subfunCalculateCutsOrFoldedFactor(intPkResourceI_I, numWidth_I, numnHeigth_I,
                    numCutWidthToCalculateFolderFactor_I, numCutHeightToCalculateFolderFactor_I,
                    numnMarginTop_I, numnMarginBottom_I, numnMarginLeft_I, numnMarginRight_I, numnVerticalGap_I,
                    numnHorizontalGap_I, boolIsOptimized_I, out cutdatajson, ref intStatus_IO, ref strUserMessage_IO,
                    ref strDevMessage_IO);

                intFoldFactorCalculated = (int)cutdatajson.numPerUnit;
            }

            if (
                intStatus_IO == 200
                )
            {


                //                                              //Create new register.
                //                                              //Create paperTransformation register
                PatransPaperTransformationEntityDB patransNew = new PatransPaperTransformationEntityDB
                {
                    numWidthI = numWidth_I,
                    numnHeightI = numnHeigth_I,
                    numWidthO = numCutWidth_I,
                    numHeightO = numCutHeight_I,
                    numnMarginTop = numnMarginTop_I,
                    numnMarginBottom = numnMarginBottom_I,
                    numnMarginLeft = numnMarginLeft_I,
                    numnMarginRight = numnMarginRight_I,
                    numnVerticalGap = numnVerticalGap_I,
                    numnHorizontalGap = numnHorizontalGap_I,
                    strUnit = strUnit_I,
                    boolTemporary = true,
                    intPkProcessInWorkflow = intPkProcessInWorkflow_I,
                    intPkResourceI = intPkResourceI_I,
                    intnPkElementElementTypeI = intnPkElementElementTypeI_I,
                    intnPkElementElementI = intnPkElementElementI_I,
                    intnPkElementElementTypeO = intnPkElementElementTypeO_I,
                    intnPkElementElementO = intnPkElementElementO_I,
                    intnPkResourceO = intPkResourceO_I,
                    intFoldFactor = intFoldFactorCalculated,
                    boolOptimized = boolIsOptimized_I,
                    boolCut = boolCut_I
                };
                context.PaperTransformation.Add(patransNew);

                //                                              //Verify if the output IO has link. It has, create a 
                //                                              //      register for the other side of the link
                List<IoentityInputsAndOutputsEntityDB> darrioentity =
                    CalCalculation.darrioentityVerifyIfIOHasLink(piwentity_I, intnPkElementElementTypeO_I,
                    intnPkElementElementO_I);

                foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
                {
                    //                                          //Get the ProcessInWorkflow.
                    PiwentityProcessInWorkflowEntityDB piwentityLink = context.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPkWorkflow == ioentity.intPkWorkflow &&
                        piw.intProcessInWorkflowId == ioentity.intnProcessInWorkflowId);

                    //                                          //Create new register.
                    PatransPaperTransformationEntityDB patransNewO = new PatransPaperTransformationEntityDB
                    {
                        numWidthI = patransNew.numWidthO,
                        numnHeightI = patransNew.numHeightO,
                        numWidthO = 0,
                        numHeightO = 0,
                        strUnit = patransNew.strUnit,
                        boolTemporary = false,
                        intPkProcessInWorkflow = piwentityLink.intPk,
                        intPkResourceI = (int)patransNew.intnPkResourceO,
                        intnPkElementElementTypeI = ioentity.intnPkElementElementType,
                        intnPkElementElementI = ioentity.intnPkElementElement,
                        intnPkElementElementTypeO = null,
                        intnPkElementElementO = null,
                        intnPkResourceO = null,
                        boolOptimized = true,
                        boolCut = boolCut_I

                    };
                    context.PaperTransformation.Add(patransNewO);
                }

                context.SaveChanges();
                intPkPaTrans_O = patransNew.intPk;

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdatePaperTransformationRegister(
            //                                              //Update the register in table paperTransformation.

            int intPkPaTrans_I,
            int? intnPkCalculation_I,
            double numWidth_I,
            double? numnHeight_I,
            double numCutWidth_I,
            double numCutHeight_I,
            double numCutWidthToCalculateFolderFactor_I,
            double numCutHeightToCalculateFolderFactor_I,
            bool boolCut_I,
            double? numnMarginTop_I,
            double? numnMarginBottom_I,
            double? numnMarginLeft_I,
            double? numnMarginRight_I,
            double? numnVerticalGap_I,
            double? numnHorizontalGap_I,
            int intPkResourceI_I,
            int? intnPkElementElementTypeO_I,
            int? intnPkElementElementO_I,
            int intPkResourceO_I,
            String strUnit_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            bool boolIsOptimized_I,
            out int intPkPaTrans_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intPkPaTrans_O = -1;
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Update.
            //                                              //Get paperTransform using PkPaTrans
            PatransPaperTransformationEntityDB patransentity =
                context.PaperTransformation.FirstOrDefault(pa => pa.intPk == intPkPaTrans_I);


            intStatus_IO = 405;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "PaperTransformation is not valid.";
            if (
                patransentity != null
                )
            {
                int intFoldFactorCalculated;
                if (
                    //                                      //It is cuts.
                    boolCut_I
                    )
                {
                    intFoldFactorCalculated = 1;
                    intStatus_IO = 200;
                }
                else
                {
                    //                                      //It is folding.
                    CutdatajsonCutDataJson cutdatajson;
                    CalCalculation.subfunCalculateCutsOrFoldedFactor(intPkResourceI_I, numWidth_I, numnHeight_I,
                        numCutWidth_I, numCutHeight_I, numnMarginTop_I, numnMarginBottom_I, numnMarginLeft_I,
                        numnMarginRight_I, numnVerticalGap_I, numnHorizontalGap_I, boolIsOptimized_I, out cutdatajson,
                        ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                    intFoldFactorCalculated = (int)cutdatajson.numPerUnit;
                }

                if (
                    intStatus_IO == 200
                    )
                {


                    if (
                    //                                      //It is just a paperTransformation edit.
                    intnPkCalculation_I == null
                    )
                    {
                        //                                      //Update information.
                        patransentity.numWidthI = numWidth_I;
                        patransentity.numnHeightI = numnHeight_I;
                        patransentity.numWidthO = numCutWidth_I;
                        patransentity.numHeightO = numCutHeight_I;
                        patransentity.numnMarginTop = numnMarginTop_I;
                        patransentity.numnMarginBottom = numnMarginBottom_I;
                        patransentity.numnMarginLeft = numnMarginLeft_I;
                        patransentity.numnMarginRight = numnMarginRight_I;
                        patransentity.numnVerticalGap = numnVerticalGap_I;
                        patransentity.numnHorizontalGap = numnHorizontalGap_I;
                        patransentity.strUnit = strUnit_I;
                        patransentity.boolOptimized = boolIsOptimized_I;
                        patransentity.boolCut = boolCut_I;
                        patransentity.intFoldFactor = intFoldFactorCalculated;
                        context.Update(patransentity);
                        context.SaveChanges();

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";

                        intPkPaTrans_O = patransentity.intPk;

                        //                                      //Update values at other side of link.
                        CalCalculation.subUpdateValueOtherSideOfLink(patransentity, numCutWidth_I, numCutHeight_I,
                            strUnit_I);

                    }
                    else
                    {
                        //                                      //It is a calculation edit.
                        //                                      //Validate PkCalculation.
                        CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(cal =>
                            cal.intPk == intnPkCalculation_I);

                        intStatus_IO = 406;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Calculation is not valid.";
                        if (
                            calentity != null
                            )
                        {
                            if (
                                patransentity.intnPkCalculationOwn == calentity.intPk
                                )
                            {
                                //                              //Add a temporary paperTransformation until the calculation
                                //                              //      be updated.
                                //                              //Create register in PaperTransformation.
                                CalCalculation.subCreatePaperTransformationRegister(numWidth_I, numnHeight_I,
                                    numCutWidth_I, numCutHeight_I, numCutWidthToCalculateFolderFactor_I,
                                    numCutHeightToCalculateFolderFactor_I, boolCut_I, numnMarginTop_I, numnMarginBottom_I,
                                    numnMarginLeft_I, numnMarginRight_I, numnVerticalGap_I, numnHorizontalGap_I,
                                    strUnit_I, patransentity.intnPkElementElementTypeI, patransentity.intnPkElementElementI,
                                    patransentity.intPkResourceI, intnPkElementElementTypeO_I, intnPkElementElementO_I,
                                    intPkResourceO_I, patransentity.intPkProcessInWorkflow, piwentity_I, boolIsOptimized_I,
                                    out intPkPaTrans_O, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                            }
                            else
                            {
                                //                              //Update information.
                                patransentity.numWidthI = numWidth_I;
                                patransentity.numnHeightI = numnHeight_I;
                                patransentity.numWidthO = numCutWidth_I;
                                patransentity.numHeightO = numCutHeight_I;
                                patransentity.numnMarginTop = numnMarginTop_I;
                                patransentity.numnMarginBottom = numnMarginBottom_I;
                                patransentity.numnMarginLeft = numnMarginLeft_I;
                                patransentity.numnMarginRight = numnMarginRight_I;
                                patransentity.numnVerticalGap = numnVerticalGap_I;
                                patransentity.numnHorizontalGap = numnHorizontalGap_I;
                                patransentity.intnPkElementElementTypeO = intnPkElementElementTypeO_I;
                                patransentity.intnPkElementElementO = intnPkElementElementO_I;
                                patransentity.intnPkResourceO = intPkResourceO_I;
                                patransentity.strUnit = strUnit_I;
                                patransentity.boolOptimized = boolIsOptimized_I;
                                patransentity.boolCut = boolCut_I;
                                patransentity.intFoldFactor = intFoldFactorCalculated;
                                context.Update(patransentity);
                                context.SaveChanges();

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";

                                intPkPaTrans_O = patransentity.intPk;
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }


        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdateValueOtherSideOfLink(
            //                                              //Update paper size at other side of the link.

            PatransPaperTransformationEntityDB patransentity_I,
            double numCutWidth_I,
            double numCutHeigth_I,
            String strUnit_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get other side of link.
            PatransPaperTransformationEntityDB patransentityLink = context.PaperTransformation.FirstOrDefault(pa
                => pa.intnPkElementElementTypeO == null &&
                pa.intnPkElementElementO == null && pa.intPkResourceI == patransentity_I.intnPkResourceO &&
                pa.intPkProcessInWorkflow != patransentity_I.intPkProcessInWorkflow &&
                pa.boolTemporary == false && pa.intnPkResourceO == null);

            if (
                //                                          //There is a Link.
                patransentityLink != null
                )
            {
                //                                          //Update information.
                patransentityLink.numWidthI = numCutWidth_I;
                patransentityLink.numnHeightI = numCutHeigth_I;
                patransentityLink.numWidthO = 0.0;
                patransentityLink.numHeightO = 0.0;
                patransentityLink.strUnit = strUnit_I;
                patransentityLink.boolCut = patransentity_I.boolCut;

                context.Update(patransentityLink);
                context.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<IoentityInputsAndOutputsEntityDB> darrioentityVerifyIfIOHasLink(
            //                                              //Take a IO(out), verify if has link and if it has create a
            //                                              //      new regiser in PaperTransformation

            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int? intnPkElementElementTypeO_I,
            int? intnPkElementElementO_I
            )
        {
            List<IoentityInputsAndOutputsEntityDB> darrioentityLink = new List<IoentityInputsAndOutputsEntityDB>();
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                io.intnPkElementElementType == intnPkElementElementTypeO_I &&
                io.intnPkElementElement == intnPkElementElementO_I);

            if (
                //                                          //IO exists in InputsAndOutputs.
                ioentity != null
                )
            {
                if (
                    //                                      //IO has link.
                    ioentity.strLink != null
                    )
                {
                    //                                      //Get the other side of the link.
                    darrioentityLink = context.InputsAndOutputs.Where(io => io.intPkWorkflow ==
                        piwentity_I.intPkWorkflow && io.strLink == ioentity.strLink && io.intPk != ioentity.intPk &&
                        //                                  //Not a node.
                        io.intnProcessInWorkflowId != null).ToList();
                }
            }

            return darrioentityLink;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateCalculationStatus(
            //                                              //Update the status for an specific calculation.

            int intPkCalculation_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Validate calculation.
            CalentityCalculationEntityDB calentity = context_M.Calculation.FirstOrDefault(cal => cal.intPk == intPkCalculation_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Calculation not found.";
            strDevMessage_IO = "Calculation not found.";
            if (
                //                                          //Calculation exists.
                calentity != null &&
                //                                          //Calculation is not deleted.
                calentity.strEndDate == null
                )
            {
                calentity.boolIsEnable = calentity.boolIsEnable ? false : true;

                intStatus_IO = 200;
                strUserMessage_IO = " ";
                strDevMessage_IO = " ";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetProfitCalculationsForAProduct(
        //                                                  //Return an array of calculations.
        int intPkProduct_I,
        String strPrintshopId_I,
        out List<CaljsonCalculationJson> darrcaljson_O,
        ref int intStatus_IO,
        ref String strUserMessage_IO,
        ref String strDevMessage_IO
        )
        {
            PsPrintShop psPrintshop = PsPrintShop.psGet(strPrintshopId_I);
            darrcaljson_O = new List<CaljsonCalculationJson>();
            //                                              //Establish the connecion.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Verify product.           
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et => et.intPk ==
                intPkProduct_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Product not found or Pk is not for a product.";
            if (
                etentity != null &&
                etentity.strResOrPro == ProdtypProductType.strProduct
                )
            {
                ProdtypProductType prodtem = (ProdtypProductType)EtElementTypeAbstract.etFromDB(intPkProduct_I);
                //                                          //Create a list of json to send to the front.
                foreach (CalCalculation cal in prodtem.darrcalCurrent.ToList())
                {

                    if (
                        (cal.strCalculationType == CalCalculation.strProfit)
                        )
                    {
                        CaljsonCalculationJson caljson = new CaljsonCalculationJson();
                        caljson.intPk = cal.intPk;
                        caljson.strDescription = cal.strDescription;
                        caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                        caljson.numnProfit = cal.numnProfit;
                        caljson.boolIsEnable = cal.boolIsEnable;
                        caljson.boolIsEditable = true;
                        caljson.strBy = cal.strByX;
                        //                                  //Add each calculation to the arr dinamic
                        darrcaljson_O.Add(caljson);
                    }
                }
                intStatus_IO = 200;
                strUserMessage_IO = " ";
                strDevMessage_IO = " ";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPerUnitCalculationsForAProduct(
        //                                                  //Return an array of calculations.
        int intPkProduct_I,
        String strPrintshopId_I,
        bool? boolnByProduct_I,
        bool? boolnByIntent_I,
        out List<CaljsonCalculationJson> darrcaljson_O,
        ref int intStatus_IO,
        ref String strUserMessage_IO,
        ref String strDevMessage_IO
        )
        {
            PsPrintShop psPrintshop = PsPrintShop.psGet(strPrintshopId_I);
            darrcaljson_O = new List<CaljsonCalculationJson>();
            //                                              //Establish the connecion.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Verify product.
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et => et.intPk ==
                intPkProduct_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Product not found or Pk is not for a product.";
            if (
                etentity != null &&
                etentity.strResOrPro == ProdtypProductType.strProduct
                )
            {
                ProdtypProductType prodtem = (ProdtypProductType)EtElementTypeAbstract.etFromDB(intPkProduct_I);
                List<CalCalculation> darrcalFinal = new List<CalCalculation>();

                List<CalCalculation> darrcal = prodtem.darrcalCurrent;

                /*CASE*/
                if (
                     //                                  //By Product
                     (boolnByIntent_I != true) &&
                     (boolnByProduct_I == true)
                     )
                {
                    darrcalFinal = darrcal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerUnit) &&
                        (cal.intnPkProcessElementBelongsTo == null) &&
                        (cal.intnPkResourceElementBelongsTo == null) &&
                        (cal.strByX == CalCalculation.strByProduct) &&
                        (cal.strAscendants == null) &&
                        (cal.strValue == null)
                    ).ToList();
                }
                else if
                    (
                     //                                 //By Intent
                     (boolnByIntent_I == true) &&
                     (boolnByProduct_I != true)
                    )
                {
                    darrcalFinal = darrcal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerUnit) &&
                        (cal.strByX == CalCalculation.strByIntent) &&
                        (cal.intnPkProcessElementBelongsTo == null) &&
                        (cal.intnPkResourceElementBelongsTo == null) &&
                        (cal.strAscendants != null) &&
                        (cal.strValue != null)
                    ).ToList();
                }
                /*END-CASE*/

                //                                      //Create the json to send to the front.
                foreach (CalCalculation cal in darrcalFinal)
                {
                    CaljsonCalculationJson caljson = new CaljsonCalculationJson();
                    caljson.intPk = cal.intPk;                    
                    caljson.numnCost = cal.numnCost;
                    caljson.strDescription = cal.strDescription;
                    caljson.intnPkProcess = cal.intnPkProcessElementBelongsTo;
                    caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                    caljson.boolIsEnable = cal.boolIsEnable;
                    caljson.strCalculationType = cal.strCalculationType;
                    caljson.strValue = cal.strValue;
                    caljson.arrAscendantName = Tools.arrstrAscendantName(cal.strAscendants);
                    caljson.strBy = cal.strByX;
                    caljson.numnMin = cal.numnMin;
                    caljson.numnBlock = cal.numnBlock;

                    //                          //Add each calculation to the array.
                    darrcaljson_O.Add(caljson);

                }
                intStatus_IO = 200;
                strUserMessage_IO = " ";
                strDevMessage_IO = " ";

            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetProcessDefaults(
            //                                              //Return an array of default processes' calculations.

            String strCalculationType_I,
            PsPrintShop ps_I,
            out List<CaljsonCalculationJson> darrcaljson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrcaljson_O = new List<CaljsonCalculationJson>();

            //                                              //Get process's default calculations.
            CalCalculation[] arrprocal = ps_I.diccalProcess.Values.Where(cal =>
                cal.strCalculationType == strCalculationType_I).ToArray();

            foreach (CalCalculation cal in arrprocal)
            {
                if (
                    //                                      //Add the calculation if still available.
                    cal.strEndDate == null
                    )
                {
                    //                                      //Json to return.
                    CaljsonCalculationJson caljson = new CaljsonCalculationJson();

                    //                                      //Fill Json.

                    caljson.strProcessName = ProProcess.proFromDB(cal.intnPkProcessElementBelongsTo).strName;
                    caljson.intPk = cal.intPk;
                    caljson.strUnitI = cal.strUnit;
                    caljson.numnQuantity = cal.numnQuantity;
                    caljson.numnPerUnits = cal.numnPerUnits;
                    caljson.numnNeeded = cal.numnNeeded;
                    caljson.numnCost = cal.numnCost;
                    caljson.arrAscendantName = Tools.arrstrAscendantName(cal.strAscendants);
                    caljson.numnMin = cal.numnMin;
                    caljson.numnProfit = cal.numnProfit;
                    caljson.strDescription = cal.strDescription;
                    caljson.numnBlock = cal.numnBlock;
                    caljson.boolIsEnable = cal.boolIsEnable;
                    caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                    caljson.intnPkProcess = cal.intnPkProcessElementBelongsTo;
                    caljson.strCalculationType = cal.strCalculationType;
                    caljson.strBy = cal.strByX;
                    caljson.intnHours = cal.intnHours;
                    caljson.intnMinutes = cal.intnMinutes;
                    caljson.intnSeconds = cal.intnSeconds;
                    caljson.boolIsEditable = true;

                    if (
                        //                                  //There is an associated account 
                        cal.intnPkAccount != null
                        )
                    {
                        Odyssey2Context context = new Odyssey2Context();
                        //                                  //Find account name
                        caljson.strAccountName = context.Account.FirstOrDefault(acc =>
                            acc.intPk == cal.intnPkAccount).strNumber;
                    }

                    darrcaljson_O.Add(caljson);
                }
            }
            intStatus_IO = 200;
            strUserMessage_IO = " ";
            strDevMessage_IO = " ";
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int[] arrintIdGroup(
            int intPkProduct_I
            )
        {
            List<int> darrintId = new List<int>();
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all calculation of the product.
            IQueryable<CalentityCalculationEntityDB> setcalentity =
                context.Calculation.Where(etentity => etentity.intnPkProduct == intPkProduct_I);
            List<CalentityCalculationEntityDB> darrcalentity = setcalentity.ToList();
            //                                              //Go through each calculation
            foreach (CalentityCalculationEntityDB calentity in darrcalentity)
            {
                //                                          //Get all group of each calculation
                IQueryable<GpcalentityGroupCalculationEntityDB> setgpcalentity =
                context.GroupCalculation.Where(gpcalentity => gpcalentity.intPkCalculation == calentity.intPk);
                List<GpcalentityGroupCalculationEntityDB> dgpcalentity = setgpcalentity.ToList();
                //                                          //Go throught each group
                foreach (GpcalentityGroupCalculationEntityDB gpcalentity in dgpcalentity)
                {
                    if (
                        !darrintId.Exists(id => id == gpcalentity.intId)
                        )
                    {
                        //                                  /Add id the arrId
                        darrintId.Add(gpcalentity.intId);
                    }
                }
            }
            return darrintId.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CalCalculation calGetFromDb(
            int intPk_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Search if the cal still exist.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(calentity =>
                calentity.intPk == intPk_I);

            CalCalculation cal = null;
            if (
                calentity != null
                )
            {
                cal = new CalCalculation(calentity.intPk,
                    calentity.strUnit, calentity.numnQuantity, calentity.numnCost, calentity.intnHours,
                    calentity.intnMinutes, calentity.intnSeconds, calentity.numnBlock, calentity.boolIsEnable,
                    calentity.strValue, calentity.strAscendants, calentity.strDescription,
                    calentity.numnProfit,
                    calentity.intnPkProduct, calentity.intnPkProcess, calentity.intnPkResource,
                    calentity.strCalculationType, calentity.strByX, calentity.strStartDate, calentity.strStartTime,
                    calentity.strEndDate, calentity.strEndTime, calentity.numnNeeded, calentity.numnPerUnits,
                    calentity.numnMin, calentity.numnQuantityWaste, calentity.numnPercentWaste,
                    calentity.intnPkWorkflow, calentity.intnProcessInWorkflowId,
                    calentity.intnPkElementElementType, calentity.intnPkElementElement,
                    calentity.intnPkQFromElementElementType, calentity.intnPkQFromElementElement,
                    calentity.intnPkQFromResource, calentity.intnPkAccount, calentity.boolnFromThickness, 
                    calentity.boolnIsBlock, calentity.boolnByArea);
            }
            return cal;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CalCalculation[] arrcalGetFromAProcess(
            int intPk_I,
            String strCalculationType_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Search if the cal still exist.
            IQueryable<CalentityCalculationEntityDB> setcalentity = context.Calculation.Where(calentity =>
                calentity.intnPkProcess == intPk_I && calentity.intnPkProduct == null &&
                calentity.strCalculationType == strCalculationType_I &&
                calentity.intnPkProduct == null && calentity.intnPkResource == null);

            List<CalCalculation> darrcal = new List<CalCalculation>();
            foreach (CalentityCalculationEntityDB calentity in setcalentity)
            {
                CalCalculation cal = new CalCalculation(calentity.intPk,
                    calentity.strUnit, calentity.numnQuantity, calentity.numnCost,
                    calentity.intnHours, calentity.intnMinutes, calentity.intnSeconds,
                    calentity.numnBlock, calentity.boolIsEnable, calentity.strValue, calentity.strAscendants,
                    calentity.strDescription, calentity.numnProfit, 
                    calentity.intnPkProduct, calentity.intnPkProcess,
                    calentity.intnPkResource, calentity.strCalculationType, calentity.strByX, calentity.strStartDate,
                    calentity.strStartTime, calentity.strEndDate, calentity.strEndTime, calentity.numnNeeded,
                    calentity.numnPerUnits, calentity.numnMin, null, null, calentity.intnPkWorkflow,
                    calentity.intnProcessInWorkflowId, calentity.intnPkElementElementType,
                    calentity.intnPkElementElement, null, null, null, calentity.intnPkAccount,
                    calentity.boolnFromThickness, calentity.boolnIsBlock, calentity.boolnByArea);

                darrcal.Add(cal);
            }

            return darrcal.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CalCalculation[] arrcalGetFromAResource(
            int intPk_I,
            int intPkElement_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Search if the cal still exist.
            IQueryable<CalentityCalculationEntityDB> setcalentity = context.Calculation.Where(calentity =>
                calentity.intnPkProduct == null && calentity.intnPkProcess == null);

            List<CalCalculation> darrcal = new List<CalCalculation>();
            foreach (CalentityCalculationEntityDB calentity in setcalentity)
            {

                CalCalculation cal = new CalCalculation(calentity.intPk, 
                    calentity.strUnit, calentity.numnQuantity, calentity.numnCost,
                    calentity.intnHours, calentity.intnMinutes, calentity.intnSeconds,
                    calentity.numnBlock, calentity.boolIsEnable, calentity.strValue, calentity.strAscendants,
                    calentity.strDescription, calentity.numnProfit,
                    calentity.intnPkProduct, calentity.intnPkProcess,
                    calentity.intnPkResource, calentity.strCalculationType, calentity.strByX, calentity.strStartDate,
                    calentity.strStartTime, calentity.strEndDate, calentity.strEndTime, calentity.numnNeeded,
                    calentity.numnPerUnits, calentity.numnMin, null, null, calentity.intnPkWorkflow,
                    calentity.intnProcessInWorkflowId, calentity.intnPkElementElementType,
                    calentity.intnPkElementElement, null, null, null, null, calentity.boolnFromThickness,
                    calentity.boolnIsBlock, calentity.boolnByArea);

                darrcal.Add(cal);
            }

            return darrcal.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CaljsonCalculationJson[] arrcaljsonGetBase(
            int? intPkProduct_I,
            bool? boolnByProcess_I,
            bool? boolnByTime_I,
            int? intnJobId_I,
            String strPrintshopId_I,
            int? intnPkWorkflow_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            CaljsonCalculationJson[] arrcaljson = null;

            //                                              //Establish the connecion.
            Odyssey2Context context = new Odyssey2Context();

            JobjsonJobJson jobjson = new JobjsonJobJson();
            if (
                (intnJobId_I != null && JobJob.boolIsValidJobId((int)intnJobId_I, strPrintshopId_I, configuration_I,
                out jobjson, ref strUserMessage_IO, ref strDevMessage_IO)) ||
                (intnJobId_I == null)
                )
            {
                //                                          //Verify product.            
                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                et.intPk == intPkProduct_I);

                WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf => wf.intPk == intnPkWorkflow_I);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Something is wrong.";
                if (
                    //                                      //It is from product or product wf.
                    ((etentity != null) && (etentity.strResOrPro == ProdtypProductType.strProduct)) ||
                    //                                      //It is from wf base.
                    ((etentity == null) && (wfentity != null) && (wfentity.intnPkProduct == null))
                    )
                {
                    //                                      //Get the product.
                    ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(intPkProduct_I);

                    //                                      //List of calculations to return.
                    List<CaljsonCalculationJson> darrcaljsonWorkflow = new List<CaljsonCalculationJson>();
                    List<CaljsonCalculationJson> darrcaljsonProduct = new List<CaljsonCalculationJson>();
                    List<CalCalculation> darrcalFinal = new List<CalCalculation>();

                    List<CalCalculation> darrcal = new List<CalCalculation>();
                    if (
                        prodtyp == null
                        )
                    {
                        //                                  //Get all calculations for wf base.
                        List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(caletentity =>
                            caletentity.intnPkProduct == null && caletentity.intnPkWorkflow == intnPkWorkflow_I &&
                            caletentity.strEndDate == null).ToList();

                        //                                  //Get all the calculations.
                        foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                        {
                            CalCalculation cal = new CalCalculation(calentity.intPk,
                                calentity.strUnit, calentity.numnQuantity, calentity.numnCost,
                                calentity.intnHours, calentity.intnMinutes, calentity.intnSeconds,
                                calentity.numnBlock, calentity.boolIsEnable, calentity.strValue,
                                calentity.strAscendants, calentity.strDescription, calentity.numnProfit,
                                calentity.intnPkProduct, calentity.intnPkProcess, calentity.intnPkResource,
                                calentity.strCalculationType, calentity.strByX, calentity.strStartDate,
                                calentity.strStartTime, calentity.strEndDate, calentity.strEndTime,
                                calentity.numnNeeded, calentity.numnPerUnits,
                                calentity.numnMin, calentity.numnQuantityWaste, calentity.numnPercentWaste,
                                calentity.intnPkWorkflow, calentity.intnProcessInWorkflowId,
                                calentity.intnPkElementElementType, calentity.intnPkElementElement,
                                calentity.intnPkQFromElementElementType, calentity.intnPkQFromElementElement,
                                calentity.intnPkQFromResource, calentity.intnPkAccount, calentity.boolnFromThickness,
                                calentity.boolnIsBlock, calentity.boolnByArea);
                            darrcal.Add(cal);
                        }
                    }
                    else
                    {
                        //                                  //Get all calculation current of a product.
                        darrcal = prodtyp.darrcalCurrent;
                    }

                    if (
                        //                                  //By Product
                        boolnByProcess_I != true
                        )
                    {
                        darrcalFinal = darrcal.Where(cal => (cal.strCalculationType == CalCalculation.strBase) &&
                            (cal.strByX == CalCalculation.strByProduct) && (cal.intnPkProcessElementBelongsTo == null) &&
                            (cal.intnPkResourceElementBelongsTo == null) && (cal.strAscendants == null) &&
                            (cal.strValue == null)).ToList();
                    }
                    else
                    {
                        if (
                            prodtyp != null
                            )
                        {
                            //                                  //Get calculations.
                            darrcalFinal = prodtyp.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intnJobId_I,
                                intnPkWorkflow_I);
                        }
                        else
                        {
                            darrcalFinal = darrcal;
                        }

                        //                                  //By Process.
                        if (
                            boolnByTime_I == true
                            )
                        {
                            darrcalFinal = darrcalFinal.Where(cal =>
                                (cal.strCalculationType == CalCalculation.strBase) &&
                                (cal.strByX == CalCalculation.strByProcess) &&
                                (cal.intnPkProcessElementBelongsTo != null) &&
                                (cal.intnPkResourceElementBelongsTo == null) && (cal.strAscendants == null) &&
                                (cal.strValue == null) && (cal.numnCost == null) && ((cal.intnHours != null) &&
                                (cal.intnMinutes != null) && (cal.intnSeconds != null))).ToList();
                        }
                        else if (
                            boolnByTime_I == false
                            )
                        {
                            //                              //Get Calculation by Cost.
                            darrcalFinal = darrcalFinal.Where(cal =>
                                (cal.strCalculationType == CalCalculation.strBase) &&
                                (cal.strByX == CalCalculation.strByProcess) &&
                                (cal.intnPkProcessElementBelongsTo != null) &&
                                (cal.intnPkResourceElementBelongsTo == null) && (cal.strAscendants == null) &&
                                (cal.strValue == null) && (cal.numnCost != null) && ((cal.intnHours == null) &&
                                (cal.intnMinutes == null) && (cal.intnSeconds == null))).ToList();
                        }
                        else
                        {
                            //                              //Return Nothing.
                            darrcalFinal = new List<CalCalculation>();
                        }
                    }

                    //                                      //Create jsons.
                    foreach (CalCalculation cal in darrcalFinal)
                    {
                        String strProcessName = null;
                        if (
                            //                              //By Process.
                            cal.intnPkProcessElementBelongsTo != null
                            )
                        {
                            EleentityElementEntityDB eleentityProcess = context.Element.FirstOrDefault(et =>
                                et.intPk == cal.intnPkProcessElementBelongsTo);

                            strProcessName = eleentityProcess.strElementName;
                        }

                        CaljsonCalculationJson caljson = new CaljsonCalculationJson();
                        caljson.intPk = cal.intPk;
                        caljson.numnCost = cal.numnCost;
                        caljson.intnHours = cal.intnHours;
                        caljson.intnMinutes = cal.intnMinutes;
                        caljson.intnSeconds = cal.intnSeconds;
                        caljson.strDescription = cal.strDescription;
                        caljson.intnPkProcess = cal.intnPkProcessElementBelongsTo;
                        caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                        caljson.boolIsEnable = cal.boolIsEnable;
                        caljson.strCalculationType = cal.strCalculationType;
                        caljson.strBy = cal.strByX;
                        caljson.strProcessName = strProcessName;
                        caljson.boolIsEditable = true;

                        //                                  //Check if the calculation has conditions
                        GpcondjsonGroupConditionJson gpcondjson = Tools.gpcondjsonGetCondition(cal.intPk, null, null,
                            null);
                        if (
                            gpcondjson != null
                            )
                        {
                            caljson.boolHasCondition = true;
                        }

                        if (
                            //                              //There is an associated account 
                            cal.intnPkAccount != null
                            )
                        {
                            //                              //Find account name
                            caljson.strAccountName = context.Account.FirstOrDefault(acc =>
                                acc.intPk == cal.intnPkAccount).strNumber;
                        }

                        if (
                            cal.intnPkWorkflowBelongsTo != null
                            )
                        {
                            caljson.boolnIsWorkflow = true;

                            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                                piw => piw.intPkWorkflow == cal.intnPkWorkflowBelongsTo &&
                                piw.intProcessInWorkflowId == cal.intnProcessInWorkflowId);

                            if (
                                piwentity.intnId != null
                                )
                            {
                                EleentityElementEntityDB eleentityProcessInWorkflow = context.Element.FirstOrDefault(
                                    ele => ele.intPk == piwentity.intPkProcess);

                                caljson.strProcessName = eleentityProcessInWorkflow.strElementName + " (" +
                                    piwentity.intnId + ")";
                            }

                            darrcaljsonWorkflow.Add(caljson);
                        }
                        else
                        {
                            caljson.boolnIsWorkflow = false;
                            darrcaljsonProduct.Add(caljson);
                        }
                    }

                    darrcaljsonProduct = darrcaljsonProduct.OrderBy(cal => cal.strDescription).ToList();
                    darrcaljsonWorkflow = darrcaljsonWorkflow.OrderBy(cal => cal.strDescription).ToList();
                    arrcaljson = darrcaljsonWorkflow.Concat(darrcaljsonProduct).ToArray();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
            return arrcaljson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CaljsonCalculationJson[] arrcaljsonGetPerQuantity(
            int? intPkProduct_I,
            bool? boolnByProcess_I,
            bool? boolnByResource_I,
            bool? boolnByProduct_I,
            bool? boolnByIntent_I,
            bool? boolnByTime_I,
            int? intnJobId_I,
            String strPrintshopId_I,
            int? intnPkWorkflow_I,
            int? intnPkProcessInWorkflow_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            CaljsonCalculationJson[] arrcaljson = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intPkProduct_I);

            WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf => wf.intPk == intnPkWorkflow_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Product not found or pk is not for a product.";
            if (
                //                                          //It comes from a product.
                ((et != null) && (et.strResOrPro == EtElementTypeAbstract.strProduct)) ||
                //                                          //It comes from a wf base.
                ((et == null) && (wfentity != null) && (wfentity.intnPkProduct == null))
                )
            {
                ProdtypProductType prodtyp = (ProdtypProductType)et;

                List<CalCalculation> darrcal = new List<CalCalculation>();
                if (
                    prodtyp == null
                    )
                {
                    //                                  //Get all calculations for wf base.
                    List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(caletentity =>
                    caletentity.intnPkProduct == null && caletentity.intnPkWorkflow == intnPkWorkflow_I &&
                    caletentity.strEndDate == null).ToList();

                    //                                  //Get all the calculations.
                    foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                    {
                        CalCalculation cal = new CalCalculation(calentity.intPk,
                            calentity.strUnit, calentity.numnQuantity, calentity.numnCost,
                            calentity.intnHours, calentity.intnMinutes, calentity.intnSeconds,
                            calentity.numnBlock, calentity.boolIsEnable, calentity.strValue, calentity.strAscendants,
                            calentity.strDescription, calentity.numnProfit, 
                            calentity.intnPkProduct, calentity.intnPkProcess,
                            calentity.intnPkResource, calentity.strCalculationType, calentity.strByX,
                            calentity.strStartDate, calentity.strStartTime,
                            calentity.strEndDate, calentity.strEndTime, calentity.numnNeeded, calentity.numnPerUnits,
                            calentity.numnMin, calentity.numnQuantityWaste, calentity.numnPercentWaste,
                            calentity.intnPkWorkflow, calentity.intnProcessInWorkflowId,
                            calentity.intnPkElementElementType, calentity.intnPkElementElement,
                            calentity.intnPkQFromElementElementType, calentity.intnPkQFromElementElement,
                            calentity.intnPkQFromResource, calentity.intnPkAccount, calentity.boolnFromThickness,
                            calentity.boolnIsBlock, calentity.boolnByArea);
                        darrcal.Add(cal);
                    }
                }
                else
                {
                    //                                      //Get all calculation current of a product.
                    darrcal = prodtyp.darrcalCurrent;
                }

                List<CalCalculation> darrcalFinal = new List<CalCalculation>();

                /*CASE*/
                if (
                    //                                      //By Resource
                    (boolnByResource_I == true) &&
                    (boolnByProcess_I != true) &&
                    (boolnByProduct_I != true) &&
                    (boolnByIntent_I != true)
                    )
                {
                    if (
                        prodtyp != null
                        )
                    {
                        //                                  //Get calculations.
                        darrcalFinal = prodtyp.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intnJobId_I,
                            intnPkWorkflow_I);
                    }
                    else
                    {
                        darrcalFinal = darrcal;
                    }

                    //                                      //Get ProcessInWorkflow.
                    PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPk == intnPkProcessInWorkflow_I);

                    int? intnPkProcessInWorkflowId = piwentity != null ? (int?)piwentity.intProcessInWorkflowId :
                        null;
                    int? intnPkWorkflow = piwentity != null ? (int?)piwentity.intPkWorkflow :
                        null;

                    darrcalFinal = darrcalFinal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerQuantity) &&
                        (cal.strByX == CalCalculation.strByResource) &&
                        (
                        (cal.intnPkResourceElementBelongsTo != null) ||
                        (cal.intnPkProcessElementBelongsTo == null) &&
                        (cal.strAscendants == null) &&
                        (cal.strValue == null)
                        ) &&
                        //                                  //FilterResource By Process.
                        (cal.intnProcessInWorkflowId == intnPkProcessInWorkflowId &&
                        cal.intnPkWorkflowBelongsTo == intnPkWorkflow)
                    ).ToList();
                }
                else if (
                    //                                      //By Process
                    (boolnByProcess_I == true) &&
                    (boolnByResource_I != true) &&
                    (boolnByProduct_I != true) &&
                    (boolnByIntent_I != true)
                    )
                {
                    if (
                        //                                  //Get Calculation by time.
                        boolnByTime_I == true
                        )
                    {
                        if (
                            prodtyp != null
                            )
                        {
                            //                                  //Get calculations.
                            darrcalFinal = prodtyp.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intnJobId_I,
                                intnPkWorkflow_I);
                        }
                        else
                        {
                            darrcalFinal = darrcal;
                        }

                        darrcalFinal = darrcalFinal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerQuantity) &&
                        (cal.strByX == CalCalculation.strByProcess) &&
                        (cal.intnPkProcessElementBelongsTo != null) &&
                        (cal.intnPkResourceElementBelongsTo == null) &&
                        (cal.strAscendants == null) &&
                        (cal.strValue == null) &&
                        (cal.numnCost == null) &&
                        (
                        (cal.intnHours != null) &&
                        (cal.intnMinutes != null) &&
                        (cal.intnSeconds != null)
                        )
                    ).ToList();
                    }
                    else if (
                        boolnByTime_I == false
                        )
                    {
                        if (
                            prodtyp != null
                            )
                        {
                            //                              //Get calculations.
                            darrcalFinal = prodtyp.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intnJobId_I,
                                intnPkWorkflow_I);
                        }
                        else
                        {
                            darrcalFinal = darrcal;
                        }

                        //                                  //Get Calculation by Cost.
                        darrcalFinal = darrcalFinal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerQuantity) &&
                        (cal.strByX == CalCalculation.strByProcess) &&
                        (cal.intnPkProcessElementBelongsTo != null) &&
                        (cal.intnPkResourceElementBelongsTo == null) &&
                        (cal.strAscendants == null) &&
                        (cal.strValue == null) &&
                        (cal.numnCost != null) &&
                        (
                        (cal.intnHours == null) &&
                        (cal.intnMinutes == null) &&
                        (cal.intnSeconds == null)
                        )
                    ).ToList();
                    }
                    else
                    {
                        //                                  //Return Nothing.
                        darrcalFinal = new List<CalCalculation>();
                    }
                }
                else if (
                    //                                      //By Product
                    (boolnByProduct_I == true) &&
                    (boolnByProcess_I != true) &&
                    (boolnByResource_I != true) &&
                    (boolnByIntent_I != true)
                    )
                {
                    darrcalFinal = darrcal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerQuantity) &&
                        (cal.strByX == CalCalculation.strByProduct) &&
                        (cal.intnPkProcessElementBelongsTo == null) &&
                        (cal.intnPkResourceElementBelongsTo == null) &&
                        (cal.strAscendants == null) &&
                        (cal.strValue == null)
                    ).ToList();
                }
                else if (
                    //                                      //By Intent
                    (boolnByIntent_I == true) &&
                    (boolnByProduct_I != true) &&
                    (boolnByProcess_I != true) &&
                    (boolnByResource_I != true)
                    )
                {
                    darrcalFinal = darrcal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerQuantity) &&
                        (cal.intnPkProcessElementBelongsTo == null) &&
                        (cal.intnPkResourceElementBelongsTo == null) &&
                        (cal.strAscendants != null) &&
                        (cal.strValue != null) &&
                        (cal.strByX == CalCalculation.strByIntent)
                    ).ToList();
                }
                /*END-CASE*/

                List<CaljsonCalculationJson> darrcaljsonWorkflow = new List<CaljsonCalculationJson>();
                List<CaljsonCalculationJson> darrcaljsonProduct = new List<CaljsonCalculationJson>();

                //darrcalFinal = darrcalFinal.Where(a => a.intnPkWorkflowBelongsTo == 4555 &&
                //    a.intnProcessInWorkflowId == 3).ToList();

                //                                          //Create the json to send to the front.
                foreach (CalCalculation cal in darrcalFinal)
                {
                    String strUnitI = cal.strUnit;
                    if (
                        cal.intnPkResourceElementBelongsTo != null
                        )
                    {
                        //                                  //Get the current unit of measurement.
                        ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(
                            (int)cal.intnPkResourceElementBelongsTo);
                        if (
                            valentity != null
                            )
                        {
                            strUnitI = valentity.strValue;
                        }
                    }

                    String strUnitO = null;
                    if (
                        cal.intnPkQFromResourceElementBelongsTo != null
                        )
                    {
                        //                                  //Get the current unit of measurement.
                        ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(
                            (int)cal.intnPkQFromResourceElementBelongsTo);
                        if (
                            valentity != null
                            )
                        {
                            strUnitO = valentity.strValue;
                        }
                    }
                    else
                    {
                        //                                  //Get unit from job.
                        strUnitO = (prodtyp != null) ? prodtyp.strCategory : null;
                    }

                    String strProcessName = null;
                    if (
                        cal.intnPkProcessElementBelongsTo != null
                        )
                    {
                        EleentityElementEntityDB eleentityProcess = context.Element.FirstOrDefault(et =>
                            et.intPk == cal.intnPkProcessElementBelongsTo);

                        strProcessName = eleentityProcess.strElementName;
                    }

                    //                                      //Get resource name.
                    String strResourceName = null;
                    if (
                        cal.intnPkResourceElementBelongsTo != null
                        )
                    {
                        ResResource res = ResResource.resFromDB(cal.intnPkResourceElementBelongsTo, false);
                        //strResourceName = res.strName;
                        String strResName = ResResource.strGetMediaResourceName(res.intPk);
                        strResourceName = strResName;
                    }

                    //                                      //Get resource name.
                    String strQtyFromResourceName = null;
                    if (
                        cal.intnPkResourceElementBelongsTo != null
                        )
                    {
                        ResResource resQtyFrom = ResResource.resFromDB(cal.intnPkQFromResourceElementBelongsTo, false);
                        strQtyFromResourceName = resQtyFrom.strName;

                        if (
                            cal.boolnByArea == true
                            )
                        {
                            strUnitO = CalCalculation.strGetWidthUnit(resQtyFrom.intPk) + "²";
                        }
                    }

                    CaljsonCalculationJson caljson = new CaljsonCalculationJson();
                    caljson.intPk = cal.intPk;
                    caljson.numnQuantity = cal.numnQuantity;
                    caljson.numnNeeded = cal.numnNeeded != null ? (double?)(((double)cal.numnNeeded).Round(2)) : null;
                    caljson.numnPerUnits = cal.numnPerUnits != null ? 
                        (double?)(((double)cal.numnPerUnits).Round(2)) : null;
                    caljson.numnMin = cal.numnMin;
                    caljson.numnQuantityWaste = cal.numnQuantityWaste;
                    caljson.numnPercentWaste = cal.numnPercentWaste;
                    caljson.numnBlock = cal.numnBlock;
                    caljson.numnCost = cal.numnCost;
                    caljson.intnHours = cal.intnHours;
                    caljson.intnMinutes = cal.intnMinutes;
                    caljson.intnSeconds = cal.intnSeconds;
                    caljson.strDescription = cal.strDescription;
                    caljson.intnPkProcess = cal.intnPkProcessElementBelongsTo;
                    caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                    caljson.boolIsEnable = cal.boolIsEnable;
                    caljson.strCalculationType = cal.strCalculationType;
                    caljson.arrAscendantName = Tools.arrstrAscendantName(cal.strAscendants);
                    caljson.strValue = cal.strValue;
                    caljson.intnGroupId = cal.intnGroup;
                    caljson.strBy = cal.strByX;
                    caljson.strProcessName = strProcessName;
                    caljson.intnPkResourceI = cal.intnPkResourceElementBelongsTo;
                    caljson.strUnitI = strUnitI;
                    caljson.intnPkResourceO = cal.intnPkQFromResourceElementBelongsTo;
                    caljson.strUnitO = strUnitO;
                    caljson.strResourceName = strResourceName;
                    caljson.strQtyFromResourceName = strQtyFromResourceName;
                    caljson.boolIsEditable = true;
                    caljson.boolFromThickness = cal.boolnFromThickness == true ? true : false;
                    caljson.boolIsBlock = cal.boolnIsBlock == true ? true : false;

                    //                                      //Check if the calculation has conditions
                    GpcondjsonGroupConditionJson gpcondjson = Tools.gpcondjsonGetCondition(cal.intPk, null, null, null);
                    if (
                        gpcondjson != null
                        )
                    {
                        caljson.boolHasCondition = true;
                    }

                    if (
                        //                                  //There is an associated account 
                        cal.intnPkAccount != null
                        )
                    {
                        //                                  //Find account name
                        caljson.strAccountName = context.Account.FirstOrDefault(acc =>
                            acc.intPk == cal.intnPkAccount).strNumber;
                    }

                    if (
                        //                                  //It is calculation process added from the workflow.
                        cal.intnPkWorkflowBelongsTo != null
                        )
                    {
                        caljson.boolnIsWorkflow = true;

                        PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                            piw => piw.intPkWorkflow == cal.intnPkWorkflowBelongsTo &&
                            piw.intProcessInWorkflowId == cal.intnProcessInWorkflowId);

                        caljson.boolIsInPostProcess = piwentity.boolIsPostProcess;

                        EleentityElementEntityDB eleentityProcessInWorkflow = context.Element.FirstOrDefault(ele =>
                                ele.intPk == piwentity.intPkProcess);

                        caljson.strProcessName = eleentityProcessInWorkflow.strElementName;
                        if (
                            piwentity.intnId != null
                            )
                        {
                            caljson.strProcessName = eleentityProcessInWorkflow.strElementName + " (" +
                                piwentity.intnId + ")";
                        }

                        //caljson.boolIsEditable = CalCalculation.boolIsEditable(cal.intnPkResourceElementBelongsTo,
                        //    cal.intnPkElementElementTypeBelongsTo, cal.intnPkElementElementBelongsTo,
                        //    cal.intnPkQFromResourceElementBelongsTo, cal.intnPkQFromElementElementTypeBelongsTo,
                        //    cal.intnPkQFromElementElementBelongsTo, configuration_I, intnJobId_I, strPrintshopId_I,
                        //    piwentity.intPk, ref intStatus_IO, ref strUserMessage_IO,
                        //    ref strDevMessage_IO);

                        if (
                            //                              //Calculation belongs to normal piw.
                            !piwentity.boolIsPostProcess ||
                            //                              //Calculation belongst to post piw and it is associated to
                            //                              //      an input without link or it is a process 
                            //                              //      calculation.
                            CalCalculation.boolCalculationIsFromInputWithoutLinkOrFromProcess(cal)
                            )
                        {
                            darrcaljsonWorkflow.Add(caljson);
                        }
                    }
                    else
                    {
                        //                                  //It is calculation added from the product.
                        caljson.boolnIsWorkflow = false;

                        darrcaljsonProduct.Add(caljson);
                    }
                }

                darrcaljsonProduct = darrcaljsonProduct.OrderBy(cal => cal.strDescription).ToList();
                darrcaljsonWorkflow = darrcaljsonWorkflow.OrderBy(cal => cal.strDescription).ToList();
                arrcaljson = darrcaljsonWorkflow.Concat(darrcaljsonProduct).ToArray();

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }

            return arrcaljson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsEditable(
            int? intnPkResourceElementBelongsTo_I,
            int? intnPkElementElementTypeBelongsTo_I,
            int? intnPkElementElementBelongsTo_I,
            int? intnPkQFromResourceElementBelongsTo_I,
            int? intnPkQFromElementElementTypeBelongsTo_I,
            int? intnPkQFromElementElementBelongsTo_I,
            IConfiguration configuration_I,
            int? intnJobId_I,
            String strPrintshopId_I,
            int intPkProcesInWorkflow_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {

            bool boolIsEditable = true;

            if (
                (intnPkResourceElementBelongsTo_I != null &&
                (intnPkElementElementTypeBelongsTo_I != null ||
                intnPkElementElementBelongsTo_I != null) &&
                intnPkQFromResourceElementBelongsTo_I != null &&
                (intnPkQFromElementElementTypeBelongsTo_I != null ||
                intnPkQFromElementElementBelongsTo_I != null))
                )
            {
                int intPkEleetOrEleele = (int)(intnPkElementElementTypeBelongsTo_I != null ?
                    intnPkElementElementTypeBelongsTo_I : intnPkElementElementBelongsTo_I);
                bool boolIsEleet = intnPkElementElementTypeBelongsTo_I != null ? true : false;
                int intPkResource = (int)intnPkResourceElementBelongsTo_I;

                int intPkEleetOrEleeleQFrom = (int)(intnPkQFromElementElementTypeBelongsTo_I != null ?
                    intnPkQFromElementElementTypeBelongsTo_I : intnPkQFromElementElementBelongsTo_I);
                bool boolIsEleetQFrom = intnPkQFromElementElementTypeBelongsTo_I != null ? true : false;
                int intPkResourceQFrom = (int)intnPkQFromResourceElementBelongsTo_I;

                //                                  //List Of process inputs.
                List<IofrmpiwjsonIOFromPIWJson> darrioinfrmpiwjsonIosFromPIW = new List<IofrmpiwjsonIOFromPIWJson>();

                ProdtypProductType.subGetProcessInputs(intnJobId_I, strPrintshopId_I, intPkProcesInWorkflow_I,
                configuration_I, intPkEleetOrEleele, boolIsEleet, intPkResource, out darrioinfrmpiwjsonIosFromPIW,
                ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                if (
                    //                              //Valid that res exist in qFrom list.
                    darrioinfrmpiwjsonIosFromPIW.Exists(io =>
                    io.boolIsEleet == boolIsEleetQFrom &&
                    io.intnPkEleetOrEleele == intPkEleetOrEleeleQFrom &&
                    io.intnPkResource == intPkResourceQFrom)
                    )
                {
                    boolIsEditable = true;
                }
                else
                {
                    boolIsEditable = false;
                }
            }
            return boolIsEditable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolCalculationIsFromInputWithoutLinkOrFromProcess(
            //                                              //Verify that cal is associated to an input without link
            //                                              //      or from a process.

            CalCalculation cal_I
            )
        {
            bool boolIsAssociated = false;

            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            if (
                //                                          //IO calculation.
                cal_I.intnPkElementElementTypeBelongsTo != null ||
                cal_I.intnPkElementElementBelongsTo != null
                )
            {
                bool boolIsInput = false;
                if (
                    cal_I.intnPkElementElementTypeBelongsTo != null
                    )
                {
                    boolIsInput = context.ElementElementType.FirstOrDefault(eleet =>
                        eleet.intPk == cal_I.intnPkElementElementTypeBelongsTo).boolUsage;
                }
                else
                {
                    boolIsInput = context.ElementElement.FirstOrDefault(eleele =>
                        eleele.intPk == cal_I.intnPkElementElementBelongsTo).boolUsage;
                }

                if (
                    boolIsInput
                    )
                {
                    //                                      //Get the io.
                    IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                        io.intPkWorkflow == cal_I.intnPkWorkflowBelongsTo &&
                        io.intnProcessInWorkflowId == cal_I.intnProcessInWorkflowId &&
                        io.intnPkElementElementType == cal_I.intnPkElementElementTypeBelongsTo &&
                        io.intnPkElementElement == cal_I.intnPkElementElementBelongsTo_Z);

                    if (
                        //                                  //It is not in db.
                        (ioentity == null) ||
                        //                                  //It is in db but it has not link.
                        ((ioentity != null) && (ioentity.strLink == null))
                        )
                    {
                        boolIsAssociated = true;
                    }
                }
            }
            else if (
                //                                          //Process calculation.
                cal_I.intnPkWorkflowBelongsTo != null &&
                cal_I.intnPkProcessElementBelongsTo != null
                )
            {
                boolIsAssociated = true;
            }
            else
            {
                //                                          //Do nothing.
            }

            return boolIsAssociated;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<CalCalculation> GetJobCalculation(
            JobentityJobEntityDB jobentity_I,
            List<CalCalculation> darrcal_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Declare calculation list to return.
            List<CalCalculation> darrcalFinal = new List<CalCalculation>();

            if (
                jobentity_I != null
                )
            {
                //                                          //Get job's date.
                ZonedTime ztimeJobDate = ZonedTimeTools.NewZonedTime(jobentity_I.strStartDate.ParseToDate(),
                        jobentity_I.strStartTime.ParseToTime());

                //                                          //Check calculations that apply.
                foreach (CalCalculation calToCheck in darrcal_I)
                {
                    /*CASE*/
                    if (
                        //                                  //Calculation still able.
                        calToCheck.strEndDate == null
                        )
                    {
                        //                                  //Get calculation's startdate.
                        ZonedTime ztimeCalStartDate = ZonedTimeTools.NewZonedTime(calToCheck.strStartDate.ParseToDate(),
                                calToCheck.strStartTime.ParseToTime());
                        if (
                            //                              //Calculation apply to the job.
                            ztimeJobDate >= ztimeCalStartDate
                            )
                        {
                            //                              //Add calculation.
                            darrcalFinal.Add(calToCheck);
                        }
                    }
                    else if (
                       //                                  //Calculation was edited or deleted.
                       calToCheck.strEndDate != null
                       )
                    {
                        //                                  //Get calculation's startdate and enddate
                        ZonedTime ztimeCalStartDate = ZonedTimeTools.NewZonedTime(calToCheck.strStartDate.ParseToDate(),
                                calToCheck.strStartTime.ParseToTime());
                        ZonedTime ztimecalEndDate = ZonedTimeTools.NewZonedTime(calToCheck.strEndDate.ParseToDate(),
                                calToCheck.strEndTime.ParseToTime());
                        if (
                            //                              //Calculation apply to the job.
                            ztimeJobDate >= ztimeCalStartDate &&
                            ztimeJobDate < ztimecalEndDate
                            )
                        {
                            //                              //Add calculation.
                            darrcalFinal.Add(calToCheck);
                        }
                    }
                }
            }
            return darrcalFinal;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CaljsonCalculationJson[] arrcaljsonGetResourceDefaults(
            String strPrintshopId_I
            )
        {
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
            CalCalculation[] arrcal = ps.diccalResource.Values.ToArray();

            List<CaljsonCalculationJson> darrcaljson = new List<CaljsonCalculationJson>();

            foreach (CalCalculation cal in arrcal)
            {
                //                                          //Get resource name.
                String strResourceName = null;
                if (
                    cal.intnPkResourceElementBelongsTo != null
                    )
                {
                    ResResource res = ResResource.resFromDB(cal.intnPkResourceElementBelongsTo, false);
                    strResourceName = res.strName;
                }

                //                                          //Get resource name.
                String strQtyFromResourceName = null;
                if (
                    cal.intnPkResourceElementBelongsTo != null
                    )
                {
                    ResResource resQtyFrom = ResResource.resFromDB(cal.intnPkQFromResourceElementBelongsTo, false);
                    strQtyFromResourceName = resQtyFrom.strName;
                }

                CaljsonCalculationJson caljson = new CaljsonCalculationJson();
                caljson.intPk = cal.intPk;
                caljson.strUnitI = cal.strUnit;
                caljson.numnQuantity = cal.numnQuantity;
                caljson.numnNeeded = cal.numnNeeded;
                caljson.numnPerUnits = cal.numnPerUnits;
                caljson.numnCost = cal.numnCost;
                caljson.arrAscendantName = Tools.arrstrAscendantName(cal.strAscendants);
                caljson.numnMin = cal.numnMin;
                caljson.strValue = cal.strValue;
                caljson.numnProfit = cal.numnProfit;
                caljson.strDescription = cal.strDescription;
                caljson.numnBlock = cal.numnBlock;
                caljson.boolIsEnable = cal.boolIsEnable;
                caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                caljson.intnPkProcess = cal.intnPkProcessElementBelongsTo;
                caljson.intnPkResourceI = cal.intnPkResourceElementBelongsTo;
                caljson.strCalculationType = cal.strCalculationType;
                caljson.strBy = cal.strByX;
                caljson.strResourceName = strResourceName;
                caljson.strQtyFromResourceName = strQtyFromResourceName;

                darrcaljson.Add(caljson);
            }

            return darrcaljson.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOneData(
            int intPk_I,
            out CaljsonCalculationJson caljson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            caljson_O = new CaljsonCalculationJson();

            CalCalculation cal = CalCalculation.calGetFromDb(intPk_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Calculation not found.";
            if (
                cal != null
                )
            {
                CaljsonCalculationJson caljson = new CaljsonCalculationJson();
                caljson.intPk = cal.intPk;
                caljson.strValue = cal.strValue;
                caljson.arrAscendantName = Tools.arrstrAscendantName(cal.strAscendants);
                caljson.numnProfit = cal.numnProfit;
                caljson.strDescription = cal.strDescription;
                caljson.boolIsEnable = cal.boolIsEnable;
                caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                caljson.intnPkProcess = cal.intnPkProcessElementBelongsTo;
                caljson.strCalculationType = cal.strCalculationType;
                caljson.strBy = cal.strByX;
                caljson.arrintAscendantPk = Tools.arrintElementPk(cal.strAscendants);
                caljson.numnNeeded = cal.numnNeeded;
                caljson.numnPerUnits = cal.numnPerUnits;
                caljson.numnQuantity = cal.numnQuantity;
                caljson.numnCost = cal.numnCost;
                caljson.intnHours = cal.intnHours;
                caljson.intnMinutes = cal.intnMinutes;
                caljson.intnSeconds = cal.intnSeconds;
                caljson.numnMin = cal.numnMin;
                caljson.numnQuantityWaste = cal.numnQuantityWaste;
                caljson.numnPercentWaste = cal.numnPercentWaste;
                caljson.numnBlock = cal.numnBlock;
                caljson.intnPkResourceI = cal.intnPkResourceElementBelongsTo;
                caljson.strUnitI = cal.strUnit;
                caljson.intnPkResourceO = cal.intnPkQFromResourceElementBelongsTo;
                caljson.boolFromThickness = cal.boolnFromThickness == true ? true : false;
                caljson.boolIsBlock = cal.boolnIsBlock == true ? true : false;
                caljson.boolnByArea = cal.boolnByArea;

                Odyssey2Context context = new Odyssey2Context();

                if (
                    cal.intnPkAccount != null
                    )
                {
                    AccentityAccountEntityDB accentity = context.Account.FirstOrDefault(acc =>
                        acc.intPk == cal.intnPkAccount);

                    caljson.strAccountName = accentity.strNumber;
                    caljson.intnPkAccount = accentity.intPk;
                }

                //                                          //EleetorEleele.
                if (
                    cal.intnPkElementElementTypeBelongsTo != null
                    )
                {
                    caljson.intnPkEleetOrEleeleI = cal.intnPkElementElementTypeBelongsTo;
                    caljson.boolnIsEleetI = true;
                }
                else
                {
                    caljson.intnPkEleetOrEleeleI = cal.intnPkElementElementBelongsTo;
                    caljson.boolnIsEleetI = false;
                }

                //                                          //Find paper transformation.
                PatransPaperTransformationEntityDB patransentity = context.PaperTransformation.FirstOrDefault(
                    paper => paper.intnPkCalculationOwn == intPk_I && paper.boolTemporary == false);

                if (
                    patransentity != null
                    )
                {
                    caljson.intnPkPaTrans = patransentity.intPk;
                }
                //                                          //Get PkProcessInWorkflow from calculation.
                caljson.intnPkProcessInWorkflow = CalCalculation.intnGetCalculationPkProcessInWorkflow(
                    cal.intnPkWorkflowBelongsTo, cal.intnProcessInWorkflowId);

                if (
                    cal.intnPkResourceElementBelongsTo != null
                    )
                {
                    //                                      //Get the cost if it exists.                    
                    //                                      //Get the cost.
                    ResResource resGetCost = ResResource.resFromDB((int)cal.intnPkResourceElementBelongsTo,
                                false);
                    //                                      //Create ztime object with data from calculation in order
                    //                                      //to get the calculation that fit with dates.
                    ZonedTime ztimeCalculationDate = ZonedTimeTools.NewZonedTime(cal.strStartDate.ParseToDate(),
                            cal.strStartTime.ParseToTime());
                    CostentityCostEntityDB costentity = resGetCost.GetCostDependingDate(ztimeCalculationDate);
                    if (
                        costentity != null
                        )
                    {
                        caljson.numnQuantity = costentity.numnQuantity;
                        caljson.numnCost = costentity.numnCost;
                        caljson.numnMin = costentity.numnMin;
                        caljson.numnBlock = costentity.numnBlock;
                    }

                    //                                      //Find QFrom resource.
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(
                        ele => ele.intPk == cal.intnPkResourceElementBelongsTo);

                    caljson.strUnitI = ProdtypProductType.strUnitFromEleentityResource(eleentity);
                }

                //                                          //If there is a Quantity From Resource
                if (
                    cal.intnPkQFromResourceElementBelongsTo != null
                    )
                {
                    //                                      //Find QFrom resource.
                    EleentityElementEntityDB eleentityqfrom = context.Element.FirstOrDefault(ele =>
                        ele.intPk == cal.intnPkQFromResourceElementBelongsTo);

                    String strQFromResName = ResResource.strGetMediaResourceName(eleentityqfrom.intPk);

                    //                                      //Find QFrom resource type.
                    EtElementTypeAbstract eletqfrom = EletemElementType.etFromDB(eleentityqfrom.intPkElementType);

                    //                                      //QFrom unit of measurement.
                    caljson.strUnitO = ProdtypProductType.strUnitFromEleentityResource(eleentityqfrom);

                    //                                      //Find resource type.
                    RestypResourceType restypeResourceType =
                        (RestypResourceType)EtElementTypeAbstract.etFromDB(eleentityqfrom.intPkElementType);

                    if (
                            cal.boolnByArea == true
                        )
                    {
                        caljson.strAreaUnitO = CalCalculation.strGetWidthUnit((int)cal.intnPkQFromResourceElementBelongsTo) + "²";
                    }

                    //                                      //QFrom Resource is ElementElementType
                    if (
                        cal.intnPkQFromElementElementTypeBelongsTo != null
                        )
                    {
                        caljson.intnPkEleetOrEleeleO = cal.intnPkQFromElementElementTypeBelongsTo;
                        caljson.boolnIsEleetO = true;

                        //                                  //Type and resource concatenation.
                        caljson.strTypeTemplateAndResourceO = "(" + eletqfrom.strXJDFTypeId + ") " + strQFromResName;
                    }
                    //                                      //QFrom Resource is ElementElement
                    else
                    {
                        caljson.intnPkEleetOrEleeleO = cal.intnPkQFromElementElementBelongsTo;
                        caljson.boolnIsEleetO = false;

                        EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(
                            eleele => eleele.intPk == cal.intnPkElementElementBelongsTo);

                        //                                  //Find template of QFrom resource.
                        EleentityElementEntityDB eleentitytemplate = context.Element.FirstOrDefault(ele =>
                            ele.intPk == eleeleentity.intPkElementSon);

                        //                                  //Type, template and resource concatenation.
                        caljson.strTypeTemplateAndResourceO = "(" + eletqfrom.strXJDFTypeId + " : " +
                            eleentitytemplate.strElementName + ") " + strQFromResName;
                    }
                }
                else
                {
                    if (
                        cal.strByX == CalCalculation.strByResource
                        )
                    {
                        ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(
                            cal.intnPkProductTypeBelongsTo);

                        caljson.strTypeTemplateAndResourceO = "(Job Quantity) " + prodtyp.strCustomTypeId;

                        //                                      //Job unit of measurement.
                        caljson.strUnitO = prodtyp.strCategory;
                    }
                }

                caljson_O = caljson;
                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static int? intnGetCalculationPkProcessInWorkflow(
            //                                              //Find PkProcessInWorkflow that belongs to a specific 
            //                                              //      calculation using PkWorkflow and ProcessInWorkfloId

            int? intnPkWorkflow_I,
            int? intnProcessInWorkflowId_I
            )
        {
            int? intnPkProcessInWorkflow = null;

            if (
                intnPkWorkflow_I != null &&
                intnProcessInWorkflowId_I != null
                )
            {
                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Get pk.

                //                                          //Find process in workflow.
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPkWorkflow == (int)intnPkWorkflow_I &&
                    piw.intProcessInWorkflowId == (int)intnProcessInWorkflowId_I);

                if (
                    piwentity != null
                    )
                {
                    intnPkProcessInWorkflow = piwentity.intPk;
                }
            }

            return intnPkProcessInWorkflow;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAttributesAndValues(
            int intPkProduct_I,
            IConfiguration configuration_I,
            out List<Attrjson2AttributeJson2> darrattrjson2_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List of Jsons to return.
            darrattrjson2_O = new List<Attrjson2AttributeJson2>();

            //                                              //Find element type.
            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intPkProduct_I);
            ProdtypProductType prodtyp = (ProdtypProductType)et;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Element type not found.";
            if (
                prodtyp != null
                )
            {
                Dictionary<int, AttrAttribute> dicattr = prodtyp.dicattr;
                AttrAttribute[] arrattr = new AttrAttribute[dicattr.Count];
                dicattr.Values.CopyTo(arrattr, 0);

                /*WHILE DO*/
                int intI = 0;
                while (
                    //                                      //Take each attribute.
                    (intI < arrattr.Length)
                    )
                {
                    List<String> darrstrvaluesByAttr = new List<string>();
                    foreach (ValjsonValueJson arrstrValues in arrattr[intI].arrstrValues)
                    {
                        //                                  //Get each value from attribute.
                        darrstrvaluesByAttr.Add(arrstrValues.strValue);
                    }
                    Attrjson2AttributeJson2 attrjson2 = new Attrjson2AttributeJson2
                    {
                        strCustomName = arrattr[intI].strCustomName,
                        intnPk = arrattr[intI].intPk,
                        arrstrValues = darrstrvaluesByAttr.ToArray()
                    };
                    darrattrjson2_O.Add(attrjson2);

                    intI = intI + 1;
                }

                //                                          //Create the Quantity attribute.
                Attrjson2AttributeJson2 attrjson2Qty = new Attrjson2AttributeJson2()
                {
                    strCustomName = "Quantity",
                    intnPk = null
                };

                darrattrjson2_O.Add(attrjson2Qty);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetValuesForAnAttribute(
            int intPkAttribute_I,
            IConfiguration configuration_I,
            out List<ValjsonValueJson> darrvaljson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //List of Jsons to return.
            darrvaljson_O = new List<ValjsonValueJson>();

            //                                              //Find attribute in DB.
            AttrAttribute attr = AttrAttribute.attrFromDB(intPkAttribute_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Attribute not found.";
            if (
                attr != null
                )
            {
                String strAttributeId = attr.intWebsiteElementId + "";

                //                                              //Get data from Wisnet.
                String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                    GetSection("Odyssey2Settings")["urlWisnetApi"];

                Task<List<ValjsonValueJson>> Task_darrvaljson = HttpTools<ValjsonValueJson>.
                    GetListAsyncToEndPoint(strUrlWisnet + "/PrintShopData/attributeValues/" + strAttributeId);

                Task_darrvaljson.Wait();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Wisnet database connection lost.";
                if (
                    Task_darrvaljson.Result != null
                    )
                {
                    darrvaljson_O = Task_darrvaljson.Result;

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOneTransformCalculation(
            //                                              //Get one tranform calculation from DB using a specific pk.

            int intnPk_I,
            //                                              //Json to return.
            out TranscaljsonTransformCalculationJson transcaljson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            transcaljson_O = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find transform calculation.
            TrfcalentityTransformCalculationEntityDB trfcalentity = context.TransformCalculation.FirstOrDefault(
                transform => transform.intPk == intnPk_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Calculation not found.";
            if (
                trfcalentity != null
                )
            {
                //                                          //INPUT RESOURCE
                //                                          //Find input resource.
                EleentityElementEntityDB eleentityInput = context.Element.FirstOrDefault(element =>
                    element.intPk == trfcalentity.intPkResourceI);

                String strInputResName = eleentityInput.strElementName;

                //                                          //Input unit of measurement.
                String strUnitInput = ProdtypProductType.strUnitFromEleentityResource(eleentityInput);

                //                                          //Find input resourceI type.
                EtElementTypeAbstract eletInput = EletemElementType.etFromDB(eleentityInput.intPkElementType);

                String strTypeTemplateAndResourceInput;
                //                                          //To easy code.
                int intPkEleetOrEleeleI;
                bool boolIsEleetI = false;
                if (
                    trfcalentity.intnPkElementElementTypeI != null
                    )
                {
                    intPkEleetOrEleeleI = (int)trfcalentity.intnPkElementElementTypeI;
                    boolIsEleetI = true;

                    //                                      //Type and resource concatenation.
                    strTypeTemplateAndResourceInput = "(" + eletInput.strXJDFTypeId + ") " + strInputResName;
                }
                else
                {
                    intPkEleetOrEleeleI = (int)trfcalentity.intnPkElementElementI;
                    EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(
                        eleele => eleele.intPk == intPkEleetOrEleeleI);

                    //                                      //Find template of resourceI.
                    EleentityElementEntityDB eleentitytemplate = context.Element.FirstOrDefault(ele =>
                        ele.intPk == eleeleentity.intPkElementSon);

                    //                                      //Type, template and resource concatenation.
                    strTypeTemplateAndResourceInput = "(" + eletInput.strXJDFTypeId + " : " +
                        eleentitytemplate.strElementName + ") " + strInputResName;
                }

                //                                          //OUTPUT RESOURCE
                //                                          //Find output resource.
                EleentityElementEntityDB eleentityOutput = context.Element.FirstOrDefault(element =>
                    element.intPk == trfcalentity.intPkResourceO);

                String strOutputResName = eleentityOutput.strElementName;

                //                                          //Output unit of measurement.
                String strUnitOutput = ProdtypProductType.strUnitFromEleentityResource(eleentityOutput);

                //                                          //Find output resource type.
                EtElementTypeAbstract eletOutput = EletemElementType.etFromDB(eleentityOutput.intPkElementType);

                String strTypeTemplateAndResourceOutput;
                //                                          //To easy code.
                int intPkEleetOrEleeleO;
                bool boolIsEleetO = false;
                if (
                    trfcalentity.intnPkElementElementTypeO != null
                    )
                {
                    intPkEleetOrEleeleO = (int)trfcalentity.intnPkElementElementTypeO;
                    boolIsEleetO = true;

                    //                                      //Type and resourceO concatenation.
                    strTypeTemplateAndResourceOutput = "(" + eletOutput.strXJDFTypeId + ") " + strOutputResName;
                }
                else
                {
                    intPkEleetOrEleeleO = (int)trfcalentity.intnPkElementElementO;
                    EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(
                        eleele => eleele.intPk == intPkEleetOrEleeleO);

                    //                                      //Find template of resourceO.
                    EleentityElementEntityDB eleentitytemplate = context.Element.FirstOrDefault(ele =>
                        ele.intPk == eleeleentity.intPkElementSon);

                    //                                      //Type, template and resource concatenation.
                    strTypeTemplateAndResourceOutput = "(" + eletOutput.strXJDFTypeId + " : " +
                        eleentitytemplate.strElementName + ") " + strOutputResName;
                }

                //                                          //Json object to return.
                transcaljson_O = new TranscaljsonTransformCalculationJson(
                    trfcalentity.intPk, trfcalentity.intPkProcessInWorkflow, trfcalentity.numNeeded,
                    trfcalentity.numPerUnits, strTypeTemplateAndResourceInput, intPkEleetOrEleeleI, boolIsEleetI,
                    trfcalentity.intPkResourceI, strUnitInput, intPkEleetOrEleeleO, boolIsEleetO,
                    trfcalentity.intPkResourceO, strUnitOutput, strTypeTemplateAndResourceOutput);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subfunCalculateCutsOrFoldedFactor(
            //                                              //Get the quantity of cuts or the factor of folded.

            int intPkResource_I,
            double numWidth_I,
            double? numnHeight_I,
            double numCutWidth_I,
            double numCutHeight_I,
            double? numnMarginTop_I,
            double? numnMarginBottom_I,
            double? numnMarginLeft_I,
            double? numnMarginRight_I,
            double? numnVerticalGap_I,
            double? numnHorizontalGap_I,
            bool boolIsOptimized_I,
            out CutdatajsonCutDataJson cutdatajson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Verify if the resource is shees or roll.
            Odyssey2Context context = new Odyssey2Context();

            double numNeeded = 1;
            ResResource res = ResResource.resFromDB(intPkResource_I, false);

            String strWidthUnit;
            String strUnitOfMeasurement;
            if (
                //                                          //It is a roll.
                res.subFunIsMediaTypeMediaUnitRoll(out strWidthUnit, out strUnitOfMeasurement)
                )
            {
                //                                          //Convert the widthCuts to unitOfMeasurement.
                numNeeded = CvtConvert.to(numCutHeight_I, strWidthUnit, strUnitOfMeasurement);
            }

            double numNewHeight = numnHeight_I == null ? numCutHeight_I : (double)numnHeight_I;

            CutdatajsonCutDataJson cutdatajsonOriginal;
            CalCalculation.subfunCalculateCutsOneGrainDirection(numWidth_I, numNewHeight, numCutWidth_I, numCutHeight_I,
                numnMarginTop_I, numnMarginBottom_I, numnMarginLeft_I, numnMarginRight_I, numnVerticalGap_I,
                numnHorizontalGap_I, out cutdatajsonOriginal, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            CutdatajsonCutDataJson cutdatajsonInverted = null;
            if (
                //                                          //Optimization true, we need to try with data inverted.
                boolIsOptimized_I
                )
            {
                CalCalculation.subfunCalculateCutsOneGrainDirection(numWidth_I, numNewHeight, numCutHeight_I, numCutWidth_I,
                    numnMarginTop_I, numnMarginBottom_I, numnMarginLeft_I, numnMarginRight_I, numnVerticalGap_I,
                    numnHorizontalGap_I, out cutdatajsonInverted, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
            }

            /*CASE*/
            if (
                //                                          //Not optimized.
                !boolIsOptimized_I
                )
            {
                cutdatajson_O = cutdatajsonOriginal;
            }
            else if (
                //                                          //Optimized but is already optimized as original.
                boolIsOptimized_I &&
                (cutdatajsonOriginal.numPerUnit >= cutdatajsonInverted.numPerUnit)
                )
            {
                cutdatajson_O = cutdatajsonOriginal;
            }
            else
            {
                //                                          //Optimized and the reversed is better.
                cutdatajson_O = cutdatajsonInverted;
                cutdatajson_O.boolIsReversed = true;
            }
            /*CASE*/

            if (
                cutdatajson_O.numPerUnit > 0
                )
            {
                cutdatajson_O.numNeeded = numNeeded;
                cutdatajson_O.numPerUnit = cutdatajson_O.numPerUnit;

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "Success.";
            }
            else
            {
                intStatus_IO = 300;
                strUserMessage_IO = "Original size is not large enough.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subfunCalculateCutsOneGrainDirection(
            double numWidth_I,
            double numHeight_I,
            double numCutWidth_I,
            double numCutHeight_I,
            double? numnMarginTop_I,
            double? numnMarginBottom_I,
            double? numnMarginLeft_I,
            double? numnMarginRight_I,
            double? numnVerticalGap_I,
            double? numnHorizontalGap_I,
            out CutdatajsonCutDataJson cutdatajson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To easy code.
            double numMarginTop_I = numnMarginTop_I == null ? 0 : (double)numnMarginTop_I;
            double numMarginBottom_I = numnMarginBottom_I == null ? 0 : (double)numnMarginBottom_I;
            double numMarginLeft_I = numnMarginLeft_I == null ? 0 : (double)numnMarginLeft_I;
            double numMarginRight_I = numnMarginRight_I == null ? 0 : (double)numnMarginRight_I;
            double numVerticalGap_I = numnVerticalGap_I == null ? 0 : (double)numnVerticalGap_I;
            double numHorizontalGap_I = numnHorizontalGap_I == null ? 0 : (double)numnHorizontalGap_I;

            //                                              //New smaller paper quantity.
            double numPerUnit = 0;

            //                                              //List of rows.
            List<RowcutdatajsonRowCutDataJson> darrrowcutdata = new List<RowcutdatajsonRowCutDataJson>();

            if (
                //                                          //All paper waste only one row.
                numHeight_I < (numMarginTop_I + numMarginBottom_I + numCutHeight_I)
                )
            {
                List<CellcutdatajsonCellDataJson> darrcellcutdatajsonAllWaste = new List<CellcutdatajsonCellDataJson>();
                bool boolIsWaste = true;
                CellcutdatajsonCellDataJson cellcutdatajson = new CellcutdatajsonCellDataJson(numWidth_I, boolIsWaste);
                darrcellcutdatajsonAllWaste.Add(cellcutdatajson);
                CellcutdatajsonCellDataJson[] arrcellcutdatajsonAllWaste = darrcellcutdatajsonAllWaste.ToArray();
                RowcutdatajsonRowCutDataJson rowcutdataAllWaste = new RowcutdatajsonRowCutDataJson(numHeight_I,
                    arrcellcutdatajsonAllWaste);
                darrrowcutdata.Add(rowcutdataAllWaste);
            }
            else
            {
                //                                          //More than one row.
                double numLeftVertical = numHeight_I;
                if (
                    //                                      //Top Margin row.
                    numMarginTop_I > 0
                    )
                {
                    List<CellcutdatajsonCellDataJson> darrcellcutdatajsonTopMargin = new List<CellcutdatajsonCellDataJson>();
                    bool boolIsWaste = true;
                    CellcutdatajsonCellDataJson cellcutdatajson = new CellcutdatajsonCellDataJson(numWidth_I, boolIsWaste);
                    darrcellcutdatajsonTopMargin.Add(cellcutdatajson);
                    CellcutdatajsonCellDataJson[] arrcellcutdatajsonTopMargin = darrcellcutdatajsonTopMargin.ToArray();
                    RowcutdatajsonRowCutDataJson rowcutdataTopMargin = new RowcutdatajsonRowCutDataJson(numMarginTop_I,
                        arrcellcutdatajsonTopMargin);
                    darrrowcutdata.Add(rowcutdataTopMargin);
                    numLeftVertical = numLeftVertical - numMarginTop_I;
                }

                //                                          //All other rows.
                /*WHILE-DO*/
                while (
                    //                                      //There still paper for another row.
                    numLeftVertical > 0
                    )
                {
                    if (
                        //                                  //Last row.
                        numLeftVertical < (numCutHeight_I + numMarginBottom_I)
                        )
                    {
                        List<CellcutdatajsonCellDataJson> darrcellcutdatajsonLastRow = new List<CellcutdatajsonCellDataJson>();
                        bool boolIsWaste = true;
                        CellcutdatajsonCellDataJson cellcutdatajson = new CellcutdatajsonCellDataJson(numWidth_I, boolIsWaste);
                        darrcellcutdatajsonLastRow.Add(cellcutdatajson);
                        CellcutdatajsonCellDataJson[] arrcellcutdatajsonLastRow = darrcellcutdatajsonLastRow.ToArray();
                        RowcutdatajsonRowCutDataJson rowcutdataLastRow = new RowcutdatajsonRowCutDataJson(numLeftVertical.Round(2),
                            arrcellcutdatajsonLastRow);
                        darrrowcutdata.Add(rowcutdataLastRow);
                        numLeftVertical = numLeftVertical - numLeftVertical;
                    }
                    else
                    {
                        //                                  //Non-waste row.
                        List<CellcutdatajsonCellDataJson> darrcellcutdatajson = new List<CellcutdatajsonCellDataJson>();

                        double numLeftHorizontal = numWidth_I;
                        double numHeight = numCutHeight_I;

                        if (
                            //                              //All row waste only one cell.
                            numWidth_I < (numMarginLeft_I + numMarginRight_I + numCutWidth_I)
                            )
                        {
                            bool boolIsWaste = true;
                            CellcutdatajsonCellDataJson cellcutdatajsonAllRowWaste = new CellcutdatajsonCellDataJson(
                                numWidth_I, boolIsWaste);
                            darrcellcutdatajson.Add(cellcutdatajsonAllRowWaste);
                        }
                        else
                        {
                            //                              //More than one cell.

                            if (
                                //                          //Left margin cell.
                                numMarginLeft_I > 0
                                )
                            {
                                bool boolIsWaste = true;
                                CellcutdatajsonCellDataJson cellcutdatajsonLeftMargin = new CellcutdatajsonCellDataJson(
                                    numMarginLeft_I, boolIsWaste);
                                darrcellcutdatajson.Add(cellcutdatajsonLeftMargin);
                                numLeftHorizontal = numLeftHorizontal - numMarginLeft_I;
                            }

                            int intCells = 0;
                            //                              //All other cells.
                            /*WHILE-DO*/
                            while (
                                //                          //New cell;
                                numLeftHorizontal > 0
                                )
                            {
                                if (
                                    //                      //Last cell.
                                    numLeftHorizontal < (numCutWidth_I + numMarginRight_I)
                                    )
                                {
                                    double numWidth = numLeftHorizontal.Round(2);
                                    bool boolIsWaste = true;

                                    CellcutdatajsonCellDataJson cellcutdatajsonLastCell = new CellcutdatajsonCellDataJson(numWidth,
                                        boolIsWaste);
                                    darrcellcutdatajson.Add(cellcutdatajsonLastCell);
                                    numLeftHorizontal = numLeftHorizontal - numLeftHorizontal;
                                }
                                else
                                {
                                    //                      //Non-Waste cell.
                                    double numWidth = numCutWidth_I;
                                    intCells++;
                                    bool boolIsWaste = false;
                                    CellcutdatajsonCellDataJson cellcutdatajsonNonWaste =
                                        new CellcutdatajsonCellDataJson(numWidth, boolIsWaste);
                                    darrcellcutdatajson.Add(cellcutdatajsonNonWaste);
                                    numLeftHorizontal = numLeftHorizontal - numWidth;

                                    //                      //Intermediate cell.
                                    if (
                                        (numHorizontalGap_I > 0) &&
                                        (numLeftHorizontal >= (numHorizontalGap_I + numMarginRight_I))
                                        )
                                    {
                                        boolIsWaste = true;
                                        CellcutdatajsonCellDataJson cellcutdatajsonWaste =
                                            new CellcutdatajsonCellDataJson(numHorizontalGap_I, boolIsWaste);
                                        darrcellcutdatajson.Add(cellcutdatajsonWaste);
                                        numLeftHorizontal = numLeftHorizontal - numHorizontalGap_I;
                                    }
                                }
                            }
                            numPerUnit = numPerUnit + intCells;
                        }

                        CellcutdatajsonCellDataJson[] arrcellcutdatajsonNonWaste = darrcellcutdatajson.ToArray();
                        RowcutdatajsonRowCutDataJson rowcutdataNonWaste = new RowcutdatajsonRowCutDataJson(numHeight,
                            arrcellcutdatajsonNonWaste);
                        darrrowcutdata.Add(rowcutdataNonWaste);
                        numLeftVertical = numLeftVertical - numHeight;

                        //                                  //Intermediate row.
                        if (
                            (numVerticalGap_I > 0) &&
                            (numLeftVertical >= (numVerticalGap_I + numMarginBottom_I))
                            )
                        {
                            List<CellcutdatajsonCellDataJson> darrcellcutdatajsonWaste = new List<CellcutdatajsonCellDataJson>();
                            CellcutdatajsonCellDataJson cellcutdatajson = new CellcutdatajsonCellDataJson(numWidth_I, true);
                            darrcellcutdatajsonWaste.Add(cellcutdatajson);
                            CellcutdatajsonCellDataJson[] arrcellcutdatajsonWaste = darrcellcutdatajsonWaste.ToArray();
                            RowcutdatajsonRowCutDataJson rowcutdataWaste = new RowcutdatajsonRowCutDataJson(numVerticalGap_I,
                                arrcellcutdatajsonWaste);
                            darrrowcutdata.Add(rowcutdataWaste);
                            numLeftVertical = numLeftVertical - numVerticalGap_I;
                        }
                    }
                }
            }

            cutdatajson_O = new CutdatajsonCutDataJson(numPerUnit, false, darrrowcutdata.ToArray());
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOnePaperTransformation(
            //                                              //Get one paper transformation from DB using a specific pk
            //                                              //      or an IO.

            //                                              //Especific paper transformation.
            int? intnPkPaTrans_I,
            //                                              //Input IO.
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            //                                              //Output IO.
            int intPkEleetOrEleeleO_I,
            bool boolIsEleetO_I,

            int intPkProcessInWorkflow_I,
            int intPkResource_I,
            int? intnJobId_I,
            IConfiguration configuration_I,
            PsPrintShop ps_I,
            //                                              //Json to return.
            out PatransjsonPaperTransformationJson patransjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            patransjson_O = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Entity found.
            PatransPaperTransformationEntityDB patransentity = null;

            bool boolInputIsChangeable = true;

            //                                              //Validate process in workflow.
            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == intPkProcessInWorkflow_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Piw not valid.";
            if (
                piwentity != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Job not valid.";
                JobjsonJobJson jobjson = new JobjsonJobJson();
                if (
                    (intnJobId_I != null &&
                    JobJob.boolIsValidJobId((int)intnJobId_I, ps_I.strPrintshopId, configuration_I,
                        out jobjson, ref strUserMessage_IO, ref strDevMessage_IO)) ||
                     intnJobId_I == null
                    )
                {
                    //                                      //Verify if is a component or media resource.

                    //                                      //Get resource entity.
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == intPkResource_I);

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Resource not found.";
                    if (
                        eleentity != null
                        )
                    {
                        //                                  //Find resource type.
                        RestypResourceType restypeResourceType =
                            (RestypResourceType)EtElementTypeAbstract.etFromDB(eleentity.intPkElementType);

                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Resource type not found.";
                        if (
                            restypeResourceType != null
                            )
                        {
                            bool boolIsMedia = restypeResourceType.strCustomTypeId == ResResource.strMedia;
                            bool boolIsComponent = restypeResourceType.strCustomTypeId == ResResource.strComponent;

                            intStatus_IO = 402;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Resource must be Paper or Component.";
                            if (
                                boolIsMedia ||
                                boolIsComponent
                                )
                            {
                                //                          //To know if the resource is a roll.
                                ResResource resResource = ResResource.resFromDB(intPkResource_I, false);
                                bool boolMediaRoll = resResource.boolMediaRoll();

                                //                          //To easy code.
                                IoentityInputsAndOutputsEntityDB ioentity = new IoentityInputsAndOutputsEntityDB();
                                if (
                                    boolIsEleetO_I
                                    )
                                {
                                    ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                                        io.intPkWorkflow == piwentity.intPkWorkflow &&
                                        io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                        io.intnPkElementElementType == intPkEleetOrEleeleO_I);
                                }
                                else
                                {
                                    ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                                        io.intPkWorkflow == piwentity.intPkWorkflow &&
                                        io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                        io.intnPkElementElement == intPkEleetOrEleeleO_I);
                                }

                                //                          //To know if the IO input is a postSize IO.
                                bool boolIsPostSize = CalCalculation.boolIsPostSize(intPkEleetOrEleele_I, boolIsEleet_I,
                                    piwentity, intnJobId_I, jobjson);

                                //                          //If resource is media, get dimensions from attributes.
                                double numWidth = 0.0;
                                double? numnHeight = 0.0;
                                String strDimUnit = "";
                                bool boolHasDimensions = false;
                                if (
                                    boolIsMedia
                                    )
                                {
                                    //                      //Verify if the resources has dimensiones setted.
                                    CalCalculation.subGetDimensionsValues(intPkResource_I, ref numWidth, ref numnHeight,
                                        ref strDimUnit, ref boolHasDimensions);
                                }

                                intStatus_IO = 404;
                                strUserMessage_IO = "This resource has not dimensions set. You can not add a paper " +
                                    "transformation.";
                                strDevMessage_IO = "";
                                if (
                                    (boolIsMedia && boolHasDimensions) ||
                                    boolIsComponent
                                    )
                                {
                                    //                      //A media resource never can be changed.
                                    boolInputIsChangeable = boolIsMedia ? false : true;

                                    //                      //If there is a job, get output dimensions from job.
                                    //                      //Just to be shown in the modal.
                                    double numWidthOForNewCal = 0.0;
                                    double? numnHeightOForNewCal = 0.0;
                                    String strDimesionsJobOriginalForNewCal = "";
                                    bool boolSizeFromJobIsCorrect = true;
                                    if (
                                        intnJobId_I != null &&
                                        ioentity.boolnSize == true
                                        )
                                    {
                                        //                  //The IO has set Size column, take size from job.
                                        CalCalculation.subGetWidthAndLengthOutput(jobjson, strDimUnit,
                                            ref numWidthOForNewCal, ref numnHeightOForNewCal,
                                            ref strDimesionsJobOriginalForNewCal, ref intStatus_IO,
                                            ref strUserMessage_IO, ref strDevMessage_IO);

                                        boolSizeFromJobIsCorrect = intStatus_IO == 200 ? true : false;
                                    }

                                    if (
                                        boolSizeFromJobIsCorrect
                                        )
                                    {
                                        strDimUnit = boolIsComponent ? strDimesionsJobOriginalForNewCal : strDimUnit;

                                        //                      //Create Paper Tranformation to return.
                                        patransjson_O = new PatransjsonPaperTransformationJson(0, numWidth, numnHeight,
                                            numWidthOForNewCal, (double)numnHeightOForNewCal, 0, 0, 0, 0, 0, 0, strDimUnit,
                                            boolInputIsChangeable, null, false, true, boolIsPostSize, true);

                                        //                      //Look for Paper Tranformation info.

                                        bool boolPaperTranformationInfoFound = false;
                                        int intPkPaperTransformation = 0;
                                        double numWidthI = 0;
                                        double? numnHeightI = 0;
                                        double numWidthO = 0;
                                        double? numnHeightO = 0;
                                        String strDimesionUnit = "";

                                        //                      //1. From input PkPaperTranformtaion to edit.
                                        //                      //2. From Propagation

                                        if (
                                            //                  //1. From input PkPaperTranformtaion to edit.
                                            //                  //Opened when editing a IO input calculation.
                                            intnPkPaTrans_I != null
                                            )
                                        {
                                            //                  //Find paper transformation.
                                            patransentity = context.PaperTransformation.FirstOrDefault(
                                                paper => paper.intPk == intnPkPaTrans_I);
                                            if (
                                                patransentity != null
                                                )
                                            {
                                                boolPaperTranformationInfoFound = true;
                                                numWidthI = patransentity.numWidthI; ;
                                                numnHeightI = patransentity.numnHeightI;
                                                numWidthO = patransentity.numWidthO; ;
                                                numnHeightO = patransentity.numHeightO;
                                                strDimesionUnit = patransentity.strUnit;
                                                intPkPaperTransformation = patransentity.intPk;
                                            }

                                            //                  //Find paper transformations that were propagated.
                                            List<PatransPaperTransformationEntityDB> darrpatransentityPropagated =
                                                context.PaperTransformation.Where(patrans =>
                                                patrans.intPkProcessInWorkflow == patransentity.intPkProcessInWorkflow &&
                                                patrans.intnPkElementElementTypeI == patransentity.intnPkElementElementTypeI &&
                                                patrans.intnPkElementElementI == patransentity.intnPkElementElementI &&
                                                patrans.intPkResourceI == patransentity.intPkResourceI &&
                                                patrans.intnPkCalculationOwn == null).ToList();

                                            if (
                                                darrpatransentityPropagated.Count == 1 &&
                                                darrpatransentityPropagated[0].numnHeightI == patransentity.numnHeightI &&
                                                darrpatransentityPropagated[0].numWidthI == patransentity.numWidthI
                                                )
                                            {
                                                boolInputIsChangeable = false;
                                            }
                                        }
                                        else
                                        {
                                            //                  //2. From Propagation
                                            //                  //Opened in an IO input with link, this means that
                                            //                  //      the IO has a temporary transformation that 
                                            //                  //      was propagated.

                                            int? intnPkElementElementType = null;
                                            int? intnPkElementElement = null;
                                            if (
                                                boolIsEleet_I == true
                                                )
                                            {
                                                intnPkElementElementType = intPkEleetOrEleele_I;
                                            }
                                            else
                                            {
                                                intnPkElementElement = intPkEleetOrEleele_I;
                                            }

                                            //                  //Find paper transformations.
                                            List<PatransPaperTransformationEntityDB> darrpatransentity =
                                                context.PaperTransformation.Where(
                                                paper => paper.intnPkElementElementI == intnPkElementElement &&
                                                paper.intnPkElementElementTypeI == intnPkElementElementType &&
                                                paper.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                                                paper.intPkResourceI == intPkResource_I &&
                                                paper.intnPkCalculationOwn == null &&
                                                paper.intnPkCalculationLink != null).ToList();

                                            if (
                                                //              //There is propagation info.
                                                darrpatransentity.Count > 0
                                                )
                                            {
                                                CalCalculation.GetDimensionsFromPropagation(intnJobId_I, boolMediaRoll,
                                                    jobjson, darrpatransentity, ref numWidthI, ref numnHeightI,
                                                    ref strDimesionUnit);

                                                boolInputIsChangeable = (numWidthI > 0) ? false : true;

                                                boolPaperTranformationInfoFound = true;
                                            }
                                        }

                                        //                      //Only if there are Paper Tranformation info was found.
                                        //                      //At the end, override the patransjson_O to return.

                                        intStatus_IO = 200;
                                        strUserMessage_IO = "Retrieving information.";
                                        strDevMessage_IO = "";
                                        if (
                                            boolPaperTranformationInfoFound
                                            )
                                        {
                                            intStatus_IO = 404;
                                            strUserMessage_IO = "";
                                            strDevMessage_IO = "";

                                            if (
                                                ioentity.boolnSize == true &&
                                                intnJobId_I != null
                                                )
                                            {
                                                String strDimesionsJobOriginalI = "";
                                                if (
                                                    //              //Can be 0 when is a propagation PT and the other size
                                                    //              // of the link is Size.
                                                    numWidthI == 0
                                                    )
                                                {
                                                    CalCalculation.subGetWidthAndLengthOutput(jobjson,
                                                        strDimesionUnit, ref numWidthI, ref numnHeightI,
                                                        ref strDimesionsJobOriginalI, ref intStatus_IO,
                                                        ref strUserMessage_IO, ref strDevMessage_IO);

                                                    boolSizeFromJobIsCorrect = intStatus_IO == 200 ? true : false;
                                                }

                                                String strDimesionsJobOriginalO = "";
                                                if (
                                                    //              //Can be 0 when output is Size.
                                                    numWidthO == 0
                                                    )
                                                {
                                                    CalCalculation.subGetWidthAndLengthOutput(jobjson,
                                                        strDimesionUnit, ref numWidthO, ref numnHeightO,
                                                        ref strDimesionsJobOriginalO,ref intStatus_IO,
                                                        ref strUserMessage_IO, ref strDevMessage_IO);

                                                    boolSizeFromJobIsCorrect = intStatus_IO == 200 ? true : false;
                                                }
                                            }


                                            if (
                                                boolSizeFromJobIsCorrect
                                                )
                                            {
                                                CutdatajsonCutDataJson cutdatajson = null;
                                                if (
                                                    //              //Not open from a propagated io.
                                                    intnPkPaTrans_I != null &&
                                                    //              //There is info to get the perunit.
                                                    numWidthO > 0
                                                    )
                                                {
                                                    //              //Calculate row array.

                                                    CalCalculation.subfunCalculateCutsOrFoldedFactor(intPkResource_I, numWidthI,
                                                        numnHeightI, numWidthO, (double)numnHeightO, patransentity.numnMarginTop,
                                                        patransentity.numnMarginBottom, patransentity.numnMarginLeft,
                                                        patransentity.numnMarginRight, patransentity.numnVerticalGap,
                                                        patransentity.numnHorizontalGap, patransentity.boolOptimized,
                                                        out cutdatajson, ref intStatus_IO, ref strUserMessage_IO,
                                                        ref strDevMessage_IO);

                                                    RowcutdatajsonRowCutDataJson[] arrrow = null;
                                                    bool boolIsReversed = false;
                                                    if (
                                                        cutdatajson != null
                                                        )
                                                    {
                                                        arrrow = cutdatajson.arrrow;
                                                        boolIsReversed = cutdatajson.boolIsReversed;
                                                    }

                                                    //                  //Json object to return.
                                                    patransjson_O = 
                                                        new PatransjsonPaperTransformationJson(patransentity.intPk,
                                                        numWidthI, numnHeightI, numWidthO, (double)numnHeightO,
                                                        patransentity.numnMarginTop, patransentity.numnMarginBottom,
                                                        patransentity.numnMarginLeft, patransentity.numnMarginRight,
                                                        patransentity.numnVerticalGap, patransentity.numnHorizontalGap,
                                                        patransentity.strUnit, boolInputIsChangeable, arrrow,
                                                        boolIsReversed, patransentity.boolOptimized, boolIsPostSize,
                                                        patransentity.boolCut);
                                                }
                                                else
                                                {
                                                    patransjson_O = new PatransjsonPaperTransformationJson(0,
                                                        numWidthI, numnHeightI, numWidthO, (double)numnHeightO, 0, 0,
                                                        0, 0, 0, 0, strDimesionUnit, boolInputIsChangeable, null,
                                                        false, true, boolIsPostSize, true);
                                                }
                                            }
                                        }

                                        //intStatus_IO = 200;
                                        //strUserMessage_IO = "";
                                        //strDevMessage_IO = "";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void GetDimensionsFromPropagation(

            int? intnJobId_I,
            bool boolMediaRoll_I,
            JobjsonJobJson jobjson_I,
            List<PatransPaperTransformationEntityDB> darrpatransentityPropagation_I,
            ref double numWidthI_IO,
            ref double? numnHeightI_IO,
            ref String strDimensionUnit_IO
            )
        {
            List<PatransPaperTransformationEntityDB> darrpatransentityApplyInput =
                    new List<PatransPaperTransformationEntityDB>();

            if (
                darrpatransentityPropagation_I.Count > 0
                )
            {
                if (
                    intnJobId_I != null
                    )
                {
                    ProdtypProductType.GetPaperTransformationPropagationApplyInput(darrpatransentityPropagation_I,
                        jobjson_I, out darrpatransentityApplyInput);
                }
                else
                {
                    darrpatransentityApplyInput.AddRange(darrpatransentityPropagation_I);
                }

                if (
                    darrpatransentityApplyInput.Count > 0
                    )
                {
                    //
                    double numWidthI = darrpatransentityApplyInput[0].numWidthI;
                    double? numnLengthI = darrpatransentityApplyInput[0].numnHeightI;
                    String strDimensionsUnit = darrpatransentityApplyInput[0].strUnit;

                    bool boolAllPaperTranformationApplySameDimensions = true;

                    int intI = 1;
                    /*WHILE*/
                    while
                        (
                        intI < darrpatransentityApplyInput.Count &&
                        boolAllPaperTranformationApplySameDimensions
                        )
                    {
                        double numWidthINext = darrpatransentityApplyInput[intI].numWidthI;
                        double? numnLengthINext = darrpatransentityApplyInput[intI].numnHeightI;
                        String strDimensionsUnitNext = darrpatransentityApplyInput[intI].strUnit;

                        if (
                            //                              //strDimensionsUnitNext same unit than original.
                            strDimensionsUnit == strDimensionsUnitNext
                            )
                        {
                            //                              //Do nothing.
                        }
                        else
                        {
                            //                              //strDimensionsUnitNext diferent unit than original.
                            //                              //Transform strDimensionsUnitNext to strDimensionsUnit.
                            if (
                                strDimensionsUnit == "cm"
                                )
                            {
                                //                          //Transform strDimensionsUnitNext to cm.
                                numWidthINext = numWidthINext * 2.54;
                                numnLengthINext = numnLengthINext * 2.54;
                            }
                            else
                            {
                                //                          //Transform strDimensionsUnitNext to in.
                                numWidthINext = numWidthINext / 2.54;
                                numnLengthINext = numnLengthINext / 2.54;
                            }
                        }

                        if (
                            (!boolMediaRoll_I) &&
                            ((Math.Abs(numWidthI - numWidthINext) <= Math.Abs(0.1)) &&
                            (Math.Abs((double)numnLengthI - (double)numnLengthINext) <= Math.Abs(0.1)))
                            )
                        {
                            //                              //Do nothing.
                        }
                        else
                        {
                            boolAllPaperTranformationApplySameDimensions = false;
                        }

                        intI = intI + 1;
                    }

                    if (
                        boolAllPaperTranformationApplySameDimensions
                        )
                    {
                        //                                  //All Paper Transformation that apply and Job
                        //                                  //      have the same dimensions.
                        numWidthI_IO = numWidthI;
                        numnHeightI_IO = numnLengthI;
                        strDimensionUnit_IO = strDimensionsUnit;
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static bool boolIsPostSize(
            //                                              //Receive and IO input and verify if is a postSize IO.

            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int? intnJobId_I,
            JobjsonJobJson jobjson_I
            )
        {
            bool boolIsPostSize = false;
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                              //To easy code.
            IoentityInputsAndOutputsEntityDB ioentity = new IoentityInputsAndOutputsEntityDB();
            if (
                boolIsEleet_I
                )
            {
                ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                    io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                    io.intnPkElementElementType == intPkEleetOrEleele_I);
            }
            else
            {
                ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                    io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                    io.intnPkElementElement == intPkEleetOrEleele_I);
            }

            //                                              //Verify if the IO input has link.
            if (
                ioentity != null && ioentity.strLink != null
                )
            {
                if (
                    intnJobId_I != null
                    )
                {
                    //                              //Get all the correct processes.
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                    List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                    ProdtypProductType.subGetWorkflowValidWay(piwentity_I.intPkWorkflow, jobjson_I,
                        out darrpiwentityAllProcesses, out darrdynlkjson);

                    int? intPkEleet = boolIsEleet_I ? intPkEleetOrEleele_I : (int?)null;
                    int? intPkEleele = boolIsEleet_I ? (int?)null : intPkEleetOrEleele_I;
                    String strLink = ioentity.strLink;
                    PiwentityProcessInWorkflowEntityDB piwentityO;
                    IoentityInputsAndOutputsEntityDB ioentityO;

                    ProdtypProductType.subGetOtherSideOfTheLink(piwentity_I, intPkEleet, intPkEleele,
                        darrpiwentityAllProcesses, darrdynlkjson, context, ref strLink, out piwentityO, out ioentityO);

                    boolIsPostSize = ioentityO.boolnSize == true ? true : false;
                }
                else
                {
                    boolIsPostSize = CalCalculation.boolIsPostSizeWorkflowJob(ioentity);
                }
            }

            return boolIsPostSize;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolIsPostSizeWorkflowJob(
            //                                              //Receive and IO input and verify if is a postSize IO.

            IoentityInputsAndOutputsEntityDB ioentity_I
            )
        {
            bool boolIsPostSize = false;
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            if (
                //                                          //We receive an IO, and is not a node.
                !(ioentity_I.intnPkElementElementType == null &&
                ioentity_I.intnPkElementElement == null)
                )
            {
                //                                              //Get other side of the link.
                //                                              //This other side can be an IO or a node.
                //                                              //IO, means we need to check if the process than contains
                //                                              //      this IO has an IO-utput set as size.
                //                                              //Node, means we need to go back until we found an IO.

                List<IoentityInputsAndOutputsEntityDB> darrioentity = context.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == ioentity_I.intPkWorkflow &&
                    io.strLink == ioentity_I.strLink &&
                    io.intPk != ioentity_I.intPk).ToList();

                /*CASE*/
                if (
                    //                                          //There is only one possible way to reach the evaluated IO.
                    //                                          //Means is a connection with other IO.
                    darrioentity.Count == 1
                    )
                {
                    //                                          //Take the  IO.
                    IoentityInputsAndOutputsEntityDB ioentity = darrioentity[0];

                    boolIsPostSize = ioentity.boolnSize == true ? true : false;
                }
                else if (
                    //                                          //The prev connection with the evaluated IO is a node.
                    darrioentity.Count > 1
                    )
                {
                    //                                          //Verify there is only one connection.
                    //                                          //Take node.

                    //                                          //Take other IOs are not node.
                    List<IoentityInputsAndOutputsEntityDB> darrioentityNotNode = darrioentity.Where(io
                        => io.intnPkElementElement != null ||
                        io.intnPkElementElementType != null).ToList();

                    //                                          //From those IOs are not node, take only those are
                    //                                          //      IO-Output.
                    List<IoentityInputsAndOutputsEntityDB> darrioentityAsOutput = new List<IoentityInputsAndOutputsEntityDB>();
                    foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentityNotNode)
                    {
                        if (
                            ioentity.intnPkElementElementType != null
                            )
                        {
                            EleetentityElementElementTypeEntityDB eleetentity =
                                context.ElementElementType.FirstOrDefault(eleet =>
                                eleet.intPk == ioentity.intnPkElementElementType);
                            if (
                                //                              //It is an IO-Output
                                eleetentity != null && eleetentity.boolUsage == false
                                )
                            {
                                darrioentityAsOutput.Add(ioentity);
                            }
                        }
                        else
                        {
                            EleeleentityElementElementEntityDB eleeleentity =
                                context.ElementElement.FirstOrDefault(eleele =>
                                eleele.intPk == ioentity.intnPkElementElement);
                            if (
                                //                              //It is an IO-Output
                                eleeleentity != null && eleeleentity.boolUsage == false
                                )
                            {
                                darrioentityAsOutput.Add(ioentity);
                            }
                        }
                    }

                    //                                          //If the list with IOs as IO-Output is equal to 1, means
                    //                                          //      the node has only one possible way to reach
                    //                                          //      evaluated IO.

                    /*CASE*/
                    if (
                        //                                  
                        darrioentityAsOutput.Count > 1
                        )
                    {
                        bool boolAllIOsHasSize = true;
                        /*WHILE*/
                        int intI = 0;
                        while (
                            boolAllIOsHasSize &&
                            intI < darrioentityAsOutput.Count
                            )
                        {
                            if (
                                darrioentityAsOutput[intI].boolnSize == false ||
                                darrioentityAsOutput[intI].boolnSize == null
                                )
                            {
                                boolAllIOsHasSize = false;
                            }
                            intI++;
                        }
                    }
                    else if (
                       darrioentityAsOutput.Count == 1
                       )
                    {
                        IoentityInputsAndOutputsEntityDB ioentityOutput = darrioentityAsOutput[0];

                        boolIsPostSize = ioentityOutput.boolnSize == true ? true : false;
                    }
                    /*CASE*/
                }
                /*END-CASE*/
            }

            return boolIsPostSize;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetWidthAndLengthOutput(
            //                                              //Get width and length attribute from job.

            JobjsonJobJson jobjson_I,
            String strUnit_I,
            ref double numWidth_IO,
            ref double? numnHeight_IO,
            ref String strDimensionsJobOriginal_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<AttrjsonAttributeJson> darrattrjson = jobjson_I.darrattrjson.ToList();

            intStatus_IO = 400;
            strUserMessage_IO = "Order form has not a size attribute.";
            strDevMessage_IO = "Order form has not a size attribute.";
            foreach (AttrjsonAttributeJson attrjson in darrattrjson)
            {
                if (
                    attrjson.strAttributeName == "Size" ||
                    attrjson.strAttributeName == "*Size" ||
                    attrjson.strAttributeName == "Finished Size" ||
                    attrjson.strAttributeName == "*Finished Size"
                    )
                {
                    intStatus_IO = 401;
                    strUserMessage_IO = "Invalid size value.";
                    strDevMessage_IO = "Size attribute is not parseable to num.";

                    int intIndexOfX = attrjson.strValue.IndexOf('x');
                    if (
                        intIndexOfX >= 0
                        )
                    {
                        String strWidth = attrjson.strValue.Substring(0, intIndexOfX - 1);
                        String strHeight = attrjson.strValue.Substring(intIndexOfX + 1,
                            (attrjson.strValue.Length - intIndexOfX) - 1);

                        //                                      //To easy code.
                        //                                      //To know the job's dimensions.

                        //                                      //To get i of inches.
                        int intIndexOfIWidthIn = strWidth.IndexOf("in");
                        int intIndexOfIHeigthIn = strHeight.IndexOf("in");

                        //                                      //To get c of centimeters.
                        int intIndexOfCWidthIn = strWidth.IndexOf("cm");
                        int intIndexOfCHeigthIn = strHeight.IndexOf("cm");

                        //                                      //To get f of feet.
                        int intIndexOfFWidthIn = strWidth.IndexOf("ft");
                        int intIndexOfFHeigthIn = strHeight.IndexOf("ft");

                        String strWidthValue = "";
                        String strHeightValue = "";

                        /*CASE*/
                        if (
                            intIndexOfIWidthIn > -1 &&
                            intIndexOfIHeigthIn > -1
                            )
                        {
                            strDimensionsJobOriginal_IO = "in";
                            strWidthValue = strWidth.Substring(0, intIndexOfIWidthIn - 1);
                            strWidthValue = strWidthValue.TrimExcel();
                            strHeightValue = strHeight.Substring(0, intIndexOfIHeigthIn - 1);
                            strHeightValue = strHeightValue.TrimExcel();
                        }
                        else if (
                            intIndexOfCWidthIn > -1 &&
                            intIndexOfCHeigthIn > -1
                            )
                        {
                            strDimensionsJobOriginal_IO = "cm";
                            strWidthValue = strWidth.Substring(0, intIndexOfCWidthIn - 1);
                            strWidthValue = strWidthValue.TrimExcel();
                            strHeightValue = strHeight.Substring(0, intIndexOfCWidthIn - 1);
                            strHeightValue = strHeightValue.TrimExcel();
                        }
                        else if (
                            intIndexOfFWidthIn > -1 &&
                            intIndexOfFHeigthIn > -1
                            )
                        {
                            strDimensionsJobOriginal_IO = "ft";
                            strWidthValue = strWidth.Substring(0, intIndexOfFWidthIn - 1);
                            strWidthValue = strWidthValue.TrimExcel();
                            strHeightValue = strHeight.Substring(0, intIndexOfFHeigthIn - 1);
                            strHeightValue = strHeightValue.TrimExcel();
                        }
                        /*END-CASE*/

                        strUnit_I = (strUnit_I == null || strUnit_I == "") ? "in" : strUnit_I;

                        if (
                            strWidthValue.IsParsableToNum() &&
                            strHeightValue.IsParsableToNum()
                            )
                        {
                            numWidth_IO = CvtConvert.to(strWidthValue.ParseToNum(), strDimensionsJobOriginal_IO,
                                strUnit_I);
                            numnHeight_IO = CvtConvert.to(strHeightValue.ParseToNum(), strDimensionsJobOriginal_IO,
                                strUnit_I);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }

                        strDimensionsJobOriginal_IO =
                            (strDimensionsJobOriginal_IO != "cm" && strDimensionsJobOriginal_IO != "in") ? "in" :
                            strDimensionsJobOriginal_IO;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetDimensionsValues(
            //                                              //Find the dimensions values for a media resource.

            int intPkRes_I,
            ref double numWidth_IO,
            ref double? numnLength_IO,
            ref String strDimUnit_IO,
            ref bool boolHasDimensions_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            List<ResattrjsonResourceAttributesJson> darrmedattrjson =
                (from attrEntity in context.Attribute
                    join valentity in context.Value
                    on attrEntity.intPk equals valentity.intPkAttribute
                    where valentity.intPkElement == intPkRes_I
                    select new ResattrjsonResourceAttributesJson(
                        attrEntity.intPk,
                        attrEntity.strXJDFName,
                        valentity.strValue)).ToList();

            //                                              //All media resource must to have width attribute.
            if (
                darrmedattrjson.Exists(attr=>attr.strAttrName == "Width")
                )
            {
                //                                          //Get resResource to know if is a roll or not.
                ResResource resResource = ResResource.resFromDB(intPkRes_I, false);
                if (
                    //                                      //It's not a roll.
                    !resResource.boolMediaRoll()
                    )
                {
                    if (
                        (darrmedattrjson.Exists(attr => attr.strAttrName == "Length") &&
                        darrmedattrjson.Exists(attr => attr.strAttrName == "DimensionsUnit")
                        )
                        ||
                        (
                        darrmedattrjson.Exists(attr => attr.strAttrName == "Length") &&
                        darrmedattrjson.Exists(attr => attr.strAttrName == "LengthUnit") &&
                        darrmedattrjson.Exists(attr => attr.strAttrName == "WidthUnit")
                        )
                        )
                    {
                        boolHasDimensions_IO = true;

                        numWidth_IO = darrmedattrjson.FirstOrDefault(attr =>
                            attr.strAttrName == "Width").strAttrValue.ParseToNum();

                        numnLength_IO = darrmedattrjson.FirstOrDefault(attr =>
                            attr.strAttrName == "Length").strAttrValue.ParseToNum();

                        String strWidthUnit = darrmedattrjson.Exists(attr => attr.strAttrName == "WidthUnit") ?
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "WidthUnit").strAttrValue :
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "DimensionsUnit").strAttrValue; 

                        String strLenghtUnit = darrmedattrjson.Exists(attr => attr.strAttrName == "LengthUnit") ?
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "LengthUnit").strAttrValue :
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "DimensionsUnit").strAttrValue;

                        strDimUnit_IO = darrmedattrjson.Exists(attr => attr.strAttrName == "DimensionsUnit") ?
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "DimensionsUnit").strAttrValue :
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "WidthUnit").strAttrValue;

                        if (
                            strWidthUnit != strLenghtUnit
                            )
                        {
                            numnLength_IO = CvtConvert.to((double)numnLength_IO, strLenghtUnit, strWidthUnit);
                        }
                    }
                }
                else
                {
                    //                                      //It´s a roll.
                    if (
                        darrmedattrjson.Exists(attr => attr.strAttrName == "DimensionsUnit") ||
                        (
                        darrmedattrjson.Exists(attr => attr.strAttrName == "LengthUnit") &&
                        darrmedattrjson.Exists(attr => attr.strAttrName == "WidthUnit")
                        )
                        )
                    {
                        boolHasDimensions_IO = true;

                        numWidth_IO = darrmedattrjson.FirstOrDefault(attr =>
                            attr.strAttrName == "Width").strAttrValue.ParseToNum();
                        numnLength_IO = null;
                        strDimUnit_IO = darrmedattrjson.Exists(attr => attr.strAttrName == "DimensionsUnit") ?
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "DimensionsUnit").strAttrValue :
                            darrmedattrjson.FirstOrDefault(attr => attr.strAttrName == "WidthUnit").strAttrValue;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String strGetWidthUnit(
            int intPkRes_I
            )
        {
            String strWidthUnit = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            List<ResattrjsonResourceAttributesJson> darrattrjson =
                (from attrEntity in context.Attribute
                 join valentity in context.Value
                 on attrEntity.intPk equals valentity.intPkAttribute
                 where valentity.intPkElement == intPkRes_I
                 select new ResattrjsonResourceAttributesJson(
                     attrEntity.intPk,
                     attrEntity.strXJDFName,
                     valentity.strValue)).ToList();

            //                                          //Get resResource.
            ResResource resResource = ResResource.resFromDB(intPkRes_I, false);
            if (
                //                                      //It's component.
                resResource.restypBelongsTo.strCustomTypeId == ResResource.strComponent ||
                resResource.restypBelongsTo.strCustomTypeId == ResResource.strMedia
                )
            {
                if (
                    darrattrjson.Exists(attr => attr.strAttrName == "WidthUnit")
                    )
                {
                    strWidthUnit = darrattrjson.FirstOrDefault(attr => attr.strAttrName == "WidthUnit")
                        .strAttrValue;
                }
            }
            return strWidthUnit;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static String strGetLengthUnit(
            int intPkRes_I
            )
        {
            String strLengthUnit = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            List<ResattrjsonResourceAttributesJson> darrattrjson =
                (from attrEntity in context.Attribute
                 join valentity in context.Value
                 on attrEntity.intPk equals valentity.intPkAttribute
                 where valentity.intPkElement == intPkRes_I
                 select new ResattrjsonResourceAttributesJson(
                     attrEntity.intPk,
                     attrEntity.strXJDFName,
                     valentity.strValue)).ToList();

            //                                          //Get resResource.
            ResResource resResource = ResResource.resFromDB(intPkRes_I, false);
            if (
                //                                      //It's component.
                resResource.restypBelongsTo.strCustomTypeId == ResResource.strComponent ||
                resResource.restypBelongsTo.strCustomTypeId == ResResource.strMedia
                )
            {
                if (
                    darrattrjson.Exists(attr => attr.strAttrName == "LengthUnit")
                    )
                {
                    strLengthUnit = darrattrjson.FirstOrDefault(attr => attr.strAttrName == "LengthUnit")
                        .strAttrValue;
                }
            }
            return strLengthUnit;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            CalCalculation cal = (CalCalculation)obj_I;

            int intGroup = 0;
            if (
                this.intnGroup != null
                )
            {
                intGroup = (int)this.intnGroup;
            }

            int intGroupB = 0;
            if (
                cal.intnGroup != null
                )
            {
                intGroupB = (int)cal.intnGroup;
            }

            return intGroupB.CompareTo(intGroup);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static CaljsonCalculationJson[] arrcaljsonGetBasePerQuantity(
            //                                              //Find all resources' base per quantity calculations 
            //                                              //      created in a normal workflow or in a base workflow.
            //                                              //A base per quantity calculation does not have Per 
            //                                              //      quantity from data and can be added only from a 
            //                                              //      workflow.

            int? intnPkProduct_I,
            int? intnJobId_I,
            String strPrintshopId_I,
            int intPkWorkflow_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Array to return.
            CaljsonCalculationJson[] arrcaljson = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get product type.
            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intnPkProduct_I);

            //                                              //Find workflow.
            WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf => wf.intPk == intPkWorkflow_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Workflow not found.";
            if (
                wfentity != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Product not found or pk is not for a product.";
                if (
                    //                                      //It comes from a product.
                    ((et != null) && (et.strResOrPro == EtElementTypeAbstract.strProduct) &&
                    (wfentity.intnPkProduct != null)) ||
                    //                                      //It comes from a wf base.
                    ((et == null) && (wfentity.intnPkProduct == null))
                    )
                {
                    //                                      //Get product type.
                    ProdtypProductType prodtyp = (ProdtypProductType)et;

                    List<CalCalculation> darrcal = new List<CalCalculation>();
                    if (
                        //                                  //Workflow base.
                        prodtyp == null
                        )
                    {
                        //                                  //Get all calculations for wf base.
                        List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(caletentity =>
                            caletentity.intnPkProduct == null && caletentity.intnPkWorkflow == intPkWorkflow_I &&
                            caletentity.strEndDate == null).ToList();

                        //                                  //Get all the calculations.
                        foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                        {
                            CalCalculation cal = new CalCalculation(calentity.intPk,
                                calentity.strUnit, calentity.numnQuantity, calentity.numnCost,
                                calentity.intnHours, calentity.intnMinutes, calentity.intnSeconds,
                                calentity.numnBlock, calentity.boolIsEnable, calentity.strValue,
                                calentity.strAscendants, calentity.strDescription, calentity.numnProfit,
                                calentity.intnPkProduct,
                                calentity.intnPkProcess, calentity.intnPkResource, calentity.strCalculationType,
                                calentity.strByX, calentity.strStartDate, calentity.strStartTime, calentity.strEndDate,
                                calentity.strEndTime, calentity.numnNeeded, calentity.numnPerUnits, calentity.numnMin,
                                calentity.numnQuantityWaste, calentity.numnPercentWaste, calentity.intnPkWorkflow,
                                calentity.intnProcessInWorkflowId, calentity.intnPkElementElementType,
                                calentity.intnPkElementElement, calentity.intnPkQFromElementElementType,
                                calentity.intnPkQFromElementElement, calentity.intnPkQFromResource,
                                calentity.intnPkAccount, calentity.boolnFromThickness, calentity.boolnIsBlock, 
                                calentity.boolnByArea);
                            darrcal.Add(cal);
                        }
                    }
                    else
                    {
                        //                                  //Get all calculation current of a product.
                        darrcal = prodtyp.darrcalCurrent;
                    }

                    //                                      //List that will contain filter calculations.
                    List<CalCalculation> darrcalFinal = new List<CalCalculation>();

                    if (
                        prodtyp != null
                        )
                    {
                        //                                  //Get calculations.
                        darrcalFinal = prodtyp.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intnJobId_I,
                            intPkWorkflow_I);
                    }
                    else
                    {
                        //                                  //Return same list as in workflow base.
                        darrcalFinal = darrcal;
                    }

                    //                                      //Filter calculations by base per quantity.
                    darrcalFinal = darrcalFinal.Where(cal =>
                        (cal.strCalculationType == CalCalculation.strPerQuantityBase) &&
                        (cal.strByX == CalCalculation.strByResource) &&
                        ((cal.intnPkResourceElementBelongsTo != null) ||
                        (cal.intnPkProcessElementBelongsTo == null) &&
                        (cal.strAscendants == null) &&
                        (cal.strValue == null))).ToList();

                    List<CaljsonCalculationJson> darrcaljsonWorkflow = new List<CaljsonCalculationJson>();

                    //                                      //Create the json to send to the front.
                    foreach (CalCalculation cal in darrcalFinal)
                    {
                        String strUnitI = cal.strUnit;
                        if (
                            cal.intnPkResourceElementBelongsTo != null
                            )
                        {
                            //                              //Get the current unit of measurement.
                            ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(
                                (int)cal.intnPkResourceElementBelongsTo);
                            if (
                                valentity != null
                                )
                            {
                                strUnitI = valentity.strValue;
                            }
                        }

                        //                                  //Get unit from product.
                        String strUnitO = (prodtyp != null) ? prodtyp.strCategory : null;

                        //                                  //Get resource's name.
                        String strResourceName = null;
                        if (
                            cal.intnPkResourceElementBelongsTo != null
                            )
                        {
                            ResResource res = ResResource.resFromDB(cal.intnPkResourceElementBelongsTo, false);
                            strResourceName = res.strName;
                        }

                        CaljsonCalculationJson caljson = new CaljsonCalculationJson();
                        caljson.intPk = cal.intPk;
                        caljson.numnQuantity = cal.numnQuantity;
                        caljson.numnNeeded = cal.numnNeeded;
                        caljson.numnPerUnits = cal.numnPerUnits;
                        caljson.numnMin = cal.numnMin;
                        caljson.numnQuantityWaste = cal.numnQuantityWaste;
                        caljson.numnPercentWaste = cal.numnPercentWaste;
                        caljson.numnBlock = cal.numnBlock;
                        caljson.numnCost = cal.numnCost;
                        caljson.strDescription = cal.strDescription;
                        caljson.intnPkProcess = cal.intnPkProcessElementBelongsTo;
                        caljson.intnPkProduct = cal.intnPkProductTypeBelongsTo;
                        caljson.boolIsEnable = cal.boolIsEnable;
                        caljson.strCalculationType = cal.strCalculationType;
                        caljson.arrAscendantName = Tools.arrstrAscendantName(cal.strAscendants);
                        caljson.strValue = cal.strValue;
                        caljson.intnGroupId = cal.intnGroup;
                        caljson.strBy = cal.strByX;
                        caljson.intnPkResourceI = cal.intnPkResourceElementBelongsTo;
                        caljson.strUnitI = strUnitI;
                        caljson.intnPkResourceO = cal.intnPkQFromResourceElementBelongsTo;
                        caljson.strUnitO = strUnitO;
                        caljson.strResourceName = strResourceName;
                        caljson.boolIsEditable = true;
                        caljson.boolnIsWorkflow = true;

                        //                                  //Check if the calculation has conditions
                        GpcondjsonGroupConditionJson gpcondjson = Tools.gpcondjsonGetCondition(cal.intPk, null, null, null);
                        if (
                            gpcondjson != null
                            )
                        {
                            caljson.boolHasCondition = true;
                        }

                        if (
                            //                              //There is an associated account 
                            cal.intnPkAccount != null
                            )
                        {
                            //                              //Find account name
                            caljson.strAccountName = context.Account.FirstOrDefault(acc =>
                                acc.intPk == cal.intnPkAccount).strName;
                        }

                        //                                  //Find information about the process where calculation was
                        //                                  //      set.

                        //                                  //Find process in workflow.
                        PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                            piw => piw.intPkWorkflow == cal.intnPkWorkflowBelongsTo &&
                            piw.intProcessInWorkflowId == cal.intnProcessInWorkflowId);

                        //                                  //PkPIW.
                        caljson.intnPkProcessInWorkflow = piwentity.intPk;

                        //                                  //Post process.
                        caljson.boolIsInPostProcess = piwentity.boolIsPostProcess;

                        //                                  //Find process.
                        EleentityElementEntityDB eleentityProcessInWorkflow = context.Element.FirstOrDefault(ele =>
                                ele.intPk == piwentity.intPkProcess);

                        caljson.strProcessName = eleentityProcessInWorkflow.strElementName;
                        if (
                            piwentity.intnId != null
                            )
                        {
                            caljson.strProcessName = eleentityProcessInWorkflow.strElementName + " (" +
                                piwentity.intnId + ")";
                        }

                        //caljson.boolIsEditable = CalCalculation.boolIsEditable(cal.intnPkResourceElementBelongsTo,
                        //    cal.intnPkElementElementTypeBelongsTo, cal.intnPkElementElementBelongsTo,
                        //    cal.intnPkQFromResourceElementBelongsTo, cal.intnPkQFromElementElementTypeBelongsTo,
                        //    cal.intnPkQFromElementElementBelongsTo, configuration_I, intnJobId_I, strPrintshopId_I,
                        //    piwentity.intPk, ref intStatus_IO, ref strUserMessage_IO,
                        //    ref strDevMessage_IO);

                        if (
                            //                          //Calculation belongs to normal piw.
                            !piwentity.boolIsPostProcess ||
                            //                          //Calculation belongst to post piw and it is associated to
                            //                          //      an input without link.
                            CalCalculation.boolCalculationIsFromInputWithoutLinkOrFromProcess(cal)
                            )
                        {
                            darrcaljsonWorkflow.Add(caljson);
                        }
                    }

                    //                                      //Re order list.
                    arrcaljson = darrcaljsonWorkflow.OrderBy(cal => cal.strDescription).ToArray();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }

            return arrcaljson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int intGetPerUnit(
            //                                              //Returns the PerUnit used when adding a calculation
            //                                              //      usign the thickness and the lift of a cutter or
            //                                              //      the thickness and the heigth of a misconsumable. 

            int intPkResource_I,
            int intPkResourceQFrom_I,
            int intPkWorkflow_I,
            int intJobId_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int intPerUnit = 0;

            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the resource entities.
            EleentityElementEntityDB eleentityRes = context.Element.FirstOrDefault(ele =>
                ele.intPk == intPkResource_I);
            EleentityElementEntityDB eleentityResQFrom = context.Element.FirstOrDefault(ele =>
                ele.intPk == intPkResourceQFrom_I);

            //                                              //Get the resource type entities.
            EtentityElementTypeEntityDB etentityRes = context.ElementType.FirstOrDefault(et =>
                et.intPk == eleentityRes.intPkElementType);
            EtentityElementTypeEntityDB etentityResQFrom = context.ElementType.FirstOrDefault(et =>
                et.intPk == eleentityResQFrom.intPkElementType);

            //                                              //To know if is a Device or MiscConsumable.
            bool boolIsDeviceOrMiscConsumable =
                (etentityRes.strXJDFTypeId == ProdtypProductType.strResourceTypeDevice ||
                etentityRes.strXJDFTypeId == ProdtypProductType.strResClasMiscConsumable) ? true :
                false;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource must be device or MiscConsumable.";
            if (
                boolIsDeviceOrMiscConsumable
                )
            {
                bool boolIsPaper =
                    (etentityResQFrom.strCustomTypeId == ResResource.strComponent) ||
                    (etentityResQFrom.strCustomTypeId == ResResource.strMedia);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "ResourceQFrom must be Paper or Component.";
                if (
                    boolIsPaper
                    )
                {
                    //                                      //Attributes for Resource.
                    List<AttrentityAttributeEntityDB> darrattrResource =
                        (from attrentity in context.Attribute
                         join attretentity in context.AttributeElementType
                         on attrentity.intPk equals attretentity.intPkAttribute
                         where attretentity.intPkElementType == eleentityRes.intPkElementType
                         select attrentity).ToList();

                    //                                      //Get the LiftOrHeight.
                    double? numnLiftOrHeight = null;
                    String strLiftOrHeightUnit = null;
                    bool boolIsLift;
                    if (
                        etentityRes.strXJDFTypeId == ProdtypProductType.strResourceTypeDevice
                        )
                    {
                        boolIsLift = true;

                        //                                  //Get the Lift and LiftUnit attributes.
                        AttrentityAttributeEntityDB attrentityLift = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "Lift" || a.strCustomName == "XJDFLift");

                        AttrentityAttributeEntityDB attrentityLiftUnit = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "LiftUnit" || a.strCustomName == "XJDFLiftUnit");

                        if (
                            attrentityLift != null && attrentityLiftUnit != null
                            )
                        {
                            //                              //Get the Lift value.
                            ValentityValueEntityDB valentityLift = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityRes.intPk &&
                                val.intPkAttribute == attrentityLift.intPk);

                            numnLiftOrHeight = valentityLift != null ?
                                (double?)((valentityLift.strValue).ParseToNum()) : null;

                            //                              //Get the LiftUnit value.
                            ValentityValueEntityDB valentityLiftUnit = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityRes.intPk &&
                                val.intPkAttribute == attrentityLiftUnit.intPk);

                            strLiftOrHeightUnit = valentityLiftUnit != null ? valentityLiftUnit.strValue : null;
                        }
                    }
                    else
                    {
                        boolIsLift = false;

                        //                                  //Get the Height and HeightUnit attributes.
                        AttrentityAttributeEntityDB attrentityHeight = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "Height" || a.strCustomName == "XJDFHeight");

                        AttrentityAttributeEntityDB attrentityHeightUnit = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "HeightUnit" || a.strCustomName == "XJDFHeightUnit");

                        if (
                            attrentityHeight != null && attrentityHeightUnit != null
                            )
                        {
                            //                              //Get the Height value.
                            ValentityValueEntityDB valentityHeight = context.Value.FirstOrDefault(val =>
                            val.intPkElement == eleentityRes.intPk &&
                            val.intPkAttribute == attrentityHeight.intPk);

                            numnLiftOrHeight = valentityHeight != null ?
                                (double?)((valentityHeight.strValue).ParseToNum()) : null;

                            //                              //Get the HeightUnit value.
                            ValentityValueEntityDB valentityHeightUnit = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityRes.intPk &&
                                val.intPkAttribute == attrentityHeightUnit.intPk);

                            strLiftOrHeightUnit = valentityHeightUnit != null ? valentityHeightUnit.strValue : null;
                        }
                    }

                    intStatus_IO = 403;
                    strUserMessage_IO = boolIsLift ? "Lift must be set in device." : "Heigth must be set in resource.";
                    strDevMessage_IO = "Lift, LiftUnit, Height or HeightUnit are null.";
                    if (
                        (numnLiftOrHeight != null) &&
                        (strLiftOrHeightUnit != null)
                        )
                    {
                        //                                  //Get resource working as thickness in a workflow.
                        EleentityElementEntityDB eleentityThicknessResource =
                            ResResource.eleentityResourceWorkingAsThickness(intPkWorkflow_I, intJobId_I, context);

                        intStatus_IO = 404;
                        strUserMessage_IO = "No thickness found in workflow.";
                        strDevMessage_IO = "";
                        if (
                            eleentityThicknessResource != null
                            )
                        {
                            //                              //Attributes for Resource as thickness.
                            List<AttrentityAttributeEntityDB> darrattrResourceQFrom =
                                (from attrentity in context.Attribute
                                 join attretentity in context.AttributeElementType
                                 on attrentity.intPk equals attretentity.intPkAttribute
                                 where attretentity.intPkElementType == eleentityThicknessResource.intPkElementType
                                 select attrentity).ToList();

                            //                              //Get the Thickness attribute.
                            AttrentityAttributeEntityDB attrentityThickness = darrattrResourceQFrom.FirstOrDefault(a =>
                                a.strXJDFName == "Thickness" || a.strCustomName == "XJDFThickness");
                            //                              //Get the ThicknessUnit attribute.
                            AttrentityAttributeEntityDB attrentityThicknessUnit = darrattrResourceQFrom.FirstOrDefault(
                                a => a.strXJDFName == "ThicknessUnit" || a.strCustomName == "XJDFThicknessUnit");

                            //                              //Get the Thickness value.
                            ValentityValueEntityDB valentityThickness = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityThicknessResource.intPk &&
                                val.intPkAttribute == attrentityThickness.intPk);

                            double? numnThickness = null;
                            numnThickness = valentityThickness != null ?
                                (double?)((valentityThickness.strValue).ParseToNum()) : null;

                            //                              //Get the ThicknessUnit value.
                            ValentityValueEntityDB valentityThicknessUnit = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityThicknessResource.intPk &&
                                val.intPkAttribute == attrentityThicknessUnit.intPk);

                            String strThicknessUnit = valentityThicknessUnit != null ?
                                valentityThicknessUnit.strValue : null;

                            double numThickness;
                            if (
                                (numnThickness != null) &&
                                (strThicknessUnit != null)
                                )
                            {
                                if (
                                    strThicknessUnit == "mm"
                                    )
                                {
                                    //                      //Thickness in milimeters into micrometers.
                                    numThickness = (double)numnThickness * 1000;
                                }
                                else if (

                                    strThicknessUnit == "point"
                                    )
                                {
                                    //                      //Thicness in points(1/1000)in into micrometers.
                                    //                      //1000 points = 1 in
                                    //                      //1 point = (1/1000) in
                                    //                      //1 in = 25400 um
                                    numThickness = (double)numnThickness * 25.4;
                                }
                                else
                                {
                                    //                      //Thickness in micrometros.
                                    numThickness = (double)numnThickness;
                                }

                                double numLiftOrHeight;
                                if (
                                    strLiftOrHeightUnit == "in"
                                    )
                                {
                                    //                      //LiftOrHeight in in into micrometers.
                                    numLiftOrHeight = (double)numnLiftOrHeight * 25400;
                                }
                                else
                                {
                                    //                      //LiftOrHeight in cm into micrometers.
                                    numLiftOrHeight = (double)numnLiftOrHeight * 10000;
                                }

                                intPerUnit = (int)(numLiftOrHeight / numThickness);

                                intStatus_IO = 200;
                                strUserMessage_IO = "Success.";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
            return intPerUnit;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int intGetPerUnit(
            //                                              //Returns the PerUnit used when adding a calculation
            //                                              //      usign the thickness and the lift of a cutter or
            //                                              //      the thickness and the heigth of a misconsumable. 

            int? intnPkEleetQFrom_I,
            int? intnPkEleeleQFrom_I,
            int intPkResource_I,
            int intPkResourceQFrom_I,
            int intPkProcessInWorkflow_I,
            List<ResthkjsonResourceThicknessJson> darrResourceThickness_I
            )
        {
            int intPerUnit = 0;

            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the resource entities.
            EleentityElementEntityDB eleentityRes = context.Element.FirstOrDefault(ele =>
                ele.intPk == intPkResource_I);
            EleentityElementEntityDB eleentityResQFrom = context.Element.FirstOrDefault(ele =>
                ele.intPk == intPkResourceQFrom_I);

            //                                              //Get the resource type entities.
            EtentityElementTypeEntityDB etentityRes = context.ElementType.FirstOrDefault(et =>
                et.intPk == eleentityRes.intPkElementType);
            EtentityElementTypeEntityDB etentityResQFrom = context.ElementType.FirstOrDefault(et =>
                et.intPk == eleentityResQFrom.intPkElementType);

            //                                              //To know if is a Device or MiscConsumable.
            bool boolIsDeviceOrMiscConsumable =
                (etentityRes.strXJDFTypeId == ProdtypProductType.strResourceTypeDevice ||
                etentityRes.strXJDFTypeId == ProdtypProductType.strResClasMiscConsumable) ? true :
                false;

            //                                              //Resource must be device or MiscConsumable.
            if (
                boolIsDeviceOrMiscConsumable
                )
            {
                bool boolIsPaper =
                    (etentityResQFrom.strCustomTypeId == ResResource.strComponent) ||
                    (etentityResQFrom.strCustomTypeId == ResResource.strMedia);

                //                                          //ResourceQFrom must be Paper or Component.
                if (
                    boolIsPaper
                    )
                {
                    //                                      //Attributes for Resource.
                    List<AttrentityAttributeEntityDB> darrattrResource =
                        (from attrentity in context.Attribute
                         join attretentity in context.AttributeElementType
                         on attrentity.intPk equals attretentity.intPkAttribute
                         where attretentity.intPkElementType == eleentityRes.intPkElementType
                         select attrentity).ToList();

                    //                                      //Get the LiftOrHeight.
                    double? numnLiftOrHeight = null;
                    String strLiftOrHeightUnit = null;
                    bool boolIsLift;
                    if (
                        etentityRes.strXJDFTypeId == ProdtypProductType.strResourceTypeDevice
                        )
                    {
                        boolIsLift = true;

                        //                                  //Get the Lift and LiftUnit attributes.
                        AttrentityAttributeEntityDB attrentityLift = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "Lift" || a.strCustomName == "XJDFLift");

                        AttrentityAttributeEntityDB attrentityLiftUnit = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "LiftUnit" || a.strCustomName == "XJDFLiftUnit");

                        if (
                            attrentityLift != null && attrentityLiftUnit != null
                            )
                        {
                            //                              //Get the Lift value.
                            ValentityValueEntityDB valentityLift = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityRes.intPk &&
                                val.intPkAttribute == attrentityLift.intPk);

                            numnLiftOrHeight = valentityLift != null ?
                                (double?)((valentityLift.strValue).ParseToNum()) : null;

                            //                              //Get the LiftUnit value.
                            ValentityValueEntityDB valentityLiftUnit = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityRes.intPk &&
                                val.intPkAttribute == attrentityLiftUnit.intPk);

                            strLiftOrHeightUnit = valentityLiftUnit != null ? valentityLiftUnit.strValue : null;
                        }
                    }
                    else
                    {
                        boolIsLift = false;

                        //                                  //Get the Height and HeightUnit attributes.
                        AttrentityAttributeEntityDB attrentityHeight = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "Height" || a.strCustomName == "XJDFHeight");

                        AttrentityAttributeEntityDB attrentityHeightUnit = darrattrResource.FirstOrDefault(a =>
                            a.strXJDFName == "HeightUnit" || a.strCustomName == "XJDFHeightUnit");

                        if (
                            attrentityHeight != null && attrentityHeightUnit != null
                            )
                        {
                            //                              //Get the Height value.
                            ValentityValueEntityDB valentityHeight = context.Value.FirstOrDefault(val =>
                            val.intPkElement == eleentityRes.intPk &&
                            val.intPkAttribute == attrentityHeight.intPk);

                            numnLiftOrHeight = valentityHeight != null ?
                                (double?)((valentityHeight.strValue).ParseToNum()) : null;

                            //                              //Get the HeightUnit value.
                            ValentityValueEntityDB valentityHeightUnit = context.Value.FirstOrDefault(val =>
                                val.intPkElement == eleentityRes.intPk &&
                                val.intPkAttribute == attrentityHeightUnit.intPk);

                            strLiftOrHeightUnit = valentityHeightUnit != null ? valentityHeightUnit.strValue : null;
                        }
                    }

                    //                                      //Lift, LiftUnit, Height or HeightUnit are null.
                    if (
                        (numnLiftOrHeight != null) &&
                        (strLiftOrHeightUnit != null)
                        )
                    {
                        ResthkjsonResourceThicknessJson resthkjson = darrResourceThickness_I.FirstOrDefault(
                            resthk => resthk.intnPkEleet == intnPkEleetQFrom_I &&
                            resthk.intnPkEleele == intnPkEleeleQFrom_I &&
                            resthk.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                            resthk.intPkResource == intPkResourceQFrom_I);

                        //                                  //No thickness found in workflow;
                        if (
                            resthkjson != null
                            )
                        {
                            double? numnThickness = resthkjson.numnThickness;

                            String strThicknessUnit = resthkjson.strThicknessUnit;

                            double numThickness;
                            if (
                                (numnThickness != null) &&
                                (strThicknessUnit != null)
                                )
                            {
                                if (
                                    strThicknessUnit == "mm"
                                    )
                                {
                                    //                      //Thickness in milimeters into micrometers.
                                    numThickness = (double)numnThickness * 1000;
                                }
                                else if (

                                    strThicknessUnit == "point"
                                    )
                                {
                                    //                      //Thicness in points(1/1000)in into micrometers.
                                    //                      //1000 points = 1 in
                                    //                      //1 point = (1/1000) in
                                    //                      //1 in = 25400 um
                                    numThickness = (double)numnThickness * 25.4;
                                }
                                else
                                {
                                    //                      //Thickness in micrometros.
                                    numThickness = (double)numnThickness;
                                }

                                double numLiftOrHeight;
                                if (
                                    strLiftOrHeightUnit == "in"
                                    )
                                {
                                    //                      //LiftOrHeight in in into micrometers.
                                    numLiftOrHeight = (double)numnLiftOrHeight * 25400;
                                }
                                else
                                {
                                    //                      //LiftOrHeight in cm into micrometers.
                                    numLiftOrHeight = (double)numnLiftOrHeight * 10000;
                                }

                                intPerUnit = (int)(numLiftOrHeight / numThickness);

                            }
                        }
                    }
                }
            }
            return intPerUnit;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetTransformCalculations(

            int? intnJobId_I,
            int intPkProcessInWorkflow_I,
            String strPrintshopId_I,
            out TranscaljsonTransformCalculationJson[] arrtranscaljson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrtranscaljson_O = null;

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get ProcessInWorkflow.
            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == intPkProcessInWorkflow_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Process in workflow not found.";
            if (
                piwentity != null
                )
            {
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                List<TranscaljsonTransformCalculationJson> darrtranscaljson =
                    new List<TranscaljsonTransformCalculationJson>();

                //                                          //Get transform calculations.
                List<TrfcalentityTransformCalculationEntityDB> darrtrfcalentity =
                    CalCalculation.darrtrfcalGetTransformCalculationsCurrentByJob(intnJobId_I, piwentity.intPk,
                    ps.intPk);

                //                                          //Create the json to send to the front.
                foreach (TrfcalentityTransformCalculationEntityDB trfcalentity in darrtrfcalentity)
                {
                    //                                      //INPUT RESOURCE
                    //                                      //Find input resource.
                    EleentityElementEntityDB eleentityInput = context.Element.FirstOrDefault(element =>
                        element.intPk == trfcalentity.intPkResourceI);

                    String strInputResName = eleentityInput.strElementName;

                    //                                      //Input unit of measurement.
                    String strUnitInput = ProdtypProductType.strUnitFromEleentityResource(eleentityInput);

                    //                                      //Find input resourceI type.
                    EtElementTypeAbstract eletInput = EletemElementType.etFromDB(eleentityInput.intPkElementType);

                    String strTypeTemplateAndResourceInput;
                    //                                      //To easy code.
                    int intPkEleetOrEleeleI;
                    bool boolIsEleetI = false;
                    if (
                        trfcalentity.intnPkElementElementTypeI != null
                        )
                    {
                        intPkEleetOrEleeleI = (int)trfcalentity.intnPkElementElementTypeI;
                        boolIsEleetI = true;

                        //                                  //Type and resource concatenation.
                        strTypeTemplateAndResourceInput = "(" + eletInput.strXJDFTypeId + ") " + strInputResName;
                    }
                    else
                    {
                        intPkEleetOrEleeleI = (int)trfcalentity.intnPkElementElementI;
                        EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(
                            eleele => eleele.intPk == intPkEleetOrEleeleI);

                        //                                  //Find template of resourceI.
                        EleentityElementEntityDB eleentitytemplate = context.Element.FirstOrDefault(ele =>
                            ele.intPk == eleeleentity.intPkElementSon);

                        //                                  //Type, template and resource concatenation.
                        strTypeTemplateAndResourceInput = "(" + eletInput.strXJDFTypeId + " : " +
                            eleentitytemplate.strElementName + ") " + strInputResName;
                    }

                    //                                      //OUTPUT RESOURCE
                    //                                      //Find output resource.
                    EleentityElementEntityDB eleentityOutput = context.Element.FirstOrDefault(element =>
                        element.intPk == trfcalentity.intPkResourceO);

                    String strOutputResName = eleentityOutput.strElementName;

                    //                                      //Output unit of measurement.
                    String strUnitOutput = ProdtypProductType.strUnitFromEleentityResource(eleentityOutput);

                    //                                      //Find output resource type.
                    EtElementTypeAbstract eletOutput = EletemElementType.etFromDB(eleentityOutput.intPkElementType);

                    String strTypeTemplateAndResourceOutput;
                    //                                      //To easy code.
                    int intPkEleetOrEleeleO;
                    bool boolIsEleetO = false;
                    if (
                        trfcalentity.intnPkElementElementTypeO != null
                        )
                    {
                        intPkEleetOrEleeleO = (int)trfcalentity.intnPkElementElementTypeO;
                        boolIsEleetO = true;

                        //                                  //Type and resourceO concatenation.
                        strTypeTemplateAndResourceOutput = "(" + eletOutput.strXJDFTypeId + ") " + strOutputResName;
                    }
                    else
                    {
                        intPkEleetOrEleeleO = (int)trfcalentity.intnPkElementElementO;
                        EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(
                            eleele => eleele.intPk == intPkEleetOrEleeleO);

                        //                                  //Find template of resourceO.
                        EleentityElementEntityDB eleentitytemplate = context.Element.FirstOrDefault(ele =>
                            ele.intPk == eleeleentity.intPkElementSon);

                        //                                  //Type, template and resource concatenation.
                        strTypeTemplateAndResourceOutput = "(" + eletOutput.strXJDFTypeId + " : " +
                            eleentitytemplate.strElementName + ") " + strOutputResName;
                    }
                    
                    //                                      //Json object to return.
                    TranscaljsonTransformCalculationJson trfcaljson = new TranscaljsonTransformCalculationJson(
                        trfcalentity.intPk, trfcalentity.intPkProcessInWorkflow, trfcalentity.numNeeded,
                        trfcalentity.numPerUnits, strTypeTemplateAndResourceInput, intPkEleetOrEleeleI, boolIsEleetI,
                        trfcalentity.intPkResourceI, strUnitInput, intPkEleetOrEleeleO, boolIsEleetO,
                        trfcalentity.intPkResourceO, strUnitOutput, strTypeTemplateAndResourceOutput);

                    //                                      //Check if the transform calculation has conditions
                    GpcondjsonGroupConditionJson gpcondjson = Tools.gpcondjsonGetCondition(null, null, null,
                        trfcalentity.intPk);
                    if (
                        gpcondjson != null
                        )
                    {
                        trfcaljson.boolHasCondition = true;
                    }

                    darrtranscaljson.Add(trfcaljson);
                }

                arrtranscaljson_O = darrtranscaljson.ToArray();

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static List<TrfcalentityTransformCalculationEntityDB> darrtrfcalGetTransformCalculationsCurrentByJob(
            //                                              //Get all trfcal for Job stage and WF from db.
            //                                              //If job is null then return all current calculations 
            //                                              //    and filter by process in workflow.

            int? intnJobId_I,
            //                                              //Pk ProcessInWorkflow.
            int intPkProcessInWorkflow_I,
            int intPkPrintshop_I
            )
        {
            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Initialize the list of trfcal.
            List<TrfcalentityTransformCalculationEntityDB> darrtrfcalAll = context.TransformCalculation.Where(
                trfcal => trfcal.intPkProcessInWorkflow == intPkProcessInWorkflow_I).ToList();

            List<TrfcalentityTransformCalculationEntityDB> darrtrfcalReturn =
                new List<TrfcalentityTransformCalculationEntityDB>();

            if (
                intnJobId_I == null
                )
            {
                //                                          //Return unfinished transform calculation.
                darrtrfcalReturn = darrtrfcalAll.Where(trfcalCurrent => trfcalCurrent.strEndDate == null).ToList();
            }
            else if (
                intnJobId_I != null
                )
            {
                //                                          //Get job's register.
                JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == intnJobId_I &&
                    job.intPkPrintshop == intPkPrintshop_I);

                if (
                    jobentity != null
                    )
                {
                    //                                      //Job is inprogress or completed.
                    //                                      //Get the transform calculations than apply.
                    darrtrfcalReturn = CalCalculation.darrtrfcalentityGetJobTransformCalculation(jobentity,
                        darrtrfcalAll);
                }
                else
                {
                    //                                      //Job is Pending.
                    //                                      //Return unfinished transform calculation.
                    darrtrfcalReturn = darrtrfcalAll.Where(trfcalCurrent => trfcalCurrent.strEndDate == null).ToList();
                }
            }

            return darrtrfcalReturn;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<TrfcalentityTransformCalculationEntityDB> darrtrfcalentityGetJobTransformCalculation(
            //                                              //Return transform calculations that apply to the job

            JobentityJobEntityDB jobentity_I,
            List<TrfcalentityTransformCalculationEntityDB> darrtrfcal_I
            )
        {
            //                                              //Declare transform calculation list to return.
            List<TrfcalentityTransformCalculationEntityDB> darrtrfcalFinal =
                new List<TrfcalentityTransformCalculationEntityDB>();

            //                                              //Get job's date.
            ZonedTime ztimeJobStartDate = ZonedTimeTools.NewZonedTime(jobentity_I.strStartDate.ParseToDate(),
                    jobentity_I.strStartTime.ParseToTime());

            //                                              //Check transform calculations that apply.
            foreach (TrfcalentityTransformCalculationEntityDB trfcalToCheck in darrtrfcal_I)
            {
                if (
                    //                                      //Transform calculation still able.
                    trfcalToCheck.strEndDate == null
                    )
                {
                    //                                      //Get transform calculation's startdate.
                    ZonedTime ztimeTrfCalStartDate = ZonedTimeTools.NewZonedTime(
                        trfcalToCheck.strStartDate.ParseToDate(), trfcalToCheck.strStartTime.ParseToTime());
                    if (
                        //                                  //Transform calculation apply to the job.
                        ztimeJobStartDate >= ztimeTrfCalStartDate
                        )
                    {
                        //                                  //Add transform calculation.
                        darrtrfcalFinal.Add(trfcalToCheck);
                    }
                }
                else if (
                    //                                      //Transform calculation was edited or deleted.
                    trfcalToCheck.strEndDate != null
                    )
                {
                    //                                      //Get transform calculation's startdate and enddate
                    ZonedTime ztimeTrfCalStartDate = ZonedTimeTools.NewZonedTime(
                        trfcalToCheck.strStartDate.ParseToDate(), trfcalToCheck.strStartTime.ParseToTime());
                    ZonedTime ztimeTrfCalEndDate = ZonedTimeTools.NewZonedTime(trfcalToCheck.strEndDate.ParseToDate(),
                        trfcalToCheck.strEndTime.ParseToTime());
                    if (
                        //                                  //Transform calculation apply to the job.
                        ztimeJobStartDate >= ztimeTrfCalStartDate &&
                        ztimeJobStartDate < ztimeTrfCalEndDate
                        )
                    {
                        //                                  //Add transform calculation.
                        darrtrfcalFinal.Add(trfcalToCheck);
                    }
                }
            }

            return darrtrfcalFinal;
        }

        //--------------------------------------------------------------------------------------------------------------
    }
    //==================================================================================================================
}
/*END-TASK*/