/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 11, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Invoice")]
    //==================================================================================================================
    public class InvoInvoiceEntityDB
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
        [Column("OrderNumber", TypeName = "int")]
        public int intOrderNumber { get; set; }

        [Required]
        [Column("InvoiceJson", TypeName = "nvarchar(4000)")]
        public String strInvoiceJson { get; set; }

        [Required]
        [Column("Amount", TypeName = "float")]
        public double numAmount { get; set; }

        [Required]
        [Column("OpenBalance", TypeName = "float")]
        public double numOpenBalance { get; set; }

        [Required]
        [Column("Balanced", TypeName = "bit")]
        public bool boolBalanced { get; set; }

        [Required]
        [Column("Date", TypeName = "nvarchar(10)")]
        public String strDate { get; set; }

        [Column("OnWisnet", TypeName = "bit")]
        public bool? boolnOnWisnet { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkPrintshop", TypeName = "int")]
        public int intPkPrintshop { get; set; }
        [ForeignKey("intPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/