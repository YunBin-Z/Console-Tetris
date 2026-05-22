namespace ConsoleTetris;

public enum E_SceneType
{
    Begin,
    Gaming,
    End
}
public class Game
{
    public const int w = 50;
    public const int h = 35;
    
    //当前选中的场景
     static public ISceneUpdate nowScene;
    //当前场景怎么和游戏接口绑定，让游戏场景类实现游戏帧更新接口，
    //然后什么时候进行游戏场景的切换能让帧更新的调用出现不同呢

    public  Game()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(w, h);
        ChangeScene(E_SceneType.Begin);
    }

    //游戏场景怎么切换啊！！！！
    //没完没了的套娃烦死了啊！！我知道了，把一件大事做成功的秘诀就是切分成无数件小事
    public static void ChangeScene(E_SceneType type)
    {
        //少了一步清空控制台
        Console.Clear();
        switch (type)
        {
            case E_SceneType.End:
                //调用结束场景类的print画法
                nowScene = new EndScene();
                break;
            case E_SceneType.Begin:
                //调用开始场景类的print画法
                nowScene = new BeginScene();
                break;
            case E_SceneType.Gaming:
                //调用游戏场景类的print画法
                nowScene = new GameScene();
                break;
        }
    }

    public void GameLoop()
    {
        //游戏主循环 主要负责 游戏场景逻辑的更新
        while (true)
        {
            if (nowScene != null)
            {
                nowScene.Update();
            }
            //调用游戏帧更新接口
        }
    }

}