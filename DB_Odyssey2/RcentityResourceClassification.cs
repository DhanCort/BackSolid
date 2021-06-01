/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: January 14, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ResourceClassification")]
    //==================================================================================================================
    public class RcentityResourceClassification
    {
        //--------------------------------------------------------------------------------------------------------------
        /*Primary key*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        [Required]
        [Column("Name", TypeName = "nvarchar(15)")]
        public String strName { get; set; }
        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
