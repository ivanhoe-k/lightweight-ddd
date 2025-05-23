﻿// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Errors
{
    public enum AddressErrorCode
    {
        MissingStreet,
        MissingCity,
        MissingCountry,
        MissingPostalCode
    }
}
