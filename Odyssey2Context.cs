/*TASK RP.Odyssey2Context Relevant Part Odyssey2 Context*/
using Microsoft.EntityFrameworkCore;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using System;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend
{
    //==================================================================================================================
    public class Odyssey2Context : DbContext
    {
        //--------------------------------------------------------------------------------------------------------------

        public DbSet<AttrentityAttributeEntityDB> Attribute { get; set; }
        public DbSet<PsentityPrintshopEntityDB> Printshop { get; set; }
        public DbSet<EtentityElementTypeEntityDB> ElementType { get; set; }
        public DbSet<CalentityCalculationEntityDB> Calculation { get; set; }
        public DbSet<EtetentityElementTypeElementTypeEntityDB> ElementTypeElementType { get; set; }
        public DbSet<EleetentityElementElementTypeEntityDB> ElementElementType { get; set; }
        public DbSet<AttretentityAttributeElementTypeEntityDB> AttributeElementType { get; set; }
        public DbSet<EnumentityEnumerationEntityDB> Enumeration { get; set; }
        public DbSet<GpcalentityGroupCalculationEntityDB> GroupCalculation { get; set; }
        public DbSet<ValentityValueEntityDB> Value { get; set; }
        public DbSet<EleentityElementEntityDB> Element { get; set; }
        public DbSet<RcentityResourceClassification> ResourceClassification { get; set; }
        public DbSet<AscentityAscendantsEntityDB> Ascendants { get; set; }
        public DbSet<EleeleentityElementElementEntityDB> ElementElement { get; set; }
        public DbSet<PiwentityProcessInWorkflowEntityDB> ProcessInWorkflow { get; set; }
        public DbSet<IoentityInputsAndOutputsEntityDB> InputsAndOutputs { get; set; }
        public DbSet<IojentityInputsAndOutputsForAJobEntityDB> InputsAndOutputsForAJob { get; set; }
        public DbSet<CostentityCostEntityDB> Cost { get; set; }        
        public DbSet<GpresentityGroupResourceEntityDB> GroupResource { get; set; }
        public DbSet<PerentityPeriodEntityDB> Period { get; set; }
        public DbSet<RuleentityRuleEntityDB> Rule { get; set; }
        public DbSet<TimeentityTimeEntityDB> Time { get; set; }
        public DbSet<JobentityJobEntityDB> Job { get; set; }
        public DbSet<PiwjentityProcessInWorkflowForAJobEntityDB> ProcessInWorkflowForAJob { get; set; }
        public DbSet<WfentityWorkflowEntityDB> Workflow { get; set; }
        public DbSet<AdminentityAdministratorEntityDB> Administrator { get; set; }
        public DbSet<FnlcostentityFinalCostEntityDB> FinalCost { get; set; }
        public DbSet<EstdataentityEstimationDataEntityDB> EstimationData { get; set; }
        public DbSet<DuedateentityDueDateEntityDB> DueDate { get; set; }
        public DbSet<PriceentityPriceEntityDB> Price { get; set; }
        public DbSet<AlerttypeentityAlertTypeEntityDB> AlertType { get; set; }
        public DbSet<AlertentityAlertEntityDB> Alert { get; set; }
        public DbSet<EstentityEstimateEntityDB> Estimate { get; set; }
        public DbSet<TrfcalentityTransformCalculationEntityDB> TransformCalculation { get; set; }
        public DbSet<TaskentityTaskEntityDB> Task { get; set; }
        public DbSet<RolentityRoleEntityDB> Role { get; set; }
        public DbSet<LinknodLinkNodeEntityDB> LinkNode { get; set; }
        public DbSet<PatransPaperTransformationEntityDB> PaperTransformation { get; set; }
        public DbSet<JobjsonentityJobJsonEntityDB> JobJson { get; set; }
        public DbSet<CusrepentityCustomResportEntityDB> CustomReport { get; set; }
        public DbSet<AccentityAccountEntityDB> Account { get; set; }
        public DbSet<AccmoventityAccountMovementEntityDB> AccountMovement { get; set; }
        public DbSet<AcctypentityAccountTypeEntityDB> AccountType { get; set; }
        public DbSet<InvoInvoiceEntityDB> Invoice { get; set; }
        public DbSet<TaxentityTaxesEntityDB> Taxes { get; set; }
        public DbSet<PaymtPaymentEntityDB> Payment { get; set; }
        public DbSet<CustbalentityCustomerBalanceEntityDB> CustomerBalance { get; set; }
        public DbSet<BkdptentityBankDepositEntityDB> BankDeposit { get; set; }
        public DbSet<CrmentityCreditMemoEntityDB> CreditMemo { get; set; }
        public DbSet<PymtmtentityPaymentMethodEntityDB> PaymentMethod { get; set; }
        public DbSet<AplpayentityApliedPaymentsEntityDB> ApliedPayment { get; set; }
        public DbSet<JobnotesJobNotesEntityDB> JobNotes { get; set; }
        public DbSet<PronotesentityProcessNotesEntityDB> ProcessNotes { get; set; }
        public DbSet<DefwfhisentityDefaultWorkflowHistoryEntityDB> DefaultWorkflowHistory { get; set; }
        public DbSet<CondentityConditionEntityDB> Condition { get; set; }
        public DbSet<GpcondentityGroupConditionEntityDB> GroupCondition { get; set; }
        public DbSet<ColcondentityCalculationOrLinkConditionEntityDB> CalculationOrLinkCondition { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //String strConnection = "Server=DESKTOP-ANEEUI8;Database=Odyssey2DB;User=DBUser;Password=DBUser2019";
            //String strConnection = "Server=DESKTOP-41KEAQ3;Database=Odyssey2DB;User=DBUser;Password=DBUser2019";
            //String strConnection = "Server=DESKTOP-URFTLE9;Database=Odyssey2DB;User=DBUser;Password=DBUser2019";
            //String strConnection = "Server=DESKTOP-V7N9AKM;Database=Odyssey2DB;User=DBUser;Password=DBUser2020";
            //String strConnection = "Server=DESKTOP-K1ISDPO;Database=Odyssey2DB;User=DBUser;Password=DBUser2020";
            //String strConnection = "Server=DESKTOP-77T6QPB;Database=Odyssey2DB;User=DBUser;Password=DBUser2020";
            //String strConnection = "Server=DESKTOP-24FK2EH;Database=Odyssey2DB;User=DBUser;Password=DBUser2019";
            //String strConnection = "Server=DESKTOP-H8G3C5J;Database=Odyssey2DB;User=DBUser;Password=DBUser2020";
            //String strConnection = "Server=TOWAAPP01;Database=Odyssey2DB;User=DBUser;Password=DBUser2019";
            String strConnection = "Server=MI4P-Odyssey;Database=Odyssey2DB;User=DBUser;Password=DBUser2020";

            optionsBuilder.UseSqlServer(strConnection);
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder   
            )
        {
            //                                              //ElementType Constraints.            
            modelBuilder.Entity<EtentityElementTypeEntityDB>()
                .HasIndex(etentity => new
                {
                    etentity.strCustomTypeId,
                    etentity.intPrintshopPk,
                    etentity.intWebsiteProductKey
                }).IsUnique();

            modelBuilder.Entity<EtentityElementTypeEntityDB>()
                .HasIndex(etentity => new
                {
                    etentity.strCustomTypeId,
                    etentity.intPrintshopPk
                });

            modelBuilder.Entity<EtentityElementTypeEntityDB>()
                .HasIndex(etentity => new
                {
                    etentity.intWebsiteProductKey
                });

            modelBuilder.Entity<EtentityElementTypeEntityDB>()
                .HasOne(etentity => etentity.PkPrintshopId).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EtentityElementTypeEntityDB>()
                .HasOne(etentity => etentity.PkAccount).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Attribute Constraints.
            modelBuilder.Entity<AttrentityAttributeEntityDB>()
                .HasIndex(jobentity => jobentity.strCustomName);

            //                                              //Ascedandants Constraints.
            modelBuilder.Entity<AscentityAscendantsEntityDB>()
                .HasOne(a => a.pkElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Printshop Constraints.
            modelBuilder.Entity<PsentityPrintshopEntityDB>()
                .HasIndex(psentity => psentity.strPrintshopId).IsUnique();

            //                                              //Calculation Constraints.
            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkProcess).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkProduct).WithMany()
                .OnDelete(DeleteBehavior.Restrict);        

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkElementElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkElementElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkQFromResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkQFromElementElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkQFromElementElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CalentityCalculationEntityDB>()
                .HasOne(calentity => calentity.PkAccount).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Cost Constraints.
            modelBuilder.Entity<CostentityCostEntityDB>()
                .HasOne(cost => cost.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CostentityCostEntityDB>()
                .HasOne(cost => cost.PkCostInherited).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CostentityCostEntityDB>()
                .HasOne(cost => cost.PkAccount).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //AttributeElementType Constraints.
            modelBuilder.Entity<AttretentityAttributeElementTypeEntityDB>()
                .HasOne(attretentity => attretentity.PkAttribute).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttretentityAttributeElementTypeEntityDB>()
                .HasOne(attretentity => attretentity.PkElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //ElementTypeElementType Constraints.
            modelBuilder.Entity<EtetentityElementTypeElementTypeEntityDB>()
                .HasOne(etetentity => etetentity.PkElementTypeDad).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EtetentityElementTypeElementTypeEntityDB>()
                .HasOne(etetentity => etetentity.PkElementTypeSon).WithMany()
                .OnDelete(DeleteBehavior.Restrict);
 
            //                                              //Element Constraints.
            modelBuilder.Entity<EleentityElementEntityDB>()
                .HasOne(eleentity => eleentity.PkElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);           

            modelBuilder.Entity<EleentityElementEntityDB>()
                .HasOne(e => e.PkElementInherited).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EleentityElementEntityDB>()
                .HasOne(e => e.PkElementCalendarInherited).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Value Constraints.
            modelBuilder.Entity<ValentityValueEntityDB>()
                .HasOne(b => b.PkElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ValentityValueEntityDB>()
                .HasOne(b => b.PkAttribute).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ValentityValueEntityDB>()
                .HasOne(b => b.PkValueInherited).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //ProcessInWorkflow Constraints.
            modelBuilder.Entity<PiwentityProcessInWorkflowEntityDB>()
                .HasIndex(piwentity => new
                {
                    piwentity.intPkWorkflow,
                    piwentity.intProcessInWorkflowId
                }).IsUnique();

            modelBuilder.Entity<PiwentityProcessInWorkflowEntityDB>()
                .HasOne(b => b.PkProcess).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PiwentityProcessInWorkflowEntityDB>()
                .HasOne(b => b.PkWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);            

            //                                              //IoInputsAndOutputs Constraints.
            modelBuilder.Entity<IoentityInputsAndOutputsEntityDB>()
                .HasOne(b => b.PkWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IoentityInputsAndOutputsEntityDB>()
                .HasOne(b => b.PkElementElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IoentityInputsAndOutputsEntityDB>()
                .HasOne(b => b.PkElementElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IoentityInputsAndOutputsEntityDB>()
                .HasOne(b => b.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //IoInputsAndOutputsForAJob Constraints.
            modelBuilder.Entity<IojentityInputsAndOutputsForAJobEntityDB>()
                .HasOne(b => b.PkProcessInWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IojentityInputsAndOutputsForAJobEntityDB>()
                .HasOne(b => b.PkElementElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IojentityInputsAndOutputsForAJobEntityDB>()
                .HasOne(b => b.PkElementElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IojentityInputsAndOutputsForAJobEntityDB>()
                .HasOne(b => b.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //ElementElement Constraints.
            modelBuilder.Entity<EleeleentityElementElementEntityDB>()
                .HasOne(b => b.PkElementDad).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EleeleentityElementElementEntityDB>()
                .HasOne(b => b.PkElementSon).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //ElementElementType Constraints.
            modelBuilder.Entity<EleetentityElementElementTypeEntityDB>()
                .HasOne(eleet => eleet.PkElementDad).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EleetentityElementElementTypeEntityDB>()
                .HasOne(eleet => eleet.PkElementTypeSon).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //GroupResources Constraints.
            //modelBuilder.Entity<GpresentityGroupResourceEntityDB>()
            //    .HasIndex(etentity => new
            //    {
            //        etentity.PkResource,
            //        etentity.PkWorkflow,
            //        etentity.intId
            //    }).IsUnique();

            modelBuilder.Entity<GpresentityGroupResourceEntityDB>()
                .HasOne(gpres => gpres.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GpresentityGroupResourceEntityDB>()
                .HasOne(gpres => gpres.PkWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //GroupCalculation Constraints.
            modelBuilder.Entity<GpcalentityGroupCalculationEntityDB>()
                .HasOne(gpcal => gpcal.PkCalculation).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Period Constraints.
            modelBuilder.Entity<PerentityPeriodEntityDB>()
                .HasOne(p => p.PkWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IoentityInputsAndOutputsEntityDB>()
                .HasOne(b => b.PkElementElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IoentityInputsAndOutputsEntityDB>()
                .HasOne(b => b.PkElementElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PerentityPeriodEntityDB>()
                .HasOne(p => p.PkElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PerentityPeriodEntityDB>()
                .HasOne(p => p.PkCalculation).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Rule Constraints.          
            modelBuilder.Entity<RuleentityRuleEntityDB>()
                .HasOne(r => r.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RuleentityRuleEntityDB>()
                .HasOne(r => r.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Time Constraints.          
            modelBuilder.Entity<TimeentityTimeEntityDB>()
                .HasOne(t => t.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Job Constraints.
            modelBuilder.Entity<JobentityJobEntityDB>()
                .HasOne(job => job.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //PIWJ Constraints.  
            modelBuilder.Entity<PiwjentityProcessInWorkflowForAJobEntityDB>()
                .HasOne(piwj => piwj.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PiwjentityProcessInWorkflowForAJobEntityDB>()
                .HasOne(piwj => piwj.PkProcessInWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Workflow constraints.
            modelBuilder.Entity<WfentityWorkflowEntityDB>()
                .HasOne(wfentity => wfentity.PkProduct).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WfentityWorkflowEntityDB>()
                .HasOne(wfentity => wfentity.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //FinalCost constraints.
            modelBuilder.Entity<FnlcostentityFinalCostEntityDB>()
                .HasOne(final => final.PkJob).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FnlcostentityFinalCostEntityDB>()
                .HasOne(final => final.PkProcessInWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FnlcostentityFinalCostEntityDB>()
                .HasOne(final => final.PkElementElementType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FnlcostentityFinalCostEntityDB>()
                .HasOne(final => final.PkElementElement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FnlcostentityFinalCostEntityDB>()
                .HasOne(final => final.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FnlcostentityFinalCostEntityDB>()
                .HasOne(final => final.PkCalculation).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FnlcostentityFinalCostEntityDB>()
                .HasOne(final => final.PkAccountMovement).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //EstimationData Constraints.
            modelBuilder.Entity<EstdataentityEstimationDataEntityDB>()
               .HasOne(etentity => etentity.PkResource).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EstdataentityEstimationDataEntityDB>()
                .HasOne(etentity => etentity.PkProcessInWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EstdataentityEstimationDataEntityDB>()
               .HasOne(etentity => etentity.PkElementElementType).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EstdataentityEstimationDataEntityDB>()
               .HasOne(etentity => etentity.PkElementElement).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //Alert constraints.
            modelBuilder.Entity<AlertentityAlertEntityDB>()
                .HasOne(alertentity => alertentity.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AlertentityAlertEntityDB>()
                .HasOne(alertentity => alertentity.PkAlertType).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AlertentityAlertEntityDB>()
                .HasOne(alertentity => alertentity.PkResource).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AlertentityAlertEntityDB>()
              .HasOne(alertentity => alertentity.PkPeriod).WithMany()
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AlertentityAlertEntityDB>()
               .HasOne(alertentity => alertentity.PkTask).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //Estimate Constraints.
            modelBuilder.Entity<EstentityEstimateEntityDB>()
               .HasOne(estentity => estentity.PkWorkflow).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //Price Constraints.
            modelBuilder.Entity<PriceentityPriceEntityDB>()
               .HasOne(price => price.PkWorkflow).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PriceentityPriceEntityDB>()
               .HasOne(price => price.PkEstimate).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //Transform Calculation Constraints.
            modelBuilder.Entity<TrfcalentityTransformCalculationEntityDB>()
                .HasOne(trfcalentity => trfcalentity.PkProcessInWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrfcalentityTransformCalculationEntityDB>()
                .HasOne(trfcalentity => trfcalentity.PkElementElementTypeI).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrfcalentityTransformCalculationEntityDB>()
                .HasOne(trfcalentity => trfcalentity.PkElementElementI).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrfcalentityTransformCalculationEntityDB>()
                .HasOne(trfcalentity => trfcalentity.PkResourceI).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrfcalentityTransformCalculationEntityDB>()
                .HasOne(trfcalentity => trfcalentity.PkElementElementTypeO).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrfcalentityTransformCalculationEntityDB>()
                .HasOne(trfcalentity => trfcalentity.PkElementElementO).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrfcalentityTransformCalculationEntityDB>()
                .HasOne(trfcalentity => trfcalentity.PkResourceO).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Task Constraints.
            modelBuilder.Entity<TaskentityTaskEntityDB>()
                .HasOne(task => task.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Supervisor Constraints.
            modelBuilder.Entity<RolentityRoleEntityDB>()
                .HasOne(role => role.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //LinkNode Constraints.
            modelBuilder.Entity<LinknodLinkNodeEntityDB>()
                .HasOne(node => node.PkWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LinknodLinkNodeEntityDB>()
                .HasOne(node => node.PkNodeI).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LinknodLinkNodeEntityDB>()
                .HasOne(node => node.PkNodeO).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Paper transformation Constraints.
            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
               .HasOne(patrans => patrans.PkCalculationOwn).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
               .HasOne(patrans => patrans.PkCalculationLink).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
                .HasOne(patrans => patrans.PkProcessInWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
                .HasOne(patrans => patrans.PkResourceI).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
                .HasOne(patrans => patrans.PkResourceO).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
                .HasOne(patrans => patrans.PkElementElementTypeI).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
                .HasOne(patrans => patrans.PkElementElementI).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
                .HasOne(patrans => patrans.PkElementElementTypeO).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatransPaperTransformationEntityDB>()
                .HasOne(patrans => patrans.PkElementElementO).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //CustomReport Constraints.  
            modelBuilder.Entity<CusrepentityCustomResportEntityDB>()
                .HasOne(cusrep => cusrep.PkPrintshop).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //Account Constraints.
            modelBuilder.Entity<AccentityAccountEntityDB>()
               .HasOne(account => account.PkPrintshop).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccentityAccountEntityDB>()
               .HasOne(account => account.PkAccountType).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //Account movement Constraints.
            modelBuilder.Entity<AccmoventityAccountMovementEntityDB>()
               .HasOne(accmov => accmov.PkAccount).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccmoventityAccountMovementEntityDB>()
               .HasOne(accmov => accmov.PkInvoice).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccmoventityAccountMovementEntityDB>()
               .HasOne(accmov => accmov.PkCreditMemo).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccmoventityAccountMovementEntityDB>()
               .HasOne(accmov => accmov.PkBankDeposit).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccmoventityAccountMovementEntityDB>()
               .HasOne(accmov => accmov.PkPayment).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //Invoice Constraints.
            modelBuilder.Entity<InvoInvoiceEntityDB>()
               .HasOne(invo => invo.PkPrintshop).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //Payment Constraints.
            modelBuilder.Entity<PaymtPaymentEntityDB>()
               .HasOne(payment => payment.PkPrintshop).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymtPaymentEntityDB>()
               .HasOne(payment => payment.PkPaymentMethod).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymtPaymentEntityDB>()
               .HasOne(payment => payment.PkBankDeposit).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //CustomerBalance Constraints.
            modelBuilder.Entity<CustbalentityCustomerBalanceEntityDB>()
               .HasOne(custbal => custbal.PkPrintshop).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //BankDeposit Constraints.
            modelBuilder.Entity<BkdptentityBankDepositEntityDB>()
               .HasOne(bkdpt => bkdpt.PkBankAccount).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //CreditMemo Constraints.
            modelBuilder.Entity<CrmentityCreditMemoEntityDB>()
               .HasOne(ctm => ctm.PkPrintshop).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //ApliedPayments Constraints.
            modelBuilder.Entity<AplpayentityApliedPaymentsEntityDB>()
               .HasOne(aplpay => aplpay.PkPayment).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AplpayentityApliedPaymentsEntityDB>()
               .HasOne(aplpay => aplpay.PkCreditMemo).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AplpayentityApliedPaymentsEntityDB>()
               .HasOne(aplpay => aplpay.PkInvoice).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //TaskNotes constraints.
            modelBuilder.Entity<PronotesentityProcessNotesEntityDB>()
                .HasOne(pronotes => pronotes.PkProcessInworkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //DefaultWorkflowHistory constraints.
            modelBuilder.Entity<DefwfhisentityDefaultWorkflowHistoryEntityDB>()
                .HasOne(defwfhis => defwfhis.PkWorkflow).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //                                              //JobJson constraints.
            modelBuilder.Entity<JobjsonentityJobJsonEntityDB>()
                .HasIndex(jobjson => jobjson.intJobID);

            //                                              //Condition Constraints.
            modelBuilder.Entity<CondentityConditionEntityDB>()
               .HasOne(condition => condition.PkAttribute).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CondentityConditionEntityDB>()
               .HasOne(condition => condition.PkGroupCondition).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //GroupCondition Constraints.
            modelBuilder.Entity<GpcondentityGroupConditionEntityDB>()
               .HasOne(condition => condition.PkGroupCondition).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            //                                              //CalculationOrLinkCondition Constraints.
            modelBuilder.Entity<ColcondentityCalculationOrLinkConditionEntityDB>()
               .HasOne(condition => condition.PkCalculation).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColcondentityCalculationOrLinkConditionEntityDB>()
               .HasOne(condition => condition.PkLinkNode).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColcondentityCalculationOrLinkConditionEntityDB>()
               .HasOne(condition => condition.PkInputOrOutput).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColcondentityCalculationOrLinkConditionEntityDB>()
               .HasOne(condition => condition.PkTransformCalculation).WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColcondentityCalculationOrLinkConditionEntityDB>()
               .HasOne(condition => condition.PkGroupCondition).WithMany()
               .OnDelete(DeleteBehavior.Restrict);
        }
    }

    //==================================================================================================================
}
