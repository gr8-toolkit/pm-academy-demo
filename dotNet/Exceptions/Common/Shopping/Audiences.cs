using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum Audiences
    {
        /// <summary>
        /// All Ages Admitted
        /// </summary>
        G = 1,
        /// <summary>
        /// Some Material May Not Be Suitable For Children
        /// </summary>
        PG,
        /// <summary>
        /// Some Material May Be Inappropriate For Children Under 13
        /// </summary>
        PG13,
        /// <summary>
        /// Children Under 17 Require Accompanying Parent or Adult Guardian
        /// </summary>
        R,
        /// <summary>
        /// No children will be admitted.
        /// </summary>
        NC17
    }
}
