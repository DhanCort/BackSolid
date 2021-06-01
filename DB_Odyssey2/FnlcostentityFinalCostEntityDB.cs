/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using Odyssey2Backend.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowaStandard;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: June 19, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("FinalCost")]
    //==================================================================================================================
    public class FnlcostentityFinalCostEntityDB : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("Cost", TypeName = "float")]
        public double? numnCost { get; set; }

        [Column("Quantity", TypeName = "float")]
        public double? numnQuantity { get; set; }

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

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkJob", TypeName = "int")]
        public int intPkJob { get; set; }
        [ForeignKey("intPkJob")]
        public JobentityJobEntityDB PkJob { get; set; }

        [Required]
        [Column("PkProcessInWorkflow", TypeName = "int")]
        public int intPkProcessInWorkflow { get; set; }
        [ForeignKey("intPkProcessInWorkflow")]
        public PiwentityProcessInWorkflowEntityDB PkProcessInWorkflow { get; set; }

        [Required]
        [Column("PkAccountMovement", TypeName = "int")]
        public int intPkAccountMovement { get; set; }
        [ForeignKey("intPkAccountMovement")]
        public AccmoventityAccountMovementEntityDB PkAccountMovement { get; set; }

        [Column("PkElementElementType", TypeName = "int")]
        public int? intnPkElementElementType { get; set; }
        [ForeignKey("intnPkElementElementType")]
        public EleetentityElementElementTypeEntityDB PkElementElementType { get; set; }

        [Column("PkElementElement", TypeName = "int")]
        public int? intnPkElementElement { get; set; }
        [ForeignKey("intnPkElementElement")]
        public EleeleentityElementElementEntityDB PkElementElement { get; set; }

        [Column("PkResource", TypeName = "int")]
        public int? intnPkResource { get; set; }
        [ForeignKey("intnPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

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
            FnlcostentityFinalCostEntityDB fnlcostentity = (FnlcostentityFinalCostEntityDB)obj_I;

            //                                              //To easy code.
            Date dateStartThis = this.strStartDate.ParseToDate();
            Time timeStartThis = this.strStartTime.ParseToTime();
            ZonedTime ztimeStartThis = ZonedTimeTools.NewZonedTime(dateStartThis, timeStartThis);

            Date dateStartToCompare = fnlcostentity.strStartDate.ParseToDate();
            Time timeStartToCompare = fnlcostentity.strStartTime.ParseToTime();
            ZonedTime ztimeStartToCompare = ZonedTimeTools.NewZonedTime(dateStartToCompare, timeStartToCompare);

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
