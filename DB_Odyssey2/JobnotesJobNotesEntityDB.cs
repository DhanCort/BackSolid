/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: January 05, 2021.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("JobNotes")]
    //==================================================================================================================
    public class JobnotesJobNotesEntityDB
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
        [Column("JobId", TypeName = "int")]
        public int intJobID { get; set; }

        [Column("ContactId", TypeName = "int")]
        public int? intnContactId { get; set; }

        [Column("Odyssey2Note", TypeName = "nvarchar(500)")]
        public String strOdyssey2Note { get; set; }

        [Column("WisnetNote", TypeName = "nvarchar(500)")]
        public String strWisnetNote { get; set; }

        [Column("Date", TypeName = "nvarchar(10)")]
        public String strDate { get; set; }

        [Column("Time", TypeName = "nvarchar(10)")]
        public String strTime { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
