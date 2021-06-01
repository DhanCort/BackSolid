/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Job")]
    //==================================================================================================================
    public class JobentityJobEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/
        [Required]
        [Column("JobId", TypeName = "int")]
        public int intJobID { get; set; }
       
        [Required]
        [Column("Stage", TypeName = "int")]
        public int intStage { get; set; }

        [Required]
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Column("EndDate", TypeName = "nvarchar(10)")]
        public String strEndDate { get; set; }

        [Column("EndTime", TypeName = "nvarchar(10)")]
        public String strEndTime { get; set; }

        [Required]
        [Column("Invoiced", TypeName = "bit")]
        public bool boolInvoiced { get; set; }

        [Column("OnWisnet", TypeName = "bit")]
        public bool? boolnOnWisnet { get; set; }

        [Required]
        [Column("Deleted", TypeName = "bit")]
        public bool boolDeleted { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkPrintshop", TypeName = "int")]
        public int intPkPrintshop { get; set; }
        [ForeignKey("intPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        [Required]
        [Column("PkWorkflow", TypeName = "int")]
        public int intPkWorkflow { get; set; }
        [ForeignKey("intPkWorkflow")]
        public WfentityWorkflowEntityDB PkWorkflow { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/