/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Element")]
    //==================================================================================================================
    public class EleentityElementEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*UNIQUE KEYS*/

        [Required]
        [Column("ElementName", TypeName = "nvarchar(200)")]
        public String strElementName { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkElementType", TypeName = "int")]
        public int intPkElementType { get; set; }
        [ForeignKey("intPkElementType")]
        public EtentityElementTypeEntityDB PkElementType { get; set; }

        [Column("PkElementInherited", TypeName = "int")]
        public int? intnPkElementInherited { get; set; }
        [ForeignKey("intnPkElementInherited")]
        public EleentityElementEntityDB PkElementInherited { get; set; }

        [Column("PkElementCalendarInherited", TypeName = "int")]
        public int? intnPkElementCalendarInherited { get; set; }
        [ForeignKey("intnPkElementCalendarInherited")]
        public EleentityElementEntityDB PkElementCalendarInherited { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Required]
        [Column("IsTemplate", TypeName = "bit")]
        public bool boolIsTemplate { get; set; }

        [Column("IsCalendar", TypeName = "bit")]
        public bool? boolnIsCalendar { get; set; }

        [Column("IsAvailable", TypeName = "bit")]
        public bool? boolnIsAvailable { get; set; }

        [Column("Deleted", TypeName = "bit")]
        public bool boolDeleted { get; set; }

        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Column("EndDate", TypeName = "nvarchar(10)")]
        public String strEndDate { get; set; }

        [Column("EndTime", TypeName = "nvarchar(10)")]
        public String strEndTime { get; set; }

        [Column("CalendarIsChangeable", TypeName = "bit")]
        public bool? boolnCalendarIsChangeable { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
