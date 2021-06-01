/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: March 13, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("InputsAndOutputsForAJob")]
    //==================================================================================================================
    public class IojentityInputsAndOutputsForAJobEntityDB
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
        [Column("PkProcessInWorkflow", TypeName = "int")]
        public int intPkProcessInWorkflow { get; set; }
        [ForeignKey("intPkProcessInWorkflow")]
        public PiwentityProcessInWorkflowEntityDB PkProcessInWorkflow { get; set; }

        [Column("PkElementElementType", TypeName = "int")]
        public int? intnPkElementElementType { get; set; }
        [ForeignKey("intnPkElementElementType")]
        public EleetentityElementElementTypeEntityDB PkElementElementType { get; set; }

        [Column("PkElementElement", TypeName = "int")]
        public int? intnPkElementElement { get; set; }
        [ForeignKey("intnPkElementElement")]
        public EleeleentityElementElementEntityDB PkElementElement { get; set; }

        [Required]
        [Column("PkResource", TypeName = "int")]
        public int intPkResource { get; set; }
        [ForeignKey("intPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/
        [Required]
        [Column("JobId", TypeName = "int")]
        public int intJobId { get; set; }

        [Column("SetAutomatically", TypeName = "bit")]
        public bool? boolnWasSetAutomatically { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/