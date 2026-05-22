using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris;

abstract class BeginOrEndBaseScene:ISceneUpdate
{
    protected int nowSelIndexl=2;
    protected string strTitle;
    protected string strOne;
    public abstract void EnterDoSomething();
    public void Update()
    {
        //开始和结束场景的逻辑
        //选择当前的选项 然后监听键盘输入wsj
        //有可能别的地方会更改颜色，所以这里为了确保颜色，重新设置一下白色
        Console.ForegroundColor = ConsoleColor.White;
        //显示标题
        //减去strTitle.length是因为中文字正好一个字应两个空，空格数除2，正好就是strTitle.length
        Console.SetCursorPosition(Game.w/2-strTitle.Length,5);
        Console.Write(strTitle);
        //显示下方的选项
        Console.SetCursorPosition(Game.w/2-strOne.Length,8);
        Console.ForegroundColor = nowSelIndexl == 0 ? ConsoleColor.Red : ConsoleColor.White;
        Console.Write(strOne);
        Console.SetCursorPosition(Game.w/2-4,10);
        Console.ForegroundColor = nowSelIndexl == 1 ?  ConsoleColor.Red :ConsoleColor.White;
        Console.Write("结束游戏");
        //检测输入
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.W:
                nowSelIndexl = 0;
                break;
            case ConsoleKey.S:
                nowSelIndexl = 1;
                break;
            case ConsoleKey.Enter:
                EnterDoSomething();
                break;
        }
    }
}