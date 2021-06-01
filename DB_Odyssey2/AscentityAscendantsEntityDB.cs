/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: January 21, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("Ascendants")]
    //==================================================================================================================
    public class AscentityAscendantsEntityDB
    {
        //--------------------------------------------------------------------------------------------------------------
        /*Primary key*/
        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        /*NULLABLE*/
        [Column("Ascendants", TypeName = "nvarchar(200)")]
        public String strAscendants { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/
        /*NULLABLE*/
        [Column("PkElement", TypeName = "int")]
        public int intPkElement { get; set; }
        [ForeignKey("intPkElement")]
        public EleentityElementEntityDB pkElement { get; set; }


        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
