using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Common
{
    public class ServiceMessage
    {
        /// <summary>
        /// Property name that has error
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the code for this message.
        /// </summary>
        /// <value>
        /// The code for this message.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description for this message.
        /// </summary>
        /// <value>
        /// The description for this message.
        /// </value>
        public string Description { get; set; }
    }
}
