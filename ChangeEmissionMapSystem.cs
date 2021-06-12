using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
    
[ExecuteInEditMode] //SendMessageでエラーが出ないように

public class ChangeEmissionMapSystem : MonoBehaviour
{
    private SpriteRenderer meshRenderer;
    //Emissionさせたい色をインスペクターで指定する
    [ColorUsage(false, true)] public Color change2EmissionColor;
    //Emissionしてない時(初期状態の色)用変数
    [ColorUsage(false, true)] public Color initializeEmissionColor;
    [SerializeField]private Sprite emissionMap;
    private Sprite lastEmissionMap;
    private static readonly int EmissionMapID = Shader.PropertyToID("_EmissionMap");
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    
    
    /// <summary>
    /// EmissionMapのproperty
    /// </summary>
    public Sprite EmissionMap
    {
        get { return emissionMap;}
        set
        {        
            emissionMap = value;
            ChangeEmissionMap();
        }
    }

    /// <summary>
    /// 初期化処理ここでキーワード宣言する必要がある。
    /// </summary>
    public void Start()
    {
        meshRenderer = GetComponent<SpriteRenderer>();
        meshRenderer.material.EnableKeyword("_EMISSION");
        initializeEmissionColor = meshRenderer.material.GetColor(EmissionColorID);
    }

    /// <summary>
    /// アニメーション中に値の変更があるとこの関数が呼び出される
    /// </summary>
    private void OnDidApplyAnimationProperties()
    {
        //EmissionMapがnullじゃないもしくはEmissionMapに更新があればEmissionMapを変更する
        if (emissionMap !=lastEmissionMap&&emissionMap!=null)
        {
            ChangeEmissionMap();
        }

        lastEmissionMap = emissionMap;
    }

    /// <summary>
    /// EmissionMapを変更します。
    /// </summary>
    public void ChangeEmissionMap()
    {
        meshRenderer.material.SetColor(EmissionColorID,change2EmissionColor);
        meshRenderer.material.SetTexture(EmissionMapID,emissionMap.texture);
    }
    
    /// <summary>
    /// EmmisionMapをnullに変更後EmissionColorも通常時と同じ色に変更。
    /// </summary>
    public void EmissionMap2Null()
    {
        meshRenderer.material.SetTexture(EmissionMapID,null);
        meshRenderer.material.SetColor(EmissionColorID,initializeEmissionColor);

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChangeEmissionMapSystem))] //拡張するクラスを指定
public class ChangeEmissionMapEditor : Editor
{

    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        //targetを変換して対象を取得
        ChangeEmissionMapSystem changeEmission = target as ChangeEmissionMapSystem;

        //PrivateMethodを実行する用のボタン
        if (GUILayout.Button("ChangeEmissionMap"))
        {
            //SendMessageを使って実行
            changeEmission.SendMessage("ChangeEmissionMap", null, SendMessageOptions.DontRequireReceiver);
        }

    }
}
#endif
