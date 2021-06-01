/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using Odyssey2Backend.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 11, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Workflow")]
    //==================================================================================================================
    public class WfentityWorkflowEntityDB : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/

        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Column("PkProduct", TypeName = "int")]
        public int? intnPkProduct { get; set; }
        [ForeignKey("intnPkProduct")]
        public EtentityElementTypeEntityDB PkProduct { get; set; }

        [Required]
        [Column("PkPrintshop", TypeName = "int")]
        public int intPkPrintshop { get; set; }
        [ForeignKey("intPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Required]
        [Column("Name")]
        public String strName { get; set; }

        [Required]
        [Column("WorkflowId", TypeName = "int")]
        public int intWorkflowId { get; set; }

        [Column("JobId", TypeName = "int")]
        public int? intnJobId { get; set; }

        [Required]
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartTime", TypeName = "nvarchar(10)")]
        public String strStartTime { get; set; }

        [Required]
        [Column("Deleted", TypeName = "bit")]
        public bool boolDeleted { get; set; }

        [Required]
        [Column("Default", TypeName = "bit")]
        public bool boolDefault { get; set; }

        //                                                  //true means this wf was used to show a price in the website.
        [Required]
        [Column("Pricing", TypeName = "bit")]
        public bool boolPricing { get; set; }

        [Column("Generic", TypeName = "bit")]
        public bool? boolnGeneric { get; set; }

        //--------------------------------------------------------------------------------------------------------------        
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            WfentityWorkflowEntityDB wfentity = (WfentityWorkflowEntityDB)obj_I;

            ZonedTime ztime = ZonedTimeTools.NewZonedTime(this.strStartDate.ParseToDate(), this.strStartTime.ParseToTime());
            ZonedTime ztimeB = ZonedTimeTools.NewZonedTime(wfentity.strStartDate.ParseToDate(),
                wfentity.strStartTime.ParseToTime());

            return ztime.CompareTo(ztimeB);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/