/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ElementTypeElementType")]
    //==================================================================================================================
    public class EtetentityElementTypeElementTypeEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        /*NULLABLE*/
        [Column("Usage", TypeName = "bit")]
        public bool? boolnUsage { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Required]
        [Column("PkElementTypeDad", TypeName = "int")]
        public int intPkElementTypeDad { get; set; }
        [ForeignKey("intPkElementTypeDad")]
        public EtentityElementTypeEntityDB PkElementTypeDad { get; set; }
        
        [Required]
        [Column("PkElementTypeSon", TypeName = "int")]
        public int intPkElementTypeSon { get; set; }
        [ForeignKey("intPkElementTypeSon")]
        public EtentityElementTypeEntityDB PkElementTypeSon { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
