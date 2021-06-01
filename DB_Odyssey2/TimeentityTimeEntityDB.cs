/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: May 04, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Time")]
    //==================================================================================================================
    public class TimeentityTimeEntityDB
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
        [Column("Quantity", TypeName = "float")]
        public double numQuantity { get; set; }

        [Required]
        [Column("Hours", TypeName = "int")]
        public int intHours { get; set; }

        [Required]
        [Column("Minutes", TypeName = "int")]
        public int intMinutes { get; set; }

        [Required]
        [Column("Seconds", TypeName = "int")]
        public int intSeconds { get; set; }

        [Required]
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Column("EndDate", TypeName = "nvarchar(10)")]
        public String strEndDate { get; set; }

        [Column("EndTime", TypeName = "nvarchar(10)")]
        public String strEndTime { get; set; }

        [Column("MinThickness", TypeName = "float")]
        public double? numnMinThickness { get; set; }

        [Column("MaxThickness", TypeName = "float")]
        public double? numnMaxThickness { get; set; }

        [Column("ThicknessUnit", TypeName = "nvarchar(10)")]
        public String strThicknessUnit { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Required]
        [Column("PkResource", TypeName = "int")]
        public int intPkResource { get; set; }
        [ForeignKey("intPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
