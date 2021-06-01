/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using Odyssey2Backend.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowaStandard;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: Mar 23, 2021.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("DefaultWorkflowHistory")]
    //==================================================================================================================
    public class DefwfhisentityDefaultWorkflowHistoryEntityDB : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/

        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkWorkflow", TypeName = "int")]
        public int intPkWorkflow { get; set; }
        [ForeignKey("intPkWorkflow")]
        public WfentityWorkflowEntityDB PkWorkflow { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Required]
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        //--------------------------------------------------------------------------------------------------------------        
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            DefwfhisentityDefaultWorkflowHistoryEntityDB defwfhisentity = (DefwfhisentityDefaultWorkflowHistoryEntityDB)obj_I;

            ZonedTime ztime = ZonedTimeTools.NewZonedTime(this.strStartDate.ParseToDate(),
                this.strStartTime.ParseToTime());
            ZonedTime ztimeB = ZonedTimeTools.NewZonedTime(defwfhisentity.strStartDate.ParseToDate(),
                defwfhisentity.strStartTime.ParseToTime());

            return ztime.CompareTo(ztimeB);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
