using CovarianceAndContravarianceExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceAndContravarianceExample
{
    public class ButtonG
    {
        public event EventHandler<ButtonClickArgs> Click;
        public event EventHandler<KeyPressArgs> KeyPress;
        public event EventHandler<MouseClickArgs> MouseClick;

        public string Text { get; set; }

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
