/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 07, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("AlertType")]
    //==================================================================================================================
    public class AlerttypeentityAlertTypeEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        public const String strAvailability = "Availability";
        public const String strPeriod = "Period";
        public const String strTask = "Task";
        public const String strNewEstimate = "New Estimate";
        public const String strNewOrder = "New Order";
        public const String strItemsToAnswer = "Items To Answer";
        public const String strReadyToGo = "Ready To Go";
        public const String strDueDateAtRisk = "Due date at risk";
        public const String strDueDateInThePast = "Due date in the past";
        public const String strMentioned = "Mentioned";

        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Column("Type", TypeName = "nvarchar(100)")]
        public String strType { get; set; }

        [Required]
        [Column("Description", TypeName = "nvarchar(300)")]
        public String strDescription { get; set; }

        //--------------------------------------------------------------------------------------------------------------

    }

    //==================================================================================================================
}
/*END-TASK*/
