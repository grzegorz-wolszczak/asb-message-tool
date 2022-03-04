using System;
using System.Runtime.ExceptionServices;

namespace Main.ExceptionHandling;

static class ExceptionExtensions
{
   public static void ThrowOnDispatcher(this Exception exc)
   {
      System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => {
         // preserve the callstack of the exception
         ExceptionDispatchInfo.Capture(exc).Throw();
      }));
   }
}