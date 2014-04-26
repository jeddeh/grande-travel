﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GrandeTravel.Entity;
using System.ComponentModel.DataAnnotations;

namespace GrandeTravel.Site.Models
{
    public class SearchPackagesViewModel
    {
        [Required]
        public List<Package> Packages { get; set; }
    }
}