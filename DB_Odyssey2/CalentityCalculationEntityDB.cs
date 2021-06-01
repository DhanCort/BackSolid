/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Odyssey2Backend.Resources;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using TowaStandard;


//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Calculation")]
    //==================================================================================================================
    public class CalentityCalculationEntityDB : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("MinAmount", TypeName = "int")]
        public int? intnMinAmount { get; set; }

        [Column("MaxAmount", TypeName = "int")]
        public int? intnMaxAmount { get; set; }

        [Column("Unit", TypeName = "nvarchar(30)")]
        public String strUnit { get; set; }

        [Column("Quantity", TypeName = "float")]
        public double? numnQuantity { get; set; }

        [Column("Needed", TypeName = "float")]
        public double? numnNeeded { get; set; }

        [Column("PerUnits", TypeName = "float")]
        public double? numnPerUnits { get; set; }

        [Column("Cost", TypeName = "float")]
        public double? numnCost { get; set; }

        [Column("Hours", TypeName = "int")]
        public int? intnHours { get; set; }

        [Column("Minutes", TypeName = "int")]
        public int? intnMinutes { get; set; }

        [Column("Seconds", TypeName = "int")]
        public int? intnSeconds { get; set; }

        [Column("Min", TypeName = "float")]
        public double? numnMin { get; set; }

        [Column("Block", TypeName = "float")]
        public double? numnBlock { get; set; }

        [Required]
        [Column("IsEnable", TypeName = "bit")]
        public bool boolIsEnable { get; set; }

        [Column("Value", TypeName = "nvarchar(200)")]
        public String strValue { get; set; }

        [Column("Ascendants", TypeName = "nvarchar(200)")]
        public String strAscendants { get; set; }

        [Column("Description", TypeName = "nvarchar(200)")]
        public String strDescription { get; set; }

        [Column("Profit", TypeName = "float")]
        public double? numnProfit { get; set; }

        [Required]
        [Column("CalculationType", TypeName = "nvarchar(5)")]
        public String strCalculationType { get; set; }

        [Column("ConditionToApply", TypeName = "nvarchar(500)")]
        public String strConditionToApply { get; set; }

        [Required]
        [Column("ConditionAnd", TypeName = "bit")]
        public bool boolConditionAnd { get; set; }

        [Required]
        [Column("ByX", TypeName = "nvarchar(5)")]
        public String strByX { get; set; }

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

        [Column("QuantityWaste", TypeName = "float")]
        public double? numnQuantityWaste { get; set; }

        [Column("PercentWaste", TypeName = "float")]
        public double? numnPercentWaste { get; set; }

        [Column("FromThickness", TypeName = "bit")]
        public bool? boolnFromThickness { get; set; }

        [Column("IsBlock", TypeName = "bit")]
        public bool? boolnIsBlock { get; set; }

        [Column("ByArea", TypeName = "bit")]
        public bool? boolnByArea { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Column("PkProduct", TypeName = "int")]
        public int? intnPkProduct { get; set; }
        [ForeignKey("intnPkProduct")]
        public EtentityElementTypeEntityDB PkProduct { get; set; }

        [Column("PkProcess", TypeName = "int")]
        public int? intnPkProcess { get; set; }
        [ForeignKey("intnPkProcess")]
        public EleentityElementEntityDB PkProcess { get; set; }

        [Column("PkResource", TypeName = "int")]
        public int? intnPkResource { get; set; }
        [ForeignKey("intnPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

        [Column("PkElementElementType", TypeName = "int")]
        public int? intnPkElementElementType { get; set; }
        [ForeignKey("intnPkElementElementType")]
        public EleetentityElementElementTypeEntityDB PkElementElementType { get; set; }

        [Column("PkElementElement", TypeName = "int")]
        public int? intnPkElementElement { get; set; }
        [ForeignKey("intnPkElementElement")]
        public EleeleentityElementElementEntityDB PkElementElement { get; set; }

        [Column("PkWorkflow", TypeName = "int")]
        public int? intnPkWorkflow { get; set; }
        [ForeignKey("intnPkWorkflow")]
        public WfentityWorkflowEntityDB PkWorkflow { get; set; }
        
        [Column("ProcessInWorkflowId", TypeName = "int")]
        public int? intnProcessInWorkflowId { get; set; }

        [Column("PkQFromElementElementType", TypeName = "int")]
        public int? intnPkQFromElementElementType { get; set; }
        [ForeignKey("intnPkQFromElementElementType")]
        public EleetentityElementElementTypeEntityDB PkQFromElementElementType { get; set; }

        [Column("PkQFromElementElement", TypeName = "int")]
        public int? intnPkQFromElementElement { get; set; }
        [ForeignKey("intnPkQFromElementElement")]
        public EleeleentityElementElementEntityDB PkQFromElementElement { get; set; }

        [Column("PkQFromResource", TypeName = "int")]
        public int? intnPkQFromResource { get; set; }
        [ForeignKey("intnPkQFromResource")]
        public EleentityElementEntityDB PkQFromResource { get; set; }

        [Column("PkAccount", TypeName = "int")]
        public int? intnPkAccount { get; set; }
        [ForeignKey("intnPkAccount")]
        public AccentityAccountEntityDB PkAccount { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            CalentityCalculationEntityDB calentity = (CalentityCalculationEntityDB)obj_I;

            ZonedTime ztime = ZonedTimeTools.NewZonedTime(this.strStartDate.ParseToDate(), this.strStartTime.ParseToTime());
            ZonedTime ztimeB = ZonedTimeTools.NewZonedTime(calentity.strStartDate.ParseToDate(),
                calentity.strStartTime.ParseToTime());

            return ztime.CompareTo(ztimeB);
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
