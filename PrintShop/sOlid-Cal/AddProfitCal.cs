using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;

namespace Odyssey2Backend.PrintShop.sOlid_Cal
{
    public class AddProfitCal : IAddCal
    {
        public void AddCalculation(
            ICalculation cal,
            PsPrintShop ps_I,
            ProProcess pro_I,
            ResResource res_I,
            ProdtypProductType prodtyp_I,
            ref int intStatus_IO,
            ref string strUserMessage_IO,
            ref string strDevMessage_IO)
        {
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "For the profit calculation you need: A product and strBy = ByProduct. The " +
                "process, resource type, resource, ascendants and value must be null.";
            if (
                //                                          //Must have a product. Must not have a process, a resource 
                //                                          //      type, a resource, ascendants, value for the 
                //                                          //      attribute for the orden form.
                (prodtyp_I != null) && (pro_I == null) && (res_I == null) &&
                (cal.strAscendantElements == null) && (cal.strValue == null) &&
                //                                          //From byProduct screen.
                (cal.strByX == CalCalculation.strByProduct)
                )
            {
                intStatus_IO = 403;
                strUserMessage_IO = "The profit should be greater than zero.";
                strDevMessage_IO = "";
                if (
                    cal.numnProfit > 0
                    )
                {
                    CalentityCalculationEntityDB calentity = new CalentityCalculationEntityDB
                    {
                        intnPkProduct = prodtyp_I.intPk,
                        strCalculationType = cal.strCalculationType,
                        strDescription = cal.strDescription,
                        boolIsEnable = cal.boolIsEnable,
                        numnProfit = cal.numnProfit,
                        strByX = cal.strByX,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString()
                    };
                    context.Calculation.Add(calentity);
                    context.SaveChanges();

                    CalCalculation.subDisableOtherProfitCalculations(calentity);
                    intStatus_IO = 200;
                    strUserMessage_IO = "Success";
                }
            }
        }
    }
}
