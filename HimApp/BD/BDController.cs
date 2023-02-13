using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimApp.BD
{
    public partial class HimBDEntities
    {
        private static HimBDEntities Context;

        public static HimBDEntities GetContext()
        {
            if (Context == null)
                Context = new HimBDEntities();
            return Context;
        }

    }
}
