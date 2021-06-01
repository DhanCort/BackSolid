/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: December 10, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ApliedPayments")]
    //==================================================================================================================
    public class AplpayentityApliedPaymentsEntityDB
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
        [Column("Time", TypeName = "nvarchar(10)")]
        public String strTime { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkInvoice", TypeName = "int")]
        public int intPkInvoice { get; set; }
        [ForeignKey("intPkInvoice")]
        public InvoInvoiceEntityDB PkInvoice { get; set; }

        [Column("PkPayment", TypeName = "int")]
        public int? intnPkPayment { get; set; }
        [ForeignKey("intnPkPayment")]
        public PaymtPaymentEntityDB PkPayment { get; set; }

        [Column("PkCreditMemo", TypeName = "int")]
        public int? intnPkCreditMemo { get; set; }
        [ForeignKey("intnPkCreditMemo")]
        public CrmentityCreditMemoEntityDB PkCreditMemo { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/