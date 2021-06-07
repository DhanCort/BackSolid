/*TASK RP.RESOURCE*/
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.App;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTemplates.Out;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TowaStandard;
using Odyssey2Backend.Alert;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Process.SRP;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: March 30, 2020. 

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public class ProProcess
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }

        private readonly String strName_Z;
        public String strName { get { return this.strName_Z; } }

        private readonly ProtypProcessType protypBelongsTo_Z;
        public ProtypProcessType protypBelongsTo { get { return this.protypBelongsTo_Z; } }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public ProProcess(
            //                                              //Primary key of the process.
            int intPk_I,
            //                                              //Process Name
            String strProcessName_I
            )
        {
            this.intPk_Z = intPk_I;
            this.strName_Z = strProcessName_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public ProProcess(
            //                                              //Primary key of the process.
            int intPk_I,
            //                                              //Process Name
            String strProcessName_I,
            //                                              //Type which it belongs to.
            ProtypProcessType protypBelongsTo_I

            )
        {
            this.intPk_Z = intPk_I;
            this.strName_Z = strProcessName_I;
            this.protypBelongsTo_Z = protypBelongsTo_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static int subAdd(
            String strName_I,
            //                                              //Type which it belongs to.
            ProtypProcessType protypBelongsTo_I,
            //                                              //Printshop id.
            String strPrintshopId_I,

            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the conecction.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 402;
            strUserMessage_IO = "Invalid type.";
            strDevMessage_IO = "Invalid type.";

            int intPkProcessAdded = -1;

            //                                              //Get the printshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            //                                              //The type is pass as ref, because it can be changed from
            //                                              //      the XJDF to the clone.
            ProtypProcessType protypBelongsTo = protypBelongsTo_I;
            if (
                ValidateProcessType.boolIsValidType(ps, context, ref protypBelongsTo)
                )
            {
                intStatus_IO = 403;
                strUserMessage_IO = "Invalid name.";
                strDevMessage_IO = "Invalid name.";
                if (
                    ValidateProcessName.isValid(strName_I, (int)protypBelongsTo.intPkPrintshop,
                    ref strUserMessage_IO)
                    )
                {
                    //                                      //Add the process.
                    SaveProcessToDB.subSave(strName_I, context, protypBelongsTo.intPk, out intPkProcessAdded);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "Added successfully";
                }
            }
            return intPkProcessAdded;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddProcessCustomType(
            //                                              //Add a new custom process type.

            //                                              //PrintshopId that create the Custom type.
            String strPrintshopId_I,
            int intPkPrintshop_I,
            //                                              //custom Process type name.
            String strCustomProcessName_I,
            //                                              //Primary key of the process created. If it is -1 the 
            //                                              //      type was not added to the db.
            out int intProcessTypePk_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 402;
            strUserMessage_IO = "The name of the process cannot start with XJDF.";
            strDevMessage_IO = "";
            intProcessTypePk_O = -1;
            if (
                //                                          //Have the XJDF prefix.
                !strCustomProcessName_I.StartsWith(EtElementTypeAbstract.strXJDFPrefix)
                )
            {
                //                                          //Make the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Verify if the custom process for the printshop exists.
                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == ProtypProcessType.strProCustomType && et.strAddedBy == strPrintshopId_I);

                if (
                    //                                      //If the custom Process dont exists will create it.
                    etentity == null &&
                    ValidateProcessName.isValid(strCustomProcessName_I, intPkPrintshop_I, ref strUserMessage_IO)
                    )
                {
                    SaveElementType.subSave(strPrintshopId_I, intPkPrintshop_I, context);

                    EtentityElementTypeEntityDB etentityPk = context.ElementType.FirstOrDefault(et =>
                        et.strCustomTypeId == ProtypProcessType.strProCustomType && et.strAddedBy == strPrintshopId_I);

                    //                                  //Create Element.
                    SaveProcessToDB.subSave(strCustomProcessName_I, context, etentityPk.intPk, out intProcessTypePk_O);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";

                }
                else
                {
                    intStatus_IO = 404;
                    strUserMessage_IO = "Invalid name.";
                    strDevMessage_IO = "";
                    if (
                        ValidateProcessName.isValid(strCustomProcessName_I, intPkPrintshop_I,
                        ref strUserMessage_IO)
                        )
                    {
                        //                                  //Create Element.
                        SaveProcessToDB.subSave(strCustomProcessName_I, context, etentity.intPk, out intProcessTypePk_O);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteIO(
            //                                              //Delete type or template of a process and associated 
            //                                              //      elements:
            //                                              //0. Estimates. Estimates are deleted first because it is 
            //                                              //      necessary to confirm that the io to delete has or 
            //                                              //      has not links if it is an output so the ios need to 
            //                                              //      be in database when a estimate is deleted.
            //                                              //1. Inputs and outputs.
            //                                              //2. Inputs and outputs for a job.
            //                                              //3. Calculations.
            //                                              //4. Periods.

            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            PsPrintShop ps_I,
            Odyssey2Context context_I,
            out int intPkProcess_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intPkProcess_O = 0;

            //                                              //To easy code.
            EleetentityElementElementTypeEntityDB eleetentity = null;
            EleeleentityElementElementEntityDB eleeleentity = null;
            int? intnId = null;
            if (
                boolIsEleet_I
                )
            {
                eleetentity = context_I.ElementElementType.FirstOrDefault(eleet => eleet.intPk == intPkEleetOrEleele_I);
                intnId = eleetentity.intnId;
            }
            else
            {
                eleeleentity = context_I.ElementElement.FirstOrDefault(eleele => eleele.intPk == intPkEleetOrEleele_I);
                intnId = eleeleentity.intnId;
            }

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No eleet or eleele found.";
            if (
                eleetentity != null || eleeleentity != null
                )
            {
                intPkProcess_O = boolIsEleet_I ? eleetentity.intPkElementDad : eleeleentity.intPkElementDad;

                bool boolWorkflowCloned;
                ProProcess.subCloneWorkflowsIfItIsNecessary(ps_I, intPkProcess_O, context_I, out boolWorkflowCloned);

                if (
                    boolWorkflowCloned
                    )
                {
                    //                                      //Find the process.
                    ProProcess proBase = ProProcess.proFromDB(intPkProcess_O, context_I);

                    //                                      //Copy the old process into a new one.
                    ProProcess proNew;
                    ProtypProcessType.subCopyAProcess(proBase, context_I, out proNew);

                    //                                      //Update tables with the new process.
                    ProtypProcessType.subUpdateProcessPkInActiveWorkflows(proBase, proNew, context_I);

                    //                                      //Find the new eleet and eleele.
                    if (
                        boolIsEleet_I
                        )
                    {
                        eleetentity = context_I.ElementElementType.FirstOrDefault(eleet =>
                            eleet.intPkElementDad == proNew.intPk &&
                            eleet.intPkElementTypeSon == eleetentity.intPkElementTypeSon &&
                            eleet.boolUsage == eleetentity.boolUsage && 
                            eleet.intnId == intnId);
                    }
                    else
                    {
                        eleeleentity = context_I.ElementElement.FirstOrDefault(eleele =>
                            eleele.intPkElementDad == proNew.intPk &&
                            eleele.intPkElementSon == eleeleentity.intPkElementSon &&
                            eleele.boolUsage == eleeleentity.boolUsage &&
                            eleele.intnId == intnId);
                    }

                    //                                  //Return current process's pk.
                    intPkProcess_O = proNew.intPk;
                }
            }

            //                                          //Delete paper transformations.
            ProProcess.subDeletePaperTransformations(eleetentity?.intPk, eleeleentity?.intPk, context_I);

            //                                          //Delete the estimates.
            ProProcess.subDeleteEstimatesForIO(eleetentity, eleeleentity, context_I);

            //                                          //Delete the inputs and outputs.
            ProProcess.subDeleteInputsAndOutputsForIO(eleetentity?.intPk, eleeleentity?.intPk, context_I);

            //                                          //Delete the inputs and outputs for a job.
            ProProcess.subDeleteInputsAndOutputsForAJobForIO(eleetentity?.intPk, eleeleentity?.intPk, context_I);

            //                                          //Delete the calculations.
            ProProcess.subDeleteCalculationsAndTransCalForIO(eleetentity?.intPk, eleeleentity?.intPk, context_I);

            //                                          //Delete the periods.
            ProProcess.subDeletePeriodsForIO(eleetentity?.intPk, eleeleentity?.intPk, context_I);

            //                                          //Delete eleet and update ids.
            ProProcess.subRemoveAndUpdateIdEleetAndEleele(eleetentity, eleeleentity, boolIsEleet_I,
                context_I);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subRemoveAndUpdateIdEleetAndEleele(
            //                                              //Update Id from eleet and eleele and delete eleet or elele.

            EleetentityElementElementTypeEntityDB eleetentityToRemove_I,
            EleeleentityElementElementEntityDB eleeleentityToRemove_I, 
            bool boolEleet_I,
            Odyssey2Context context_M
            )
        {
            if (
                boolEleet_I
                )
            {
                if (
                    eleetentityToRemove_I.intnId != null
                    )
                {
                    //                                      //Get all son from dad excludin 
                    //                                      //    eleetentityToRemove_I.
                    List<EleetentityElementElementTypeEntityDB> darreleetToUpdate =
                        context_M.ElementElementType.Where(eleet => eleet.intPkElementDad ==
                        eleetentityToRemove_I.intPkElementDad && 
                        eleet.intPkElementTypeSon == eleetentityToRemove_I.intPkElementTypeSon &&
                        eleet.boolDeleted == eleetentityToRemove_I.boolDeleted &&
                        eleet.boolUsage == eleetentityToRemove_I.boolUsage &&
                        eleet.intPk != eleetentityToRemove_I.intPk).ToList();

                    //                                      //Get Id less.
                    List<EleetentityElementElementTypeEntityDB> darreleetLess = darreleetToUpdate.Where(
                        eleet => eleet.intnId < eleetentityToRemove_I.intnId).ToList();

                    //                                      //Get Id Greather.
                    List<EleetentityElementElementTypeEntityDB> darreleetGreather = darreleetToUpdate.Where(
                        eleet => eleet.intnId > eleetentityToRemove_I.intnId).ToList();

                    //                                      //take each ID greather.
                    foreach (EleetentityElementElementTypeEntityDB eleetToUpdate in darreleetGreather)
                    {
                        eleetToUpdate.intnId = eleetToUpdate.intnId - 1;
                        context_M.ElementElementType.Update(eleetToUpdate);
                        context_M.SaveChanges();
                    }

                    if (
                        //                                  //Ids less equal 1.
                        darreleetLess.Count == 1 &&
                        darreleetGreather.Count == 0
                        )
                    {
                        //                                  //Update to null.
                        darreleetLess[0].intnId = null;
                        context_M.ElementElementType.Update(darreleetLess[0]);
                        context_M.SaveChanges();
                    }
                }

                //                                      //Delete eleet.
                context_M.ElementElementType.Remove(eleetentityToRemove_I);
                context_M.SaveChanges();
            }
            else
            {
                if (
                    eleeleentityToRemove_I.intnId != null
                    )
                {
                    //                                      //Get all son from dad excludin 
                    //                                      //    eleeleentityToRemove_I.
                    List<EleeleentityElementElementEntityDB> darreleeleToUpdate =
                        context_M.ElementElement.Where(eleele => eleele.intPkElementDad ==
                        eleeleentityToRemove_I.intPkElementDad &&
                        eleele.intPkElementSon == eleeleentityToRemove_I.intPkElementSon &&
                        eleele.boolDeleted == eleeleentityToRemove_I.boolDeleted &&
                        eleele.boolUsage == eleeleentityToRemove_I.boolUsage &&
                        eleele.intPk != eleeleentityToRemove_I.intPk).ToList();

                    //                                      //Get Id less.
                    List<EleeleentityElementElementEntityDB> darreleeleLess = darreleeleToUpdate.Where(
                        eleele => eleele.intnId < eleetentityToRemove_I.intnId).ToList();

                    //                                      //Get Id Greather.
                    List<EleeleentityElementElementEntityDB> darreleeleGreather = darreleeleToUpdate.Where(
                        eleele => eleele.intnId > eleetentityToRemove_I.intnId).ToList();

                    foreach (EleeleentityElementElementEntityDB eleeleToUpdate in darreleeleGreather)
                    {
                        eleeleToUpdate.intnId = eleeleToUpdate.intnId - 1;
                        context_M.ElementElement.Update(eleeleToUpdate);
                        context_M.SaveChanges();
                    }

                    if (
                        //                                  //Ids less equal 1.
                        darreleeleLess.Count == 1 &&
                        darreleeleGreather.Count == 0
                        )
                    {
                        //                                  //Update to null.
                        darreleeleLess[0].intnId = null;
                        context_M.ElementElement.Update(darreleeleLess[0]);
                        context_M.SaveChanges();
                    }
                }

                //                                      //Delete eleele.
                context_M.ElementElement.Remove(eleeleentityToRemove_I);
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCloneWorkflowsIfItIsNecessary(
            //                                              //Find all the process in wf and verify if one od those wf
            //                                              //      has jobs in progress or completed. If there are jobs
            //                                              //      clone the necessary wf.

            PsPrintShop ps_I,
            int intPkProcess_I,
            Odyssey2Context context_I,
            out bool boolWorkflowCloned_O
            )
        {
            boolWorkflowCloned_O = false;

            //                                              //Get the wf for this process.
            //                                              //It is necessary to find all workflows, even the ones 
            //                                              //      already deleted.
            List<WfentityWorkflowEntityDB> darrwfentity = (
                from wfentity in context_I.Workflow
                join piwentity in context_I.ProcessInWorkflow
                on wfentity.intPk equals piwentity.intPkWorkflow
                where piwentity.intPkProcess == intPkProcess_I && !wfentity.boolDeleted
                select wfentity).Distinct().ToList();

            foreach (WfentityWorkflowEntityDB wfentity in darrwfentity)
            {
                //                                          //Verifies if a workflow has a job in progress or completed.
                WfentityWorkflowEntityDB wfentityNew;
                ProdtypProductType.subAddWorkflowIfItIsNecessary(ps_I, wfentity, context_I, out wfentityNew);
                if (
                    //                                      //Workflow was cloned.
                    wfentity.intPk != wfentityNew.intPk
                    )
                {
                    boolWorkflowCloned_O = true;
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteInputsAndOutputsForIO(
            //                                              //Delete the entries for an io in the table InputsAndOutputs
            //                                              //      and verify if the row has links, if it has verify if
            //                                              //      the linked register has resource or group, if it has
            //                                              //      then the register is only updated, if it has not 
            //                                              //      then the register is deleted too.

            int? intnPkEleet_I,
            int? intnPkEleele_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get entries for this io.
            List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
            io.intnPkElementElementType == intnPkEleet_I && io.intnPkElementElement == intnPkEleele_I).ToList();

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
            {
                if (
                    //                                      //Row has link.
                    ioentity.strLink != null
                    )
                {
                    IoentityInputsAndOutputsEntityDB ioentityLinked = context_I.InputsAndOutputs.FirstOrDefault(io =>
                    io.intPkWorkflow == ioentity.intPkWorkflow && io.strLink == ioentity.strLink &&
                    io.intPk != ioentity.intPk);

                    if (
                        //                                  //The linked row has not resource or group associated.
                        (ioentityLinked.intnPkResource == null || ioentityLinked.intnGroupResourceId == null) &&
                        //                                  //It is not a node.
                        (ioentityLinked.intnPkElementElementType != null || ioentityLinked.intnPkElementElement != null)
                        )
                    {
                        Tools.subDeleteCondition(null, null, ioentity.intPk, null, context_I);

                        //                                  //Remove linked row.
                        context_I.InputsAndOutputs.Remove(ioentityLinked);
                    }
                    else if (
                        //                                  //It is not a node.
                        (ioentityLinked.intnPkElementElementType != null || ioentityLinked.intnPkElementElement != null)
                        )
                    {
                        //                                  //Update linked row.
                        ioentityLinked.strLink = null;
                        context_I.InputsAndOutputs.Update(ioentityLinked);
                    }
                }

                //                                          //remove the row.
                context_I.InputsAndOutputs.Remove(ioentity);
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteInputsAndOutputsForAJobForIO(
            //                                              //Delete the entries for an io in the table 
            //                                              //      InputsAndOutputsForAJob.

            int? intnPkEleet_I,
            int? intnPkEleele_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get entries.
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity = context_I.InputsAndOutputsForAJob.Where(io =>
            io.intnPkElementElementType == intnPkEleet_I && io.intnPkElementElement == intnPkEleele_I).ToList();

            foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
            {
                //                                          //Remove the entry.
                context_I.InputsAndOutputsForAJob.Remove(iojentity);
            }
            context_I.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteCalculationsAndTransCalForIO(
            //                                              //Delete the entries for an io in the table Calculation and
            //                                              //      transform calculations.

            int? intnPkEleet_I,
            int? intnPkEleele_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get calculations.
            List<CalentityCalculationEntityDB> darrcalentityEleele = context_I.Calculation.Where(cal =>
            cal.intnPkElementElementType == intnPkEleet_I && cal.intnPkElementElement == intnPkEleele_I).ToList();

            foreach (CalentityCalculationEntityDB calentity in darrcalentityEleele)
            {
                //                                          //Find paper transformations associated to this cal.
                List<PatransPaperTransformationEntityDB> darrpatransentity = context_I.PaperTransformation.Where(
                    paper => paper.intnPkCalculationOwn == calentity.intPk ||
                    paper.intnPkCalculationLink == calentity.intPk).ToList();

                foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                {
                    //                                      //Remove paper trans.
                    context_I.PaperTransformation.Remove(patransentity);
                }

                Tools.subDeleteCondition(calentity.intPk, null, null, null, context_I);

                //                                          //Delete calculation.
                context_I.Calculation.Remove(calentity);
            }

            //                                              //Find Eleet's transform calculations.
            List<TrfcalentityTransformCalculationEntityDB> darrtrfcalentity =
                context_I.TransformCalculation.Where(trf =>
                (trf.intnPkElementElementTypeI == intnPkEleet_I && trf.intnPkElementElementI == intnPkEleele_I) ||
                (trf.intnPkElementElementTypeO == intnPkEleet_I && trf.intnPkElementElementO == intnPkEleele_I))
                .ToList();

            foreach (TrfcalentityTransformCalculationEntityDB trfcalentity in darrtrfcalentity)
            {
                Tools.subDeleteCondition(null, null, null, trfcalentity.intPk, context_I);

                context_I.TransformCalculation.Remove(trfcalentity);
            }

            context_I.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeletePeriodsForIO(
            //                                              //Delete the entries for an io in the table Period.

            int? intnPkEleet_I,
            int? intnPkEleele_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get periods.
            List<PerentityPeriodEntityDB> darrperentity = context_I.Period.Where(per =>
            per.intnPkElementElementType == intnPkEleet_I && per.intnPkElementElement == intnPkEleele_I).ToList();

            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                //                                          //Delete period.
                context_I.Period.Remove(perentity);
            }

            context_I.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeletePaperTransformations(
            //                                              //Delete the entries for an io in the paper transformation
            //                                              //      table.

            int? intnPkEleet_I,
            int? intnPkEleele_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Find paper transformations.
            List<PatransPaperTransformationEntityDB> darrpatransentity = context_I.PaperTransformation.Where(
                paper => (paper.intnPkElementElementTypeI == intnPkEleet_I ||
                paper.intnPkElementElementTypeO == intnPkEleet_I) &&
                (paper.intnPkElementElementI == intnPkEleele_I ||
                paper.intnPkElementElementO == intnPkEleele_I)).ToList();

            foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
            {
                //                                          //Delete paper transformations.
                context_I.PaperTransformation.Remove(patransentity);
            }

            context_I.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteEstimatesForIO(
            //                                              //Delete the entries for an io in the table Period.

            EleetentityElementElementTypeEntityDB eleetentity_I,
            EleeleentityElementElementEntityDB eleeleentity_I,
            Odyssey2Context context_I
            )
        {
            //                                              //To easy code.
            int? intnPkEleet = eleetentity_I?.intPk;
            int? intnPkEleele = eleeleentity_I?.intPk;
            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(eleetentity_I?.intPkElementTypeSon);
            ResResource resTemplate = ResResource.resFromDB(eleeleentity_I?.intPkElementSon, true);

            //                                              //Get entries for this io.
            List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
            io.intnPkElementElementType == intnPkEleet && io.intnPkElementElement == intnPkEleele).ToList();

            if (
                //                                          //It is eleet input and physical or output with links.
                (eleetentity_I != null && ((eleetentity_I.boolUsage &&
                RestypResourceType.boolIsPhysical(et.strClassification)) ||
                (!eleetentity_I.boolUsage && darrioentity.FirstOrDefault(o => o.strLink != null) != null))) ||
                //                                          //It is eleele input and physical or output with links.
                (eleeleentity_I != null && ((eleeleentity_I.boolUsage &&
                RestypResourceType.boolIsPhysical(resTemplate.restypBelongsTo.strClassification)) ||
                (!eleeleentity_I.boolUsage && darrioentity.FirstOrDefault(o => o.strLink != null) != null)))
                )
            {
                int intPkProcess = (intnPkEleet != null) ? eleetentity_I.intPkElementDad :
                    eleeleentity_I.intPkElementDad;

                List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = new List<PiwentityProcessInWorkflowEntityDB>();
                if (
                    intnPkEleet != null
                    )
                {
                    //                                      //Find process in PIW table.
                    darrpiwentity = context_I.ProcessInWorkflow.Where(piw => piw.intPkProcess == intPkProcess).ToList();
                }
                else
                {
                    //                                      //Find process in PIW table.
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityModified = context_I.ProcessInWorkflow.Where(
                        piw => piw.intPkProcess == intPkProcess).ToList();

                    foreach (PiwentityProcessInWorkflowEntityDB piwentityModified in darrpiwentityModified)
                    {
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityFromWf = context_I.ProcessInWorkflow.
                            Where(piw => piw.intPkWorkflow == piwentityModified.intPkWorkflow).ToList();
                        darrpiwentity.AddRange(darrpiwentityFromWf);
                    }
                }

                //                                          //Delete EstimationData entries.
                JobJob.subDeleteEstimationDataEntriesForAWorkflow(context_I, darrpiwentity);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeletePeriod(
            //                                              //Delete a period from Period table.

            int intPkPeriod_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            out String strLastSunday_O,
            out String strEstimatedDate_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            strLastSunday_O = null;
            strEstimatedDate_O = "";
            //                                              //Establish the connection with the db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get period to delete.
            PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per => per.intPk == intPkPeriod_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Period not found or not a process type.";
            if (
                //                                          //Period exists and it comes from a process.
                perentity != null &&
                perentity.intnPkElementElement == null &&
                perentity.intnPkElementElementType == null
                )
            {
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw => 
                    piw.intPkWorkflow == perentity.intPkWorkflow && 
                    piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId);

                PiwjentityProcessInWorkflowForAJobEntityDB piwjentity = context.ProcessInWorkflowForAJob.FirstOrDefault(
                    piwj => piwj.intPkProcessInWorkflow == piwentity.intPk);

                bool boolProcessCompled = false;
                bool boolProcessStarted = false;
                if (
                    piwjentity != null
                    )
                {
                    boolProcessCompled = (piwjentity.intStage == JobJob.intProcessInWorkflowCompleted);
                    boolProcessStarted = (piwjentity.intStage == JobJob.intProcessInWorkflowStarted);
                }

                intStatus_IO = 413;
                intStatus_IO = 413;
                strUserMessage_IO = boolProcessCompled ? "Cannot delete a period to a completed Process." :
                    "Cannot delete a period to a started Process.";
                if (
                    !boolProcessCompled &&
                    !boolProcessStarted
                    )
                {
                    //                                      //Transform str to date. 
                    Date dateStartDate = perentity.strStartDate.ParseToDate();
                    //                                      //Get the day of the week of that date. 
                    int intDayOfWeek = (int)dateStartDate.DayOfWeek;
                    if (
                        //                                  //Verify if it is a sunday. 
                        intDayOfWeek == 0
                        )
                    {
                        //                                  //Return the same date. 
                        strLastSunday_O = perentity.strStartDate;
                    }
                    else
                    {
                        //                                  //Calculate last sunday.
                        Date dateLastSunday = dateStartDate - intDayOfWeek;
                        strLastSunday_O = dateLastSunday.ToText();
                    }

                    //                                      //Find alerts about this period.
                    List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                        alert.intnPkPeriod == perentity.intPk).ToList();

                    foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                    {
                        //                                  //Delete alerts about this period.

                        if (
                            //                              //Notification not read.
                            !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                            )
                        {
                            AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                                iHubContext_I);
                        }

                        context.Alert.Remove(alertentity);
                    }

                    //                                          //Remove period from db.
                    context.Period.Remove(perentity);
                    context.SaveChanges();

                    //                                          //Get the JobJson.
                    JobjsonJobJson jobjsonJob;
                    JobJob.boolIsValidJobId(perentity.intJobId, strPrintshopId_I, configuration_I, out jobjsonJob,
                        ref strUserMessage_IO, ref strDevMessage_IO);

                    //                                          //Get the estimate date. 
                    strEstimatedDate_O = ProProcess.strEstimateDateJob(perentity.intPkWorkflow, strPrintshopId_I,
                        jobjsonJob, context);

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subEditName(

            int intPkProcess_I,
            String strProcessName_I,
            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find the process to Edit.
            ProProcess pro = ProProcess.proFromDB(intPkProcess_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Process not found.";
            if (
                pro.protypBelongsTo != null && pro.protypBelongsTo.intPkPrintshop == ps_I.intPk
                )
            {
                //                                          //No user message because this method returns that.
                strDevMessage_IO = "";
                if (
                    ValidateProcessName.isValid(strProcessName_I, (int)pro.protypBelongsTo.intPkPrintshop, ref
                        strUserMessage_IO)
                    )
                {
                    //                                      //Get process.
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == pro.intPk);
                    //                                      //Rename.
                    eleentity.strElementName = strProcessName_I;
                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddGenericProcess(

            int intPkNewProcessType_I,
            String strNewProcessName_I,
            Odyssey2Context context_M,
            out EleentityElementEntityDB eleentityNewProcess_O
            )
        {
            EleentityElementEntityDB eleentityNewProcess = new EleentityElementEntityDB
            {
                strElementName = strNewProcessName_I,
                intPkElementType = intPkNewProcessType_I
            };

            context_M.Add(eleentityNewProcess);
            context_M.SaveChanges();

            eleentityNewProcess_O = eleentityNewProcess;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static ProProcess proFromDB(
            int? intnPk_I
            )
        {
            ProProcess pro = null;
            if (
                //                                          //It is a invalid primary key.
                intnPk_I > 0
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(eleentity =>
                    eleentity.intPk == intnPk_I);

                if (
                    eleentity != null
                    )
                {
                    EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(eleentity.intPkElementType);
                    ProtypProcessType prodtyp = ((et != null) && (et.strResOrPro == EtElementTypeAbstract.strProcess)) ?
                        (ProtypProcessType)et : null;

                    pro = new ProProcess(eleentity.intPk, eleentity.strElementName, prodtyp);
                }
            }

            return pro;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ProProcess proFromDB(
            int? intnPk_I,
            Odyssey2Context context_I
            )
        {
            ProProcess pro = null;
            if (
                //                                          //It is a invalid primary key.
                intnPk_I > 0
                )
            {
                EleentityElementEntityDB eleentity = context_I.Element.FirstOrDefault(eleentity =>
                    eleentity.intPk == intnPk_I && eleentity.boolDeleted == false);

                if (
                    eleentity != null
                    )
                {
                    EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(context_I, eleentity.intPkElementType);
                    ProtypProcessType prodtyp = ((et != null) && (et.strResOrPro == EtElementTypeAbstract.strProcess)) ?
                        (ProtypProcessType)et : null;

                    pro = new ProProcess(eleentity.intPk, eleentity.strElementName, prodtyp);
                }
            }

            return pro;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subProcessModifiable(
            //                                              //If it is to add an output the process is modifiable.
            //                                              //If it is to add an input, verify all the workflows where
            //                                              //      the process is used and if at least one workflow has
            //                                              //      estimates the process is not modifiable.

            int intPkProcess_I,
            String strInputOrOutput_I,
            int? intnPkType_I,
            int? intnPkTemplate_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> IHubContext_I,
            out bool boolIsModifiable_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            boolIsModifiable_O = false;

            //                                              //Get process.
            ProProcess pro = ProProcess.proFromDB(intPkProcess_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No process found.";
            if (
                pro != null
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                //                                          //To easy code.
                EtentityElementTypeEntityDB etentity;
                if (
                    intnPkType_I != null
                    )
                {
                    etentity = context.ElementType.FirstOrDefault(et => et.intPk == intnPkType_I);
                }
                else
                {
                    ResResource resTemplate = ResResource.resFromDB(intnPkTemplate_I, true);
                    etentity = context.ElementType.FirstOrDefault(et => et.intPk == resTemplate.restypBelongsTo.intPk);
                }

                if (
                    //                                      //It is to add an input.
                    strInputOrOutput_I == ProtypProcessType.strInput &&
                    //                                      //It is physical type or template.
                    RestypResourceType.boolIsPhysical(etentity.strClassification)
                    )
                {
                    //                                      //Get the estimations from the workflows where this process
                    //                                      //      is in.
                    List<EstentityEstimateEntityDB> darrestentity = (
                        from estentity in context.Estimate
                        join wfentity in context.Workflow
                        on estentity.intPkWorkflow equals wfentity.intPk
                        join piwentity in context.ProcessInWorkflow
                        on wfentity.intPk equals piwentity.intPkWorkflow
                        where piwentity.intPkProcess == pro.intPk
                        select estentity).ToList();

                    String strJobNumber;
                    bool boolUpdateJobAndEstimate = false;
                    //                                      //Construction of the message;
                    String strJobs = "";
                    foreach (EstentityEstimateEntityDB estentity in darrestentity)
                    {
                        //                                  //Get strJobNumber
                        JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
                            job.intJobID == estentity.intJobId);

                        if (
                            jobjsonentity == null &&
                            !boolUpdateJobAndEstimate
                            )
                        {
                            //                                          //using is to release connection at the end
                            using (Odyssey2Context contextT = new Odyssey2Context())
                            {
                                //                                      //Starts a new transaction.
                                using (var dbContextTransaction = contextT.Database.BeginTransaction())
                                {
                                    try
                                    {

                                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                                        List<JobjsonJobJson> darrjobjson;
                                        JobJob.subGetPrintshopJobs(ps, null, true,
                                            null, null, null, null, null, null, configuration_I, IHubContext_I, contextT,
                                            out darrjobjson, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                                        if (
                                            intStatus_IO == 200
                                            )
                                        {
                                            List<EstimjsonEstimateJson> darrestimjson;
                                            JobJob.subGetPrintshopEstimates(ps, true, false,
                                                false, configuration_I, out darrestimjson, contextT, ref intStatus_IO,
                                                ref strUserMessage_IO, ref strDevMessage_IO);
                                        }
                                        
                                        //                              //Commits all changes made to the database in the current
                                        //                              //      transaction.
                                        if (
                                            intStatus_IO == 200
                                            )
                                        {
                                            dbContextTransaction.Commit();
                                            boolUpdateJobAndEstimate = true;
                                        }
                                        else
                                        {
                                            dbContextTransaction.Rollback();
                                            throw new Exception("Something Wrong.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //                              //Discards all changes made to the database in the current
                                        //                              //      transaction.
                                        dbContextTransaction.Rollback();

                                        //                              //Making a log for the exception.
                                        Tools.subExceptionHandler(ex, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                                        throw new Exception("Something Wrong.");
                                    }
                                }
                            }

                            jobjsonentity = context.JobJson.FirstOrDefault(job =>
                                job.intJobID == estentity.intJobId);
                        }

                        if (
                            jobjsonentity != null
                            )
                        {
                            if (
                                jobjsonentity.intnEstimateNumber != null
                                )
                            {
                                strJobNumber = jobjsonentity.intnEstimateNumber.ToString();
                            }
                            else
                            {
                                strJobNumber = jobjsonentity.intnOrderNumber.ToString() + " - " +
                                    jobjsonentity.intnJobNumber.ToString();
                            }

                            strJobs = strJobs.Contains(strJobNumber + "") ? strJobs : strJobs + strJobNumber +
                                ", ";
                        }
                    }

                    strJobs = (strJobs != "") ? strJobs.TrimEnd(' ', ',') : "";

                    //                                      //Returning the bool and the message if it is necessary.
                    boolIsModifiable_O = darrestentity.Count == 0;

                    intStatus_IO = 200;
                    strUserMessage_IO = boolIsModifiable_O ? "" : "Some estimates: " + strJobs +
                        ", will be deleted.";
                    strDevMessage_IO = "";
                }
                else
                {
                    boolIsModifiable_O = true;

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool boolIsDispensable(
            //                                              //True if the process is available to delete.

            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 403;
            strUserMessage_IO = "This process:";

            bool boolHasCalculationsAssociated = this.boolHasCalculationsAssociated(ref intStatus_IO,
                ref strUserMessage_IO);

            bool boolHasLinks = this.boolHasLinks(ref intStatus_IO, ref strUserMessage_IO);

            bool boolIsInWorkflowsWithEstimates = this.boolIsInWorkflowsWithEstimates(ref intStatus_IO,
                ref strUserMessage_IO);

            if (
                intStatus_IO == 403
                )
            {
                intStatus_IO = 200;
                strUserMessage_IO = "Process dispensable.";
            }

            return (
                !boolHasCalculationsAssociated && !boolHasLinks && !boolIsInWorkflowsWithEstimates
                );
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolHasCalculationsAssociated(
            //                                              //Return true if there is calculations for some process 
            //                                              //      associated to this.
            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            bool boolHasCalculationAssociated = false;

            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the first calculation associated.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(calentity =>
                calentity.intnPkProcess == this.intPk);

            //                                              //If the calculation is null means that there is no 
            //                                              //      calculations associated to this.
            boolHasCalculationAssociated = calentity != null;

            if (
                boolHasCalculationAssociated
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = strUserMessage_IO + " Has alculations.";
            }

            return boolHasCalculationAssociated;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -         
        public bool boolHasLinks(
            //                                              //Return true if there is links for some process 
            //                                              //      associated to this.
            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            bool boolHasLinks = false;

            //                                              //Establish the connection with db.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all the piw.
            IQueryable<PiwentityProcessInWorkflowEntityDB> setpiwentity = context.
                ProcessInWorkflow.Where(piw => piw.intPkProcess == this.intPk);
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = setpiwentity.ToList();

            String strProducts = "";

            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity)
            {
                //                                          //Get all the IOs for this piw.
                IQueryable<IoentityInputsAndOutputsEntityDB> setioentity = context.InputsAndOutputs.Where(io =>
                   io.intPkWorkflow == piwentity.intPkWorkflow &&
                   io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId);
                List<IoentityInputsAndOutputsEntityDB> darrioentity = setioentity.ToList();

                bool boolHasLinksInThisProduct = false;
                int intJ = 0;
                /*WHILE*/
                while (
                    intJ < darrioentity.Count &&
                    !boolHasLinksInThisProduct
                    )
                {
                    if (
                        darrioentity[intJ].strLink != null
                        )
                    {
                        WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                            wf.intPk == piwentity.intPkWorkflow);

                        EtentityElementTypeEntityDB etentityProduct = context.ElementType.FirstOrDefault(et =>
                            et.intPk == wfentity.intnPkProduct);

                        boolHasLinksInThisProduct = true;
                        boolHasLinks = true;
                        if (
                            !strProducts.Contains(etentityProduct.strCustomTypeId + ",")
                            )
                        {
                            strProducts = strProducts + " " + etentityProduct.strCustomTypeId + ",";
                        }
                    }
                    intJ = intJ + 1;
                }
            }

            if (
                boolHasLinks
                )
            {
                intStatus_IO = 200;
                if (
                    strProducts.Count<Char>(c => c == ',') > 1
                    )
                {
                    strUserMessage_IO = strUserMessage_IO + " Has links on these products:" + strProducts;
                }
                else
                {
                    strUserMessage_IO = strUserMessage_IO + " Has links on this product:" + strProducts;
                }

                strUserMessage_IO = strUserMessage_IO.Substring(0, strUserMessage_IO.Count() - 1) + ".";

            }

            return boolHasLinks;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public bool boolIsInWorkflowsWithEstimates(
            //                                              //Return true if this is used by workflows with estimates.

            ref int intStatus_IO,
            ref String strUserMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            bool boolIsInWorkflowsWithEstimates;

            //                                              //From IO table in PkResource column.
            List<EstentityEstimateEntityDB> darrestentity = (
                from estentity in context.Estimate
                join wfentity in context.Workflow
                on estentity.intPkWorkflow equals wfentity.intPk
                join piwentity in context.ProcessInWorkflow
                on wfentity.intPk equals piwentity.intPkWorkflow
                where wfentity.boolDeleted == false && piwentity.intPkProcess == this.intPk
                select estentity).ToList();
            darrestentity.AddRange(darrestentity);

            intStatus_IO = darrestentity.Count == 0 ? intStatus_IO : 402;
            strUserMessage_IO = darrestentity.Count == 0 ? strUserMessage_IO : strUserMessage_IO + " Is " +
                "associated to a job with estimates.";
            boolIsInWorkflowsWithEstimates = darrestentity.Count > 0;

            return boolIsInWorkflowsWithEstimates;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subIOIsDispensable(
            //                                              //If the io is an output without links is dispensable.
            //                                              //If the io is an output with links, is not dispensable also
            //                                              //      it is necessary to verify if estimates will be 
            //                                              //      deleted if the io is deleted.
            //                                              //If the io is an input verify the links and estimates.

            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> IHubContext_I,
            out bool boolIOIsDispensable_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            boolIOIsDispensable_O = false;

            //                                              //DB connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //To easy code.
            EleetentityElementElementTypeEntityDB eleetentity = null;
            EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(eleele =>
            eleele.intPk == intPkEleetOrEleele_I);
            if (
                boolIsEleet_I
                )
            {
                eleetentity = context.ElementElementType.FirstOrDefault(eleet => eleet.intPk == intPkEleetOrEleele_I);
            }

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No eleet or eleele found.";
            if (
                eleetentity != null || eleeleentity != null
                )
            {
                bool boolTypeIsDispensable;
                String strMessage;
                ProProcess.subTypeOrTemplateIsDispensable(strPrintshopId_I, eleetentity, eleeleentity, configuration_I, 
                    IHubContext_I, out boolTypeIsDispensable, out strMessage);
                boolIOIsDispensable_O = boolTypeIsDispensable;

                intStatus_IO = 200;
                strUserMessage_IO = strMessage;
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subTypeOrTemplateIsDispensable(
            String strPrintshopId_I,
            EleetentityElementElementTypeEntityDB eleetentity_I,
            EleeleentityElementElementEntityDB eleeleentity_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> IHubContext_I,
            out bool boolTypeIsDispensable_O,
            out String strMessage_O
            )
        {
            //                                              //To easy code.
            int? intnPkEleet = eleetentity_I?.intPk;
            int? intnPkEleele = eleeleentity_I?.intPk;

            Odyssey2Context context = new Odyssey2Context();
            //                                              //Get links where this eleet participates.
            List<EtentityElementTypeEntityDB> darretentityProduct = (
                from ioentity in context.InputsAndOutputs
                join wfentity in context.Workflow
                on ioentity.intPkWorkflow equals wfentity.intPk
                join etentity in context.ElementType
                on wfentity.intnPkProduct equals etentity.intPk
                where ioentity.intnPkElementElementType == intnPkEleet &&
                ioentity.intnPkElementElement == intnPkEleele &&
                ioentity.strLink != null &&
                wfentity.boolDeleted == false
                select etentity).ToList();

            String strProducts = "";
            foreach (EtentityElementTypeEntityDB etentity in darretentityProduct)
            {
                String strProductName = (etentity.strXJDFTypeId == "None") ? etentity.strCustomTypeId :
                    etentity.strXJDFTypeId;
                strProducts = strProducts.Contains(strProductName) ? strProducts : strProducts +
                    strProductName + ", ";
            }
            strProducts = strProducts.TrimEnd(' ', ',');

            //                                              //To easy code.
            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(eleetentity_I?.intPkElementTypeSon);
            ResResource resTemplate = ResResource.resFromDB(eleeleentity_I?.intPkElementSon, true);
            bool boolInput = (eleetentity_I != null) ? eleetentity_I.boolUsage : eleeleentity_I.boolUsage;
            bool boolPhysical = (et != null) ? RestypResourceType.boolIsPhysical(et.strClassification) :
                RestypResourceType.boolIsPhysical(resTemplate.restypBelongsTo.strClassification);

            String strJobsWithEstimates = "";
            if (
                //                                          //Is an input and it is physical it is necessary to verify 
                //                                          //      the estimates.
                (boolInput && boolPhysical) ||
                //                                          //Is an output with link.
                (!boolInput && strProducts != "")
                )
            {

                int intPkElementDad = eleetentity_I == null ?
                    eleeleentity_I.intPkElementDad : eleetentity_I.intPkElementDad;

                //                                          //Get the estimates for this process.
                List <EstentityEstimateEntityDB> darrestentity = (
                    from estentity in context.Estimate
                    join wfentity in context.Workflow
                    on estentity.intPkWorkflow equals wfentity.intPk
                    join piwentity in context.ProcessInWorkflow
                    on wfentity.intPk equals piwentity.intPkWorkflow
                    where piwentity.intPkProcess == intPkElementDad &&
                    wfentity.boolDeleted == false
                    select estentity).ToList();

                String strJobNumber;
                bool boolUpdateJobAndEstimate = false;
                //                                          //Construction of the message;
                String strJobs = "";
                foreach (EstentityEstimateEntityDB estentity in darrestentity)
                {
                    //                                      //Get strJobNumber
                    JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
                        job.intJobID == estentity.intJobId);

                    if (
                        jobjsonentity == null &&
                        !boolUpdateJobAndEstimate
                        )
                    {
                        //                                  //using is to release connection at the end
                        using (Odyssey2Context contextT = new Odyssey2Context())
                        {
                            //                              //Starts a new transaction.
                            using (var dbContextTransaction = contextT.Database.BeginTransaction())
                            {
                                int intStatus = 0; String strUserMessage = "", strDevMessage = "";
                                try
                                {
                                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                                    List<JobjsonJobJson> darrjobjson;
                                    JobJob.subGetPrintshopJobs(ps, null, true,
                                        null, null, null, null, null, null, configuration_I, IHubContext_I, contextT,
                                        out darrjobjson, ref intStatus, ref strUserMessage, ref strDevMessage);

                                    if (
                                        intStatus == 200
                                        )
                                    {
                                        List<EstimjsonEstimateJson> darrestimjson;
                                        JobJob.subGetPrintshopEstimates(ps, true, false,
                                            false, configuration_I, out darrestimjson, contextT, ref intStatus,
                                            ref strUserMessage, ref strDevMessage);
                                    }

                                    //                      //Commits all changes made to the database in the current
                                    //                      //      transaction.
                                    if (
                                        intStatus == 200
                                        )
                                    {
                                        dbContextTransaction.Commit();
                                        boolUpdateJobAndEstimate = true;
                                    }
                                    else
                                    {
                                        dbContextTransaction.Rollback();
                                        throw new Exception("Something Wrong.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //                      //Discards all changes made to the database in the current
                                    //                      //      transaction.
                                    dbContextTransaction.Rollback();

                                    //                      //Making a log for the exception.
                                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                                    throw new Exception("Something Wrong.");
                                }
                            }
                        }

                        jobjsonentity = context.JobJson.FirstOrDefault(job =>
                            job.intJobID == estentity.intJobId);
                    }

                    if (
                        jobjsonentity != null
                        )
                    {
                        if (
                            jobjsonentity.intnEstimateNumber != null
                            )
                        {
                            strJobNumber = jobjsonentity.intnEstimateNumber.ToString();
                        }
                        else
                        {
                            strJobNumber = jobjsonentity.intnOrderNumber.ToString() + " - " +
                                jobjsonentity.intnJobNumber.ToString();
                        }

                        strJobs = strJobs.Contains(strJobNumber + "") ? strJobs : strJobs + strJobNumber +
                            ", ";
                    }
                }
                strJobsWithEstimates = strJobsWithEstimates.TrimEnd(' ', ',');
            }

            boolTypeIsDispensable_O = (strProducts == "") && (strJobsWithEstimates == "");

            strMessage_O = "";
            /*CASE*/
            if (
                strProducts != "" && strJobsWithEstimates == ""
                )
            {
                strMessage_O = "Some links for products: " + strProducts + ", will be deleted.";
            }
            else if (
                strProducts == "" && strJobsWithEstimates != ""
                )
            {
                strMessage_O = strJobsWithEstimates != "" ?
                    "Some estimates for jobs: " + strJobsWithEstimates + ", will be deleted." :
                    "Some estimates will be deleted. ";
            }
            else if (
                strProducts != "" && strJobsWithEstimates != ""
                )
            {
                strMessage_O = strJobsWithEstimates != "" ?
                "Some links for products: " + strProducts + " and some estimates for jobs" +
                    ": " + strJobsWithEstimates + ", will be deleted." :
                    "Some links for products: " + strProducts + " and some estimates will be deleted.";
            }
            /*END-CASE*/
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshopProcesses(
            String strPrintshopId_I,
            out List<Proelejson1ProcessElementJson1> darrprojson_O,
            //                                              //Status:
            //                                              //      200 - Success.
            //                                              //      401 - Something is wrong. (Printshop not found.)
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Create the list for the processes.
            darrprojson_O = new List<Proelejson1ProcessElementJson1>();

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No processes found.";
            if (
                //                                          //Verify if the printshop is not null.
                ps != null
                )
            {
                //                                          //Bring the dictionary of processes.
                Dictionary<int, ProProcess> dicpro = ps.dicpro;
                foreach (KeyValuePair<int, ProProcess> pro in dicpro)
                {
                    //                                      //Create the json for processes.
                    Proelejson1ProcessElementJson1 projson = new Proelejson1ProcessElementJson1(pro.Value.intPk,
                        pro.Value.strName);
                    darrprojson_O.Add(projson);
                }

                darrprojson_O = darrprojson_O.OrderBy(pro=> pro.strElementName).ToList();

                intStatus_IO = 200;
                strUserMessage_IO = "Success";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetPeriod(
            int? intnPkPeriod_I,
            String strPassword_I,
            String strPrintshopId_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            int intPkCalculation_I,
            int? intnContactId_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            //                                              //True -> this method is used to confirm temporary periods.
            //                                              //False -> this method is used from the controller,
            //                                              //      to verify if a period from job workflow is addable.  
            bool boolIsTemporary_I,
            int  intMinsBeforeDelete_I,
            IConfiguration configuration_I,
            out CalperjsonCalculationPeriodJson calperjson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            calperjson_O = null;
            JobjsonJobJson jobjson;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson, 
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                PerentityPeriodEntityDB perentityOld = context.Period.FirstOrDefault(per =>
                    per.intPk == intnPkPeriod_I);
                if (
                    perentityOld != null
                    )
                {
                    CalentityCalculationEntityDB calentityOld = context.Calculation.FirstOrDefault(cal =>
                        cal.intPk == perentityOld.intnPkCalculation);

                    int intAllSeconds = ((int)calentityOld.intnHours * 3600) + ((int)calentityOld.intnMinutes *
                        60) + (int)calentityOld.intnSeconds;
                    if (
                        calentityOld.strCalculationType == CalCalculation.strPerQuantity
                        )
                    {
                        intAllSeconds = (int)((((calentityOld.numnNeeded / calentityOld.numnPerUnits) *
                            (double)jobjson.intnQuantity)) * ((double)(((int)calentityOld.intnHours * 3600) +
                            ((int)calentityOld.intnMinutes * 60) + ((int)calentityOld.intnSeconds)) /
                            (calentityOld.numnQuantity)));
                    }

                    //                                      //Seconds to hours.
                    int intHours = (int)(intAllSeconds / 3600);
                    //                                      //Seconds to minutes.
                    int intMinutes = (int)((intAllSeconds % 3600) / 60);
                    //                                      //Seconds to seconds.
                    int intSeconds = (intAllSeconds % 3600) % 60;

                    //                                      //Get boolean for period started and completed.
                    bool boolPeriodStarted = (perentityOld.strFinalStartDate != null &&
                           perentityOld.strFinalEndDate == null) ? true : false;
                    bool boolPeriodCompleted = (perentityOld.strFinalStartDate != null &&
                        perentityOld.strFinalEndDate != null) ? true : false;

                    calperjson_O = new CalperjsonCalculationPeriodJson(calentityOld.intPk,
                        calentityOld.strDescription, intHours, intMinutes, intSeconds, perentityOld.intPk,
                        perentityOld.strStartDate, perentityOld.strStartTime, perentityOld.strEndDate,
                        perentityOld.strEndTime, null, null, perentityOld.intnContactId, false,
                        perentityOld.intMinsBeforeDelete, boolPeriodStarted, boolPeriodCompleted, null);
                }

                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 404;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No process in workflow found.";
                if (
                    piwentity != null
                    )
                {
                    //                                      //If process is already completed, period can not be set.
                    //                                      //Verify process status.
                    PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                        context.ProcessInWorkflowForAJob.FirstOrDefault(piwj => piwj.intJobId == intJobId_I &&
                        piwj.intPkProcessInWorkflow == piwentity.intPk);

                    bool boolProcessCompled = false;
                    bool boolProcessStarted = false;
                    if (
                        piwjentity != null
                        )
                    {
                        boolProcessCompled = (piwjentity.intStage == JobJob.intProcessInWorkflowCompleted);
                        boolProcessStarted = (piwjentity.intStage == JobJob.intProcessInWorkflowStarted);
                    }

                    intStatus_IO = 413;
                    strUserMessage_IO = boolProcessCompled ? "Cannot set a period to a completed Process." :
                    "Cannot set a period to a started Process.";

                    strDevMessage_IO = "Process is already completed.";
                    if (
                        !boolProcessCompled &&
                        !boolProcessStarted
                        )
                    {
                        CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(cal =>
                            cal.intPk == intPkCalculation_I && cal.intnPkWorkflow != null &&
                            cal.intnProcessInWorkflowId != null && cal.intnPkProcess != null &&
                            cal.boolIsEnable == true);

                        intStatus_IO = 405;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "No calculation found.";
                        if (
                            calentity != null
                            )
                        {
                            intStatus_IO = 406;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Invalid format for dates and times.";
                            if (
                                strStartDate_I.IsParsableToDate() &&
                                strStartTime_I.IsParsableToTime() &&
                                strEndDate_I.IsParsableToDate() &&
                                strEndTime_I.IsParsableToTime()
                                )
                            {
                                intStatus_IO = 405;
                                strUserMessage_IO = "Somthing wrong.";
                                strDevMessage_IO = "No employee found.";
                                if (
                                    (intnContactId_I == null) ||
                                    ResResource.boolEmployeeIsFromPrintshop(strPrintshopId_I,
                                    (int)intnContactId_I)
                                    )
                                {
                                    //                      //To easy code.
                                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                                    ZonedTime ztimeStartPeriod = 
                                        ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                                        strStartDate_I.ParseToDate(), strStartTime_I.ParseToTime(), ps.strTimeZone);
                                    strStartDate_I = ztimeStartPeriod.Date.ToString();
                                    strStartTime_I = ztimeStartPeriod.Time.ToString();

                                    ZonedTime ztimeEndPeriod = 
                                        ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                                        strEndDate_I.ParseToDate(), strEndTime_I.ParseToTime(), ps.strTimeZone);
                                    strEndDate_I = ztimeEndPeriod.Date.ToString();
                                    strEndTime_I = ztimeEndPeriod.Time.ToString();

                                    intStatus_IO = 407;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "Invalid format for dates and times.";
                                    if (
                                        ztimeEndPeriod > ztimeStartPeriod
                                        )
                                    {
                                        intStatus_IO = 408;
                                        strUserMessage_IO = "The period must be greater than the current time.";
                                        strDevMessage_IO = "";
                                        if (
                                            ztimeStartPeriod >= ZonedTimeTools.ztimeNow
                                            )
                                        {
                                            //              //Get job's correct processes.
                                            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                                            List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                                            ProdtypProductType.subGetWorkflowValidWay(piwentity.intPkWorkflow, jobjson,
                                                out darrpiwentityAllProcesses, out darrdynlkjson);

                                            intStatus_IO = 409;
                                            strUserMessage_IO = "The times are for an unavailable period of time.";
                                            strDevMessage_IO = "";
                                            if (
                                                ProProcess.boolIsValidConsideringThePreviousProcesses(
                                                    intPkProcessInWorkflow_I, intJobId_I, ztimeStartPeriod,
                                                    boolIsTemporary_I, darrpiwentityAllProcesses)
                                                )
                                            {
                                                ////          //Get printshop register.
                                                //PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                                                intStatus_IO = 410;
                                                strUserMessage_IO = "The times are for an unavailable period" +
                                                    " of time or incorrect password.";

                                                strDevMessage_IO = "";
                                                bool boolIsValidForEmployee = ProProcess.boolEmployeeIsValid(
                                                    strPrintshopId_I, intnContactId_I, configuration_I,
                                                    ztimeStartPeriod, ztimeEndPeriod);

                                                bool boolIsValidConsideringTheRules = ProProcess.
                                                    boolIsValidConsideringTheRules(strPrintshopId_I,
                                                    ztimeStartPeriod, ztimeEndPeriod);
                                                if (
                                                    boolIsValidConsideringTheRules ||
                                                    (!boolIsValidConsideringTheRules &&
                                                    strPassword_I == ps.strSpecialPassword) ||
                                                    boolIsValidForEmployee ||
                                                    (!boolIsValidForEmployee &&
                                                    strPassword_I == ps.strSpecialPassword)
                                                    )
                                                {
                                                    bool boolIsException = false;
                                                    if (
                                                        (!boolIsValidForEmployee &&
                                                        strPassword_I == ps.strSpecialPassword) ||
                                                        (!boolIsValidConsideringTheRules &&
                                                        strPassword_I == ps.strSpecialPassword)
                                                        )
                                                    {
                                                        boolIsException = true;
                                                    }

                                                    String strDeleteDate;
                                                    String strDeleteHour;
                                                    String strDeleteMinute;
                                                    intStatus_IO = 411;
                                                    if (
                                                        ProProcess.boolIsMinsBeforeDeleteCorrect(
                                                            strStartDate_I, strStartTime_I, strEndDate_I,
                                                            strEndTime_I, intMinsBeforeDelete_I,
                                                            out strDeleteDate, out strDeleteHour,
                                                            out strDeleteMinute, ref strUserMessage_IO)
                                                        )
                                                    {
                                                        if (
                                                            //  //Is a new register
                                                            intnPkPeriod_I == null
                                                            )
                                                        {
                                                            List<PerentityPeriodEntityDB> darrperentity =
                                                                context.Period.Where(per =>
                                                                per.intnPkCalculation == intPkCalculation_I &&
                                                                per.intJobId == intJobId_I).ToList();

                                                            intStatus_IO = 412;
                                                            strUserMessage_IO = "Somethig wrong.";
                                                            strDevMessage_IO = "There is already a period " +
                                                                "associated with the calculation.";
                                                            if (
                                                                darrperentity.Count() == 0
                                                                )
                                                            {
                                                                PerentityPeriodEntityDB perentity = new
                                                                        PerentityPeriodEntityDB
                                                                {
                                                                    strStartDate = strStartDate_I,
                                                                    strStartTime = strStartTime_I,
                                                                    strEndDate = strEndDate_I,
                                                                    strEndTime = strEndTime_I,
                                                                    intJobId = intJobId_I,
                                                                    intPkWorkflow = piwentity.intPkWorkflow,
                                                                    intProcessInWorkflowId = piwentity.
                                                                    intProcessInWorkflowId,
                                                                    intPkElement = piwentity.intPkProcess,
                                                                    intnPkCalculation = intPkCalculation_I,
                                                                    boolIsException = boolIsException,
                                                                    intnContactId = intnContactId_I,
                                                                    intMinsBeforeDelete = intMinsBeforeDelete_I,
                                                                    strDeleteDate = strDeleteDate,
                                                                    strDeleteHour = strDeleteHour,
                                                                    strDeleteMinute = strDeleteMinute
                                                                };
                                                                context.Period.Add(perentity);

                                                                String strProcesses;
                                                                List<int> darrintPkPeriodToDelete;
                                                                if (
                                                                    !ProProcess.boolIsValidConsideringTheNextProcesses(
                                                                        intPkProcessInWorkflow_I, intJobId_I,
                                                                        ztimeEndPeriod, boolIsTemporary_I, 
                                                                        darrpiwentityAllProcesses,
                                                                        out strProcesses, out darrintPkPeriodToDelete)
                                                                    )
                                                                {
                                                                    foreach (int intPkPeriod in darrintPkPeriodToDelete)
                                                                    {
                                                                        PerentityPeriodEntityDB perentityToDelete =
                                                                            context.Period.FirstOrDefault(per =>
                                                                            per.intPk == intPkPeriod);
                                                                        context.Period.Remove(perentityToDelete);
                                                                    }
                                                                }
                                                                context.SaveChanges();

                                                                calperjson_O = calperjsonCalculatePeriod(intnContactId_I,
                                                                    strPrintshopId_I, jobjson, perentity, calentity,
                                                                    context);

                                                                intStatus_IO = 200;
                                                                strUserMessage_IO = "";
                                                                strDevMessage_IO = "";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //It's an update.

                                                            //Find period.
                                                            PerentityPeriodEntityDB perentity =
                                                                context.Period.FirstOrDefault(per =>
                                                                    per.intPk == intnPkPeriod_I);

                                                            intStatus_IO = 413;
                                                            strUserMessage_IO = "Somethig wrong.";
                                                            strDevMessage_IO = "Period not found.";
                                                            if (
                                                                perentity != null
                                                                )
                                                            {
                                                                //Update period.
                                                                perentity.strStartDate = strStartDate_I;
                                                                perentity.strStartTime = strStartTime_I;
                                                                perentity.strEndDate = strEndDate_I;
                                                                perentity.strEndTime = strEndTime_I;
                                                                perentity.boolIsException = boolIsException;
                                                                perentity.intnContactId = intnContactId_I;
                                                                perentity.intMinsBeforeDelete =
                                                                    intMinsBeforeDelete_I;
                                                                perentity.strDeleteDate = strDeleteDate;
                                                                perentity.strDeleteHour = strDeleteHour;
                                                                perentity.strDeleteMinute = strDeleteMinute;

                                                                context.Period.Update(perentity);

                                                                String strProcesses;
                                                                List<int> darrintPkPeriodToDelete;
                                                                if (
                                                                    !ProProcess.boolIsValidConsideringTheNextProcesses(
                                                                        intPkProcessInWorkflow_I, intJobId_I,
                                                                        ztimeEndPeriod, boolIsTemporary_I, 
                                                                        darrpiwentityAllProcesses,
                                                                        out strProcesses, out darrintPkPeriodToDelete)
                                                                    )
                                                                {
                                                                    foreach (int intPkPeriod in darrintPkPeriodToDelete)
                                                                    {
                                                                        PerentityPeriodEntityDB perentityToDelete =
                                                                            context.Period.FirstOrDefault(per =>
                                                                            per.intPk == intPkPeriod);

                                                                        context.Period.Remove(perentityToDelete);
                                                                    }
                                                                }
                                                                context.SaveChanges();

                                                                calperjson_O = calperjsonCalculatePeriod(intnContactId_I,
                                                                    strPrintshopId_I, jobjson, perentity, calentity, 
                                                                    context);

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
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strEstimateDateJob(
            //                                              //Return the EstimatedDate of the Job.

            int intPKWorkflow_I,
            String strPrintshopId_I,
            JobjsonJobJson jobjsonJob_I,
            Odyssey2Context context_I
            )
        {
            String strEstimatedDate = "";

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            //                                              //Init values necesary.
            ZonedTime ztimeTemporary = ZonedTimeTools.NewZonedTime("1000-01-01".ParseToDate(),
                            "01:00:00".ParseToTime());
            ZonedTime ztimeEstimateDate = ZonedTimeTools.NewZonedTime("1000-01-01".ParseToDate(),
                            "01:00:00".ParseToTime());

            //                                              //Get job's correct processes.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
            List<DynLkjsonDynamicLinkJson> darrdynlkjson;
            ProdtypProductType.subGetWorkflowValidWay(intPKWorkflow_I, jobjsonJob_I,
                out darrpiwentityAllProcesses, out darrdynlkjson);

            if (
                //                                          //There are process valids.
                darrpiwentityAllProcesses != null &&
                darrpiwentityAllProcesses.Count > 0
                )
            {
                //                                          //Get all periods from the Job.
                List<PerentityPeriodEntityDB> darrperentityPeriodsFromJob = context_I.Period.Where(per =>
                    per.intJobId == jobjsonJob_I.intJobId && per.intPkWorkflow == intPKWorkflow_I).ToList();

                List<CalentityCalculationEntityDB> darrcalentity = (
                    from cal in context_I.Calculation
                    join per in context_I.Period
                    on cal.intPk equals per.intnPkCalculation
                    where per.intJobId == jobjsonJob_I.intJobId && per.intPkWorkflow == intPKWorkflow_I
                    select cal).ToList();

                //                                          //Take each Period of the Job.
                foreach (PerentityPeriodEntityDB perentityPeriodFromJob in darrperentityPeriodsFromJob)
                {
                    if (
                        //                                  //It is a period for a process valid.
                        darrpiwentityAllProcesses.Exists(piw => 
                            piw.intProcessInWorkflowId == perentityPeriodFromJob.intProcessInWorkflowId)
                        )
                    {
                        bool boolIsApplicablePeriod = true;

                        if (
                            //                              //Period is of the Process.
                            perentityPeriodFromJob.intnPkElementElement == null &&
                            perentityPeriodFromJob.intnPkElementElement == null &&
                            perentityPeriodFromJob.intnPkCalculation != null
                            )
                        {
                            //                              //Get the calculation.
                            CalentityCalculationEntityDB calentityFromProcess = darrcalentity.FirstOrDefault(
                                cal => cal.intPk == perentityPeriodFromJob.intnPkCalculation);

                            if (
                                //                          //Not apply the calculation for this Job.
                                !Tools.boolCalculationOrLinkApplies(calentityFromProcess.intPk, null, null, null, 
                                jobjsonJob_I)
                                )
                            {
                                boolIsApplicablePeriod = false;
                            }
                        }

                        if (
                            //                              //It is a valid period of resource or process.
                            boolIsApplicablePeriod
                            )
                        {
                            //                              //Get the estimateDate of this period.
                            ZonedTime ztimeEstimateDatePeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                perentityPeriodFromJob.strEndDate.ParseToDate(),
                                perentityPeriodFromJob.strEndTime.ParseToTime(), ps.strTimeZone);

                            //                              //Get the last EstimateDate.
                            ztimeEstimateDate = (ztimeEstimateDatePeriod > ztimeEstimateDate ?
                            ztimeEstimateDatePeriod : ztimeEstimateDate);
                        }
                    } 
                }
            }

            //                                              //Get the estimate date.
            strEstimatedDate = ztimeEstimateDate == ztimeTemporary ? "" : ztimeEstimateDate.Date.ToString();

            return strEstimatedDate;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsMinsBeforeDeleteCorrect(
            //                                              //Compares tolerance time to time range.
            //                                              //Verify tolerance time exists.
            //                                              //Calculates delete date, hour and minute values.

            //                                              //Time range.
            String strStartDate_I, 
            String strStartTime_I, 
            String strEndDate_I,
            String strEndTime_I, 
            //                                              //This is also called tolerance time.
            int intMinsBeforeDelete_I,
            out String strDeleteDate_O,
            out String strDeleteHour_O,
            out String strDeleteMinute_O,
            ref String strUserMessage_IO
            )
        {
            bool boolIsMinsBeforeDeleteCorrect = false;
            strDeleteDate_O = null;
            strDeleteHour_O = null;
            strDeleteMinute_O = null;

            //                                              //Compare mins before delete with actual duration time.

            //                                              //Create ztime objects.
            ZonedTime ztimeStartTime = ZonedTimeTools.NewZonedTime(strStartDate_I.ParseToDate(),
                       strStartTime_I.ParseToTime());
            ZonedTime ztimeEndTime = ZonedTimeTools.NewZonedTime(strEndDate_I.ParseToDate(),
                       strEndTime_I.ParseToTime());
            //                                              //Get duration time in miliseconds.
            double numMillisecondsTimeDuration = ztimeEndTime - ztimeStartTime;
            //                                              //Minutes to milliseconds.
            int intMillisecondsBeforeDelete = intMinsBeforeDelete_I * 60000;

            strUserMessage_IO = "Schedule Reset Allowance must be greater than 0 and shorter than the whole period.";
            if (
                intMinsBeforeDelete_I > 0 &&
                //                                          //Duration time must be greater than tolerance time.
                numMillisecondsTimeDuration > intMillisecondsBeforeDelete
                )
            {
                //                                          //Calculate new start time using tolerance time.

                ztimeStartTime = ztimeStartTime + intMillisecondsBeforeDelete;
                String strTime = ztimeStartTime.Time.ToString();

                //                                          //Calculate delete date and time.

                strDeleteDate_O = ztimeStartTime.Date.ToString();
                strDeleteHour_O = strTime.Substring(0, 2);
                strDeleteMinute_O = strTime.Substring(3, 2);

                boolIsMinsBeforeDeleteCorrect = true;
            }

            return boolIsMinsBeforeDeleteCorrect;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static CalperjsonCalculationPeriodJson calperjsonCalculatePeriod(

            int? intnContactId_I,
            String strPrintshopId_I,
            JobjsonJobJson jobjson_I,
            PerentityPeriodEntityDB perentity_I,
            CalentityCalculationEntityDB calentity_I, 
            Odyssey2Context context_I
            )
        {
            int intAllSeconds = ((int)calentity_I.intnHours * 3600) + ((int)calentity_I.intnMinutes * 60) +
                (int)calentity_I.intnSeconds;
            if (
                calentity_I.strCalculationType == CalCalculation.strPerQuantity
                )
            {
                intAllSeconds = (int)(
                    (((calentity_I.numnNeeded / calentity_I.numnPerUnits) * ((double)jobjson_I.intnQuantity))) * 
                    ((double)(((int)calentity_I.intnHours * 3600) +  ((int)calentity_I.intnMinutes * 60) + 
                    ((int)calentity_I.intnSeconds)) / (calentity_I.numnQuantity)));
            }

            //                                              //Seconds to hours.
            int intHours = (int)(intAllSeconds / 3600);
            //                                              //Seconds to minutes.
            int intMinutes = (int)((intAllSeconds % 3600) / 60);
            //                                              //Seconds to seconds.
            int intSeconds = (intAllSeconds % 3600) % 60;

            //                                              //Get boolean for period started and completed.
            bool boolPeriodStarted = (perentity_I.strFinalStartDate != null &&
                   perentity_I.strFinalEndDate == null) ? true : false;
            bool boolPeriodCompleted = (perentity_I.strFinalStartDate != null &&
                perentity_I.strFinalEndDate != null) ? true : false;

            //                                              //Get Estimate date for the job.
            String strEstimateDate = ProProcess.strEstimateDateJob(perentity_I.intPkWorkflow, strPrintshopId_I, 
                jobjson_I, context_I);


            CalperjsonCalculationPeriodJson calperjson = new CalperjsonCalculationPeriodJson( calentity_I.intPk,
                calentity_I.strDescription, intHours, intMinutes, intSeconds, perentity_I.intPk, 
                perentity_I.strStartDate, perentity_I.strStartTime, perentity_I.strEndDate, perentity_I.strEndTime, 
                null, null, intnContactId_I, false, perentity_I.intMinsBeforeDelete, boolPeriodStarted,
                boolPeriodCompleted, strEstimateDate);

            return calperjson;
        }
        //--------------------------------------------------------------------------------------------------------------
        public static void subPeriodIsAddable(
            String strPrintshopId_I,
            int intJobId_I,
            int? intnPkPeriod_I,
            int intPkProcessInWorkflow_I,
            int intPkCalculation_I,
            int? intnContactId_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            //                                              //True -> this method is used to confirm temporary periods.
            //                                              //Periods from the same JobId are not considered.
            //                                              //False -> this method is used from the controller (front),
            //                                              //      to verify if a period from job workflow is addable. 
            //                                              //All periods including this job periods are considered.
            bool boolIsTemporary_I,
            IConfiguration configuration_I,
            out PerisaddablejsonPeriodIsAddableJson perisaddablejson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            perisaddablejson_O = null;

            JobjsonJobJson jobjson = null;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(
                    intJobId_I, strPrintshopId_I, configuration_I, out jobjson, ref strUserMessage_IO, 
                    ref strDevMessage_IO
                    )
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                PerentityPeriodEntityDB perentityOld = context.Period.FirstOrDefault(per =>
                    per.intPk == intnPkPeriod_I);
                if (
                    perentityOld != null
                    )
                {
                    CalentityCalculationEntityDB calentityOld = context.Calculation.FirstOrDefault(cal =>
                    cal.intPk == perentityOld.intnPkCalculation);

                    int intAllSeconds = ((int)calentityOld.intnHours * 3600) + ((int)calentityOld.intnMinutes * 60) +
                            (int)calentityOld.intnSeconds;
                    if (
                        calentityOld.strCalculationType == CalCalculation.strPerQuantity
                        )
                    {
                        intAllSeconds = (int)((((calentityOld.numnNeeded / calentityOld.numnPerUnits) *
                            (double)jobjson.intnQuantity)) * ((double)(((int)calentityOld.intnHours * 3600) +
                            ((int)calentityOld.intnMinutes * 60) + ((int)calentityOld.intnSeconds)) /
                            (calentityOld.numnQuantity)));
                    }

                    int intHours = (int)(intAllSeconds / 3600);
                    int intMinutes = (int)((intAllSeconds % 3600) / 60);
                    int intSeconds = (intAllSeconds % 3600) % 60;

                    ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        perentityOld.strStartDate.ParseToDate(), perentityOld.strStartTime.ParseToTime(),
                        ps.strTimeZone);

                    ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        perentityOld.strEndDate.ParseToDate(), perentityOld.strEndTime.ParseToTime(), ps.strTimeZone);

                    perisaddablejson_O = new PerisaddablejsonPeriodIsAddableJson(false, false, false,
                        calentityOld.intPk, calentityOld.strDescription, intHours, intMinutes, intSeconds,
                        perentityOld.intPk, ztimeStartPeriod.Date.ToString(), ztimeStartPeriod.Time.ToString(),
                        ztimeEndPeriod.Date.ToString(), ztimeEndPeriod.Time.ToString());
                }

                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 404;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No process in workflow found.";
                if (
                    piwentity != null
                    )
                {
                    //                                  //If process is already completed, period can not be set.
                    //                                  //Verify process status.
                    PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                        context.ProcessInWorkflowForAJob.FirstOrDefault(piwj => piwj.intJobId == intJobId_I &&
                        piwj.intPkProcessInWorkflow == piwentity.intPk);

                    bool boolProcessCompled = false;
                    if (
                        piwjentity != null
                        )
                    {
                        boolProcessCompled = (
                            piwjentity.intStage == JobJob.intProcessInWorkflowCompleted
                            );
                    }

                    intStatus_IO = 410;
                    strUserMessage_IO = "Can not set a period to a Completed Process.";
                    strDevMessage_IO = "Process is already completed.";
                    if (
                        !boolProcessCompled
                        )
                    {
                        CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(cal =>
                        cal.intPk == intPkCalculation_I && cal.intnPkWorkflow != null &&
                        cal.intnProcessInWorkflowId != null && cal.intnPkProcess != null);

                        intStatus_IO = 405;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "No calculation found.";
                        if (
                            calentity != null
                            )
                        {
                            intStatus_IO = 405;
                            strUserMessage_IO = "Somthing wrong.";
                            strDevMessage_IO = "No employee found.";
                            if (
                                (intnContactId_I == null) ||
                                ResResource.boolEmployeeIsFromPrintshop(strPrintshopId_I,
                                (int)intnContactId_I)
                                )
                            {
                                //                          //To easy code.
                                ZonedTime ztimeStartPeriodToAdd = new ZonedTime();
                                ZonedTime ztimeEndPeriodToAdd = new ZonedTime();

                                intStatus_IO = 403;
                                if (
                                    ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTime(strStartDate_I, strStartTime_I,
                                    strEndDate_I, strEndTime_I, ps.strTimeZone, out ztimeStartPeriodToAdd,
                                    out ztimeEndPeriodToAdd, ref strUserMessage_IO, ref strDevMessage_IO)
                                    )
                                {
                                    //              //Get job's correct processes.
                                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                                    List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                                    ProdtypProductType.subGetWorkflowValidWay(piwentity.intPkWorkflow, jobjson,
                                        out darrpiwentityAllProcesses, out darrdynlkjson);

                                    intStatus_IO = 409;
                                    strUserMessage_IO = "Previous processes will not have been completed by" +
                                            " this time. Please set a different period.";
                                    strDevMessage_IO = "";
                                    if (
                                        ProProcess.boolIsValidConsideringThePreviousProcesses(intPkProcessInWorkflow_I, 
                                            intJobId_I, ztimeStartPeriodToAdd, boolIsTemporary_I, 
                                            darrpiwentityAllProcesses)
                                        )
                                    {
                                        intStatus_IO = 200;
                                        strDevMessage_IO = "";
                                        strUserMessage_IO = "";

                                        bool boolIsAddableAboutPeriods = true;

                                        String strProcesses;
                                        List<int> darrintPkPeriodToDelete;
                                        if (
                                            !ProProcess.boolIsValidConsideringTheNextProcesses(
                                                intPkProcessInWorkflow_I, intJobId_I, ztimeEndPeriodToAdd,
                                                boolIsTemporary_I, darrpiwentityAllProcesses, out strProcesses, 
                                                out darrintPkPeriodToDelete)
                                            )
                                        {
                                            String strForProcesses;
                                            ProProcess.subCreateTheStringsForProcessesAndResources(
                                                strProcesses, out strForProcesses);

                                            boolIsAddableAboutPeriods = false;
                                            strUserMessage_IO = strForProcesses;
                                        }

                                        bool boolIsAddableAboutEmployeesPeriods = true;
                                        if (
                                            intnContactId_I != null
                                            )
                                        {
                                            boolIsAddableAboutEmployeesPeriods = ProProcess.
                                                boolGetIsAddableAboutEmployeesPeriods((int)intnContactId_I,
                                                ztimeStartPeriodToAdd, ztimeEndPeriodToAdd, intnPkPeriod_I);
                                        }

                                        if (
                                            !boolIsAddableAboutEmployeesPeriods
                                            )
                                        {
                                            strUserMessage_IO = !boolIsAddableAboutPeriods ? 
                                                strUserMessage_IO.TrimEnd('.') + ", " +
                                                "This period overlaps with another employee's period." :
                                                "This period overlaps with another employee's period.";
                                        }

                                        bool boolIsValidForEmployee = ProProcess.boolEmployeeIsValid(
                                        strPrintshopId_I, intnContactId_I, configuration_I,
                                        ztimeStartPeriodToAdd, ztimeEndPeriodToAdd);

                                        bool boolIsAddableAboutRules = true;
                                        if (
                                            !ProProcess.boolIsValidConsideringTheRules(strPrintshopId_I,
                                            ztimeStartPeriodToAdd, ztimeEndPeriodToAdd) ||
                                            !boolIsValidForEmployee
                                            )
                                        {
                                            boolIsAddableAboutRules = false;

                                            if (
                                                !boolIsAddableAboutPeriods
                                                )
                                            {
                                                strUserMessage_IO = strUserMessage_IO.TrimEnd('.') + " and " +
                                                    "the times are for an unavailable period of time. Set the" +
                                                    " password to set the period.";
                                            }
                                            else
                                            {
                                                strUserMessage_IO = "The times are for an unavailable period " +
                                                    "of time. Set the password to set the period.";
                                            }
                                        }

                                        if (
                                            perisaddablejson_O != null
                                        )
                                        {
                                            perisaddablejson_O.boolIsAddableAboutPeriods = boolIsAddableAboutPeriods;
                                            perisaddablejson_O.boolIsAddableAboutRules = boolIsAddableAboutRules;
                                            perisaddablejson_O.boolIsAddableAboutEmployeesPeriods = 
                                                boolIsAddableAboutEmployeesPeriods;
                                        }
                                        else
                                        {
                                            perisaddablejson_O = new PerisaddablejsonPeriodIsAddableJson(
                                                boolIsAddableAboutPeriods, boolIsAddableAboutRules, 
                                                boolIsAddableAboutEmployeesPeriods, null, null, null, null, null, null,
                                                null, null, null, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidConsideringTheRules(
            String strPrintshopId_I,
            ZonedTime ztimeStartPeriod_I,
            ZonedTime ztimeEndPeriod_I
            )
        {
            bool boolIsAddable = true;

            Odyssey2Context context = new Odyssey2Context();

            //                                              //To easy code.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            //                                              //Get the printshop rules.
            List<RuleentityRuleEntityDB> darrruleentity = context.Rule.Where(rule =>
                rule.intnPkPrintshop == ps.intPk &&
                rule.intnPkResource == null).ToList();

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= darrruleentity.Count()) ||
                !boolIsAddable
                ))
            {
                //                                          //To easy code.
                RuleentityRuleEntityDB ruleentity = darrruleentity[intI];

                if (
                    ruleentity.strFrecuency == ResResource.strOnce
                    )
                {
                    //                                      //start and end date of the rule.
                    Date dateStartRule = (ruleentity.strFrecuencyValue.Substring(0, 10)).ParseToDate();
                    Date dateEndRule = (ruleentity.strFrecuencyValue.Substring(11, 10)).ParseToDate();

                    //                                      //start and end of the rule.
                    ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(dateStartRule,
                        ruleentity.strStartTime.ParseToTime());

                    ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(dateEndRule,
                        ruleentity.strEndTime.ParseToTime());
                    if (
                        ((ztimeStartPeriod_I >= ztimeStartRule) && (ztimeStartPeriod_I < ztimeEndRule)) ||
                        ((ztimeEndPeriod_I > ztimeStartRule) && (ztimeEndPeriod_I <= ztimeEndRule))
                        )
                    {
                        boolIsAddable = false;
                    }
                }
                else
                {
                    //                                      //To easy code.
                    ZonedTime ztimeRuleRangeStart = ZonedTimeTools.NewZonedTime(ruleentity.strRangeStartDate.
                        ParseToDate(), ruleentity.strRangeStartTime.ParseToTime());
                    ZonedTime ztimeRuleRangeEnd = ZonedTimeTools.NewZonedTime(ruleentity.strRangeEndDate.ParseToDate(),
                        ruleentity.strRangeEndTime.ParseToTime());

                    if (
                        //                                  //The period is between the start and end of the range.
                        ((ztimeStartPeriod_I >= ztimeRuleRangeStart) && (ztimeStartPeriod_I < ztimeRuleRangeEnd)) ||
                        ((ztimeEndPeriod_I > ztimeRuleRangeStart) && (ztimeEndPeriod_I <= ztimeRuleRangeEnd))
                        )
                    {
                        int intJ = 0;
                        Date date = ztimeStartPeriod_I.Date;
                        /*UNTIL-DO*/
                        while (!(
                            ((date + intJ) > ztimeEndPeriod_I.Date) ||
                            !boolIsAddable
                            ))
                        {
                            ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(date,
                                ruleentity.strStartTime.ParseToTime());
                            ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(date,
                                ruleentity.strEndTime.ParseToTime());

                            if (
                                //                          //DAILY RULE.
                                ((ruleentity.strFrecuency == ResResource.strDaily) ||
                                //                          //WEEKLY.
                                ((ruleentity.strFrecuency == ResResource.strWeekly) &&
                                (ruleentity.strFrecuencyValue[(int)date.DayOfWeek] == '1')) ||
                                //                          //MONTHLY.
                                ((ruleentity.strFrecuency == ResResource.strMonthly) &&
                                (ruleentity.strFrecuencyValue[date.Day - 1] == '1')) ||
                                //                          //ANNUALLY.
                                ((ruleentity.strFrecuency == ResResource.strAnnually) &&
                                (ruleentity.strFrecuencyValue == date.ToString("MMdd")))) &&
                                //                          //The start is over the rule.
                                (((ztimeStartPeriod_I >= ztimeStartRule) && (ztimeStartPeriod_I < ztimeEndRule)) ||
                                //                          //The end is over the rule.
                                ((ztimeEndPeriod_I > ztimeStartRule) && (ztimeEndPeriod_I <= ztimeEndRule)))
                                )
                            {
                                boolIsAddable = false;
                            }

                            intJ = intJ + 1;
                        }
                    }
                }
                intI = intI + 1;
            }

            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolGetIsAddableAboutEmployeesPeriods(
            int intnContactId_I,
            ZonedTime ztimeStartPeriod_I,
            ZonedTime ztimeEndPeriod_I,
            int? intnPkPeriod_I
            )
        {
            bool boolIsAddable = true;

            Odyssey2Context context = new Odyssey2Context();

            List<PerentityPeriodEntityDB> darrperentityPeriodEmployee = context.Period.Where(perentity =>
                perentity.intnContactId == intnContactId_I &&
                perentity.intPk != intnPkPeriod_I).ToList();

            int intI = 0;
            bool boolIsEmployeePeriodBusy = false;
            /*REPEAT-WHILE*/
            while (
                intI < darrperentityPeriodEmployee.Count &&
                !boolIsEmployeePeriodBusy
                )
            {
                //                                          //To easy code.
                ZonedTime ztimeStartEmployeePeriod = ZonedTimeTools.NewZonedTime(
                    darrperentityPeriodEmployee[intI].strStartDate.ParseToDate(),
                    darrperentityPeriodEmployee[intI].strStartTime.ParseToTime());

                ZonedTime ztimeEndEmployeePeriod = ZonedTimeTools.NewZonedTime(
                    darrperentityPeriodEmployee[intI].strEndDate.ParseToDate(),
                    darrperentityPeriodEmployee[intI].strEndTime.ParseToTime());

                if (
                    //                                      //Employee has many process periods assigned.
                    (
                    ztimeStartPeriod_I >= ztimeStartEmployeePeriod &&
                    ztimeStartPeriod_I < ztimeEndEmployeePeriod)
                    ||
                    (
                    ztimeEndPeriod_I >= ztimeStartEmployeePeriod &&
                    ztimeEndPeriod_I < ztimeEndEmployeePeriod)
                    )
                {
                    boolIsEmployeePeriodBusy = true;
                }

                intI = intI + 1;
            }

            boolIsAddable = !boolIsEmployeePeriodBusy;

            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolEmployeeIsValid(
            String strPrintshopId_I,
            int? intnContactId_I,
            IConfiguration configuration_I,
            ZonedTime ztimeStartPeriod_I,
            ZonedTime ztimeEndPeriod_I
            )
        {
            bool boolIsValid = true;
            if (
                intnContactId_I != null
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                List<RuleentityRuleEntityDB> darrruleentity = context.Rule.Where(rule => rule.intnPkPrintshop == null &&
                rule.intnPkResource == null && rule.intnContactId == intnContactId_I).ToList();

                int intI = 0;
                /*UNTIL-DO*/
                while (!(
                    (intI >= darrruleentity.Count) ||
                    (!boolIsValid)
                    ))
                {
                    //                                      //To easy code.
                    RuleentityRuleEntityDB ruleentity = darrruleentity[intI];

                    if (
                        ruleentity.strFrecuency == ResResource.strOnce
                        )
                    {
                        //                                  //start and end date of the rule.
                        Date dateStartRule = ruleentity.strFrecuencyValue.Substring(0, 10).ParseToDate();
                        Date dateEndRule = ruleentity.strFrecuencyValue.Substring(11, 10).ParseToDate();

                        //                                  //start and end of the rule.
                        ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(dateStartRule,
                            ruleentity.strStartTime.ParseToTime());

                        ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(dateEndRule,
                            ruleentity.strEndTime.ParseToTime());
                        if (
                            ((ztimeStartPeriod_I >= ztimeStartRule) && (ztimeStartPeriod_I < ztimeEndRule)) ||
                            ((ztimeEndPeriod_I > ztimeStartRule) && (ztimeEndPeriod_I <= ztimeEndRule))
                            )
                        {
                            boolIsValid = false;
                        }
                    }
                    else
                    {
                        //                                  //To easy code.
                        ZonedTime ztimeRuleRangeStart = ZonedTimeTools.NewZonedTime(ruleentity.strRangeStartDate.
                            ParseToDate(), ruleentity.strRangeStartTime.ParseToTime());
                        ZonedTime ztimeRuleRangeEnd = ZonedTimeTools.NewZonedTime(ruleentity.strRangeEndDate.ParseToDate(),
                            ruleentity.strRangeEndTime.ParseToTime());

                        if (
                            //                              //The period is between the start and end of the range.
                            ((ztimeStartPeriod_I >= ztimeRuleRangeStart) && (ztimeStartPeriod_I < ztimeRuleRangeEnd)) ||
                            ((ztimeEndPeriod_I > ztimeRuleRangeStart) && (ztimeEndPeriod_I <= ztimeRuleRangeEnd))
                            )
                        {
                            int intJ = 0;
                            Date date = ztimeStartPeriod_I.Date;
                            /*UNTIL-DO*/
                            while (!(
                                ((date + intJ) > ztimeEndPeriod_I.Date) ||
                                !boolIsValid
                                ))
                            {
                                ZonedTime ztimeStartRule = ZonedTimeTools.NewZonedTime(date,
                                    ruleentity.strStartTime.ParseToTime());
                                ZonedTime ztimeEndRule = ZonedTimeTools.NewZonedTime(date,
                                    ruleentity.strEndTime.ParseToTime());

                                if (
                                    //                      //DAILY RULE.
                                    ((ruleentity.strFrecuency == ResResource.strDaily) ||
                                    //                      //WEEKLY.
                                    ((ruleentity.strFrecuency == ResResource.strWeekly) &&
                                    (ruleentity.strFrecuencyValue[(int)date.DayOfWeek] == '1')) ||
                                    //                      //MONTHLY.
                                    ((ruleentity.strFrecuency == ResResource.strMonthly) &&
                                    (ruleentity.strFrecuencyValue[date.Day - 1] == '1')) ||
                                    //                      //ANNUALLY.
                                    ((ruleentity.strFrecuency == ResResource.strAnnually) &&
                                    (ruleentity.strFrecuencyValue == date.ToString("MMdd")))) &&
                                    //                      //The start is over the rule.
                                    (((ztimeStartPeriod_I >= ztimeStartRule) && (ztimeStartPeriod_I < ztimeEndRule)) ||
                                    //                      //The end is over the rule.
                                    ((ztimeEndPeriod_I > ztimeStartRule) && (ztimeEndPeriod_I <= ztimeEndRule)))
                                    )
                                {
                                    boolIsValid = false;
                                }

                                intJ = intJ + 1;
                            }
                        }
                    }
                    intI = intI + 1;
                }
            }

            return boolIsValid;
        }


        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidConsideringThePreviousProcesses(
            int intPkProcessInWorkflow_I,
            int intJobId_I,
            ZonedTime ztimeStartPeriod_I,
            //                                              //true, when is used to confirm temporary periods.
            bool boolIsTemporary_I,
            //                                              //Process valids for the Job.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses_I
            )
        {
            bool boolIsAddable = true;

            if (
                !boolIsTemporary_I
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Get the process in workflow.
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                //                                          //Create the list of the piw previous.
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPrevious = ProProcess.darrpiwentityPreviousOrNext(
                    piwentity, true, darrpiwentityAllProcesses_I);

                int intI = 0;
                /*UNTIL-DO*/
                while (!(
                    //                                      //There is no more piw.
                    (intI >= darrpiwentityPrevious.Count) ||
                    //                                      //The period is not addable already.
                    !boolIsAddable
                    ))
                {
                    //                                      //To easy code.
                    PiwentityProcessInWorkflowEntityDB piwentityPrevious = darrpiwentityPrevious[intI];

                    //                                      //Get all periods for that piw, including the periods of
                    //                                      //      the resources.
                    List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                        per.intPkWorkflow == piwentityPrevious.intPkWorkflow &&
                        per.intProcessInWorkflowId == piwentityPrevious.intProcessInWorkflowId &&
                        per.intJobId == intJobId_I &&
                        per.intnEstimateId == null).ToList();

                    int intJ = 0;
                    /*UNTIL-DO*/
                    while (!(
                        (intJ >= darrperentity.Count) ||
                        !boolIsAddable
                        ))
                    {
                        //                                  //To easy code.
                        PerentityPeriodEntityDB perentity = darrperentity[intJ];
                        ZonedTime ztimeEnd = ZonedTimeTools.NewZonedTime(perentity.strEndDate.ParseToDate(),
                            perentity.strEndTime.ParseToTime());

                        if (
                            ztimeStartPeriod_I < ztimeEnd
                            )
                        {
                            boolIsAddable = false;
                        }

                        intJ = intJ + 1;
                    }

                    intI = intI + 1;
                }
            }
            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidConsideringTheNextProcesses(
            int intPkProcessInWorkflow_I,
            int intJobId_I,
            ZonedTime ztimeEndPeriod_I,
            bool boolIsTemporary_I,
            //                                              //Process valids for the Job.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses_I,
            out String strProcesses_O,
            out List<int> darrintPkPeriodToDelete_O
            )
        {
            bool boolIsAddable = true;
            strProcesses_O = "";
            darrintPkPeriodToDelete_O = new List<int>();

            if (
                !boolIsTemporary_I
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                //                                              //Get the process in workflow.
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                //                                              //Create the list of the piw next.
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNext = ProProcess.darrpiwentityPreviousOrNext(
                    piwentity, false, darrpiwentityAllProcesses_I);

                foreach (PiwentityProcessInWorkflowEntityDB piwentityNext in darrpiwentityNext)
                {
                    //                                          //Get all periods for that piw, including the periods of
                    //                                          //      the resources.
                    List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                        per.intPkWorkflow == piwentityNext.intPkWorkflow &&
                        per.intProcessInWorkflowId == piwentityNext.intProcessInWorkflowId &&
                        per.intJobId == intJobId_I &&
                        per.intnEstimateId == null).ToList();

                    foreach (PerentityPeriodEntityDB perentity in darrperentity)
                    {
                        ZonedTime ztimeStart = ZonedTimeTools.NewZonedTime(perentity.strStartDate.ParseToDate(),
                            perentity.strStartTime.ParseToTime());

                        if (
                            //                                  //The End of the period to Add should be less or equal
                            //                                  //      from the existing period.
                            ztimeEndPeriod_I > ztimeStart
                            )
                        {
                            boolIsAddable = false;
                            darrintPkPeriodToDelete_O.Add(perentity.intPk);

                            ProProcess pro = ProProcess.proFromDB(piwentity.intPkProcess);
                            String strProcessId = (piwentity.intnId == null) ? "" : " (" + piwentity.intnId + ")";
                            String strProcessName = pro.strName + strProcessId;
                            if (
                                !strProcesses_O.Contains(strProcessName)
                                )
                            {
                                strProcesses_O = strProcesses_O + ", " + strProcessName;
                            }
                        }
                    }
                }
            }
            return boolIsAddable;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPreviousOrNext(
            //                                              //Get Next or Previus Processes
            //                                              //    depending the boolean.
            //                                              //It is True: get Previus.
            //                                              //Devuelve el proceso inmediato anterio o posterior,
            //                                              

            PiwentityProcessInWorkflowEntityDB piwentity_I,
            bool boolInput_I,
            //                                              //Process valids for this jobs.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcessesValids_I
            )
        {
            //                                              //Create the list of the piw previous.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = new
                List<PiwentityProcessInWorkflowEntityDB>();

            Odyssey2Context context = new Odyssey2Context();
            //                                              //Get the eleet inputs/outputs.
            List<EleetentityElementElementTypeEntityDB> darreleetentity =
                (from eleentityProcess in context.Element
                 join eleetentity in context.ElementElementType
                 on eleentityProcess.intPk equals eleetentity.intPkElementDad
                 where eleentityProcess.intPk == piwentity_I.intPkProcess && eleetentity.boolUsage == boolInput_I
                 select eleetentity).ToList();

            List<int> darrintPIWIDStock = new List<int>();
            foreach (EleetentityElementElementTypeEntityDB eleetentity in darreleetentity)
            {
                //                                          //Get the io if it exists and has link.
                IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                    io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                    io.intnPkElementElementType == eleetentity.intPk && io.strLink != null);

                if (
                    //                                      //exist the IO with Link.
                    ioentity != null
                    )
                {
                    ProProcess.subAddPreviusOrNextProcessesRecursive(ioentity, !boolInput_I,
                    darrpiwentityAllProcessesValids_I, context, ref darrpiwentity, ref darrintPIWIDStock);
                }
            }

            //                                              //Get the eleele inputs/outputs.
            List<EleeleentityElementElementEntityDB> darreleeleentity =
                (from eleentityProcess in context.Element
                 join eleeleentity in context.ElementElement
                 on eleentityProcess.intPk equals eleeleentity.intPkElementDad
                 where eleentityProcess.intPk == piwentity_I.intPkProcess && eleeleentity.boolUsage == boolInput_I
                 select eleeleentity).ToList();

            foreach (EleeleentityElementElementEntityDB eleeleentity in darreleeleentity)
            {
                //                                          //Get the io if it exists and has link.
                IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                    io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                    io.intnPkElementElement == eleeleentity.intPk && io.strLink != null);

                if (
                    //                                      //exist the IO with Link.
                    ioentity != null
                    )
                {
                    ProProcess.subAddPreviusOrNextProcessesRecursive(ioentity, !boolInput_I,
                    darrpiwentityAllProcessesValids_I, context, ref darrpiwentity, ref darrintPIWIDStock);
                }
            }

            return darrpiwentity;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddPreviusOrNextProcessesRecursive(
            //                                              //Add previus or next processes.

            //                                              //In the first call passed a IOProcess, 
            //                                              //  and next passed IONodes.
            IoentityInputsAndOutputsEntityDB ioentity_I,
            bool boolGetNext_I,
            //                                              //Process valids for this Job.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcessesValid_I,
            Odyssey2Context context_I,
            ref List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAdded_M,
            ref List<int> darrintPIWIDStock_M
            )
        {
            if (
                //                                          //It is a node.
                (
                    ioentity_I.intnPkElementElement == null &&
                    ioentity_I.intnPkElementElementType == null &&
                    ioentity_I.intnProcessInWorkflowId == null
                ) 
                ||
                (
                    (
                        //                                      //IO of the process.
                        ioentity_I.intnProcessInWorkflowId != null
                    ) 
                    &&
                    (
                        //                                      //It is not analized.
                        !darrintPIWIDStock_M.Exists(intID => intID == ioentity_I.intnProcessInWorkflowId)
                    )
                )
                )
            {
                List<IobefornexjsonInputsAndOutputsBeforeOrNextJson>
                    darriobefornextjsonIOsBeforeProcessesResult;

                List<IobefornexjsonInputsAndOutputsBeforeOrNextJson>
                    darriobefornextjsonIOsBeforeNodesResult;

                //                                              //Get the next or previus process/nodes.
                ProdtypProductType.subGetIOsProcessesOrNodesBeforeOrNextFROMIOProcessOrIONode(ioentity_I,
                    boolGetNext_I, true, out darriobefornextjsonIOsBeforeProcessesResult,
                    out darriobefornextjsonIOsBeforeNodesResult, context_I);

                //                                              //Take each next or previus IOProcess.
                foreach (IobefornexjsonInputsAndOutputsBeforeOrNextJson iobefornextjsonProcess in
                    darriobefornextjsonIOsBeforeProcessesResult)
                {
                    IoentityInputsAndOutputsEntityDB ioentityBefOrNext = iobefornextjsonProcess.ioentityBeforeOrNext;

                    //                                          //Get the piw of the other side.
                    PiwentityProcessInWorkflowEntityDB piwentityOtherSideOfLink = context_I.ProcessInWorkflow.
                        FirstOrDefault(piw => piw.intPkWorkflow == ioentityBefOrNext.intPkWorkflow &&
                        piw.intProcessInWorkflowId == ioentityBefOrNext.intnProcessInWorkflowId);

                    PerentityPeriodEntityDB perentityPeriods = null;
                    if (
                        ProProcess.boolIsValidProcessForTheJob(piwentityOtherSideOfLink, darrpiwentityAllProcessesValid_I,
                        darrpiwentityAdded_M, context_I, ref perentityPeriods)
                    )
                    {
                        if (
                            //                                  //There is period for this process.
                            perentityPeriods != null
                            )
                        {
                            darrpiwentityAdded_M.Add(piwentityOtherSideOfLink);
                        }
                        else
                        {
                            if (
                                //                              //Only for previus.
                                !boolGetNext_I
                                )
                            {
                                ProProcess.subAddPreviusOrNextProcessesRecursive(ioentityBefOrNext, boolGetNext_I,
                                darrpiwentityAllProcessesValid_I, context_I, ref darrpiwentityAdded_M, 
                                ref darrintPIWIDStock_M);
                            }
                        }
                    }

                }

                //                                              //Take each next or previus IONodes.
                foreach (IobefornexjsonInputsAndOutputsBeforeOrNextJson iobefornextjsonNodes in
                    darriobefornextjsonIOsBeforeNodesResult)
                {
                    //                                          //Continue add process of the nodes.
                    ProProcess.subAddPreviusOrNextProcessesRecursive(iobefornextjsonNodes.ioentityBeforeOrNext, boolGetNext_I,
                        darrpiwentityAllProcessesValid_I, context_I, ref darrpiwentityAdded_M,
                        ref darrintPIWIDStock_M);
                }

                if (
                    ioentity_I.intnProcessInWorkflowId != null
                    )
                {
                    //                                          //PIWID analized.
                    darrintPIWIDStock_M.Add((int)ioentity_I.intnProcessInWorkflowId);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidProcessForTheJob(
            //                                              //Verify if the process is valid.

            PiwentityProcessInWorkflowEntityDB piwentity_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcessesValid_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAdded_I,
            Odyssey2Context context_I,
            ref PerentityPeriodEntityDB perentityPeriod_M
            )
        {
            perentityPeriod_M = null;
            bool boolIsValidProcess = false;
            if (
                //                                          //Process is valid.
                darrpiwentityAllProcessesValid_I.Exists(piw => piw.intProcessInWorkflowId == 
                piwentity_I.intProcessInWorkflowId) &&
                //                                          //and Not Exist in the list added.
                !darrpiwentityAdded_I.Exists(piw => piw.intProcessInWorkflowId ==
                piwentity_I.intProcessInWorkflowId)
                )
            {
                boolIsValidProcess = true;

                //                                          //Find if there is at least one period at this process.
                perentityPeriod_M = context_I.Period.FirstOrDefault(per =>
                    per.intPkWorkflow == piwentity_I.intPkWorkflow &&
                    per.intProcessInWorkflowId == piwentity_I.intProcessInWorkflowId);
            }

            return boolIsValidProcess;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCreateTheStringsForProcessesAndResources(
            String strProcesses_I,
            out String strFinalForProcesses_O
            )
        {
            strFinalForProcesses_O = "";
            if (
                strProcesses_I != ""
                )
            {
                String str2 = strProcesses_I.Substring(2);
                if (
                    str2.Count(c => c == ',') > 0
                    )
                {
                    int intLastCommaIndex = str2.LastIndexOf(',');
                    String str2FirstPart = str2.Substring(0, intLastCommaIndex);
                    String str2SecondPart = str2.Substring(intLastCommaIndex + 1);

                    str2 = str2FirstPart + " and" + str2SecondPart;
                }

                String str0 = (str2.Contains(',') || str2.Contains("and")) ? "these" : "this";
                String str1 = (str2.Contains(',') || str2.Contains("and")) ? "es" : "";


                strFinalForProcesses_O = String.Format("If you add this period, periods will be deleted for {0}" +
                    " process{1}: {2}.", str0, str1, str2);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetEndOfPeriod(
            String strPrintshopId_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            int intPkCalculation_I,
            String strStartDate_I,
            String strStartTime_I,
            IConfiguration configuration_I,
            out EndperjsonEndOfPeriodJson endperjson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            endperjson_O = null;
            
            //                                              //Validate job.
            JobjsonJobJson jobjson;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson, 
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No process in workflow found.";
                if (
                    piwentity != null
                    )
                {
                    CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(cal =>
                        cal.intPk == intPkCalculation_I && cal.intnPkWorkflow != null &&
                        cal.intnProcessInWorkflowId != null && cal.intnPkProcess != null);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "No calculation found.";
                    if (
                        calentity != null
                        )
                    {
                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Invalid format for dates and times.";
                        if (
                            strStartDate_I.IsParsableToDate() &&
                            strStartTime_I.IsParsableToTime()
                            )
                        {
                            //                                              //To easy code.
                            ZonedTime ztimeStartPeriod = ZonedTimeTools.NewZonedTime(
                                strStartDate_I.ParseToDate(), strStartTime_I.ParseToTime());

                            intStatus_IO = 405;
                            strUserMessage_IO = "The period must be greater than the current time.";
                            strDevMessage_IO = "";
                            if (
                                ztimeStartPeriod > ZonedTimeTools.ztimeNow
                                )
                            {
                                long longMilliseconds = (((int)calentity.intnHours * 3600) +
                                    ((int)calentity.intnMinutes * 60) + (int)calentity.intnSeconds) * 1000;
                                if (
                                    calentity.strCalculationType == CalCalculation.strPerQuantity
                                    )
                                {
                                    longMilliseconds = (long)(((((calentity.numnNeeded / calentity.numnPerUnits)
                                        * (double)jobjson.intnQuantity)) * ((double)(((int)calentity.intnHours *
                                        3600) + ((int)calentity.intnMinutes * 60) +
                                        ((int)calentity.intnSeconds)) / (calentity.numnQuantity))) * 1000);
                                }

                                ZonedTime ztimeEnd = ztimeStartPeriod + longMilliseconds;

                                endperjson_O = new EndperjsonEndOfPeriodJson(ztimeEnd.Date.ToText(),
                                    ztimeEnd.Time.ToString());

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
