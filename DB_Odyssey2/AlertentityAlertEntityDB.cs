/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 07, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Alert")]
    //==================================================================================================================
    public class AlertentityAlertEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Required]
        [Column("PkPrintshop", TypeName = "int")]
        public int intPkPrintshop { get; set; }
        [ForeignKey("intPkPrintshop")]
        public PsentityPrintshopEntityDB PkPrintshop { get; set; }

        [Required]
        [Column("PkAlertType", TypeName = "int")]
        public int intPkAlertType { get; set; }
        [ForeignKey("intPkAlertType")]
        public AlerttypeentityAlertTypeEntityDB PkAlertType { get; set; }

        [Column("PkResource", TypeName = "int")]
        public int? intnPkResource { get; set; }
        [ForeignKey("intnPkResource")]
        public EleentityElementEntityDB PkResource { get; set; }

        [Column("PkPeriod", TypeName = "int")]
        public int? intnPkPeriod { get; set; }
        [ForeignKey("intnPkPeriod")]
        public PerentityPeriodEntityDB PkPeriod { get; set; }

        [Column("PkTask", TypeName = "int")]
        public int? intnPkTask { get; set; }
        [ForeignKey("intnPkTask")]
        public TaskentityTaskEntityDB PkTask { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        //                                                  //Complex data (string) intContactId|intContactId
        //                                                  //Ex. 133066|152068|224789
        [Column("ReadBy")]
        public String strReadBy { get; set; }

        [Column("JobId", TypeName = "int")]
        public int? intnJobId { get; set; }

        [Column("ContactId", TypeName = "int")]
        public int? intnContactId { get; set; }

        [Column("OtherAttributes", TypeName = "int")]
        public int? intnOtherAttributes { get; set; }

        [Column("Message", TypeName = "nvarchar(250)")]
        public String strMessage { get; set; }

        //--------------------------------------------------------------------------------------------------------------

    }

    //==================================================================================================================
}
/*END-TASK*/
