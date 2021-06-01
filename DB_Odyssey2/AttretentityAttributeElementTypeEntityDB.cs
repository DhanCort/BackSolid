/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("AttributeElementType")]
    //==================================================================================================================
    public class AttretentityAttributeElementTypeEntityDB
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
        [Column("PkAttribute", TypeName = "int")]
        public int? intPkAttribute { get; set; }
        [ForeignKey("intPkAttribute")]
        public AttrentityAttributeEntityDB PkAttribute { get; set; }

        [Required]
        [Column("PkElementType", TypeName = "int")]
        public int? intPkElementType { get; set; }
        [ForeignKey("intPkElementType")]
        public EtentityElementTypeEntityDB PkElementType { get; set; }
        
        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
