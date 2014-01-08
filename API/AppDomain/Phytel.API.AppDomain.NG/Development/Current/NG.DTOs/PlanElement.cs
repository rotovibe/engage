﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PlanElement : IPlanElement
    {
        /// <summary>
        /// Id of element
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Ordinal of the current item in a list
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Will show disabled or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Completed summary
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Next like dependent element Id.
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// Previous like dependent element Id.
        /// </summary>
        public string Previous { get; set; }

        public SpawnElement SpawnElement { get; set; }
    }
}
