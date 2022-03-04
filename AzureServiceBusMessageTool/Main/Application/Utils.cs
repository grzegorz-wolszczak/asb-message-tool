using System;

namespace Main.Application
{
   public static class Utils
   {
      public static string GetShortTimestamp()
      {
         var time = DateTime.Now.ToString("HH:mm:ss.fff");
         return $"[{time}]";
      }
   }
}
