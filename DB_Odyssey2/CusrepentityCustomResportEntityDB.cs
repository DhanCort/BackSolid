/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 27, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("CustomReport")]
    //==================================================================================================================
    public class CusrepentityCustomResportEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Column("PkPrintshop", TypeName = "int")]
        public int? intnPkPrintshop { get; set; }
        [ForeignKey("intnPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }
        
        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Required]
        [Column("Name", TypeName = "nvarchar(100)")]
        public String strName { get; set; }

        [Required]
        [Column("DataSet", TypeName = "nvarchar(9)")]
        public String strDataSet { get; set; }

        [Required]
        [Column("Filter", TypeName = "text")]
        public String strFilter { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
