/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 24, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Rule")]
    //==================================================================================================================
    public class RuleentityRuleEntityDB
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
        [Column("Frecuency", TypeName = "nvarchar(100)")]
        public String strFrecuency { get; set; }

        [Column("FrecuencyValue", TypeName = "nvarchar(31)")]
        public String strFrecuencyValue { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Required]
        [Column("EndTime", TypeName = "nvarchar(10)")]
        public String strEndTime { get; set; }

        [Column("RangeStartDate", TypeName = "nvarchar(10)")]
        public String strRangeStartDate { get; set; }

        [Column("RangeEndDate", TypeName = "nvarchar(10)")]
        public String strRangeEndDate { get; set; }

        [Column("RangeStartTime", TypeName = "nvarchar(10)")]
        public String strRangeStartTime { get; set; }

        [Column("RangeEndTime", TypeName = "nvarchar(10)")]
        public String strRangeEndTime { get; set; }

        [Column("ContactId", TypeName = "int")]
        public int? intnContactId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Column("PkResource", TypeName = "int")]
        public int? intnPkResource { get; set; }
        [ForeignKey("intnPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

        [Column("PkPrintshop", TypeName = "int")]
        public int? intnPkPrintshop { get; set; }
        [ForeignKey("intnPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        //--------------------------------------------------------------------------------------------------------------

    }

    //==================================================================================================================
}
/*END-TASK*/
