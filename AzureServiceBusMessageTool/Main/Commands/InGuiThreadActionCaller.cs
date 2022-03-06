using System;

namespace Main.Commands
{
   public class InGuiThreadActionCaller : IInGuiThreadActionCaller
   {
      public void Call(Action action)
      {
         System.Windows.Application.Current.Dispatcher.Invoke(action);
      }
      public T InvokeFunction<T>(Func<T> function)
      {
         return System.Windows.Application.Current.Dispatcher.Invoke(function);
      }
   }
}
