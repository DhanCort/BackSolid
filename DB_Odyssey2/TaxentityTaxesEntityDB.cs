/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: November 79, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Taxes")]
    //==================================================================================================================
    public class TaxentityTaxesEntityDB
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
        [Column("ZipCode", TypeName = "nvarchar(5)")]
        public String strZipCode { get; set; }

        [Required]
        [Column("TaxValue", TypeName = "float")]
        public double numTaxValue { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
