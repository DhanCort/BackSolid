/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CCC-Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: October 01, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("LinkNode")]
    //==================================================================================================================
    public class LinknodLinkNodeEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/

        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkWorkflow", TypeName = "int")]
        public int intPkWorkflow { get; set; }
        [ForeignKey("intPkWorkflow")]
        public WfentityWorkflowEntityDB PkWorkflow { get; set; }

        [Required]
        [Column("PkNodeI", TypeName = "int")]
        public int intPkNodeI { get; set; }
        [ForeignKey("intPkNodeI")]
        public IoentityInputsAndOutputsEntityDB PkNodeI { get; set; }

        [Required]
        [Column("PkNodeO", TypeName = "int")]
        public int intPkNodeO { get; set; }
        [ForeignKey("intPkNodeO")]
        public IoentityInputsAndOutputsEntityDB PkNodeO { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("ConditionToApply", TypeName = "nvarchar(500)")]
        public String strConditionToApply { get; set; }

        [Required]
        [Column("ConditionAnd", TypeName = "bit")]
        public bool boolConditionAnd { get; set; }

        [Column("ConditionQuantity", TypeName = "nvarchar(500)")]
        public String strConditionQuantity { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
