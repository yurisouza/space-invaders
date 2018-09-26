using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.YuriSouza.Utility
{
    public class ControlImplementation : IControl
    {
        private Control control;

        public ControlImplementation(object obj)
        {
            control = (Control)obj;
        }

        public object GetControl()
        {
            return control;
        }
        
    }
}
