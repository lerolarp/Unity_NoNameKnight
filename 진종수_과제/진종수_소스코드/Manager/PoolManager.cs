using UnityEngine;
using System.Collections.Generic;
using Data;
using static PoolEntity;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance = null;

    private Dictionary<PoolType, List<PoolEntity>> _deActivelist = new Dictionary<PoolType, List<PoolEntity>>();
    private Dictionary<PoolType, List<PoolEntity>> _activelist = new Dictionary<PoolType, List<PoolEntity>>();

    [SerializeField] public Transform _deActiveTr;
    [SerializeField] public Transform _activeTr;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        
    }


    public void Dispose()
    {
        foreach (var poolList in _activelist)
        {
            for (int i = 0; i < poolList.Value.Count; ++i)
            {
                poolList.Value[i].Dispose();
                Push(poolList.Value[i]);
            }
        }
    }

    private void OnDestroy()
    {
        foreach(var poolList in _deActivelist)
        {
            for(int i = 0; i < poolList.Value.Count; ++i)
            {
                poolList.Value[i].Dispose();
                Destroy(poolList.Value[i]);
            }

            poolList.Value.Clear();
        }
        _deActivelist = null;


        foreach (var poolList in _activelist)
        {
            for (int i = 0; i < poolList.Value.Count; ++i)
            {
                poolList.Value[i].Dispose();
                Destroy(poolList.Value[i]);
            }

            poolList.Value.Clear();
        }
        _activelist = null;
    }

    public static PoolManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public PoolEntity Create(PoolType type, Vector3 createPos, string tag = null, Transform parent = null)
    {
        PoolEntity entity = Pool(type);

        if (_activelist.ContainsKey(type) == false)
            _activelist.Add(type, new List<PoolEntity>());

        _activelist[type].Add(entity);

        if (parent != null)
            entity.transform.SetParent(parent);
        else
            entity.transform.SetParent(_activeTr);

        entity.Initialize(type, createPos);

        if (tag != null)
            entity.gameObject.layer = TagLayerTableInfo.GetTagLayerData(tag);

        entity.gameObject.SetActive(true);

        return entity;
    }

    private PoolEntity Pool(PoolType type)
    {
        if (_deActivelist.ContainsKey(type) == false)
            _deActivelist.Add(type, new List<PoolEntity>());

        PoolEntity entity;

        _deActivelist.TryGetValue(type, out var list);
        if (list.Count == 0)
        {
            GameObject instObj = null;

            switch (type)
            {
                case PoolType.AttackBox:
                    {
                        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath("AttackBox"));
                        instObj = Instantiate(obj);
                    }
                    break;

                case PoolType.Projectile:
                    {
                        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath("Projectile"));
                        instObj = Instantiate(obj);
                    }
                    break;

                case PoolType.Channeling:
                    {
                        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath("Channeling"));
                        instObj = Instantiate(obj);
                    }
                    break;

                case PoolType.Summon:
                    {
                        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath("Summon"));
                        instObj = Instantiate(obj);
                    }
                    break;

                case PoolType.UIFloatingDamage:
                    {
                        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath("UIFloatingDamage"));
                        instObj = Instantiate(obj);
                    }
                    break;

                default:
                    {
                        Debug.LogError($"Error : Not Found Type : {type}");
                    }
                    break;
            }
            list.Add(instObj.GetComponent<PoolEntity>());
        }

        entity = list[0];
        list.RemoveAt(0);

        return entity;
    }

    public void Push(PoolEntity entity)
    {
        _deActivelist[entity.Type].Add(entity);

        entity.transform.parent = this._deActiveTr;
        entity.gameObject.SetActive(false);
    }
}
