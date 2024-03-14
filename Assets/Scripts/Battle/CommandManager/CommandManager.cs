using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    public Character SelectCharacter { get; protected set; }

    // キャラクターのコマンド
    [SerializeField] protected GameObject[] SelectCommandAttributeObjArray = new GameObject[3];  // 選択した属性を表示するObject
    protected Image[] SelectCommandAttributeImageArray = new Image[3];                           // 属性表示ObjectのImage
    [SerializeField] protected GameObject[] SelectCommandMindObjArray = new GameObject[3];       // 選択した陰陽を表示するObject
    protected Image[] SelectCommandMindImageArray = new Image[3];                                // 陰陽表示ObjectのImage

    // 表示
    [SerializeField] protected GameObject SelectCommandObj;                              // 表示領域のObject
    [SerializeField] private Sprite[] _yinYangSprites = new Sprite[2];  // 陰陽のSprite

    // 選択コマンド
    public List<int> CommandIdList { get; private set; } = new List<int>();  // 選択された属性のIDリスト
    public List<bool> IsYinList { get; private set; } = new List<bool>();    // 選択された気が陰かどうかのリスト
    

    protected virtual void Awake()
    {
        // 選択したコマンドの表示領域を取得
        int commandIndex = 0;
        for (int i = 0; i < SelectCommandAttributeObjArray.Length; i++)
        {
            // Imageコンポーネント取得
            SelectCommandAttributeImageArray[i] = SelectCommandAttributeObjArray[i].GetComponent<Image>();

            // 陰陽表示ObjectのImageコンポーネント取得
            SelectCommandMindImageArray[i] = SelectCommandMindObjArray[i].GetComponent<Image>();

            commandIndex++;
        }
    }

    protected virtual void Update() { }

    /// <summary>
    /// 選択された属性の画像を表示領域にセットする
    /// </summary>
    /// <param name="selectingCommandSequence">表示する位置</param>
    public virtual void SelectCommand(int selectingCommandSequence)
    {
        // 選択した属性の画像をセット
        int spriteIndex = CommandIdList[selectingCommandSequence];
        SelectCommandAttributeImageArray[selectingCommandSequence].sprite = SelectCharacter.SelectCommandSprites[spriteIndex];
    }

    /// <summary>
    /// 選択された気の画像を表示領域にセットする
    /// </summary>
    /// <param name="selectingCommandSequence">表示する位置</param>
    public virtual void SelectMind(int selectingCommandSequence)
    {
        // 陰の画像をセット
        if (IsYinList[selectingCommandSequence])
        {
            SelectCommandMindImageArray[selectingCommandSequence].sprite = _yinYangSprites[0];
            return;
        }

        // 陽の画像をセット
        SelectCommandMindImageArray[selectingCommandSequence].sprite = _yinYangSprites[1];
    }
    
}
