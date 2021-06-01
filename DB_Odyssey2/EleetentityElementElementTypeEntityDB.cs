/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa Constantino).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: March 27, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ElementElementType")]
    //==================================================================================================================

    public class EleetentityElementElementTypeEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/

        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        [Required]
        [Column("Usage", TypeName = "bit")]
        public bool boolUsage { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkElementDad", TypeName = "int")]
        public int intPkElementDad { get; set; }
        [ForeignKey("intPkElementDad")]
        public EleentityElementEntityDB PkElementDad { get; set; }

        [Required]
        [Column("PkElementTypeSon", TypeName = "int")]
        public int intPkElementTypeSon { get; set; }
        [ForeignKey("intPkElementTypeSon")]
        public EtentityElementTypeEntityDB PkElementTypeSon { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/
        
        [Required]
        [Column("Deleted", TypeName = "bit")]
        public bool boolDeleted { get; set; }

        [Column("Id", TypeName = "int")]
        public int? intnId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================

}
/*END-TASK*/
