using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMan.Blues.Model
{
    public class SystemAction
    {
        public int OrderNo { get; set; }

        public ActionType CurrentAction { get; set; }

        public DateTime ActionTime { get; set; }
    }

    public enum ActionType
    {
        Unknown = -1,
        Lock = 0,
        Unlock = 1,
        StartUp = 3
    }
}
