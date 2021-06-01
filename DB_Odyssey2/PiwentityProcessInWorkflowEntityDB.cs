/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: February 24, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ProcessInWorkflow")]
    //==================================================================================================================
    public class PiwentityProcessInWorkflowEntityDB : IComparable

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
        [Column("PkProcess", TypeName = "int")]
        public int intPkProcess { get; set; }
        [ForeignKey("intPkProcess")]
        public EleentityElementEntityDB PkProcess { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("Id", TypeName = "int")]
        public int? intnId { get; set; }

        [Required]
        [Column("ProcessInWorkflowId", TypeName = "int")]
        public int intProcessInWorkflowId { get; set; }

        [Required]
        [Column("IsPostProcess", TypeName = "bit")]
        public bool boolIsPostProcess { get; set; }

        //--------------------------------------------------------------------------------------------------------------

        public int CompareTo(
            object obj_I
            )
        {
            PiwentityProcessInWorkflowEntityDB piwentity = (PiwentityProcessInWorkflowEntityDB)obj_I;

            return this.intProcessInWorkflowId.CompareTo(piwentity.intProcessInWorkflowId);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
