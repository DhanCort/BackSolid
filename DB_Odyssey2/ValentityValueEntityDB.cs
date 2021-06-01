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
    [Table("Value")]
    //==================================================================================================================
    public class ValentityValueEntityDB : IComparable
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
        [Column("Value", TypeName = "nvarchar(500)")]
        public String strValue { get; set; }        

        [Column("IsChangeable", TypeName = "bit")]
        public bool? boolnIsChangeable { get; set; }

        [Column("SetDate", TypeName = "nvarchar(10)")]
        public String strSetDate { get; set; }

        [Column("SetTime", TypeName = "nvarchar(10)")]
        public String strSetTime { get; set; }

        [Column("Decimal", TypeName = "bit")]
        public bool? boolnIsDecimal { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Required]
        [Column("PkAttribute", TypeName = "int")]
        public int intPkAttribute { get; set; }
        [ForeignKey("intPkAttribute")]
        public AttrentityAttributeEntityDB PkAttribute { get; set; }

        [Required]
        [Column("PkElement", TypeName = "int")]
        public int intPkElement { get; set; }
        [ForeignKey("intPkElement")]
        public EleentityElementEntityDB PkElement { get; set; }

        [Column("PkValueInherited", TypeName = "int")]
        public int? intnPkValueInherited { get; set; }
        [ForeignKey("intnPkValueInherited")]
        public ValentityValueEntityDB PkValueInherited { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            ValentityValueEntityDB valentity = (ValentityValueEntityDB)obj_I;

            ZonedTime ztime = ZonedTimeTools.NewZonedTime(this.strSetDate.ParseToDate(), this.strSetTime.ParseToTime());
            ZonedTime ztimeB = ZonedTimeTools.NewZonedTime(valentity.strSetDate.ParseToDate(),
                valentity.strSetTime.ParseToTime());

            return ztime.CompareTo(ztimeB);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
