using System;
using Main.Windows;

namespace Main.ExceptionHandling;
// code from here: https://thecolorofcode.com/2019/08/11/how-to-show-an-unhandled-exception-window-in-wpf/
/// <summary>
/// This ExceptionHandler implementation opens a new
/// error window for every unhandled exception that occurs.
/// </summary>
public class WindowExceptionHandler : GlobalExceptionHandlerBase
{
   /// <summary>
   /// This method opens a new ExceptionWindow with the
   /// passed exception object as datacontext.
   /// </summary>
   public override void OnUnhandledException(Exception e)
   {
      System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => {
         var exceptionWindow = new ExceptionWindow();
         exceptionWindow.DataContext = new ExceptionWindowVM(e);
         exceptionWindow.Show();
      }));
   }
}
