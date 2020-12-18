using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceAndContravarianceExample.Models
{
    public static class ButtonGExample
    {
        public static void Foo()
        {
            Console.WriteLine("Generic button example");

            var button = new ButtonG() { Text = "Click here" };
            button.Click += Button_Action;
            button.KeyPress += Button_Action;
            button.MouseClick += Button_Action;

            button.PerformClick(new ButtonClickArgs());
            button.PerformKeyPress(new KeyPressArgs());
            button.PerformMouseClick(new MouseClickArgs());
        }

        private static void Button_Action<T>(object sender, T e) where T : ButtonActionArgs
        {
            switch (e)
            {
                case ButtonClickArgs:
                case KeyPressArgs:
                    Console.WriteLine("Button_Click or Button_KeyPress");
                    break;
                case MouseClickArgs:
                    Console.WriteLine("Button_MouseClick");
                    break;
                default:
                    Console.WriteLine("Undefined action");
                    break;
            }
        }
    }
}
