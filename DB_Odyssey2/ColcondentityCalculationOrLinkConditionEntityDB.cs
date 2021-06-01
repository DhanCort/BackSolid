/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 12, 2021.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("CalculationOrLinkCondition")]
    //==================================================================================================================
    public class ColcondentityCalculationOrLinkConditionEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/

        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Column("PkCalculation", TypeName = "int")]
        public int? intnPkCalculation { get; set; }
        [ForeignKey("intnPkCalculation")]
        public CalentityCalculationEntityDB PkCalculation { get; set; }

        [Column("PkLinkNode", TypeName = "int")]
        public int? intnPkLinkNode { get; set; }
        [ForeignKey("intnPkLinkNode")]
        public LinknodLinkNodeEntityDB PkLinkNode { get; set; }

        [Column("PkInputOrOutput", TypeName = "int")]
        public int? intnPkInputOrOutput { get; set; }
        [ForeignKey("intnPkInputOrOutput")]
        public IoentityInputsAndOutputsEntityDB PkInputOrOutput { get; set; }

        [Column("PkTransformCalculation", TypeName = "int")]
        public int? intnPkTransformCalculation { get; set; }
        [ForeignKey("intnPkTransformCalculation")]
        public TrfcalentityTransformCalculationEntityDB PkTransformCalculation { get; set; }

        [Required]
        [Column("PkGroupCondition", TypeName = "int")]
        public int intPkGroupCondition { get; set; }
        [ForeignKey("intPkGroupCondition")]
        public GpcondentityGroupConditionEntityDB PkGroupCondition { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
