/*TASK DateTextEnum*/
//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 30, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public enum DateTextEnum : byte
    {
        //                                                  //English: Apr 21, 2018 | Spanish: 21 abr 2018 |
        //                                                  //      French: 21 avr 2018
        SHORT,

        //                                                  //English: Apr, 2018 | Spanish: abr 2018 | French: avr 2018
        SHORT_YEAR_AND_MONTH,

        //                                                  //English: Apr 21 | Spanish: 21 abr | French: 21 avr
        SHORT_MONTH_DAY,

        //                                                  //English: April 21, 2018 | Spanish: 21 de abril de 2018 |
        //                                                  //      French: le 21 avril 2018
        LONG,

        //                                                  //English: April, 2018 | Spanish: abril de 2018 |
        //                                                  //      French: avril 2018
        LONG_YEAR_AND_MONTH,

        //                                                  //English: April 21 | Spanish: 21 abril | French: 21 avril
        LONG_MONTH_DAY,

        //                                                  //English: Saturday, April 21, 2018 | 
        //                                                  //      Spanish: sábado, 21 de abril de 2018 |
        //                                                  //      French: samedi, le 21 avril 2018
        FULL,

        //                                                  //English: Saturday, April 21 | Spanish: sábado, 21 abril
        //                                                  //      French: samedi 21 avril
        FULL_MONTH_DAY,

        //                                                  //English: Apr | Spanish: abr | French: avr
        MONTH_SHORT,

        //                                                  //English: April | Spanish: abril | French: avril
        MONTH_FULL,

        //                                                  //English: Sat | Spanish: sáb | French: sam
        DAY_OF_WEEK_SHORT,

        //                                                  //English: Saturday | Spanish: sábado | French: samedi
        DAY_OF_WEEK_FULL,
    }

    //==================================================================================================================

}
/*END-TASK*/