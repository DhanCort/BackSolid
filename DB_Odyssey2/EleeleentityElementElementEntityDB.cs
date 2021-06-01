/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: February 20, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ElementElement")]
    //==================================================================================================================
    public class EleeleentityElementElementEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        /*NULLABLE*/
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
        [Column("PkElementSon", TypeName = "int")]
        public int intPkElementSon { get; set; }
        [ForeignKey("intPkElementSon")]
        public EleentityElementEntityDB PkElementSon { get; set; }

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
