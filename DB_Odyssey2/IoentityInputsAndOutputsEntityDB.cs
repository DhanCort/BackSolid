/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: February 24, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("InputsAndOutputs")]
    //==================================================================================================================
    public class IoentityInputsAndOutputsEntityDB : IComparable
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

        [Column("PkElementElementType", TypeName = "int")]
        public int? intnPkElementElementType { get; set; }
        [ForeignKey("intnPkElementElementType")]
        public EleetentityElementElementTypeEntityDB PkElementElementType { get; set; }

        [Column("PkElementElement", TypeName = "int")]
        public int? intnPkElementElement { get; set; }
        [ForeignKey("intnPkElementElement")]
        public EleeleentityElementElementEntityDB PkElementElement { get; set; }

        [Column("PkResource", TypeName = "int")]
        public int? intnPkResource { get; set; }
        [ForeignKey("intnPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("Link", TypeName = "nvarchar(100)")]
        public String strLink { get; set; }

        [Column("GroupResourceId", TypeName = "int")]
        public int? intnGroupResourceId { get; set; }

        [Column("IsFinalProduct", TypeName = "bit")]
        public bool? boolnIsFinalProduct { get; set; }

        [Column("ProcessInWorkflowId", TypeName = "int")]
        public int? intnProcessInWorkflowId { get; set; }

        [Column("ConditionToApply", TypeName = "nvarchar(500)")]
        public String strConditionToApply { get; set; }

        [Required]
        [Column("ConditionAnd", TypeName = "bit")]
        public bool boolConditionAnd { get; set; }

        [Column("ConditionQuantity", TypeName = "nvarchar(500)")]
        public String strConditionQuantity { get; set; }

        [Column("Size", TypeName = "bit")]
        public bool? boolnSize { get; set; }
        [Column("Thickness", TypeName = "bit")]
        public bool? boolnThickness { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
             object obj_I
             )
        {
            IoentityInputsAndOutputsEntityDB ioentity = (IoentityInputsAndOutputsEntityDB)obj_I;

            return this.intPk.CompareTo(ioentity.intPk);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
