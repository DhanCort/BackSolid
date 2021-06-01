/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 14, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Administrator")]
    //==================================================================================================================
    public class AdminentityAdministratorEntityDB
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
        [Column("Email", TypeName = "nvarchar(300)")]
        public String strEmail { get; set; }

        [Required]
        [Column("Password", TypeName = "nvarchar(300)")]
        public String strPassword { get; set; }

        [Required]
        [Column("Name", TypeName = "nvarchar(300)")]
        public String strName { get; set; }

        [Required]
        [Column("LastName", TypeName = "nvarchar(300)")]
        public String strLastName { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
