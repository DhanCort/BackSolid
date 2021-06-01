/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CCC-Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: July 02, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("EstimationData")]
    //==================================================================================================================
    public class EstdataentityEstimationDataEntityDB
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

        [Required]
        [Column("JobId", TypeName = "int")]
        public int intJobId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkResource", TypeName = "int")]
        public int intPkResource { get; set; }
        [ForeignKey("intPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

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

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
