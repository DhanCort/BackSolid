/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 29, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("AccountMovement")]
    //==================================================================================================================
    public class AccmoventityAccountMovementEntityDB
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
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Required]
        [Column("Concept", TypeName = "nvarchar(200)")]
        public String strConcept { get; set; }

        [Column("JobId", TypeName = "int")]
        public int? intnJobId { get; set; }

        [Column("Increase", TypeName = "float")]
        public double? numnIncrease { get; set; }

        [Column("Decrease", TypeName = "float")]
        public double? numnDecrease { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkAccount", TypeName = "int")]
        public int intPkAccount { get; set; }
        [ForeignKey("intPkAccount")]
        public AccentityAccountEntityDB PkAccount { get; set; }

        [Column("PkInvoice", TypeName = "int")]
        public int? intnPkInvoice { get; set; }
        [ForeignKey("intnPkInvoice")]
        public InvoInvoiceEntityDB PkInvoice { get; set; }

        [Column("PkCreditMemo", TypeName = "int")]
        public int? intnPkCreditMemo { get; set; }
        [ForeignKey("intnPkCreditMemo")]
        public CrmentityCreditMemoEntityDB PkCreditMemo { get; set; }

        [Column("PkBankDeposit", TypeName = "int")]
        public int? intnPkBankDeposit { get; set; }
        [ForeignKey("intnPkBankDeposit")]
        public BkdptentityBankDepositEntityDB PkBankDeposit { get; set; }

        [Column("PkPayment", TypeName = "int")]
        public int? intnPkPayment { get; set; }
        [ForeignKey("intnPkPayment")]
        public PaymtPaymentEntityDB PkPayment { get; set; }
        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/