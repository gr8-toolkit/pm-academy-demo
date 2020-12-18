using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceAndContravarianceExample.Models
{
    public class Button
    {
        public delegate void ClickHandler(object sender, ButtonClickArgs args);
        public delegate void KeyPressHandler(object sender, KeyPressArgs args);
        public delegate void MouseClickHandler(object sender, MouseClickArgs args);

        public string Text { get; set; }
        public event ClickHandler Click;
        public event KeyPressHandler KeyPress;
        public event MouseClickHandler MouseClick;


        public void PerformClick(ButtonClickArgs args)
        {
            Click?.Invoke(this, args);
        }

        public void PerformKeyPress(KeyPressArgs args)
        {
            KeyPress?.Invoke(this, args);
        }

        public void PerformMouseClick(MouseClickArgs args)
        {
            MouseClick?.Invoke(this, args);
        }
    }
}
