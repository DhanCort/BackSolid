/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 14, 2019.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("ElementType")]
    //==================================================================================================================
    public class EtentityElementTypeEntityDB
    {
        //                                                  //1. Contiene los datos de los productos del printshop.
        //                                                  //      Pk    
        //                                                  //      CustomTypeId = nombre del producto.
        //                                                  //      PkPrintshop
        //                                                  //      Classification = *****NULL*****
        //                                                  //      XJDFTypeId = *****NULL*****
        //                                                  //      AddedBy = printshopId.
        //                                                  //      ResOrPro = "Product".
        //                                                  //      WebsiteProductKey = Aqui si tiene sentido, para 
        //                                                  //          procesos y tipos de recursos no.
        //                                                  //      Category = La categoria del producto, solo aqui
        //                                                  //          hace sentidom en procesos y tipos de recursos no.

        //                                                  //2.1 Contiene los datos de los procesos XJDF.
        //                                                  //      Pk    
        //                                                  //      CustomTypeId = 
        //                                                  //      PkPrintshop
        //                                                  //      Classification = 
        //                                                  //      XJDFTypeId = 
        //                                                  //      AddedBy = 
        //                                                  //      ResOrPro = 
        //                                                  //      WebsiteProductKey = *****NULL*****
        //                                                  //      Category = *****NULL*****

        //                                                  //2.2 Contiene los datos de los procesos del printshop.
        //                                                  //      Pk    
        //                                                  //      CustomTypeId = 
        //                                                  //      PkPrintshop
        //                                                  //      Classification = 
        //                                                  //      XJDFTypeId = 
        //                                                  //      AddedBy = 
        //                                                  //      ResOrPro = 
        //                                                  //      WebsiteProductKey = *****NULL*****
        //                                                  //      Category = *****NULL*****


        //--------------------------------------------------------------------------------------------------------------
        /*PRIMARY KEY*/

        [Key]
        [Column("Pk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int intPk { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*UNIQUE KEYS*/

        [Required]
        [Column("CustomTypeId", TypeName = "nvarchar(3500)")]
        public String strCustomTypeId { get; set; }
        
        [Column("PkPrintshop")]
        public int? intPrintshopPk { get; set; }
        [ForeignKey("intPrintshopPk")]
        public PsentityPrintshopEntityDB PkPrintshopId { get; set; }

        [Column("Classification")]
        public String strClassification{ get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*COLUMNS*/

        [Required]
        [Column("XJDFTypeId", TypeName = "nvarchar(100)")]
        public String strXJDFTypeId { get; set; }

        [Required]
        [Column("AddedBy", TypeName = "nvarchar(20)")]
        public String strAddedBy { get; set; }

        [Required]
        [Column("ResOrPro", TypeName = "nvarchar(10)")]
        public String strResOrPro { get; set; }
        
        [Column("WebsiteProductKey", TypeName = "int")]
        public int? intWebsiteProductKey { get; set; }
        
        [Column("Category")]
        public String strCategory { get; set; }

        [Column("IsPublic", TypeName = "bit")]
        public bool? boolnIsPublic { get; set; }

        [Required]
        [Column("Deleted", TypeName = "bit")]
        public bool boolDeleted { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Column("PkAccount", TypeName = "int")]
        public int? intnPkAccount { get; set; }
        [ForeignKey("intnPkAccount")]
        public AccentityAccountEntityDB PkAccount { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
