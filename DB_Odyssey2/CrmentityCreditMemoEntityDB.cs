/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowaStandard;

//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 08, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("CreditMemo")]
    //==================================================================================================================
    public class CrmentityCreditMemoEntityDB : IComparable
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
        [Column("CreditMemoNumber", TypeName = "nvarchar(40)")]
        public String strCreditMemoNumber { get; set; }

        [Required]
        [Column("OriginalAmount", TypeName = "float")]
        public double numOriginalAmount { get; set; }

        [Required]
        [Column("Memo", TypeName = "nvarchar(4000)")]
        public String strMemo { get; set; }

        [Required]
        [Column("Date", TypeName = "nvarchar(10)")]
        public String strDate { get; set; }

        [Required]
        [Column("OpenBalance", TypeName = "float")]
        public double numOpenBalance { get; set; }

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

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            CrmentityCreditMemoEntityDB crmentity = (CrmentityCreditMemoEntityDB) obj_I;

            Date dateThis = this.strDate.ParseToDate();
            Date dateToCompare = crmentity.strDate.ParseToDate();

            return dateThis.CompareTo(dateToCompare);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
