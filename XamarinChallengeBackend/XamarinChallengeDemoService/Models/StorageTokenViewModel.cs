using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XamarinChallengeDemoService.Models
{
    public class StorageTokenViewModel
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public string SasToken { get; set; }
    }
}