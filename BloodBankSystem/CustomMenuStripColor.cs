using System.Drawing;
using System.Windows.Forms;

namespace BloodBankSystem
{
    public class CustomMenuStripColor : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.SkyBlue; }
        }
        public override Color MenuBorder
        {
            get { return Color.Blue; }
        }
    }
}
