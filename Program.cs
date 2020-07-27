using System;

namespace WizardGrenade
{
#if WINDOWS || LINUX

    public static class Program
    {

        [STAThread]
        static void Main()
        {
            using (var game = new WizardGrenadeGame())
                game.Run();
        }
    }
#endif
}
