using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ald.Internationalization;

namespace ALDBluetoothATConfig.Internationalization
{
    public class MyLanguages : ResourcesManager
    {
        private static MyLanguages current;

        public static MyLanguages Current
        {
            get
            {
                if (current == null)
                    current = new MyLanguages();
                return current;
            }
        }

        private MyLanguages() : base()
        {
            this.RegisterLanguage("Spanish", new Uri("Internationalization/Resources/es-ES.xaml", UriKind.Relative));
            this.RegisterLanguage("English", new Uri("Internationalization/Resources/en-US.xaml", UriKind.Relative));
        }
    }
}
