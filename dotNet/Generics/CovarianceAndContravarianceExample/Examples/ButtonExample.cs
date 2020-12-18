using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceAndContravarianceExample.Models
{

    public static class ButtonExample
    {
        delegate void Click(object sender, ButtonClickArgs args);
        delegate void KeyPress(object sender, KeyPressArgs args);
        delegate void MouseClick(object sender, MouseClickArgs args);

        public static void Foo()
        {
            Console.WriteLine("Button example");

            var button = new Button() { Text = "Click here" };

            button.Click += Button_Click;
            button.KeyPress += Button_KeyPress;
            button.MouseClick += Button_MouseClick;

            // contrvariance
            button.Click += Button_Action;
            button.KeyPress += Button_Action;
            button.MouseClick += Button_Action;

            button.PerformClick(new ButtonClickArgs());
            button.PerformKeyPress(new KeyPressArgs());
            button.PerformMouseClick(new MouseClickArgs());
        }

        private static void Button_Click(object sender, ButtonClickArgs args)
        {
            Console.WriteLine("Button_Click");
        }

        private static void Button_KeyPress(object sender, KeyPressArgs args)
        {
            Console.WriteLine("Button_KeyPress");
        }

        private static void Button_MouseClick(object sender, MouseClickArgs args)
        {
            Console.WriteLine("Button_MouseClick");
        }


        public static void Button_Action(object sender, ButtonActionArgs args)
        {
            Console.WriteLine("Button_Action");
        }
    }
}
