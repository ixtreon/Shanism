using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoRPGWin.UI
{
    public partial class DamageDefenseMoveSpeed : UserControl
    {
        public DamageDefenseMoveSpeed()
        {
       

            InitializeComponent();

        }

        public int Damage
            {
                set
                {
                    lblDmgNum.Text = value.ToString();
                }
            }
        public int Defense
        {
            set
            {
                lblDefNum.Text = value.ToString();
 
            }
        }
        public int MoveSpeed
        {
            set
            {
                lblMoveNum.Text = value.ToString();  
            }
        }
    }
}
