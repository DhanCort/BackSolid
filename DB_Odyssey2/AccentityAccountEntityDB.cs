/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 29, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Account")]
    //==================================================================================================================
    public class AccentityAccountEntityDB
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
        [Column("Number", TypeName = "nvarchar(40)")]
        public String strNumber { get; set; }
       
        [Required]
        [Column("Name", TypeName = "nvarchar(50)")]
        public String strName { get; set; }

        [Required]
        [Column("Available", TypeName = "bit")]
        public bool boolAvailable { get; set; }

        [Required]
        [Column("Generic", TypeName = "bit")]
        public bool boolGeneric { get; set; }

        [Required]
        [Column("Balance", TypeName = "float")]
        public double numBalance { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkPrintshop", TypeName = "int")]
        public int intPkPrintshop { get; set; }
        [ForeignKey("intPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        [Required]
        [Column("PkAccountType", TypeName = "int")]
        public int intPkAccountType { get; set; }
        [ForeignKey("intPkAccountType")]
        public AcctypentityAccountTypeEntityDB PkAccountType { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/