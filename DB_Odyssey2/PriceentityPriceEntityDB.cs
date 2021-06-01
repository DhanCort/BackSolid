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
    [Table("Price")]
    //==================================================================================================================
    public class PriceentityPriceEntityDB : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("Price", TypeName = "float")]
        public double? numnPrice { get; set; }

        [Required]
        [Column("JobId", TypeName = "int")]
        public int intJobId { get; set; }

        [Required]
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Required]
        [Column("Description", TypeName = "nvarchar(200)")]
        public String strDescription { get; set; }

        [Required]
        [Column("ContactId", TypeName = "int")]
        public int intContactId { get; set; }

        [Required]
        [Column("IsReset", TypeName = "bit")]
        public bool boolIsReset { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Column("PkWorkflow", TypeName = "int")]
        public int? intnPkWorkflow { get; set; }
        [ForeignKey("intnPkWorkflow")]
        public WfentityWorkflowEntityDB PkWorkflow { get; set; }

        [Column("PkEstimate", TypeName = "int")]
        public int? intnPkEstimate { get; set; }
        [ForeignKey("intnPkEstimate")]
        public EstentityEstimateEntityDB PkEstimate { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            PriceentityPriceEntityDB priceentity = (PriceentityPriceEntityDB)obj_I;

            //                                              //To easy code.
            Date dateStartThis = this.strStartDate.ParseToDate();
            Time timeStartThis = this.strStartTime.ParseToTime();
            ZonedTime ztimeStartThis = ZonedTimeTools.NewZonedTime(dateStartThis, timeStartThis);

            Date dateStartToCompare = priceentity.strStartDate.ParseToDate();
            Time timeStartToCompare = priceentity.strStartTime.ParseToTime();
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
