/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowaStandard;
using Odyssey2Backend.Utilities;

//                                                          //AUTHOR: Towa (CCC-Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 27, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Payment")]
    //==================================================================================================================
    public class PaymtPaymentEntityDB : IComparable
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
        [Column("ContactId", TypeName = "int")]
        public int intContactId { get; set; }

        [Required]
        [Column("Date", TypeName = "nvarchar(10)")]
        public String strDate { get; set; }

        [Required]
        [Column("Amount", TypeName = "float")]
        public double numAmount { get; set; }

        [Required]
        [Column("OpenBalance", TypeName = "float")]
        public double numOpenBalance { get; set; }

        [Column("Reference", TypeName = "nvarchar(40)")]
        public String strReference { get; set; }

        [Required]
        [Column("Balanced", TypeName = "bit")]
        public bool boolBalanced { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkPrintshop", TypeName = "int")]
        public int intPkPrintshop { get; set; }
        [ForeignKey("intPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        [Required]
        [Column("PkPaymentMethod", TypeName = "int")]
        public int intPkPaymentMethod { get; set; }
        [ForeignKey("intPkPaymentMethod")]
        public PymtmtentityPaymentMethodEntityDB PkPaymentMethod { get; set; }

        [Column("PkBankDeposit", TypeName = "int")]
        public int? intnPkBankDeposit { get; set; }
        [ForeignKey("intnPkBankDeposit")]
        public BkdptentityBankDepositEntityDB PkBankDeposit { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            PaymtPaymentEntityDB paymtentity = (PaymtPaymentEntityDB) obj_I;

            //                                              //To easy code.
            Date dateThis = this.strDate.ParseToDate();

            Date dateToCompare = paymtentity.strDate.ParseToDate();

            return dateThis.CompareTo(dateToCompare);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/