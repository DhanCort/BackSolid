/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: August 03, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Task")]
    //==================================================================================================================
    public class TaskentityTaskEntityDB
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
        [Column("StartDate", TypeName = "nvarchar(10)")]
        public String strStartDate { get; set; }

        [Required]
        [Column("StartHour", TypeName = "nvarchar(2)")]
        public String strStartHour { get; set; }

        [Required]
        [Column("StartMinute", TypeName = "nvarchar(2)")]
        public String strStartMinute { get; set; }

        [Required]
        [Column("StartSecond", TypeName = "nvarchar(2)")]
        public String strStartSecond { get; set; }

        [Required]
        [Column("EndDate", TypeName = "nvarchar(10)")]
        public String strEndDate { get; set; }

        [Required]
        [Column("EndTime", TypeName = "nvarchar(10)")]
        public String strEndTime { get; set; }

        [Required]
        [Column("Description", TypeName = "nvarchar(100)")]
        public String strDescription { get; set; }

        [Required]
        [Column("ContactId", TypeName = "int")]
        public int intContactId { get; set; }

        [Column("CustomerId", TypeName = "int")]
        public int? intnCustomerId { get; set; }

        [Required]
        [Column("MinsBeforeNotify", TypeName = "int")]
        public int intMinsBeforeNotify { get; set; }

        [Required]
        [Column("IsNotifiedable", TypeName = "bit")]
        public bool boolIsNotifiedable { get; set; }

        [Required]
        [Column("IsCompleted", TypeName = "bit")]
        public bool boolIsCompleted { get; set; }

        [Column("NotificationDate", TypeName = "nvarchar(10)")]
        public String strNotificationDate { get; set; }

        [Column("NotificationHour", TypeName = "nvarchar(2)")]
        public String strNotificationHour { get; set; }

        [Column("NotificationMinute", TypeName = "nvarchar(2)")]
        public String strNotificationMinute { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        [Required]
        [Column("PkPrintshop", TypeName = "int")]
        public int intPkPrintshop { get; set; }
        [ForeignKey("intPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
