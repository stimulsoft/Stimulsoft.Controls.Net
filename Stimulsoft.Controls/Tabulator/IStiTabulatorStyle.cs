using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Stimulsoft.Controls
{
    public interface IStiTabulatorStyle
    {
        #region Methods
        void DrawPage(Graphics g, StiTabTitlePosition position, StiTabulatorPage page);

        void DrawPageTitle(Graphics g, StiTabTitlePosition position, StiTabulatorPage page);
        #endregion
    }
}
