﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDomain.Engage.Population.DTO.Context
{
    public interface IServiceContext
    {
        string Contract { get; set; }
        double Version { get; set; }
        string UserId { get; set; }
        string Token { get; set; }
        object Tag { get; set; }
    }
}
