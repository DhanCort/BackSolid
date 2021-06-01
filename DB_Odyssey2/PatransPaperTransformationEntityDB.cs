/*TASK RP.DB_Odyssey2 Relevant Part Odyssey2 Data Base Design*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: September 09, 2020.

namespace Odyssey2Backend.DB_Odyssey2
{
    [Table("PaperTransformation")]
    //==================================================================================================================
    public class PatransPaperTransformationEntityDB
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
        [Column("WidthI", TypeName = "float")]
        public double numWidthI { get; set; }

        [Column("HeightI", TypeName = "float")]
        public double? numnHeightI { get; set; }

        [Required]
        [Column("WidthO", TypeName = "float")]
        public double numWidthO { get; set; }

        [Required]
        [Column("HeightO", TypeName = "float")]
        public double numHeightO { get; set; }

        [Column("Unit", TypeName = "nvarchar(30)")]
        public String strUnit { get; set; }

        [Required]
        [Column("Temporary", TypeName = "bit")]
        public bool boolTemporary { get; set; }

        [Column("MarginTop", TypeName = "float")]
        public double? numnMarginTop { get; set; }

        [Column("MarginBottom", TypeName = "float")]
        public double? numnMarginBottom { get; set; }

        [Column("MarginLeft", TypeName = "float")]
        public double? numnMarginLeft { get; set; }

        [Column("MarginRight", TypeName = "float")]
        public double? numnMarginRight { get; set; }

        [Column("HorizontalGap", TypeName = "float")]
        public double? numnHorizontalGap { get; set; }

        [Column("VerticalGap", TypeName = "float")]
        public double? numnVerticalGap { get; set; }

        [Required]
        [Column("Optimized", TypeName = "bit")]
        public bool boolOptimized { get; set; }

        [Required]
        [Column("Cut", TypeName = "bit")]
        public bool boolCut { get; set; }

        [Required]
        [Column("FoldFactor", TypeName = "int")]
        public int intFoldFactor { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        /*FOREIGN KEYS*/

        [Column("PkCalculationOwn", TypeName = "int")]
        public int? intnPkCalculationOwn { get; set; }
        [ForeignKey("intnPkCalculationOwn")]
        public CalentityCalculationEntityDB PkCalculationOwn { get; set; }

        [Column("PkCalculationLink", TypeName = "int")]
        public int? intnPkCalculationLink { get; set; }
        [ForeignKey("intnPkCalculationLink")]
        public CalentityCalculationEntityDB PkCalculationLink { get; set; }

        [Required]
        [Column("PkProcessInWorkflow", TypeName = "int")]
        public int intPkProcessInWorkflow { get; set; }
        [ForeignKey("intPkProcessInWorkflow")]
        public PiwentityProcessInWorkflowEntityDB PkProcessInWorkflow { get; set; }

        [Required]
        [Column("PkResourceI", TypeName = "int")]
        public int intPkResourceI { get; set; }
        [ForeignKey("intPkResourceI")]
        public EleentityElementEntityDB PkResourceI { get; set; }

        [Column("PkResourceO", TypeName = "int")]
        public int? intnPkResourceO { get; set; }
        [ForeignKey("intnPkResourceO")]
        public EleentityElementEntityDB PkResourceO { get; set; }

        [Column("PkElementElementTypeI", TypeName = "int")]
        public int? intnPkElementElementTypeI { get; set; }
        [ForeignKey("intnPkElementElementTypeI")]
        public EleetentityElementElementTypeEntityDB PkElementElementTypeI { get; set; }

        [Column("PkElementElementI", TypeName = "int")]
        public int? intnPkElementElementI { get; set; }
        [ForeignKey("intnPkElementElementI")]
        public EleeleentityElementElementEntityDB PkElementElementI { get; set; }

        [Column("PkElementElementTypeO", TypeName = "int")]
        public int? intnPkElementElementTypeO { get; set; }
        [ForeignKey("intnPkElementElementTypeO")]
        public EleetentityElementElementTypeEntityDB PkElementElementTypeO { get; set; }

        [Column("PkElementElementO", TypeName = "int")]
        public int? intnPkElementElementO { get; set; }
        [ForeignKey("intnPkElementElementO")]
        public EleeleentityElementElementEntityDB PkElementElementO { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
