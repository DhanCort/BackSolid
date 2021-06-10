using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odyssey2Backend.PrintShop.sOlid_Cal
{
    public interface IAddCal
    {
        void AddCalculation(
            ICalculation cal,
            PsPrintShop ps_I,
            ProProcess pro_I,
            ResResource res_I,
            ProdtypProductType prodtyp_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            );
    }
}
