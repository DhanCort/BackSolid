/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Odyssey2Backend.Resources;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using TowaStandard;


//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: March 31, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Cost")]
    //==================================================================================================================
    public class CostentityCostEntityDB : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("Quantity", TypeName = "float")]
        public double? numnQuantity { get; set; }

        [Column("Cost", TypeName = "float")]
        public double? numnCost { get; set; }        

        [Column("Min", TypeName = "float")]
        public double? numnMin { get; set; }

        [Column("Block", TypeName = "float")]
        public double? numnBlock { get; set; }

        [Required]
        [Column("SetDate", TypeName = "nvarchar(10)")]
        public String strSetDate { get; set; }

        [Required]
        [Column("SetTime", TypeName = "nvarchar(10)")]
        public String strSetTime { get; set; }

        [Column("IsChangeable", TypeName = "bit")]
        public bool? boolnIsChangeable { get; set; }

        [Column("HourlyRate", TypeName = "float")]
        public double? numnHourlyRate { get; set; }

        [Column("Area", TypeName = "bit")]
        public bool? boolnArea { get; set; } 

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkResource", TypeName = "int")]
        public int intPkResource { get; set; }
        [ForeignKey("intPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

        [Column("PkCostInherited", TypeName = "int")]
        public int? intnPkCostInherited { get; set; }
        [ForeignKey("intnPkCostInherited")]
        public CostentityCostEntityDB PkCostInherited { get; set; }

        [Required]
        [Column("PkAccount", TypeName = "int")]
        public int intPkAccount { get; set; }
        [ForeignKey("intPkAccount")]
        public AccentityAccountEntityDB PkAccount { get; set; }

        //--------------------------------------------------------------------------------------------------------------        
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            CostentityCostEntityDB costentity = (CostentityCostEntityDB)obj_I;

            ZonedTime ztime = ZonedTimeTools.NewZonedTime(this.strSetDate.ParseToDate(), this.strSetTime.ParseToTime());
            ZonedTime ztimeB = ZonedTimeTools.NewZonedTime(costentity.strSetDate.ParseToDate(),
                costentity.strSetTime.ParseToTime());

            return ztime.CompareTo(ztimeB);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
