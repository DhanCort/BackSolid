/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using Odyssey2Backend.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowaStandard;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 04, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("DueDate")]
    //==================================================================================================================
    public class DuedateentityDueDateEntityDB : IComparable
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
        public int intJobId { get; set; }

        [Required]
        [Column("Date", TypeName = "nvarchar(10)")]
        public String strDate { get; set; }

        [Required]
        [Column("Hour", TypeName = "nvarchar(2)")]
        public String strHour { get; set; }

        [Required]
        [Column("Minute", TypeName = "nvarchar(2)")]
        public String strMinute { get; set; }

        [Required]
        [Column("Second", TypeName = "nvarchar(2)")]
        public String strSecond { get; set; }

        //                                                  //This column is related to the DueDate log.
        [Required]
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        //                                                  //This column is related to the DueDate log.
        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Required]
        [Column("Description", TypeName = "nvarchar(200)")]
        public String strDescription { get; set; }

        [Required]
        [Column("ContactId", TypeName = "int")]
        public int intContactId { get; set; }

        //                                                  //This means that due date is the one being used or not.
        [Required]
        [Column("Active", TypeName = "bit")]
        public bool boolCurrent { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            DuedateentityDueDateEntityDB duedateentity = (DuedateentityDueDateEntityDB)obj_I;

            //                                              //To easy code.
            Date dateStartThis = this.strStartDate.ParseToDate();
            Time timeStartThis = this.strStartTime.ParseToTime();
            ZonedTime ztimeStartThis = ZonedTimeTools.NewZonedTime(dateStartThis, timeStartThis);

            Date dateStartToCompare = duedateentity.strStartDate.ParseToDate();
            Time timeStartToCompare = duedateentity.strStartTime.ParseToTime();
            ZonedTime ztimeStartToCompare = ZonedTimeTools.NewZonedTime(dateStartToCompare, timeStartToCompare);

            int intToReturn = 0;
            /*CASE*/
            if (
                //                                          //This is after to dr.
                ztimeStartThis < ztimeStartToCompare
                )
            {
                intToReturn = 1;
            }
            else if (
                //                                          //This is after to dr.
                ztimeStartThis > ztimeStartToCompare
                )
            {
                intToReturn = -1;
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
