/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: July 17, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Estimate")]
    //==================================================================================================================
    public class EstentityEstimateEntityDB
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
        [Column("Id", TypeName = "int")]
        public int intId { get; set; }

        [Column("CopyNumber", TypeName = "int")]
        public int? intnCopyNumber { get; set; }

        [Required]
        [Column("JobId", TypeName = "int")]
        public int intJobId { get; set; }

        [Required]
        [Column("BaseDate", TypeName = "nvarchar(10)")]
        public String strBaseDate { get; set; }

        [Required]
        [Column("BaseTime", TypeName = "nvarchar(10)")]
        public String strBaseTime { get; set; }

        [Column("Name", TypeName = "nvarchar(30)")]
        public String strName { get; set; }

        [Column("Quantity", TypeName = "int")]
        public int? intnQuantity { get; set; }

        [Column("LastPrice", TypeName = "float")]
        public double? numnLastPrice { get; set; }

        [Required]
        [Column("SentToWebSite", TypeName = "bit")]
        public bool boolSentToWebSite { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

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
