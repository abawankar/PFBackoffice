using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Report
{
    public class Form3AModel
    {
        public string UAN { get; set; }
        public string Name { get; set; }
        public string Month { get; set; }
        public double Wages { get; set; }
        public double SelfCont { get; set; }
        public double ERCont { get; set; }
        public double EPSCont { get; set; }
        public double Total { get; set; }
        public int ContCount { get; set; }
    }
}