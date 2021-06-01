/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 29, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("AccountType")]
    //==================================================================================================================
    public class AcctypentityAccountTypeEntityDB
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
        [Column("Type", TypeName = "nvarchar(15)")]
        public String strType { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/