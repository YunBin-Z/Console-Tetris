using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace C_进阶俄罗斯方块;

public class GameScene:ISceneUpdate
{
    /// <summary>
    /// 创建类成员的目的是保证两个线程都能够正常访问到成员
    /// </summary>

    // 输入队列（线程安全）
    ConcurrentQueue<ConsoleKey> inputQueue = new ConcurrentQueue<ConsoleKey>();

    // 游戏对象
    private Tetromino current;
    private Map map;
    Random rand ;

    // 游戏状态
    private bool running = true;

    // 下落控制
    private int dropInterval = 500; // 毫秒
    private DateTime lastDropTime = DateTime.Now;


    //游戏场景的构造函数，主要负责游戏场景的初始化,游戏主循环是重复调用当前场景的update函数的，这里写只会调用一次

    //任务;调用工厂类生成墙壁和方块的坐标数据，并调用它们的draw方法来渲染到控制台上
    public GameScene()
    {
        Console.CursorVisible = false;
        rand = new Random();

        Thread inputThread = new Thread(InputLoop);
        inputThread.Start();
        inputThread.IsBackground = true;

        map = new Map(10,20);
        
        RCT();//快速创建随机类型的方块对象，方块对象赋值给current变量



    }
    //构造函数只跑一次，因此这里可以while (true)
    public void InputLoop()
    {
        while (true)
        {
            //Console.ReadKey(true).Key等于：检测输入键盘按下的按键对于的枚举值，参数true，代表输入时不会显示输入
            ConsoleKey key = Console.ReadKey(true).Key;
            inputQueue.Enqueue(key);
        }
    }

    //游戏场景的更新函数，主要负责游戏场景的逻辑更新和渲染
    public void Update()
    {
         if(running)
        {
            if (!running) return;

            // 1 先擦掉旧方块
            current.Clear();

            // 2 处理输入
            HandleInput();

            // 3 自动下落
            AutoDrop();

            // 4 画地图（静态块）
            Renderer.DrawMap(map);

            // 5 画当前方块  （先画地图再画方块能保证地图空白格子不会覆盖方块）
            current.Draw();

            // 6 分数
            Renderer.printScore(map);

            Thread.Sleep(16);
        }
    }
    //为了不让两个线程同时修改方块位置产生数据竞争，输入线程只负责监听用户输入。
    //采用输入队列来
   
    // 游戏主循环

    public void HandleInput()
    {
        //我该怎么用这个输入队列来处理输入呢？
        //我该如何正确取用队列的值呢，在哪个代码块里写呢？switch需要固定值，因此需要一个枚举的变量
        //队列需要取用出队，出队就是一次handleInput吗？还是说应该一次把一个队列的值全部都拿出来呢，执行多少次呢？
        //比如在俄罗斯方块下落的过程中，我按了5次左，那么在实际中，我应该立刻跑5次左，然后才下落一次吗，
        //可是实际在游玩俄罗斯方块的时候，我是每次往左都渲染了一遍的啊，往左跑了两次，就渲染了两次往左后的图案，
        //渲染的场景应该是啥？目标是啥？算了，不想了，先只做一次操作的处理把
        while (inputQueue.TryDequeue(out var key))
        switch(key)
        {
                case ConsoleKey.A:
                    current.MoveLeft(map);
                    break;

                case ConsoleKey.D:
                    current.MoveRight(map);
                    break;

                case ConsoleKey.W:
                    current.Rotate(map);
                    break;

                case ConsoleKey.S:
                    current.MoveDown(map);
                    break;
            }
    }

    /// <summary>
    /// 随机创建方块List<GameObject>的函数，每次调用本函数，会给current
    /// </summary>
    public void RCT()
    {
        BrickType brickType;
        // Unity 注意：如果是Unity工程，建议用 Random.Range(0, 7) 替代 rand.Next(7)
        int randInt = rand.Next(7);

        switch (randInt)
        {
            case 0:
                brickType = BrickType.I;
                break;
            case 1:
                brickType = BrickType.O;
                break;
            case 2:
                brickType = BrickType.T;
                break;
            case 3:
                brickType = BrickType.L;
                break;
            case 4:
                brickType = BrickType.J;
                break;
            case 5:
                brickType = BrickType.S;
                break;
            case 6:
                brickType = BrickType.Z;
                break;
            // 修复2：增加 default 分支（兜底，即使逻辑上不会走到）
            default:
                brickType = BrickType.I;
                break;
        }

        current = new Tetromino(GameObjectFactory.CreateTetromino(brickType, new Position(6, 1)));
    }
     // 自动下落
    private void AutoDrop()
    {
        if ((DateTime.Now - lastDropTime).TotalMilliseconds >= dropInterval)
        {
            bool moved = current.MoveDown(map);

            if (!moved)
            {
                //如果不能继续下落，先通知地图类，把当前方块的list<GameObject>给地图，把方块变墙壁
                map.FixBlocks(current.GetBlocks());

                //尝试消除满行，能消除就会消除
                map.ClearLines();

                //重新创建新的方块给current
                RCT();

                if (!current.CanSpawn(map))
                {
                    running = false;
                    Console.Clear();
                    Game.ChangeScene(E_SceneType.End);
                }
            }

            lastDropTime = DateTime.Now;
        }
    }
}

