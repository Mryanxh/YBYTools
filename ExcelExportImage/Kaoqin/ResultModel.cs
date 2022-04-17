using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaYanTools.Kaoqin
{
    public class ResultModel
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<DataModel> Data { get; set; }
    }

    public class DataModel
    {
        public int Id { get; set; }
        public string Pin { get; set; }
        public string Username { get; set; }
        public string Deptname { get; set; }
        public DateTime TTime { get; set; }
        public string SN { get; set; }
        public string State { get; set; }
    }
    public class HolidayModel
    {
        public bool Holiday { get; set; }
        public string Name { get; set; }
        public int Wage { get; set; }
        public DateTime Date { get; set; }
        public bool After { get; set; }
        public string Target { get; set; }
        public int Rest { get; set; }
    }
}
