/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using Odyssey2Backend.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowaStandard;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 24, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Period")]
    //==================================================================================================================
    public class PerentityPeriodEntityDB : IComparable
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
        [Column("EndDate", TypeName = "nvarchar(10)")]
        public String strEndDate { get; set; }

        [Required]
        [Column("EndTime", TypeName = "nvarchar(10)")]
        public String strEndTime { get; set; }

        [Required]
        [Column("JobId", TypeName = "int")]
        public int intJobId { get; set; }

        [Required]
        [Column("IsException", TypeName = "bit")]
        public bool? boolIsException { get; set; }

        [Column("ContactId", TypeName = "int")]
        public int? intnContactId { get; set; }

        [Column("FinalStartDate", TypeName = "nvarchar(10)")]
        public String strFinalStartDate { get; set; }

        [Column("FinalStartTime", TypeName = "nvarchar(10)")]
        public String strFinalStartTime { get; set; }

        [Column("FinalEndDate", TypeName = "nvarchar(10)")]
        public String strFinalEndDate { get; set; }

        [Column("FinalEndTime", TypeName = "nvarchar(10)")]
        public String strFinalEndTime { get; set; }

        [Column("EstimateId", TypeName = "int")]
        public int? intnEstimateId { get; set; }

        [Required]
        [Column("MinsBeforeDelete", TypeName = "int")]
        public int intMinsBeforeDelete { get; set; }

        [Required]
        [Column("DeleteDate", TypeName = "nvarchar(10)")]
        public String strDeleteDate { get; set; }

        [Required]
        [Column("DeleteHour", TypeName = "nvarchar(2)")]
        public String strDeleteHour { get; set; }

        [Required]
        [Column("DeleteMinute", TypeName = "nvarchar(2)")]
        public String strDeleteMinute { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkWorkflow", TypeName = "int")]
        public int intPkWorkflow { get; set; }
        [ForeignKey("intPkWorkflow")]
        public WfentityWorkflowEntityDB PkWorkflow { get; set; }

        [Required]
        [Column("ProcessInWorkflowId", TypeName = "int")]
        public int intProcessInWorkflowId { get; set; }

        [Column("PkElementElementType", TypeName = "int")]
        public int? intnPkElementElementType { get; set; }
        [ForeignKey("intnPkElementElementType")]
        public EleetentityElementElementTypeEntityDB PkElementElementType { get; set; }

        [Column("PkElementElement", TypeName = "int")]
        public int? intnPkElementElement { get; set; }
        [ForeignKey("intnPkElementElement")]
        public EleeleentityElementElementEntityDB PkElementElement { get; set; }

        [Required]
        [Column("PkElement", TypeName = "int")]
        public int intPkElement { get; set; }
        [ForeignKey("intPkElement")]
        public EleentityElementEntityDB PkElement { get; set; }

        [Column("PkCalculation", TypeName = "int")]
        public int? intnPkCalculation { get; set; }
        [ForeignKey("intnPkCalculation")]
        public CalentityCalculationEntityDB PkCalculation { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            PerentityPeriodEntityDB per = (PerentityPeriodEntityDB)obj_I;

            //                                              //To easy code.
            Date dateStartThis = this.strStartDate.ParseToDate();
            Date dateEndThis = this.strEndDate.ParseToDate();
            Time timeStartThis = this.strStartTime.ParseToTime();
            Time timeEndThis = this.strEndTime.ParseToTime();
            ZonedTime ztimeStartThis = ZonedTimeTools.NewZonedTime(dateStartThis, timeStartThis);
            ZonedTime ztimeEndThis = ZonedTimeTools.NewZonedTime(dateEndThis, timeEndThis);

            Date dateStartToCompare = per.strStartDate.ParseToDate();
            Date dateEndToCompare = per.strEndDate.ParseToDate();
            Time timeStartToCompare = per.strStartTime.ParseToTime();
            Time timeEndToCompare = per.strEndTime.ParseToTime();
            ZonedTime ztimeStartToCompare = ZonedTimeTools.NewZonedTime(dateStartToCompare, timeStartToCompare);
            ZonedTime ztimeEndToCompare = ZonedTimeTools.NewZonedTime(dateEndToCompare, timeEndToCompare);

            int intToReturn = 0;
            /*CASE*/
            if (
                //                                          //This is after to dr.
                ztimeStartThis < ztimeStartToCompare
                )
            {
                intToReturn = -1;
            }
            else if (
                //                                          //This is after to dr.
                ztimeStartThis > ztimeStartToCompare
                )
            {
                intToReturn = 1;
            }
            else
            {

            }
            /*CASE*/

            return intToReturn;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
