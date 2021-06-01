/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 12, 2021.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Condition")]
    //==================================================================================================================
    public class CondentityConditionEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/

        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Column("PkAttribute", TypeName = "int")]
        public int? intnPkAttribute { get; set; }
        [ForeignKey("intnPkAttribute")]
        public AttrentityAttributeEntityDB PkAttribute { get; set; }

        [Column("PkGroupCondition", TypeName = "int")]
        public int intPkGroupCondition { get; set; }
        [ForeignKey("intPkGroupCondition")]
        public GpcondentityGroupConditionEntityDB PkGroupCondition { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Required]
        [Column("Condition")]
        public String strCondition { get; set; }

        [Required]
        [Column("Value")]
        public String strValue { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
