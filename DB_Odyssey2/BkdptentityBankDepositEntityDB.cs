/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 08, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("BankDeposit")]
    //==================================================================================================================
    public class BkdptentityBankDepositEntityDB
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
        [Column("Date", TypeName = "nvarchar(10)")]
        public String strDate { get; set; }

        [Required]
        [Column("Amount", TypeName = "float")]
        public double numAmount { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Required]
        [Column("PkBankAccount", TypeName = "int")]
        public int intPkBankAccount { get; set; }
        [ForeignKey("intPkBankAccount")]
        public AccentityAccountEntityDB PkBankAccount { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
