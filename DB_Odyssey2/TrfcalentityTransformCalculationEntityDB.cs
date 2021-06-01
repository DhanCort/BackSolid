/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Odyssey2Backend.Resources;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using TowaStandard;


//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: Agost 03, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("TransformCalculation")]
    //==================================================================================================================
    public class TrfcalentityTransformCalculationEntityDB : IComparable
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
        [Column("Needed", TypeName = "float")]
        public double numNeeded { get; set; }

        [Required]
        [Column("PerUnits", TypeName = "float")]
        public double numPerUnits { get; set; }

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

        [Column("ConditionToApply", TypeName = "nvarchar(500)")]
        public String strConditionToApply { get; set; }

        [Required]
        [Column("ConditionAnd", TypeName = "bit")]
        public bool boolConditionAnd { get; set; }

        [Column("ConditionQuantity", TypeName = "nvarchar(50)")]
        public String strConditionQuantity { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkProcessInWorkflow", TypeName = "int")]
        public int intPkProcessInWorkflow { get; set; }
        [ForeignKey("intPkProcessInWorkflow")]
        public PiwentityProcessInWorkflowEntityDB PkProcessInWorkflow { get; set; }

        [Column("PkElementElementTypeI", TypeName = "int")]
        public int? intnPkElementElementTypeI { get; set; }
        [ForeignKey("intnPkElementElementTypeI")]
        public EleetentityElementElementTypeEntityDB PkElementElementTypeI { get; set; }

        [Column("PkElementElementI", TypeName = "int")]
        public int? intnPkElementElementI { get; set; }
        [ForeignKey("intnPkElementElementI")]
        public EleeleentityElementElementEntityDB PkElementElementI { get; set; }

        [Required]
        [Column("PkResourceI", TypeName = "int")]
        public int intPkResourceI { get; set; }
        [ForeignKey("intPkResourceI")]
        public EleentityElementEntityDB PkResourceI { get; set; }

        [Column("PkElementElementTypeO", TypeName = "int")]
        public int? intnPkElementElementTypeO { get; set; }
        [ForeignKey("intnPkElementElementTypeO")]
        public EleetentityElementElementTypeEntityDB PkElementElementTypeO { get; set; }

        [Column("PkElementElementO", TypeName = "int")]
        public int? intnPkElementElementO { get; set; }
        [ForeignKey("intnPkElementElementO")]
        public EleeleentityElementElementEntityDB PkElementElementO { get; set; }

        [Required]
        [Column("PkResourceO", TypeName = "int")]
        public int intPkResourceO { get; set; }
        [ForeignKey("intPkResourceO")]
        public EleentityElementEntityDB PkResourceO { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            TrfcalentityTransformCalculationEntityDB trfcalentity = (TrfcalentityTransformCalculationEntityDB)obj_I;

            ZonedTime ztime = ZonedTimeTools.NewZonedTime(this.strStartDate.ParseToDate(), this.strStartTime.ParseToTime());
            ZonedTime ztimeB = ZonedTimeTools.NewZonedTime(trfcalentity.strStartDate.ParseToDate(),
                trfcalentity.strStartTime.ParseToTime());

            return ztime.CompareTo(ztimeB);
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
