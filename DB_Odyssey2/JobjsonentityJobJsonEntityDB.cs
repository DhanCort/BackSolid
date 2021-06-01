/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: October 19, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("JobJson")]
    //==================================================================================================================
    public class JobjsonentityJobJsonEntityDB
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
        [Column("JobId", TypeName = "int")]
        public int intJobID { get; set; }

        [Column("PrintshopId", TypeName = "nvarchar(10)")]
        public String strPrintshopId { get; set; }

        [Column("Json", TypeName = "nvarchar(4000)")]
        public String jobjson { get; set; }

        [Required]
        [Column("OrderId", TypeName = "int")]
        public int intOrderId { get; set; }

        [Column("OrderNumber", TypeName = "int")]
        public int? intnOrderNumber { get; set; }

        [Column("JobNumber", TypeName = "int")]
        public int? intnJobNumber { get; set; }

        [Column("EstimateNumber", TypeName = "int")]
        public int? intnEstimateNumber { get; set; }

        [Column("Price", TypeName = "nvarchar(50)")]
        public String strPrice { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/