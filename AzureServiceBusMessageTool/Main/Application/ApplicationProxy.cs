namespace Main.Application;

public class ApplicationProxy : IApplicationProxy
{
   private readonly System.Windows.Application _wpfApplication;

   public ApplicationProxy(System.Windows.Application wpfApplication)
   {
      _wpfApplication = wpfApplication;
   }
}
