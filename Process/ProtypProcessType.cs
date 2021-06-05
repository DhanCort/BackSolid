/*TASK RP.JDF*/
using Odyssey2Backend.Alert;
using Odyssey2Backend.App;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowaStandard;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.Process.SRP;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.XJDF
{
    //=================================================================================================================
    public class ProtypProcessType : EtElementTypeAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        public const String strGeneralTypeId = "Process General Attributes";
        public const String strInput = "Input";
        public const String strOutput = "Output";
        public static readonly String[] arrstrCommon = { "ConventionalPrinting", "Cutting", "Delivery", 
            "DigitalPrinting", "Folding", "ManualLabor" };

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DYNAMIC VARIABLES.

        //                                                  //Dictionary of resources for this type.
        public Dictionary<String, RestypResourceType> dicrestem_Z;
        public Dictionary<String, RestypResourceType> dicrestem
        {
            get
            {
                this.subGetResourceTypeFromDB(out this.dicrestem_Z);
                return this.dicrestem_Z;
            }
        }

        //                                                  //Dictionary of process for this process type.
        public Dictionary<int, ProProcess> dicpro_Z;
        public Dictionary<int, ProProcess> dicpro
        {
            get
            {
                this.subGetProcessesFromDB(out this.dicpro_Z);
                return this.dicpro_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //SUPPORT METHODS FOR DYNAMIC VARIABLES.

        //--------------------------------------------------------------------------------------------------------------
        private void subGetResourceTypeFromDB(
            //                                              //Get all res for this type from db.

            //                                              //Dic where the res will be saved.
            out Dictionary<String, RestypResourceType> dicrestem_O
            )
        {
            //                                              //Initialize the dicres.
            dicrestem_O = new Dictionary<String, RestypResourceType>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all relations for this.
            IQueryable<EtetentityElementTypeElementTypeEntityDB> setetetentity =
                context.ElementTypeElementType.Where(etetentity =>
                etetentity.intPkElementTypeDad == this.intPk);

            //                                              //Get all the resources.
            foreach (EtetentityElementTypeElementTypeEntityDB etetentity in setetetentity)
            {
                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                etentity.intPk == etetentity.intPkElementTypeSon &&
                etentity.strResOrPro == EtElementTypeAbstract.strResource);

                RestypResourceType restem = new RestypResourceType(etentity.intPk, etentity.strXJDFTypeId,
                    etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                    etentity.strClassification);

                dicrestem_O.Add(restem.strCustomTypeId, restem);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subGetProcessesFromDB(
            //                                              //Get all pro for this process type from db.

            //                                              //Dic where the process will be saved.
            out Dictionary<int, ProProcess> dicpro_O
            )
        {
            //                                              //Initialize the dicpro.
            dicpro_O = new Dictionary<int, ProProcess>();

            //                                              //Create the context.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all relations for this.
            List<EleentityElementEntityDB> darreleentityProcess =
                context.Element.Where(eleentity =>
                eleentity.intPkElementType == this.intPk && eleentity.boolIsTemplate == false &&
                eleentity.boolDeleted == false).ToList();

            //                                              //Get all the processes.
            foreach (EleentityElementEntityDB eleProcess in darreleentityProcess)
            {
                ProProcess proProcess = new ProProcess(eleProcess.intPk, eleProcess.strElementName);
                dicpro_O.Add(eleProcess.intPk, proProcess);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public ProtypProcessType(
            //                                              //Receive the values of the instance variables and the attr 
            //                                              //      and set it. The dictionary of attr can be empty. 

            //                                              //Primary key of the type.
            int intPk_I,
            //                                              //Specific type of process when it is a XJDF type, it 
            //                                              //      can be empty string when this is a printshop 
            //                                              //      type.
            String strXJDFTypeId_I,
            //                                              //Added by: XJDFX.X, MI4P or printshop id.
            String strAddedBy_I,
            //                                              //Modified by: XJDFX.X, MI4P or printshop id
            int? intPkPrintshop_I,
            //                                              //Custom type id.
            String strCustomTypeId_I,
            //                                              //Process Classification.
            String strClassification_I
            )
            : base(intPk_I, strXJDFTypeId_I, strAddedBy_I, intPkPrintshop_I, strCustomTypeId_I,
                  EtElementTypeAbstract.strProcess, strClassification_I)
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static int subAddResourceTypeOrTemplateToProcess(
            //                                              //Create the relation between the process and its resource 
            //                                              //      type or template as an input or output.

            String strPrintshopId_I,
            int intPkProcess_I,
            int? intnPkType_I,
            int? intnPkTemplate_I,
            String strInputOrOutput_I,
            Odyssey2Context context_M,
            out int intPkProcessLast_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int intPkEleetOrEleele = 0;
            intPkProcessLast_O = 0;
            
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "A type or template needs to be specified and only one.";
            if (
                //                                          //Is a type.
                ((intnPkTemplate_I == null) && (intnPkType_I != null) && (intnPkType_I > 0)) ||
                //                                          //Is a template.
                ((intnPkType_I == null) && (intnPkTemplate_I != null) && (intnPkTemplate_I > 0))
                )
            {
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                ProProcess pro = ProProcess.proFromDB(intPkProcess_I, context_M);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No product found.";
                if (
                    (pro != null) && (pro.protypBelongsTo.intPkPrintshop == ps.intPk)
                    )
                {
                    if (
                        //                                  //It is a type.
                        intnPkType_I != null
                        )
                    {
                        ProtypProcessType.subAddTypeAsIOToProcess((int)intnPkType_I, ps, pro, strInputOrOutput_I, 
                            context_M, out intPkProcessLast_O, ref intPkEleetOrEleele, ref intStatus_IO, 
                            ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                    else
                    {
                        ProtypProcessType.subAddTemplateAsIOToProcess((int)intnPkTemplate_I, ps, pro, 
                            strInputOrOutput_I, context_M, out intPkProcessLast_O, ref intPkEleetOrEleele, 
                            ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                    }
                }
            }

            return intPkEleetOrEleele;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddTypeAsIOToProcess(
            int intPkType_I,
            PsPrintShop ps_I,
            ProProcess pro_I,
            String strInputOrOutput_I,
            Odyssey2Context context_M,
            out int intPkProcessLast_O,
            ref int intPkEleetOrEleele_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intPkProcessLast_O = pro_I.intPk;

            //                                              //Get the restyp.
            EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(context_M, intPkType_I);

            intStatus_IO = 403;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No resource type found.";
            if (
                (et != null) && 
                (et.strResOrPro == EtElementTypeAbstract.strResource)
                )
            {
                intStatus_IO = 404;
                strUserMessage_IO = "Cannot set a Consumable or Implementation resource to output.";
                strDevMessage_IO = "";
                if (
                    //                                      //Outputs cannot have consumable or implementation types.
                    !(strInputOrOutput_I == ProtypProcessType.strOutput &&
                    ((et.strClassification == EtElementTypeAbstract.strResClasConsumable) ||
                    (et.strClassification == EtElementTypeAbstract.strResClasImplementation)))
                    )
                {
                    RestypResourceType restyp = (RestypResourceType)et;

                    intStatus_IO = 405;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The type is not from the printshop logged in.";
                    if (
                        //                                  //Is from the ps.
                        ((restyp.intPkPrintshop != null) && (restyp.intPkPrintshop == ps_I.intPk)) ||
                        //                                  //Is the generic type.
                        (restyp.intPkPrintshop == null)
                        )
                    {
                        bool boolUsage = strInputOrOutput_I == ProtypProcessType.strInput;

                        if (
                            restyp.intPkPrintshop == null
                            )
                        {
                            EtentityElementTypeEntityDB etentityPrintshopRestyp = context_M.ElementType.FirstOrDefault(
                                et => et.intPrintshopPk == ps_I.intPk && et.strCustomTypeId == restyp.strCustomTypeId);

                            int intPkType;
                            if (
                                //                          //The printshop has not this template.
                                etentityPrintshopRestyp == null
                                )
                            {
                                //                          //Create clone Resource XJDF for the printshop.
                                int intStatus;
                                EtentityElementTypeEntityDB etentity = new EtentityElementTypeEntityDB();
                                Odyssey2.subAddTypeToPrintshop(intPkType_I, ps_I, context_M, out intStatus, 
                                    out intPkType);
                            }
                            else
                            {
                                intPkType = etentityPrintshopRestyp.intPk;
                            }

                            restyp = (RestypResourceType)EtElementTypeAbstract.etFromDB(context_M, intPkType);
                        }

                        if (
                            //                              //The process is used by jobs in progress o completed.
                            ProtypProcessType.boolIsProcessInSomeWorkflowWithJobsInProgressOrCompleted(pro_I, context_M)
                            )
                        {
                            //                              //Copy the process and the workflows.
                            ProProcess proNew;
                            ProtypProcessType.subCopyAProcess(pro_I, context_M, out proNew);
                            ProtypProcessType.subCreateTheNewWorkflowsForModifiedTheProcess(ps_I, pro_I, context_M);
                            ProtypProcessType.subUpdateProcessPkInActiveWorkflows(pro_I, proNew, context_M);

                            intPkProcessLast_O = proNew.intPk;

                            int? intnId = ProtypProcessType.intnGetEleetId(proNew.intPk, restyp.intPk, boolUsage,
                                context_M);

                            //                              //Modified the process.
                            EleetentityElementElementTypeEntityDB eleetentity = new EleetentityElementElementTypeEntityDB
                            {
                                intPkElementDad = proNew.intPk,
                                intPkElementTypeSon = restyp.intPk,
                                boolUsage = boolUsage,
                                intnId = intnId
                            };
                            context_M.ElementElementType.Add(eleetentity);
                            context_M.SaveChanges();

                            intPkEleetOrEleele_IO = eleetentity.intPk;

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";

                            //                              //Delete EstimationData entries.
                            if (
                                strInputOrOutput_I == ProtypProcessType.strInput &&
                                RestypResourceType.boolIsPhysical(et.strClassification)
                                )
                            {
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityToDelete = (
                                    from piwentityToDeleteEstimates in context_M.ProcessInWorkflow
                                    join wfentity in context_M.Workflow
                                    on piwentityToDeleteEstimates.intPkWorkflow equals wfentity.intPk
                                    join piwentityWithProcess in context_M.ProcessInWorkflow
                                    on wfentity.intPk equals piwentityWithProcess.intPkWorkflow
                                    where piwentityWithProcess.intPkProcess == pro_I.intPk
                                    select piwentityToDeleteEstimates).ToList();

                                JobJob.subDeleteEstimationDataEntriesForAWorkflow(context_M, darrpiwentityToDelete);
                            }
                            
                        }
                        else
                        {
                            int? intnId = ProtypProcessType.intnGetEleetId(pro_I.intPk, restyp.intPk, boolUsage, 
                                context_M);

                            //                              //Create the relation between the process and resource type.
                            EleetentityElementElementTypeEntityDB eleetentity = new EleetentityElementElementTypeEntityDB
                            {
                                intPkElementDad = pro_I.intPk,
                                intPkElementTypeSon = restyp.intPk,
                                boolUsage = boolUsage,
                                intnId = intnId
                            };
                            context_M.ElementElementType.Add(eleetentity);
                            context_M.SaveChanges();

                            intPkEleetOrEleele_IO = eleetentity.intPk;

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";

                            //                              //Delete EstimationData entries.
                            if (
                                strInputOrOutput_I == ProtypProcessType.strInput &&
                                RestypResourceType.boolIsPhysical(et.strClassification)
                                )
                            {
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityToDelete = (
                                    from piwentityToDeleteEstimates in context_M.ProcessInWorkflow
                                    join wfentity in context_M.Workflow
                                    on piwentityToDeleteEstimates.intPkWorkflow equals wfentity.intPk
                                    join piwentityWithProcess in context_M.ProcessInWorkflow
                                    on wfentity.intPk equals piwentityWithProcess.intPkWorkflow
                                    where piwentityWithProcess.intPkProcess == pro_I.intPk
                                    select piwentityToDeleteEstimates).ToList();

                                JobJob.subDeleteEstimationDataEntriesForAWorkflow(context_M, darrpiwentityToDelete);
                            }                            
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddTemplateAsIOToProcess(
            int intPkTemplate_I,
            PsPrintShop ps_I,
            ProProcess pro_I,
            String strInputOrOutput_I,
            Odyssey2Context context_M,
            out int intPkProcessLast_O,
            ref int intPkEleetOrEleele_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intPkProcessLast_O = pro_I.intPk;

            //                                              //It is a template.
            ResResource resTemplate = ResResource.resFromDB(context_M, intPkTemplate_I, true);

            intStatus_IO = 406;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No template found.";
            if (
                resTemplate != null
                )
            {
                //                                          //Get the template from DB.
                EleentityElementEntityDB eleentity = context_M.Element.FirstOrDefault(eleentity => 
                    eleentity.intPk == resTemplate.intPk);

                //                                          //Get the restyp.
                EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(context_M, eleentity.intPkElementType);

                intStatus_IO = 407;
                strUserMessage_IO = "Cannot set a Consumable or Implementation resource to output.";
                strDevMessage_IO = "";
                if (
                    //                                      //Outputs cannot have consumable or implementation types.
                    !(strInputOrOutput_I == ProtypProcessType.strOutput &&
                    ((et.strClassification == EtElementTypeAbstract.strResClasConsumable) ||
                    (et.strClassification == EtElementTypeAbstract.strResClasImplementation)))
                    )
                { 
                    intStatus_IO = 408;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The template is not from the printshop logged in.";
                    if (
                        resTemplate.restypBelongsTo.intPkPrintshop == ps_I.intPk
                        )
                    {
                        bool boolUsage = strInputOrOutput_I == ProtypProcessType.strInput;

                        if (
                            //                              //The process is used by jobs in progress o completed. 
                            ProtypProcessType.boolIsProcessInSomeWorkflowWithJobsInProgressOrCompleted(pro_I, context_M)
                            )
                        {
                            //                              //Copy the process and the workflows.
                            ProProcess proNew;
                            ProtypProcessType.subCopyAProcess(pro_I, context_M, out proNew);
                            ProtypProcessType.subCreateTheNewWorkflowsForModifiedTheProcess(ps_I, pro_I, context_M);
                            ProtypProcessType.subUpdateProcessPkInActiveWorkflows(pro_I, proNew, context_M);

                            intPkProcessLast_O = proNew.intPk;

                            int? intnId = ProtypProcessType.intnGetEleeleId(proNew.intPk, resTemplate.intPk, boolUsage,
                                context_M);

                            //                              //Modified the process.
                            EleeleentityElementElementEntityDB eleeleentity = new EleeleentityElementElementEntityDB
                            {
                                intPkElementDad = proNew.intPk,
                                intPkElementSon = resTemplate.intPk,
                                boolUsage = boolUsage,
                                intnId = intnId
                            };
                            context_M.ElementElement.Add(eleeleentity);
                            context_M.SaveChanges();

                            intPkEleetOrEleele_IO = eleeleentity.intPk;

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";

                            //                              //Delete EstimationData entries.
                            if (
                                strInputOrOutput_I == ProtypProcessType.strInput &&
                                RestypResourceType.boolIsPhysical(resTemplate.restypBelongsTo.strClassification)
                                )
                            {
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityToDelete = (
                                    from piwentityToDeleteEstimates in context_M.ProcessInWorkflow
                                    join wfentity in context_M.Workflow
                                    on piwentityToDeleteEstimates.intPkWorkflow equals wfentity.intPk
                                    join piwentityWithProcess in context_M.ProcessInWorkflow
                                    on wfentity.intPk equals piwentityWithProcess.intPkWorkflow
                                    where piwentityWithProcess.intPkProcess == pro_I.intPk
                                    select piwentityToDeleteEstimates).ToList();

                                JobJob.subDeleteEstimationDataEntriesForAWorkflow(context_M, darrpiwentityToDelete);
                            }                            
                        }
                        else
                        {
                            int? intnId = ProtypProcessType.intnGetEleeleId(pro_I.intPk, resTemplate.intPk, boolUsage,
                                context_M);

                            //                              //Create the relation between the process and resource 
                            //                              //      type.
                            EleeleentityElementElementEntityDB eleeleentity = new EleeleentityElementElementEntityDB
                            {
                                intPkElementDad = pro_I.intPk,
                                intPkElementSon = resTemplate.intPk,
                                boolUsage = boolUsage,
                                intnId = intnId
                            };
                            context_M.ElementElement.Add(eleeleentity);
                            context_M.SaveChanges();

                            intPkEleetOrEleele_IO = eleeleentity.intPk;

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";

                            //                              //Delete EstimationData entries.
                            if (
                                strInputOrOutput_I == ProtypProcessType.strInput &&
                                RestypResourceType.boolIsPhysical(resTemplate.restypBelongsTo.strClassification)
                                )
                            {
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityToDelete = (
                                    from piwentityToDeleteEstimates in context_M.ProcessInWorkflow
                                    join wfentity in context_M.Workflow
                                    on piwentityToDeleteEstimates.intPkWorkflow equals wfentity.intPk
                                    join piwentityWithProcess in context_M.ProcessInWorkflow
                                    on wfentity.intPk equals piwentityWithProcess.intPkWorkflow
                                    where piwentityWithProcess.intPkProcess == pro_I.intPk
                                    select piwentityToDeleteEstimates).ToList();

                                JobJob.subDeleteEstimationDataEntriesForAWorkflow(context_M, darrpiwentityToDelete);
                            }                            
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsProcessInSomeWorkflowWithJobsInProgressOrCompleted(
            ProProcess pro_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get all the workflows with this process.
            IQueryable<WfentityWorkflowEntityDB> setwfentity =
                from wfentity in context_M.Workflow
                join piwentity in context_M.ProcessInWorkflow
                on wfentity.intPk equals piwentity.intPkWorkflow
                where piwentity.intPkProcess == pro_I.intPk && wfentity.boolDeleted == false
                select wfentity;
            List<WfentityWorkflowEntityDB> darrwfentity = setwfentity.ToList();

            bool boolIsProcessInWorkflowfWithJobs = false;
            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= darrwfentity.Count) ||
                boolIsProcessInWorkflowfWithJobs
                ))
            {
                if (
                    context_M.Job.FirstOrDefault(job => job.intPkWorkflow == darrwfentity[intI].intPk) != null
                    )
                {
                    boolIsProcessInWorkflowfWithJobs = true;
                }
                intI = intI + 1;
            }

            return boolIsProcessInWorkflowfWithJobs;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCopyAProcess(
            ProProcess pro_I,
            Odyssey2Context context_I,
            out ProProcess proNew_O
            )
        {
            //                                              //Get process.
            EleentityElementEntityDB eleentityBase = context_I.Element.FirstOrDefault(ele => ele.intPk == pro_I.intPk);

            //                                              //Clone process.
            EleentityElementEntityDB eleentityNew = new EleentityElementEntityDB
            {
                strElementName = eleentityBase.strElementName,
                intPkElementType = eleentityBase.intPkElementType,
                intnPkElementInherited = eleentityBase.intnPkElementInherited,
                boolIsTemplate = eleentityBase.boolIsTemplate,
                boolnIsCalendar = eleentityBase.boolnIsCalendar,
                boolnIsAvailable = eleentityBase.boolnIsAvailable,
                boolDeleted = false
            };
            context_I.Element.Add(eleentityNew);
            context_I.SaveChanges();

            //                                              //Find Eleet related to that process.
            IQueryable<EleetentityElementElementTypeEntityDB> seteleetentity = context_I.ElementElementType.Where(
                eleet => eleet.intPkElementDad == pro_I.intPk);
            List<EleetentityElementElementTypeEntityDB> darreleetentity = seteleetentity.ToList();
            foreach (EleetentityElementElementTypeEntityDB eleet in darreleetentity)
            {
                //                                          //Duplicate Eleet.
                EleetentityElementElementTypeEntityDB eleetNew = new EleetentityElementElementTypeEntityDB
                {
                    boolUsage = eleet.boolUsage,
                    //                                      //Duplicate with new pk.
                    intPkElementDad = eleentityNew.intPk,
                    intPkElementTypeSon = eleet.intPkElementTypeSon,
                    boolDeleted = false,
                    intnId = eleet.intnId
                };
                context_I.ElementElementType.Add(eleetNew);

                //                                          //Delete old Eleet.
                eleet.boolDeleted = true;
                context_I.ElementElementType .Update(eleet);
            }

            //                                              //Find EleEle related to that process.
            IQueryable<EleeleentityElementElementEntityDB> seteleeleentity = context_I.ElementElement.Where(
                eleele => eleele.intPkElementDad == pro_I.intPk);
            List<EleeleentityElementElementEntityDB> darreleeleentity = seteleeleentity.ToList();
            foreach (EleeleentityElementElementEntityDB eleele in darreleeleentity)
            {
                //                                          //Duplicate EleEle.
                EleeleentityElementElementEntityDB eleeleNew = new EleeleentityElementElementEntityDB
                {
                    boolUsage = eleele.boolUsage,
                    //                                      //Duplicate with new pk.
                    intPkElementDad = eleentityNew.intPk,
                    intPkElementSon = eleele.intPkElementSon,
                    boolDeleted = false,
                    intnId = eleele.intnId
                };
                context_I.ElementElement.Add(eleeleNew);

                //                                          //Delete old EleEle.
                eleele.boolDeleted = true;
                context_I.ElementElement.Update(eleele);
            }

            //                                              //Delete old process.
            eleentityBase.boolDeleted = true;
            context_I.Element.Update(eleentityBase);

            context_I.SaveChanges();

            //                                              //Return new process.
            proNew_O = ProProcess.proFromDB(eleentityNew.intPk, context_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCreateTheNewWorkflowsForModifiedTheProcess(
            PsPrintShop ps_I,
            ProProcess pro_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get all the workflows with this process.
            IQueryable<WfentityWorkflowEntityDB> setwfentity =
                from wfentity in context_I.Workflow
                join piwentity in context_I.ProcessInWorkflow
                on wfentity.intPk equals piwentity.intPkWorkflow
                where piwentity.intPkProcess == pro_I.intPk && wfentity.boolDeleted == false
                select wfentity;
            List<WfentityWorkflowEntityDB> darrwfentity = setwfentity.ToList();

            foreach (WfentityWorkflowEntityDB wfentity in darrwfentity)
            {
                if (
                    context_I.Job.FirstOrDefault(job => job.intPkWorkflow == wfentity.intPk) != null
                    )
                {
                    WfentityWorkflowEntityDB wfentityNew;
                    ProdtypProductType.subAddWorkflowIfItIsNecessary(ps_I, wfentity, context_I, out wfentityNew);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subUpdateProcessPkInActiveWorkflows(
            ProProcess pro_I,
            ProProcess proNew_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get all active workflows with this process.
            IQueryable<WfentityWorkflowEntityDB> setwfentity =
                from wfentity in context_I.Workflow
                join piwentity in context_I.ProcessInWorkflow
                on wfentity.intPk equals piwentity.intPkWorkflow
                where piwentity.intPkProcess == pro_I.intPk && wfentity.boolDeleted == false
                select wfentity;
            List<WfentityWorkflowEntityDB> darrwfentity = setwfentity.ToList();

            foreach (WfentityWorkflowEntityDB wfentity in darrwfentity)
            {
                //                                          //Get the piw about this process in this wf.
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = context_I.ProcessInWorkflow.Where(piw =>
                    piw.intPkProcess == pro_I.intPk && piw.intPkWorkflow == wfentity.intPk).ToList();

                foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity)
                {
                    //                                      //Update the process in the piw.
                    piwentity.intPkProcess = proNew_I.intPk;
                    context_I.ProcessInWorkflow.Update(piwentity);

                    //                                      //Get the eleet for the past process.
                    List<EleetentityElementElementTypeEntityDB> darreleetentity = context_I.ElementElementType.Where(
                        eleet => eleet.intPkElementDad == pro_I.intPk).ToList();

                    foreach (EleetentityElementElementTypeEntityDB eleetentity in darreleetentity)
                    {
                        //                                  //Get the eleet about this process.
                        EleetentityElementElementTypeEntityDB eleetentityNew = 
                            context_I.ElementElementType.FirstOrDefault(eleet => 
                            eleet.intPkElementDad == proNew_I.intPk &&
                            eleet.intPkElementTypeSon == eleetentity.intPkElementTypeSon &&
                            eleet.boolUsage == eleetentity.boolUsage &&
                            eleet.intnId == eleetentity.intnId);

                        //                                  //Find the ios of the eleets in this wf.
                        List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
                            io.intPkWorkflow == wfentity.intPk &&
                            io.intnPkElementElementType == eleetentity.intPk).ToList();

                        foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
                        {
                            ioentity.intnPkElementElementType = eleetentityNew.intPk;
                            context_I.InputsAndOutputs.Update(ioentity);
                        }

                        //                                  //Find the iojs of the eleets in this wf.
                        List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity = context_I.InputsAndOutputsForAJob.
                            Where(io => io.intPkProcessInWorkflow == piwentity.intPk &&
                            io.intnPkElementElementType == eleetentity.intPk).ToList();

                        foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
                        {
                            iojentity.intnPkElementElementType = eleetentityNew.intPk;
                            context_I.InputsAndOutputsForAJob.Update(iojentity);
                        }

                        //                                  //Find the transform calculations of the eleets in this
                        //                                  //      piw.
                        List<TrfcalentityTransformCalculationEntityDB> darrtrfentity =
                            context_I.TransformCalculation.Where(trf => 
                            ((trf.intnPkElementElementTypeI == eleetentity.intPk) ||
                            (trf.intnPkElementElementTypeO == eleetentity.intPk)) &&
                            (trf.intPkProcessInWorkflow == piwentity.intPk)).ToList();

                        foreach (TrfcalentityTransformCalculationEntityDB trfcalentity in darrtrfentity)
                        {
                            if (
                                trfcalentity.intnPkElementElementTypeI == eleetentity.intPk
                                )
                            {
                                trfcalentity.intnPkElementElementTypeI = eleetentityNew.intPk;
                            }
                            else
                            {
                                trfcalentity.intnPkElementElementTypeO = eleetentityNew.intPk;
                            }

                            context_I.TransformCalculation.Update(trfcalentity);
                        }

                        //                                  //Find paper transformations.
                        List<PatransPaperTransformationEntityDB> darrpatransentity = context_I.PaperTransformation.Where(
                            paper => paper.intnPkElementElementTypeI == eleetentity.intPk &&
                            paper.intPkProcessInWorkflow == piwentity.intPk).ToList();

                        foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                        {
                            if (
                                patransentity.intnPkElementElementTypeI == eleetentity.intPk
                                )
                            {
                                patransentity.intnPkElementElementTypeI = eleetentityNew.intPk;
                            }
                            else
                            {
                                patransentity.intnPkElementElementTypeO = eleetentityNew.intPk;
                            }
                            context_I.PaperTransformation.Update(patransentity);
                        }

                        //                                  //Find the periods of the eleets for this wf.
                        List<PerentityPeriodEntityDB> darrperentity = context_I.Period.Where(per =>
                            per.intPkWorkflow == wfentity.intPk &&
                            per.intnPkElementElementType == eleetentity.intPk).ToList();

                        foreach (PerentityPeriodEntityDB perentity in darrperentity)
                        {
                            perentity.intnPkElementElementType = eleetentityNew.intPk;
                            context_I.Period.Update(perentity);
                        }
                    }

                    //                                      //Get the eleele for the past process.
                    List<EleeleentityElementElementEntityDB> darreleeleentity = context_I.ElementElement.Where(eleele =>
                        eleele.intPkElementDad == pro_I.intPk).ToList();

                    foreach (EleeleentityElementElementEntityDB eleeleentity in darreleeleentity)
                    {
                        //                                  //Get the eleele about this process.
                        EleeleentityElementElementEntityDB eleeleentityNew = context_I.ElementElement.FirstOrDefault(
                            eleele => eleele.intPkElementDad == proNew_I.intPk &&
                            eleele.intPkElementSon == eleeleentity.intPkElementSon &&
                            eleele.boolUsage == eleeleentity.boolUsage &&
                            eleele.intnId == eleeleentity.intnId);

                        //                                  //Find the ios of the eleeles in this wf.
                        List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
                            io.intPkWorkflow == wfentity.intPk &&
                            io.intnPkElementElement == eleeleentity.intPk).ToList();

                        foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
                        {
                            ioentity.intnPkElementElement = eleeleentityNew.intPk;
                            context_I.InputsAndOutputs.Update(ioentity);
                        }

                        //                                  //Find the iojs of the eleeles in this wf.
                        List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity = context_I.InputsAndOutputsForAJob.
                            Where(io => io.intPkProcessInWorkflow == piwentity.intPk &&
                            io.intnPkElementElement == eleeleentity.intPk).ToList();

                        foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
                        {
                            iojentity.intnPkElementElement = eleeleentityNew.intPk;
                            context_I.InputsAndOutputsForAJob.Update(iojentity);
                        }

                        //                                  //Find the transform calculations of the eleeles in this
                        //                                  //      wf.
                        List<TrfcalentityTransformCalculationEntityDB> darrtrfentity =
                            context_I.TransformCalculation.Where(trf =>
                            ((trf.intnPkElementElementI == eleeleentity.intPk) ||
                            (trf.intnPkElementElementO == eleeleentity.intPk)) &&
                            (trf.intPkProcessInWorkflow == piwentity.intPk)).ToList();

                        foreach (TrfcalentityTransformCalculationEntityDB trfcalentity in darrtrfentity)
                        {
                            if (
                                trfcalentity.intnPkElementElementI == eleeleentity.intPk
                                )
                            {
                                trfcalentity.intnPkElementElementI = eleeleentityNew.intPk;
                            }
                            else
                            {
                                trfcalentity.intnPkElementElementO = eleeleentityNew.intPk;
                            }

                            context_I.TransformCalculation.Update(trfcalentity);
                        }

                        //                                  //Find paper transformations.
                        List<PatransPaperTransformationEntityDB> darrpatransentity = context_I.PaperTransformation.Where(
                            paper => paper.intnPkElementElementI == eleeleentity.intPk &&
                            paper.intPkProcessInWorkflow == piwentity.intPk).ToList();

                        foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                        {
                            if (
                                patransentity.intnPkElementElementI == eleeleentity.intPk
                                )
                            {
                                patransentity.intnPkElementElementI = eleeleentityNew.intPk;
                            }
                            else
                            {
                                patransentity.intnPkElementElementO = eleeleentityNew.intPk;
                            }
                            context_I.PaperTransformation.Update(patransentity);
                        }

                        //                                  //Find the periods of the eleeles for this wf.
                        List<PerentityPeriodEntityDB> darrperentity = context_I.Period.Where(per =>
                            per.intPkWorkflow == wfentity.intPk &&
                            per.intnPkElementElement == eleeleentity.intPk).ToList();

                        foreach (PerentityPeriodEntityDB perentity in darrperentity)
                        {
                            perentity.intnPkElementElement = eleeleentityNew.intPk;
                            context_I.Period.Update(perentity);
                        }
                    }
                }
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static int? intnGetEleetId(

            int intPkElementDad_I,
            int intPkElementTypeSon_I,
            bool boolUsage_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get all the eleet with the same ElementDad, 
            //                                              //      ElementTypeSon and Usage.
            List<EleetentityElementElementTypeEntityDB> darreleetentity = context_M.ElementElementType.Where(
                eleet => eleet.intPkElementDad == intPkElementDad_I && eleet.intPkElementTypeSon == intPkElementTypeSon_I &&
                eleet.boolUsage == boolUsage_I).ToList();

            int? intnId = null;
            /*CASE*/
            if (
                //                                          //There is only one record equal to this eleet.   
                darreleetentity.Count == 1
                )
            {
                darreleetentity[0].intnId = 1;
                context_M.ElementElementType.Update(darreleetentity[0]);
                context_M.SaveChanges();

                intnId = 2;
            }
            else if (
                //                                          //There is more than one record equal to this eleet
                darreleetentity.Count > 1
                )
            {
                intnId = darreleetentity.Max(eleet => eleet.intnId) + 1;
            }
            /*END-CASE*/

            return intnId;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static int? intnGetEleeleId(

            int intPkElementDad_I,
            int intPkElementSon_I,
            bool boolUsage_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get all the eleele with the same ElementDad, 
            //                                              //      ElementSon and Usage.
            List<EleeleentityElementElementEntityDB> darreleeleentity = context_M.ElementElement.Where(
                eleele => eleele.intPkElementDad == intPkElementDad_I && eleele.intPkElementSon == intPkElementSon_I &&
                eleele.boolUsage == boolUsage_I).ToList();

            int? intnId = null;
            /*CASE*/
            if (
                //                                          //There is only one record equal to this eleet.   
                darreleeleentity.Count == 1
                )
            {
                darreleeleentity[0].intnId = 1;
                context_M.ElementElement.Update(darreleeleentity[0]);
                context_M.SaveChanges();

                intnId = 2;
            }
            else if (
                //                                          //There is more than one record equal to this eleet
                darreleeleentity.Count > 1
                )
            {
                intnId = darreleeleentity.Max(eleele => eleele.intnId) + 1;
            }
            /*END-CASE*/

            return intnId;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddCustomTypeToProcess(
            //                                              //Create the relation between the process and its custom 
            //                                              //      resource type as an input or output.

            PsPrintShop ps_I,
            int intPkProcess_I,
            String strInputOrOutput_I,
            Odyssey2Context context_M,
            out int intPkProcessLast_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intPkProcessLast_O = 0;

            //                                              //Find the process.
            ProProcess proBase = ProProcess.proFromDB(intPkProcess_I, context_M);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Process not found.";
            if (
                proBase != null
                )
            {
                intPkProcessLast_O = proBase.intPk;

                //                                      //Find the custom type of a printshop.
                EtentityElementTypeEntityDB etentityCustom = context_M.ElementType.FirstOrDefault(
                    elet => elet.strCustomTypeId == RestypResourceType.strResCustomType
                    && elet.intPrintshopPk == ps_I.intPk);

                intStatus_IO = 402;
                strUserMessage_IO = "Custom resource not found.";
                strDevMessage_IO = "Custom type not found.";
                if (
                    etentityCustom != null
                    )
                {
                    bool boolUsage = strInputOrOutput_I == ProtypProcessType.strInput;

                    //                                      //Find process in workflow.
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = context_M.ProcessInWorkflow.Where(piw =>
                        piw.intPkProcess == proBase.intPk).ToList();

                    bool boolAnyWfWasCloned = false;
                    //                                      //Stores Workflows´ Pk.
                    List<int> darrintPkWfAlreadyCloned = new List<int>();
                    foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity)
                    {
                        //                                  //Find workflows.
                        WfentityWorkflowEntityDB wfentityBase = context_M.Workflow.FirstOrDefault(wf => wf.intPk ==
                            piwentity.intPkWorkflow);

                        //                                  //Stores the workflow after being verified.
                        WfentityWorkflowEntityDB wfentityNew = null;
                        if (
                            //                              //Checks whether a Workflow has been verified.
                            !darrintPkWfAlreadyCloned.Contains(wfentityBase.intPk)
                            )
                        {
                            //                              //Verifies if a workflow has a job.
                            ProdtypProductType.subAddWorkflowIfItIsNecessary(ps_I, wfentityBase, context_M,
                                out wfentityNew);
                            if (
                                //                          //Workflow was cloned.
                                wfentityBase != wfentityNew
                                )
                            {
                                boolAnyWfWasCloned = true;
                                darrintPkWfAlreadyCloned.Add(wfentityBase.intPk);
                            }
                        }
                    }

                    if (
                        boolAnyWfWasCloned
                        )
                    {
                        ProProcess proNew;
                        //                                  //Copy the old process into a new one.
                        ProtypProcessType.subCopyAProcess(proBase, context_M, out proNew);
                        //                                  //Update tables with the new process.
                        ProtypProcessType.subUpdateProcessPkInActiveWorkflows(proBase, proNew, context_M);

                        int? intnId = ProtypProcessType.intnGetEleetId(proNew.intPk, etentityCustom.intPk, boolUsage,
                            context_M);

                        //                                  //Create the relation between the process and resource type.
                        EleetentityElementElementTypeEntityDB eleetentity = new
                            EleetentityElementElementTypeEntityDB
                        {
                            intPkElementDad = proNew.intPk,
                            intPkElementTypeSon = etentityCustom.intPk,
                            boolUsage = boolUsage,
                            intnId = intnId
                        };
                        context_M.ElementElementType.Add(eleetentity);

                        //                                  //Return current process´s pk.
                        intPkProcessLast_O = proNew.intPk;
                    }
                    else
                    {
                        int? intnId = ProtypProcessType.intnGetEleetId(proBase.intPk, etentityCustom.intPk, boolUsage,
                            context_M);

                        //                                  //Create the relation between the process and resource type.
                        EleetentityElementElementTypeEntityDB eleetentity = new
                            EleetentityElementElementTypeEntityDB
                        {
                            intPkElementDad = proBase.intPk,
                            intPkElementTypeSon = etentityCustom.intPk,
                            boolUsage = boolUsage,
                            intnId = intnId
                        };
                        context_M.ElementElementType.Add(eleetentity);
                    }

                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddInitialDataToDb()
        {
            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Verify if the general type already exists.
            EtentityElementTypeEntityDB etentityGeneral = context.ElementType.FirstOrDefault(et =>
                et.strCustomTypeId == ProtypProcessType.strGeneralTypeId &&
                et.strResOrPro == EtElementTypeAbstract.strProcess &&
                et.intPrintshopPk == null);
            if (
                //                                          //General type does not exist.
                etentityGeneral == null
                )
            {
                //                                          //Add general type.
                int intTypePk;
                ProtypProcessType.subAddGeneralTypeToDB(out intTypePk);

                //                                          //Get the attributes.
                PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
                PathX syspath = syspathA.AddName("Process.csv");
                FileInfo sysfile = FileX.New(syspath);
                String[] arrAttributesData = sysfile.ReadAll();

                //                                          //Add the attributes to db.
                ProtypProcessType.subAddGeneralAttributesToDB(arrAttributesData, intTypePk);
            }

            ProtypProcessType.subAddXJDFProcessTypes();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddGeneralTypeToDB(
            out int intTypePK
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            //                                          //Create the type.
            EtentityElementTypeEntityDB etentityGeneral = new EtentityElementTypeEntityDB
            {
                strXJDFTypeId = ProtypProcessType.strGeneralTypeId,
                strAddedBy = EtElementTypeAbstract.strXJDFVersion,
                intPrintshopPk = null,
                strCustomTypeId = EtElementTypeAbstract.strXJDFPrefix +
                    ProtypProcessType.strGeneralTypeId,
                strResOrPro = EtElementTypeAbstract.strProcess
            };

            context.ElementType.Add(etentityGeneral);
            context.SaveChanges();

            intTypePK = etentityGeneral.intPk;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAddGeneralAttributesToDB(
            String[] arrstrAttributesData_I,
            int intTypePk_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            foreach (String strAttributeData in arrstrAttributesData_I)
            {
                //                                          //Get XJDF name.
                String strData = strAttributeData;

                //                                      //Get the name.
                String strXJDFName = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                      //Get the cardinality.
                String strCardinality = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                      //Get the datatype.
                String strDatatype = strData.Substring(0, strData.IndexOf(EtElementTypeAbstract.strSeparator));
                strData = strData.Substring(strData.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                      //Get the description.
                String strDescription = strData;

                //                                      //Add the attr to db.
                AttrentityAttributeEntityDB attrentity = new AttrentityAttributeEntityDB
                {
                    strXJDFName = strXJDFName,
                    strCustomName = EtElementTypeAbstract.strXJDFPrefix + strXJDFName,
                    strCardinality = strCardinality,
                    strDatatype = strDatatype,
                    strDescription = strDescription,
                    strScope = EtElementTypeAbstract.strXJDFVersion
                };

                context.Attribute.Add(attrentity);
                context.SaveChanges();

                int intAttributePk = attrentity.intPk;

                //                                      //Create the relation between the type and the attr.
                AttretentityAttributeElementTypeEntityDB attretentity = new AttretentityAttributeElementTypeEntityDB
                {
                    intPkAttribute = intAttributePk,
                    intPkElementType = intTypePk_I
                };

                context.AttributeElementType.Add(attretentity);
                context.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddXJDFProcessTypes(
            //                                              //Add all the processes that are define in the XJDFv2.0.
            )
        {
            //                                          //Get the attributes.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName("XJDFProcesses.csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrstrProcessData = sysfile.ReadAll();

            foreach (String strProcessName in arrstrProcessData)
            {
                int intPk;
                int intStatus;
                EtElementTypeAbstract.subAddXJDFType(strProcessName, EtElementTypeAbstract.strProcess,
                    out intPk, out intStatus);

                if (
                    intPk > 0
                    )
                {
                    AttrAttribute[] arrattr = ProtypProcessType.darrattrGetGeneralAttribute();
                    List<int> darrint = new List<int>();
                    foreach (AttrAttribute attr in arrattr)
                    {
                        darrint.Add(attr.intPk);
                    }

                    EtElementTypeAbstract.subAddExistingAttributesToType(intPk, darrint.ToArray(),
                        out intStatus);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddInitialDataOfProcessWithResourcesToDb(
            )
        {
            //                                              //Get the process.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName("ProcessesWithResources.csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrProcessesData = sysfile.ReadAll();

            Odyssey2Context context = new Odyssey2Context();
            foreach (String strProcessName in arrProcessesData)
            {
                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                    etentity.strResOrPro == EtElementTypeAbstract.strProcess &&
                    etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion &&
                    etentity.strXJDFTypeId == strProcessName);

                if (
                    etentity != null
                    )
                {
                    ProtypProcessType.subAddResourcesToProcess(strProcessName, etentity.intPk);
                }

            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddResourcesToProcess(
            String strProcessName_I,
            int intPk_I
            )
        {
            //                                              //Get the resources for the process.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName(strProcessName_I + ".csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrResourcesData = sysfile.ReadAll();

            Odyssey2Context context = new Odyssey2Context();
            foreach (String strResourceData in arrResourcesData)
            {
                String strInputOrOutput = strResourceData.Substring(0,
                    strResourceData.IndexOf(EtElementTypeAbstract.strSeparator));

                String strResourceName = strResourceData.Substring(strResourceData.IndexOf(
                    EtElementTypeAbstract.strSeparator) + 1);

                bool boolUsage = strInputOrOutput == ProtypProcessType.strInput;

                EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                    etentity.strResOrPro == EtElementTypeAbstract.strResource &&
                    etentity.strAddedBy == EtElementTypeAbstract.strXJDFVersion &&
                    etentity.strXJDFTypeId == strResourceName);

                if (
                    etentity != null
                    )
                {
                    EtetentityElementTypeElementTypeEntityDB etetentity =
                        new EtetentityElementTypeElementTypeEntityDB
                        {
                            intPkElementTypeDad = intPk_I,
                            intPkElementTypeSon = etentity.intPk,
                            boolnUsage = boolUsage
                        };
                    context.ElementTypeElementType.Add(etetentity);
                    context.SaveChanges();
                }

            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateInitialClassificationProcessToDb()
        {
            //                                              //Get the types.
            PathX syspathA = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
            PathX syspath = syspathA.AddName("ClassificationProcess.csv");
            FileInfo sysfile = FileX.New(syspath);
            String[] arrProcessData = sysfile.ReadAll();

            //                                              //Create a connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (String strProcess in arrProcessData)
            {
                //                                          //Get Process'name 
                String strProcessName = strProcess.Substring(0,
                    strProcess.IndexOf(EtElementTypeAbstract.strSeparator));
                //                                          //Get Process'classification 
                String strClassificationProcess = strProcess.Substring(
                    strProcess.IndexOf(EtElementTypeAbstract.strSeparator) + 1);

                //                                          //Verify if the process already exists.
                EtentityElementTypeEntityDB etentityGeneral = context.ElementType.FirstOrDefault(et =>
                    et.strCustomTypeId == EtElementTypeAbstract.strXJDFPrefix + strProcessName &&
                    et.strResOrPro == EtElementTypeAbstract.strProcess &&
                    et.intPrintshopPk == null);

                if (
                   //                                       //Process exist and string classification is empty
                   //                                       //    update the clasification.
                   etentityGeneral != null &&
                   etentityGeneral.strClassification == null
                   )
                {
                    //                                      //Update classification.
                    etentityGeneral.strClassification = strClassificationProcess;
                    context.ElementType.Update(etentityGeneral);
                    context.SaveChanges();
                }

            }

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteProcess(
            //                                              //Delete a process or a node in a workflow.

            int? intnPkProcessInWorkflow_I,
            int? intnPkNode_I,
            PsPrintShop ps_I,
            bool boolSuperAdmin_I,
            IHubContext<ConnectionHub> iHubContext_I,
            out int intPkWorkflow_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intPkWorkflow_O = 0;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Can not delete a PIW and a node same time or at least one should be removed.";

            if (
                //                                          //It is a process
                intnPkProcessInWorkflow_I != null && intnPkNode_I == null
                )
            {
                //                                          //Find Piw.
                //                                      //Find process in workflow.
                PiwentityProcessInWorkflowEntityDB piwentity = (from piwentityValid in context.ProcessInWorkflow
                                                                join wfentity in context.Workflow on
                                                                piwentityValid.intPkWorkflow equals wfentity.intPk
                                                                where piwentityValid.intPk ==
                                                                intnPkProcessInWorkflow_I &&
                                                                wfentity.intPkPrintshop == ps_I.intPk
                                                                select piwentityValid).FirstOrDefault();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No process in workflow found.";
                if (
                    piwentity != null
                    )
                {
                    //                                      //Workflow that contains piw to delete.
                    WfentityWorkflowEntityDB wfentityBase = context.Workflow.FirstOrDefault(wf =>
                        wf.intPk == piwentity.intPkWorkflow);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The WF generic only can be modified for the administrator.";
                    if (
                        (
                        //                                      //The workflow is generic and
                        //                                      //Only can be modified for the 
                        //                                      //  admin(super user.)
                        wfentityBase.boolnGeneric == true &&
                        boolSuperAdmin_I
                        )
                        ||
                        (
                        (wfentityBase.boolnGeneric == null || wfentityBase.boolnGeneric == false)
                        )
                        )
                    {
                        WfentityWorkflowEntityDB wfentityNew;
                        //                                      //Verify if it is necessary to clone the wf.
                        ProdtypProductType.subAddWorkflowIfItIsNecessary(ps_I, wfentityBase, context, out wfentityNew);
                        intPkWorkflow_O = wfentityNew.intPk;

                        //                                      //Overwrite piw object if workflow was cloned.
                        piwentity = context.ProcessInWorkflow.FirstOrDefault(piw => piw.intPkWorkflow == wfentityNew.intPk &&
                            piw.intProcessInWorkflowId == piwentity.intProcessInWorkflowId);

                        //                                      //Check if there is a final product or a post proccess
                        //                                      //      in the previous processes to the node
                        bool boolThereIsAFinalProductOrAPostProcess =
                            ProdtypProductType.boolThereIsAFinalProductInPreviousProcessesRecursive(piwentity, null,
                            context);

                        if (
                            boolThereIsAFinalProductOrAPostProcess
                            )
                        {
                            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityFollowingPIWs =
                                new List<PiwentityProcessInWorkflowEntityDB>();
                            List<IoentityInputsAndOutputsEntityDB> darrioentityNodesNotUsed =
                                new List<IoentityInputsAndOutputsEntityDB>();

                            ProdtypProductType.subGetFollowingPIWsAndNodesRecursive(piwentity, null,
                                ref darrpiwentityFollowingPIWs, ref darrioentityNodesNotUsed);

                            //                                  //Update processes to normal process
                            foreach (PiwentityProcessInWorkflowEntityDB piwentityToUpdate in darrpiwentityFollowingPIWs)
                            {
                                piwentityToUpdate.boolIsPostProcess = false;

                                context.ProcessInWorkflow.Update(piwentityToUpdate);
                            }
                        }

                        //                                      //Delete the calculations.
                        ProtypProcessType.subDeleteCalculationForAProcessInWorkflow(piwentity, iHubContext_I);

                        //                                      //Delete the ios for products.
                        ProtypProcessType.subDeleteIOsForAProcessInWorkflow(piwentity);

                        //                                      //Delete the ios for jobs.
                        ProtypProcessType.subDeleteIosForAJobForAProcessInWorkflow(piwentity);

                        //                                      //Delete periods.
                        ProtypProcessType.subDeletePeriodsForAProcessInWorkflow(piwentity, iHubContext_I);

                        //                                      //Delete paper transformation.
                        ProtypProcessType.subDeletePaperTransformations(piwentity);

                        if (
                            piwentity.intnId != null
                            )
                        {
                            //                                  //Update the id for the processes in the same wf.
                            ProtypProcessType.subUpdateProcessInWorkflowId(piwentity);
                        }

                        //                                      //Get the process for that wf.
                        IQueryable<PiwentityProcessInWorkflowEntityDB> setpiwentityAll = context.ProcessInWorkflow.Where(
                            piw => piw.intPkWorkflow == piwentity.intPkWorkflow);
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAll = setpiwentityAll.ToList();

                        //                                      //Delete EstimationData entries.
                        JobJob.subDeleteEstimationDataEntriesForAWorkflow(context, darrpiwentityAll);

                        //                                      //Delete the piw.
                        context.ProcessInWorkflow.Remove(piwentity);
                        context.SaveChanges();

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success";
                        strDevMessage_IO = "";
                    }
                }
            }
            else if (
                //                                          //It is a node
                intnPkNode_I != null && intnPkProcessInWorkflow_I == null
                )
            {
                //                                          //Find Node.
                IoentityInputsAndOutputsEntityDB ioentity = (from ioentityNode in context.InputsAndOutputs
                                                                join wfentity in context.Workflow on
                                                                ioentityNode.intPkWorkflow equals wfentity.intPk
                                                                where ioentityNode.intPk == intnPkNode_I &&
                                                                ioentityNode.intnPkElementElement == null &&
                                                                ioentityNode.intnPkElementElementType == null &&
                                                                ioentityNode.intnProcessInWorkflowId == null &&
                                                                wfentity.intPkPrintshop == ps_I.intPk
                                                                select ioentityNode).FirstOrDefault();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Not node in workflow found or it is not a node.";
                if (
                    ioentity != null
                    )
                {
                    //                                      //Workflow that contains node to delete.
                    WfentityWorkflowEntityDB wfentityBase = context.Workflow.FirstOrDefault(wf =>
                        wf.intPk == ioentity.intPkWorkflow);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "The WF generic only can be modified for the administrator.";
                    if (
                        (
                        //                                      //The workflow is generic and
                        //                                      //Only can be modified for the 
                        //                                      //  admin(super user.)
                        wfentityBase.boolnGeneric == true &&
                        boolSuperAdmin_I
                        )
                        ||
                        (
                        (wfentityBase.boolnGeneric == null || wfentityBase.boolnGeneric == false)
                        )
                        )
                    {
                        WfentityWorkflowEntityDB wfentityNew;
                        //                                      //Verify if it is necessary to clone the wf.
                        ProdtypProductType.subAddWorkflowIfItIsNecessary(ps_I, wfentityBase, context, out wfentityNew);
                        intPkWorkflow_O = wfentityNew.intPk;

                        //                                      //Overwrite Node object if workflow was cloned.
                        ioentity = context.InputsAndOutputs.FirstOrDefault(io => io.intPkWorkflow == wfentityNew.intPk &&
                            io.intnPkElementElement == null && io.intnPkElementElementType == null &&
                            io.intnProcessInWorkflowId == null && io.strLink == ioentity.strLink);

                        //                                      //Check if there is a final product or a post proccess
                        //                                      //      in the previous processes to the node
                        bool boolThereIsAFinalProductOrAPostProcess =
                            ProdtypProductType.boolThereIsAFinalProductInPreviousProcessesRecursive(null, ioentity,
                            context);

                        if (
                            boolThereIsAFinalProductOrAPostProcess
                            )
                        {
                            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityFollowingPIWs =
                                new List<PiwentityProcessInWorkflowEntityDB>();
                            List<IoentityInputsAndOutputsEntityDB> darrioentityNodesNotUsed =
                                new List<IoentityInputsAndOutputsEntityDB>();

                            ProdtypProductType.subGetFollowingPIWsAndNodesRecursive(null, ioentity,
                                ref darrpiwentityFollowingPIWs, ref darrioentityNodesNotUsed);

                            //                                  //Update processes to normal process
                            foreach (PiwentityProcessInWorkflowEntityDB piwentityToUpdate in darrpiwentityFollowingPIWs)
                            {
                                piwentityToUpdate.boolIsPostProcess = false;

                                context.ProcessInWorkflow.Update(piwentityToUpdate);
                            }
                        }

                        //                                      //Find IOs that have link with this node
                        List<IoentityInputsAndOutputsEntityDB> darrioentity = context.InputsAndOutputs.Where(io =>
                            io.intPkWorkflow == wfentityNew.intPk && io.strLink == ioentity.strLink &&
                            io.intPk != ioentity.intPk).ToList();

                        //                                      //Delete IO links related with this node
                        foreach (IoentityInputsAndOutputsEntityDB ioentityCurrent in darrioentity)
                        {
                            //                                  //Remove link.
                            ProdtypProductType.subRemoveLink(ioentityCurrent, context);
                        }

                        //                                      //Find nodes that have link with this node
                        List<LinknodLinkNodeEntityDB> darrlinknodentity = context.LinkNode.Where(node =>
                            node.intPkNodeI == ioentity.intPk || node.intPkNodeO == ioentity.intPk).ToList();

                        //                                      //Delete node links related with this node
                        foreach (LinknodLinkNodeEntityDB linknodeentity in darrlinknodentity)
                        {
                            Tools.subDeleteCondition(null, linknodeentity.intPk, null, null, context);
                            context.LinkNode.Remove(linknodeentity);
                        }

                        //                                      //Get the process for that wf.
                        IQueryable<PiwentityProcessInWorkflowEntityDB> setpiwentityAll = context.ProcessInWorkflow.Where(
                            piw => piw.intPkWorkflow == ioentity.intPkWorkflow);
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAll = setpiwentityAll.ToList();

                        //                                      //Delete EstimationData entries.
                        JobJob.subDeleteEstimationDataEntriesForAWorkflow(context, darrpiwentityAll);

                        //                                      //Delete the node.
                        context.InputsAndOutputs.Remove(ioentity);
                        context.SaveChanges();

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteCalculationForAProcessInWorkflow(
            //                                              //Delete the calculations made in this piw.
            //                                              //Delete transform calculations made in this piw.

            PiwentityProcessInWorkflowEntityDB piwentity_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get calculations.
            List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(cal =>
                cal.intnPkWorkflow == piwentity_I.intPkWorkflow &&
                cal.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId).ToList();

            foreach (CalentityCalculationEntityDB calentity in darrcalentity)
            {
                //                                          //Delete period associated with the calculation.
                PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per => 
                    per.intnPkCalculation == calentity.intPk);
                if (
                    perentity != null
                    )
                {
                    //                                      //Find alerts about this period.
                    List<AlertentityAlertEntityDB> darralertentity =
                        context.Alert.Where(alert =>
                        alert.intnPkPeriod == perentity.intPk).ToList();

                    foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                    {
                        //                                  //Delete alerts about this period.

                        if (
                            //                              //Notification not read.
                            !PsPrintShop.boolNotificationReadByUser(alertentity,
                                (int)alertentity.intnContactId)
                            )
                        {
                            AlnotAlertNotification.subReduceToOne(
                                (int)alertentity.intnContactId,
                                iHubContext_I);
                        }

                        context.Alert.Remove(alertentity);
                    }

                    context.Period.Remove(perentity);
                }

                //                                          //Find paper transformations associated to this cal.
                List<PatransPaperTransformationEntityDB> darrpatransentity = context.PaperTransformation.Where(paper =>
                    paper.intnPkCalculationOwn == calentity.intPk || 
                    paper.intnPkCalculationLink == calentity.intPk).ToList();

                foreach (PatransPaperTransformationEntityDB patransentityToDelete in darrpatransentity)
                {
                    context.PaperTransformation.Remove(patransentityToDelete);
                }

                //                                          //Delete their conditions.
                Tools.subDeleteCondition(calentity.intPk, null, null, null, context);

                //                                          //Delete calculations.
                context.Calculation.Remove(calentity);
            }

            //                                              //Find transform calculations.
            List<TrfcalentityTransformCalculationEntityDB> darrtrfcalentity = context.TransformCalculation.Where(trf =>
                trf.intPkProcessInWorkflow == piwentity_I.intPk).ToList();

            foreach (TrfcalentityTransformCalculationEntityDB trfcalentity in darrtrfcalentity)
            {
                //                                          //Delete their conditions.
                Tools.subDeleteCondition(null, null, null, trfcalentity.intPk, context);

                //                                          //Delete transform calculations.
                context.TransformCalculation.Remove(trfcalentity);
            }

            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteIOsForAProcessInWorkflow(
            //                                              //Delete the info about ios for the process in workflow
            //                                              //      in product workflow.

            PiwentityProcessInWorkflowEntityDB piwentity_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get ios.
            IQueryable<IoentityInputsAndOutputsEntityDB> setioentity = context.InputsAndOutputs.Where(io =>
                io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId);
            List<IoentityInputsAndOutputsEntityDB> darrioentity = setioentity.ToList();

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
            {
                if (
                    ioentity.strLink != null
                    )
                {
                    //                                      //Valid if there is a node with the same link
                    bool boolThereIsAnode = context.InputsAndOutputs.Where(io =>
                        io.intPkWorkflow == ioentity.intPkWorkflow && io.strLink == ioentity.strLink).ToList().
                        Any(io => ProdtypProductType.boolIsTheIOANode(io) == true);

                    if (
                        //                                  //If there is not a node with the same link
                        !boolThereIsAnode
                        )
                    {
                        //                                  //Get the elements for that link.
                        IQueryable<IoentityInputsAndOutputsEntityDB> setioentityLink = context.InputsAndOutputs.Where(io =>
                            io.intPkWorkflow == ioentity.intPkWorkflow && io.strLink == ioentity.strLink);
                        List<IoentityInputsAndOutputsEntityDB> darrioentityLink = setioentityLink.ToList();

                        //                                  //Update the io with link.
                        foreach (IoentityInputsAndOutputsEntityDB ioentityLink in darrioentityLink)
                        { 
                            if (
                            (ioentityLink.intnPkResource != null) || (ioentityLink.intnGroupResourceId != null)
                            )
                            {
                                ioentityLink.strLink = null;
                            }
                            else
                            {
                                context.InputsAndOutputs.Remove(ioentityLink);
                            } 
                        }
                    }
                }
                else
                {
                    //                                          //Delete GroupResources if exists.
                    if (
                        ioentity.intnGroupResourceId != null
                        )
                    {
                        //                                      //Get group to delete.
                        List<GpresentityGroupResourceEntityDB> darrgpres = context.GroupResource.Where(gp =>
                            gp.intId == ioentity.intnGroupResourceId).ToList();

                        //                                      //Delete the GroupResource.
                        foreach (GpresentityGroupResourceEntityDB gpres in darrgpres)
                        {
                            context.GroupResource.Remove(gpres);
                        }
                        context.SaveChanges();
                    }
                }
                //                                          //Delete the io.
                context.InputsAndOutputs.Remove(ioentity);
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteIosForAJobForAProcessInWorkflow(
            //                                              //Delete the info about ios for the process in workflow
            //                                              //      in jobs workflows.

            PiwentityProcessInWorkflowEntityDB piwentity_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get ios for a job.
            IQueryable<IojentityInputsAndOutputsForAJobEntityDB> setiojentity = context.InputsAndOutputsForAJob.
                Where(ioj => ioj.intPkProcessInWorkflow == piwentity_I.intPk);
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity = setiojentity.ToList();

            //                                              //Delete.
            foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
            {
                context.InputsAndOutputsForAJob.Remove(iojentity);
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeletePeriodsForAProcessInWorkflow(
            //                                              //Deleted periods related to the piw.

            PiwentityProcessInWorkflowEntityDB piwentity_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find periods.
            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(period =>
                period.intPkWorkflow == piwentity_I.intPkWorkflow &&
                period.intProcessInWorkflowId == piwentity_I.intProcessInWorkflowId).ToList();

            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                //                                          //Find alerts about this period.
                List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert =>
                    alert.intnPkPeriod == perentity.intPk).ToList();

                foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                {
                    //                                      //Delete alerts about this period.

                    if (
                        //                                  //Notification not read.
                        !PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                        )
                    {
                        AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId,
                            iHubContext_I);
                    }

                    context.Alert.Remove(alertentity);
                }

                context.Period.Remove(perentity);
            }

            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeletePaperTransformations(
            PiwentityProcessInWorkflowEntityDB piwentity_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find paper transformations.
            List<PatransPaperTransformationEntityDB> darrpatransentity = context.PaperTransformation.Where(
                paper => paper.intPkProcessInWorkflow == piwentity_I.intPk).ToList();

            foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
            {
                context.PaperTransformation.Remove(patransentity);
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subUpdateProcessInWorkflowId(
            //                                              //Update the id for the processes in workflow that are in 
            //                                              //      the same product and are about the same process. 

            PiwentityProcessInWorkflowEntityDB piwentity_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            IQueryable<PiwentityProcessInWorkflowEntityDB> setpiwentity = context.ProcessInWorkflow.Where(piw =>
                piw.intPkProcess == piwentity_I.intPkProcess &&
                piw.intPkWorkflow == piwentity_I.intPkWorkflow);
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = setpiwentity.ToList();

            int? intnId = darrpiwentity.Max(piw => piw.intnId);

            /*CASE*/
            if (
                //                                  //There is only two process in wf.
                intnId == 2
                )
            {
                foreach (PiwentityProcessInWorkflowEntityDB piwentityOther in darrpiwentity)
                {
                    if (
                        piwentityOther.intPk != piwentity_I.intPk
                        )
                    {
                        //                          //The id is not necessary anymore.
                        piwentityOther.intnId = null;
                        context.ProcessInWorkflow.Update(piwentityOther);
                    }
                }
                context.SaveChanges();
            }
            else if (
                //                                          //Is an intermediate process.
                (intnId > 2) &&
                (piwentity_I.intnId != intnId)
                )
            {
                foreach (PiwentityProcessInWorkflowEntityDB piwentityOther in darrpiwentity)
                {
                    if (
                        //                                  //The id is bigger.
                        (piwentityOther.intPk != piwentity_I.intPk) &&
                        (piwentityOther.intnId > piwentity_I.intnId)
                        )
                    {
                        //                                  //Now is one less because piwentity is going to be deleted.
                        piwentityOther.intnId = piwentityOther.intnId - 1;
                        context.ProcessInWorkflow.Update(piwentityOther);
                    }
                }
            }
            else if (
                //                                          //Is the last process.
                (intnId > 2) &&
                (piwentity_I.intnId != intnId)
                )
            {
                //                                          //There is nothing to do.
            }
            /*END-CASE*/
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteProcessFromPrintshop(
            //                                              //Delete a process from DB and all data related to it.

            int intPkProcess_I,
            PsPrintShop ps_I,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the process to Delete.
            ProProcess pro = ProProcess.proFromDB(intPkProcess_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Process not found.";
            if (
                pro != null && pro.protypBelongsTo.intPkPrintshop == ps_I.intPk
                )
            {
                if (
                    ProtypProcessType.boolIsProcessInSomeWorkflowWithJobsInProgressOrCompleted(pro, context)
                    )
                {
                    //                                      //Copy the process and the workflows.
                    ProProcess proNew;
                    ProtypProcessType.subCopyAProcess(pro, context, out proNew);
                    ProtypProcessType.subCreateTheNewWorkflowsForModifiedTheProcess(ps_I, pro, context);
                    ProtypProcessType.subUpdateProcessPkInActiveWorkflows(pro, proNew, context);

                    //                                      //Overwrite proces object.
                    pro = proNew;
                }

                //                                          //Find PIW which are of the same process.
                IQueryable<PiwentityProcessInWorkflowEntityDB> setpiwentity = context.ProcessInWorkflow.Where(piw =>
                    piw.intPkProcess == pro.intPk);
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = setpiwentity.ToList();

                //                                          //List of piw that will hold the ones to delete their 
                //                                          //      estimates.
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityOfAllWfToDeleteEstimates = new 
                    List<PiwentityProcessInWorkflowEntityDB>();

                foreach(PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity)
                {
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityOfOneWf = 
                        context.ProcessInWorkflow.Where(piw => piw.intPkWorkflow == piwentity.intPkWorkflow).ToList();
                    darrpiwentityOfAllWfToDeleteEstimates.AddRange(darrpiwentityOfOneWf);
                }

                //                                          //Delete EstimationData entries.
                JobJob.subDeleteEstimationDataEntriesForAWorkflow(context, darrpiwentityOfAllWfToDeleteEstimates);

                foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity)
                {
                    //                                      //Find records for this piw in IO table.
                    List<IoentityInputsAndOutputsEntityDB> darrioentity =
                        context.InputsAndOutputs.Where(ioentity =>
                        ioentity.intPkWorkflow == piwentity.intPkWorkflow &&
                        ioentity.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                    foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity)
                    {
                        //                                  //Delete IOs.
                        ProtypProcessType.subDeleteIO(ioentity);
                    }

                    //                                      //Find in IOJob table.
                    List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity =
                        context.InputsAndOutputsForAJob.Where(iojentity =>
                        iojentity.intPkProcessInWorkflow == piwentity.intPk).ToList();

                    
                    foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
                    {
                        //                                  //Delete process in IOJob table.
                        context.InputsAndOutputsForAJob.Remove(iojentity);
                    }

                    //                                      //Find paper trans.
                    List<PatransPaperTransformationEntityDB> darrpatransentity = context.PaperTransformation.Where(
                        paper => paper.intPkProcessInWorkflow == piwentity.intPk).ToList();

                    foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                    {
                        //                                  //Delete paper transformation.
                        context.PaperTransformation.Remove(patransentity);
                    }

                    //                                      //Find in calculation table.
                    List<CalentityCalculationEntityDB> darrcalentityPiw = context.Calculation.Where(calentity =>
                        calentity.intnPkWorkflow == piwentity.intPkWorkflow &&
                        calentity.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                    foreach (CalentityCalculationEntityDB calentity in darrcalentityPiw)
                    {
                        //                                  //Find paper transformations associated to this cal.
                        List<PatransPaperTransformationEntityDB> darrpatransentityToDelete = 
                            context.PaperTransformation.Where(paper => paper.intnPkCalculationOwn == calentity.intPk ||
                            paper.intnPkCalculationLink == calentity.intPk).ToList();

                        foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentityToDelete)
                        {
                            //                              //Remove paper trans.
                            context.PaperTransformation.Remove(patransentity);
                        }

                        Tools.subDeleteCondition(calentity.intPk, null, null, null, context);
                        //                                  //Delete calculations.
                        context.Calculation.Remove(calentity);
                    }

                    //                                      //Find transform calculations.
                    List<TrfcalentityTransformCalculationEntityDB> darrtrfcalentity = context.TransformCalculation.Where(
                        trf => trf.intPkProcessInWorkflow == piwentity.intPk).ToList();

                    foreach (TrfcalentityTransformCalculationEntityDB trfcalentity in darrtrfcalentity)
                    {
                        Tools.subDeleteCondition(null, null, null, trfcalentity.intPk, context);
                        //                                  //Delete transform calculation.
                        context.TransformCalculation.Remove(trfcalentity);
                    }

                    //                                      //Delete process in workflow.
                    //                                      //The first SaveChanges is needed because piw is FK.
                    context.SaveChanges();
                    context.ProcessInWorkflow.Remove(piwentity); 
                    context.SaveChanges();
                }
                
                //                                          //Get calculations for this process.
                List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(calentity =>
                    calentity.intnPkProcess == intPkProcess_I && calentity.strEndDate == null).ToList();

                foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                {
                    //                                      //Find paper transformations associated to this cal.
                    List<PatransPaperTransformationEntityDB> darrpatransentityToDelete =
                        context.PaperTransformation.Where(paper => paper.intnPkCalculationOwn == calentity.intPk ||
                        paper.intnPkCalculationLink == calentity.intPk).ToList();

                    foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentityToDelete)
                    {
                        //                                  //Remove paper trans.
                        context.PaperTransformation.Remove(patransentity);
                    }

                    Tools.subDeleteCondition(calentity.intPk, null, null, null, context);
                    //                                      //Delete the calculations.
                    context.Calculation.Remove(calentity);
                }

                ProtypProcessType.subDeleteinEleeleAndEleet(pro);

                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele => ele.intPk == pro.intPk);
                //                                          //Delete the process in Element table.
                context.Element.Remove(eleentity);

                //                                          //Delete the process type if no more processes.
                List<EleentityElementEntityDB> darreleentity = context.Element.Where(ele => ele.intPkElementType ==
                    eleentity.intPkElementType).ToList();
                if (
                    //                                      //Verify that type process does not have processes.
                    darreleentity.Count == 0
                    )
                {
                    EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                        et.intPk == eleentity.intPkElementType);
                    //                                      //Delete the process type.
                    context.ElementType.Remove(etentity);
                }

                context.SaveChanges();

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteIO(

            IoentityInputsAndOutputsEntityDB ioentity_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            if (
               //                                           //IO without link, means with res or group.
               ioentity_I.strLink == null
               )
            {
                Tools.subDeleteCondition(null, null, ioentity_I.intPk, null, context);
                //                                          //Delete the res.
                context.InputsAndOutputs.Remove(ioentity_I);
            }
            else
            {
                //                                          //IO has link.
                //                                          //Get the two parts of the link. 
                List<IoentityInputsAndOutputsEntityDB> darrioentityHasLink =
                    context.InputsAndOutputs.Where(io => io.strLink == ioentity_I.strLink && 
                    io.intPkWorkflow == ioentity_I.intPkWorkflow).ToList();
                if (
                    //                                      //IO without res or group.
                    (ioentity_I.intnPkResource == null) &&
                    (ioentity_I.intnGroupResourceId == null)
                    )
                {
                    //                                      //Delete the link.
                    foreach (IoentityInputsAndOutputsEntityDB ioentityHasLink in darrioentityHasLink)
                    {
                        Tools.subDeleteCondition(null, null, ioentityHasLink.intPk, null, context);
                        context.InputsAndOutputs.Remove(ioentityHasLink);
                    }
                }
                else
                {
                    //                                      //IO with res or group.

                    //                                      //Delete the link.
                    foreach (IoentityInputsAndOutputsEntityDB ioentityHasLink in darrioentityHasLink)
                    {
                        ioentityHasLink.strLink = null;
                        context.InputsAndOutputs.Update(ioentityHasLink);
                    }

                    //                                      //Delete the IO for the one of this piw (this pro).
                    IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                        io.intPk == ioentity_I.intPk);

                    Tools.subDeleteCondition(null, null, ioentity.intPk, null, context);
                    context.InputsAndOutputs.Remove(ioentity);
                }
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteinEleeleAndEleet(
            ProProcess pro_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find process in ElementElement table.
            List<EleeleentityElementElementEntityDB> darreleeleentity = context.ElementElement.Where(eleele =>
            eleele.intPkElementDad == pro_I.intPk).ToList();

            foreach (EleeleentityElementElementEntityDB eleeleentity in darreleeleentity)
            {
                //                                          //Get calculations for this eleele.
                List<CalentityCalculationEntityDB> darrcalentityEleele = context.Calculation.Where(
                    calentityEleele => calentityEleele.intnPkElementElement == eleeleentity.intPk).ToList();

                //                                          //Delete the calculations.
                foreach (CalentityCalculationEntityDB calentityEleele in darrcalentityEleele)
                {
                    //                                      //Find paper transformations associated to this cal.
                    List<PatransPaperTransformationEntityDB> darrpatransentity = context.PaperTransformation.Where(
                        paper => paper.intnPkCalculationOwn == calentityEleele.intPk ||
                        paper.intnPkCalculationLink == calentityEleele.intPk).ToList();

                    foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                    {
                        //                                  //Remove paper trans.
                        context.PaperTransformation.Remove(patransentity);
                    }

                    Tools.subDeleteCondition(calentityEleele.intPk, null, null, null, context);
                    context.Calculation.Remove(calentityEleele);
                }

                //                                          //Delete eleele in ElementElement table.
                context.ElementElement.Remove(eleeleentity);
            }

            //                                              //Find process in ElementElementType table.
            List<EleetentityElementElementTypeEntityDB> darreleetentity = context.ElementElementType.Where(
                eleet => eleet.intPkElementDad == pro_I.intPk).ToList();

            foreach (EleetentityElementElementTypeEntityDB eleetentity in darreleetentity)
            {
                //                                          //Get calculations for this eleet.
                List<CalentityCalculationEntityDB> darrcalentityEleet = context.Calculation.Where(
                    calentityEleet => calentityEleet.intnPkElementElementType == eleetentity.intPk).ToList();

                //                                          //Delete the calculations.
                foreach (CalentityCalculationEntityDB calentityEleet in darrcalentityEleet)
                {
                    //                                      //Find paper transformations associated to this cal.
                    List<PatransPaperTransformationEntityDB> darrpatransentity = context.PaperTransformation.Where(
                        paper => paper.intnPkCalculationOwn == calentityEleet.intPk ||
                        paper.intnPkCalculationLink == calentityEleet.intPk).ToList();

                    foreach (PatransPaperTransformationEntityDB patransentity in darrpatransentity)
                    {
                        //                                  //Remove paper trans.
                        context.PaperTransformation.Remove(patransentity);
                    }

                    Tools.subDeleteCondition(calentityEleet.intPk, null, null, null, context);
                    context.Calculation.Remove(calentityEleet);
                }

                //                                          //Delete eleet in ElementElementType table.
                context.ElementElementType.Remove(eleetentity);
            }
            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddProcessType(

            int intPkProcessTypeBase_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            ref int intPkNewProcessType_IO
            )
        {
            //                                              //Find base workflow process type
            EtentityElementTypeEntityDB etentityBaseProcessType = context_M.ElementType.FirstOrDefault(et =>
                et.intPk == intPkProcessTypeBase_I);

            //                                              //Find new workflow process type
            EtentityElementTypeEntityDB etentityNewProcessType = context_M.ElementType.FirstOrDefault(et =>
                et.strXJDFTypeId == etentityBaseProcessType.strXJDFTypeId &&
                et.strResOrPro == EtElementTypeAbstract.strProcess &&
                et.intPrintshopPk == ps_I.intPk);

            int intStatus = 400;
            if (
                //                                          //There is not a clone.
                (etentityNewProcessType == null)
                )
            {
                //                                          //Find generic process type
                EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                    et.strXJDFTypeId == etentityBaseProcessType.strXJDFTypeId &&
                    et.strResOrPro == EtElementTypeAbstract.strProcess &&
                    et.intPrintshopPk == null);

                if (
                    etentity != null
                    )
                {
                    ProtypProcessType protyp = new ProtypProcessType(etentity.intPk, etentity.strXJDFTypeId,
                        etentity.strAddedBy, etentity.intPrintshopPk, etentity.strCustomTypeId,
                        etentity.strClassification);

                    if (
                        //                                  //The type is pass as ref, because it can be 
                        //                                  //      changed from the XJDF to the clone.
                        ValidateProcessType.boolIsValidType(ps_I, context_M, ref protyp)
                        )
                    {
                        if (
                            protyp.intPkPrintshop == ps_I.intPk
                            )
                        {
                            intPkNewProcessType_IO = protyp.intPk;
                            intStatus = 200;
                        }
                    }
                }
                else
                //                                          //It is a custom type
                {
                    //                                      //Verify if the custom type for the printshop exists.
                    EtentityElementTypeEntityDB etentityCustom = context_M.ElementType.FirstOrDefault(et =>
                        et.strCustomTypeId == ProtypProcessType.strProCustomType && 
                        et.intPrintshopPk == ps_I.intPk);

                    if (
                        etentityCustom != null
                        )
                    {
                        //                                  //Do nothing
                    }
                    else
                    {
                        //                                  //Add CustomType to printshop
                        //                                  //Create the entity
                        etentityCustom = new EtentityElementTypeEntityDB
                        {
                            strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                            strResOrPro = EtElementTypeAbstract.strProcess,
                            intPrintshopPk = ps_I.intPk,
                            strAddedBy = ps_I.strPrintshopId,
                            strCustomTypeId = ProtypProcessType.strProCustomType
                        };

                        context_M.ElementType.Add(etentityCustom);
                        context_M.SaveChanges();
                    }

                    intPkNewProcessType_IO = etentityCustom.intPk;
                    intStatus = 200;
                }
            }
            else
            {
                intPkNewProcessType_IO = etentityNewProcessType.intPk;
                intStatus = 200;
            }

            if (
                !(intStatus == 200)
                )
            {
                throw new CustomException("The process type could not be added.");
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddTypeToGenericProcess(

            EleetentityElementElementTypeEntityDB eleetentityBase_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            out int intPkTypeNew_O
            )
        {
            intPkTypeNew_O = 0;

            RestypResourceType.subAddResourceType(eleetentityBase_I.intPkElementTypeSon, ps_I, context_M,
                ref intPkTypeNew_O);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddTemplateToGenericProcess(

            EleeleentityElementElementEntityDB eleeleentityBase_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            out int intPkTemplateNew_O,
            ref Dictionary<int, int> dicResource_M
            )
        {
            intPkTemplateNew_O = 0;
            String strTemplateName;
            //                                              //It is a template.
            ResResource resBaseTemplate = ResResource.resFromDB(context_M, eleeleentityBase_I.intPkElementSon, true);

            if (
                resBaseTemplate != null
                )
            {
                //                                          //Get the base template from DB.
                EleentityElementEntityDB eleentityBaseTemplate = context_M.Element.FirstOrDefault(eleentity =>
                    eleentity.intPk == resBaseTemplate.intPk);

                int intPkTemplateTypeNew = 0;
                RestypResourceType.subAddResourceType(eleentityBaseTemplate.intPkElementType, ps_I, context_M, 
                    ref intPkTemplateTypeNew);

                strTemplateName = resBaseTemplate.strName + " (Generic)";
                //                                          //Find new template
                EleentityElementEntityDB eleentityNewTemplate = context_M.Element.FirstOrDefault(ele =>
                    ele.strElementName == strTemplateName &&
                    ele.intPkElementType == intPkTemplateTypeNew);

                if (
                    //                                      //There is already a template with the same data.
                    eleentityNewTemplate != null
                    )
                {
                    //                                      //Add relationship between the base and the new resource 
                    //                                      //      to the dictionary
                    dicResource_M.Add(eleentityBaseTemplate.intPk, eleentityNewTemplate.intPk);
                }
                else
                {
                    ResResource.subAddGenericResource(intPkTemplateTypeNew, strTemplateName, eleentityBaseTemplate,
                        context_M, out eleentityNewTemplate, ref dicResource_M);

                    //                                  //Add template
                    /*eleentityNewTemplate = new EleentityElementEntityDB
                    {
                        strElementName = strTemplateName,
                        intPkElementType = intPkTemplateTypeNew,
                        //                              //Will be updated later
                        intnPkElementInherited = null,
                        intnPkElementCalendarInherited = null,
                        boolIsTemplate = true,
                        boolnIsCalendar = resBaseTemplate.boolnIsCalendar,
                        boolnIsAvailable = true,
                        boolDeleted = false,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        boolnCalendarIsChangeable = resBaseTemplate.boolnCalendarIsChangeable
                    };

                    context_M.Add(eleentityNewTemplate);
                    context_M.SaveChanges();*/
                }

                intPkTemplateNew_O = eleentityNewTemplate.intPk;
            }
            else
            {
                throw new CustomException("No template found.");
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static AttrAttribute[] darrattrGetGeneralAttribute(
            //                                              //Return an array of common attr for XJDF processes.
            )
        {
            //                                              //Get the general type.
            Odyssey2Context context = new Odyssey2Context();
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(etentity =>
                etentity.strXJDFTypeId == ProtypProcessType.strGeneralTypeId &&
                etentity.intPrintshopPk == null);

            int intTypePk = etentity.intPk;
            List<AttrAttribute> darrattr = new List<AttrAttribute>();
            if (
                //                                          //Type found.
                etentity != null
                )
            {
                //                                          //Get the relations for general attributes.
                IQueryable<AttretentityAttributeElementTypeEntityDB> setattrentity = context.
                AttributeElementType.Where(attretentity =>
                attretentity.intPkElementType == intTypePk);

                List<int> darrint = new List<int>();
                foreach (AttretentityAttributeElementTypeEntityDB attretentity in setattrentity)
                {
                    darrint.Add((int)attretentity.intPkAttribute);
                }

                //                                      //Get the name for every attribute associated with the general 
                //                                      //      type.
                foreach (int intPk in darrint)
                {
                    AttrentityAttributeEntityDB attrentity = context.Attribute.FirstOrDefault(attr =>
                    attr.intPk == intPk);

                    //                                      //Create the attribute.
                    AttrAttribute attr = new AttrAttribute(attrentity.intPk, attrentity.strCustomName,
                        attrentity.strXJDFName, attrentity.strCardinality, attrentity.strDatatype,
                        attrentity.strDescription, attrentity.strScope, attrentity.intWebsiteAttributeId, 
                        null);
                    darrattr.Add(attr);
                }
            }

            return darrattr.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
