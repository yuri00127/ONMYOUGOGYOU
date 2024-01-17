using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    // スクリプトを取得するオブジェクト
    private const string _battleManagerObjName = "BattleManager";

    // スクリプト
    public BattleManager _battleManager { get; protected set; }
    public Character SelectCharacter { get; protected set; }

    // キャラクターのコマンド
    protected GameObject[] SelectCommandObjArray = new GameObject[3];   // 選択したコマンドの表示Object
    protected Image[] SelectCommandImageArray = new Image[3];           // 表示ObjectのImageコンポーネント
    protected const string MindObjName = "Mind";                        // 選択した陰陽を表示するObjectの名前
    protected Image[] MindImageArray = new Image[3];                    // 陰陽表示ObjectのImageコンポーネント

    // 表示
    protected GameObject SelectCommandObj;                              // 表示領域のObject
    [SerializeField] private Sprite[] _yinYangSprites = new Sprite[2];  // 陰陽のSprite

    // 選択コマンド
    public List<int> CommandIdList { get; private set; } = new List<int>();  // 選択された属性のIDリスト
    public List<bool> IsYinList { get; private set; } = new List<bool>();    // 選択された気が陰かどうかのリスト
    

    protected virtual void Awake()
    {
        // スクリプト取得
        _battleManager = GameObject.Find(_battleManagerObjName).GetComponent<BattleManager>();

        // 選択したコマンドの表示領域を取得
        int commandIndex = 0;
        for (int i = 0; i < SelectCommandObjArray.Length; i++)
        {
            // Object取得
            commandIndex++;
            SelectCommandObjArray[i] = SelectCommandObj.transform.Find(commandIndex.ToString()).gameObject;

            // Imageコンポーネント取得
            SelectCommandImageArray[i] = SelectCommandObjArray[i].GetComponent<Image>();

            // 陰陽表示ObjectのImageコンポーネント取得
            MindImageArray[i] = SelectCommandObjArray[i].transform.Find(MindObjName).GetComponent<Image>();
        }
    }

    protected virtual void Update()
    {

    }

    /// <summary>
    /// 選択されたコマンドの画像を表示領域にセットする
    /// </summary>
    /// <param name="selectingCommandSequence">表示する位置</param>
    public virtual void SelectCommand(int selectingCommandSequence)
    {
        // 選択したコマンドの画像をセット
        int spriteIndex = CommandIdList[selectingCommandSequence];
        SelectCommandImageArray[selectingCommandSequence].sprite = SelectCharacter.SelectCommandSprites[spriteIndex];
    }

    /// <summary>
    /// 選択された気の画像を表示領域にセットする
    /// </summary>
    /// <param name="selectingCommandSequence">表示する位置</param>
    protected virtual void SelectMind(int selectingCommandSequence)
    {
        // 陽が選択されていたら
        if (IsYinList[selectingCommandSequence])
        {
            // 陰の画像をセット
            MindImageArray[selectingCommandSequence].sprite = _yinYangSprites[0];
            return;
        }

        // 陽の画像をセット
        MindImageArray[selectingCommandSequence].sprite = _yinYangSprites[1];
    }
    
}
