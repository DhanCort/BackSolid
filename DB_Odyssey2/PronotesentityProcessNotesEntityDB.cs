/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 05, 2021.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ProcessNotes")]
    //==================================================================================================================
    public class PronotesentityProcessNotesEntityDB
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

        [Required]
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Required]
        [Column("ContactId", TypeName = "int")]
        public int intContactId { get; set; }

        [Required]
        [Column("Text", TypeName = "nvarchar(250)")]
        public String strText { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Required]
        [Column("PkProcessInworkflow", TypeName = "int")]
        public int intPkProcessInworkflow { get; set; }
        [ForeignKey("intPkProcessInworkflow")]
        public PiwentityProcessInWorkflowEntityDB PkProcessInworkflow { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
