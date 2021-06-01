/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Printshop")]
    //==================================================================================================================
    public class PsentityPrintshopEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*UNIQUE KEY*/
        [Required]
        [Column("PrintshopId")]
        public String strPrintshopId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Required]
        [Column("Name")]
        public String strName { get; set; }

        [Required]
        [Column("SpecialPassword")]
        public String strSpecialPassword { get; set; }

        [Column("Url")]
        public String strUrl { get; set; }

        [Required]
        [Column("OffsetNumber", TypeName = "bit")]
        public bool boolOffsetNumber { get; set; }

        [Column("TimeZone", TypeName = "nvarchar(40)")]
        public String strTimeZone { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
