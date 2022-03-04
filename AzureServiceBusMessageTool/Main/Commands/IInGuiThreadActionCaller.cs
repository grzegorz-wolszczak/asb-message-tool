using System;

namespace Main.Commands;

public interface IInGuiThreadActionCaller
{
   public void Call(Action action);
}
