using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Butthesda
{
    public partial class CheckBox_custom
    {
        public CheckBox checkBox;
        public Device.BodyPart bodyPart;
        public Device.EventType eventType;

        public CheckBox_custom(CheckBox checkBox, Device.BodyPart bodyPart, Device.EventType eventType)
        {
            this.checkBox = checkBox;
            this.bodyPart = bodyPart;
            this.eventType = eventType;
        }
    }
}
