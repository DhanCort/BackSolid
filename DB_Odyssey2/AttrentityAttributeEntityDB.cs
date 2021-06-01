/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Attribute")]
    //==================================================================================================================
    public class AttrentityAttributeEntityDB
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
        [Column("CustomName", TypeName = "nvarchar(100)")]
        public String strCustomName { get; set; }

        [Required]
        [Column("XJDFName", TypeName = "nvarchar(100)")]
        public String strXJDFName { get; set; }

        [Required]
        [Column("Cardinality", TypeName = "nvarchar(10)")]
        public String strCardinality { get; set; }

        [Required]
        [Column("Datatype", TypeName = "nvarchar(50)")]
        public String strDatatype { get; set; }

        [Column("EnumAssoc", TypeName = "nvarchar(100)")]
        public String strEnumAssoc { get; set; }

        [Required]
        [Column("Description", TypeName = "text")]
        public String strDescription { get; set; }

        [Required]
        [Column("Scope", TypeName = "nvarchar(10)")]
        public String strScope { get; set; }

        /*NULLABLE*/
        [Column("WebsiteAttributeId", TypeName = "int")]
        public int? intWebsiteAttributeId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/